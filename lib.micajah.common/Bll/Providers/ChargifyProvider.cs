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
            ChargifyAccountRetrieverSection config = ConfigurationManager.GetSection("chargify") as ChargifyAccountRetrieverSection;

            if (config == null) return null;

            // new instance
            ChargifyConnect _chargify = new ChargifyConnect();

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

        public static ISubscription GetCustomerSubscription(ChargifyConnect chargify, Guid OrganizationId, Guid InstanceId)
        {
            ICustomer _cust = chargify.LoadCustomer(OrganizationId.ToString() + "," + InstanceId.ToString());

            if (_cust == null) return null;

            return GetCustomerSubscription(chargify, _cust.ChargifyID);
        }

        public static bool DeleteCustomerSubscription(ChargifyConnect chargify, Guid OrganizationId, Guid InstanceId)
        {
            ISubscription subscr = GetCustomerSubscription(chargify, OrganizationId, InstanceId);
            if (subscr==null) return false;
            return chargify.DeleteSubscription(subscr.SubscriptionID, "Cancel Account Registration");
        }

        public static ISubscription GetCustomerSubscription(ChargifyConnect chargify, int chargifyCustomerId)
        {           
            IDictionary<int, ISubscription> _subscrList = chargify.GetSubscriptionListForCustomer(chargifyCustomerId);
            
            if (_subscrList.Count<= 0) return null;

            foreach (KeyValuePair<int, ISubscription> kvp in _subscrList) return kvp.Value;

            return null;
        }

        public static void UpdateSubscriptionAllocations(ChargifyConnect chargify, int SubscriptionId, Instance inst)
        {
            decimal _TotalSum = 0;

            SettingCollection CounterSettings = SettingProvider.GetCounterSettings(inst.OrganizationId, inst.InstanceId);

            foreach (Setting setting in CounterSettings)
            {
                if (setting.Paid)
                {
                    bool isChecked;
                    if (!Boolean.TryParse(setting.Value, out isChecked))
                    {
                        if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
                    }
                    if (isChecked) _TotalSum += setting.Price;
                    int _cid = 0;
                    if (SubscriptionId == 0 || string.IsNullOrEmpty(setting.ExternalId) || !int.TryParse(setting.ExternalId, out _cid)) continue;
                    chargify.UpdateComponentAllocationForSubscription(SubscriptionId, _cid, isChecked ? 1 : 0);
                }
                else
                {
                    int usageCount;
                    int.TryParse(setting.Value, out usageCount);
                    int _paidQty = usageCount - setting.UsageCountLimit;
                    decimal _priceMonth = _paidQty > 0 ? _paidQty * setting.Price : 0;
                    if (_priceMonth > 0) _TotalSum += _priceMonth;
                    int _cid = 0;
                    if (SubscriptionId == 0 || string.IsNullOrEmpty(setting.ExternalId) || !int.TryParse(setting.ExternalId, out _cid)) continue;
                    chargify.UpdateComponentAllocationForSubscription(SubscriptionId, _cid, usageCount < 0 ? 0 : usageCount);
                }
            }

            if (_TotalSum>0 && inst.BillingPlan!=BillingPlan.Paid) InstanceProvider.UpdateInstance(inst, BillingPlan.Paid);
            else if (_TotalSum==0 && inst.BillingPlan!=BillingPlan.Free) InstanceProvider.UpdateInstance(inst, BillingPlan.Free);
        }
    }
}
