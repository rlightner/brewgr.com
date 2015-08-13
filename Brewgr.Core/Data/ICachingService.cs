using System;

namespace ctorx.Core.Data
{
	public interface ICachingService
	{
		/// <summary>
		/// Determines if data is cached under a given key
		/// </summary>
		bool IsInCache(string key);

		/// <summary>
		/// Gets a value from cache if it exists, otherwise invokes the func setting its result in Cache
		/// </summary>
		TValue Get<TValue>(string key, ICacheExpirationSettings cacheExpirationSettings = null, Func<TValue> retrieveFunc = null) where TValue : class;
		/// <summary>
		/// Sets a value in cache using the provided settings
		/// </summary>
		void Set<TValue>(string key, TValue value, ICacheExpirationSettings cacheExpirationSettings) where TValue : class;

		/// <summary>
		/// Updates a value in the Cache
		/// </summary>
		void Update<TValue>(string key, TValue value) where TValue : class;

		/// <summary>
		/// Removes a value from the Cache
		/// </summary>
		void Remove(string key);
	}
}