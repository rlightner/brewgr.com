using System;
using System.Web.Caching;

namespace ctorx.Core.Data
{
	public class AbsoluteCacheExpirationSettings : ICacheExpirationSettings
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
		private AbsoluteCacheExpirationSettings() { }

		/// <summary>
		/// Makes a new Absolute Cache Expiration Settings
		/// </summary>
		public static ICacheExpirationSettings Make(int minutesToCache, CacheItemPriority cacheItemPriority = CacheItemPriority.Normal)
		{
			return new AbsoluteCacheExpirationSettings
			{
				CacheExpirationType = CacheExpirationType.Absolute,
				AbsoluteExpiration = DateTime.Now.AddMinutes(minutesToCache),
				SlidingExpiration = Cache.NoSlidingExpiration,
				Priority = cacheItemPriority
			};
		}
	}
}