using System.Collections.Generic;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
	public interface IRecipeSearchService
	{
		/// <summary>
		/// Searches for recipes
		/// </summary>
		IList<RecipeSummary> SearchRecipes(RecipeSearchOptions recipeSearchOptions);
	}
}