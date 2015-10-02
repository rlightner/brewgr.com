using System.Collections.Generic;
using System.Web.Mvc;
using Brewgr.Web.Models;
using ctorx.Core.Web;

namespace Brewgr.Web.Controllers
{
    [RoutePrefix("calculators")]
    public class CalculatorsController : BrewgrController
    {
        /// <summary>
        /// Executes the View for Calculators
        /// </summary>
        [Route("")]
        public ViewResult Calculators()
        {
            return View();
        }

        /// <summary>
        /// Executes the View for CalculatorsHydrometerTemp
        /// </summary>
        [Route("hydrometer-correction")]
        public ViewResult CalculatorsHydrometerTemp()
        {
            // Temperature Units
            ViewBag.TempUnitOptions = new List<OptionViewModel>
            {
                new OptionViewModel { Value = "Fahrenheit", DisplayText = "F" },
                new OptionViewModel { Value = "Celcius", DisplayText = "C" }
            };

            // Mash Time
            ViewBag.MashMinutesOptions = new List<OptionViewModel> { new OptionViewModel { Value = null, DisplayText = null } };
            for (var i = 20; i <= 120; i += 5)
            {
                ViewBag.MashMinutesOptions.Add(new OptionViewModel { Value = i.ToString(), DisplayText = i.ToString() + " Minute(s)" });
            }

            // Gravity Options
            ViewBag.GravityOptions = new List<OptionViewModel> { new OptionViewModel { Value = null, DisplayText = null } };
            for (var i = 1.000; i <= 1.150; i += .001)
            {
                ViewBag.GravityOptions.Add(new OptionViewModel { Value = i.ToString(), DisplayText = i.ToString("0.000") });
            }

            return View(new CalculatorHydrometerTempViewModel { SpecificGravity = 1.04999999999999, TargetSpecificGravityTemp = 60 });
        }

        /// <summary>
        /// Executes the View for CalculatorsMashSpargeWater
        /// </summary>
        [Route("mash-sparge-water-infusion")]
        public ViewResult CalculatorsMashSpargeWater()
        {
            return this.View(new BrewSessionViewModel());
        }
    }
}