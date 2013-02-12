using System;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Micajah.Common.Configuration;
using System.Globalization;
using Telerik.Web.UI;
using Micajah.Common.Bll;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage instances.
    /// </summary>
    public class InstancesControl : BaseControl
    {
        #region Private Methods

        private void Redirect()
        {
            if (UserContext.Current.SelectedInstanceId == (Guid)EditForm.DataKey[0])
            {
                Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.InstancesPageActionId);
                if (action != null)
                    Response.Redirect(action.AbsoluteNavigateUrl);
            }
        }

        #endregion

        #region Overriden Methods

        protected override void ListInitialize()
        {
            base.ListInitialize();

            List.AutoGenerateEditButton = false;
            List.AutoGenerateDeleteButton = false;
        }

        protected override void EditFormInitialize()
        {
            base.EditFormInitialize();

            EditForm.Fields[6].Visible = UserContext.Current.IsFrameworkAdministrator;
            EditForm.AutoGenerateDeleteButton = true;
            EditForm.ItemDeleted += new DetailsViewDeletedEventHandler(EditForm_ItemDeleted);
            EditForm.DataBound += new EventHandler(EditForm_DataBound);
            EditForm.ItemUpdated+=new DetailsViewUpdatedEventHandler(EditForm_ItemUpdated);
        }

        void EditForm_DataBound(object sender, EventArgs e)
        {
            ComboBox cmb = EditForm.FindControl("cmbBillingPlan") as ComboBox;
            if (cmb != null)
            {
                cmb.Items.Add(new RadComboBoxItem(BillingPlan.Free.ToString(), ((int)BillingPlan.Free).ToString(CultureInfo.InvariantCulture)));
                cmb.Items.Add(new RadComboBoxItem(BillingPlan.Paid.ToString(), ((int)BillingPlan.Paid).ToString(CultureInfo.InvariantCulture)));
                cmb.Items.Add(new RadComboBoxItem(BillingPlan.Custom.ToString(), ((int)BillingPlan.Custom).ToString(CultureInfo.InvariantCulture)));
                cmb.SelectedValue = ((int)((Instance)EditForm.DataItem).BillingPlan).ToString(CultureInfo.InvariantCulture);
            }
        }

        protected override void LoadResources()
        {
            base.LoadResources();

            List.Columns[0].HeaderText = Resources.InstancesControl_List_NameColumn_HeaderText;

            EditForm.Fields[1].HeaderText = Resources.InstancesControl_EditForm_VanityUrlField_HeaderText;
            EditForm.Fields[2].HeaderText = Resources.InstancesControl_EditForm_TemplateField_HeaderText;
        }

        protected override void List_PageIndexChanged(object sender, EventArgs e)
        {
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception != null) return;
            if (UserContext.Current.IsFrameworkAdministrator)
            {
                ComboBox cmb = EditForm.FindControl("cmbBillingPlan") as ComboBox;
                int val = 0;
                if (cmb != null && int.TryParse(cmb.SelectedValue, out val))
                {
                    Guid instId = (Guid) EditForm.DataKey[0];
                    Instance inst = UserContext.Current.SelectedInstance;
                    if (inst == null || inst.InstanceId != instId) inst = InstanceProvider.GetInstance(instId);
                    if (inst.BillingPlan!=(BillingPlan)val)
                        InstanceProvider.UpdateInstance(inst, (BillingPlan)val);
                }
            }
            this.Redirect();
        }

        #endregion

        #region Protected Methods

        protected void List_DataBound(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (string.IsNullOrEmpty(List.SortExpression))
                    List.Sort("Name", SortDirection.Ascending);
            }
        }

        protected void List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (e == null) return;

            e.Cancel = true;
            this.List_Action(sender, new CommonGridViewActionEventArgs(CommandActions.Edit, e.NewEditIndex));
        }

        protected void EditForm_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            if (e == null) return;

            if (ShowError(e.Exception))
                e.ExceptionHandled = true;
            else
                EditFormReset();
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            base.List_Action(sender, e);
            
            switch (e.Action)
            {
                case CommandActions.Add:                    
                    EditForm.Fields[1].Visible = true;
                    EditForm.Fields[2].Visible = true;
                    break;

                default:
                    EditForm.Fields[1].Visible = false;
                    EditForm.Fields[2].Visible = false;
                    break;
            }
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                TextBox PartialCustomUrlTextBox = EditForm.FindControl("PartialCustomUrlTextBox") as TextBox;
                if (PartialCustomUrlTextBox != null)
                    e.InputParameters["vanityUrl"] = PartialCustomUrlTextBox.Text;


                DropDownList TemplateList = EditForm.FindControl("TemplateList") as DropDownList;
                if (TemplateList != null)
                    e.InputParameters["templateInstanceId"] = new Guid(TemplateList.SelectedValue);
            }
        }

        #endregion
    }
}

