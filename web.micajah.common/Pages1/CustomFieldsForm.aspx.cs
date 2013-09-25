using System;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Telerik.Web.UI;

public partial class CustomFieldsFormTestPage : System.Web.UI.Page
{
    private void SetCustomFieldsFormProperties()
    {
        CustomFieldsForm1.EntityId = new Guid(ComboBox1.SelectedValue);
        CustomFieldsForm1.LocalEntityId = TextBox1.Text;
        CustomFieldsForm1.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ComboBox1.Items.Add(new RadComboBoxItem());

            foreach (Entity entity in EntityFieldProvider.Entities)
            {
                if (!entity.EnableHierarchy)
                    ComboBox1.Items.Add(new RadComboBoxItem(entity.Name, entity.Id.ToString()));
            }

            if (ComboBox1.Items.Count > 0) ComboBox1.SelectedIndex = 0;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this.IsPostBack)
        {
            Label1.Visible = CustomFieldsForm1.IsEmpty;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SetCustomFieldsFormProperties();
    }
    protected void CustomFieldsForm1_Action(object sender, Micajah.Common.WebControls.MagicFormActionEventArgs e)
    {
        switch (e.Action)
        {
            case Micajah.Common.WebControls.CommandActions.Cancel:
                CustomFieldsForm1.Visible = false;
                break;
        }
    }
}
