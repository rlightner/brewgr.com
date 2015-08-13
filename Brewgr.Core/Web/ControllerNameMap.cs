using System;

namespace ctorx.Core.Web
{
	/// <summary>
	/// Used to map controllers by name to a different name
	/// </summary>
	public class ControllerNameMap
	{
		/// <summary>
		/// Gets or sets the source controller name 
		/// </summary>
		public string SourceName { get; set; }

		/// <summary>
		/// Gets or sets the target controller name
		/// </summary>
		public string TargetName { get; set; }
	}
}