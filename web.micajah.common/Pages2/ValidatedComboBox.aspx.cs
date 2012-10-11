using System;
using System.Web.UI;

public partial class ValidatedComboBoxTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Micajah.Common.WebControls.ComboBox.ApplyStyle(RadComboBox1);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ValidatedComboBox1.SelectedItem != null)
            Label1.Text = ValidatedComboBox1.SelectedItem.Text;
    }
    protected void ValidatedComboBox1_DataBound(object sender, EventArgs e)
    {
        ValidatedComboBox1.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("", "0"));
    }
}
