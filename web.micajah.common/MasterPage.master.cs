using System;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class MasterPage : Micajah.Common.Pages.MasterPage
{
    protected override void OnInit(EventArgs e)
    {
        this.SearchButtonClick += new CommandEventHandler(MasterPage_SearchButtonClick);
        base.OnInit(e);
    }

    protected void MasterPage_SearchButtonClick(object sender, CommandEventArgs e)
    {
        this.MessageType = Micajah.Common.WebControls.NoticeMessageType.Information;
        this.MessageDescription = "You have searched the following: " + e.CommandArgument.ToString();
        this.Message = "Search completed.";
    }
}
