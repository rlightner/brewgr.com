using System;

namespace Brewgr.Web.Core.Configuration
{
	public interface IFacebookConnectSettings
	{
		/// <summary>
		/// Gets or sets the ApplicationKey
		/// </summary>
		string ApplicationKey { get; }

		/// <summary>
		/// Gets or sets the ApplicationSecret
		/// </summary>
		string ApplicationSecret { get; }
	}
}