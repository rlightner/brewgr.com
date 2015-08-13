using System;
using System.Linq;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
	public interface IFacebookConnectService
	{
		/// <summary>
		/// Gets user info from an oauth response code
		/// </summary>
		OAuthUserInfo GetUserInfoFromOAuthCode(string code, string loginUrl);
	}
}