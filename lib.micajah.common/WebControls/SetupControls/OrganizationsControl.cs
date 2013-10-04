using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage organizations.
    /// </summary>
    public class OrganizationsControl : BaseControl
    {
        #region Members

        protected ObjectDataSource DatabaseListDataSource;
        protected TextBox OrganizationName;
        protected Button SearchButton;
        protected Panel SearchPanel;
        protected ComboBox ActiveValueList;

        private ComboBox m_DatabaseList;
        private ComboBox m_ParentOrgsList;
        private ImageUpload m_LogoImageUpload;
        private DatePicker m_ExpirationTime;
        private DatePicker m_CanceledTime;
        private UserContext m_UserContext;

        #endregion

        #region Private Properties

        private ComboBox DatabaseList
        {
            get
            {
                if (m_DatabaseList == null) m_DatabaseList = EditForm.FindControl("DatabaseList") as ComboBox;
                return m_DatabaseList;
            }
        }

        private ComboBox ParentOrgsList
        {
            get
            {
                if (m_ParentOrgsList == null) m_ParentOrgsList = EditForm.FindControl("ParentOrgsList") as ComboBox;
                return m_ParentOrgsList;
            }
        }

        private ImageUpload LogoImageUpload
        {
            get
            {
                if (m_LogoImageUpload == null) m_LogoImageUpload = EditForm.FindControl("LogoImageUpload") as ImageUpload;
                return m_LogoImageUpload;
            }
        }

        private DatePicker ExpirationTime
        {
            get
            {
                if (m_ExpirationTime == null) m_ExpirationTime = EditForm.Rows[7].Cells[1].Controls[0] as DatePicker;
                return m_ExpirationTime;
            }
        }

        private DatePicker CanceledTime
        {
            get
            {
                if (m_CanceledTime == null) m_CanceledTime = EditForm.Rows[9].Cells[1].Controls[0] as DatePicker;
                return m_CanceledTime;
            }
        }

        #endregion

        #region Private Methods

        private void ActiveValueListDataBind()
        {
            using (RadComboBoxItem item = new RadComboBoxItem(Resources.OrganizationsControl_ActiveValueList_ActiveItem_Text, "1"))
            {
                ActiveValueList.Items.Add(item);
            }

            using (RadComboBoxItem item = new RadComboBoxItem(Resources.OrganizationsControl_ActiveValueList_InactiveItem_Text, "2"))
            {
                ActiveValueList.Items.Add(item);
            }

            using (RadComboBoxItem item = new RadComboBoxItem(Resources.OrganizationsControl_ActiveValueList_DeletedItem_Text, "3"))
            {
                ActiveValueList.Items.Add(item);
            }

            using (RadComboBoxItem item = new RadComboBoxItem(Resources.OrganizationsControl_ActiveValueList_AllItem_Text, "0"))
            {
                ActiveValueList.Items.Add(item);
            }
        }

        private void Redirect()
        {
            if (UserContext.Current.OrganizationId == (Guid)EditForm.DataKey[0])
            {
                Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.OrganizationsPageActionId);
                if (action != null)
                    Response.Redirect(action.AbsoluteNavigateUrl);
            }
        }

        #endregion

        #region Protected Methods

        protected static string GetValidatedValue(object value, object databaseId)
        {
            if (!Support.IsNullOrDBNull(value))
            {
                if (value.ToString() == "Error:DatabaseDoesntExistOrInactive")
                {
                    return string.Format(CultureInfo.InvariantCulture, "<div style='width: 100%;' title=\"{0}\">&nbsp;</div>"
                        , string.Format(CultureInfo.InvariantCulture, Support.PreserveDoubleQuote(Resources.OrganizationsControl_DatabaseDoesntExistOrInactive), databaseId));
                }
                else
                    return value.ToString();
            }
            return string.Empty;
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            object obj = Support.ConvertStringToType(this.DatabaseList.SelectedValue, typeof(Guid));
            e.InputParameters["databaseId"] = ((obj == null) ? null : new Guid?((Guid)obj));

            if (this.ExpirationTime.IsEmpty)
                e.InputParameters["expirationTime"] = null;
            else
                e.InputParameters["expirationTime"] = TimeZoneInfo.ConvertTimeToUtc(this.ExpirationTime.SelectedDate, m_UserContext.TimeZone);

            if (this.CanceledTime.IsEmpty)
                e.InputParameters["canceledTime"] = null;
            else
                e.InputParameters["canceledTime"] = TimeZoneInfo.ConvertTimeToUtc(this.CanceledTime.SelectedDate, m_UserContext.TimeZone);

            obj = Support.ConvertStringToType(this.ParentOrgsList.SelectedValue, typeof(Guid));
            e.InputParameters["parentOrganizationId"] = ((obj == null) ? null : new Guid?((Guid)obj));

            EntityDataSource.Selected += new ObjectDataSourceStatusEventHandler(EntityDataSource_Selected);
        }

        protected void EntityDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            Organization org = e.ReturnValue as Organization;
            if (org != null)
            {
                if (org.Deleted)
                    EditForm.AutoGenerateDeleteButton = false;
            }
        }

        protected void EditForm_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
            if (e == null) return;

            if (ShowError(e.Exception))
                e.ExceptionHandled = true;
            else
                EditFormReset();
        }

        protected void EntityDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            this.LogoImageUpload.LocalObjectId = string.Format(CultureInfo.InvariantCulture, "{0:N}", e.ReturnValue);
        }

        protected void List_DataBound(object sender, EventArgs e)
        {
            SearchPanel.Visible = true;
            if (!this.IsPostBack)
            {
                if (string.IsNullOrEmpty(List.SortExpression))
                    List.Sort("Name", SortDirection.Ascending);
            }
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            SearchPanel.Visible = false;

            object obj = null;

            if (this.DatabaseList != null)
            {
                using (RadComboBoxItem item = new RadComboBoxItem(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationsControl_DatabaseList_MasterDatabaseItem_Text, Resources.OrganizationProvider_MasterDatabaseText), string.Empty))
                {
                    this.DatabaseList.Items.Insert(0, item);
                }

                using (RadComboBoxItem item = new RadComboBoxItem(string.Empty, "x"))
                {
                    this.DatabaseList.Items.Insert(0, item);
                }

                obj = DataBinder.Eval(EditForm.DataItem, "DatabaseId");
                if (!Support.IsNullOrDBNull(obj))
                {
                    RadComboBoxItem item1 = this.DatabaseList.FindItemByValue(obj.ToString());
                    if (item1 != null)
                        this.DatabaseList.SelectedValue = obj.ToString();
                }
            }

            if (this.ParentOrgsList != null)
            {
                using (RadComboBoxItem item = new RadComboBoxItem(string.Empty, string.Empty))
                {
                    this.ParentOrgsList.Items.Add(item);
                }

                Guid databaseId = Guid.Empty;
                Guid? organizationId = null;
                if (EditForm.CurrentMode == DetailsViewMode.Insert)
                {
                    databaseId = ((DatabaseList.Items.Count > 2) ? (Guid)Support.ConvertStringToType(DatabaseList.Items[2].Value, typeof(Guid)) : Guid.Empty);
                }
                else
                {
                    databaseId = (Support.IsNullOrDBNull(obj) ? Guid.Empty : (Guid)obj);
                    organizationId = new Guid?((Guid)EditForm.DataKey[0]);
                }

                this.ParentOrgsList.DataSource = OrganizationProvider.GetOrganizationsByParentOrganizationIdAndDatabaseId(databaseId, organizationId);
                this.ParentOrgsList.DataBind();

                obj = DataBinder.Eval(EditForm.DataItem, "ParentOrganizationId");
                if (Support.IsNullOrDBNull(obj)) obj = string.Empty;

                RadComboBoxItem item1 = this.ParentOrgsList.FindItemByValue(obj.ToString());
                if (item1 != null)
                    this.ParentOrgsList.SelectedValue = obj.ToString();
            }

            obj = DataBinder.Eval(EditForm.DataItem, "CreatedTime");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)EditForm.FindControl("CreatedTimeLiteral");
                lit.Text = Support.ToShortDateString((DateTime)obj, m_UserContext.TimeZone);
            }

            obj = DataBinder.Eval(EditForm.DataItem, "ExpirationTime");
            if (!Support.IsNullOrDBNull(obj))
                this.ExpirationTime.SelectedDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)obj, m_UserContext.TimeZone);

            obj = DataBinder.Eval(EditForm.DataItem, "CanceledTime");
            if (!Support.IsNullOrDBNull(obj))
                this.CanceledTime.SelectedDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)obj, m_UserContext.TimeZone);
        }

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            LinkButton btn = e.Row.FindControl("UpdateActiveButton") as LinkButton;
            if (btn != null)
            {
                if ((bool)DataBinder.Eval(e.Row.DataItem, "Deleted"))
                {
                    btn.Text = Resources.OrganizationsControl_List_UpdateActiveButton_UndeleteText;
                    btn.CommandName = "UnDelete";
                }
                else if ((bool)DataBinder.Eval(e.Row.DataItem, "Active"))
                {
                    btn.Text = Resources.OrganizationsControl_List_UpdateActiveButton_InactivateText;
                    btn.CommandName = "Inactivate";
                }
                else
                {
                    btn.Text = Resources.OrganizationsControl_List_UpdateActiveButton_ActivateText;
                    btn.CommandName = "Activate";
                }
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "OrganizationId").ToString();
            }
        }

        protected void List_RowCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            object obj = Support.ConvertStringToType((string)e.CommandArgument, typeof(Guid));

            switch (e.CommandName)
            {
                case "Inactivate":
                case "Activate":
                    if (obj != null)
                    {
                        OrganizationProvider.UpdateOrganizationActive((Guid)obj, (e.CommandName == "Activate"));
                        List.DataBind();
                    }
                    break;
                case "UnDelete":
                    if (obj != null)
                    {
                        OrganizationProvider.UndeleteOrganization((Guid)obj);
                        List.DataBind();
                    }
                    break;

            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            List.DataBind();
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            m_UserContext = UserContext.Current;

            if (!this.IsPostBack)
                this.ActiveValueListDataBind();
        }

        protected override void LoadResources()
        {
            base.LoadResources();
            List.Columns[0].HeaderText = Resources.OrganizationsControl_List_NameColumn_HeaderText;
            List.Columns[1].HeaderText = "Parent";
            List.Columns[2].HeaderText = "Expiration";
            EditForm.Fields[3].HeaderText = Resources.OrganizationsControl_EditForm_LogoImageField_HeaderText;
            EditForm.Fields[4].HeaderText = Resources.OrganizationsControl_EditForm_DatabaseIdField_HeaderText;
            EditForm.Fields[5].HeaderText = "Parent Organization";
            EditForm.Fields[12].HeaderText = Resources.OrganizationsControl_EditForm_CreatedTimeField_HeaderText;
        }

        protected override void ListInitialize()
        {
            base.ListInitialize();

            List.AutoGenerateDeleteButton = false;
        }

        protected override void EditFormInitialize()
        {
            base.EditFormInitialize();

            EditForm.AutoGenerateDeleteButton = true;
            EditForm.ItemDeleted += new DetailsViewDeletedEventHandler(EditForm_ItemDeleted);
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            base.EditForm_ItemCommand(sender, e);

            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                this.LogoImageUpload.RejectChanges();
                SearchPanel.Visible = true;
            }
        }

        protected override void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            base.EditForm_ItemInserted(sender, e);

            if (e == null) return;

            if (e.Exception == null)
                this.LogoImageUpload.AcceptChanges();
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception == null)
            {
                this.LogoImageUpload.AcceptChanges();
                this.Redirect();
            }
        }

        protected override void EditFormReset()
        {
            base.EditFormReset();
            EditForm.Fields[6].Visible = true;
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            base.List_Action(sender, e);

            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Edit:
                    EditForm.Fields[6].Visible = false;
                    break;
            }
        }

        protected override void List_PageIndexChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
