using System;
using System.Web.Caching;

namespace ctorx.Core.Data
{
	public interface ICacheExpirationSettings
	{
		/// <summary>
		/// Gets the Cache Expiration Type
		/// </summary>
		CacheExpirationType CacheExpirationType { get; }

		/// <summary>
		/// Gets or sets the absolute expiration
		/// </summary>
		DateTime AbsoluteExpiration { get; set; }

		/// <summary>
		/// Gets or sets the sliding expirtation
		/// </summary>
		TimeSpan SlidingExpiration { get; set; }

		/// <summary>
		/// Gets or sets the Caching Priority
		/// </summary>
		CacheItemPriority Priority { get; }
	}
}