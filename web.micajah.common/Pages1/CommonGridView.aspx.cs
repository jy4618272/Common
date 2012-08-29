using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.WebControls;

public partial class CommonGridViewTestPage : Page
{
    ColorScheme selectedScheme = ColorScheme.White;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ColorScheme"] != null)
                selectedScheme = (ColorScheme)Enum.Parse(typeof(ColorScheme), Request.QueryString["ColorScheme"]);

            foreach (ColorScheme scheme in Enum.GetValues(typeof(ColorScheme)))
            {
                ListItem item = new ListItem(scheme.ToString(), scheme.ToString());
                DropDownList1.Items.Add(item);
                if (scheme == selectedScheme) item.Selected = true;
            }

            CommonGridView4.ColorScheme = CommonGridView1.ColorScheme = selectedScheme;
            CommonGridView.ApplyStyle(Table1, selectedScheme);
            CommonGridView.ApplyStyle(Table2, selectedScheme);

            CommonGridView4.DataBind();

            //GridView1.EmptyDataText = "No Data Found.";
            //GridView1.DataBind();

            if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Micajah.Common.Pages.MasterPageTheme.Modern)
                SelectHolder.Visible = false;
        }

        //CommonGridView1.Columns[0].Visible = (!CommonGridView1.Columns[0].Visible);

        HyperLink1.Text = "Gooooooogle!";
        //MyOwnedLink.NavigateUrl = "javascript:alert('1');";
    }

    //OnDataBinding="cmbColSetting_DataBinding" OnInit="cmbColSetting_Init"
    //protected void cmbColSetting_DataBinding(object sender, EventArgs e)
    //{
    //}

    //protected void cmbColSetting_Init(object sender, EventArgs e)
    //{
    //}

    protected void CommonGridView1_Action(object sender, CommonGridViewActionEventArgs e)
    {
        if (e.Action == CommandActions.Search)
            CommonGridView1.DataBind();
    }

    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);
    //    if (this.IsPostBack)
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + Request.Form["__EVENTTARGET"] + "');", true);
    //    //if (this.IsPostBack)
    //    //CommonGridView1.Columns[0].HeaderText = "test caption text change";
    //}

    protected void ObjectDataSource1_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
    {
        e.ParameterValues[0] = CommonGridView1.SearchText;
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages1/CommonGridView.aspx?ColorScheme=" + DropDownList1.SelectedValue);
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        CommonGridView.ApplyStyle(GridView1, selectedScheme);
    }

    protected void ShowInactiveButton_Click(object sender, EventArgs e)
    {
    }

    protected void ShowActiveButton_Click(object sender, EventArgs e)
    {
    }

    protected void SelectButton_Click(object sender, EventArgs e)
    {

    }
}
