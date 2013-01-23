using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Security.Authentication;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SecurityControls
{
    /// <summary>
    /// Provides user interface (UI) elements for logging in to a Web site without typing login credentials.
    /// </summary>
    public class LogOnAsUserControl : UserControl
    {
        #region Members

        protected HtmlGenericControl ErrorDiv;
        protected Table SearchTable;
        protected Literal CaptionLiteral;
        protected Label OrganizationListLabel;
        protected ComboBox OrganizationList;
        protected TableRow InstanceListRow;
        protected Label InstanceListLabel;
        protected ComboBox InstanceList;
        protected Label RoleListLabel;
        protected ComboBox RoleList;
        protected TableCell FooterCell;
        protected PlaceHolder ButtonsHolder;
        protected Button SubmitButton;
        protected LinkButton CancelButton;
        protected PlaceHolder ButtonsSeparator;
        protected ObjectDataSource InstancesDataSource;
        protected CommonGridView List;
        protected ObjectDataSource EntityListDataSource;
        protected LinkButton InjectButton;

        private Guid m_OrgId;
        private Guid m_InstanceId;
        private Guid m_RoleId;
        private UserContext m_UserContext;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            List.Columns[2].HeaderText = Resources.LogOnAsUserControl_List_LastLoginDateColumn_HeaderText;

            InjectButton.Text = Resources.LogOnAsUserControl_InjectButton_Text;

            OrganizationListLabel.Text = Resources.LogOnAsUserControl_OrganizationListLabel_Text;
            InstanceListLabel.Text = Resources.LogOnAsUserControl_InstanceListLabel_Text;
            RoleListLabel.Text = Resources.LogOnAsUserControl_RoleListLabel_Text;

            CaptionLiteral.Text = Resources.LogOnAsUserControl_SearchTable_Caption;
            SubmitButton.Text = Resources.LogOnAsUserControl_SearchTable_Caption;
            CancelButton.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
        }

        private void ListDataBind()
        {
            InjectButton.Visible = (!WebApplication.LoginProvider.LoginIsOrganizationAdministrator(m_UserContext.UserId, m_OrgId));

            List.SelectedIndex = -1;
            List.Visible = true;
            List.DataBind();
        }

        private void ParseParams()
        {
            object obj = Support.ConvertStringToType(OrganizationList.SelectedValue, typeof(Guid));
            m_OrgId = ((obj == null) ? Guid.Empty : (Guid)obj);

            obj = Support.ConvertStringToType(InstanceList.SelectedValue, typeof(Guid));
            m_InstanceId = ((obj == null) ? Guid.Empty : (Guid)obj);

            obj = Support.ConvertStringToType(RoleList.SelectedValue, typeof(Guid));
            m_RoleId = ((obj == null) ? Guid.Empty : (Guid)obj);
        }

        private void ShowError(ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                Exception ex = e.Exception.GetBaseException();
                if ((ex is DbException) || (ex is DataException))
                {
                    e.ExceptionHandled = true;

                    List.SelectedIndex = -1;
                    List.Visible = false;

                    ErrorDiv.InnerHtml = ex.Message;
                    ErrorDiv.Visible = true;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            m_UserContext = UserContext.Current;

            this.LoadResources();
            this.ParseParams();

            MagicForm.ApplyStyle(SearchTable);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);
            Control container = MagicForm.AddRequiredTable(FooterCell);
            container.Controls.Add(ButtonsHolder);

            if (this.IsPostBack)
                BaseControl.LoadResources(List, this.GetType().BaseType.Name);
            else
                InstanceListRow.Visible = FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances;
        }

        protected void ComboBox_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.ID == InstanceList.ID)
                {
                    if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                        return;
                }
                using (RadComboBoxItem item = new RadComboBoxItem())
                {
                    comboBox.Items.Insert(0, item);
                }
            }
        }

        protected void RoleList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            InstanceList.Required = (m_RoleId != RoleProvider.OrganizationAdministratorRoleId);
        }

        protected void InstancesDataSource_Selecting(object sender, CancelEventArgs e)
        {
            if (e == null) return;

            if (m_OrgId == Guid.Empty)
            {
                e.Cancel = true;
                InstanceList.Items.Clear();
            }
        }

        protected void InstancesDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            if (e.Exception != null)
            {
                ShowError(e);
                InstanceList.Items.Clear();
            }
        }

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = m_OrgId;
            e.InputParameters["instanceId"] = m_InstanceId;
            e.InputParameters["roleId"] = m_RoleId;
        }

        protected void EntityListDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            if (e.Exception != null)
                ShowError(e);
            else
                BaseControl.ListAllowPaging(List, e.ReturnValue);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ListDataBind();
        }

        protected void InjectButton_Click(object sender, EventArgs e)
        {
            if (!WebApplication.LoginProvider.LoginIsOrganizationAdministrator(m_UserContext.UserId, m_OrgId))
            {
                string password = null;
                UserProvider.AddUserToOrganization(m_UserContext.LoginName, null, null, null
                    , null, null, null, null, null, null
                    , null, null, null, null, null
                    , null, null, null
                    , Guid.Empty.ToString(), true
                    , m_OrgId, true
                    , false, true
                    , 0, 0, out password);

                RoleList.ClearSelection();
                InstanceList.ClearSelection();
                InstanceList.Required = false;

                ParseParams();

                ListDataBind();
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            List.Visible = false;

            if (OrganizationList.Items.Count > 0)
                OrganizationList.SelectedIndex = 0;
            if (InstanceList.Items.Count > 0)
                InstanceList.SelectedIndex = 0;
            if (RoleList.Items.Count > 0)
                RoleList.SelectedIndex = 0;

            this.ParseParams();
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;

            if (e.Action == CommandActions.Select)
            {
                Guid userId = ((List.SelectedValue == null) ? Guid.Empty : (Guid)List.SelectedValue);
                List.SelectedIndex = -1;

                if ((m_OrgId != Guid.Empty) && (userId != Guid.Empty))
                {
                    string loginName = string.Empty;
                    string password = string.Empty;
                    DataRowView drv = WebApplication.LoginProvider.GetLogin(userId);
                    if (drv != null)
                    {
                        loginName = drv["LoginName"].ToString();
                        password = drv["Password"].ToString();

                        try
                        {
                            WebApplication.LoginProvider.Authenticate(loginName, password, false, false, m_OrgId, m_InstanceId);

                            string redirectUrl = null;
                            ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);

                            if (!string.IsNullOrEmpty(redirectUrl))
                                Response.Redirect(redirectUrl);
                        }
                        catch (AuthenticationException ex)
                        {
                            ErrorDiv.InnerHtml = ex.Message;
                            ErrorDiv.Visible = true;
                            return;
                        }
                    }
                }

                ErrorDiv.InnerHtml = Resources.LoginElement_FailureText;
                ErrorDiv.Visible = true;
            }
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

        #endregion
    }
}
