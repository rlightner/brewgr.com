using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Models
{
    public class CalculatorHydrometerTempViewModel
    {

        /// <summary>
        /// Gets or sets the SpecificGravity
        /// </summary>
        public double? SpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the SpecificGravityTemp
        /// </summary>
        public double? SpecificGravityTemp { get; set; }

        /// <summary>
        /// Gets or sets the SpecificGravityTempUnit
        /// </summary>
        public TemperatureUnit? SpecificGravityTempUnit { get; set; }

        /// <summary>
        /// Gets or sets the SpecificGravityTemp
        /// </summary>
        public double? TargetSpecificGravityTemp { get; set; }

        /// <summary>
        /// Gets or sets the SpecificGravityTempUnit
        /// </summary>
        public TemperatureUnit? TargetSpecificGravityTempUnit { get; set; }

    }
}