using System;

public partial class BwdUserSidePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.VisibleMainMenu = false;
        Master.VisibleLeftArea = false;
        Master.VisibleSubmenu = false;
        Master.VisibleBreadcrumbs = false;
    }
}
