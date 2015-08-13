using System;

namespace Brewgr.Web.Core.Configuration
{
	public class DefaultFacebookConnectSettings : IFacebookConnectSettings
	{
		readonly IWebSettings WebSettings;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultFacebookConnectSettings(IWebSettings webSettings)
		{
			this.WebSettings = webSettings;
		}

		/// <summary>
		/// Gets or sets the ApplicationKey
		/// </summary>
		public string ApplicationKey
		{
			get { return Environment.GetEnvironmentVariable("FB_ApplicationKey") ?? "ThisIsTheDevFBKeyAndShouldNotChange"; }
		}

		/// <summary>
		/// Gets or sets the ApplicationSecret
		/// </summary>
		public string ApplicationSecret
		{
			get { return Environment.GetEnvironmentVariable("FB_ApplicationSecret") ?? "ThisIsTheDevFBSecretAndShouldNotChange"; }
		}
	}
}