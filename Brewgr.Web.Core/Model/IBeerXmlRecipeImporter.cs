using System;

namespace Brewgr.Web.Core.Model
{
	public interface IBeerXmlRecipeImporter
	{
		/// <summary>
		/// Imports a Recipe from Beer Xml
		/// </summary>
		Recipe Import(string beerXml);
	}
}