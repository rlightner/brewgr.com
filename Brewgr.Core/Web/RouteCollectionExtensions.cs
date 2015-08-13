using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using ctorx.Core.Collections;

namespace ctorx.Core.Web
{
	public static class RouteCollectionExtensions
	{
		private static readonly Type[] ActionReturnTypes = new[]
		{
		    typeof (ActionResult),
		    typeof (ViewResult),
		    typeof (PartialViewResult),
		    typeof (RedirectResult),
		    typeof (RedirectToRouteResult),
		    typeof (ContentResult),
		    typeof (JsonResult),
		    typeof (JavaScriptResult),
		    typeof (FileResult),
			typeof (FileContentResult),
		    typeof (EmptyResult)
		};

		/// <summary>
		/// Infers and maps routes through refelection of the Assembly
		/// </summary>
		/// <param name="targetAssembly">The assembly to scan for controllers</param>
		/// <param name="controllerSearchType">Specifies the Type that is used when searching for Controllers </param>
		/// <param name="controllerNameMappings">Specifies controller name mappings to apply to the inspection</param>
		/// <param name="ignoredControllers">Controller names to ignore while inferring</param>
		public static void MapInferredRoutes(this RouteCollection routeCollection, Assembly targetAssembly = null,
			Type controllerSearchType = null, IEnumerable<ControllerNameMap> controllerNameMappings = null, IEnumerable<string> ignoredControllers = null)
		{
			// Track mapped routes
			var mappedRoutes = new List<string>();

			// If not provided, set controller type to System.Web.Mvc.Controller
			if (controllerSearchType == null)
			{
				controllerSearchType = typeof (Controller);
			}

			// Get Executing Assembly
			if (targetAssembly == null)
			{
				targetAssembly = Assembly.GetCallingAssembly();
			}

			// Get all Controller Class Types
			var controllers = GetControllersFromAssemblyByType(targetAssembly, controllerSearchType);

			foreach (var controllerType in controllers)
			{
				// Get controller name
				var controllerName = DetermineControllerName(controllerType);

				// Get the Controller Name used for the URL
				var controllerUrlPart = DetermineControllerUrlPart(controllerNameMappings, controllerName);

				// Skip ignored controllers
				if (ignoredControllers != null && ignoredControllers.Select(x => x.ToLower()).Where(x => x == controllerUrlPart).Any())
				{
					continue;
				}

				// Get all MVC Action Methods
				var methods = controllerType.GetMethods().Where(x => ActionReturnTypes.Contains(x.ReturnType));

				// Create the Routes for the Methods
				foreach (var actionMethod in methods)
				{
					var actionUrlPart = DetermineActionUrlPart(actionMethod);

					var routeName = string.Concat(controllerUrlPart, "-", actionUrlPart);

					// Check if Route already exists
					// ( this happens with HttpPosts)
					if (mappedRoutes.Contains(routeName))
					{
						continue;
					}

					var parameters = actionMethod.GetParameters().Select(x => x.Name.ToLower());

					// Checkfor HttpPost Attribute
					if(actionMethod.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
					{
						parameters = null;
					}

					// Add the Route
					AddRouteToCollection(routeCollection, routeName, controllerName, controllerUrlPart, actionUrlPart, parameters);
					mappedRoutes.Add(routeName);
				}
			}
		}

		/// <summary>
		/// Maps a catch all route to support correct 404
		/// </summary>
		/// <param name="targetController">Controller to target for the 404 View</param>
		/// <param name="targetAction">Action to target for the 404 View</param>
		public static void MapCatchAllFor404(this RouteCollection routeCollection, string targetController, string targetAction)
		{
			routeCollection.MapRoute(
				"404-PageNotFound",
				"{*url}",
				new { controller = targetController, action = targetAction }
			);
		}

		/// <summary>
		/// Maps a default root level route (for entry point of domain)
		/// </summary>
		/// <param name="targetController">Controller to target for the default View</param>
		/// <param name="targetAction">Action to target for the default View</param>
		public static void MapEntryRoute(this RouteCollection routeCollection, string targetController, string targetAction)
		{
			routeCollection.MapRoute(
				"Default-Root-Route",
				"",
				new { controller = targetController, action = targetAction }
			);
		}

		/// <summary>
		/// Gets the controllers from an assembly by type
		/// </summary>
		static IEnumerable<Type> GetControllersFromAssemblyByType(Assembly targetAssembly, Type controllerType)
		{
			return targetAssembly.GetTypes().Where(x => x.BaseType != null && x.BaseType.Equals(controllerType));
		}

		/// <summary>
		/// Determines the Controller Name
		/// </summary>
		static string DetermineControllerName(Type controllerType)
		{
			return controllerType.Name.Substring(0, controllerType.Name.ToLower().LastIndexOf("controller")).ToLower();
		}

		/// <summary>
		/// Determines the URL part for a controller
		/// </summary>
		static string DetermineControllerUrlPart(IEnumerable<ControllerNameMap> controllerNameMappings, string controllerName)
		{
			// Check if an alternate name has been provided via the controller mapping
			if (controllerNameMappings != null && controllerNameMappings.Where(x => x.SourceName.ToLower() == controllerName).Any())
			{
				return controllerNameMappings.Where(x => x.SourceName.ToLower() == controllerName).FirstOrDefault().TargetName;
			}

			return controllerName;
		}

		/// <summary>
		/// Determines the URL Part for an Action
		/// </summary>
		static string DetermineActionUrlPart(MethodInfo method)
		{
			var actionUrlPart = method.Name.ToLower();

			var explicitActionName = method.GetCustomAttributes(typeof(ActionNameAttribute), true).FirstOrDefault();
			if (explicitActionName != null)
			{
				actionUrlPart = (explicitActionName as ActionNameAttribute).Name;
			}
			return actionUrlPart;
		}

		/// <summary>
		/// Adds the route to the collection
		/// </summary>
		static void AddRouteToCollection(RouteCollection routeCollection, string routeName, string controllerName, string controllerUrlPart, 
			string actionUrlPart, IEnumerable<string> parameters)
		{
			var routePath = string.Format("{0}{1}{2}",
				controllerUrlPart,
				!string.IsNullOrWhiteSpace(controllerUrlPart) ? "/" : "",
				actionUrlPart);

			// Set the Id Parameters
			if(parameters != null)
			{
				parameters.ForEach(x => routePath += "/{" + x + "}");				
			} 

			routeCollection.MapRoute(
				routeName,
				routePath,
				new { controller = controllerName, action = actionUrlPart }
			);			
		}
	}
}