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

        public static bool RegisterCreditCard(ChargifyConnect chargify, Guid OrgId, Guid InstId, string OrgName, string InstName, string UserEmail, string UserFirstName, string UserLastName, string CardNumber, string CardExprMonth, string CardExprYear, int GraceDays, out string errorMessage)
        {
            errorMessage = string.Empty;
            string _CustSystemId = OrgId.ToString() + "," + InstId.ToString();

            ICustomer _cust = chargify.LoadCustomer(_CustSystemId);
            ISubscription _subscr = null;

            try
            {
                if (_cust == null)
                {
                    errorMessage = "Can't create Chargify Customer!";
                    _cust = new Customer();
                    _cust.SystemID = _CustSystemId;
                    _cust.Organization = OrgName + " " + InstName;
                    _cust.Email = UserEmail;
                    _cust.FirstName = UserFirstName;
                    _cust.LastName = UserLastName;
                    _cust = chargify.CreateCustomer(_cust);
                }
                else if (_cust.Organization != OrgName + " " + InstName || _cust.Email != UserEmail || _cust.FirstName != UserFirstName || _cust.LastName != UserLastName)
                {
                    errorMessage = "Can't update Chargify Customer!";
                    _cust.Organization = OrgName + " " + InstName;
                    _cust.Email = UserEmail;
                    _cust.FirstName = UserFirstName;
                    _cust.LastName = UserLastName;
                    _cust = chargify.UpdateCustomer(_cust);
                    errorMessage = "Can't get Chargify Customer Substriction!";
                    _subscr = ChargifyProvider.GetCustomerSubscription(chargify, _cust.ChargifyID);
                }
                else
                {
                    errorMessage = "Can't get Chargify Customer Substriction!";
                    _subscr = ChargifyProvider.GetCustomerSubscription(chargify, _cust.ChargifyID);
                }
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode != 422) errorMessage += " " + cex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage += " " + ex.Message;
                return false;
            }

            errorMessage = string.Empty;

            if (CardNumber.Contains("XXXX"))
            {
                if (_subscr != null && _subscr.CreditCard != null && _subscr.State != SubscriptionState.Active)
                {
                    try
                    {
                        chargify.ReactivateSubscription(_subscr.SubscriptionID);
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Can't reactivate Customer Subscription! " + ex.Message;
                        return false;
                    }
                    return true;
                }
                errorMessage = "Invalid Credit Card Information!";
                return false;
            }

            CreditCardAttributes _ccattr = new CreditCardAttributes(_cust.FirstName, _cust.LastName, CardNumber, 2000 + int.Parse(CardExprYear), int.Parse(CardExprMonth), string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            try
            {
                if (_subscr == null)
                {
                    errorMessage = "Can't create Chargify Subscription!";
                    _subscr = chargify.CreateSubscription(ChargifyProvider.GetProductHandle(), _cust.ChargifyID, _ccattr);
                    chargify.UpdateBillingDateForSubscription(_subscr.SubscriptionID, DateTime.UtcNow.AddDays(GraceDays));
                }
                else
                {
                    errorMessage = "Can't update Chargify Subscription!";
                    chargify.UpdateSubscriptionCreditCard(_subscr, _ccattr);
                    if (_subscr.State != SubscriptionState.Active) chargify.ReactivateSubscription(_subscr.SubscriptionID);
                }
            }
            catch (ChargifyException cex)
            {
                if ((int)cex.StatusCode == 422) errorMessage += " Invalid Credit Card Information!";
                else errorMessage += " " + cex.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage += " " + ex.Message;
                return false;
            }

            errorMessage = string.Empty;
            return true;

        }

        public static void UpdateSubscriptionAllocations(ChargifyConnect chargify, int SubscriptionId, Instance inst, SettingCollection modifiedSettings, SettingCollection paidSettings)
        {
            decimal monthlySum = 0;

            foreach (Setting setting in paidSettings)
            {
                if (setting.ShortName == "PhoneSupport") continue;
                if (setting.ShortName == "Training1Hour") continue;
                if (setting.ShortName == "Training3Hours") continue;
                if (setting.ShortName == "Training8Hours") continue;

                if (setting.Paid)
                {
                    bool enabled = false;
                    if (!Boolean.TryParse(setting.Value, out enabled))
                    {
                        if (!Boolean.TryParse(setting.DefaultValue, out enabled)) enabled = false;
                    }
                    if (enabled) monthlySum += setting.Price;
                }

                int usageCount = 0;
                int.TryParse(setting.Value, out usageCount);
                int paidQty = usageCount - setting.UsageCountLimit;
                decimal priceMonth = paidQty > 0 ? paidQty * setting.Price : 0;
                monthlySum += priceMonth;
            }

            if (monthlySum>0 && inst.BillingPlan!=BillingPlan.Paid) InstanceProvider.UpdateInstance(inst, BillingPlan.Paid);
            else if (monthlySum==0 && inst.BillingPlan!=BillingPlan.Free) InstanceProvider.UpdateInstance(inst, BillingPlan.Free);

            if (SubscriptionId == 0) return;

            foreach (Setting setting in modifiedSettings)
            {
                int extId = 0;
                if (setting.Paid)
                {
                    bool enabled = false;
                    if (!Boolean.TryParse(setting.Value, out enabled))
                    {
                        if (!Boolean.TryParse(setting.DefaultValue, out enabled)) enabled = false;
                    }
                    if (string.IsNullOrEmpty(setting.ExternalId) || !int.TryParse(setting.ExternalId, out extId)) continue;
                    if (extId == 0) continue;
                    chargify.UpdateComponentAllocationForSubscription(SubscriptionId, extId, enabled ? 1 : 0);
                    continue;
                }

                int usageCount = 0;
                int.TryParse(setting.Value, out usageCount);

                if (string.IsNullOrEmpty(setting.ExternalId) || !int.TryParse(setting.ExternalId, out extId)) continue;
                if (extId == 0) continue;
                chargify.UpdateComponentAllocationForSubscription(SubscriptionId, extId, usageCount < 0 ? 0 : usageCount);
            }
        }
    }
}
