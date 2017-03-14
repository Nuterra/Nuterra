using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nuterra.Build.ModuleImport
{
	internal abstract class ImportedType
	{
		/// <summary>
		/// New or existing type in target module
		/// </summary>
		public TypeDef TargetType { get; protected set; }
	}

	/// <summary>
	/// This is a new type that got imported into the target module
	/// </summary>
	internal sealed class NewImportedType : ImportedType
	{
		public NewImportedType(TypeDef targetType)
		{
			TargetType = targetType;
		}
	}

	internal struct EditedProperty
	{
		public PropertyDef OriginalProperty { get; }
		public PropertyDefOptions PropertyDefOptions { get; }

		public EditedProperty(PropertyDef originalProperty, PropertyDefOptions propertyDefOptions)
		{
			OriginalProperty = originalProperty;
			PropertyDefOptions = propertyDefOptions;
		}
	}

	internal struct EditedEvent
	{
		public EventDef OriginalEvent { get; }
		public EventDefOptions EventDefOptions { get; }

		public EditedEvent(EventDef originalEvent, EventDefOptions eventDefOptions)
		{
			OriginalEvent = originalEvent;
			EventDefOptions = eventDefOptions;
		}
	}

	internal struct EditedMethod
	{
		public MethodDef OriginalMethod { get; }
		public MethodBody NewBody { get; }
		public MethodDefOptions MethodDefOptions { get; }

		public EditedMethod(MethodDef originalMethod, MethodBody newBody, MethodDefOptions methodDefOptions)
		{
			OriginalMethod = originalMethod;
			NewBody = newBody;
			MethodDefOptions = methodDefOptions;
		}
	}

	internal struct EditedField
	{
		public FieldDef OriginalField { get; }
		public FieldDefOptions FieldDefOptions { get; }

		public EditedField(FieldDef originalField, FieldDefOptions fieldDefOptions)
		{
			OriginalField = originalField;
			FieldDefOptions = fieldDefOptions;
		}
	}

	internal enum MergeKind
	{
		Rename,
		Merge,
		Edit,
	}
}