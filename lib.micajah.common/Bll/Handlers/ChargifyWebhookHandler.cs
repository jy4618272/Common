using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ChargifyNET;
using ChargifyNET.Configuration;
using Micajah.Common.Application;
using Micajah.Common.Configuration;

namespace Micajah.Common.Bll.Handlers
{
    class ChargifyWebhookHandler : WebhookHandler
    {
        public override void OnChargifyUpdate(int webhookID, string signature, string data)
        {
            ChargifyAccountRetrieverSection config =
                ConfigurationManager.GetSection("chargify") as ChargifyAccountRetrieverSection;
            if (config==null) throw new NoNullAllowedException("Chargify section is not defined in a web.config file");
            if (!data.IsChargifyWebhookContentValid(signature, config.GetSharedKeyForDefaultOrFirstSite())) throw new InvalidDataException("Chargify Webhook Exception. Data was INVALID through self-validation. WebhookID: "+webhookID.ToString()+" Data: " + data);
            NameValueCollection query = new NameValueCollection();
            string[] _arr = data.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string _arrElem in _arr)
            {
                string[] _a = _arrElem.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (_a[0].Length > 0) query.Add(_a[0], _a.Length > 1 ? System.Web.HttpUtility.HtmlDecode(_a[1]) : string.Empty);
            }

            Guid _orgId = Guid.Empty;
            string _event = query["event"];

            if (_event == "payment_success" || _event == "renewal_failure" || _event == "payment_failure")
            {
                if (string.IsNullOrEmpty(query["payload[subscription][customer][reference]"])) throw new InvalidDataException("Chargify \""+_event+"\" Webhook event. Customer Refererence Guid is not defined. WebhookID: " + webhookID.ToString() + " Data: " + data);
                if (!Guid.TryParse(query["payload[subscription][customer][reference]"], out _orgId)) throw new InvalidDataException("Chargify \""+_event+"\" Webhook event. Customer Refererence Guid is invalid. Reference: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                Organization _org = Micajah.Common.Bll.Providers.OrganizationProvider.GetOrganization(_orgId);
                if (_org == null) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Can't find Organization for Refererence: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                if (_event == "renewal_failure" || _event == "payment_failure")
                {
                    Providers.OrganizationProvider.UpdateOrganizationCreditCardStatus(_orgId, CreditCardStatus.Declined);
                    string _uEmail = query["payload[subscription][customer][email]"];
                    string _appName = FrameworkConfiguration.Current.WebApplication.Name;
                    string _supportEmail = FrameworkConfiguration.Current.WebApplication.Support.Email;
                    string _subject = _appName + " Chargify payment attempt failded.";
                    string _body = "Chargify service can't process payment operation for \"" + _org.Name +
                                   "\" organization.\r\n. Operation status is " + _event;
                    try
                    {
                        Support.SendEmail(_uEmail, _supportEmail, string.Empty, _subject, _body, false, false, EmailSendingReason.Undefined);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Chargify \"" + _event + "\" Webhook event. Can't send support email. WebhookID: " + webhookID.ToString() + " Data: " + data+" Error: "+ex.ToString());
                    }
                }
                else if (_event == "payment_success" && _org.CreditCardStatus!=CreditCardStatus.Registered)
                {
                    Providers.OrganizationProvider.UpdateOrganizationCreditCardStatus(_orgId, CreditCardStatus.Registered);
                }
            }
            else throw new ArgumentException("Unknown Chargify Webhook event. WebhookID: " + webhookID.ToString() + " Data: " + data);
        }
    }
}
