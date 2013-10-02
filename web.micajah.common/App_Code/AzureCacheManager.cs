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

        public override void Put(string key, object value)
        {
            AzureCache.Put(key, value);
        }

        public override void Put(string key, object value, TimeSpan timeout)
        {
            AzureCache.Put(key, value, timeout);
        }

        public override object Get(string key)
        {
            return AzureCache.Get(key);
        }

        public override void Remove(string key)
        {
            AzureCache.Remove(key);
        }
    }
}