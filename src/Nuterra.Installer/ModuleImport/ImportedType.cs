using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nuterra.Installer.ModuleImport
{
	abstract class ImportedType
	{
		/// <summary>
		/// New or existing type in target module
		/// </summary>
		public TypeDef TargetType { get; protected set; }
	}

	/// <summary>
	/// This is a new type that got imported into the target module
	/// </summary>
	sealed class NewImportedType : ImportedType
	{
		public NewImportedType(TypeDef targetType)
		{
			TargetType = targetType;
		}
	}

	struct EditedProperty
	{
		public PropertyDef OriginalProperty { get; }
		public PropertyDefOptions PropertyDefOptions { get; }
		public EditedProperty(PropertyDef originalProperty, PropertyDefOptions propertyDefOptions)
		{
			OriginalProperty = originalProperty;
			PropertyDefOptions = propertyDefOptions;
		}
	}

	struct EditedEvent
	{
		public EventDef OriginalEvent { get; }
		public EventDefOptions EventDefOptions { get; }
		public EditedEvent(EventDef originalEvent, EventDefOptions eventDefOptions)
		{
			OriginalEvent = originalEvent;
			EventDefOptions = eventDefOptions;
		}
	}

	struct EditedMethod
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

	struct EditedField
	{
		public FieldDef OriginalField { get; }
		public FieldDefOptions FieldDefOptions { get; }
		public EditedField(FieldDef originalField, FieldDefOptions fieldDefOptions)
		{
			OriginalField = originalField;
			FieldDefOptions = fieldDefOptions;
		}
	}

	enum MergeKind
	{
		Rename,
		Merge,
		Edit,
	}
}
