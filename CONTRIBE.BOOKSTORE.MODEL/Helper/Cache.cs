#region "File Header"

//----------------------------------------------------------------------------------------------------------------------------------------
//File Name			: Cache.cs
//Purpose			: Contains the static methods for caching values
//Date				: 24-June-2018
//Written By		: Giri
//Modified			: 
//-----------------------------------------------------------------------------------------------------------------------------------------

#endregion

#region "Imports"

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

#endregion

namespace CONTRIBE.BOOKSTORE.MODEL.Helper
{
    /// <summary>
    /// Cache class
    /// </summary>
    public static class Cache
    {
        #region Properties

        public static IEnumerable<string> CacheItems
        {
            get
            {
                var cache = MemoryCache.Default;
                return cache.Select(i => i.Key);
            }
        }

        public static void Clear()
        {
            var cache = MemoryCache.Default;
            cache.Trim(100);
            var cacheItems = cache.ToList();

            foreach (var item in cacheItems)
            {
                cache.Remove(item.Key);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method will generate a cache key based on the parameters and retrieve the value from the cache accordingly.
        /// If the key isn't present, the worker method will be called to fetch the data and then the data will be stored in cache.
        /// </summary>
        /// <typeparam name="T">Return type of the worker function</typeparam>
        /// <param name="getData">Worker function that will be used if the required data doesn't exist in the cache already</param>
        /// <param name="callingMethod">calling method name</param>
        /// <param name="timeoutMinutes">timeout minutes</param>
        /// <param name="parameters">Array of values that will be used to generate a unique key into the cache</param>
        /// <returns>The requested data from either the cache, if it was present, or the worker function otherwise</returns>
        public static T GetCacheValue<T>(Func<T> getData, MethodBase callingMethod, double timeoutMinutes, params object[] parameters)
        {
            T result;
            ObjectCache cache = MemoryCache.Default;

            string key = GetCacheKey(callingMethod, parameters);

            var cacheValue = cache[key];

            if (cacheValue != null)
            {
                return (T)cacheValue;
            }

            result = getData();

            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(timeoutMinutes)
            };

            bool shouldCache = true;

            // Do not cache nulls or empty lists
            if (result == null || (result is IEnumerable && !((IEnumerable)result).GetEnumerator().MoveNext()))
            {
                shouldCache = false;
            }

            if (shouldCache)
            {
                cache.Set(key, result, policy);
            }

            return result;
        }

        public static T GetCacheValue<T>(Func<T> getData, MethodBase callingMethod, params object[] parameters)
        {
            return GetCacheValue(getData, callingMethod, 5, parameters);
        }

        /// <summary>
        /// This method will generate a cache key based on the parameters and clear the value from the cache accordingly.
        /// If the key isn't present, no operation done.
        /// </summary>
        /// <param name="keyPart">key part name</param>
        /// <param name="parameters">Array of values that will be used to generate a unique key into the cache</param>
        public static void ClearCacheValue(string keyPart, params object[] parameters)
        {
            ObjectCache cache = MemoryCache.Default;

            StringBuilder sb = new StringBuilder(keyPart);
            sb.Append("_");

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    sb.Append(string.Format("_{0}", null == param ? "" : param.ToString()));
                }
            }

            string key = sb.ToString();

            var cacheValueList = cache.Where(k => k.Key.StartsWith(key));

            if (cacheValueList.IsNotEmpty())
            {
                cacheValueList.ForEach(c =>
                {
                    cache.Remove(c.Key);
                });
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method will generate a cache key for the method GetCacheValue.  It should only be called
        /// by that method because there's a ordinal dependency within the stack on when this method is called. 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static string GetCacheKey(MethodBase callingMethod, params object[] parameters)
        {
            return MakeKeyFromMethodBaseAndParameters(callingMethod, parameters);
        }

        /// <summary>
        /// Takes the method name and an array of parameters and generates a key to be used for cache indexing.
        /// </summary>
        /// <param name="callingMethod"></param>
        /// <param name="callingParameters"></param>
        /// <returns></returns>
        private static string MakeKeyFromMethodBaseAndParameters(MethodBase callingMethod, params object[] callingParameters)
        {
            StringBuilder sb = new StringBuilder(callingMethod.DeclaringType.FullName);
            sb.Append("_");
            sb.Append(callingMethod.Name);
            sb.Append("_");

            if (callingParameters != null)
            {
                foreach (var param in callingParameters)
                {
                    sb.Append(string.Format("_{0}", null == param ? "" : param.ToString()));
                }
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}