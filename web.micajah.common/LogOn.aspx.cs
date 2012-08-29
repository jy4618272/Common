using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Security;

public partial class LogOn : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        LogOn1.InnerControl.Load += new EventHandler(InnerControl_Init);
        LogOn1.InnerControl.InitParameters += new EventHandler(InnerControl_InitParameters);
    }

    void InnerControl_Init(object sender, EventArgs e)
    {
        UserContext.SelectedInstanceChanged += new EventHandler(UserContext_SelectedInstanceChanged);
    }

    void UserContext_SelectedInstanceChanged(object sender, EventArgs e)
    {
    }

    void InnerControl_InitParameters(object sender, EventArgs e)
    {
        // Adds some custom logic here.
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }
}