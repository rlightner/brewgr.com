using System;
using System.Web.Caching;

namespace ctorx.Core.Data
{
	public class NeverExpiresCacheExpirationSettings : ICacheExpirationSettings
	{
		/// <summary>
		/// Gets the Cache Expiration Type
		/// </summary>
		public CacheExpirationType CacheExpirationType { get; private set; }

		/// <summary>
		/// Gets or sets the absolute expiration
		/// </summary>
		public DateTime AbsoluteExpiration { get; set; }

		/// <summary>
		/// Gets or sets the sliding expirtation
		/// </summary>
		public TimeSpan SlidingExpiration { get; set; }

		/// <summary>
		/// Gets or sets the Caching Priority
		/// </summary>
		public CacheItemPriority Priority { get; private set; }

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		private NeverExpiresCacheExpirationSettings() { }

		/// <summary>
		/// Makes a NeverExpiresCacheExpirationSettings
		/// </summary>
		public static ICacheExpirationSettings Make(CacheItemPriority cacheItemPriority = CacheItemPriority.Normal)
		{
			return new NeverExpiresCacheExpirationSettings
			{
				CacheExpirationType = CacheExpirationType.Never,
				AbsoluteExpiration = DateTime.UtcNow.AddYears(30),
				SlidingExpiration = Cache.NoSlidingExpiration,
				Priority = cacheItemPriority
			};
		}
	}
}