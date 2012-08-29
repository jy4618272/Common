using System;
using System.Web.UI;
using System.Threading;

public partial class UpdateProgressPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        if (GenerateTimeout.Checked)
        {
            Thread.Sleep(5500);
        }
        else
        {
            Thread.Sleep(3000);

            FreeTextLabel.Text = FreeText.Text;
            FreeText.Text = string.Empty;

            UpdatePanel2.Update();
        }
    }
}
