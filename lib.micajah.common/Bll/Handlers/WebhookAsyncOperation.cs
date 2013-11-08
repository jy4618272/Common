using System;
using System.IO;
using System.Web;
using System.Threading;

namespace Micajah.Common.Bll.Handlers
{
    public class WebhookAsyncOperation : IAsyncResult
    {
        private bool completed;
        private Object state;
        private AsyncCallback callback;
        private HttpContext context;
        private int webhookId;
        private string webhookData;

        public WebhookAsyncOperation(AsyncCallback cb, HttpContext ct, Object opState, int whId, string whData)
        {
            callback = cb;
            context = ct;
            state = opState;
            completed = false;
            webhookId = whId;
            webhookData = whData;
        }

        public HttpContext Context
        {
            get { return context; }
        }

        public object AsyncState
        {
            get { return state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return completed; }
        }

        public string WebhookData
        {
            get { return webhookData; }
        }

        public int WebhookId
        {
            get { return webhookId; }
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), state);
        }

        private void StartAsyncTask(Object workItemState)
        {
            if (state != null && state is Exception)
            {
                completed = true;
                callback(this);
                return;
            }

            try
            {
                if (webhookId == 0) //test webhook functionality
                    context.Response.Write("Webhook test passed.\r\n");
                else if (webhookId == 1) //run update Chargify Update Allocations
                {
                    context.Response.Write("Executing Update Chargify Subscriptions Allocations.\r\n");
                    int updatedCount = ChargifyHandler.UpdateChargifyAllocations();
                    context.Response.Write("Finished Update Chargify Subscriptions Allocations. Updated " + updatedCount.ToString() + " allocations.\r\n");
                }
                else if (webhookId == 2) //run ldap replication
                {
                    context.Response.Write("Executing LDAP replication.\r\n");
                    LdapHandler lh = new LdapHandler();
                    lh.ReplicateAllOrganizations();
                    context.Response.Write("Finished LDAP replication.\r\n");
                }
                else throw new HttpException(400, "Unknown Webhook ID. ID=" + webhookId.ToString());
                context.Response.Write(string.Format("Completion Webhook request processing at {0:d-MMM-yyyy HH:mm}.\r\n", DateTime.UtcNow));
            }
            catch (Exception ex)
            {
                state = ex;
            }
            completed = true;
            callback(this);
        }
    }
}
