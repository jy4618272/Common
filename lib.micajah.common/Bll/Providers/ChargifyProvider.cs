using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ChargifyNET;
using ChargifyNET.Configuration;

namespace Micajah.Common.Bll.Providers
{
    public class ChargifyProvider
    {
        public static ChargifyConnect CreateChargify()
        {
            // new instance
            ChargifyConnect _chargify = new ChargifyConnect();

            ChargifyAccountRetrieverSection config = ConfigurationManager.GetSection("chargify") as ChargifyAccountRetrieverSection;
            ChargifyAccountElement accountInfo = config.GetDefaultOrFirst();
            _chargify.apiKey = accountInfo.ApiKey;
            _chargify.Password = accountInfo.ApiPassword;
            _chargify.URL = accountInfo.Site;
            _chargify.SharedKey = accountInfo.SharedKey;
            _chargify.UseJSON = config.UseJSON;

            return _chargify;
        }

        public static string GetProductHandle()
        {
            ChargifyAccountRetrieverSection config = ConfigurationManager.GetSection("chargify") as ChargifyAccountRetrieverSection;
            ChargifyAccountElement accountInfo = config.GetDefaultOrFirst();
            return accountInfo.ProductHandle;
        }

        public static ISubscription GetCustomerSubscription(ChargifyConnect chargify, Guid OrganizationId)
        {
            ICustomer _cust = chargify.LoadCustomer(OrganizationId.ToString());
            
            if (_cust == null) return null;
            
            IDictionary<int, ISubscription> _subscrList = chargify.GetSubscriptionListForCustomer(_cust.ChargifyID);
            
            if (_subscrList.Count<= 0) return null;

            foreach (KeyValuePair<int, ISubscription> kvp in _subscrList) return kvp.Value;

            return null;
        }

        public static void UpdateSubscriptionAllocations(ChargifyConnect chargify, int SubscriptionId, Guid OrganizationId)
        {
            SettingCollection PaidSettings = SettingProvider.GetPaidSettings(OrganizationId);
            foreach (Setting setting in PaidSettings)
            {
                if (string.IsNullOrEmpty(setting.ExternalId)) continue;

                int _cid = 0;
                bool _checked = false;
                if (!int.TryParse(setting.ExternalId, out _cid) || _cid == 0 || setting.DefaultValue=="-1" || !bool.TryParse(setting.Value, out _checked)) continue;
                chargify.UpdateComponentAllocationForSubscription(SubscriptionId, _cid, _checked ? 1 : 0);
            }

            SettingCollection CounterSettings = SettingProvider.GetCounterSettings(OrganizationId);

            foreach (Setting setting in CounterSettings)
            {
                if (string.IsNullOrEmpty(setting.ExternalId)) continue;

                int _cid = 0;
                string _val = setting.Value;
                int _count = 0;

                if (!int.TryParse(setting.ExternalId, out _cid) || _cid == 0 || string.IsNullOrEmpty(_val) || !int.TryParse(_val, out _count)) continue;

                chargify.UpdateComponentAllocationForSubscription(SubscriptionId, _cid, _count);
            }
        }

    }
}
