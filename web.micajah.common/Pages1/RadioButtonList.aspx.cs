using System;
using System.Web.UI;

public partial class RadioButtonListTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ValidatedRadioButtonList1.SelectedItem != null)
            Label1.Text = ValidatedRadioButtonList1.SelectedItem.Text;
    }
}
