using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brewgr.Web.Controllers
{
    [RoutePrefix("")]
	public class ErrorController : BrewgrController
	{
		[Route("404")]
		public ViewResult NotFound()
		{
			return View();
		}

		[Route("500")]
		public ViewResult Error()
		{
			return View();
		}

		/// <summary>
		/// Executes the View for ErrorTest
		/// </summary>
		[Route("500-test")]
		public ViewResult ErrorTest()
		{
			throw new Exception("Error-Test");
		}

		/// <summary>
		/// Executes the View for NotFoundTest
		/// </summary>
		[Route("404-test")]
		public ActionResult NotFoundTest()
		{
			return this.Issue404();
		}

		[Route("403-test")]
		public ViewResult ForbiddenText()
		{
			throw new HttpException(403, "Forbidden Test");
		}
	}
}