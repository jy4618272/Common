using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    public class ComboBoxField : BaseValidatedField, IComboBox
    {
        #region Members

        private string m_SelectedValue = string.Empty;
        private ComboBox m_InnerControl;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ComboBoxField()
        {
            m_InnerControl = new ComboBox();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of items in the list control.
        /// </summary>
        [DefaultValue(false)]
        public bool AllowCustomText
        {
            get
            {
                object obj = ViewState["AllowCustomText"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["AllowCustomText"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether list items are cleared before data binding.
        /// </summary>
        [DefaultValue(false)]
        public bool AppendDataBoundItems
        {
            get
            {
                object obj = ViewState["AppendDataBoundItems"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["AppendDataBoundItems"] = value; }
        }

        /// <summary>
        /// Gets or sets a list of separators: autocomplete logic is reset afer a separator is entered and users can autocomplete multiple items.
        /// You can use several separators at once.
        /// </summary>
        [DefaultValue("")]
        public string AutoCompleteSeparator
        {
            get
            {
                object obj = ViewState["AutoCompleteSeparator"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["AutoCompleteSeparator"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a postback to the server automatically occurs when the user changes the control selection.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                object obj = ViewState["AutoPostBack"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["AutoPostBack"] = value; }
        }

        /// <summary>
        /// Gets or sets the data sourceRow of the list that is being bound. The default value is a null reference.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return (object)ViewState["DataSource"]; }
            set { ViewState["DataSource"] = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the control from which the data-bound control retrieves its list of data items.
        /// The default is System.String.Empty.
        [Category("Data")]
        [DefaultValue("")]
        [IDReferenceProperty(typeof(DataSourceControl))]
        public string DataSourceId
        {
            get
            {
                object obj = ViewState["DataSourceId"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataSourceId"] = value; }
        }

        /// <summary>
        /// Specifies which property of a data-bound item to use when binding an item's Text property.
        /// The default is a System.String.Empty.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public virtual string DataTextField
        {
            get
            {
                object obj = ViewState["DataTextField"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataTextField"] = value; }
        }

        /// <summary>
        /// Specifies which property of a data-bound item to use when binding an item's Value property.
        /// The default is a System.String.Empty.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        public virtual string DataValueField
        {
            get
            {
                object obj = ViewState["DataValueField"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataValueField"] = value; }
        }

        /// <summary>
        /// Gets or sets a brief phrase that summarizes what a control does, for use in ToolTips and catalogs of WebPart controls.
        /// </summary>
        [DefaultValue("")]
        public string Description
        {
            get
            {
                object obj = ViewState["Description"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Description"] = value; }
        }

        /// <summary>
        /// Gets or sets the width of the dropdown in pixels.
        /// </summary>
        [DefaultValue(typeof(Unit), "")]
        public Unit DropDownWidth
        {
            get
            {
                object obj = ViewState["DropDownWidth"];
                return ((obj == null) ? Unit.Empty : (Unit)obj);
            }
            set { ViewState["DropDownWidth"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combobox should issue a callback to the server whenever end-users change the text of the combo (keypress, paste, etc).
        /// </summary>
        [DefaultValue(false)]
        public bool EnableLoadOnDemand
        {
            get
            {
                object obj = ViewState["EnableLoadOnDemand"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableLoadOnDemand"] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the external callback streamer page.
        /// </summary>
        [DefaultValue("")]
        public string ExternalCallbackPage
        {
            get
            {
                object obj = ViewState["ExternalCallBackPage"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ExternalCallBackPage"] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the page request is the result of a combobox callback.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCallback
        {
            get
            {
                object obj = ViewState["IsCallBack"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["IsCallBack"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the combobox autocompletion logic is case-sensitive or not.
        /// </summary>
        [DefaultValue(false)]
        public bool IsCaseSensitive
        {
            get
            {
                object obj = ViewState["IsCaseSensitive"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["IsCaseSensitive"] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the current instance of the combobox has child items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEmpty
        {
            get
            {
                object obj = ViewState["IsEmpty"];
                return ((obj == null) ? false : (bool)obj);
            }
        }

        /// <summary>
        /// Specifies the timeout after each keypress before RadComboBox fires an AJAX callback to the ItemsRequested server-side event.
        /// In miliseconds. ItemRequestTimeout = 500 is equal to half a second delay.
        /// </summary>
        [DefaultValue(300)]
        public int ItemRequestTimeout
        {
            get
            {
                object obj = ViewState["ItemRequestTimeout"];
                return ((obj == null) ? 300 : (int)obj);
            }
            set { ViewState["ItemRequestTimeout"] = value; }
        }

        /// <summary>
        /// Gets the collection of items.
        /// </summary>
        [MergableProperty(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Editor("Telerik.Web.Design.ControlItemCollectionEditor, Telerik.Web.Design", typeof(UITypeEditor))]
        [DefaultValue("")]
        public RadComboBoxItemCollection Items
        {
            get { return ((m_InnerControl == null) ? null : m_InnerControl.Items); }
        }

        /// <summary>
        /// Gets or sets the template for the items in the control.
        /// </summary>
        [Browsable(false)]
        [TemplateContainer(typeof(RadComboBoxItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ITemplate ItemTemplate
        {
            get { return (ITemplate)ViewState["ItemTemplate"]; }
            set { ViewState["ItemTemplate"] = value; }
        }

        /// <summary>
        /// The value of the message that is shown in RadComboBox while AJAX callback call is in effect.
        /// </summary>
        [DefaultValue("")]
        public string LoadingMessage
        {
            get
            {
                object obj = ViewState["LoadingMessage"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["LoadingMessage"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combobox should automatically autocomplete and 
        /// highlight the currently typed text to the closest item text match.
        /// </summary>
        [DefaultValue(false)]
        public bool MarkFirstMatch
        {
            get
            {
                object obj = ViewState["MarkFirstMatch"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["MarkFirstMatch"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the combobox.
        /// </summary>
        [DefaultValue(0)]
        public virtual int MaxLength
        {
            get
            {
                object obj = ViewState["MaxLength"];
                return ((obj == null) ? 0 : (int)obj);
            }
            set { ViewState["MaxLength"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in a combobox item automatically
        /// continues on the next line when it reaches the end of the dropdown.
        /// </summary>
        [DefaultValue(false)]
        public bool NoWrap
        {
            get
            {
                object obj = ViewState["NoWrap"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["NoWrap"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the page to post to from the current page when a
        /// tab from the tabstrip is clicked.
        /// </summary>
        [DefaultValue("")]
        public virtual string PostBackUrl
        {
            get
            {
                object obj = ViewState["PostBackUrl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["PostBackUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the text content of the control.
        /// </summary>
        [DefaultValue("")]
        public string Text
        {
            get
            {
                object obj = ViewState["Text"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Text"] = value; }
        }

        /// <summary>
        /// Gets or sets the value content of the control.
        /// </summary>
        [DefaultValue("")]
        public string Value
        {
            get
            {
                object obj = ViewState["Value"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Value"] = value; }
        }

        [DefaultValue("")]
        public string SelectedValue
        {
            get
            {
                object obj = ViewState["SelectedValue"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["SelectedValue"] = value; }
        }

        #endregion

        #region Events

        public event RadComboBoxSelectedIndexChangedEventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs after the server control binds to a data sourceRow.
        /// </summary>
        [Category("Data")]
        [Description("Occurs after the server control binds to a data source.")]
        public event EventHandler DataBound;

        #endregion

        #region Private Methods

        private void CopyProperties(ComboBox control)
        {
            control.Width = base.ControlStyle.Width;
            control.AllowCustomText = this.AllowCustomText;
            control.AppendDataBoundItems = this.AppendDataBoundItems;
            control.AutoCompleteSeparator = this.AutoCompleteSeparator;
            control.AutoPostBack = this.AutoPostBack;
            control.DropDownWidth = this.DropDownWidth;
            control.Enabled = (!this.ReadOnly);
            control.EnableLoadOnDemand = this.EnableLoadOnDemand;
            control.IsCaseSensitive = this.IsCaseSensitive;
            control.ItemRequestTimeout = this.ItemRequestTimeout;
            control.ItemTemplate = this.ItemTemplate;
            control.LoadingMessage = this.LoadingMessage;
            control.MarkFirstMatch = this.MarkFirstMatch;
            control.MaxLength = this.MaxLength;
            control.NoWrap = this.NoWrap;
            control.PostBackUrl = this.PostBackUrl;
            control.DataSource = this.DataSource;
            control.DataSourceID = this.DataSourceId;
            control.DataTextField = this.DataTextField;
            control.DataValueField = this.DataValueField;

            // It's important to call the method next the code above.
            CopyProperties(this, control);
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                m_SelectedValue = this.LookupStringValue(comboBox);
                comboBox.DataBound += new EventHandler(OnDataBound);
            }
        }

        private void OnDataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (!string.IsNullOrEmpty(m_SelectedValue))
                    comboBox.SelectedValue = m_SelectedValue;
                else if (comboBox.Items.Count > 0)
                    comboBox.Items[0].Selected = true;
            }

            if (this.DataBound != null) this.DataBound(sender, e);
        }

        #endregion

        #region Overriden Methods

        protected override DataControlField CreateField()
        {
            return new ComboBoxField();
        }

        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            ComboBoxField field = newField as ComboBoxField;
            if (field != null)
            {
                field.ControlStyle.Width = this.ControlStyle.Width;
                field.AllowCustomText = this.AllowCustomText;
                field.AppendDataBoundItems = this.AppendDataBoundItems;
                field.AutoCompleteSeparator = this.AutoCompleteSeparator;
                field.AutoPostBack = this.AutoPostBack;
                field.DataSource = this.DataSource;
                field.DataSourceId = this.DataSourceId;
                field.DataTextField = this.DataTextField;
                field.DataValueField = this.DataValueField;
                field.Description = this.Description;
                field.DropDownWidth = this.DropDownWidth;
                field.EnableLoadOnDemand = this.EnableLoadOnDemand;
                field.ExternalCallbackPage = this.ExternalCallbackPage;
                field.IsCallback = this.IsCallback;
                field.ItemRequestTimeout = this.ItemRequestTimeout;
                field.ItemTemplate = this.ItemTemplate;
                field.LoadingMessage = this.LoadingMessage;
                field.MarkFirstMatch = this.MarkFirstMatch;
                field.MaxLength = this.MaxLength;
                field.NoWrap = this.NoWrap;
                field.PostBackUrl = this.PostBackUrl;
                field.Text = this.Text;
                field.Value = this.Value;
                field.SelectedValue = this.SelectedValue;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            string value = string.Empty;
            ComboBox comboBox = control as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.SelectedIndex > -1)
                    value = comboBox.SelectedValue;
                else if (comboBox.AllowCustomText)
                    value = comboBox.Text;
            }
            return value;
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            this.CopyProperties(m_InnerControl);
            m_InnerControl.Init += OnControlInit;

            if (!(this.EditMode || this.InsertMode))
                m_InnerControl.Enabled = m_InnerControl.Required = false;

            if (base.Visible)
            {
                m_InnerControl.DataBinding += new EventHandler(this.OnBindingField);
                if (this.SelectedIndexChanged != null) m_InnerControl.SelectedIndexChanged += this.SelectedIndexChanged;
            }
            else
                m_InnerControl.Required = false;

            if (cell != null)
                cell.Controls.Add(m_InnerControl);
        }

        #endregion
    }
}
