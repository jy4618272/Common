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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CacheManager()
        {
            DefaultTimeout = new TimeSpan(23, 30, 0);
        }

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
            set { this.Put(key, value); }
        }

        /// <summary>
        /// Gets the instance of the cache object for the current application.
        /// </summary>
        public virtual object Cache
        {
            get { return HttpRuntime.Cache; }
        }

        /// <summary>
        /// Gets or sets the default timeout for the cache. By default it is 23.5 hours.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes the specified item from System.Web.HttpContext object for the current HTTP request.
        /// </summary>
        /// <param name="key">A System.String identifier for the cache item to remove.</param>
        private static void RemoveFromHttpContext(string key)
        {
            HttpContext http = HttpContext.Current;
            if (http != null)
            {
                if (http.Items.Contains(key))
                    http.Items.Remove(key);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        protected virtual object GetFromCache(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// Adds or replaces an object in the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        protected virtual void PutToCache(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Adds or replaces an object in the cache. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="timeout">The amount of time that the object should reside in the cache before expiration.</param>
        protected virtual void PutToCache(string key, object value, TimeSpan timeout)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(timeout), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">A System.String identifier for the cache item to remove.</param>
        protected virtual void RemoveFromCache(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds or replaces an object in the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        public void Put(string key, object value)
        {
            this.PutToCache(key, value);
            RemoveFromHttpContext(key);
        }

        /// <summary>
        /// Adds or replaces an object in the cache. Specifies the timeout value of the cached object.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="timeout">The amount of time that the object should reside in the cache before expiration.</param>
        public void Put(string key, object value, TimeSpan timeout)
        {
            this.PutToCache(key, value, timeout);
            RemoveFromHttpContext(key);
        }

        /// <summary>
        /// Adds or replaces an object in the cache with default timeout.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        public void PutWithDefaultTimeout(string key, object value)
        {
            this.Put(key, value, this.DefaultTimeout);
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public object Get(string key)
        {
            return this.GetFromCache(key);
        }

        /// <summary>
        /// Retrieves the specified item from System.Web.HttpContext object for the current HTTP request or from the cache.
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve.</param>
        /// <param name="httpContext">Whether the item is cached in System.Web.HttpContext object and used to get the item.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public object Get(string key, bool httpContext)
        {
            object value = null;

            if (httpContext)
            {
                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    if (http.Items.Contains(key))
                        value = http.Items[key];
                    else
                    {
                        value = this.Get(key);
                        if (value != null)
                            http.Items[key] = value;
                    }
                }
                else
                    value = this.GetFromCache(key);
            }
            else
                value = this.GetFromCache(key);

            return value;
        }

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key">A System.String identifier for the cache item to remove.</param>
        public void Remove(string key)
        {
            this.RemoveFromCache(key);
            RemoveFromHttpContext(key);
        }

        #endregion
    }
}
