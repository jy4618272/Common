using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SecurityControls;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage users.
    /// </summary>
    public class UsersControl : BaseControl
    {
        #region Members

        protected UpdatePanel UpdatePanel1;
        protected Micajah.Common.WebControls.SecurityControls.ChangePasswordControl PasswordForm;
        protected DetailMenu UserDetailMenu;
        protected MagicForm EditUserGroupsForm;
        protected MagicForm EditUserActiveForm;
        protected CommonGridView InvitedUsersList;
        protected ObjectDataSource InvitedUsersListDataSource;
        protected HtmlGenericControl InvitedUsersDiv;
        protected ObjectDataSource GroupDataSource;

        private ComboBox m_CountryList;
        private DropDownList m_TimeZoneList;
        private DropDownList m_TimeFormatList;
        private DropDownList m_DateFormatList;
        private UserContext m_UserContext;

        #endregion

        #region Private Properties

        protected ComboBox CountryList
        {
            get
            {
                if (m_CountryList == null) m_CountryList = EditForm.Rows[14].Cells[1].Controls[0] as ComboBox;
                return m_CountryList;
            }
        }

        private DropDownList TimeZoneList
        {
            get
            {
                if (m_TimeZoneList == null) m_TimeZoneList = EditForm.FindControl("TimeZoneList") as DropDownList;
                return m_TimeZoneList;
            }
        }

        private DropDownList TimeFormatList
        {
            get
            {
                if (m_TimeFormatList == null) m_TimeFormatList = EditForm.FindControl("TimeFormatList") as DropDownList;
                return m_TimeFormatList;
            }
        }

        private DropDownList DateFormatList
        {
            get
            {
                if (m_DateFormatList == null) m_DateFormatList = EditForm.FindControl("DateFormatList") as DropDownList;
                return m_DateFormatList;
            }
        }

        private bool UserChecked
        {
            get
            {
                object obj = ViewState["UserChecked"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["UserChecked"] = value; }
        }

        private Guid SelectedUserId
        {
            get { return ((List.SelectedIndex > -1) ? (Guid)List.SelectedDataKey["UserId"] : Guid.Empty); }
        }

        #endregion

        #region Private Methods

        private void PwdFormReset()
        {
            PasswordForm.LoginId = Guid.Empty;
            PasswordForm.Visible = false;
            EditFormReset();
        }

        private void EditForm_ShowDetails(bool visible)
        {
            EditForm.Fields[1].Visible = visible;
            EditForm.Fields[2].Visible = visible;
            EditForm.Fields[3].Visible = visible;
            EditForm.Fields[4].Visible = visible;
            EditForm.Fields[5].Visible = visible;
            EditForm.Fields[6].Visible = visible;
            EditForm.Fields[7].Visible = visible;
            EditForm.Fields[8].Visible = visible;
            EditForm.Fields[9].Visible = visible;
            EditForm.Fields[10].Visible = visible;
            EditForm.Fields[11].Visible = visible;
            EditForm.Fields[12].Visible = visible;
            EditForm.Fields[13].Visible = visible;
            EditForm.Fields[14].Visible = true;
            EditForm.Fields[15].Visible = visible;
            EditForm.Fields[16].Visible = visible;
            EditForm.Fields[17].Visible = visible;
            EditForm.Fields[18].Visible = visible;
            EditForm.Fields[19].Visible = visible;
            EditForm.Fields[20].Visible = visible;
        }

        private void AddBreadCrumbs(Micajah.Common.Bll.Action action, object sender)
        {
            if (this.MasterPage != null)
            {
                Guid actionId = Guid.NewGuid();

                Micajah.Common.Bll.Action item = this.MasterPage.ActiveAction.Clone();
                item.ActionId = actionId;
                item.Name = UserDetailMenu.Title;
                item.Description = UserDetailMenu.Title;
                item.ParentAction = this.MasterPage.ActiveAction;

                if (sender != null)
                {
                    Control ctl = sender as Control;
                    if (ctl != null)
                    {
                        ctl = ctl.FindControl("btnCancel");
                        if (ctl != null) item.NavigateUrl = this.Page.ClientScript.GetPostBackClientHyperlink(ctl, null);
                    }
                }

                UserContext.Breadcrumbs.Add(item);

                if (action != null)
                {
                    action.ParentActionId = actionId;
                    UserContext.Breadcrumbs.Add(action);
                }

                this.MasterPage.UpdateBreadcrumbs(false);
            }
        }

        private void BackToDetailMenu()
        {
            List.Visible = false;
            InvitedUsersDiv.Visible = false;
            EditForm.Visible = false;
            PasswordForm.Visible = false;
            EditUserGroupsForm.Visible = false;
            EditUserActiveForm.Visible = false;
            UserDetailMenu.Visible = true;
            UserContext.Breadcrumbs.RemoveLast();
            this.MasterPage.UpdateBreadcrumbs(false);
        }

        private void SaveCountry()
        {
            if (CountryList != null)
            {
                if (m_CountryList.SelectedIndex == -1)
                    CountryProvider.InsertCountry(m_CountryList.Text);
            }
        }

        #endregion

        #region Protected Methods

        protected void List_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (e == null) return;

            Guid userId = (Guid)e.Keys["UserId"];
            if (m_UserContext.UserId == userId && m_UserContext.IsOrganizationAdministrator)
                e.Cancel = true;
        }

        protected void PasswordForm_PasswordUpdated(object sender, EventArgs e)
        {
            this.BackToDetailMenu();
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["timeZoneId"] = (string.IsNullOrEmpty(TimeZoneList.SelectedValue) ? null : TimeZoneList.SelectedValue);

            int value = 0;
            if (int.TryParse(TimeFormatList.SelectedValue, out value))
                e.InputParameters["timeFormat"] = value;
            else
                e.InputParameters["timeFormat"] = null;

            if (int.TryParse(DateFormatList.SelectedValue, out value))
                e.InputParameters["dateFormat"] = value;
            else
                e.InputParameters["dateFormat"] = null;
        }

        protected void EditForm_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (e == null) return;
            if (!UserChecked)
            {
                e.Cancel = true;

                string loginName = e.Values["Email"].ToString();
                string firstName = string.Empty;
                string middleName = string.Empty;
                string lastName = string.Empty;

                if (WebApplication.LoginProvider.LoginNameExists(loginName))
                {
                    if (WebApplication.LoginProvider.LoginInOrganization(loginName, m_UserContext.SelectedOrganizationId))
                    {
                        ErrorDiv.InnerHtml = Resources.UsersControl_ErrorMessage_UserInOrganizationExists;
                        ErrorDiv.Visible = true;

                        return;
                    }

                    ClientDataSet.UserRow userRow = UserProvider.GetUserRow(loginName);
                    if (userRow != null)
                    {
                        firstName = userRow.FirstName;
                        middleName = userRow.MiddleName;
                        lastName = userRow.LastName;
                    }
                }

                TextBoxField field = (EditForm.Fields[0] as TextBoxField);
                if (field != null)
                {
                    field.DefaultText = loginName;
                    field.ReadOnly = true;
                }

                field = (EditForm.Fields[1] as TextBoxField);
                if (field != null) field.DefaultText = firstName;

                field = (EditForm.Fields[2] as TextBoxField);
                if (field != null) field.DefaultText = middleName;

                field = (EditForm.Fields[3] as TextBoxField);
                if (field != null) field.DefaultText = lastName;

                UserChecked = true;

                EditForm_ShowDetails(true);
            }
        }

        protected void EditForm_DataBinding(object sender, EventArgs e)
        {
            if (EditForm.CurrentMode == DetailsViewMode.Edit)
                EditForm.ObjectName = Resources.UsersControl_EditForm_EditMode_ObjectName;
            else
                EditForm.ObjectName = Resources.UsersControl_EditForm_ObjectName;
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            if (EditForm.CurrentMode == DetailsViewMode.Insert)
            {
                if (!UserChecked)
                    EditForm.Rows[14].Visible = false;
            }

            string timeZoneId = null;
            string timeFormat = null;
            string dateFormat = null;

            if (EditForm.DataItem != null)
            {
                ClientDataSet.UserRow row = (ClientDataSet.UserRow)EditForm.DataItem;

                if (!row.IsTimeZoneIdNull())
                    timeZoneId = row.TimeZoneId;

                if (!row.IsTimeFormatNull())
                    timeFormat = row.TimeFormat.ToString(CultureInfo.InvariantCulture);

                if (!row.IsDateFormatNull())
                    dateFormat = row.DateFormat.ToString(CultureInfo.InvariantCulture);

                TokenControl token = (TokenControl)EditForm.FindControl("Token");
                token.LoginId = row.UserId;
            }

            BaseControl.TimeZoneListDataBind(TimeZoneList, timeZoneId, false);
            BaseControl.TimeFormatListDataBind(TimeFormatList, timeFormat, false);
            BaseControl.DateFormatListDataBind(DateFormatList, dateFormat, false);
        }

        protected void EditForm_ItemUpdated_Generic(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (e == null) return;

            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;

                MagicForm form = (sender as MagicForm);
                if (form != null)
                {
                    HtmlGenericControl errorDiv = form.FindControl(form.ID + "ErrorDiv") as HtmlGenericControl;
                    if (errorDiv != null)
                    {
                        errorDiv.InnerHtml = e.Exception.GetBaseException().Message;
                        errorDiv.Visible = true;
                    }
                }
            }
            else
                this.BackToDetailMenu();
        }

        protected void EditUserActiveForm_DataBound(object sender, EventArgs e)
        {
            EditUserActiveForm.Caption = string.Format(CultureInfo.InvariantCulture, Resources.UsersControl_EditUserActiveForm_CaptionFormat, DataBinder.Eval(EditUserGroupsForm.DataItem, "Email"));
        }

        protected void EditForm_ItemCommand_Generic(object sender, CommandEventArgs e)
        {
            if (e != null)
            {
                if (e.CommandName == "Cancel") this.BackToDetailMenu();
            }
        }

        protected void UserDetailMenu_ItemClick(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            Micajah.Common.Bll.Action action = e.CommandArgument as Micajah.Common.Bll.Action;
            if (action == null) return;

            if (action.ActionId == ActionProvider.UserNameAndEmailPageActionId)
            {
                UserDetailMenu.Visible = false;
                EditForm.Visible = true;
                EditForm_ShowDetails(true);
                EditForm.Fields[19].Visible = false;
                EditForm.ChangeMode(DetailsViewMode.Edit);
                EditForm.DataBind();
                this.AddBreadCrumbs(action, EditForm);
            }
            else if (action.ActionId == ActionProvider.UserPasswordPageActionId)
            {
                UserDetailMenu.Visible = false;
                PasswordForm.Visible = true;
                PasswordForm.LoginId = this.SelectedUserId;
                this.AddBreadCrumbs(action, PasswordForm.EditForm);
            }
            else if (action.ActionId == ActionProvider.UserGroupsPageActionId)
            {
                UserDetailMenu.Visible = false;
                EditUserGroupsForm.Visible = true;
                EditUserGroupsForm.ChangeMode(DetailsViewMode.Edit);
                EditUserGroupsForm.DataBind();
                this.AddBreadCrumbs(action, EditUserGroupsForm);
            }
            else if (action.ActionId == ActionProvider.UserActivateInactivatePageActionId)
            {
                UserDetailMenu.Visible = false;
                EditUserActiveForm.Visible = true;
                EditUserActiveForm.ChangeMode(DetailsViewMode.Edit);
                EditUserActiveForm.DataBind();
                this.AddBreadCrumbs(action, EditUserActiveForm);
            }
        }

        protected void AppGroupTreeView_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            if (e.Node.DataItem != null)
            {
                DataRowView dataSourceRow = (DataRowView)e.Node.DataItem;
                if (dataSourceRow != null)
                {
                    e.Node.ToolTip = dataSourceRow["InstancesRoles"].ToString();

                    string[] groupIds = ((Micajah.Common.Dal.ClientDataSet.UserRow)((MagicForm)((Micajah.Common.WebControls.TreeView)sender).Parent.Parent.Parent.Parent).DataItem)["GroupId"].ToString().Split(',');
                    if (groupIds.Length > 0)
                    {
                        foreach (string groupId in groupIds)
                        {
                            if (groupId == dataSourceRow["GroupId"].ToString())
                            {
                                e.Node.Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void AppGroupDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;
            e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
        }

        protected void InvitedUsersListDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            if (e == null) return;
            e.ObjectInstance = WebApplication.LoginProvider;
        }

        protected void InvitedUsersListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;
            e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
        }

        protected virtual void InvitedUsersListDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;
            ListAllowPaging(InvitedUsersList, e.ReturnValue);
        }

        protected void InviteUsersLink_Init(object sender, EventArgs e)
        {
            HyperLink lnk = sender as HyperLink;
            if (lnk != null)
            {
                lnk.Text = Resources.UsersControl_InviteUsersLink_Text;
                lnk.NavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.InviteUsersPageVirtualPath);
            }
        }

        protected void CountryList_ControlInit(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
                comboBox.DataBound += new EventHandler(CountryList_DataBound);
        }

        protected void CountryList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                using (RadComboBoxItem item = new RadComboBoxItem())
                {
                    comboBox.Items.Insert(0, item);
                }
            }
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            CheckBoxList checkBoxList = (sender as CheckBoxList);
            if (checkBoxList != null)
            {
                checkBoxList.Items.Insert(0, new ListItem(Resources.UsersControl_EditUserActiveForm_Organization, Guid.Empty.ToString()));

                ArrayList list = UserProvider.GetInstanceIdListWhereUserIsInactive((Guid)EditUserActiveForm.DataKey[0], UserContext.Current.SelectedOrganizationId);
                foreach (ListItem item in checkBoxList.Items)
                {
                    item.Selected = true;
                    object obj = Support.ConvertStringToType(item.Value, typeof(Guid));
                    if (obj != null)
                    {
                        if (list.Contains((Guid)obj))
                            item.Selected = false;
                    }
                }
            }
        }

        protected void UserGroupsDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            TreeView appGroupTreeView = EditUserGroupsForm.FindControl("AppGroupTreeView") as TreeView;
            if (appGroupTreeView != null)
            {
                string groupId = string.Empty;
                foreach (var node in appGroupTreeView.CheckedNodes)
                {
                    if (groupId.Length > 0)
                        groupId += ",";

                    groupId += node.Value;
                }
                e.InputParameters["groupId"] = groupId;
            }
        }

        protected void UserActiveDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;

            CheckBoxList instanceList = EditUserActiveForm.FindControl("InstanceList") as CheckBoxList;
            if (instanceList != null)
                e.InputParameters["instanceIdListWhereUserIsActive"] = instanceList.SelectedValue;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            List.Columns[2].HeaderText = Resources.UsersControl_List_LastLoginDateColumn_HeaderText;

            EditForm.Fields[15].HeaderText = Resources.UsersControl_EditForm_TimeZoneField_HeaderText;
            EditForm.Fields[16].HeaderText = Resources.UsersControl_EditForm_TimeFormatField_HeaderText;
            EditForm.Fields[17].HeaderText = Resources.UsersControl_EditForm_DateFormatField_HeaderText;
            EditForm.Fields[20].HeaderText = Resources.UsersControl_EditForm_TokenField_HeaderText;

            PasswordForm.ObjectName = Resources.UsersControl_PwdForm_ObjectName;
            LoadResources(EditUserGroupsForm, this.GetType().BaseType.Name);
            LoadResources(EditUserActiveForm, this.GetType().BaseType.Name);

            LoadResources(InvitedUsersList, this.GetType().BaseType.Name);
            (InvitedUsersList.Columns[0] as TextBoxField).DataFormatString = Resources.UsersControl_InvitedUsersList_LoginNameColumn_DataFormatString;
            ButtonField btn = InvitedUsersList.Columns[1] as ButtonField;
            if (btn != null)
            {
                btn.Text = Resources.UsersControl_InvitedUsersList_DeleteColumn_Text;
                btn.OnClientClick = string.Format(CultureInfo.InvariantCulture, "return confirm(\"{0}\");", Support.PreserveDoubleQuote(Resources.UsersControl_InvitedUsersList_DeleteColumn_ConfirmText));
            }
        }

        protected override void EditFormReset()
        {
            base.EditFormReset();
            TextBoxField field = (EditForm.Fields[0] as TextBoxField);
            if (field != null)
            {
                field.DefaultText = string.Empty;
                field.ReadOnly = false;
            }
            EditForm_ShowDetails(false);
            UserChecked = false;
            InvitedUsersDiv.Visible = true;
        }

        protected override void EditFormInitialize()
        {
            base.EditFormInitialize();
            EditForm.ShowCloseButton = CloseButtonVisibilityMode.Always;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Initialize(EditUserGroupsForm);
            EditUserGroupsForm.ShowCloseButton = CloseButtonVisibilityMode.Always;

            Initialize(EditUserActiveForm);
            EditUserActiveForm.ShowCloseButton = CloseButtonVisibilityMode.Always;

            Initialize(InvitedUsersList);
            InvitedUsersList.AutoGenerateEditButton = false;
            InvitedUsersList.AutoGenerateDeleteButton = false;
            InvitedUsersList.ShowAddLink = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            m_UserContext = UserContext.Current;

            base.OnLoad(e);
        }

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            object obj = DataBinder.Eval(e.Row.DataItem, "LastLoginDate");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)e.Row.FindControl("LastLoginDateLiteral");
                lit.Text = Support.ToShortDateString((DateTime)obj, m_UserContext.TimeZone);
            }
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                    PwdFormReset();
                    base.List_Action(sender, e);
                    InvitedUsersDiv.Visible = false;
                    break;
                case CommandActions.Edit:
                    PwdFormReset();
                    List.SelectedIndex = e.RowIndex;
                    List.Visible = false;
                    InvitedUsersDiv.Visible = false;
                    UserDetailMenu.Visible = true;
                    UserDetailMenu.Title = string.Format(CultureInfo.CurrentUICulture, Resources.UsersControl_UserDetailMenu_TitleFormatString, WebApplication.LoginProvider.GetEmail(this.SelectedUserId));
                    UserDetailMenu.ObjectId = this.SelectedUserId.ToString();
                    this.AddBreadCrumbs(null, null);
                    break;
            }
        }

        protected override void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            base.EditForm_ItemInserted(sender, e);

            if (e == null) return;

            if (e.Exception == null)
                this.SaveCountry();
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (e != null)
            {
                if (e.Exception != null)
                {
                    e.ExceptionHandled = true;
                    e.KeepInEditMode = true;

                    ErrorDiv.InnerHtml = e.Exception.GetBaseException().Message;
                    ErrorDiv.Visible = true;
                }
                else
                {
                    if (EditForm.CurrentMode == DetailsViewMode.Insert)
                        base.EditForm_ItemUpdated(sender, e);
                    else
                        this.BackToDetailMenu();

                    if (e.Exception == null)
                        this.SaveCountry();
                }
            }
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (EditForm.CurrentMode == DetailsViewMode.Insert)
                base.EditForm_ItemCommand(sender, e);
            else
            {
                if ((e != null) && (e.CommandName == "Cancel"))
                    this.BackToDetailMenu();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if ((ErrorDiv != null) && ErrorDiv.Visible)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ClientScripts",
                     string.Concat("window.setTimeout(\"document.getElementById('", ErrorDiv.ClientID, "').style.display = 'none'\", 10000);\r\n"),
                     true);
            }
        }

        #endregion
    }
}
