using Microsoft.ApplicationServer.Caching;
using System;

namespace Micajah.Common.TestSite
{
    public class AzureCacheManager : Micajah.Common.Application.CacheManager
    {
        private DataCacheFactory m_CacheFactory;
        private DataCache m_Cache;

        private DataCacheFactory CacheFactory
        {
            get
            {
                if (m_CacheFactory == null)
                    m_CacheFactory = new DataCacheFactory();
                return m_CacheFactory;
            }
        }

        private DataCache AzureCache
        {
            get
            {
                if (m_Cache == null)
                    m_Cache = this.CacheFactory.GetDefaultCache();
                return m_Cache;
            }
        }

        public override object Cache
        {
            get { return AzureCache; }
        }

        protected override void PutToCache(string key, object value)
        {
            AzureCache.Put(key, value);
        }

        protected override void PutToCache(string key, object value, TimeSpan timeout)
        {
            AzureCache.Put(key, value, timeout);
        }

        protected override object GetFromCache(string key)
        {
            return AzureCache.Get(key);
        }

        protected override void RemoveFromCache(string key)
        {
            AzureCache.Remove(key);
        }
    }
}