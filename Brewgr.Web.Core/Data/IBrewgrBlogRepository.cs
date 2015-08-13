using System;
using System.Collections.Generic;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Data
{
	public interface IBrewgrBlogRepository
	{
		/// <summary>
		/// Searches Blog Posts
		/// </summary>
		IEnumerable<BlogPost> SearchBlogPosts(string searchTerm);
	}
}