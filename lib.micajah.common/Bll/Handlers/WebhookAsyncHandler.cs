using System;
using System.IO;
using System.Web;
using System.Threading;

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

            if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.Integration.Webhook == null) throw new HttpException(500, "Webhook integration section is not defined.");

            if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.Integration.Webhook.Secret != secret) throw new HttpException(500, "Webhook Request secret is not valid.");

            int webhookID = -1;
            if (string.IsNullOrEmpty(context.Request.Headers[WebhookIdHandle]) || !int.TryParse(context.Request.Headers[WebhookIdHandle], out webhookID)) throw new HttpException(500, "Webhook ID is invalid.");

            context.Response.Write(string.Format("<p>Begin Webhook request processing at {0:d-MMM-yyyy HH:mm}.</p>\r\n", DateTime.UtcNow));

            WebhookAsyncOperation asyncOperation = new WebhookAsyncOperation(cb, context, extraData, webhookID, possibleData);
                       
            asyncOperation.StartAsyncWork();

            return asyncOperation;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            if (result.AsyncState is Exception) throw new Exception("Webhook Async Operation Exception.", (Exception)result.AsyncState);
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
