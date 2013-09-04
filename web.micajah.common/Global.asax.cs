using Micajah.Common.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Micajah.Common.TestSite
{
    public class Global1 : Micajah.Common.Application.WebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            //    OrganizationDataSetTableAdapters.InstanceTableAdapter.InsertCommand.CommandText = "[dbo].[Instance_Insert]";
            //    OrganizationDataSetTableAdapters.InstanceTableAdapter.UpdateCommand.CommandText = "[dbo].[Instance_Update]";
            base.Application_Start(sender, e);

            Micajah.Common.Bll.Handlers.ActionHandler.Current = new ActionCustomHandler();
            Micajah.Common.Bll.Handlers.SettingHandler.Current = new SettingCustomHandler();
            LoginProvider = new CustomLoginProvider();

            EmailSending += new EventHandler<EmailSendingEventArgs>(Global_EmailSending);
        }

        void Global_EmailSending(object sender, EmailSendingEventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                if (ctx.Request != null)
                {
                    if (ctx.Request.Url.ToString().Contains("localhost"))
                        e.Cancel = true;
                }
            }
        }
    }
}