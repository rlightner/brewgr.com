using System;
using System.Configuration;
using Brewgr.Web.Email;
using ctorx.Core.Crypto;
using ctorx.Core.Data;
using ctorx.Core.Email;
using ctorx.Core.Identity;
using ctorx.Core.Security;
using Brewgr.Web.Core.Configuration;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using Ninject.Modules;
using Ninject.Web.Common;
using ctorx.Core.Web;
using Brewgr.Web.Controllers;

namespace Brewgr.Web
{
	public class Dependencies : NinjectModule
	{
		public override void Load()
		{
			// Get Environment for Conditional Binding
			var configEnvironment = ConfigurationManager.AppSettings["Environment"];

            // Settings
            this.Bind<IWebSettings>().To<DevWebSettings>().When(_ => configEnvironment == "dev");
            this.Bind<IWebSettings>().To<ProdWebSettings>().When(_ => configEnvironment == "prod");

            // EF/UOW Setup
            this.Bind<IUnitOfWorkFactory<BrewgrContext>>().To<DefaultUnitOfWorkFactory<BrewgrContext>>().InTransientScope();
			this.Bind<IDataContextFactory<BrewgrContext>>().To<DefaultDataContextFactory<BrewgrContext>>().InSingletonScope();
			this.Bind<IDataContextResolver<BrewgrContext>>().To<DefaultDataContextResolver<BrewgrContext>>().InRequestScope();
			this.Bind<IDataContextActivationInfo<BrewgrContext>>().To<BrewgrContextActivationInfo>().InSingletonScope();

			// Crypto
			this.Bind<IHasher>().To<SHA512Hasher>();
			this.Bind<IStringCryptoKeyProvider>().To<BrewgrCryptoKeyProvider>();
			this.Bind<IStringCryptoService>().To<AESStringCryptoService>();

			// Repositories
			this.Bind<IBrewgrRepository>().To<DefaultBrewgrRepository>();
			this.Bind<IBrewgrBlogRepository>().To<DefaultBrewgrBlogRepository>();

			// Services
			this.Bind<IUserLoginService>().To<DefaultUserLoginService>();
			this.Bind<IUserService>().To<DefaultUserService>();
			this.Bind<IAuthenticationService>().To<ASPNETFormsAuthenticationService>();
			this.Bind<IOAuthService>().To<DefaultOAuthService>();
			this.Bind<IRecipeService>().To<DefaultRecipeService>();
			this.Bind<IBeerStyleService>().To<DefaultBeerStyleService>();
			this.Bind<IRecipeDataService>().To<DefaultRecipeDataService>().InRequestScope();
			this.Bind<IMarketingService>().To<DefaultMarketingService>();
			this.Bind<IStaticContentService>().To<DefaultStaticContentService>();
			this.Bind<ISearchService>().To<DefaultSearchService>().WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));
			this.Bind<IAdminService>().To<DefaultAdminService>();
			this.Bind<IAffiliateService>().To<DefaultAffiliateService>();
			this.Bind<IContentService>().To<DefaultContentService>();
			this.Bind<IPartnerService>().To<DefaultPartnerService>();
			this.Bind<ISendToShopService>().To<DefaultSendToShopService>();
			this.Bind<IRecipeSearchService>().To<DefaultRecipeSearchService>();

			// Services with Readonly Repositories
			this.Bind<IUserService>().To<DefaultUserService>().WhenInjectedInto(typeof(RecipeCommentNotification))
				.WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));
			this.Bind<IRecipeService>().To<DefaultRecipeService>().WhenInjectedInto(typeof(RecipeCommentNotification))
				.WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));
			this.Bind<IRecipeService>().To<DefaultRecipeService>().WhenInjectedInto(typeof(BrewSessionCommentNotification))
				.WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));
			this.Bind<IUserService>().To<DefaultUserService>().WhenInjectedInto(typeof(BrewerFollowNotification))
				.WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));
			this.Bind<INotificationService>().To<DefaultNotificationService>()
				.WithConstructorArgument("repository", x => x.Kernel.GetService(typeof (BrewgrReadOnlyRepository)));
            this.Bind<IRecipeService>().To<DefaultRecipeService>().WhenInjectedInto(typeof(DashboardController))
                .WithConstructorArgument("repository", x => x.Kernel.GetService(typeof(BrewgrReadOnlyRepository)));

			// Miscellaneous
			this.Bind<ICachingService>().To<HttpContextCachingService>();
			this.Bind<IUserResolver>().To<DefaultUserResolver>();
			this.Bind<IUserHostAddressResolver>().To<HttpRequestUserHostAddressResolver>();
			this.Bind<ISmtpConfiguration>().To<BrewgrSmtpConfiguration>();
			this.Bind<IEmailSender>().To<SmtpEmailSender>();
			this.Bind<IEmailMessageFactory>().To<DefaultEmailMessageFactory>();
			this.Bind<ISeoSitemap>().To<BrewgrSeoSitemap>();
			this.Bind<INotificationFactory>().To<DefaultNotificationFactory>();
			this.Bind<IRecipeUnitConverter>().To<DefaultRecipeUnitConverter>();
			this.Bind<IBeerXmlRecipeExporter>().To<DefaultBeerXmlRecipeExporter>();
			this.Bind<IBeerXmlRecipeImporter>().To<DefaultBeerXmlRecipeImporter>();
			this.Bind<IIngredientCategorizer>().To<DefaultIngredientCategorizer>();
			this.Bind<IPartnerIdResolver>().To<DefaultPartnerIdResolver>();
			this.Bind<ISendToShopEmailMessageFactory>().To<DefaultSendToShopEmailMessageFactory>();

			// Third Party Services
			this.Bind<IFacebookConnectSettings>().To<DefaultFacebookConnectSettings>();
			this.Bind<IFacebookConnectService>().To<DefaultFacebookService>();
		    this.Bind<IBrewgrBlogConnection>().To<DefaultBrewgrBlogConnection>();
		}
	}
}