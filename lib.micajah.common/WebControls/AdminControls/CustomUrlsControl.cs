using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage custom URLs.
    /// </summary>
    public class CustomUrlsControl : BaseControl
    {
        #region Members

        protected Panel DnsAddressPanel;
        protected Label DnsAddressCaptionLabel;
        protected Label DnsAddressLabel;
        protected ObjectDataSource InstanceListDataSource;

        protected MultiView CustomUrlsMultiView;
        protected View SimpleView;
        protected View AdvancedView;
        protected LinkButton ChangeViewButton;

        protected Literal SimpleViewTitleLabel;
        protected TextBox VanityUrlTextbox;
        protected Label VanityUrlDomainLabel;
        protected Button SimpleViewSaveButton;
        protected System.Web.UI.HtmlControls.HtmlGenericControl SimpleErrorDiv;
        protected CustomValidator SimpleViewCustomValidator;

        private ComboBox m_InstanceList;
        private Label m_NameLabel;
        private TextBox m_FullCustomUrlTextBox;
        private TextBox m_PartialCustomUrlTextBox;
        private ComboBox m_RootAddressesList;

        private UserContext m_UserContext;

        #endregion

        #region Events

        public event EventHandler SaveButtonClick;

        #endregion

        #region Private Properties

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (CustomUrlsMultiView.ActiveViewIndex == 0)
                {
                    sb.Append("function ValidateCustomUrls(source, arguments) { arguments.IsValid = true; ");
                    sb.AppendFormat("var elem1 = document.getElementById('{0}_txt'); ", VanityUrlTextbox.ClientID);
                    sb.Append("if (elem1) { arguments.IsValid = (!StringIsEmpty(elem1.value)); } }\r\n");
                }
                else
                {
                    if ((FullCustomUrlTextBox != null) && (PartialCustomUrlTextBox != null))
                    {
                        sb.Append("function ValidateCustomUrls(source, arguments) { arguments.IsValid = true; ");
                        sb.AppendFormat("var elem1 = document.getElementById('{0}_txt'); ", FullCustomUrlTextBox.ClientID);
                        sb.AppendFormat("var elem2 = document.getElementById('{0}_txt'); ", PartialCustomUrlTextBox.ClientID);
                        sb.Append("if (elem1 && elem2) { arguments.IsValid = ((!StringIsEmpty(elem1.value)) || (!StringIsEmpty(elem2.value))); } }\r\n");
                    }
                }
                return sb.ToString();
            }
        }

        private TextBox FullCustomUrlTextBox
        {
            get
            {
                if (m_FullCustomUrlTextBox == null) m_FullCustomUrlTextBox = EditForm.FindControl("FullCustomUrlTextBox") as TextBox;
                return m_FullCustomUrlTextBox;
            }
        }

        private TextBox PartialCustomUrlTextBox
        {
            get
            {
                if (m_PartialCustomUrlTextBox == null) m_PartialCustomUrlTextBox = EditForm.FindControl("PartialCustomUrlTextBox") as TextBox;
                return m_PartialCustomUrlTextBox;
            }
        }

        private ComboBox InstanceList
        {
            get
            {
                if (m_InstanceList == null) m_InstanceList = EditForm.FindControl("InstanceList") as ComboBox;
                return m_InstanceList;
            }
        }

        private Label NameLabel
        {
            get
            {
                if (m_NameLabel == null) m_NameLabel = EditForm.FindControl("NameLabel") as Label;
                return m_NameLabel;
            }
        }

        private ComboBox RootAddressesList
        {
            get
            {
                if (m_RootAddressesList == null) m_RootAddressesList = EditForm.FindControl("RootAddressesList") as ComboBox;
                return m_RootAddressesList;
            }
        }

        #endregion

        #region Protected Properties

        protected static string CustomUrlsValidatorErrorMessage
        {
            get { return Resources.CustomUrlsControl_CustomUrlsValidator_ErrorMessage; }
        }

        #endregion

        #region Public Properties

        public bool ShowSwitchViewButton { get; set; }
        public bool ShowSavedMessage { get; set; }

        #endregion

        #region Private Methods

        private void SelectInstance(Guid? instanceId)
        {
            if (instanceId.HasValue)
            {
                InstanceList.Visible = true;
                InstanceList.Enabled = false;
                InstanceList.SelectedValue = instanceId.Value.ToString();
            }
            else
            {
                InstanceList.Visible = false;
                InstanceList.Enabled = true;
                InstanceList.SelectedIndex = -1;
            }
        }

        #endregion

        #region Protected Methods

        protected void EditForm_DataBound(object sender, System.EventArgs e)
        {
            Guid? instanceId = null;
            string partialCustomUrl = string.Empty;
            if (EditForm.CurrentMode == DetailsViewMode.Edit)
            {
                CommonDataSet.CustomUrlRow row = EditForm.DataItem as CommonDataSet.CustomUrlRow;
                if (row != null)
                {
                    if (!row.IsInstanceIdNull()) instanceId = row.InstanceId;
                    if (instanceId.HasValue)
                    {
                        NameLabel.Visible = false;
                        EditForm.ObjectName = Resources.CustomUrlsControl_EditForm_ObjectName_Instance;
                    }
                    else
                    {
                        NameLabel.Visible = true;
                        NameLabel.Text = OrganizationProvider.GetName(row.OrganizationId);
                        EditForm.ObjectName = Resources.CustomUrlsControl_EditForm_ObjectName_Organization;
                    }
                    partialCustomUrl = row.PartialCustomUrl;
                    this.SelectInstance(instanceId);
                }
            }

            if (EditForm.CurrentMode != DetailsViewMode.ReadOnly)
            {
                string adress = FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst;
                using (RadComboBoxItem item = new RadComboBoxItem(adress, adress))
                {
                    item.Selected = true;
                    RootAddressesList.Items.Add(item);
                }
            }
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            m_InstanceList = sender as ComboBox;
            if (m_InstanceList != null)
            {
                using (RadComboBoxItem item = new RadComboBoxItem(m_UserContext.SelectedOrganization.Name, string.Empty))
                {
                    m_InstanceList.Items.Insert(0, item);
                }
                m_InstanceList.SelectedIndex = 0;
            }
        }

        protected void InstanceListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
        }

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;

                Guid? instanceId = null;
                if (this.InstanceList != null)
                {
                    object obj = Support.ConvertStringToType(m_InstanceList.SelectedValue, typeof(Guid));
                    if (obj != null) instanceId = (Guid)obj;
                }
                e.InputParameters["instanceId"] = instanceId;
                e.InputParameters["partialCustomUrl"] = PartialCustomUrlTextBox.Text.ToLowerInvariant();
            }
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["partialCustomUrl"] = string.IsNullOrEmpty(PartialCustomUrlTextBox.Text) ? null : PartialCustomUrlTextBox.Text.ToLowerInvariant();
        }

        protected void ChangeViewButton_Click(object sender, EventArgs e)
        {
            if (CustomUrlsMultiView.ActiveViewIndex == 0)
            {
                CustomUrlsMultiView.SetActiveView(AdvancedView);
                ChangeViewButton.Text = Resources.CustomUrlsControl_ChangeToSimpleView_Text;
            }
            else
            {
                CustomUrlsMultiView.SetActiveView(SimpleView);
                ChangeViewButton.Text = Resources.CustomUrlsControl_ChangeToAdvancedView_Text;
            }
        }

        protected void EntityDataSourceSimpleView_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = m_UserContext.SelectedOrganizationId;
        }

        protected void EntityDataSourceSimpleView_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                UserContext user = m_UserContext;
                e.InputParameters["organizationId"] = user.SelectedOrganizationId;

                if (user.SelectedInstanceId != Guid.Empty)
                    e.InputParameters["instanceId"] = user.SelectedInstanceId;
                else
                    e.InputParameters["instanceId"] = null;

                e.InputParameters["partialCustomUrl"] = VanityUrlTextbox.Text.ToLowerInvariant();
            }
        }

        protected void EntityDataSourceSimpleView_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["partialCustomUrl"] = string.IsNullOrEmpty(VanityUrlTextbox.Text) ? null : VanityUrlTextbox.Text.ToLowerInvariant();
        }

        protected void SimpleViewSaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Guid orgId = m_UserContext.SelectedOrganization.OrganizationId;
                    Guid? instId = null;
                    if (m_UserContext.SelectedInstance != null)
                        instId = m_UserContext.SelectedInstance.InstanceId;

                    CommonDataSet.CustomUrlRow row = null;

                    if (instId.HasValue)
                        row = CustomUrlProvider.GetCustomUrl(orgId, instId.Value);

                    if (row == null)
                        row = CustomUrlProvider.GetCustomUrlByOrganizationId(orgId);

                    if (row != null)
                        CustomUrlProvider.UpdateCustomUrl(row.CustomUrlId, null, VanityUrlTextbox.Text.ToLowerInvariant());
                    else
                        CustomUrlProvider.InsertCustomUrl(orgId, instId, null, VanityUrlTextbox.Text.ToLowerInvariant());

                    SimpleViewTitleLabel.Text = Resources.CustomUrlsControl_SimpleViewMessageLabel_Text;

                    Micajah.Common.Application.WebApplication.RefreshAllData();

                    if (SaveButtonClick != null)
                        SaveButtonClick(sender, e);
                }
                catch (Exception ex)
                {
                    BaseControl.ShowError(ex, SimpleErrorDiv);
                }
            }
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            DnsAddressCaptionLabel.Text = Resources.CustomUrlsControl_DnsAddressCaptionLabel_Text;
            DnsAddressLabel.Text = FrameworkConfiguration.Current.WebApplication.DnsAddress;

            EditForm.Fields[0].HeaderText = Resources.CustomUrlsControl_EditForm_NameField_HeaderText;
            EditForm.Fields[1].HeaderText = Resources.CustomUrlsControl_EditForm_FullCustomUrlField_HeaderText;
            EditForm.Fields[2].HeaderText = Resources.CustomUrlsControl_EditForm_PartialCustomUrlField_HeaderText;

            ChangeViewButton.Text = Resources.CustomUrlsControl_ChangeToAdvancedView_Text;

            SimpleViewTitleLabel.Text = this.ShowSavedMessage ? Resources.CustomUrlsControl_SimpleViewMessageLabel_Text : Resources.CustomUrlsControl_SimpleViewTitleLabel_Text;
            SimpleViewSaveButton.Text = Resources.CustomUrlsControl_SimpleViewSaveButton_Text;
            SimpleViewCustomValidator.ErrorMessage = Resources.CustomUrlsControl_CustomUrlsSimpleValidator_ErrorMessage;

            VanityUrlDomainLabel.Text = string.Format(CultureInfo.InvariantCulture, ".{0}", FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst);

            CustomUrlsMultiView.ActiveViewIndex = 0;

            ChangeViewButton.Visible = this.ShowSwitchViewButton;
        }

        protected override void EditFormReset()
        {
            base.EditFormReset();
            DnsAddressPanel.Visible = true;
            EditForm.ObjectName = Resources.CustomUrlsControl_EditForm_ObjectName;
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            base.List_Action(sender, e);

            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                case CommandActions.Edit:
                    DnsAddressPanel.Visible = false;
                    break;
            }
        }

        protected override void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            base.EditForm_ItemInserted(sender, e);
            Micajah.Common.Application.WebApplication.RefreshAllData();
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);
            Micajah.Common.Application.WebApplication.RefreshAllData();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string scripts = ClientScripts;
            if (!string.IsNullOrEmpty(scripts)) ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ValidateCustomUrlsScript", scripts, true);
            base.Render(writer);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_UserContext = UserContext.Current;

            if (!Page.IsPostBack)
            {
                CommonDataSet.CustomUrlRow row = null;

                if (m_UserContext.SelectedInstance != null)
                    row = CustomUrlProvider.GetCustomUrl(m_UserContext.SelectedOrganization.OrganizationId, m_UserContext.SelectedInstance.InstanceId);

                if (row == null)
                    row = CustomUrlProvider.GetCustomUrlByOrganizationId(m_UserContext.SelectedOrganization.OrganizationId);

                if (row != null)
                    VanityUrlTextbox.Text = row.PartialCustomUrl.ToLowerInvariant();
            }
        }

        #endregion
    }
}
