using System.Web.Mvc;

namespace ctorx.Core.Web
{
	public interface ISeoSitemap
	{
		/// <summary>
		/// Generates the Xml Sitemap
		/// </summary>
		/// <param name="urlHelper"> </param>
		string GenerateXml(UrlHelper urlHelper);
	}
}