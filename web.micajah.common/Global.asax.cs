﻿using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using System;
using System.Web;

namespace Micajah.Common.TestSite
{
    public class Global1 : Micajah.Common.Application.WebApplication
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            Micajah.Common.Bll.Handlers.ActionHandler.Current = new ActionCustomHandler();
            Micajah.Common.Bll.Handlers.SettingHandler.Current = new SettingCustomHandler();
            LoginProvider.Current = new CustomLoginProvider();

            EmailSending += new EventHandler<EmailSendingEventArgs>(Global_EmailSending);

            //if (Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.IsAvailable)
            //    Micajah.Common.Application.CacheManager.Current = new AzureCacheManager();
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