using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Configuration;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using ChargifyNET;
using ChargifyNET.Configuration;
using System.Configuration;
using System.IO;
using System.Web;

namespace Micajah.Common.WebControls
{
    public class WebEventHandler : UserControl
    {
        private const string WebhookIdHandle = "X-Chargify-Webhook-Id";
        private const string SignatureHeaderHandle = "X-Chargify-Webhook-Signature";
        private const string SignatureQueryStringHandle = "signature";

        #region Overriden Methods
        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack) SaveLogToFile("WebEventHandler", "BEGIN POSTBACK" + Environment.NewLine);
            else SaveLogToFile("WebEventHandler", "BEGIN REQUEST" + Environment.NewLine);
            string possibleData = string.Empty;
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                possibleData = sr.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(possibleData))
            {
                string signature = Request.Headers[SignatureHeaderHandle];
                if (string.IsNullOrEmpty(signature))
                {
                    signature = Request.QueryString[SignatureQueryStringHandle];
                }
                // Grab the webhook id as well, since it'll be used for validation.
                int webhookID = int.Parse(Request.Headers[WebhookIdHandle]);

                // Now that we have data, signature and webhook id, pass it back.
                OnChargifyUpdate(webhookID, signature, possibleData);
            }
            if (IsPostBack) SaveLogToFile("WebEventHandler", "END POSTBACK" + Environment.NewLine);
            else SaveLogToFile("WebEventHandler", "END REQUEST" + Environment.NewLine);
        }
        #endregion

        public virtual void OnChargifyUpdate(int webhookID, string signature, string data)
        {
            string _CrLf = Environment.NewLine;
            string _post = "Postback Headers:" + _CrLf;
            foreach (string _key in Request.Headers)
                _post += "     "+_key + ": " + Request.Headers[_key] + _CrLf;
            _post+="Postback Data:"+_CrLf;
            foreach (string _key in Request.Form.AllKeys)
                _post += "     " + _key + ": " + Request.Form[_key] +_CrLf;
//            _post+="Postback Content: "+data+_CrLf;
            ChargifyConnect _chargify = null;
            try {_chargify=ChargifyProvider.CreateChargify();}
            catch (Exception _ex) 
            {
                _post+="ERROR: "+_ex.ToString()+_CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;            
            }
            if (!data.IsChargifyWebhookContentValid(signature, _chargify.SharedKey))
            {
                _post += "ERROR: Webhook Content is Not Valid." + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;            
            }
            if (string.IsNullOrEmpty(Request.Form["event"]) || Request.Form["event"] == "renewal_success")
            {
                _post += "ERROR: event type is empty or is not renewal_success."+_CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;            
            }

            Guid _orgId=Guid.Empty;

            if (string.IsNullOrEmpty(Request.Form["payload[subscription][customer][reference]"]))
            {
                _post += "ERROR: organization reference Guid is empty." + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;
            }
            try { _orgId = new Guid(Request.Form["payload[subscription][customer][reference]"]); }
            catch (Exception _ex) 
            { 
                _post += "ERROR: Can't parse Organization Guid! " + _ex.ToString() + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;            
            }
            
            DateTime _expDate=DateTime.MinValue;

            if (string.IsNullOrEmpty(Request.Form["payload[subscription][current_period_ends_at]"]))
            {
                _post += "ERROR: Customer Subscription current period ends at is empty." + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;
            }

            try { _expDate = DateTime.Parse(Request.Form["payload[subscription][current_period_ends_at]"]); }
            catch (Exception _ex)
            {
                _post += "ERROR: Can't parse Customer current period ends at datetime! " + _ex.ToString() + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;
            }

            try
            {
                Micajah.Common.Bll.Providers.OrganizationProvider.UpdateOrganization(_orgId, _expDate);
            }
            catch (Exception _ex)
            {
                _post += "ERROR: Can't update Organization expiration datetime! " + _ex.ToString() + _CrLf;
                SaveLogToFile("ChargifyWebHook", _post);
                return;
            }
        }

        public static void SaveLogToFile(string MsgType, string Msg)
        {

            //logPrefix used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            string logPrefix = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string filename = "ChargifyWebHooks_Log_" + year + "_" + month + "_" + day + ".txt";


            string path = HttpContext.Current.Request.MapPath("~/temp");
            if (path == null || path.Length == 0) throw new Exception("Can't create Log File. Application setting LogFile.Path is undefined.");
            StreamWriter sw = new StreamWriter(path +"\\"+ filename, true);
            sw.WriteLine(logPrefix + MsgType);
            foreach (string _line in Msg.Split(new string[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)) sw.WriteLine(_line);
            sw.Flush();
            sw.Close();
        }
    }
}
