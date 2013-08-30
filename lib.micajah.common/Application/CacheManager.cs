using System;
using System.Web;
using System.Web.Caching;

namespace Micajah.Common.Application
{
    /// <summary>
    /// Implements the cache manager.
    /// </summary>
    public class CacheManager
    {
        #region Members

        private static CacheManager s_Current;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current instance of the cache manager.
        /// </summary>
        public static CacheManager Current
        {
            get
            {
                if (s_Current == null)
                    s_Current = new CacheManager();
                return s_Current;
            }
            set
            {
                s_Current = value;
            }
        }

        /// <summary>
        /// Gets or sets the cache item at the specified key.
        /// </summary>
        /// <param name="key">A System.String object that represents the key for the cache item.</param>
        /// <returns>The specified cache item.</returns>
        public virtual object this[string key]
        {
            get { return this.Get(key); }
            set { this.Add(key, value); }
        }

        /// <summary>
        /// Gets the instance of the cache object for the current application.
        /// </summary>
        public virtual object Cache
        {
            get { return HttpRuntime.Cache; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified item to the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        public virtual void Add(string key, object value)
        {
            this.Add(key, value, TimeSpan.MaxValue);
        }

        /// <summary>
        /// Adds the specified item to the cache with dependencies, expiration and priority policies, and a delegate you can use to notify your application when the inserted item is removed from the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="timeout">The interval between the time the object was added and the time at which that object expires.</param>
        public virtual void Add(string key, object value, TimeSpan timeout)
        {
            HttpRuntime.Cache.Add(key, value, null, ((timeout == TimeSpan.MaxValue) ? System.Web.Caching.Cache.NoAbsoluteExpiration : DateTime.UtcNow.Add(timeout)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Adds the specified item to the cache with default timeout (23.5 hours).
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        public virtual void AddWithDefaultExpiration(string key, object value)
        {
            this.Add(key, value, new TimeSpan(23, 5, 0));
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public virtual object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        ///  Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">A System.String identifier for the cache item to remove.</param>
        /// <returns>The item removed from the Cache. If the value in the key parameter is not found, returns null.</returns>
        public virtual void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        #endregion
    }
}
