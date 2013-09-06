using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage settings by organization and/or instance administrators.
    /// </summary>
    public class SettingsControl : Micajah.Common.WebControls.SetupControls.MasterControl, IPostBackEventHandler
    {
        #region Members

        /// <summary>
        /// The identifier prefix of the control.
        /// </summary>
        protected const string ControlIdPrefix = "v";

        protected PlaceHolder PageContent;
        protected Repeater Repeater1;
        protected Button UpdateButton;
        protected PlaceHolder ButtonsSeparator;
        protected HyperLink CancelLink;
        protected DetailMenu DetailMenu1;
        protected HtmlGenericControl CommandBarDiv;

        private SettingCollection m_Settings;
        private Micajah.Common.Bll.Action m_Action;
        private UserContext m_UserContext;
        private ArrayList m_GroupIdList;
        private bool m_IsModernTheme;

        #endregion

        #region Events

        /// <summary>
        /// Occurs after an item in the control is data-bound but before it is rendered on the page.
        /// </summary>
        public event RepeaterItemEventHandler ItemDataBound;

        /// <summary>
        /// Occurs when an update setting values operation has completed.
        /// </summary>
        public event EventHandler ValuesUpdated;

        #endregion

        #region Private Properties

        private Micajah.Common.Bll.Action Action
        {
            get
            {
                if (m_Action == null)
                {
                    m_Action = ActionProvider.FindAction(this.ActionId);
                    if (m_Action == null)
                        m_Action = this.MasterPage.ActiveAction;
                }
                return m_Action;
            }
        }

        private Guid ActionId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["actionid"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        private Guid GroupId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["groupid"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        private string ToggleOnOffSwitchClientScript
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture
                    , @"function ToggleOnOffSwitch(settingId, state) {{
    var Label1 = document.getElementById('{0}' + settingId + '_0_lbl');
    var Label2 = document.getElementById('{0}' + settingId + '_1_lbl');
    if (Label1 && Label2) {{
        if (state) {{
            Label1.className = 'Rbl';
            Label2.className = 'Off';
        }}
        else {{
            Label1.className = 'On';
            Label2.className = 'Rbl';
        }}
    }}
    {1}
}}
"
                    , ControlIdPrefix, ((this.Settings.Count == 1) ? string.Format(CultureInfo.InvariantCulture, "__doPostBack('{0}', settingId)", this.UniqueID) : string.Empty));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the level of the settings to display.
        /// </summary>
        [Category("Behavior")]
        [Description("The level of the settings to display.")]
        [DefaultValue(SettingLevels.None)]
        public SettingLevels DisplayedSettingLevel
        {
            get
            {
                object obj = ViewState["DisplayedSettingLevel"];
                if (obj == null)
                {
                    if (this.Action != null)
                    {
                        if (m_Action.AuthenticationRequired)
                            obj = SettingLevels.Organization;
                        if (m_Action.InstanceRequired)
                            obj = SettingLevels.Instance;
                    }
                }
                return ((obj == null) ? SettingLevels.None : (SettingLevels)obj);
            }
            set { ViewState["DisplayedSettingLevel"] = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the instance.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid InstanceId
        {
            get
            {
                object obj = ViewState["InstanceId"];
                if (obj == null)
                {
                    obj = Support.ConvertStringToType(Request.QueryString["instanceid"], typeof(Guid));
                    if (obj == null)
                    {

                        if ((m_UserContext != null) && (m_UserContext.SelectedInstance != null))
                            obj = m_UserContext.SelectedInstance.InstanceId;
                    }
                }

                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { ViewState["InstanceId"] = value; }
        }

        /// <summary>
        /// Gets a collection of the groups identifiers.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ArrayList GroupIdList
        {
            get
            {
                if (m_GroupIdList == null) m_GroupIdList = new ArrayList();
                return m_GroupIdList;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating the control is used to diagnose the conflicting settings.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the control is used to diagnose the conflicting settings.")]
        [DefaultValue(false)]
        public bool DiagnoseConflictingSettings
        {
            get
            {
                object obj = ViewState["DiagnoseConflictingSettings"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["DiagnoseConflictingSettings"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL that the user will redirect to after saving or canceling.
        /// </summary>
        [Category("ReturnUrl")]
        [Description("Whether the control is used to diagnose the conflicting settings.")]
        [DefaultValue(false)]
        public string ReturnUrl
        {
            get
            {
                object obj = ViewState["ReturnUrl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ReturnUrl"] = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SettingCollection Settings
        {
            get
            {
                if (m_Settings == null)
                {
                    switch (this.DisplayedSettingLevel)
                    {
                        case SettingLevels.Organization:
                            m_Settings = SettingProvider.GetSettings(((this.Action == null) ? Guid.Empty : m_Action.ActionId), m_UserContext.SelectedOrganization.OrganizationId, null, true);
                            break;
                        case SettingLevels.Instance:
                            m_Settings = SettingProvider.GetSettings(((this.Action == null) ? Guid.Empty : m_Action.ActionId), m_UserContext.SelectedOrganization.OrganizationId, this.InstanceId, true);
                            break;
                        case SettingLevels.Group:
                            SettingCollection settings = null;
                            if (this.DiagnoseConflictingSettings)
                                settings = SettingProvider.GetGroupSettings(m_UserContext.SelectedOrganization.OrganizationId, this.InstanceId, this.GroupIdList);
                            else if (this.GroupId != Guid.Empty)
                                settings = SettingProvider.GetGroupSettings(m_UserContext.SelectedOrganization.OrganizationId, this.InstanceId, this.GroupId, true);

                            if (settings.Count > 0)
                            {
                                Setting parentSetting = settings[0];
                                if ((parentSetting.SettingType != SettingType.OnOffSwitch) && (parentSetting.SettingType != SettingType.NotSet))
                                {
                                    parentSetting = new Setting();
                                    parentSetting.SettingId = Guid.Empty;
                                    parentSetting.SettingType = SettingType.NotSet;
                                    parentSetting.EnableGroup = true;
                                    parentSetting.Visible = true;

                                    m_Settings = new SettingCollection();
                                    m_Settings.Add(parentSetting);

                                    settings.ChangeParent(parentSetting);
                                    parentSetting.EnsureChildSettings();
                                    parentSetting.ChildSettings.AddRange(settings);
                                    m_Settings.AddRange(parentSetting.ChildSettings);
                                }
                                else
                                    m_Settings = settings;
                            }
                            else
                                m_Settings = settings;
                            break;
                    }
                }
                return m_Settings;
            }
        }

        #endregion

        #region Private Methods

        private static Control CreateRadioButton(Setting setting, bool value, bool selectedValue)
        {
            HtmlGenericControl radioButton = null;
            HtmlGenericControl label = null;
            Control container = null;

            try
            {
                string str = setting.SettingId.ToString("N");
                string controlId = string.Concat(ControlIdPrefix, str);

                str = string.Concat("ToggleOnOffSwitch('", str, "', ", (value ? "false" : "true"), ");");

                radioButton = new HtmlGenericControl("input");
                radioButton.Attributes["type"] = "radio";
                radioButton.Attributes["onclick"] = str;
                radioButton.Attributes["name"] = controlId;
                str = controlId + (value ? "_0" : "_1");
                radioButton.Attributes["id"] = str;
                radioButton.Attributes["value"] = (value ? "true" : "false");
                if (selectedValue == value)
                    radioButton.Attributes["checked"] = "checked";

                label = new HtmlGenericControl("label");
                label.Attributes["id"] = str + "_lbl";
                label.Attributes["for"] = str;
                if (selectedValue == value)
                    label.Attributes["class"] = (value ? "On" : "Off");
                else
                    label.Attributes["class"] = "Rbl";
                label.InnerHtml = (value ? Resources.SettingsControl_OnOffSwitch_OnText : Resources.SettingsControl_OnOffSwitch_OffText) + "&nbsp;";

                container = new Control();
                container.Controls.Add(radioButton);
                container.Controls.Add(label);

                return container;
            }
            finally
            {
                if (radioButton != null) radioButton.Dispose();
                if (label != null) label.Dispose();
                if (container != null) container.Dispose();
            }
        }

        private Control CreateTextBox(Setting setting, bool diagnoseConflictingSettings)
        {
            TextBox textBox = null;
            DatePicker datePicker = null;

            try
            {
                object obj = Support.ConvertStringToType(setting.ValidationType, typeof(DatePickerType));
                if (obj != null)
                {
                    datePicker = new DatePicker();
                    datePicker.ID = string.Concat(ControlIdPrefix, setting.SettingId.ToString("N"));
                    datePicker.Type = (DatePickerType)obj;
                    datePicker.ShowRequired = false;

                    switch (datePicker.Type)
                    {
                        case DatePickerType.Date:
                        case DatePickerType.DatePicker:
                            datePicker.DateFormat = Support.GetShortDateFormat(m_UserContext.DateFormat);
                            break;
                        case DatePickerType.DateTime:
                        case DatePickerType.DateTimePicker:
                            datePicker.DateFormat = Support.GetLongDateTimeFormat(m_UserContext.TimeFormat, m_UserContext.DateFormat);
                            break;
                        case DatePickerType.Time:
                        case DatePickerType.TimePicker:
                            datePicker.DateFormat = Support.GetTimeFormat(m_UserContext.TimeFormat);
                            break;
                    }

                    DateTime value = DateTime.MinValue;
                    if (DateTime.TryParse(setting.MinimumValue, out value))
                        datePicker.MinDate = TimeZoneInfo.ConvertTimeFromUtc(value, m_UserContext.TimeZone);

                    if (DateTime.TryParse(setting.MaximumValue, out value))
                        datePicker.MaxDate = TimeZoneInfo.ConvertTimeFromUtc(value, m_UserContext.TimeZone);

                    if (setting.IsConflicting)
                    {
                        datePicker.SetDateInputValue(Resources.SettingsControl_SettingValueUndefined);
                    }
                    else
                    {
                        if (DateTime.TryParse(setting.Value, out value))
                            datePicker.SelectedDate = TimeZoneInfo.ConvertTimeFromUtc(value, m_UserContext.TimeZone);
                    }

                    if (diagnoseConflictingSettings)
                        datePicker.Enabled = false;

                    return datePicker;
                }
                else
                {
                    textBox = new TextBox();
                    textBox.ShowRequired = false;
                    textBox.Columns = 40;
                    textBox.ID = string.Concat(ControlIdPrefix, setting.SettingId.ToString("N"));
                    textBox.ValidationExpression = setting.ValidationExpression;
                    textBox.MaximumValue = setting.MaximumValue;
                    textBox.MinimumValue = setting.MinimumValue;
                    textBox.MaxLength = setting.MaxLength;

                    obj = Support.ConvertStringToType(setting.ValidationType, typeof(CustomValidationDataType));
                    if (obj != null)
                        textBox.ValidationType = (CustomValidationDataType)obj;

                    textBox.Text = (setting.IsConflicting ? Resources.SettingsControl_SettingValueUndefined : setting.Value);

                    if (diagnoseConflictingSettings)
                        textBox.Enabled = false;

                    return textBox;
                }
            }
            finally
            {
                if (textBox != null) textBox.Dispose();
                if (datePicker != null) datePicker.Dispose();
            }
        }

        private static Control CreateCheckBox(Setting setting, bool diagnoseConflictingSettings)
        {
            Control container = null;
            HtmlInputCheckBox checkBox = null;

            try
            {
                string str = setting.SettingId.ToString("N");
                string controlId = string.Concat(ControlIdPrefix, str);

                bool isChecked = false;
                if (!Boolean.TryParse(setting.Value, out isChecked))
                {
                    if (!Boolean.TryParse(setting.DefaultValue, out isChecked)) isChecked = false;
                }

                container = new Control();
                checkBox = new HtmlInputCheckBox();
                checkBox.Attributes["class"] = "Nm";
                checkBox.ID = controlId;
                checkBox.Checked = isChecked;
                if (diagnoseConflictingSettings)
                {
                    checkBox.Disabled = true;
                    checkBox.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                }
                container.Controls.Add(checkBox);

                return container;
            }
            finally
            {
                if (container != null) container.Dispose();
                if (checkBox != null) checkBox.Dispose();
            }
        }

        private static Control CreateDropDownList(Setting setting, bool diagnoseConflictingSettings)
        {
            HtmlGenericControl ctl = null;
            HtmlGenericControl select = null;

            try
            {
                string valueStr = setting.Value;
                string currentValue = null;
                select = new HtmlGenericControl("select");

                if (setting.IsConflicting)
                {
                    ctl = new HtmlGenericControl("option");
                    ctl.InnerHtml = Resources.SettingsControl_SettingValueUndefined;
                    select.Controls.Add(ctl);
                }
                else
                {
                    select.Attributes["id"] = select.Attributes["name"] = string.Concat(ControlIdPrefix, setting.SettingId.ToString("N"));

                    foreach (DataRow row in SettingProvider.GetSettingListValues(setting.SettingId).Rows)
                    {
                        currentValue = row["Value"].ToString();
                        ctl = new HtmlGenericControl("option");
                        ctl.Attributes["value"] = currentValue;
                        if (string.Compare(currentValue, valueStr, StringComparison.CurrentCulture) == 0)
                            ctl.Attributes["selected"] = "selected";
                        ctl.InnerHtml = row["Name"].ToString();
                        select.Controls.Add(ctl);
                    }
                }

                if (diagnoseConflictingSettings)
                {
                    select.Disabled = true;
                    select.Style.Add(HtmlTextWriterStyle.Color, "Gray");
                }

                return select;
            }
            finally
            {
                if (select != null) select.Dispose();
                if (ctl != null) ctl.Dispose();
            }
        }

        private string GetCheckBoxValue(string id)
        {
            string value = "false";
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.EndsWith(id, StringComparison.Ordinal))
                {
                    value = "true";
                    break;
                }
            }
            return value;
        }

        private void LoadResources()
        {
            UpdateButton.Text = MagicForm.GetUpdateButtonText(DetailsViewMode.Edit, string.Empty, InsertButtonCaptionType.Create, UpdateButtonCaptionType.Save
                , (m_IsModernTheme ? CloseButtonVisibilityMode.None : CloseButtonVisibilityMode.Edit));
            CancelLink.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
            CancelLink.Visible = true;
            ButtonsSeparator.Visible = true;
        }

        private void List_DataBind()
        {
            this.OnDataBinding(EventArgs.Empty);

            SettingCollection settings = null;
            if (this.Settings != null)
            {
                settings = m_Settings.FindChildSettings(null);
                if (!this.DiagnoseConflictingSettings)
                    settings = settings.FindAllByVisible(true);
            }
            if ((settings != null) && (settings.Count > 0))
            {
                Repeater1.DataSource = settings;

                Repeater1.DataBind();
            }
            else
            {
                PageContent.Visible = false;
            }
        }

        private void UpdateSettings()
        {
            SettingCollection settings = this.Settings;
            string value = null;

            foreach (Setting setting in settings)
            {
                value = GetSettingValue(setting.SettingId, setting.SettingType);
                if (value != null)
                    setting.Value = value;

                if (setting.ParentSetting != null)
                {
                    if ((setting.ParentSetting.SettingType == SettingType.CheckBox) || (setting.ParentSetting.SettingType == SettingType.OnOffSwitch))
                    {
                        bool boolValue = false;
                        if ((!bool.TryParse(setting.ParentSetting.Value, out boolValue)) || (!boolValue))
                            setting.Value = setting.DefaultValue;
                    }
                }
            }

            switch (this.DisplayedSettingLevel)
            {
                case SettingLevels.Organization:
                    if (this.Action != null)
                        settings.UpdateValues(m_UserContext.SelectedOrganizationId);
                    break;
                case SettingLevels.Instance:
                    if (this.Action != null)
                        settings.UpdateValues(m_UserContext.SelectedOrganizationId, m_UserContext.SelectedInstanceId);
                    break;
                case SettingLevels.Group:
                    settings.UpdateValues(m_UserContext.SelectedOrganizationId, this.InstanceId, this.GroupId);
                    break;
            }

            m_UserContext.Refresh();

            if (this.ValuesUpdated != null)
                this.ValuesUpdated(this, EventArgs.Empty);
        }

        private void CreateSettingControl(Setting setting, Control container, int visibleChildSettingsCount)
        {
            if (setting == null) return;
            if (setting.SettingId == SettingProvider.MasterPageCustomStyleSheetSettingId) return;

            switch (setting.SettingType)
            {
                case SettingType.OnOffSwitch:
                    bool value = false;
                    if (!Boolean.TryParse(setting.Value, out value))
                    {
                        if (!Boolean.TryParse(setting.DefaultValue, out value)) value = false;
                    }
                    if (m_IsModernTheme)
                    {
                        CheckBox onOffSwitch = new CheckBox();
                        onOffSwitch.ID = string.Concat(ControlIdPrefix, setting.SettingId.ToString("N"));
                        onOffSwitch.RenderingMode = CheckBoxRenderingMode.OnOffSwitch;
                        if (visibleChildSettingsCount <= 0)
                        {
                            onOffSwitch.AutoPostBack = true;
                            onOffSwitch.CheckedChanged += new EventHandler(OnOffSwitch_CheckedChanged);
                        }
                        onOffSwitch.Checked = value;
                        container.Controls.Add(onOffSwitch);
                    }
                    else
                    {
                        container.Controls.Add(CreateRadioButton(setting, true, value));
                        container.Controls.Add(CreateRadioButton(setting, false, value));
                    }
                    break;
                case SettingType.CheckBox:
                    container.Controls.Add(CreateCheckBox(setting, this.DiagnoseConflictingSettings));
                    break;
                case SettingType.Value:
                    container.Controls.Add(CreateTextBox(setting, this.DiagnoseConflictingSettings));
                    break;
                case SettingType.List:
                    container.Controls.Add(CreateDropDownList(setting, this.DiagnoseConflictingSettings));
                    break;
            }
        }

        private void EnsureActiveInstance()
        {
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && (this.Action != null))
            {
                if (m_Action.InstanceRequired)
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                    {
                        if (user.SelectedInstance == null)
                            Response.Redirect(ResourceProvider.GetActiveInstanceUrl(Request.Url.PathAndQuery));
                    }
                }
            }
        }

        private void Repeater_ItemDataBound(RepeaterItemEventArgs e, string controlHolderId)
        {
            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Control controlHolder = e.Item.FindControl(controlHolderId);
            if (controlHolder == null) return;

            SettingCollection childSettings = setting.ChildSettings;
            if (!this.DiagnoseConflictingSettings)
                childSettings = childSettings.FindAllByVisible(true);
            bool leftMargin = false;

            if (setting.SettingType != SettingType.NotSet)
            {
                CreateSettingControl(setting, controlHolder, childSettings.Count);
                leftMargin = true;
            }

            Repeater repeater2 = e.Item.FindControl("Repeater2") as Repeater;
            if (repeater2 == null) return;

            if (setting.Action != null)
            {
                if (!string.IsNullOrEmpty(setting.Action.LearnMoreUrl))
                {
                    using (HyperLink learnMoreLink = new HyperLink())
                    {
                        learnMoreLink.ID = "LearnMoreLink";
                        learnMoreLink.CssClass = "Lm";
                        if (leftMargin)
                            learnMoreLink.CssClass += " Ml";
                        learnMoreLink.Target = "_blank";
                        learnMoreLink.Text = Resources.SettingsControl_LearnMoreLink_Text;
                        learnMoreLink.NavigateUrl = setting.Action.LearnMoreUrl;
                        controlHolder.Controls.Add(learnMoreLink);
                        leftMargin = true;
                    }
                }
            }

            if ((!setting.ParentSettingId.HasValue) && (!string.IsNullOrEmpty(setting.PaidUpgradeUrl)))
            {
                using (HyperLink paidUpgradeLink = new HyperLink())
                {
                    paidUpgradeLink.ID = "PaidUpgradeLink";
                    paidUpgradeLink.CssClass = "Lm";
                    if (leftMargin)
                        paidUpgradeLink.CssClass += " Ml";
                    paidUpgradeLink.Target = "_blank";
                    paidUpgradeLink.Text = Resources.SettingsControl_PaidUpgradeLink_Text;
                    paidUpgradeLink.NavigateUrl = setting.PaidUpgradeUrl;
                    controlHolder.Controls.Add(paidUpgradeLink);
                    leftMargin = true;
                }
            }

            if (m_IsModernTheme && leftMargin)
            {
                HtmlGenericControl div = controlHolder as HtmlGenericControl;
                if (div != null)
                    div.Attributes["class"] += " Modern";
            }

            repeater2.DataSource = childSettings;
            repeater2.DataBind();
        }

        private void OnOffSwitch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateButton_Click(sender, e);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            m_IsModernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern);
            m_UserContext = UserContext.Current;

            this.LoadResources();

            if (!this.IsPostBack)
                this.EnsureActiveInstance();

            this.List_DataBind();

            if (!string.IsNullOrEmpty(this.ReturnUrl))
                CancelLink.NavigateUrl = this.ReturnUrl;

            DetailMenu1.ShowDescriptionAsToolTip = true;

            if (this.DisplayedSettingLevel != SettingLevels.Group)
            {
                if (this.MasterPage != null)
                {
                    if (this.ActionId != Guid.Empty)
                        this.MasterPage.CustomName = this.Action.CustomName;

                    Micajah.Common.Bll.Action action = this.MasterPage.ActiveAction;
                    while ((action != null) && (action.ActionId != ActionProvider.ConfigurationPageActionId))
                    {
                        action = action.ParentAction;
                        if (action != null)
                        {
                            if (!action.GroupInDetailMenu) break;
                        }
                    }

                    if (string.IsNullOrEmpty(this.ReturnUrl))
                    {
                        if (action != null)
                            CancelLink.NavigateUrl = action.CustomAbsoluteNavigateUrl;
                    }
                }

                if (this.Action == null)
                    DetailMenu1.Visible = false;
                else
                    DetailMenu1.ParentActionId = m_Action.ActionId;
            }
            else
            {
                if (string.IsNullOrEmpty(this.ReturnUrl))
                    CancelLink.NavigateUrl = string.Concat(CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.GroupsInstancesRolesPageVirtualPath), "?GroupId=", GroupId.ToString("N"));

                DetailMenu1.Visible = false;

                if (this.DiagnoseConflictingSettings)
                {
                    CommandBarDiv.Visible = false;
                }
            }

            if (m_IsModernTheme)
                CancelLink.Visible = false;
            else
                AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (!((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Repeater_ItemDataBound(e, "ControlHolder1");
        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null) return;
            if (!((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))) return;

            Setting setting = e.Item.DataItem as Setting;
            if (setting == null) return;

            Repeater_ItemDataBound(e, "ControlHolder2");

            Label label = e.Item.FindControl("NameLabel2") as Label;
            if (label != null)
            {
                label.Text = setting.CustomName;
                label.ToolTip = setting.CustomDescription;
            }

            if (this.ItemDataBound != null)
                this.ItemDataBound(sender, e);
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            this.UpdateSettings();

            if (string.IsNullOrEmpty(CancelLink.NavigateUrl) || m_IsModernTheme)
            {
                this.List_DataBind();

                if (this.Settings.Count > 1)
                {
                    if (this.MasterPage != null)
                    {
                        this.MasterPage.MessageType = NoticeMessageType.Success;
                        this.MasterPage.Message = Resources.BaseEditFormControl_SuccessMessage;
                    }
                }
            }
            else
                Response.Redirect(CancelLink.NavigateUrl);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event and registers the style sheet of the control.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ResourceProvider.RegisterStyleSheetResource(this, "Styles.Settings.css", "SettingsStyleSheet", false);
            MagicForm.RegisterStyleSheet(this.Page);

            if (!m_IsModernTheme)
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "ToggleOnOffSwitchClientScript", this.ToggleOnOffSwitchClientScript, true);

            if (this.DisplayedSettingLevel != SettingLevels.Group)
                CommandBarDiv.Visible = (this.Settings.Count > 1);
        }

        public override Control FindControl(string id)
        {
            Control ctl = base.FindControl(id);

            if (ctl == null)
            {
                foreach (RepeaterItem item1 in Repeater1.Items)
                {
                    if (ctl != null)
                        break;

                    ctl = item1.FindControl(id);

                    if (ctl == null)
                    {
                        Repeater repeater2 = item1.FindControl("Repeater2") as Repeater;
                        if (repeater2 != null)
                        {
                            foreach (RepeaterItem item2 in repeater2.Items)
                            {
                                ctl = item2.FindControl(id);
                                if (ctl != null)
                                    break;
                            }
                        }
                    }
                }
            }

            return ctl;
        }

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            this.List_DataBind();
        }

        #endregion

        #region Public Methods

        public void RaisePostBackEvent(string eventArgument)
        {
            this.UpdateSettings();

            this.List_DataBind();
        }

        public string GetSettingValue(Guid settingId, SettingType settingType)
        {
            string id = string.Concat(ControlIdPrefix, settingId.ToString("N"));
            string value = Request.Form[id];

            if (settingType == SettingType.CheckBox)
            {
                value = GetCheckBoxValue(id);
            }
            else if (settingType == SettingType.Value)
            {
                foreach (string key in Request.Form.AllKeys)
                {
                    if (key.EndsWith(id + this.IdSeparator + "txt", StringComparison.Ordinal)
                        || key.EndsWith(id + this.IdSeparator + "rdp", StringComparison.Ordinal))
                    {
                        Control ctl = Support.FindTargetControl(key.Substring(0, key.Length - 4), this, true);
                        if (ctl != null)
                        {
                            TextBox textBox = ctl as TextBox;
                            if (textBox != null)
                            {
                                value = textBox.Text;
                                break;
                            }
                            else
                            {
                                DatePicker datePicker = ctl as DatePicker;
                                if (datePicker != null)
                                {
                                    value = (datePicker.IsEmpty ? string.Empty : TimeZoneInfo.ConvertTimeToUtc(datePicker.SelectedDate, m_UserContext.TimeZone).ToString("g"));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (m_IsModernTheme && (settingType == SettingType.OnOffSwitch))
            {
                value = GetCheckBoxValue(id + this.IdSeparator + "CheckBox");
            }

            return value;
        }

        #endregion
    }
}