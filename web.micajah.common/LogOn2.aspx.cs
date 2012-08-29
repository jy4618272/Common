using System;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using System.Security.Authentication;

public partial class LogOn2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Login1.DestinationPageUrl = ResourceProvider.GetActiveOrganizationUrl();
        }
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            e.Authenticated = WebApplication.LoginProvider.Authenticate(Login1.UserName, Login1.Password, true, Login1.RememberMeSet);

            // Yo can specify the identifiers of the organization or/and instance.
            //e.Authenticated = WebApplication.LoginProvider.Authenticate(Login1.UserName, Login1.Password, true, Login1.RememberMeSet, organizationId, instanceId);
        }
        catch (AuthenticationException) { }
    }
}
