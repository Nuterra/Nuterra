using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.PE;
using dnlib.Threading;

namespace Nuterra.Installer.ModuleImport
{
	[Serializable]
	internal sealed class ModuleImporterAbortedException : Exception
	{
	}

	sealed partial class ModuleImporter
	{
		[Flags]
		public enum Options
		{
			None = 0,

			/// <summary>
			/// Module and assembly attributes replace target module and assembly attributes
			/// </summary>
			ReplaceModuleAssemblyAttributes = 0x00000001,

			/// <summary>
			/// Assembly declared security attributes replace target assembly's declared security attributes
			/// </summary>
			ReplaceAssemblyDeclSecurities = 0x00000002,
		}

		private const string IM0001 = nameof(IM0001);
		private const string IM0002 = nameof(IM0002);
		private const string IM0003 = nameof(IM0003);
		private const string IM0004 = nameof(IM0004);
		private const string IM0005 = nameof(IM0005);
		private const string IM0006 = nameof(IM0006);
		private const string IM0007 = nameof(IM0007);
		private const string IM0008 = nameof(IM0008);
		private const string IM0009 = nameof(IM0009);
		private const string IM0010 = nameof(IM0010);

		public CompilerDiagnostic[] Diagnostics => diagnostics.ToArray();
		public NewImportedType[] NewNonNestedTypes => newNonNestedImportedTypes.ToArray();
		public CustomAttribute[] NewAssemblyCustomAttributes { get; private set; }
		public DeclSecurity[] NewAssemblyDeclSecurities { get; private set; }
		public CustomAttribute[] NewModuleCustomAttributes { get; private set; }
		public Resource[] NewResources { get; private set; }

		private readonly ModuleDef targetModule;
		private readonly bool makeEverythingPublic;
		private readonly List<CompilerDiagnostic> diagnostics;
		private readonly List<NewImportedType> newNonNestedImportedTypes;
		private readonly HashSet<TypeDef> newStateMachineTypes;

		private ModuleDef sourceModule;
		private readonly Dictionary<TypeDef, ImportedType> oldTypeToNewType;
		private readonly Dictionary<ITypeDefOrRef, ImportedType> oldTypeRefToNewType;
		private readonly Dictionary<MethodDef, MemberInfo<MethodDef>> oldMethodToNewMethod;
		private readonly Dictionary<FieldDef, MemberInfo<FieldDef>> oldFieldToNewField;
		private readonly Dictionary<PropertyDef, MemberInfo<PropertyDef>> oldPropertyToNewProperty;
		private readonly Dictionary<EventDef, MemberInfo<EventDef>> oldEventToNewEvent;
		private readonly Dictionary<object, object> bodyDict;
		private readonly Dictionary<ImportedType, ExtraImportedTypeData> toExtraData;
		private readonly HashSet<MethodDef> usedMethods;
		private readonly HashSet<object> isStub;
		private ImportSigComparerOptions importSigComparerOptions;

		private const SigComparerOptions SIG_COMPARER_OPTIONS = SigComparerOptions.TypeRefCanReferenceGlobalType | SigComparerOptions.PrivateScopeIsComparable;

		public ModuleImporter(ModuleDef targetModule, bool makeEverythingPublic)
		{
			this.targetModule = targetModule;
			this.makeEverythingPublic = makeEverythingPublic;
			diagnostics = new List<CompilerDiagnostic>();
			newNonNestedImportedTypes = new List<NewImportedType>();
			newStateMachineTypes = new HashSet<TypeDef>();
			oldTypeToNewType = new Dictionary<TypeDef, ImportedType>();
			oldTypeRefToNewType = new Dictionary<ITypeDefOrRef, ImportedType>(TypeEqualityComparer.Instance);
			oldMethodToNewMethod = new Dictionary<MethodDef, MemberInfo<MethodDef>>();
			oldFieldToNewField = new Dictionary<FieldDef, MemberInfo<FieldDef>>();
			oldPropertyToNewProperty = new Dictionary<PropertyDef, MemberInfo<PropertyDef>>();
			oldEventToNewEvent = new Dictionary<EventDef, MemberInfo<EventDef>>();
			bodyDict = new Dictionary<object, object>();
			toExtraData = new Dictionary<ImportedType, ExtraImportedTypeData>();
			usedMethods = new HashSet<MethodDef>();
			isStub = new HashSet<object>();
		}

		private void AddError(string id, string msg) => diagnostics.Add(new CompilerDiagnostic(CompilerDiagnosticSeverity.Error, msg, id, null, null));

		private void AddErrorThrow(string id, string msg)
		{
			AddError(id, msg);
			throw new ModuleImporterAbortedException();
		}

		private ModuleDefMD LoadModule(byte[] rawGeneratedModule, DebugFileResult debugFile)
		{
			var opts = new ModuleCreationOptions();

			switch (debugFile.Format)
			{
				case DebugFileFormat.None:
					break;

				case DebugFileFormat.Pdb:
					opts.PdbFileOrData = debugFile.RawFile;
					break;

				case DebugFileFormat.PortablePdb:
					Debug.Fail("Portable PDB isn't supported yet");//TODO:
					break;

				case DebugFileFormat.Embedded:
					Debug.Fail("Embedded Portable PDB isn't supported yet");//TODO:
					break;

				default:
					Debug.Fail($"Unknown debug file format: {debugFile.Format}");
					break;
			}

			return ModuleDefMD.Load(rawGeneratedModule, opts);
		}

		private static void RemoveDuplicates(List<CustomAttribute> attributes, string fullName)
		{
			bool found = false;
			for (int i = 0; i < attributes.Count; i++)
			{
				var ca = attributes[i];
				if (ca.TypeFullName != fullName)
					continue;
				if (!found)
				{
					found = true;
					continue;
				}
				attributes.RemoveAt(i);
				i--;
			}
		}

		private static void RemoveDuplicateSecurityPermissionAttributes(List<DeclSecurity> secAttrs)
		{
			foreach (var declSec in secAttrs)
			{
				if (declSec.Action != SecurityAction.RequestMinimum)
					continue;
				bool found = false;
				var list = declSec.SecurityAttributes;
				for (int i = 0; i < list.Count; i++)
				{
					var ca = list[i];
					if (ca.TypeFullName != "System.Security.Permissions.SecurityPermissionAttribute")
						continue;
					if (!found)
					{
						found = true;
						continue;
					}
					list.RemoveAt(i);
					i--;
				}
			}
		}

		private void InitializeTypesAndMethods()
		{
			// Step 1: Initialize all definitions
			InitializeTypesStep1(oldTypeToNewType.Values.OfType<NewImportedType>());

			// Step 2: import the rest, which depend on defs having been initialized,
			// eg. ca.Constructor could be a MethodDef
			InitializeTypesStep2(oldTypeToNewType.Values.OfType<NewImportedType>());

			InitializeTypesMethods(oldTypeToNewType.Values.OfType<NewImportedType>());
		}

		private void ImportResources()
		{
			var newResources = new List<Resource>(sourceModule.Resources.Count);
			foreach (var resource in sourceModule.Resources)
			{
				var newResource = Import(resource);
				if (newResource == null)
					continue;
				newResources.Add(newResource);
			}

			//TODO: Need to rename some resources if the owner type has been renamed, this also
			//		requires fixing strings in method bodies.

			NewResources = newResources.ToArray();
		}

		private Resource Import(Resource resource)
		{
			var er = resource as EmbeddedResource;
			if (er != null)
				return Import(er);

			var alr = resource as AssemblyLinkedResource;
			if (alr != null)
				return Import(alr);

			var lr = resource as LinkedResource;
			if (lr != null)
				return Import(lr);

			Debug.Fail($"Unknown resource type: {resource?.GetType()}");
			return null;
		}

		private EmbeddedResource Import(EmbeddedResource resource) =>
			new EmbeddedResource(resource.Name, resource.GetResourceData(), resource.Attributes);

		private AssemblyLinkedResource Import(AssemblyLinkedResource resource) =>
			new AssemblyLinkedResource(resource.Name, resource.Assembly?.ToAssemblyRef(), resource.Attributes);

		private LinkedResource Import(LinkedResource resource) =>
			new LinkedResource(resource.Name, Import(resource.File), resource.Attributes) { Hash = resource.Hash };

		private FileDef Import(FileDef file)
		{
			var createdFile = targetModule.UpdateRowId(new FileDefUser(file.Name, file.Flags, file.HashValue));
			ImportCustomAttributes(createdFile, file);
			return createdFile;
		}

		/// <summary>
		/// Imports everything into the target module. All global members are merged and possibly renamed.
		/// All non-nested types are renamed if a type with the same name exists in the target module.
		/// </summary>
		/// <param name="rawGeneratedModule">Raw bytes of compiled assembly</param>
		/// <param name="debugFile">Debug file</param>
		/// <param name="options">Options</param>
		public void Import(byte[] rawGeneratedModule, DebugFileResult debugFile, Options options)
		{
			SetSourceModule(LoadModule(rawGeneratedModule, debugFile));

			//AddGlobalTypeMembers(sourceModule.GlobalType);

			foreach (var type in sourceModule.Types)
			{
				if (type.IsGlobalModuleType)
					continue;
				newNonNestedImportedTypes.Add(CreateNewImportedType(type, targetModule.Types));
			}
			InitializeTypesAndMethods();

			if ((options & Options.ReplaceModuleAssemblyAttributes) != 0)
			{
				var attributes = new List<CustomAttribute>();
				ImportCustomAttributes(attributes, sourceModule);
				// The compiler adds [UnverifiableCode] causing a duplicate attribute
				RemoveDuplicates(attributes, "System.Security.UnverifiableCodeAttribute");
				NewModuleCustomAttributes = attributes.ToArray();
				var asm = sourceModule.Assembly;
				if (asm != null)
				{
					attributes.Clear();
					ImportCustomAttributes(attributes, asm);
					NewAssemblyCustomAttributes = attributes.ToArray();
				}
			}

			if ((options & Options.ReplaceAssemblyDeclSecurities) != 0)
			{
				var asm = sourceModule.Assembly;
				if (asm != null)
				{
					var declSecs = new List<DeclSecurity>();
					ImportDeclSecurities(declSecs, asm);
					// The C# compiler always adds this security attribute:
					//	SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)
					RemoveDuplicateSecurityPermissionAttributes(declSecs);
					NewAssemblyDeclSecurities = declSecs.ToArray();
				}
			}

			ImportResources();
			SetSourceModule(null);
		}

		private TypeDef FindSourceType(TypeDef targetType)
		{
			var newType = sourceModule.Find(targetType.Module.Import(targetType));
			if (newType != null)
				return newType;

			AddErrorThrow(IM0010, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindEditedType, targetType));
			throw new InvalidOperationException();
		}

		private struct ExistingMember<T> where T : IMemberDef
		{
			/// <summary>Compiled member</summary>
			public T CompiledMember { get; }

			/// <summary>Original member that exists in the target module</summary>
			public T TargetMember { get; }

			public ExistingMember(T compiledMember, T targetMember)
			{
				CompiledMember = compiledMember;
				TargetMember = targetMember;
			}
		}

		private void InitializeNewStateMachineTypes(TypeDef compiledType)
		{
			foreach (var method in compiledType.Methods)
			{
				var smType = StateMachineHelpers.GetStateMachineType(method);
				if (smType != null)
				{
					Debug.Assert(!newStateMachineTypes.Contains(smType), "Two or more methods share the same state machine type");
					newStateMachineTypes.Add(smType);
				}
			}
		}

		private MethodDef FindSourceMethod(MethodDef targetMethod)
		{
			var newType = sourceModule.Find(targetMethod.Module.Import(targetMethod.DeclaringType));
			if (newType == null)
				AddErrorThrow(IM0001, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindMethodType, targetMethod.DeclaringType));

			// Don't check type scopes or we won't be able to find methods with edited nested types.
			const SigComparerOptions comparerFlags = SIG_COMPARER_OPTIONS | SigComparerOptions.DontCompareTypeScope;

			var newMethod = newType.FindMethod(targetMethod.Name, targetMethod.MethodSig, comparerFlags, targetMethod.Module);
			if (newMethod != null)
				return newMethod;

			if (targetMethod.Overrides.Count != 0)
			{
				var targetOverriddenMethod = targetMethod.Overrides[0].MethodDeclaration;
				var comparer = new SigComparer(comparerFlags, targetModule);
				foreach (var method in newType.Methods)
				{
					foreach (var o in method.Overrides)
					{
						if (!comparer.Equals(o.MethodDeclaration, targetOverriddenMethod))
							continue;
						if (!comparer.Equals(o.MethodDeclaration.DeclaringType, targetOverriddenMethod.DeclaringType))
							continue;
						return method;
					}
				}
			}

			AddErrorThrow(IM0002, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindEditedMethod, targetMethod));
			throw new InvalidOperationException();
		}

		private void SetSourceModule(ModuleDef newSourceModule)
		{
			sourceModule = newSourceModule;
			importSigComparerOptions = newSourceModule == null ? null : new ImportSigComparerOptions(newSourceModule, targetModule);
		}

		private FieldDefOptions CreateFieldDefOptions(FieldDef newField, FieldDef targetField)
		{
			var options = new FieldDefOptions(newField);
			// All fields are made public, so do not copy the access bits
			if (makeEverythingPublic && (options.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
				options.Attributes = (options.Attributes & ~FieldAttributes.FieldAccessMask) | (targetField.Attributes & FieldAttributes.FieldAccessMask);
			return options;
		}

		private PropertyDefOptions CreatePropertyDefOptions(PropertyDef newProperty) =>
			new PropertyDefOptions(newProperty);

		private EventDefOptions CreateEventDefOptions(EventDef newEvent) =>
			new EventDefOptions(newEvent);

		private MethodDefOptions CreateMethodDefOptions(MethodDef newMethod, MethodDef targetMethod)
		{
			var options = new MethodDefOptions(newMethod);
			options.ParamDefs.Clear();
			options.ParamDefs.AddRange(newMethod.ParamDefs.Select(a => Clone(a)));
			options.GenericParameters.Clear();
			options.GenericParameters.AddRange(newMethod.GenericParameters.Select(a => Clone(a)));
			// All methods are made public, so do not copy the access bits
			if (makeEverythingPublic && (options.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
				options.Attributes = (options.Attributes & ~MethodAttributes.MemberAccessMask) | (targetMethod.Attributes & MethodAttributes.MemberAccessMask);
			return options;
		}

		private ParamDef Clone(ParamDef paramDef)
		{
			if (paramDef == null)
				return null;
			var importedParamDef = new ParamDefUser(paramDef.Name, paramDef.Sequence, paramDef.Attributes);
			importedParamDef.Rid = paramDef.Rid;
			importedParamDef.CustomAttributes.AddRange(paramDef.CustomAttributes);
			importedParamDef.MarshalType = paramDef.MarshalType;
			importedParamDef.Constant = paramDef.Constant;
			return importedParamDef;
		}

		private GenericParam Clone(GenericParam gp)
		{
			if (gp == null)
				return null;
			var importedGenericParam = new GenericParamUser(gp.Number, gp.Flags, gp.Name);
			importedGenericParam.Rid = gp.Rid;
			importedGenericParam.CustomAttributes.AddRange(gp.CustomAttributes);
			importedGenericParam.Kind = gp.Kind;
			foreach (var gpc in gp.GenericParamConstraints)
				importedGenericParam.GenericParamConstraints.Add(Clone(gpc));
			return importedGenericParam;
		}

		private GenericParamConstraint Clone(GenericParamConstraint gpc)
		{
			if (gpc == null)
				return null;
			var importedGenericParamConstraint = new GenericParamConstraintUser(gpc.Constraint);
			importedGenericParamConstraint.Rid = gpc.Rid;
			importedGenericParamConstraint.CustomAttributes.AddRange(gpc.CustomAttributes);
			return importedGenericParamConstraint;
		}

		//void AddGlobalTypeMembers(TypeDef newGlobalType) => nonNestedMergedImportedTypes.Add(MergeTypesRename(newGlobalType, targetModule.GlobalType));

		private static bool IsVirtual(PropertyDef property) => property.GetMethod?.IsVirtual == true || property.SetMethod?.IsVirtual == true;

		private static bool IsVirtual(EventDef @event) => @event.AddMethod?.IsVirtual == true || @event.RemoveMethod?.IsVirtual == true || @event.InvokeMethod?.IsVirtual == true;

		private struct TypeName : IEquatable<TypeName>
		{
			private readonly UTF8String ns;
			private readonly UTF8String name;

			public TypeName(TypeDef type)
				: this(type.Namespace, type.Name)
			{
			}

			public TypeName(UTF8String ns, UTF8String name)
			{
				this.ns = ns ?? UTF8String.Empty;
				this.name = name ?? UTF8String.Empty;
			}

			public bool Equals(TypeName other) =>
				UTF8String.Equals(ns, other.ns) &&
				UTF8String.Equals(name, other.name);

			public override bool Equals(object obj) => obj is TypeName && Equals((TypeName)obj);

			public override int GetHashCode() =>
				UTF8String.GetHashCode(ns) ^
				UTF8String.GetHashCode(name);

			public override string ToString()
			{
				if (UTF8String.IsNullOrEmpty(ns))
					return name;
				return ns + "." + name;
			}
		}

		private sealed class TypeNames
		{
			private readonly HashSet<TypeName> names;

			public TypeNames()
			{
				names = new HashSet<TypeName>();
			}

			public TypeNames(IEnumerable<TypeDef> existingTypes)
			{
				names = new HashSet<TypeName>();
				foreach (var t in existingTypes)
					names.Add(new TypeName(t.Namespace, t.Name));
			}

			public bool Contains(UTF8String ns, UTF8String name) =>
				names.Contains(new TypeName(ns, name));

			public bool Contains(TypeDef type) =>
				names.Contains(new TypeName(type.Namespace, type.Name));

			public void Add(UTF8String ns, UTF8String name) =>
				names.Add(new TypeName(ns, name));

			public void Add(TypeDef type) =>
				names.Add(new TypeName(type.Namespace, type.Name));
		}

		private sealed class UsedTypeNames
		{
			private readonly TypeNames typeNames;

			public UsedTypeNames()
			{
				typeNames = new TypeNames();
			}

			public UsedTypeNames(IEnumerable<TypeDef> existingTypes)
			{
				typeNames = new TypeNames(existingTypes);
			}

			public UTF8String GetNewName(TypeDef type)
			{
				var ns = type.Namespace;
				var name = type.Name;
				for (int counter = 0; ; counter++)
				{
					if (!typeNames.Contains(ns, name))
						break;
					// It's prepended because generic types have a `<number> appended to the name,
					// which they still should have after the rename.
					name = "__" + counter.ToString() + "__" + type.Name.String;
				}
				typeNames.Add(ns, name);
				return name;
			}

			public void Add(TypeDef type) =>
				typeNames.Add(type);
		}

		private NewImportedType CreateNewImportedType(TypeDef newType, IList<TypeDef> existingTypes) =>
			CreateNewImportedType(newType, new UsedTypeNames(existingTypes));

		private NewImportedType CreateNewImportedType(TypeDef newType, UsedTypeNames usedTypeNames)
		{
			var name = usedTypeNames.GetNewName(newType);
			var importedType = AddNewImportedType(newType, name);
			AddNewNestedTypes(newType);
			return importedType;
		}

		private void AddNewNestedTypes(TypeDef newType)
		{
			if (newType.NestedTypes.Count == 0)
				return;
			var stack = new Stack<TypeDef>();
			foreach (var t in newType.NestedTypes)
				stack.Push(t);
			while (stack.Count > 0)
			{
				var newNestedType = stack.Pop();
				var importedNewNestedType = AddNewImportedType(newNestedType, newNestedType.Name);
				foreach (var t in newNestedType.NestedTypes)
					stack.Push(t);
			}
		}

		private NewImportedType AddNewImportedType(TypeDef type, UTF8String name)
		{
			var createdType = targetModule.UpdateRowId(new TypeDefUser(type.Namespace, name) { Attributes = type.Attributes });
			var importedType = new NewImportedType(createdType);
			toExtraData.Add(importedType, new ExtraImportedTypeData(type));
			oldTypeToNewType.Add(type, importedType);
			oldTypeRefToNewType.Add(type, importedType);
			return importedType;
		}

		private void InitializeTypesStep1(IEnumerable<NewImportedType> importedTypes)
		{
			foreach (var importedType in importedTypes)
			{
				var compiledType = toExtraData[importedType].CompiledType;
				foreach (var field in compiledType.Fields)
					Create(field);
				foreach (var method in compiledType.Methods)
					Create(method);
				foreach (var prop in compiledType.Properties)
					Create(prop);
				foreach (var evt in compiledType.Events)
					Create(evt);
			}
		}

		private void InitializeTypesStep2(IEnumerable<NewImportedType> importedTypes)
		{
			foreach (var importedType in importedTypes)
			{
				var compiledType = toExtraData[importedType].CompiledType;
				importedType.TargetType.BaseType = Import(compiledType.BaseType);
				ImportCustomAttributes(importedType.TargetType, compiledType);
				ImportDeclSecurities(importedType.TargetType, compiledType);
				importedType.TargetType.ClassLayout = Import(compiledType.ClassLayout);
				foreach (var genericParam in compiledType.GenericParameters)
					importedType.TargetType.GenericParameters.Add(Import(genericParam));
				foreach (var iface in compiledType.Interfaces)
					importedType.TargetType.Interfaces.Add(Import(iface));
				foreach (var nestedType in compiledType.NestedTypes)
					importedType.TargetType.NestedTypes.Add(oldTypeToNewType[nestedType].TargetType);

				foreach (var field in compiledType.Fields)
					importedType.TargetType.Fields.Add(Initialize(field));
				foreach (var method in compiledType.Methods)
					importedType.TargetType.Methods.Add(Initialize(method));
				foreach (var prop in compiledType.Properties)
					importedType.TargetType.Properties.Add(Initialize(prop));
				foreach (var evt in compiledType.Events)
					importedType.TargetType.Events.Add(Initialize(evt));
			}
		}

		// Adds all methods of existing properties and events of a merged type to make sure
		// the methods aren't accidentally used twice. Could happen if the compiler (eg. mcs)
		// doesn't set the PropertyDef.Type's HasThis flag even if it's an instance property.
		// Roslyn will set this flag, so our comparison would fail to match the two (now different)
		// properties. This comparison has since been fixed.
		private void AddUsedMethods(ImportedType importedType)
		{
			foreach (var p in importedType.TargetType.Properties)
			{
				foreach (var m in p.GetMethods)
					usedMethods.Add(m);
				foreach (var m in p.SetMethods)
					usedMethods.Add(m);
				foreach (var m in p.OtherMethods)
					usedMethods.Add(m);
			}
			foreach (var p in importedType.TargetType.Events)
			{
				if (p.AddMethod != null)
					usedMethods.Add(p.AddMethod);
				if (p.InvokeMethod != null)
					usedMethods.Add(p.InvokeMethod);
				if (p.RemoveMethod != null)
					usedMethods.Add(p.RemoveMethod);
				foreach (var m in p.OtherMethods)
					usedMethods.Add(m);
			}
		}

		private void Initialize(TypeDef compiledType, TypeDef targetType, TypeDefOptions options)
		{
			options.Attributes = compiledType.Attributes;

			// All types are made public, so do not copy the access bits
			var publicValue = targetType.DeclaringType == null ? TypeAttributes.Public : TypeAttributes.NestedPublic;
			if (makeEverythingPublic && (options.Attributes & TypeAttributes.VisibilityMask) == publicValue)
				options.Attributes = (options.Attributes & ~TypeAttributes.VisibilityMask) | (targetType.Attributes & TypeAttributes.VisibilityMask);

			options.Namespace = compiledType.Namespace;
			options.Name = compiledType.Name;
			options.PackingSize = compiledType.ClassLayout?.PackingSize;
			options.ClassSize = compiledType.ClassLayout?.ClassSize;
			options.BaseType = Import(compiledType.BaseType);
			options.CustomAttributes.Clear();
			ImportCustomAttributes(options.CustomAttributes, compiledType);
			options.DeclSecurities.Clear();
			ImportDeclSecurities(options.DeclSecurities, compiledType);
			options.GenericParameters.Clear();
			foreach (var genericParam in compiledType.GenericParameters)
				options.GenericParameters.Add(Import(genericParam));
			options.Interfaces.Clear();
			foreach (var ifaceImpl in compiledType.Interfaces)
				options.Interfaces.Add(Import(ifaceImpl));
		}

		private void InitializeTypesMethods(IEnumerable<NewImportedType> importedTypes)
		{
			foreach (var importedType in importedTypes)
			{
				foreach (var compiledMethod in toExtraData[importedType].CompiledType.Methods)
				{
					var targetMethod = oldMethodToNewMethod[compiledMethod].EditedMember;
					targetMethod.Body = CreateBody(targetMethod, compiledMethod);
				}
			}
		}

		private MethodOverride Import(MethodOverride o) => new MethodOverride(Import(o.MethodBody), Import(o.MethodDeclaration));

		private IMethodDefOrRef Import(IMethodDefOrRef method) => (IMethodDefOrRef)Import((IMethod)method);

		private ITypeDefOrRef Import(ITypeDefOrRef type)
		{
			if (type == null)
				return null;

			ImportedType importedType;
			var res = TryGetTypeInTargetModule(type, out importedType);
			if (res != null)
				return res;

			var tr = type as TypeRef;
			if (tr != null)
				return ImportTypeRefNoModuleChecks(tr, 0);

			var ts = type as TypeSpec;
			if (ts != null)
				return ImportTypeSpec(ts);

			// TypeDefs are already handled elsewhere
			throw new InvalidOperationException();
		}

		private TypeRef ImportTypeRefNoModuleChecks(TypeRef tr, int recurseCount)
		{
			const int MAX_RECURSE_COUNT = 500;
			if (recurseCount >= MAX_RECURSE_COUNT)
				return null;

			var scope = tr.ResolutionScope;
			IResolutionScope importedScope;

			var scopeTypeRef = scope as TypeRef;
			if (scopeTypeRef != null)
				importedScope = ImportTypeRefNoModuleChecks(scopeTypeRef, recurseCount + 1);
			else if (scope is AssemblyRef)
				importedScope = Import((AssemblyRef)scope);
			else if (scope is ModuleRef)
				importedScope = Import((ModuleRef)scope, true);
			else if (scope is ModuleDef)
			{
				if (scope == targetModule || scope == sourceModule)
					importedScope = targetModule;
				else
					throw new InvalidOperationException();
			}
			else
				throw new InvalidOperationException();

			var importedTypeRef = targetModule.UpdateRowId(new TypeRefUser(targetModule, tr.Namespace, tr.Name, importedScope));
			ImportCustomAttributes(importedTypeRef, tr);
			return importedTypeRef;
		}

		private AssemblyRef Import(AssemblyRef asmRef)
		{
			if (asmRef == null)
				return null;
			var importedAssemblyRef = targetModule.UpdateRowId(new AssemblyRefUser(asmRef.Name, asmRef.Version, asmRef.PublicKeyOrToken, asmRef.Culture));
			ImportCustomAttributes(importedAssemblyRef, asmRef);
			importedAssemblyRef.Attributes = asmRef.Attributes;
			importedAssemblyRef.Hash = asmRef.Hash;
			return importedAssemblyRef;
		}

		private TypeSpec ImportTypeSpec(TypeSpec ts)
		{
			if (ts == null)
				return null;
			var importedTypeSpec = targetModule.UpdateRowId(new TypeSpecUser(Import(ts.TypeSig)));
			ImportCustomAttributes(importedTypeSpec, ts);
			importedTypeSpec.ExtraData = ts.ExtraData;
			return importedTypeSpec;
		}

		private TypeDef TryGetTypeInTargetModule(ITypeDefOrRef tdr, out ImportedType importedType)
		{
			if (tdr == null)
			{
				importedType = null;
				return null;
			}

			var td = tdr as TypeDef;
			if (td != null)
				return (importedType = oldTypeToNewType[td]).TargetType;

			var tr = tdr as TypeRef;
			if (tr != null)
			{
				ImportedType importedTypeTmp;
				if (oldTypeRefToNewType.TryGetValue(tr, out importedTypeTmp))
					return (importedType = importedTypeTmp).TargetType;

				var tr2 = (TypeRef)tr.GetNonNestedTypeRefScope();
				if (IsTarget(tr2.ResolutionScope))
				{
					td = targetModule.Find(tr);
					if (td != null)
					{
						importedType = null;
						return td;
					}

					AddError(IM0003, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindType, tr));
					importedType = null;
					return null;
				}
				if (IsSource(tr2.ResolutionScope))
					throw new InvalidOperationException();
			}

			importedType = null;
			return null;
		}

		private bool IsSourceOrTarget(IResolutionScope scope) => IsSource(scope) || IsTarget(scope);

		private bool IsSource(IResolutionScope scope)
		{
			var asmRef = scope as AssemblyRef;
			if (asmRef != null)
				return IsSource(asmRef);

			var modRef = scope as ModuleRef;
			if (modRef != null)
				return IsSource(modRef);

			return scope == sourceModule;
		}

		private bool IsTarget(IResolutionScope scope)
		{
			var asmRef = scope as AssemblyRef;
			if (asmRef != null)
				return IsTarget(asmRef);

			var modRef = scope as ModuleRef;
			if (modRef != null)
				return IsTarget(modRef);

			return scope == targetModule;
		}

		private bool IsSourceOrTarget(AssemblyRef asmRef) => IsSource(asmRef) || IsTarget(asmRef);

		private bool IsSource(AssemblyRef asmRef) => AssemblyNameComparer.CompareAll.Equals(asmRef, sourceModule.Assembly);

		private bool IsTarget(AssemblyRef asmRef) => AssemblyNameComparer.CompareAll.Equals(asmRef, targetModule.Assembly);

		private bool IsSourceOrTarget(ModuleRef modRef) => IsSource(modRef) || IsTarget(modRef);

		private bool IsSource(ModuleRef modRef) => StringComparer.OrdinalIgnoreCase.Equals(modRef?.Name, sourceModule.Name);

		private bool IsTarget(ModuleRef modRef) => StringComparer.OrdinalIgnoreCase.Equals(modRef?.Name, targetModule.Name);

		private TypeDef ImportTypeDef(TypeDef type) => type == null ? null : oldTypeToNewType[type].TargetType;

		private MethodDef ImportMethodDef(MethodDef method) => method == null ? null : oldMethodToNewMethod[method].TargetMember;

		private TypeSig Import(TypeSig type)
		{
			if (type == null)
				return null;

			TypeSig result;
			switch (type.ElementType)
			{
				case ElementType.Void: result = targetModule.CorLibTypes.Void; break;
				case ElementType.Boolean: result = targetModule.CorLibTypes.Boolean; break;
				case ElementType.Char: result = targetModule.CorLibTypes.Char; break;
				case ElementType.I1: result = targetModule.CorLibTypes.SByte; break;
				case ElementType.U1: result = targetModule.CorLibTypes.Byte; break;
				case ElementType.I2: result = targetModule.CorLibTypes.Int16; break;
				case ElementType.U2: result = targetModule.CorLibTypes.UInt16; break;
				case ElementType.I4: result = targetModule.CorLibTypes.Int32; break;
				case ElementType.U4: result = targetModule.CorLibTypes.UInt32; break;
				case ElementType.I8: result = targetModule.CorLibTypes.Int64; break;
				case ElementType.U8: result = targetModule.CorLibTypes.UInt64; break;
				case ElementType.R4: result = targetModule.CorLibTypes.Single; break;
				case ElementType.R8: result = targetModule.CorLibTypes.Double; break;
				case ElementType.String: result = targetModule.CorLibTypes.String; break;
				case ElementType.TypedByRef: result = targetModule.CorLibTypes.TypedReference; break;
				case ElementType.I: result = targetModule.CorLibTypes.IntPtr; break;
				case ElementType.U: result = targetModule.CorLibTypes.UIntPtr; break;
				case ElementType.Object: result = targetModule.CorLibTypes.Object; break;
				case ElementType.Ptr: result = new PtrSig(Import(type.Next)); break;
				case ElementType.ByRef: result = new ByRefSig(Import(type.Next)); break;
				case ElementType.ValueType: result = CreateClassOrValueType((type as ClassOrValueTypeSig).TypeDefOrRef, true); break;
				case ElementType.Class: result = CreateClassOrValueType((type as ClassOrValueTypeSig).TypeDefOrRef, false); break;
				case ElementType.Var: result = new GenericVar((type as GenericVar).Number, ImportTypeDef((type as GenericVar).OwnerType)); break;
				case ElementType.ValueArray: result = new ValueArraySig(Import(type.Next), (type as ValueArraySig).Size); break;
				case ElementType.FnPtr: result = new FnPtrSig(Import((type as FnPtrSig).Signature)); break;
				case ElementType.SZArray: result = new SZArraySig(Import(type.Next)); break;
				case ElementType.MVar: result = new GenericMVar((type as GenericMVar).Number, ImportMethodDef((type as GenericMVar).OwnerMethod)); break;
				case ElementType.CModReqd: result = new CModReqdSig(Import((type as ModifierSig).Modifier), Import(type.Next)); break;
				case ElementType.CModOpt: result = new CModOptSig(Import((type as ModifierSig).Modifier), Import(type.Next)); break;
				case ElementType.Module: result = new ModuleSig((type as ModuleSig).Index, Import(type.Next)); break;
				case ElementType.Sentinel: result = new SentinelSig(); break;
				case ElementType.Pinned: result = new PinnedSig(Import(type.Next)); break;

				case ElementType.Array:
					var arraySig = (ArraySig)type;
					var sizes = new List<uint>(arraySig.Sizes);
					var lbounds = new List<int>(arraySig.LowerBounds);
					result = new ArraySig(Import(type.Next), arraySig.Rank, sizes, lbounds);
					break;

				case ElementType.GenericInst:
					var gis = (GenericInstSig)type;
					var genArgs = new List<TypeSig>(gis.GenericArguments.Count);
					foreach (var ga in gis.GenericArguments)
						genArgs.Add(Import(ga));
					result = new GenericInstSig(Import(gis.GenericType) as ClassOrValueTypeSig, genArgs);
					break;

				case ElementType.End:
				case ElementType.R:
				case ElementType.Internal:
				default:
					result = null;
					break;
			}

			return result;
		}

		private TypeSig CreateClassOrValueType(ITypeDefOrRef type, bool isValueType)
		{
			var corLibType = targetModule.CorLibTypes.GetCorLibTypeSig(type);
			if (corLibType != null)
				return corLibType;

			if (isValueType)
				return new ValueTypeSig(Import(type));
			return new ClassSig(Import(type));
		}

		private void ImportCustomAttributes(IHasCustomAttribute target, IHasCustomAttribute source) =>
			ImportCustomAttributes(target.CustomAttributes, source);

		private void ImportCustomAttributes(IList<CustomAttribute> targetList, IHasCustomAttribute source)
		{
			foreach (var ca in source.CustomAttributes)
				targetList.Add(Import(ca));
		}

		private ICustomAttributeType Import(ICustomAttributeType caType)
		{
			var mr = caType as MemberRef;
			if (mr != null)
			{
				Debug.Assert(mr.IsMethodRef);
				if (mr.IsMethodRef)
					return (ICustomAttributeType)Import((IMethod)mr);
				return (ICustomAttributeType)Import((IField)mr);
			}
			var md = caType as MethodDef;
			if (md != null)
				return oldMethodToNewMethod[md].TargetMember;
			return null;
		}

		private CustomAttribute Import(CustomAttribute ca)
		{
			if (ca == null)
				return null;
			if (ca.IsRawBlob)
				return new CustomAttribute(Import(ca.Constructor), ca.RawData);

			var importedCustomAttribute = new CustomAttribute(Import(ca.Constructor));
			foreach (var arg in ca.ConstructorArguments)
				importedCustomAttribute.ConstructorArguments.Add(Import(arg));
			foreach (var namedArg in ca.NamedArguments)
				importedCustomAttribute.NamedArguments.Add(Import(namedArg));

			return importedCustomAttribute;
		}

		private CAArgument Import(CAArgument arg) => new CAArgument(Import(arg.Type), ImportCAValue(arg.Value));

		private object ImportCAValue(object value)
		{
			if (value is CAArgument)
				return Import((CAArgument)value);
			if (value is IList<CAArgument>)
			{
				var args = (IList<CAArgument>)value;
				var newArgs = ThreadSafeListCreator.Create<CAArgument>(args.Count);
				foreach (var arg in args)
					newArgs.Add(Import(arg));
				return newArgs;
			}
			if (value is TypeSig)
				return Import((TypeSig)value);
			return value;
		}

		private CANamedArgument Import(CANamedArgument namedArg) =>
			new CANamedArgument(namedArg.IsField, Import(namedArg.Type), namedArg.Name, Import(namedArg.Argument));

		private void ImportDeclSecurities(IHasDeclSecurity target, IHasDeclSecurity source) =>
			ImportDeclSecurities(target.DeclSecurities, source);

		private void ImportDeclSecurities(IList<DeclSecurity> targetList, IHasDeclSecurity source)
		{
			foreach (var ds in source.DeclSecurities)
				targetList.Add(Import(ds));
		}

		private DeclSecurity Import(DeclSecurity ds)
		{
			if (ds == null)
				return null;

			var importedDeclSecurity = targetModule.UpdateRowId(new DeclSecurityUser());
			ImportCustomAttributes(importedDeclSecurity, ds);
			importedDeclSecurity.Action = ds.Action;
			foreach (var sa in ds.SecurityAttributes)
				importedDeclSecurity.SecurityAttributes.Add(Import(sa));

			return importedDeclSecurity;
		}

		private SecurityAttribute Import(SecurityAttribute sa)
		{
			if (sa == null)
				return null;

			var importedSecurityAttribute = new SecurityAttribute(Import(sa.AttributeType));
			foreach (var namedArg in sa.NamedArguments)
				importedSecurityAttribute.NamedArguments.Add(Import(namedArg));

			return importedSecurityAttribute;
		}

		private Constant Import(Constant constant)
		{
			if (constant == null)
				return null;
			return targetModule.UpdateRowId(new ConstantUser(constant.Value, constant.Type));
		}

		private MarshalType Import(MarshalType marshalType)
		{
			if (marshalType == null)
				return null;

			if (marshalType is RawMarshalType)
			{
				var mt = (RawMarshalType)marshalType;
				return new RawMarshalType(mt.Data);
			}

			if (marshalType is FixedSysStringMarshalType)
			{
				var mt = (FixedSysStringMarshalType)marshalType;
				return new FixedSysStringMarshalType(mt.Size);
			}

			if (marshalType is SafeArrayMarshalType)
			{
				var mt = (SafeArrayMarshalType)marshalType;
				return new SafeArrayMarshalType(mt.VariantType, Import(mt.UserDefinedSubType));
			}

			if (marshalType is FixedArrayMarshalType)
			{
				var mt = (FixedArrayMarshalType)marshalType;
				return new FixedArrayMarshalType(mt.Size, mt.ElementType);
			}

			if (marshalType is ArrayMarshalType)
			{
				var mt = (ArrayMarshalType)marshalType;
				return new ArrayMarshalType(mt.ElementType, mt.ParamNumber, mt.Size, mt.Flags);
			}

			if (marshalType is CustomMarshalType)
			{
				var mt = (CustomMarshalType)marshalType;
				return new CustomMarshalType(mt.Guid, mt.NativeTypeName, Import(mt.CustomMarshaler), mt.Cookie);
			}

			if (marshalType is InterfaceMarshalType)
			{
				var mt = (InterfaceMarshalType)marshalType;
				return new InterfaceMarshalType(mt.NativeType, mt.IidParamIndex);
			}

			Debug.Assert(marshalType.GetType() == typeof(MarshalType));
			return new MarshalType(marshalType.NativeType);
		}

		private ImplMap Import(ImplMap implMap)
		{
			if (implMap == null)
				return null;
			return targetModule.UpdateRowId(new ImplMapUser(Import(implMap.Module, false), implMap.Name, implMap.Attributes));
		}

		private ModuleRef Import(ModuleRef module, bool canConvertToTargetModule)
		{
			var name = canConvertToTargetModule && IsSourceOrTarget(module) ? targetModule.Name : module.Name;
			var importedModuleRef = targetModule.UpdateRowId(new ModuleRefUser(targetModule, name));
			ImportCustomAttributes(importedModuleRef, module);
			return importedModuleRef;
		}

		private ClassLayout Import(ClassLayout classLayout)
		{
			if (classLayout == null)
				return null;
			return targetModule.UpdateRowId(new ClassLayoutUser(classLayout.PackingSize, classLayout.ClassSize));
		}

		private CallingConventionSig Import(CallingConventionSig signature)
		{
			if (signature == null)
				return null;

			if (signature is MethodSig)
				return Import((MethodSig)signature);
			if (signature is FieldSig)
				return Import((FieldSig)signature);
			if (signature is GenericInstMethodSig)
				return Import((GenericInstMethodSig)signature);
			if (signature is PropertySig)
				return Import((PropertySig)signature);
			if (signature is LocalSig)
				return Import((LocalSig)signature);
			return null;
		}

		private MethodSig Import(MethodSig sig)
		{
			if (sig == null)
				return null;
			return Import(new MethodSig(sig.GetCallingConvention()), sig);
		}

		private PropertySig Import(PropertySig sig)
		{
			if (sig == null)
				return null;
			return Import(new PropertySig(sig.HasThis), sig);
		}

		private T Import<T>(T sig, T old) where T : MethodBaseSig
		{
			sig.RetType = Import(old.RetType);
			foreach (var p in old.Params)
				sig.Params.Add(Import(p));
			sig.GenParamCount = old.GenParamCount;
			var paramsAfterSentinel = sig.ParamsAfterSentinel;
			if (paramsAfterSentinel != null)
			{
				foreach (var p in old.ParamsAfterSentinel)
					paramsAfterSentinel.Add(Import(p));
			}
			return sig;
		}

		private FieldSig Import(FieldSig sig)
		{
			if (sig == null)
				return null;
			return new FieldSig(Import(sig.Type));
		}

		private GenericInstMethodSig Import(GenericInstMethodSig sig)
		{
			if (sig == null)
				return null;

			var result = new GenericInstMethodSig();
			foreach (var l in sig.GenericArguments)
				result.GenericArguments.Add(Import(l));

			return result;
		}

		private LocalSig Import(LocalSig sig)
		{
			if (sig == null)
				return null;

			var result = new LocalSig();
			foreach (var l in sig.Locals)
				result.Locals.Add(Import(l));

			return result;
		}

		private static readonly bool keepImportedRva = false;

		private RVA GetRVA(RVA rva) => keepImportedRva ? rva : 0;

		private void Create(FieldDef field)
		{
			var importedField = targetModule.UpdateRowId(new FieldDefUser(field.Name));
			oldFieldToNewField.Add(field, new MemberInfo<FieldDef>(importedField, importedField));
		}

		private void Create(MethodDef method)
		{
			var importedMethodDef = targetModule.UpdateRowId(new MethodDefUser(method.Name));
			oldMethodToNewMethod.Add(method, new MemberInfo<MethodDef>(importedMethodDef, importedMethodDef));
		}

		private void Create(PropertyDef propDef)
		{
			var importedPropertyDef = targetModule.UpdateRowId(new PropertyDefUser(propDef.Name));
			oldPropertyToNewProperty.Add(propDef, new MemberInfo<PropertyDef>(importedPropertyDef, importedPropertyDef));
		}

		private void Create(EventDef eventDef)
		{
			var importedEventDef = targetModule.UpdateRowId(new EventDefUser(eventDef.Name));
			oldEventToNewEvent.Add(eventDef, new MemberInfo<EventDef>(importedEventDef, importedEventDef));
		}

		private FieldDef Initialize(FieldDef field)
		{
			if (field == null)
				return null;
			var importedField = oldFieldToNewField[field].EditedMember;
			ImportCustomAttributes(importedField, field);
			importedField.Signature = Import(field.Signature);
			importedField.Attributes = field.Attributes;
			importedField.RVA = GetRVA(field.RVA);
			importedField.InitialValue = field.InitialValue;
			importedField.Constant = Import(field.Constant);
			importedField.FieldOffset = field.FieldOffset;
			importedField.MarshalType = Import(field.MarshalType);
			importedField.ImplMap = Import(field.ImplMap);
			return importedField;
		}

		private MethodDef Initialize(MethodDef method)
		{
			if (method == null)
				return null;
			var importedMethodDef = oldMethodToNewMethod[method].EditedMember;
			ImportCustomAttributes(importedMethodDef, method);
			ImportDeclSecurities(importedMethodDef, method);
			importedMethodDef.RVA = GetRVA(method.RVA);
			importedMethodDef.ImplAttributes = method.ImplAttributes;
			importedMethodDef.Attributes = method.Attributes;
			importedMethodDef.Signature = Import(method.Signature);
			importedMethodDef.SemanticsAttributes = method.SemanticsAttributes;
			importedMethodDef.ImplMap = Import(method.ImplMap);
			foreach (var paramDef in method.ParamDefs)
				importedMethodDef.ParamDefs.Add(Import(paramDef));
			foreach (var genericParam in method.GenericParameters)
				importedMethodDef.GenericParameters.Add(Import(genericParam));
			foreach (var ovr in method.Overrides)
				importedMethodDef.Overrides.Add(new MethodOverride(Import(ovr.MethodBody), Import(ovr.MethodDeclaration)));
			importedMethodDef.Parameters.UpdateParameterTypes();
			return importedMethodDef;
		}

		private GenericParam Import(GenericParam gp)
		{
			if (gp == null)
				return null;
			var importedGenericParam = targetModule.UpdateRowId(new GenericParamUser(gp.Number, gp.Flags, gp.Name));
			ImportCustomAttributes(importedGenericParam, gp);
			importedGenericParam.Kind = Import(gp.Kind);
			foreach (var gpc in gp.GenericParamConstraints)
				importedGenericParam.GenericParamConstraints.Add(Import(gpc));
			return importedGenericParam;
		}

		private GenericParamConstraint Import(GenericParamConstraint gpc)
		{
			if (gpc == null)
				return null;
			var importedGenericParamConstraint = targetModule.UpdateRowId(new GenericParamConstraintUser(Import(gpc.Constraint)));
			ImportCustomAttributes(importedGenericParamConstraint, gpc);
			return importedGenericParamConstraint;
		}

		private InterfaceImpl Import(InterfaceImpl ifaceImpl)
		{
			if (ifaceImpl == null)
				return null;
			var importedInterfaceImpl = targetModule.UpdateRowId(new InterfaceImplUser(Import(ifaceImpl.Interface)));
			ImportCustomAttributes(importedInterfaceImpl, ifaceImpl);
			return importedInterfaceImpl;
		}

		private ParamDef Import(ParamDef paramDef)
		{
			if (paramDef == null)
				return null;
			var importedParamDef = targetModule.UpdateRowId(new ParamDefUser(paramDef.Name, paramDef.Sequence, paramDef.Attributes));
			ImportCustomAttributes(importedParamDef, paramDef);
			importedParamDef.MarshalType = Import(paramDef.MarshalType);
			importedParamDef.Constant = Import(paramDef.Constant);
			return importedParamDef;
		}

		private PropertyDef Initialize(PropertyDef propDef)
		{
			if (propDef == null)
				return null;
			var importedPropertyDef = oldPropertyToNewProperty[propDef].EditedMember;
			ImportCustomAttributes(importedPropertyDef, propDef);
			importedPropertyDef.Attributes = propDef.Attributes;
			importedPropertyDef.Type = Import(propDef.Type);
			importedPropertyDef.Constant = Import(propDef.Constant);
			foreach (var m in propDef.GetMethods)
			{
				var newMethod = TryGetMethod(m);
				if (newMethod != null)
					importedPropertyDef.GetMethods.Add(newMethod);
			}
			foreach (var m in propDef.SetMethods)
			{
				var newMethod = TryGetMethod(m);
				if (newMethod != null)
					importedPropertyDef.SetMethods.Add(newMethod);
			}
			foreach (var m in propDef.OtherMethods)
			{
				var newMethod = TryGetMethod(m);
				if (newMethod != null)
					importedPropertyDef.OtherMethods.Add(newMethod);
			}
			return importedPropertyDef;
		}

		private MethodDef TryGetMethod(MethodDef method)
		{
			if (method == null)
				return null;
			var m = oldMethodToNewMethod[method].TargetMember;
			if (usedMethods.Contains(m))
				return null;
			usedMethods.Add(m);
			return m;
		}

		private EventDef Initialize(EventDef eventDef)
		{
			if (eventDef == null)
				return null;
			var importedEventDef = oldEventToNewEvent[eventDef].EditedMember;
			importedEventDef.EventType = Import(eventDef.EventType);
			importedEventDef.Attributes = eventDef.Attributes;
			ImportCustomAttributes(importedEventDef, eventDef);
			if (eventDef.AddMethod != null)
			{
				var newMethod = TryGetMethod(eventDef.AddMethod);
				if (newMethod != null)
					importedEventDef.AddMethod = newMethod;
			}
			if (eventDef.InvokeMethod != null)
			{
				var newMethod = TryGetMethod(eventDef.InvokeMethod);
				if (newMethod != null)
					importedEventDef.InvokeMethod = newMethod;
			}
			if (eventDef.RemoveMethod != null)
			{
				var newMethod = TryGetMethod(eventDef.RemoveMethod);
				if (newMethod != null)
					importedEventDef.RemoveMethod = newMethod;
			}
			foreach (var m in eventDef.OtherMethods)
			{
				var newMethod = TryGetMethod(m);
				if (newMethod != null)
					importedEventDef.OtherMethods.Add(newMethod);
			}
			return importedEventDef;
		}

		private CilBody CreateBody(MethodDef paramsSourceMethod, MethodDef sourceMethod)
		{
			// NOTE: Both methods can be identical: targetMethod == sourceMethod

			var sourceBody = sourceMethod.Body;
			if (sourceBody == null)
				return null;

			var targetBody = new CilBody();
			targetBody.KeepOldMaxStack = sourceBody.KeepOldMaxStack;
			targetBody.InitLocals = sourceBody.InitLocals;
			targetBody.HeaderSize = sourceBody.HeaderSize;
			targetBody.MaxStack = sourceBody.MaxStack;
			targetBody.LocalVarSigTok = sourceBody.LocalVarSigTok;

			bodyDict.Clear();
			foreach (var local in sourceBody.Variables)
			{
				var newLocal = new Local(Import(local.Type), local.Name);
				bodyDict[local] = newLocal;
				newLocal.PdbAttributes = local.PdbAttributes;
				targetBody.Variables.Add(newLocal);
			}

			int si = sourceMethod.IsStatic ? 0 : 1;
			int ti = paramsSourceMethod.IsStatic ? 0 : 1;
			if (sourceMethod.Parameters.Count - si != paramsSourceMethod.Parameters.Count - ti)
				throw new InvalidOperationException();
			for (; si < sourceMethod.Parameters.Count && ti < paramsSourceMethod.Parameters.Count; si++, ti++)
				bodyDict[sourceMethod.Parameters[si]] = paramsSourceMethod.Parameters[ti];

			foreach (var instr in sourceBody.Instructions)
			{
				var newInstr = new Instruction(instr.OpCode, instr.Operand);
				newInstr.Offset = instr.Offset;
				newInstr.SequencePoint = instr.SequencePoint?.Clone();
				bodyDict[instr] = newInstr;
				targetBody.Instructions.Add(newInstr);
			}

			foreach (var eh in sourceBody.ExceptionHandlers)
			{
				var newEh = new dnlib.DotNet.Emit.ExceptionHandler(eh.HandlerType);
				newEh.TryStart = GetInstruction(bodyDict, eh.TryStart);
				newEh.TryEnd = GetInstruction(bodyDict, eh.TryEnd);
				newEh.FilterStart = GetInstruction(bodyDict, eh.FilterStart);
				newEh.HandlerStart = GetInstruction(bodyDict, eh.HandlerStart);
				newEh.HandlerEnd = GetInstruction(bodyDict, eh.HandlerEnd);
				newEh.CatchType = Import(eh.CatchType);
				targetBody.ExceptionHandlers.Add(newEh);
			}

			foreach (var newInstr in targetBody.Instructions)
			{
				var op = newInstr.Operand;
				if (op == null)
					continue;

				object obj;
				if (bodyDict.TryGetValue(op, out obj))
				{
					newInstr.Operand = obj;
					continue;
				}

				var oldList = op as IList<Instruction>;
				if (oldList != null)
				{
					var targets = new Instruction[oldList.Count];
					for (int i = 0; i < oldList.Count; i++)
						targets[i] = GetInstruction(bodyDict, oldList[i]);
					newInstr.Operand = targets;
					continue;
				}

				var tdr = op as ITypeDefOrRef;
				if (tdr != null)
				{
					newInstr.Operand = Import(tdr);
					continue;
				}

				var method = op as IMethod;
				if (method != null && method.IsMethod)
				{
					newInstr.Operand = Import(method);
					continue;
				}

				var field = op as IField;
				if (field != null)
				{
					newInstr.Operand = Import(field);
					continue;
				}

				var msig = op as MethodSig;
				if (msig != null)
				{
					newInstr.Operand = Import(msig);
					continue;
				}

				Debug.Assert(op is sbyte || op is byte || op is int || op is long || op is float || op is double || op is string);
			}

			return targetBody;
		}

		private static Instruction GetInstruction(Dictionary<object, object> dict, Instruction instr)
		{
			object obj;
			if (instr == null || !dict.TryGetValue(instr, out obj))
				return null;
			return (Instruction)obj;
		}

		private IMethod Import(IMethod method)
		{
			if (method == null)
				return null;

			var md = method as MethodDef;
			if (md != null)
				return oldMethodToNewMethod[md].TargetMember;

			var ms = method as MethodSpec;
			if (ms != null)
			{
				var importedMethodSpec = new MethodSpecUser(Import(ms.Method), Import(ms.GenericInstMethodSig));
				ImportCustomAttributes(importedMethodSpec, ms);
				return importedMethodSpec;
			}

			var mr = (MemberRef)method;
			ImportedType importedType;
			var td = TryGetTypeInTargetModule(mr.Class as ITypeDefOrRef, out importedType);
			if (td != null)
			{
				var targetMethod = FindMethod(td, mr);
				if (targetMethod != null)
					return targetMethod;
				if (importedType != null)
				{
					var compiledMethod = FindMethod(toExtraData[importedType].CompiledType, mr);
					if (compiledMethod != null)
						return oldMethodToNewMethod[compiledMethod].TargetMember;
				}

				AddError(IM0004, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindMethod, mr));
				return null;
			}

			return ImportNoCheckForDefs(mr);
		}

		private MethodDef FindMethod(TypeDef targetType, MemberRef mr)
		{
			var comparer = new ImportSigComparer(importSigComparerOptions, SIG_COMPARER_OPTIONS, targetModule);
			foreach (var method in targetType.Methods)
			{
				if (!UTF8String.Equals(method.Name, mr.Name))
					continue;
				if (comparer.Equals(method.MethodSig, mr.MethodSig))
					return method;
			}
			return null;
		}

		private IField Import(IField field)
		{
			if (field == null)
				return null;

			var fd = field as FieldDef;
			if (fd != null)
				return oldFieldToNewField[fd].TargetMember;

			var mr = (MemberRef)field;
			ImportedType importedType;
			var td = TryGetTypeInTargetModule(mr.Class as ITypeDefOrRef, out importedType);
			if (td != null)
			{
				var targetField = FindField(td, mr);
				if (targetField != null)
					return targetField;
				if (importedType != null)
				{
					var compiledField = FindField(toExtraData[importedType].CompiledType, mr);
					if (compiledField != null)
						return oldFieldToNewField[compiledField].TargetMember;
				}

				AddError(IM0005, string.Format(dnSpy_AsmEditor_Resources.ERR_IM_CouldNotFindField, mr));
				return null;
			}

			return ImportNoCheckForDefs(mr);
		}

		private FieldDef FindField(TypeDef targetType, MemberRef mr)
		{
			var comparer = new ImportSigComparer(importSigComparerOptions, SIG_COMPARER_OPTIONS, targetModule);
			foreach (var field in targetType.Fields)
			{
				if (!UTF8String.Equals(field.Name, mr.Name))
					continue;
				if (comparer.Equals(field.FieldSig, mr.FieldSig))
					return field;
			}
			return null;
		}

		private MemberRef ImportNoCheckForDefs(MemberRef mr)
		{
			var importedMemberRef = targetModule.UpdateRowId(new MemberRefUser(targetModule, mr.Name));
			ImportCustomAttributes(importedMemberRef, mr);
			importedMemberRef.Signature = Import(mr.Signature);
			importedMemberRef.Class = Import(mr.Class);
			return importedMemberRef;
		}

		private IMemberRefParent Import(IMemberRefParent cls)
		{
			if (cls == null)
				return null;

			var tdr = cls as ITypeDefOrRef;
			if (tdr != null)
				return Import(tdr);

			var md = cls as MethodDef;
			if (md != null)
				return oldMethodToNewMethod[md].TargetMember;

			var modRef = cls as ModuleRef;
			if (modRef != null)
				return Import(modRef, true);

			throw new InvalidOperationException();
		}
	}
}