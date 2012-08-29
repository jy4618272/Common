using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.WebControls;

public partial class MagicFormTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ColorScheme selectedScheme = ColorScheme.White;
            if (Request.QueryString["ColorScheme"] != null)
                selectedScheme = (ColorScheme)Enum.Parse(typeof(ColorScheme), Request.QueryString["ColorScheme"]);

            foreach (ColorScheme scheme in Enum.GetValues(typeof(ColorScheme)))
            {
                ListItem item = new ListItem(scheme.ToString(), scheme.ToString());
                DropDownList1.Items.Add(item);
                if (scheme == selectedScheme) item.Selected = true;
            }

            MagicForm1.ColorScheme = selectedScheme;
            MagicForm.ApplyStyle(Table1, MagicForm1.ColorScheme);
            MagicForm.ApplyStyle(Table2, MagicForm1.ColorScheme, false, false);

            if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Micajah.Common.Pages.MasterPageTheme.Modern)
                SelectHolder.Visible = false;
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages1/MagicForm.aspx?ColorScheme=" + DropDownList1.SelectedValue);
    }

    protected void ShowInactiveButton_Click(object sender, EventArgs e)
    {
    }

    protected void ShowActiveButton_Click(object sender, EventArgs e)
    {
    }
}
