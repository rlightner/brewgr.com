using System;
using System.Collections.Generic;

namespace Brewgr.Web.Core.Configuration
{
	public class DevWebSettings : AbstractWebSettings
	{
		/// <summary>
		/// Gets the RootPath
		/// </summary>
		public override string RootPath
		{
			get { return "http://dev.brewgr.com"; }
		}

		/// <summary>
		/// Gets the RootPathSecure
		/// </summary>
		public override string RootPathSecure
		{
			get { return "https://dev.brewgr.com"; }
		}

		/// <summary>
		/// Gets the static root path
		/// </summary>
		public override string StaticRootPath
		{
			get { return "http://static.dev.brewgr.com"; }
		}

		/// <summary>
		/// Gets the secure static root path
		/// </summary>
		public override string StaticRootPathSecure
		{
			get { return "https://static.dev.brewgr.com"; }
		}

		/// <summary>
		/// Gets a value specifying whether or not https is disabled
		/// </summary>
		public override bool DisableHttps
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the MediaPhysicalRoot
		/// </summary>
		public override string MediaPhysicalRoot
		{
			get { return "D:\\REPOS\\brewgr\\trunk\\Brewgr.Web\\Media"; }
		}
	}
}