using System;
using System.Collections.Generic;
using System.Linq;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Models
{
	public class HomePageViewModel
	{
		/// <summary>
		/// Gets or sets the NewRecipes
		/// </summary>
		public IList<RecipeSummary> NewRecipes { get; set; }

		/// <summary>
		/// Gets or sets the TopContributors
		/// </summary>
		public IList<UserSummary> TopContributors { get; set; }

		/// <summary>
		/// Gets or sets the PopularRecipes
		/// </summary>
		public IList<RecipeSummary> PopularRecipes { get; set; }
	}
}