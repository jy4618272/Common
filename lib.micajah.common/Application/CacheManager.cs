using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Micajah.Common.Application
{
    /// <summary>
    /// Implements the cache manager.
    /// </summary>
    public class CacheManager : IEnumerable
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified item to the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        public void Add(string key, object value)
        {
            this.Add(key, value, null, DateTime.UtcNow.AddHours(23.5), TimeSpan.Zero, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Adds the specified item to the cache with dependencies, expiration and priority policies, and a delegate you can use to notify your application when the inserted item is removed from the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="dependencies">The file or cache key dependencies for the item. When any dependency changes, the object becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains null.</param>
        /// <param name="absoluteExpiration">The time at which the added object expires and is removed from the cache.</param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed and the time at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from the cache 20 minutes after it is last accessed.</param>
        /// <param name="priority">The relative cost of the object, as expressed by the System.Web.Caching.CacheItemPriority enumeration. The cache uses this value when it evicts objects; objects with a lower cost are removed from the cache before objects with a higher cost.</param>
        /// <param name="onRemoveCallback">A delegate that, if provided, is called when an object is removed from the cache. You can use this to notify applications when their objects are deleted from the cache.</param>
        public virtual void Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
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
        /// Retrieves an enumerator used to iterate through the key settings and their values contained in the cache.
        /// </summary>
        /// <returns>An enumerator to iterate through the cache.</returns>
        public virtual IDictionaryEnumerator GetEnumerator()
        {
            return HttpRuntime.Cache.GetEnumerator();
        }

        /// <summary>
        /// Retrieves an enumerator used to iterate through the key settings and their values contained in the cache.
        /// </summary>
        /// <returns>An enumerator to iterate through the cache.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
