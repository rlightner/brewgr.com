using System;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Models
{
	public class RecipeOtherViewModel : RecipeIngredientViewModel
	{
		/// <summary>
		/// Gets or sets the Amount
		/// </summary>
		public string Amt { get; set; }

		/// <summary>
		/// Gets or sets the UnitOfMeasure
		/// </summary>
		public string Unit { get; set; }

		/// <summary>
		/// Gets or sets the Use
		/// </summary>
		public string Use { get; set; }		
	}
}