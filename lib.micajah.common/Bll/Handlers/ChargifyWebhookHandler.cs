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
            if (config == null) throw new HttpException(500, "Chargify section is not defined in a web.config file");
            if (!data.IsChargifyWebhookContentValid(signature, config.GetSharedKeyForDefaultOrFirstSite())) throw new HttpException(500, "Chargify Webhook Exception. Data was INVALID through self-validation. WebhookID: " + webhookID.ToString() + " Data: " + data);

            NameValueCollection query = new NameValueCollection();
            string[] _arr = data.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string _arrElem in _arr)
            {
                string[] _a = _arrElem.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (_a[0].Length > 0) query.Add(_a[0], _a.Length > 1 ? Uri.UnescapeDataString(_a[1]) : string.Empty);
            }

            Guid _orgId = Guid.Empty;
            Guid _instId = Guid.Empty;
            string _event = query["event"];

            if (_event == "payment_success" || _event == "payment_failure")
            {
                if (string.IsNullOrEmpty(query["payload[subscription][customer][reference]"])) throw new HttpException(500, "Customer Refererence Organization and Instance Guids is not defined. Chargify Webhook Event:\"" + _event + "\" Id:" + webhookID.ToString() + " Data:" + data);
                string custRef = query["payload[subscription][customer][reference]"];
                string errDescr = "Chargify Webhook Event:\"" + _event + "\" Id:" + webhookID.ToString() + " Reference:\"" + custRef + "\" Data:" + data;
                string[] _arrCustRef = custRef.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (_arrCustRef.Length < 2) throw new HttpException(500, "Instance Refererence Guid is not defined. " + errDescr);
                if (!Guid.TryParse(_arrCustRef[0], out _orgId)) throw new HttpException(500, "Customer Organization Refererence Guid is invalid. " + errDescr);
                if (!Guid.TryParse(_arrCustRef[1], out _instId)) throw new HttpException(500, "Customer Instance Refererence Guid is invalid. " + errDescr);
                Organization _org = null;
                try
                {
                    _org=Providers.OrganizationProvider.GetOrganization(_orgId);
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, "Can't execute Providers.OrganizationProvider.GetOrganization() method. " + errDescr, ex);
                }
                if (_org == null) throw new HttpException(500, "Can't find Organization. " + errDescr);
                Instance _inst = null;
                try
                {
                    _inst=Providers.InstanceProvider.GetInstance(_instId, _orgId);
                }
                catch (Exception ex)
                {
                    throw new HttpException(500, "Can't execute Providers.InstanceProvider.GetInstance() method. " + errDescr, ex);
                }
                if (_inst == null) throw new HttpException(500, "Can't find Instance. " + errDescr);
                if (_event == "payment_failure" && _inst.CreditCardStatus == CreditCardStatus.Registered)
                {
                    try
                    {
                        Providers.InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Declined);
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, "Can't Update Instance CreditCardStatus to Declined. " + errDescr, ex);
                    }
                    string _uEmail =  query["payload[subscription][customer][email]"];
                    string _uFirstName = query["payload[subscription][customer][first_name]"];
                    string _uLastName = query["payload[subscription][customer][last_name]"];
                    string _appName = FrameworkConfiguration.Current.WebApplication.Name;
                    string _salesEmail = FrameworkConfiguration.Current.WebApplication.Email.SalesTeam;
                    string _subject = _org.Name+" "+_inst.Name + " Chargify payment attempt failded.";
                    string _body = _appName +" Chargify service can't process payment operation for \"" + _org.Name + " "+_inst.Name+"\" organization instance.\r\n. Operation status is " + _event+"\r\n\r\nContact User Email: "+_uEmail+" "+_uFirstName+" "+_uLastName;
                    if (!string.IsNullOrEmpty(_salesEmail))
                    {
                        try
                        {
                            Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, FrameworkConfiguration.Current.WebApplication.Support.Email, _salesEmail, string.Empty, _subject, _body, false, false, EmailSendingReason.Undefined);
                        }
                        catch (Exception ex)
                        {
                            throw new HttpException(500, "Can't send failure event support email. " + errDescr, ex);
                        }
                    }
                }
                else if (_event == "payment_success" && _inst.CreditCardStatus != CreditCardStatus.Registered)
                {
                    try
                    {
                        Providers.InstanceProvider.UpdateInstance(_inst, CreditCardStatus.Registered);
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, "Can't Update Instance CreditCardStatus to Registered. "+errDescr, ex);
                    }
                }
            }
            else throw new HttpException(500, "Unknown Chargify Webhook. Event:\""+_event+"\" Id:" + webhookID.ToString() + " Data:" + data);
        }
    }
}
