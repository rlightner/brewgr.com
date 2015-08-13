using System;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

namespace ctorx.Core.Mapping
{
	public class AutoMapperModule : NinjectModule
	{
		readonly Assembly TargetAssembly;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public AutoMapperModule(Assembly targetAssembly)
		{
			this.TargetAssembly = targetAssembly;
		}

		/// <summary>
		/// Loads the module into the kernel.
		/// </summary>
		public override void Load()
		{
			var mappingDefTypes = this.TargetAssembly
				.GetTypes()
				.Where(x => !x.IsAbstract && typeof(IMappingDefinition).IsAssignableFrom(x))
				.ToList();

			foreach(var mappingDefType in mappingDefTypes)
			{
				var instance = this.Kernel.GetService(mappingDefType) as IMappingDefinition;
				instance.DefineMappings();
			}
		}
	}
}