using System;
using System.Collections.Generic;
using System.Linq;

namespace Brewgr.Web.Models
{
	public class BrowseRecipesViewModel
	{
		/// <summary>
		/// Gets or sets the Page
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		/// Gets or sets the list of Bjcp Categories
		/// </summary>
		public IList<BjcpCategoryViewModel> BjcpCategories { get; set; }

		/// <summary>
		/// Gets or sets the RecipeCount
		/// </summary>
		public int RecipeCount { get; set; }

		/// <summary>
		/// Gets or sets the UnCategorizedRecipeCount
		/// </summary>
		public int UnCategorizedRecipeCount { get; set; }

        /// <summary>
        /// Gets the first hald of the category list
        /// </summary>
	    public IList<BjcpCategoryViewModel> GetFirstHalf()
	    {
	        return this.BjcpCategories.OrderBy(x => x.CategoryId).Take(Convert.ToInt32(Math.Floor(this.BjcpCategories.Count / 2d))).ToList();
	    }

        /// <summary>
        /// Gets the second half of the category list
        /// </summary>
        public IList<BjcpCategoryViewModel> GetSecondHalf()
        {
            return this.BjcpCategories.OrderBy(x => x.CategoryId).Skip(Convert.ToInt32(Math.Floor(this.BjcpCategories.Count / 2d))).ToList();
        }
    }
}