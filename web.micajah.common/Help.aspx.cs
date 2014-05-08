using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.HeaderMessage = "No Credit Card on File.";
        this.Master.HeaderMessageDescription = "<a href='/mc/admin/accountsettings.aspx'>Click Here</a> to update your account.";
        this.Master.HeaderMessageType = Micajah.Common.WebControls.NoticeMessageType.Warning;
    }
}