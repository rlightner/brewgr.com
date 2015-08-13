using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace ctorx.Core.Web
{
	/// <summary>
	/// Taken from: http://christesene.com/mvc-3-action-filters-enable-page-compression-gzip/
	/// </summary>
	public class EnableCompressionAttribute : ActionFilterAttribute
	{
		const CompressionMode Compress = CompressionMode.Compress;

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpRequestBase request = filterContext.HttpContext.Request;
			HttpResponseBase response = filterContext.HttpContext.Response;
			string acceptEncoding = request.Headers["Accept-Encoding"];
			if (acceptEncoding == null)
				return;
			else if (acceptEncoding.ToLower().Contains("gzip"))
			{
				response.Filter = new GZipStream(response.Filter, Compress);
				response.AppendHeader("Content-Encoding", "gzip");
			}
			else if (acceptEncoding.ToLower().Contains("deflate"))
			{
				response.Filter = new DeflateStream(response.Filter, Compress);
				response.AppendHeader("Content-Encoding", "deflate");
			}
		}
	}
}