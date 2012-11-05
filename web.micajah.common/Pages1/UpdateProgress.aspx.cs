using System;
using System.Web.UI;
using System.Threading;

public partial class UpdateProgressPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.IsPostBack)
            Thread.Sleep(3000);
        else
            UpdateProgress1.PostBackControlId = SubmitButton.ClientID;
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {


        if (GenerateTimeout.Checked)
        {
            UpdateProgress1.PostBackHasError = true;
        }
        else
        {
            FreeTextLabel.Text = FreeText.Text;
            FreeText.Text = string.Empty;

            UpdatePanel2.Update();
        }
    }
}
