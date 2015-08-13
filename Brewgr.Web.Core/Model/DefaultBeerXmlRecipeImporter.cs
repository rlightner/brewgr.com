using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Brewgr.Web.Core.Service;

namespace Brewgr.Web.Core.Model
{
	public class DefaultBeerXmlRecipeImporter : IBeerXmlRecipeImporter
	{
		readonly IRecipeUnitConverter RecipeUnitConverter;
		readonly IRecipeDataService BrewDataService;
		readonly IBeerStyleService BeerStyleService;
		readonly int? UserId;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultBeerXmlRecipeImporter(IRecipeUnitConverter recipeUnitConverter, IRecipeDataService brewDataService, IBeerStyleService beerStyleService,
			IUserResolver userResolver)
		{
			this.RecipeUnitConverter = recipeUnitConverter;
			this.BrewDataService = brewDataService;
			this.BeerStyleService = beerStyleService;

			// Detect User Id
			var user = userResolver.Resolve();
			if(user != null)
			{
				this.UserId = user.UserId;
			}
		}

		/// <summary>
		/// Imports a Recipe from Beer Xml
		/// </summary>
		public Recipe Import(string beerXml)
		{
			if (string.IsNullOrWhiteSpace(beerXml))
			{
				throw new ArgumentNullException("beerXml");
			}

			var recipe = new Recipe();
			recipe.UnitTypeId = (int)UnitType.USStandard;
			recipe.IbuFormulaId = (int)IbuFormula.Tinseth;

			// Parse the Xml
			try
			{
				var xdoc = XDocument.Parse(beerXml);

				var entryPoint =
					xdoc.Elements()
					    .FirstOrDefault(x => x.Name.LocalName == "RECIPES")
					    .Elements()
					    .FirstOrDefault(x => x.Name.LocalName == "RECIPE");

				// Set Recipe Info
				try
				{
					this.SetRecipeInfo(recipe, entryPoint);
				}
				catch (Exception ex)
				{
				}

				// Hops
				try
				{
					this.SetHops(recipe, entryPoint);
				}
				catch (Exception ex)
				{
				}

				// Fermentables
				try
				{
					this.SetFermentables(recipe, entryPoint);
				}
				catch(Exception ex)
				{
				}

				// Yeasts
				try
				{
					this.SetYeasts(recipe, entryPoint);
				}
				catch (Exception ex)
				{
				}

				// Miscs
				try
				{
					this.SetMiscs(recipe, entryPoint);
				}
				catch (Exception ex)
				{
				}

				return recipe;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Sets the miscs on the recipe
		/// </summary>
		void SetMiscs(Recipe recipe, XElement entryPoint)
		{
			var allMiscs = this.BrewDataService.GetUsableIngredients<Adjunct>(this.UserId);
			recipe.Adjuncts = new List<RecipeAdjunct>();

			if (entryPoint.Element("MISCS") != null)
			{
				foreach (var miscElement in entryPoint.Element("MISCS").Elements("MISC"))
				{
					var misc = new RecipeAdjunct();

					var matchingAdjunct = allMiscs.FirstOrDefault(x => x.Name.Trim().ToLower() == miscElement.Element("NAME").Value.Trim().ToLower());
					if (matchingAdjunct != null)
					{
						misc.IngredientId = matchingAdjunct.IngredientId;
						misc.Adjunct = matchingAdjunct;
					}
					else
					{
						misc.IngredientId = 0;
						misc.Adjunct = new Adjunct();
						misc.Adjunct.Name = miscElement.Element("NAME").Value;
					}

					// Detect Unit
					if (miscElement.Element("amount_is_weight") != null)
					{
						misc.Unit = (miscElement.Element("amount_is_weight").Value.ToLower() == "true") ? "oz" : "floz";
					}

					misc.Amount = Math.Round((misc.Unit == "oz" ?
						this.RecipeUnitConverter.ConvertKilogramsToOunces(Convert.ToDouble(miscElement.Element("AMOUNT").Value)) :
						this.RecipeUnitConverter.ConvertLitersToFluidOunces(Convert.ToDouble(miscElement.Element("AMOUNT").Value))) * 10000) / 10000;

					misc.AdjunctUsageTypeId = (int)AdjunctUsageType.Boil;
					var usageType = miscElement.Element("use").Value.ToLower();
					if (usageType == "mash")
					{
						misc.AdjunctUsageTypeId = (int) AdjunctUsageType.Mash;
					} 
					else if (usageType == "primary")
					{
						misc.AdjunctUsageTypeId = (int) AdjunctUsageType.Primary;
					}
					else if (usageType == "secondary")
					{
						misc.AdjunctUsageTypeId = (int) AdjunctUsageType.Secondary;
					}
					else if (usageType == "bottling")
					{
						misc.AdjunctUsageTypeId = (int) AdjunctUsageType.Bottle;
					}

					recipe.Adjuncts.Add(misc);
				}
			}
		}

		/// <summary>
		/// Sets the yeasts on the recipe
		/// </summary>
		void SetYeasts(Recipe recipe, XElement entryPoint)
		{
			var allYeasts = this.BrewDataService.GetUsableIngredients<Yeast>(this.UserId);
			recipe.Yeasts = new List<RecipeYeast>();
			if (entryPoint.Element("YEASTS") != null)
			{
				foreach (var yeastElement in entryPoint.Element("YEASTS").Elements("YEAST"))
				{
					var yeast = new RecipeYeast();

					var matchingYeast = allYeasts.FirstOrDefault(x => x.Name.Trim().ToLower() == yeastElement.Element("NAME").Value.Trim().ToLower());
					if (matchingYeast != null)
					{
						yeast.IngredientId = matchingYeast.IngredientId;
						yeast.Yeast = matchingYeast;
					}
					else
					{
						yeast.IngredientId = 0;
						yeast.Yeast = new Yeast();
						yeast.Yeast.Name = yeastElement.Element("NAME").Value;
					}

					yeast.Attenuation = .75d;
					var attenuation = yeastElement.Element("ATTENUATION") != null ? Convert.ToDouble(yeastElement.Element("ATTENUATION").Value) : (double?)null;
					if (attenuation != null)
					{
						yeast.Attenuation = Math.Round(Convert.ToDouble(yeastElement.Element("ATTENUATION").Value)) / 100;
					}

					recipe.Yeasts.Add(yeast);
				}
			}
		}

		/// <summary>
		/// Sets the fermentables on the recipe
		/// </summary>
		void SetFermentables(Recipe recipe, XElement entryPoint)
		{
			var allFermentables = this.BrewDataService.GetUsableIngredients<Fermentable>(this.UserId);
			recipe.Fermentables = new List<RecipeFermentable>();
			if (entryPoint.Element("FERMENTABLES") != null)
			{
				foreach (var fermentableElement in entryPoint.Element("FERMENTABLES").Elements("FERMENTABLE"))
				{
					var fermentable = new RecipeFermentable();

					var matchingFermentable =
						allFermentables.FirstOrDefault(
							x => x.Name.Trim().ToLower() == fermentableElement.Element("NAME").Value.Trim().ToLower());
					if (matchingFermentable != null)
					{
						fermentable.IngredientId = matchingFermentable.IngredientId;
						fermentable.Fermentable = matchingFermentable;
					}
					else
					{
						fermentable.IngredientId = 0;
						fermentable.Fermentable = new Fermentable();
						fermentable.Fermentable.Name = fermentableElement.Element("NAME").Value;
					}

					var kilograms = Convert.ToDouble(fermentableElement.Element("AMOUNT").Value);
					var pounds = this.RecipeUnitConverter.ConvertKilogramsToPounds(kilograms);
					fermentable.Amount = Math.Round(pounds * 10000) / 10000;

					fermentable.Ppg = Convert.ToInt16(Math.Round(Convert.ToDouble(fermentableElement.Element("YIELD").Value)*46.214*0.01, 2));
					fermentable.Lovibond = Convert.ToInt32(Math.Round(Convert.ToDouble(fermentableElement.Element("COLOR").Value), 0));

					var fermentableUsageType = fermentableElement.Element("TYPE").Value.ToLower();
					fermentable.FermentableUsageTypeId = (int) FermentableUsageType.Mash;

					if (fermentableUsageType == "grain")
					{
						fermentable.FermentableUsageTypeId = (int) FermentableUsageType.Mash;
					}
					else if (fermentableUsageType.IndexOf("extract") > -1)
					{
						fermentable.FermentableUsageTypeId = (int) FermentableUsageType.Extract;
					}
					else if (fermentableUsageType == "sugar")
					{
						fermentable.FermentableUsageTypeId = (int) FermentableUsageType.Late;
					}

					recipe.Fermentables.Add(fermentable);
				}
			}
		}

		/// <summary>
		/// Sets the hops on the Recipe
		/// </summary>
		void SetHops(Recipe recipe, XElement entryPoint)
		{
			var allHops = this.BrewDataService.GetUsableIngredients<Hop>(this.UserId);
			recipe.Hops = new List<RecipeHop>();
			if (entryPoint.Element("HOPS") != null)
			{
				foreach (var hopElement in entryPoint.Element("HOPS").Elements("HOP"))
				{
					var hop = new RecipeHop();

					var matchingHop = allHops.FirstOrDefault(x => x.Name.Trim().ToLower() == hopElement.Element("NAME").Value.Trim().ToLower());

					if (matchingHop != null)
					{
						hop.IngredientId = matchingHop.IngredientId;
						hop.Hop = matchingHop;
					}
					else
					{
						hop.IngredientId = 0;
						hop.Hop = new Hop();
						hop.Hop.Name = hopElement.Element("NAME").Value;
						hop.Hop.AA = Convert.ToDouble(hopElement.Element("ALPHA").Value);
					}

					hop.AlphaAcidAmount = Convert.ToDouble(hopElement.Element("ALPHA").Value);
					hop.Amount = Math.Round(this.RecipeUnitConverter.ConvertKilogramsToOunces(Convert.ToDouble(hopElement.Element("AMOUNT").Value)) * 10000) / 10000;
					hop.TimeInMinutes = Convert.ToInt16(hopElement.Element("TIME").Value);
					hop.HopTypeId = (int) HopType.Pellet;

					var hopUsageType = hopElement.Element("USE").Value.Trim().ToLower();
					if (hopUsageType == "boil")
					{
						hop.HopUsageTypeId = (int) HopUsageType.Boil;
					}
					else if (hopUsageType == "mash")
					{
						hop.HopUsageTypeId = (int) HopUsageType.Mash;
					}
					else if (hopUsageType == "dry hop")
					{
						hop.HopUsageTypeId = (int) HopUsageType.DryHop;
					}
					else
					{
						hop.HopUsageTypeId = (int) HopUsageType.Boil;
					}

					recipe.Hops.Add(hop);
				}
			}
		}

		/// <summary>
		///Sets the Recipe Info
		/// </summary>
		void SetRecipeInfo(Recipe recipe, XElement entryPoint)
		{
			// Name
			recipe.RecipeName = entryPoint.Element("NAME") != null ? entryPoint.Element("NAME").Value.Replace("(exported from brewgr.com)", "") : null;

			// Style
			if (entryPoint.Element("STYLE") != null)
			{
				var categoryNumber = entryPoint.Element("STYLE").Element("CATEGORY_NUMBER").Value;
				var styleLetter = entryPoint.Element("STYLE").Element("STYLE_LETTER").Value;

				var subCategoryId = string.Concat(categoryNumber, styleLetter);
				var style = this.BeerStyleService.GetStyleSummaries().FirstOrDefault(x => x.SubCategoryId == subCategoryId);

				if (style != null)
				{
					recipe.BjcpStyleSubCategoryId = style.SubCategoryId;
					recipe.BjcpStyleSummary = style;
				}
			}

			// Batch Size
			recipe.BatchSize = 5;
			var batchSize = entryPoint.Element("BATCH_SIZE") != null ? Convert.ToDouble(entryPoint.Element("BATCH_SIZE").Value) : (double?) null;
			if (batchSize != null)
			{
				recipe.BatchSize = Math.Round(this.RecipeUnitConverter.ConvertLitersToGallons(batchSize.Value) * 10000) / 10000;
			}

			// Boil Size
			recipe.BoilSize = 6.5;
			var boilSize = entryPoint.Element("BOIL_SIZE") != null ? Convert.ToDouble(entryPoint.Element("BOIL_SIZE").Value) : (double?) null;
			if (boilSize != null)
			{
				recipe.BoilSize = Math.Round(this.RecipeUnitConverter.ConvertLitersToGallons(boilSize.Value) * 10000) / 10000;
			}

			// Boil Time
			recipe.BoilTime = 60;
			var boilTime = entryPoint.Element("BOIL_TIME") != null ? Convert.ToInt16(entryPoint.Element("BOIL_TIME").Value) : (int?) null;
			if (boilTime != null)
			{
				recipe.BoilTime = boilTime.Value;
			}

			// Efficiency
			recipe.Efficiency = .75d;
			var efficiency = entryPoint.Element("EFFICIENCY") != null ? Convert.ToDouble(entryPoint.Element("EFFICIENCY").Value) : (double?)null;
			if (efficiency != null)
			{
				recipe.Efficiency = Math.Round(efficiency.Value) / 100;
			}

			// IBU Formula
			var ibuMethod = entryPoint.Element("IBU_METHOD") != null ? entryPoint.Element("IBU_METHOD").Value : null;
			if(ibuMethod != null)
			{
				if(ibuMethod.ToLower() == "tinseth")
				{
					recipe.IbuFormulaId = (int)IbuFormula.Tinseth;
				} 
				else if(ibuMethod.ToLower() == "rager")
				{
					recipe.IbuFormulaId = (int)IbuFormula.Rager;
				}
				else if(ibuMethod.ToLower() == "brewgr")
				{
					recipe.IbuFormulaId = (int)IbuFormula.Brewgr;
				}
				else
				{
					recipe.IbuFormulaId = (int)IbuFormula.Tinseth;
				}
			}
		}
	}
}