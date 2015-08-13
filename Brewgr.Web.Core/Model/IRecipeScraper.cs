using System;

namespace Brewgr.Web.Core.Model
{
	public interface IRecipeScraper
	{
		/// <summary>
		/// Scrapes a Recipe from a Url
		/// </summary>
		Recipe Scrape(string url);
	}
}