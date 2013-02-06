using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
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
            if (config == null) throw new HttpException(500, "Chargify section is not defined in a web.config file", new NoNullAllowedException("Chargify section is not defined in a web.config file"));
            if (!data.IsChargifyWebhookContentValid(signature, config.GetSharedKeyForDefaultOrFirstSite())) throw new HttpException(500, "Chargify Webhook Exception. Data was INVALID through self-validation.", new InvalidDataException("Chargify Webhook Exception. Data was INVALID through self-validation. WebhookID: " + webhookID.ToString() + " Data: " + data));
            NameValueCollection query = new NameValueCollection();
            string[] _arr = data.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string _arrElem in _arr)
            {
                string[] _a = _arrElem.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (_a[0].Length > 0) query.Add(_a[0], _a.Length > 1 ? System.Web.HttpUtility.HtmlDecode(_a[1]) : string.Empty);
            }

            Guid _orgId = Guid.Empty;
            Guid _instId = Guid.Empty;
            string _event = query["event"];

            if (_event == "payment_success" || _event == "renewal_failure" || _event == "payment_failure")
            {
                if (string.IsNullOrEmpty(query["payload[subscription][customer][reference]"])) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Customer Refererence Organization and Instance Guids is not defined. WebhookID: " + webhookID.ToString() + " Data: " + data);
                string[] _arrCustRef = query["payload[subscription][customer][reference]"].Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                if (_arrCustRef.Length < 2) new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Instance Refererence Guid is not defined. WebhookID: " + webhookID.ToString() + " Data: " + data);
                if (!Guid.TryParse(_arrCustRef[0], out _orgId)) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Customer Organization Refererence Guid is invalid. Reference: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                if (!Guid.TryParse(_arrCustRef[1], out _instId)) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Customer Instance Refererence Guid is invalid. Reference: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                Organization _org = Providers.OrganizationProvider.GetOrganization(_orgId);
                if (_org == null) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Can't find Organization for Refererence: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                Instance _inst = Providers.InstanceProvider.GetInstance(_instId, _orgId);
                if (_inst == null) throw new InvalidDataException("Chargify \"" + _event + "\" Webhook event. Can't find Instance for Refererence: \"" + query["payload[subscription][customer][reference]"] + "\" WebhookID: " + webhookID.ToString() + " Data: " + data);
                if (_event == "renewal_failure" || _event == "payment_failure")
                {
                    Providers.InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Declined);
                    string _uEmail = query["payload[subscription][customer][email]"];
                    string _appName = FrameworkConfiguration.Current.WebApplication.Name;
                    string _salesEmail = FrameworkConfiguration.Current.WebApplication.Email.SalesTeam;
                    string _subject = _appName + " Chargify payment attempt failded.";
                    string _body = "Chargify service can't process payment operation for \"" + _org.Name + " "+_inst.Name+"\" organization instance.\r\n. Operation status is " + _event;
                    if (!string.IsNullOrEmpty(_salesEmail))
                    {
                        Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, _uEmail, _salesEmail, string.Empty, _subject, _body, false, false, EmailSendingReason.Undefined);
                    }
                }
                else if (_event == "payment_success" && _inst.CreditCardStatus != CreditCardStatus.Registered)
                {
                    Providers.InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Registered);
                }
            }
            else throw new ArgumentException("Unknown Chargify Webhook \""+_event+"\" event. WebhookID: " + webhookID.ToString() + " Data: " + data);
        }
    }
}
