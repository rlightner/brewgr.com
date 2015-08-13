using System;
using System.Web;

namespace ctorx.Core.Identity
{
	public class HttpRequestUserHostAddressResolver : IUserHostAddressResolver
	{
		/// <summary>
		/// Resolves a user host address
		/// </summary>
		public string Resolve()
		{
			return HttpContext.Current.Request.UserHostAddress;
		}
	}
}