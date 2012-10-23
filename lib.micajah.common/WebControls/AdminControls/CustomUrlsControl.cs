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

        private ComboBox m_InstanceList;
        private Label m_NameLabel;
        private TextBox m_FullCustomUrlTextBox;
        private TextBox m_PartialCustomUrlTextBox;
        private ComboBox m_RootAddressesList;

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
                foreach (string adress in FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses)
                {
                    using (RadComboBoxItem item = new RadComboBoxItem(adress, adress))
                    {
                        if (partialCustomUrl.EndsWith(adress, StringComparison.OrdinalIgnoreCase))
                        {
                            item.Selected = true;
                            PartialCustomUrlTextBox.Text = partialCustomUrl.ToLower(CultureInfo.CurrentCulture).Split(new string[] { adress }, StringSplitOptions.None)[0].TrimEnd('.');
                        }
                        RootAddressesList.Items.Add(item);
                    }
                }
            }
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            m_InstanceList = sender as ComboBox;
            if (m_InstanceList != null)
            {
                using (RadComboBoxItem item = new RadComboBoxItem(UserContext.Current.SelectedOrganization.Name, string.Empty))
                {
                    m_InstanceList.Items.Insert(0, item);
                }
                m_InstanceList.SelectedIndex = 0;
            }
        }

        protected void InstanceListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;

                Guid? instanceId = null;
                if (this.InstanceList != null)
                {
                    object obj = Support.ConvertStringToType(m_InstanceList.SelectedValue, typeof(Guid));
                    if (obj != null) instanceId = (Guid)obj;
                }
                e.InputParameters["instanceId"] = instanceId;
                e.InputParameters["partialCustomUrl"] = PartialCustomUrlTextBox.Text.ToLower(CultureInfo.CurrentCulture) + "." + RootAddressesList.SelectedValue;
            }
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["partialCustomUrl"] = (string.IsNullOrEmpty(PartialCustomUrlTextBox.Text) ? null : (PartialCustomUrlTextBox.Text.ToLower(CultureInfo.CurrentCulture) + "." + RootAddressesList.SelectedValue));
            }
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
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;            
        }

        protected void EntityDataSourceSimpleView_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;

                if (UserContext.SelectedInstanceId != Guid.Empty)
                    e.InputParameters["instanceId"] = UserContext.SelectedInstanceId;
                else
                    e.InputParameters["instanceId"] = null;

                e.InputParameters["partialCustomUrl"] = VanityUrlTextbox.Text.ToLower(CultureInfo.CurrentCulture) + "." + VanityUrlDomainLabel.Text;
            }
        }

        protected void EntityDataSourceSimpleView_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["partialCustomUrl"] = (string.IsNullOrEmpty(VanityUrlTextbox.Text) ? null : (VanityUrlTextbox.Text.ToLower(CultureInfo.CurrentCulture) + "." + VanityUrlDomainLabel.Text));            
        }

        protected void SimpleViewSaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses.Count > 0)
                    {
                        string address = FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses[0];

                        Guid orgId = UserContext.Current.SelectedOrganization.OrganizationId;
                        Guid? instId = null;
                        if (UserContext.Current.SelectedInstance != null)
                            instId = UserContext.Current.SelectedInstance.InstanceId;

                        CommonDataSet.CustomUrlRow row = null;

                        if (UserContext.Current.SelectedInstance != null)
                            row = CustomUrlProvider.GetCustomUrl(UserContext.Current.SelectedOrganization.OrganizationId, UserContext.Current.SelectedInstance.InstanceId);

                        if (row == null)
                            row = CustomUrlProvider.GetCustomUrlByOrganizationId(UserContext.Current.SelectedOrganization.OrganizationId);

                        if (row == null)
                        {
                            System.Data.DataView table = CustomUrlProvider.GetCustomUrls(UserContext.Current.SelectedOrganization.OrganizationId);
                            if (table != null && table.Table.Rows.Count > 0)
                                row = table.Table.Rows[0] as CommonDataSet.CustomUrlRow;
                        }


                        if (row != null)
                            CustomUrlProvider.UpdateCustomUrl(row.CustomUrlId, null, VanityUrlTextbox.Text.ToLower(CultureInfo.CurrentCulture) + "." + address);
                        else
                            CustomUrlProvider.InsertCustomUrl(orgId, instId, null, VanityUrlTextbox.Text.ToLower(CultureInfo.CurrentCulture) + "." + address);

                        SimpleViewTitleLabel.Text = Resources.CustomUrlsControl_SimpleViewMessageLabel_Text;

                        Micajah.Common.Application.WebApplication.RefreshAllData();
                    }
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

            CustomUrlsMultiView.ActiveViewIndex = 0;
            ChangeViewButton.Text = Resources.CustomUrlsControl_ChangeToAdvancedView_Text;

            SimpleViewTitleLabel.Text = Resources.CustomUrlsControl_SimpleViewTitleLabel_Text;
            SimpleViewSaveButton.Text = Resources.CustomUrlsControl_SimpleViewSaveButton_Text;
            SimpleViewCustomValidator.ErrorMessage = Resources.CustomUrlsControl_CustomUrlsSimpleValidator_ErrorMessage;

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses.Count > 0)
            {
                string address = FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses[0];
                VanityUrlDomainLabel.Text = string.Format(".{0}", address);

                CustomUrlsMultiView.ActiveViewIndex = 0;

                if (!Page.IsPostBack)
                {
                    CommonDataSet.CustomUrlRow row = null;

                    if (UserContext.Current.SelectedInstance != null)
                        row = CustomUrlProvider.GetCustomUrl(UserContext.Current.SelectedOrganization.OrganizationId, UserContext.Current.SelectedInstance.InstanceId);
                    
                    if (row == null)                    
                        row = CustomUrlProvider.GetCustomUrlByOrganizationId(UserContext.Current.SelectedOrganization.OrganizationId);

                    if (row == null)
                    {
                        System.Data.DataView table = CustomUrlProvider.GetCustomUrls(UserContext.Current.SelectedOrganization.OrganizationId);
                        if (table != null && table.Table.Rows.Count > 0)
                            row = table.Table.Rows[0] as CommonDataSet.CustomUrlRow; 
                    }

                    if (row != null)
                    {
                        string partialCustomUrl = row.PartialCustomUrl;

                        if (partialCustomUrl.EndsWith(address, StringComparison.OrdinalIgnoreCase))
                        {
                            VanityUrlTextbox.Text = partialCustomUrl.ToLower(CultureInfo.CurrentCulture).Split(new string[] { address }, StringSplitOptions.None)[0].TrimEnd('.');
                        }
                    }
                }
            }
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

        #endregion
    }
}
