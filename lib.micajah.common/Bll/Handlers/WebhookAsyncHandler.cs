using System;
using System.IO;
using System.Web;
using System.Threading;
using Micajah.Common.Configuration;

namespace Micajah.Common.Bll.Handlers
{
    public class WebhookAsyncHandler : IHttpAsyncHandler
    {
        private const string WebhookIdHandle = "X-Micajah-Webhook-Id";
        private const string SecretHeaderHandle = "X-Micajah-Webhook-Secret";
        private const string SecretQueryStringHandle = "secret";

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            string possibleData = string.Empty;
            using (StreamReader sr = new StreamReader(context.Request.InputStream))
            {
                possibleData = sr.ReadToEnd();
            }

            string secret = context.Request.Headers[SecretHeaderHandle];
            
            if (string.IsNullOrEmpty(secret)) secret = context.Request.QueryString[SecretQueryStringHandle];

            WebhookAsyncOperation asyncOperation = null;
            int webhookID = -1;

            if (FrameworkConfiguration.Current.WebApplication.Integration.Webhook == null)
                asyncOperation = new WebhookAsyncOperation(cb, context, new HttpException(400, "Webhook integration section is not defined."), -1, string.Empty);
            else if (FrameworkConfiguration.Current.WebApplication.Integration.Webhook.Secret != secret)
                asyncOperation = new WebhookAsyncOperation(cb, context, new HttpException(400, "Webhook Request secret is not valid."), -1, string.Empty);
            else if (string.IsNullOrEmpty(context.Request.Headers[WebhookIdHandle]))
                asyncOperation = new WebhookAsyncOperation(cb, context, new HttpException(400, "Webhook ID is not defined."), -1, string.Empty);
            else if (!int.TryParse(context.Request.Headers[WebhookIdHandle], out webhookID))
                asyncOperation = new WebhookAsyncOperation(cb, context, new HttpException(400, "Webhook ID is invalid. ID=" + context.Request.Headers[WebhookIdHandle]), -1, string.Empty);
            else
            {
                context.Response.Write(string.Format("Webhook request processing started at {0:d-MMM-yyyy HH:mm}.\r\n", DateTime.UtcNow));
                asyncOperation = new WebhookAsyncOperation(cb, context, extraData, webhookID, possibleData);
            }

            asyncOperation.StartAsyncWork();

            return asyncOperation;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            HttpContext context = ((WebhookAsyncOperation)result).Context;

            if (result != null && result.AsyncState != null && result.AsyncState is Exception)
            {
                if (!FrameworkConfiguration.Current.WebApplication.Integration.Webhook.HandleErrors) throw new Exception("Webhooh Async Operation Error.", (Exception)result.AsyncState);

                context.Response.Clear();
                if (result.AsyncState is HttpException) context.Response.StatusCode = ((HttpException)result.AsyncState).GetHttpCode();
                else context.Response.StatusCode = 500;
                context.Response.Write(result.AsyncState.ToString());
            }
            else context.Response.Write(string.Format("Webhook request processing completed at {0:d-MMM-yyyy HH:mm}.", DateTime.UtcNow));
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }
    }
}
