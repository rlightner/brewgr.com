using System;
using System.Configuration;

namespace ctorx.Core.Configuration
{
	public static class ConfigReader
	{
		/// <summary>
		/// Reads App Settings
		/// </summary>
		public static class AppSettings
		{
			/// <summary>
			/// Reads an app setting from the confioguration file
			/// </summary>
			public static string Read(string key)
			{
				return ConfigurationManager.AppSettings[key];
			}
		}

		/// <summary>
		/// Reads Connection Strings
		/// </summary>
		public static class ConnectionStrings
		{
			/// <summary>
			/// Reads a Connection String from the configuration file
			/// </summary>
			public static string Read(string name)
			{
				return ConfigurationManager.ConnectionStrings[name].ConnectionString;
			}
		}
	}
}