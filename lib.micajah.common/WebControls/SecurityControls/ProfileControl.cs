using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class ProfileControl : BaseEditFormControl
    {
        #region Members

        protected Label UserIdLabel;

        private ComboBox m_CountryList;
        private DropDownList m_TimeZoneList;
        private DropDownList m_TimeFormatList;

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

        #endregion

        #region Private Methods

        private void SaveCountry()
        {
            if (CountryList != null)
            {
                if (m_CountryList.SelectedIndex == -1)
                    CountryProvider.InsertCountry(m_CountryList.Text, false);
            }
        }

        private void Redirect()
        {
            RedirectToActionOrStartPage(ActionProvider.MyAccountPageActionId);
        }

        #endregion

        #region Protected Methods

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["timeZoneId"] = (string.IsNullOrEmpty(TimeZoneList.SelectedValue) ? null : TimeZoneList.SelectedValue);

            int value = 0;
            if (int.TryParse(TimeFormatList.SelectedValue, out value))
                e.InputParameters["timeFormat"] = value;
            else
                e.InputParameters["timeFormat"] = null;
        }

        protected void EditForm_DataBound(object sender, EventArgs e)
        {
            string timeZoneId = null;
            string timeFormat = null;

            if (EditForm.DataItem != null)
            {
                OrganizationDataSet.UserRow row = (OrganizationDataSet.UserRow)EditForm.DataItem;

                if (!row.IsTimeZoneIdNull())
                    timeZoneId = row.TimeZoneId;

                if (!row.IsTimeFormatNull())
                    timeFormat = row.TimeFormat.ToString(CultureInfo.InvariantCulture);
            }

            BaseControl.TimeZoneListDataBind(TimeZoneList, timeZoneId, false);
            BaseControl.TimeFormatsListDataBind(TimeFormatList, timeFormat, false);
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

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            EditForm.Fields[15].HeaderText = Resources.ProfileControl_EditForm_TimeZoneField_HeaderText;
            EditForm.Fields[16].HeaderText = Resources.ProfileControl_EditForm_TimeFormatField_HeaderText;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
                UserIdLabel.Text = UserContext.Current.UserId.ToString();
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            if (e == null) return;

            if (e.Exception == null)
            {
                this.SaveCountry();
                this.Redirect();
            }
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                this.Redirect();
        }

        #endregion
    }
}
