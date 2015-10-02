using System;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Brewgr.Web.Email;
using ctorx.Core.Crypto;
using ctorx.Core.Data;
using ctorx.Core.Email;
using ctorx.Core.Messaging;
using ctorx.Core.Security;
using Brewgr.Web.Core.Configuration;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using Brewgr.Web.Models;
using ctorx.Core.Web;
using System.Collections.Generic;

namespace Brewgr.Web.Controllers
{
    [RoutePrefix("calculations")]
    public class CalculationsController : BrewgrController
	{
		/// <summary>
        /// Executes the View for Calculations
		/// </summary>
		[Route("")]
		public ViewResult Calculations()
		{
			return View();
		}

        /// <summary>
        /// Executes the View for Calculations/OriginalGravity
        /// </summary>
        [Route("original-gravity")]
        public ViewResult CalculationsOriginalGravity()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for Calculations/FinalGravity
        /// </summary>
        [Route("final-gravity")]
        public ViewResult CalculationsFinalGravity()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for Calculations/SRM
        /// </summary>
        [Route("srm-beer-color")]
        public ViewResult CalculationsSRM()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for Calculations/IBU
        /// </summary>
        [Route("ibu-hop-bitterness")]
        public ViewResult CalculationsIBU()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for Calculations/Alcohol
        /// </summary>
        [Route("alcohol-content")]
        public ViewResult CalculationsAlcohol()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for Calculations/Calories
        /// </summary>
        [Route("calories")]
        public ViewResult CalculationsCalories()
        {
            return View();
        }
    }
}
