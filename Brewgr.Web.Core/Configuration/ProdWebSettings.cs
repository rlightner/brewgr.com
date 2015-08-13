using System;
using System.Collections.Generic;

namespace Brewgr.Web.Core.Configuration
{
	public class ProdWebSettings : AbstractWebSettings
	{
		/// <summary>
		/// Gets the RootPath
		/// </summary>
		public override string RootPath
		{
			get { return "http://brewgr.com"; }
		}

		/// <summary>
		/// Gets the RootPathSecure
		/// </summary>
		public override string RootPathSecure
		{
			get { return "https://brewgr.com"; }
		}

		/// <summary>
		/// Gets the static root path
		/// </summary>
		public override string StaticRootPath
		{
			get { return "http://static.brewgr.com"; }
		}

		/// <summary>
		/// Gets the secure static root path
		/// </summary>
		public override string StaticRootPathSecure
		{
			get { return "https://static.brewgr.com"; }
		}

		/// <summary>
		/// Gets a value specifying whether or not https is disabled
		/// </summary>
		public override bool DisableHttps
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the MediaPhysicalRoot
		/// </summary>
		public override string MediaPhysicalRoot
		{
			get { return Environment.GetEnvironmentVariable("Setting_MediaPhysicalRoot"); }
		}
	}
}