using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// Provides the UI elements for the diagnostic conflicts between settings values of the groups in the instances.
    /// </summary>
    public class SettingsDiagnosticControl : UserControl
    {
        #region Members

        protected Table FormTable;
        protected Literal CaptionLiteral;
        protected TableRow UserListRow;
        protected TableRow GroupListRow;
        protected TableRow InstanceListRow;
        protected HtmlTableCell RequiredCell;
        protected Label UserListLabel;
        protected Label GroupListLabel;
        protected Label InstanceListLabel;
        protected CheckBoxList GroupList;
        protected DropDownList UserList;
        protected DropDownList InstanceList;
        protected TableCell ButtonHeaderCell;
        protected TableCell ButtonCell;
        protected LinkButton SwitchButton;
        protected Button SubmitButton;
        protected PlaceHolder ButtonsSeparator;
        protected HyperLink CancelLink;
        protected SettingsControl Settings;
        protected ObjectDataSource UserListDataSource;
        protected ObjectDataSource GroupListDataSource;
        protected ObjectDataSource InstanceListDataSource;

        #endregion

        #region Private Properties

        private ArrayList GroupIdArrayList
        {
            get
            {
                ArrayList list = new ArrayList();
                object obj = null;
                foreach (ListItem item in GroupList.Items)
                {
                    obj = Support.ConvertStringToType(item.Value, typeof(Guid));
                    if (obj != null) list.Add(obj);
                }
                return list;
            }
        }

        #endregion

        #region Private Methods

        private void ApplyFilters()
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            UserContext user = UserContext.Current;
            if ((user != null) && (user.SelectedOrganizationId != Guid.Empty))
            {
                if (!user.IsOrganizationAdministrator)
                {
                    OrganizationDataSet dataSet = WebApplication.GetOrganizationDataSetByOrganizationId(user.SelectedOrganizationId);
                    ClientDataSet.UserRow userRow = UserProvider.GetUserRow(user.UserId, user.SelectedOrganizationId);

                    if (userRow != null)
                    {
                        ArrayList userGroups = user.GroupIdList;
                        OrganizationDataSet.GroupsInstancesRolesDataTable girTable = dataSet.GroupsInstancesRoles;

                        foreach (OrganizationDataSet.GroupRow groupRow in dataSet.Group)
                        {
                            foreach (OrganizationDataSet.InstanceRow instanceRow in dataSet.Instance)
                            {
                                OrganizationDataSet.GroupsInstancesRolesRow girRow = girTable.FindByGroupIdInstanceId(groupRow.GroupId, instanceRow.InstanceId);
                                if (girRow != null)
                                {
                                    if (girRow.RoleId == RoleProvider.InstanceAdministratorRoleId)
                                    {
                                        if (!userGroups.Contains(groupRow.GroupId))
                                        {
                                            sb1.AppendFormat(",'{0}'", instanceRow.InstanceId);
                                            sb2.AppendFormat(",'{0}'", groupRow.GroupId);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (sb1.Length > 0)
                    {
                        sb1.Remove(0, 1);
                        sb1.Append(")");
                        sb1.Insert(0, "CONVERT(InstanceId, 'System.String') NOT IN (");
                        InstanceListDataSource.FilterExpression = sb1.ToString();
                    }

                    if (sb2.Length > 0)
                    {
                        sb2.Remove(0, 1);
                        sb2.Append(")");
                        sb2.Insert(0, "CONVERT(GroupId, 'System.String') NOT IN (");
                        GroupListDataSource.FilterExpression = sb2.ToString();
                    }
                }
            }
        }

        private void LoadResources()
        {
            this.LoadObjectName();

            UserListLabel.Text = Resources.SettingsDiagnosticControl_UserListLabel_Text;
            GroupListLabel.Text = Resources.SettingsDiagnosticControl_GroupListLabel_Text;
            InstanceListLabel.Text = Resources.SettingsDiagnosticControl_InstanceListLabel_Text;
            CancelLink.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
        }

        private void LoadObjectName()
        {
            if (GroupListRow.Visible)
            {
                SwitchButton.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsDiagnosticControl_SwitchButton_Text, Resources.SettingsDiagnosticControl_FormTable_ObjectName_UserSettings);
                SubmitButton.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsDiagnosticControl_SubmitButton_Text, Resources.SettingsDiagnosticControl_FormTable_ObjectName_GroupsSettings);
            }
            else if (UserListRow.Visible)
            {
                SwitchButton.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsDiagnosticControl_SwitchButton_Text, Resources.SettingsDiagnosticControl_FormTable_ObjectName_GroupsSettings);
                SubmitButton.Text = string.Format(CultureInfo.CurrentCulture, Resources.SettingsDiagnosticControl_SubmitButton_Text, Resources.SettingsDiagnosticControl_FormTable_ObjectName_UserSettings);
            }
            CaptionLiteral.Text = SubmitButton.Text;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeAdminPage(this.Page);

            if (!IsPostBack)
            {
                this.LoadResources();
                UserListRow.Visible = false;

                Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.ConfigurationPageActionId);
                if (action != null)
                    CancelLink.NavigateUrl = action.CustomAbsoluteNavigateUrl;

                this.ApplyFilters();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            MagicForm.ApplyStyle(FormTable);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                CancelLink.Visible = false;
            else
            {
                HtmlTable table = MagicForm.RequiredTable;
                if (table != null)
                    RequiredCell.Controls.Add(table);
                else
                    RequiredCell.Visible = false;
                AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);
            }

            if (InstanceListRow.Visible)
                InstanceListRow.Visible = FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                ButtonHeaderCell.Visible = true;
                ButtonCell.ColumnSpan = 0;
            }
            else
            {
                ButtonHeaderCell.Visible = false;
                ButtonCell.ColumnSpan = 2;
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            object obj = null;
            ArrayList list = null;
            if (GroupListRow.Visible)
                list = GroupIdArrayList;
            else if (UserListRow.Visible)
            {
                obj = Support.ConvertStringToType(UserList.SelectedValue, typeof(Guid));
                list = UserProvider.GetUserGroupIdList(UserContext.Current.SelectedOrganizationId, ((obj == null) ? Guid.Empty : (Guid)obj));
            }

            Settings.GroupIdList.Clear();
            if (list != null) Settings.GroupIdList.AddRange(list);
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
            {
                obj = Support.ConvertStringToType(InstanceList.SelectedValue, typeof(Guid));
                Settings.InstanceId = ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            else
            {
                Instance firstInstance = InstanceProvider.GetFirstInstance();
                if (firstInstance != null) Settings.InstanceId = firstInstance.InstanceId;
            }
            Settings.Visible = true;
            Settings.DataBind();
        }

        protected void SwitchButton_Click(object sender, EventArgs e)
        {
            if (GroupListRow.Visible)
            {
                UserListRow.Visible = true;
                GroupListRow.Visible = false;
                UserList.ClearSelection();
            }
            else if (UserListRow.Visible)
            {
                UserListRow.Visible = false;
                GroupListRow.Visible = true;
                GroupList.ClearSelection();
            }

            Settings.GroupIdList.Clear();
            Settings.InstanceId = Guid.Empty;
            Settings.Visible = false;

            LoadObjectName();
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            if (InstanceList.Items.Count == 1)
                InstanceListRow.Visible = InstanceList.Visible = false;
        }

        #endregion
    }
}
