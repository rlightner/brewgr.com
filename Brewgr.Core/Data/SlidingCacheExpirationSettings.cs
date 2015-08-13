using System;
using System.Web.Caching;

namespace ctorx.Core.Data
{
	public class SlidingCacheExpirationSettings : ICacheExpirationSettings
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
		private SlidingCacheExpirationSettings() { }

		/// <summary>
		/// Makes a new SlidingCacheExpirationSettings
		/// </summary>
		public static ICacheExpirationSettings Make(int slidingMinutes, CacheItemPriority cacheItemPriority = CacheItemPriority.AboveNormal)
		{
			return new SlidingCacheExpirationSettings
			{
				CacheExpirationType = CacheExpirationType.Sliding,
				AbsoluteExpiration = Cache.NoAbsoluteExpiration,
				SlidingExpiration = new TimeSpan(0, 0, slidingMinutes, 0),
				Priority = cacheItemPriority
			};
		}
	}
}