using System;
using System.Collections.Generic;
using System.Linq;

namespace Brewgr.Web.Core.Model
{
	public class Affiliate
	{
		/// <summary>
		/// Gets or sets the AffiliateId
		/// </summary>
		public int AffiliateId { get; set; }

		/// <summary>
		/// Gets or sets the AffiliateName
		/// </summary>
		public string AffiliateName { get; set; }

		/// <summary>
		/// Gets or sets the AffiliateUrl
		/// </summary>
		public string AffiliateUrl { get; set; }

		/// <summary>
		/// Gets or sets the DateCreated
		/// </summary>
		public DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the AffiliateProducts
		/// </summary>
		public IList<AffiliateProduct> AffiliateProducts { get; set; }
	}
}