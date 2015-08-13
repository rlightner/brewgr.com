using System;
using System.Collections.Generic;
using System.Linq;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Models
{
	public class RecipeBrewSessionsViewModel
	{
		/// <summary>
		/// Gets or sets the RecipeSummary
		/// </summary>
		public RecipeSummaryViewModel RecipeSummary { get; set; }

		/// <summary>
		/// Gets or sets the BrewSessions
		/// </summary>
		public IList<BrewSessionSummary> BrewSessions { get; set; }
	}
}