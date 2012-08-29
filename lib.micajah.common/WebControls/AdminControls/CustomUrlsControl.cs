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

        #endregion

        #region Private Properties

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((FullCustomUrlTextBox != null) && (PartialCustomUrlTextBox != null))
                {
                    sb.Append("function ValidateCustomUrls(source, arguments) { arguments.IsValid = true; ");
                    sb.AppendFormat("var elem1 = document.getElementById('{0}_txt'); ", FullCustomUrlTextBox.ClientID);
                    sb.AppendFormat("var elem2 = document.getElementById('{0}_txt'); ", PartialCustomUrlTextBox.ClientID);
                    sb.Append("if (elem1 && elem2) { arguments.IsValid = ((!StringIsEmpty(elem1.value)) || (!StringIsEmpty(elem2.value))); } }\r\n");
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

        protected override void Render(HtmlTextWriter writer)
        {
            string scripts = ClientScripts;
            if (!string.IsNullOrEmpty(scripts)) ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ValidateCustomUrlsScript", scripts, true);
            base.Render(writer);
        }

        #endregion
    }
}
