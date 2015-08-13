using System.Collections.Generic;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
	public interface ISearchService
	{
		/// <summary>
		/// Performs a Search
		/// </summary>
		SearchResult Search(string searchTerm);
	}
}