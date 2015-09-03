using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Brewgr.Web.Core.Configuration;
using ctorx.Core.Collections;
using ctorx.Core.Conversion;
using ctorx.Core.Data;
using ctorx.Core.Formatting;
using ctorx.Core.Messaging;
using ctorx.Core.Serialization;
using ctorx.Core.Web;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Service;
using Brewgr.Web.Models;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Controllers
{
	public class RecipeController : BrewgrController
	{
		readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
		readonly IRecipeService RecipeService;
		readonly IBeerStyleService BeerStyleService;
        readonly IStaticContentService StaticContentService;
        readonly IUserService UserService;
		readonly INotificationService NotificationService;
		readonly IPartnerIdResolver PartnerIdResolver;
		readonly IBeerXmlRecipeExporter BeerXmlExporter;
		readonly IBeerXmlRecipeImporter BeerXmlImporter;
		readonly IPartnerService PartnerService;
		readonly ISendToShopService SendToShopService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public RecipeController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IRecipeService recipeService, IBeerStyleService beerStyleService, 
			IStaticContentService staticContentService, IUserService userService, INotificationService notificationService, IPartnerIdResolver partnerIdResolver,
			IBeerXmlRecipeExporter beerXmlExporter, IBeerXmlRecipeImporter beerXmlImporter, IPartnerService partnerService, ISendToShopService sendToShopService)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.RecipeService = recipeService;
			this.BeerStyleService = beerStyleService;
			this.StaticContentService = staticContentService;
            this.UserService = userService;
			this.NotificationService = notificationService;
			this.PartnerIdResolver = partnerIdResolver;
			this.BeerXmlExporter = beerXmlExporter;
			this.BeerXmlImporter = beerXmlImporter;
			this.PartnerService = partnerService;
			this.SendToShopService = sendToShopService;
		}

		#region BROWSE RECIPES / STYLE DETAIL

		/// <summary>
		/// Executes the View for Recipes
		/// </summary>
		[ActionName("homebrew-recipes")]
		public ViewResult Recipes()
		{
			var styles = this.BeerStyleService.GetStyleSummaries();
			var uncategorizedCount = this.BeerStyleService.GetUnCategorizedRecipeCount();

			var model = Mapper.Map(styles, new BrowseRecipesViewModel());
			model.UnCategorizedRecipeCount = uncategorizedCount;

			return View("Recipes", model);
		}

		[ActionName("other-homebrew-recipes")]
		public ActionResult UnCategorizedRecipes(int? page)
		{
			var pager = new Pager { CurrentPage = page ?? 1, ItemsPerPage = this.WebSettings.DefaultRecipesPerPage };

			var recipes = this.BeerStyleService.GetUnCategorizedRecipesPage(pager);

			if (recipes.Any() && !pager.IsInRange())
			{
				return this.Issue404();
			}

			return View("UnCategorized", new UnCategorizedRecipesViewModel { Recipes = recipes, Pager = pager, 
				BaseUrl = Url.Action("other-homebrew-recipes", "Recipe", new { page = (int?)null }, "http")});
		}

		/// <summary>
		/// Executes the View for StyleDetail
		/// </summary>
		public ActionResult StyleDetail(string urlFriendlyName, int? page)
		{
			// 301 for old page 1 URL from previous button
			if(page != null && page.Value == 1)
			{
				return this.RedirectPermanent(Request.Url.ToString().Replace("/1", ""));
			}

			var style = this.BeerStyleService.GetStyleByUrlFriendlyName(urlFriendlyName.ToLower().Replace("-recipes", ""));

			if(style == null)
			{
				return this.Issue404();
			}

			var pager = new Pager { CurrentPage = page ?? 1, ItemsPerPage = this.WebSettings.DefaultRecipesPerPage };
			
			var styleRecipes = this.BeerStyleService.GetStyleRecipesPage(style.SubCategoryId, pager);

			if(styleRecipes.Any() && !pager.IsInRange())
			{
				return this.Issue404();
			}

			var topRatedRecipes = this.BeerStyleService.GetTopRatedRecipes(style.SubCategoryId, 5);

			var model = new StyleDetailViewModel
			{
				BjcpStyle = style, 
				Recipes = styleRecipes, 
				Pager = pager, 
				BaseUrl = Url.StyleDetailUrl(urlFriendlyName),
				TopRatedRecipes = topRatedRecipes
			};

			return View(model);
		}

		#endregion
		
		#region SAVE RECIPE 

		[HttpPost]
		[Authorize]
		[ForceHttps]
		public ActionResult SaveRecipe(PostedRecipeViewModel postedRecipeViewModel)
		{
			// NOTE: This action handles saving for both new recipes
			// and edited recipes.  New Recipes post to this action while
			// edits post to this action via Ajax.

			var isNewRecipe = false;

			// Hydrate JSON ReceipeViewModel
			var recipeViewModel = postedRecipeViewModel.HydrateRecipeJson();
			isNewRecipe = recipeViewModel.IsNewRecipe();

			// Validation (client validates also ... this to ensure data consistency)
			var validator = new RecipeViewModelValidator();
			var validationResult = validator.Validate(recipeViewModel);
			if(!validationResult.IsValid)
			{
				if(isNewRecipe)
				{
					this.AppendMessage(new ErrorMessage { Text = "Did you leave something blank?  Please check your entries and try again."});
					ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();
					return this.View("NewRecipe", recipeViewModel);
				}

				// Signals Invalid
				return this.Content("0");
			}

			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{				
				try
				{
					var recipe = recipeViewModel.IsNewRecipe() ? new Recipe() : this.RecipeService.GetRecipeById(recipeViewModel.RecipeId);

					// Issue 404 if recipe does not exists or not owned by user
					if (!isNewRecipe && !recipe.WasCreatedBy(this.ActiveUser.UserId))
					{
						return this.Issue404();
					}

					// Map Recipe
					Mapper.Map(recipeViewModel, recipe);

					#region INGREDIENT DELETIONS

					if(!isNewRecipe)
					{
						// Delete Fermentables that were removed
						var fermentablesForDeletion = recipe.Fermentables.Except(recipe.Fermentables.Join(recipeViewModel.Fermentables ?? new List<RecipeFermentableViewModel>(),
							x => x.RecipeFermentableId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
						this.RecipeService.MarkRecipeIngredientsForDeletion<RecipeFermentable>(fermentablesForDeletion);

						// Delete Hops that were removed
						var hopsForDeletion = recipe.Hops.Except(recipe.Hops.Join(recipeViewModel.Hops ?? new List<RecipeHopViewModel>(),
							x => x.RecipeHopId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
						this.RecipeService.MarkRecipeIngredientsForDeletion<RecipeHop>(hopsForDeletion);

						// Delete Yeasts that were removed
						var yeastsForDeletion = recipe.Yeasts.Except(recipe.Yeasts.Join(recipeViewModel.Yeasts ?? new List<RecipeYeastViewModel>(),
							x => x.RecipeYeastId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
						this.RecipeService.MarkRecipeIngredientsForDeletion<RecipeYeast>(yeastsForDeletion);

						// Delete Adjuncts that were removed
						var adjunctsForDeletion = recipe.Adjuncts.Except(recipe.Adjuncts.Join(recipeViewModel.Others ?? new List<RecipeOtherViewModel>(),
							x => x.RecipeAdjunctId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
						this.RecipeService.MarkRecipeIngredientsForDeletion<RecipeAdjunct>(adjunctsForDeletion);

                        // Delete MashSteps that were removed
                        var mashStepsForDeletion = recipe.MashSteps.Except(recipe.MashSteps.Join(recipeViewModel.MashSteps ?? new List<RecipeMashStepViewModel>(),
                            x => x.RecipeMashStepId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
                        this.RecipeService.MarkRecipeIngredientsForDeletion<RecipeMashStep>(mashStepsForDeletion);
					}

					#endregion

					#region INGREDIENT ADDITIONS

					// Add Fermentables
					if (recipeViewModel.Fermentables != null)
					{
						recipeViewModel.Fermentables.Where(x => Converter.Convert<int>(x.Id) == 0)
							.ForEach(x => recipe.Fermentables.Add(Mapper.Map(x, new RecipeFermentable())));
					}

					// Add Hops
					if (recipeViewModel.Hops != null)
					{
						recipeViewModel.Hops.Where(x => Converter.Convert<int>(x.Id) == 0)
							.ForEach(x => recipe.Hops.Add(Mapper.Map(x, new RecipeHop())));
					}

					// Add Yeasts
					if (recipeViewModel.Yeasts != null)
					{
						recipeViewModel.Yeasts.Where(x => Converter.Convert<int>(x.Id) == 0)
							.ForEach(x => recipe.Yeasts.Add(Mapper.Map(x, new RecipeYeast())));
					}

					// Add Adjuncts
					if (recipeViewModel.Others != null)
					{
						recipeViewModel.Others.Where(x => Converter.Convert<int>(x.Id) == 0)
							.ForEach(x => recipe.Adjuncts.Add(Mapper.Map(x, new RecipeAdjunct())));
					}

                    // Add MashStep
                    if (recipeViewModel.MashSteps != null)
                    {
                        recipeViewModel.MashSteps.Where(x => Converter.Convert<int>(x.Id) == 0)
                            .ForEach(x => recipe.MashSteps.Add(Mapper.Map(x, new RecipeMashStep())));
                    }

					#endregion

					#region INGREDIENT UPDATES

					if (!isNewRecipe)
					{
						// Update Fermentables
						if (recipeViewModel.Fermentables != null)
						{
							foreach (var recipeFermentableViewModel in recipeViewModel.Fermentables.Where(x => Converter.Convert<int>(x.Id) > 0))
							{
								var match = recipe.Fermentables.FirstOrDefault(x => x.RecipeFermentableId == Converter.Convert<int>(recipeFermentableViewModel.Id));
								if (match == null)
								{
									throw new InvalidOperationException("Unable to find matching fermentable");
								}

								Mapper.Map(recipeFermentableViewModel, match);
							}
						}

						// Update Hops
						if (recipeViewModel.Hops != null)
						{
							foreach (var recipeHopViewModel in recipeViewModel.Hops.Where(x => Converter.Convert<int>(x.Id) > 0))
							{
								var match = recipe.Hops.FirstOrDefault(x => x.RecipeHopId == Converter.Convert<int>(recipeHopViewModel.Id));
								if (match == null)
								{
									throw new InvalidOperationException("Unable to find matching Hop");
								}

								Mapper.Map(recipeHopViewModel, match);
							}
						}

						// Update Yeasts
						if (recipeViewModel.Yeasts != null)
						{
							foreach (var recipeYeastViewModel in recipeViewModel.Yeasts.Where(x => Converter.Convert<int>(x.Id) > 0))
							{
								var match = recipe.Yeasts.FirstOrDefault(x => x.RecipeYeastId == Converter.Convert<int>(recipeYeastViewModel.Id));
								if (match == null)
								{
									throw new InvalidOperationException("Unable to find matching Yeast");
								}

								Mapper.Map(recipeYeastViewModel, match);
							}
						}

						// Update Adjuncts
						if (recipeViewModel.Others != null)
						{
							foreach (var recipeAdjunctViewModel in recipeViewModel.Others.Where(x => Converter.Convert<int>(x.Id) > 0))
							{
								var match = recipe.Adjuncts.FirstOrDefault(x => x.RecipeAdjunctId == Converter.Convert<int>(recipeAdjunctViewModel.Id));
								if (match == null)
								{
									throw new InvalidOperationException("Unable to find matching Adjunct");
								}

								Mapper.Map(recipeAdjunctViewModel, match);
							}
						}

                        // Update MashSteps
                        if (recipeViewModel.MashSteps != null)
                        {
                            foreach (var recipeMashStepViewModel in recipeViewModel.MashSteps.Where(x => Converter.Convert<int>(x.Id) > 0))
                            {
                                var match = recipe.MashSteps.FirstOrDefault(x => x.RecipeMashStepId == Converter.Convert<int>(recipeMashStepViewModel.Id));
                                if (match == null)
                                {
                                    throw new InvalidOperationException("Unable to find matching MashStep");
                                }

                                Mapper.Map(recipeMashStepViewModel, match);
                            }
                        }
					}

					#endregion

					#region STEP DELETIONS / ADDITIONS / UPDATES

					// Deletions
					if (!isNewRecipe)
					{
						var stepsForDeletion = recipe.Steps.Except(recipe.Steps.Join(recipeViewModel.Steps ?? new List<RecipeStepViewModel>(),
							x => x.RecipeStepId, y => Converter.Convert<int>(y.Id), (x, y) => x)).ToList();
						this.RecipeService.MarkRecipeStepsForDeletion(stepsForDeletion);
					}

					// Additions
					if (recipeViewModel.Steps != null)
					{
						if(recipe.Steps == null)
						{
							recipe.Steps = new List<RecipeStep>();
						}

						recipeViewModel.GetSteps()
							.Where(x => Converter.Convert<int>(x.Id) == 0)
							.Where(x => !string.IsNullOrWhiteSpace(x.Text))
							.ForEach(x => recipe.Steps.Add(Mapper.Map(x, new RecipeStep { DateCreated = DateTime.Now })));
					}

					// Updates
					if (!isNewRecipe)
					{
						if (recipeViewModel.Steps != null)
						{
							foreach (var recipeStep in recipeViewModel.GetSteps()
								.Where(x => Converter.Convert<int>(x.Id) > 0)
								.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
							{
								var match = recipe.Steps.FirstOrDefault(x => x.RecipeStepId == Converter.Convert<int>(recipeStep.Id));
								if (match == null)
								{
									throw new InvalidOperationException("Unable to find matching step");
								}

								match = Mapper.Map(recipeStep, match);
								match.DateModified = DateTime.Now;
							}
						}
					}

					#endregion

					// Save the Image
					if(isNewRecipe)
					{
						if(recipeViewModel.PhotoForUpload != null)
						{
							// Save the New Image
							recipe.ImageUrlRoot = this.StaticContentService.SaveRecipeImage(recipeViewModel.PhotoForUpload.InputStream,
								this.WebSettings.MediaPhysicalRoot);
						}
					}

					// Finalize Recipe
					this.RecipeService.FinalizeRecipe(recipe);

					unitOfWork.Commit();

					if(isNewRecipe)
					{
						this.ForwardMessage(new SuccessMessage { Text = BrewgrMessages.RecipeSaved });
						return Redirect(Url.RecipeEditUrl(recipe.RecipeId));
					}
					else
					{
						// Signals Success
						return Content("1");
					}
				}
				catch (Exception ex)
				{
					this.LogHandledException(ex);
					unitOfWork.Rollback();

					if(isNewRecipe)
					{
						ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();
						this.AppendMessage(new ErrorMessage { Text = GenericMessages.ErrorMessage });
						return View("NewRecipe", recipeViewModel);
					}
					else
					{
						// Signals Failure
						return Content("-1");
					}
				}
			}
		}

		#endregion

		#region NEW RECIPE

		/// <summary>
		/// Executes the View for RecipeClone
		/// </summary>
		[ForceHttps]
		public ActionResult RecipeClone(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);

			if (recipe == null)
			{
				return this.Issue404();
			}

			ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();

			var cloned = Mapper.Map(recipe, new RecipeViewModel());
			cloned.RecipeId = 0;
			cloned.OriginalRecipeId = recipe.RecipeId;
			cloned.Name = "Clone Of " + recipe.RecipeName;
			cloned.Description = null;

			// Reset Recipe Ingredent Ids to 0 
			cloned.Fermentables.ForEach(x => x.Id = "0");
			cloned.Hops.ForEach(x => x.Id = "0");
			cloned.Yeasts.ForEach(x => x.Id = "0");
			cloned.Others.ForEach(x => x.Id = "0");
			cloned.Steps.ForEach(x => x.Id = "0");

			// Add Messaging
			this.AppendMessage(new InfoMessage { Text = "You are cloning \"" + recipe.RecipeName + "\".  Once you have made your changes, click \"Save Recipe\"" });

			return View("NewRecipe", cloned);
		}

		/// <summary>
		/// Executes the View for RecipeBuilder301
		/// </summary>
		[ActionName("homebrew-recipe-builder")]
		public ActionResult RecipeBuilder301()
		{
			return RedirectPermanent(Url.Action("homebrew-recipe-calculator"));
		}

		/// <summary>
		/// Executes the View for NewRecipe
		/// </summary>
		[ActionName("homebrew-recipe-calculator")]
		[ForceHttps]
		public ViewResult NewRecipe()
		{
			ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();

			// Source Recipe (or Default) // TODO: Derive Defaults from user preferences
			var recipe = new RecipeViewModel();
			recipe.UnitType = "s";
			recipe.BatchSize = 5;
			recipe.BoilSize = 6.5;
			recipe.BoilTime = 60;
			recipe.Efficiency = .75;
			recipe.IbuFormula = "t";

			return View("NewRecipe", recipe);
		}

		#endregion

		#region NEW ING ROWS

		[ActionName("buildertemplates-v2")]
		[ForceHttps]
		public ViewResult BuilderTemplates()
		{
			return View("_BuilderTemplates");			
		}

		#endregion

		#region EDIT RECIPE

		/// <summary>
		/// Executes the View for RecipeEdit
		/// </summary>
		[Authorize]
		[ForceHttps]
		public ActionResult RecipeEdit(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);
			
			// Issue 404 if recipe does not exists or not owned by user
			if (recipe == null || !recipe.WasCreatedBy(this.ActiveUser.UserId))
			{
				return this.Issue404();
			}

			// Notify user that the recipe is not yet public
			if (!recipe.IsPublic)
			{
				this.AppendMessage(new WarnMessage { Text = "This recipe is not complete and only you can see it.  Add Fermentables and Yeast to make it public." });
			}

			ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();

			var recipeModel = Mapper.Map(recipe, new RecipeViewModel());


			// Fetch Tasting Notes
			var tastingNotes = this.RecipeService.GetRecipeTastingNotes(recipe.RecipeId);
			recipeModel.TastingNotes = tastingNotes;

			var commentWrapperViewModel = new CommentWrapperViewModel();
			commentWrapperViewModel.CommentViewModels = Mapper.Map(this.RecipeService.GetRecipeComments(recipeId), new List<CommentViewModel>());
			commentWrapperViewModel.GenericId = recipeId;
			commentWrapperViewModel.CommentType = CommentType.Recipe;
			recipeModel.CommentWrapperViewModel = commentWrapperViewModel;

			// Get the most recent brew session -- this should be added to the recipe in the service, really but hey
			recipeModel.MostRecentBrewSession = this.RecipeService.GetMostRecentBrewSession(recipeId);

			return View(recipeModel);
		}

		/// <summary>
		/// /Executes the view for ChangeRecipePhoto
		/// </summary>
		[Authorize]
		[ForceHttps]
		public ActionResult BuilderChangeRecipePhoto(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);

			if(recipe.CreatedBy != this.ActiveUser.UserId)
			{
				return this.Issue404();
			}

			this.AppendMessage(new InfoMessage { Text = "<span>Your photo is being uploaded</span><img src=\"/img/h-loader.gif\" />" });

			ViewBag.UploadComplete = false;
			return View(Mapper.Map(recipe, new RecipeViewModel()));
		}

		/// <summary>
		/// /Executes the view for ChangeRecipePhoto
		/// </summary>
		[HttpPost]
		[ForceHttps]
		public ActionResult BuilderChangeRecipePhoto(ChangeRecipePhotoViewModel changeRecipePhotoViewModel)
		{
			using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					var recipe = this.RecipeService.GetRecipeById(changeRecipePhotoViewModel.RecipeId);

					if (recipe.CreatedBy != this.ActiveUser.UserId)
					{
						return this.Issue404();
					}

					var oldImageUrlRoot = recipe.ImageUrlRoot;

					// Save the New Image
					recipe.ImageUrlRoot = this.StaticContentService.SaveRecipeImage(changeRecipePhotoViewModel.PhotoForUpload.InputStream,
						this.WebSettings.MediaPhysicalRoot);

					// Delete the Old Image (if it exists)
					if (!string.IsNullOrWhiteSpace(oldImageUrlRoot))
					{
						this.StaticContentService.DeleteRecipeImage(this.WebSettings.MediaPhysicalRoot, oldImageUrlRoot);
					}

					unitOfWork.Commit();

					this.AppendMessage(new SuccessMessage { Text = "Your photo has been uploaded." });
				}
				catch(Exception ex)
				{
					this.LogHandledException(ex);					
					unitOfWork.Rollback();

					this.AppendMessage(new ErrorMessage { Text = "There was a problem saving your photo.  Please try again."});
				}
			}

			ViewBag.UploadComplete = true;
			return View();
		}

		#endregion

		#region RECIPE DETAIL

		/// <summary>
		/// Executes the View for RecipeDetail
		/// </summary>
		public ActionResult RecipeDetail(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);

			if(recipe == null)
			{
				return this.Issue404();
			}

			// Auto Redirect to EDIT page for owner
			if (this.ActiveUser != null && recipe.CreatedBy == this.ActiveUser.UserId && string.IsNullOrWhiteSpace(Request["public"]))
			{
				return this.Redirect(Url.RecipeEditUrl(recipeId));
			}

			// Notify use if the recipe is not public
			if(!recipe.IsPublic)
			{
				this.AppendMessage(new WarnMessage { Text = "This recipe is not complete and only you can see it.  Add Fermentables and Yeast to make it public."});
			}

			// Get Similar Recipes
			ViewBag.SimilarRecipes = this.RecipeService.GetSimilarRecipes(recipe, 4);

			var recipeViewModel = Mapper.Map(recipe, new RecipeViewModel());

			// Fetch Brew Session Count (this should really go in a service as part of recipe get)
			recipeViewModel.BrewSessionCount = this.RecipeService.GetRecipeBrewSessionsCount(recipeId);

			// Fetch most recent brew session
			if(recipeViewModel.BrewSessionCount > 0)
			{
				recipeViewModel.MostRecentBrewSession = this.RecipeService.GetMostRecentBrewSession(recipeId);
			}

			// Fetch Tasting Notes
			var tastingNotes = this.RecipeService.GetRecipeTastingNotes(recipe.RecipeId);

			// Get Additional Data
            var user = this.UserService.GetUserById(recipe.CreatedBy);
			
            if ((recipeViewModel.OriginalRecipeId ?? 0) != 0)
            { 
                var originalRecipe = this.RecipeService.GetRecipeById((recipeViewModel.OriginalRecipeId ?? 0));
	            if(originalRecipe != null)
	            {
		            recipeViewModel.OriginalRecipe = Mapper.Map(originalRecipe, new RecipeViewModel());
	            }
            }
            
            var recipeDetailViewModel = new RecipeDetailViewModel();
            recipeDetailViewModel.RecipeViewModel = recipeViewModel;
            recipeDetailViewModel.UserSummary = Mapper.Map(user, new UserSummary());
			recipeDetailViewModel.TastingNotes = tastingNotes;
			
            var commentWrapperViewModel = new CommentWrapperViewModel();
            commentWrapperViewModel.CommentViewModels = Mapper.Map(this.RecipeService.GetRecipeComments(recipeId), new List<CommentViewModel>());
            commentWrapperViewModel.GenericId = recipeId;
            commentWrapperViewModel.CommentType = CommentType.Recipe;
            recipeDetailViewModel.RecipeViewModel.CommentWrapperViewModel = commentWrapperViewModel;

			// TODO: Check if the name passed in the URL is different than what
			// TODO: is in the DB.  If it is....do a 301 Redirect.  This is for SEO.
            ViewData["DisableEditing"] = true;


			// Get Send To Shop Settings (if any)
			ViewBag.SendToShopSettings = this.SendToShopService.GetRecipeCreationSendToShopSettings(false);

            return View(recipeDetailViewModel);
		}

 
        [HttpPost]
        public ActionResult AddComment(CommentAddViewModel commentAddViewModel)
        {
            if (!commentAddViewModel.Validate().IsValid)
            {
                return this.Issue404();
            }

            // Normalize the Line Breaks
            commentAddViewModel.CommentText = commentAddViewModel.CommentText.Replace("\n", Environment.NewLine);

            switch (commentAddViewModel.CommentType)
            {
                case CommentType.Recipe:
                    RecipeComment recipeComment;

                    using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
                    {
                        recipeComment = new RecipeComment();
                        recipeComment.CommentText = commentAddViewModel.CommentText;
                        recipeComment.RecipeId = commentAddViewModel.GenericId;
                        this.RecipeService.AddRecipeComment(recipeComment);
                        unitOfWork.Commit();
                    }

                    // Queue Comment Notification
                    this.NotificationService.QueueNotification(NotificationType.RecipeComment, recipeComment);
                    break;
                case CommentType.Session:
                    BrewSessionComment brewSessionComment;

                    using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
                    {
                        brewSessionComment = new BrewSessionComment();
                        brewSessionComment.CommentText = commentAddViewModel.CommentText;
                        brewSessionComment.BrewSessionId = commentAddViewModel.GenericId;
                        this.RecipeService.AddBrewSessionComment(brewSessionComment);
                        unitOfWork.Commit();
                    }

                    // Queue Comment Notification
                    this.NotificationService.QueueNotification(NotificationType.BrewSessionComment, brewSessionComment);
                    break;
                default:
                    return this.Issue404();
            }
            
            var commentViewModel = new CommentViewModel();
            commentViewModel.CommentText = commentAddViewModel.CommentText;
            commentViewModel.UserId = this.ActiveUser.UserId;
            commentViewModel.UserName = this.ActiveUser.Username;
            commentViewModel.UserAvatarUrl = UserAvatar.GetAvatar(59, this.ActiveUser.EmailAddress);
            commentViewModel.DateCreated = DateTime.Now;

            return View("_Comment", commentViewModel);
        }



		#endregion

		#region DELETE RECIPE 

		/// <summary>
        /// Executes the View for RecipeDelete
        /// </summary>
        [ForceHttps]
        public ActionResult RecipeDelete(int recipeId)
        {
            using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
            {
                try
                {
                    var recipe = this.RecipeService.GetRecipeById(recipeId);

                    // Issue 404 if recipe does not exists or not owned by user
                    if (recipe == null || !recipe.WasCreatedBy(this.ActiveUser.UserId))
                    {
                        return this.Issue404();
                    }

					// Delete the Recipe
					this.RecipeService.DeleteRecipe(recipe);

                    unitOfWork.Commit();

                    this.ForwardMessage(new SuccessMessage { Text = BrewgrMessages.RecipeDeleted });

                    if (Request.UrlReferrer == null)
                    {
                        return Redirect(Url.Action("my-recipes", "recipe", null, "http"));
                    }

                    if (Request.UrlReferrer.ToString() == Url.RecipeEditUrl(recipe.RecipeId))
                    {
                        // if they are deleting from the recipe edit page then redirect to the my recipes page
                        return Redirect("/dashboard#recipes");
                    }

                    if (Request.UrlReferrer.ToString().ToLower().Contains("/dashboard"))
                    {
                        return Redirect(Request.UrlReferrer + "#recipes");
                    }
                    else
                    {
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                }
                catch (Exception ex)
                {
                    this.LogHandledException(ex);

                    this.AppendMessage(new ErrorMessage { Text = GenericMessages.ErrorMessage });

                    unitOfWork.Rollback();

                    return this.Issue404();
                }
            }
		}

		#endregion

		#region PRINT RECIPE 

		[Route("recipe/{recipeid:int}/print")]
		public ActionResult RecipePrint(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);

            if (recipe == null)
                return this.Issue404();

            var recipeViewModel = Mapper.Map(recipe, new RecipeViewModel());
			recipeViewModel.CommentWrapperViewModel = new CommentWrapperViewModel { CommentViewModels = new List<CommentViewModel>() };

			// Get Additional Data
			var user = this.UserService.GetUserById(recipe.CreatedBy);

			var recipeDetailViewModel = new RecipeDetailViewModel
			{
				RecipeViewModel = recipeViewModel,
				UserSummary = Mapper.Map(user, new UserSummary()),
				TastingNotes = new List<TastingNote>()
			};

			return this.View(recipeDetailViewModel);
		}

		#endregion

		#region RECIPE BREW SESSIONS

		/// <summary>
		/// Executes the View for RecipeBrewSessions
		/// </summary>
		public ActionResult RecipeBrewSessions(int recipeId)
		{
			var recipeSummary = this.RecipeService.GetRecipeSummaryById(recipeId);

			if (recipeSummary == null)
			{
				return this.Issue404();
			}

			var brewSessions = this.RecipeService.GetRecipeBrewSessions(recipeId)
				.OrderByDescending(x => x.BrewDate)
				.ToList();

			return View(new RecipeBrewSessionsViewModel
			{
				RecipeSummary = Mapper.Map(recipeSummary, new RecipeSummaryViewModel()),
				BrewSessions = brewSessions
			});
		}

		#endregion

		#region EXPORT

		/// <summary>
		/// Executes the View for Export
		/// </summary>
		public ActionResult BeerXml(int recipeId)
		{
			var recipe = this.RecipeService.GetRecipeById(recipeId);

			if (recipe == null)
			{
				return this.Issue404();
			}

			var xmlString = this.BeerXmlExporter.Export(recipe);
			var xmlBytes = Encoding.Default.GetBytes(xmlString);

			var disposition = new ContentDisposition
			{
				// for example foo.bak
				FileName = string.Format("{0}-brewgr.xml", StringCleaner.CleanForUrl(recipe.RecipeName)),

				// always prompt the user for downloading, set to true if you want 
				// the browser to try to show the file inline
				Inline = false,
			};
			Response.AppendHeader("Content-Disposition", disposition.ToString());
			return new FileStreamResult(new MemoryStream(xmlBytes), "text/xml");
		}

		#endregion

		#region IMPORT 

		/// <summary>
		/// Executes the View for ImportBeerXml
		/// </summary>
		[ForceHttps]
		public ViewResult ImportBeerXmlDialog()
		{
			return View();
		}

		/// <summary>
		/// Executes the Http Post View for ImportBeerXml
		/// </summary>
		[HttpPost]
		[ForceHttps]
		public ActionResult ImportBeerXmlDialog(HttpPostedFileBase beerXmlFile)
		{
			this.AppendMessage(new InfoMessage { Text = "The recipe has been imported below. Please review before saving." });

			var reader = new StreamReader(beerXmlFile.InputStream);
			var xmlContent = reader.ReadToEnd();

			var recipe = this.BeerXmlImporter.Import(xmlContent);

			// Import Failed
			if (recipe == null)
			{
				this.ForwardMessage(new ErrorMessage { Text = "Import Failed. There seems to be a problem with the Beer Xml file." });
				return RedirectToAction("homebrew-recipe-calculator");
			}

			ViewBag.RecipeCreationOptions = this.RecipeService.GetRecipeCreationOptions();

			return View("NewRecipe", Mapper.Map(recipe, new RecipeViewModel()));
		}

		#endregion

		#region SPECIAL PAGES

		[Route("pliny-the-elder-clone-recipes")]
		public ActionResult PlinyTheElderClones()
		{
			var recipes = this.RecipeService.GetPopularRecipeClones("pliny");
			return this.View(recipes);
		}

		#endregion
	}
}