using System;
using System.Collections.Generic;
using System.Linq;
using Brewgr.Web.Core.Model;
using ctorx.Core.Collections;

namespace Brewgr.Web.Models
{
	public class UnCategorizedRecipesViewModel : PageableViewModel
	{
		/// <summary>
		/// Gets or sets the Recipes
		/// </summary>
		public IList<RecipeSummary> Recipes { get; set; }
	}
}