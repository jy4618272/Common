using System;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Security;
using System.Web.UI.WebControls;
using System.Text;

public partial class MessageListTestPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        Guid? toUserId = null;
        StringBuilder sb = new StringBuilder();

        foreach (ListItem item in UserList.Items)
        {
            if (item.Selected)
            {
                if (!toUserId.HasValue)
                    toUserId = new Guid(item.Value);
                else
                    sb.AppendFormat(", {0}", item.Text);
            }
        }

        UserList.ClearSelection();

        if (sb.Length > 0)
        {
            sb.Remove(0, 2);
            sb.Insert(0, "This message additionally were sent to:<br />");
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
                sb.Insert(0, "<br /><br />--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------<br />");
        }
        sb.Insert(0, MessageTextBox.Text);

        MessageProvider.InsertMessage(MessageList1.ParentMessageId, MessageList1.LocalObjectType, MessageList1.LocalObjectId, UserContext.Current.UserId, toUserId, "Tkt Response", sb.ToString());

        MessageList1.DataBind();

        MessageTextBox.Text = string.Empty;
    }
}
