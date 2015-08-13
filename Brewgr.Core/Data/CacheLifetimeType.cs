using System;

namespace ctorx.Core.Data
{
	public enum CacheLifetimeType
	{
		/// <summary>
		/// The cached data will live as long as the Application is running
		/// </summary>
		Forever,

		/// <summary>
		/// The cached data will live for a fixed amount of time
		/// </summary>
		FixedLength,

		/// <summary>
		/// The cached data will live for an unspecified amount of time based on importance
		/// </summary>
		Variable
	}
}