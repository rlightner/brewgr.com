using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using StackExchange.Exceptional;
using StackExchange.Exceptional.Stores;
using ctorx.Core.Ninject;
using Brewgr.Web.Controllers;
using Brewgr.Web.Core.Service;
using Ninject;
using FluentValidation.Mvc;

namespace Brewgr.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801


	public class MvcApplication : System.Web.HttpApplication
	{
		static readonly Regex WwwRegex = new Regex("(http|https)://www\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// Fires on Application Start
		/// </summary>
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			MvcHandler.DisableMvcResponseHeader = true;

			FluentValidationModelValidatorProvider.Configure(provider =>
			{
				provider.ValidatorFactory = new ViewModelValidatorFactory();
			});

			// Disable Code First Migrations
			Database.SetInitializer<BrewgrContext>(null);

			// Setup Exception Error Store
			ErrorStore.Setup("Brewgr.com", new SQLErrorStore(Environment.GetEnvironmentVariable("Brewgr_ConnectionString")));
		}

		/// <summary>
		/// Fires on Begin Request
		/// </summary>
		protected void Application_BeginRequest()
		{
			var context = HttpContext.Current;
			Uri url = context.Request.Url;

			if (WwwRegex.IsMatch(url.ToString()))
			{
				String newUrl = WwwRegex.Replace(url.ToString(), String.Format("{0}://", url.Scheme));
				context.Response.RedirectPermanent(newUrl);
			}

			// Handle Partner Detection
			if (!string.IsNullOrWhiteSpace(context.Request.QueryString["pid"]))
			{
				var kernel = KernelPersister.Get();
				var partnerService = kernel.Get<IPartnerService>();
				var partnerIdResolver = kernel.Get<IPartnerIdResolver>();

				var token = context.Request.QueryString["pid"];
				var partnerId = partnerService.GetPartnerIdFromToken(token);

				if (partnerId != null)
				{
					// Persist the Partner Id
					partnerIdResolver.Persist(partnerId.Value);

					// Redirect (to remove the url token)
					context.Response.RedirectPermanent(url.ToString().ToLower().Replace("pid=" + token, "").TrimEnd(new[] { '?', '#' }), true);
				}
			}
		}

		/// <summary>
		/// Fires on Application Error
		/// </summary>
		protected void Application_Error(object sender, EventArgs e)
		{
			var environment = ConfigurationManager.AppSettings["Environment"];

			if (environment != "dev")
			{
				var ex = Server.GetLastError().GetBaseException();

				Context.Response.Clear();
				Server.ClearError();

				var routeData = new RouteData();
				routeData.Values.Add("controller", "Error");
				routeData.Values.Add("action", "500");

				if (ex.GetType() == typeof(HttpException))
				{
					var httpException = (HttpException)ex;
					var code = httpException.GetHttpCode();

					// Is it a 4xx Error
					if (code % 400 < 100)
					{
						routeData.Values["action"] = "404";
					}
				}

				ErrorStore.LogException(ex, this.Context);

				routeData.Values.Add("error", ex);

				IController errorController = new ErrorController();
				errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
			}
		}
	}
}