using System;
using System.Linq;
using System.Net;
using System.Web;
using Brewgr.Web.Core.Configuration;
using Brewgr.Web.Core.Model;
using Facebook;

namespace Brewgr.Web.Core.Service
{
	public class DefaultFacebookService : IFacebookConnectService
	{
		readonly IFacebookConnectSettings FacebookConnectSettings;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultFacebookService(IFacebookConnectSettings facebookConnectSettings)
		{
			this.FacebookConnectSettings = facebookConnectSettings;
		}

		/// <summary>
		/// Gets user info from an oauth response code
		/// </summary>
		public OAuthUserInfo GetUserInfoFromOAuthCode(string code, string loginUrl)
		{
			var accessToken = this.AcquireAccessTokenFromAuthCode(code, loginUrl);

			var client = new FacebookClient(accessToken);
			dynamic result = client.Get("me", new { fields = "id, email, first_name, last_name" });

			return new OAuthUserInfo
			{
				OAuthUserId = result.id,
				EmailAddress = result.email,
				FirstName = result.first_name,
				LastName = result.last_name,
				SourceProvider = OAuthProvider.Facebook
			};
		}

		/// <summary>
		/// Acquires an access token from an auth code
		/// </summary>
		string AcquireAccessTokenFromAuthCode(string code, string loginUrl)
		{
			var url = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
				this.FacebookConnectSettings.ApplicationKey,
				HttpUtility.UrlEncode(loginUrl.ToLower()), 
				HttpUtility.UrlEncode(this.FacebookConnectSettings.ApplicationSecret), 
				HttpUtility.UrlEncode(code));

			// Get Access Token
			var webClient = new WebClient();
			var responseBody = webClient.DownloadString(url);

			return responseBody.Split('&')
				.First()
				.Replace("access_token=", "");
		}
	}
}