using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Start9.Api.Modules
{
	[Serializable]
	public abstract class Module
	{
		private static readonly List<Module> _modules = new List<Module>();

		static Module()
		{
		}

		public static ReadOnlyCollection<Module> Modules => _modules.AsReadOnly();

		public abstract string Name { get; }

		public Guid Guid { get; } = Guid.NewGuid();

		public static void Poke()
		{
			//this runs the ctor
		}

		protected virtual void MessageRecieved<T>(Message<T> message)
		{
			// TBD
		}

		protected virtual void MessageRecieved(Message message)
		{
		}


		public override string ToString()
		{
			return $"[{GetType().Assembly}]\"{Name}\":{Guid}";
		}
	}
}