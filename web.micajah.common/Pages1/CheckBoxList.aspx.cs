using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.WebControls;

public partial class CheckBoxListTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = string.Empty;
        foreach (ListItem li in ValidatedCheckBoxList1.Items)
        {
            if (li.Selected)
                Label1.Text += li.Text + ", ";
        }
        Label1.Text = Label1.Text.TrimEnd(' ', ',');
    }
}
