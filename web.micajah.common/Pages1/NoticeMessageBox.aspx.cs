using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.WebControls;

public partial class NoticeMessageBoxTestPage : Page
{
    private void SetNoticeMessageBoxProperties()
    {
        NoticeMessageBox1.Message = TextBox1.Text;
        NoticeMessageBox1.Description = TextBox2.Text;
        NoticeMessageBox1.MessageType = (NoticeMessageType)Enum.Parse(typeof(NoticeMessageType), DropDownList1.SelectedValue);
        NoticeMessageBox1.Size = (NoticeMessageBoxSize)Enum.Parse(typeof(NoticeMessageBoxSize), DropDownList2.SelectedValue);
        if (!string.IsNullOrEmpty(TextBox3.Text))
            NoticeMessageBox1.Width = Unit.Parse(TextBox3.Text);
        else
            NoticeMessageBox1.Width = Unit.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (string name in Enum.GetNames(typeof(NoticeMessageType)))
            {
                DropDownList1.Items.Add(name);
            }
            if (DropDownList1.Items.Count > 0) DropDownList1.SelectedIndex = 0;

            foreach (string name in Enum.GetNames(typeof(NoticeMessageBoxSize)))
            {
                DropDownList2.Items.Add(name);
            }
            if (DropDownList2.Items.Count > 0) DropDownList1.SelectedIndex = 0;

            SetNoticeMessageBoxProperties();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SetNoticeMessageBoxProperties();
    }
}
