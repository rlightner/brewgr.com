using System;
using System.Web;
using System.Web.Caching;

namespace ctorx.Core.Data
{
	public class HttpContextCachingService : ICachingService
	{
		HttpContext _Context;

		/// <summary>
		/// Gets the Http Context Cache
		/// </summary>
		HttpContext Context
		{
			get
			{
				if (this._Context == null)
				{
					this._Context = HttpContext.Current;
				}

				return this._Context;
			}
		}

		/// <summary>
		/// Determines if data is cached under a given key
		/// </summary>
		public bool IsInCache(string key)
		{
			return this.Context.Cache[key] != null;
		}

		/// <summary>
		/// Gets a value from cache if it exists, otherwise invokes the func setting its result in Cache
		/// </summary>
		public TValue Get<TValue>(string key, ICacheExpirationSettings cacheExpirationSettings = null, Func<TValue> retrieveFunc = null) where TValue : class
		{
			if (this.IsInCache(key))
			{
				return this.Context.Cache.Get(key) as TValue;
			}

			if(retrieveFunc == null)
			{
				return null;
			}

			// Invoke Func to Get Data
			var value = retrieveFunc.Invoke();

			if (value != null)
			{
				this.Set(key, value, cacheExpirationSettings);
			}

			return value;
		}

		/// <summary>
		/// Sets a value in cache using the provided settings
		/// </summary>
		public void Set<TValue>(string key, TValue value, ICacheExpirationSettings cacheExpirationSettings = null) where TValue : class
		{
			if (this.IsInCache(key))
			{
				this.Remove(key);
			}

			if(cacheExpirationSettings == null)
			{
				cacheExpirationSettings = AbsoluteCacheExpirationSettings.Make(5);
			}

			if (value != null)
			{
				this.Context.Cache.Insert(key, value, null, cacheExpirationSettings.AbsoluteExpiration, cacheExpirationSettings.SlidingExpiration, cacheExpirationSettings.Priority, null);
			}
		}

		/// <summary>
		/// Updates a value in the Cache
		/// </summary>
		public void Update<TValue>(string key, TValue value) where TValue : class
		{
			this.Context.Cache[key] = value;
		}

		/// <summary>
		/// Removes a value from the Cache
		/// </summary>
		public void Remove(string key)
		{
			this.Context.Cache.Remove(key);
		}
	}
}