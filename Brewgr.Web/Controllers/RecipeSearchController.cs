using System;
using System.Linq;
using System.Web.Mvc;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using ctorx.Core.Messaging;

namespace Brewgr.Web.Controllers
{
	[RoutePrefix("")]
	public class RecipeSearchController : BrewgrController
	{
		readonly IBeerStyleService BeerStyleService;
		readonly IRecipeSearchService RecipeSearchService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public RecipeSearchController(IBeerStyleService beerStyleService, IRecipeSearchService recipeSearchService)
		{
			this.BeerStyleService = beerStyleService;
			this.RecipeSearchService = recipeSearchService;
		}

		[Route("homebrew-recipe-finder")]
		public ActionResult RecipeFinder()
		{
			ViewBag.Styles = this.BeerStyleService.GetStyleSummaries();
			return this.View();
		}

		[HttpPost]
		[Route("homebrew-recipe-finder")]
		public ActionResult RecipeFinder(RecipeSearchOptions recipeSearchOptions)
		{
			var recipes = this.RecipeSearchService.SearchRecipes(recipeSearchOptions);

			if(!recipes.Any())
			{		
				this.AppendMessage(new InfoMessage { Text = "We couldn't find any recipes that match your search options" });
				return this.View(recipeSearchOptions);
			}

			return this.View("RecipeFinderResults", new RecipeSearchResults { RecipeSearchOptions = recipeSearchOptions, Recipes = recipes });
		}
	}
}