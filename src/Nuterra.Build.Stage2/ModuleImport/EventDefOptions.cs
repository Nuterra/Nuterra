using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace Nuterra.Build.ModuleImport
{
	internal sealed class EventDefOptions
	{
		public EventAttributes Attributes;
		public UTF8String Name;
		public ITypeDefOrRef EventType;
		public MethodDef AddMethod;
		public MethodDef InvokeMethod;
		public MethodDef RemoveMethod;
		public List<MethodDef> OtherMethods = new List<MethodDef>();
		public List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();

		public EventDefOptions()
		{
		}

		public EventDefOptions(EventDef evt)
		{
			Attributes = evt.Attributes;
			Name = evt.Name;
			EventType = evt.EventType;
			AddMethod = evt.AddMethod;
			InvokeMethod = evt.InvokeMethod;
			RemoveMethod = evt.RemoveMethod;
			OtherMethods.AddRange(evt.OtherMethods);
			CustomAttributes.AddRange(evt.CustomAttributes);
		}

		public EventDef CopyTo(EventDef evt)
		{
			evt.Attributes = Attributes;
			evt.Name = Name ?? UTF8String.Empty;
			evt.EventType = EventType;
			evt.AddMethod = AddMethod;
			evt.InvokeMethod = InvokeMethod;
			evt.RemoveMethod = RemoveMethod;
			evt.OtherMethods.Clear();
			evt.OtherMethods.AddRange(OtherMethods);
			evt.CustomAttributes.Clear();
			evt.CustomAttributes.AddRange(CustomAttributes);
			return evt;
		}

		public EventDef CreateEventDef(ModuleDef ownerModule) => ownerModule.UpdateRowId(CopyTo(new EventDefUser()));

		public static EventDefOptions Create(UTF8String name, ITypeDefOrRef eventType)
		{
			return new EventDefOptions
			{
				Attributes = 0,
				Name = name,
				EventType = eventType,
			};
		}
	}
}