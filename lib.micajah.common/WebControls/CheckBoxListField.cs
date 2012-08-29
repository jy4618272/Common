using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a field that is displayed as a multi selection check box group in a data-bound control.
    /// Can be dynamically created by binding the control to a data source.
    /// </summary>
    public class CheckBoxListField : BaseValidatedField
    {
        #region Members

        private string m_SelectedValue = string.Empty;
        private CheckBoxList m_InnerControl;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CheckBoxListField()
        {
            m_InnerControl = new CheckBoxList();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether list items are cleared before data binding.
        /// </summary>
        [DefaultValue(false)]
        [Description("Whether list items are cleared before data binding.")]
        [Category("Behavior")]
        [Themeable(false)]
        public virtual bool AppendDataBoundItems
        {
            get
            {
                object obj = ViewState["AppendDataBoundItems"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["AppendDataBoundItems"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a postback to the server automatically occurs when the user changes the list selection.
        /// The default is false.
        /// </summary>
        [Category("Behavior")]
        [Description("Automatically posts back to the server after selection is changed.")]
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
        /// Gets or sets the name of the list of data that the data-bound control binds
        /// to, in cases where the data source contains more than one distinct list of data items.
        /// The default is System.String.Empty.
        /// </summary>
        [Category("Data")]
        [Description("The table or view used for binding against.")]
        [DefaultValue("")]
        public string DataMember
        {
            get
            {
                object obj = ViewState["DataMember"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataMember"] = value; }
        }

        /// <summary>
        /// Gets or sets the object from which the data-bound control retrieves its list of data items.
        /// </summary>
        [Browsable(false)]
        public object DataSource
        {
            get
            {
                object obj = ViewState["DataSource"];
                return ((obj == null) ? null : obj);
            }
            set { ViewState["DataSource"] = value; }
        }

        /// <summary>
        /// Gets or sets the ID of the control from which the data-bound control retrieves its list of data items.
        /// The default is System.String.Empty.
        /// </summary>
        [Category("Data")]
        [Description("The control ID of an IDataSource that will be used as the data source.")]
        [DefaultValue("")]
        [IDReferenceProperty(typeof(DataSourceControl))]
        public string DataSourceId
        {
            get
            {
                object obj = ViewState["DataSourceID"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataSourceID"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the text content of the list items.
        /// The default is System.String.Empty.
        /// </summary>
        [Category("Data")]
        [Description("The field in the data source which provides the item text.")]
        [DefaultValue("")]
        public string DataTextField
        {
            get
            {
                object obj = ViewState["DataTextField"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataTextField"] = value; }
        }

        /// <summary>
        /// Gets or sets the formatting string used to control how data bound to the list control is displayed.
        /// The default is System.String.Empty.
        /// </summary>
        [Category("Data")]
        [Description("The formatting applied to the text field. For example, \"{0:d}\"")]
        [DefaultValue("")]
        public string DataTextFormatString
        {
            get
            {
                object obj = ViewState["DataTextFormatString"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataTextFormatString"] = value; }
        }

        /// <summary>
        /// Gets or sets the field of the data source that provides the value of each list item.
        /// The default is System.String.Empty.
        /// </summary>
        [Category("Data")]
        [Description("The field in the data source which provides the item value.")]
        [DefaultValue("")]
        public string DataValueField
        {
            get
            {
                object obj = ViewState["DataValueField"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DataValueField"] = value; }
        }

        /// <summary>
        /// Gets or sets the distance (in pixels) between the border and the contents of the table cell.
        /// The default is -1, which indicates that this property is not set.
        /// </summary>
        [Category("Layout")]
        [Description("The padding between each item.")]
        [DefaultValue(-1)]
        public int CellPadding
        {
            get
            {
                object obj = ViewState["CellPadding"];
                return ((obj == null) ? -1 : (int)obj);
            }
            set { ViewState["CellPadding"] = value; }
        }

        /// <summary>
        /// Gets or sets the distance (in pixels) between adjacent table cells.
        /// The default is -1, which indicates that this property is not set.
        /// </summary>
        [Category("Layout")]
        [Description("The spacing between each item.")]
        [DefaultValue(-1)]
        public int CellSpacing
        {
            get
            {
                object obj = ViewState["CellSpacing"];
                return ((obj == null) ? -1 : (int)obj);
            }
            set { ViewState["CellSpacing"] = value; }
        }

        /// <summary>
        /// Gets the collection of items.
        /// </summary>
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Category("Default")]
        [Editor(typeof(System.Web.UI.Design.WebControls.ListItemsCollectionEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        [Description("The collection of items.")]
        public virtual ListItemCollection Items
        {
            get { return ((m_InnerControl == null) ? null : m_InnerControl.Items); }
        }

        /// <summary>
        /// Gets or sets the number of columns to display in the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        [Category("Layout")]
        [Description("The number of columns used to lay out the items.")]
        [DefaultValue(0)]
        public int RepeatColumns
        {
            get
            {
                object obj = ViewState["RepeatColumns"];
                return ((obj == null) ? 0 : (int)obj);
            }
            set
            {
                if (value >= 0) ViewState["RepeatColumns"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the direction in which the radio buttons within the group are displayed.
        /// One of the System.Web.UI.WebControls.RepeatDirection values. The default is Vertical.
        /// </summary>
        [Category("Layout")]
        [Description("The direction in which items are laid out.")]
        [DefaultValue(RepeatDirection.Vertical)]
        public RepeatDirection RepeatDirection
        {
            get
            {
                object obj = ViewState["RepeatDirection"];
                return (obj == null ? RepeatDirection.Vertical : (RepeatDirection)obj);
            }
            set { ViewState["RepeatDirection"] = value; }
        }

        /// <summary>
        /// Gets or sets the layout of radio buttons within the group.
        /// One of the System.Web.UI.WebControls.RepeatLayout values. The default is Table.
        /// </summary>
        [Category("Layout")]
        [Description("Whether items are repeated in a table or in-flow.")]
        [DefaultValue(RepeatLayout.Table)]
        public RepeatLayout RepeatLayout
        {
            get
            {
                object obj = ViewState["RepeatLayout"];
                return (obj == null ? RepeatLayout.Table : (RepeatLayout)obj);
            }
            set { ViewState["RepeatLayout"] = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the server control binds to a data source.
        /// </summary>
        [Category("Data")]
        [Description("Occurs after the server control binds to a data source.")]
        public event EventHandler DataBound;

        #endregion

        #region Private Methods

        private void CopyProperties(CheckBoxList control)
        {
            CopyProperties(this, control);

            control.AppendDataBoundItems = this.AppendDataBoundItems;
            control.DataMember = this.DataMember;
            control.DataSource = this.DataSource;
            control.DataSourceID = this.DataSourceId;
            control.DataTextField = this.DataTextField;
            control.DataTextFormatString = this.DataTextFormatString;
            control.DataValueField = this.DataValueField;
            control.CellPadding = this.CellPadding;
            control.CellSpacing = this.CellSpacing;
            control.RepeatColumns = this.RepeatColumns;
            control.RepeatDirection = this.RepeatDirection;
            control.RepeatLayout = this.RepeatLayout;
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            CheckBoxList checkBoxList = sender as CheckBoxList;
            if (checkBoxList != null)
            {
                m_SelectedValue = this.LookupStringValue(checkBoxList);
                checkBoxList.DataBound += new EventHandler(OnDataBound);
            }
        }

        private void OnDataBound(object sender, EventArgs e)
        {
            CheckBoxList checkBoxList = sender as CheckBoxList;
            if ((checkBoxList != null) && (m_SelectedValue != null))
                checkBoxList.SelectedValue = m_SelectedValue;

            if (this.DataBound != null) this.DataBound(sender, e);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.CheckBoxListField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.CheckBoxListField.</returns>
        protected override DataControlField CreateField()
        {
            return new CheckBoxListField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.CheckBoxListField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.CheckBoxListField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            CheckBoxListField field = newField as CheckBoxListField;
            if (field != null)
            {
                field.AppendDataBoundItems = this.AppendDataBoundItems;
                field.AutoPostBack = this.AutoPostBack;
                field.DataMember = this.DataMember;
                field.DataSource = this.DataSource;
                field.DataSourceId = this.DataSourceId;
                field.DataTextField = this.DataTextField;
                field.DataTextFormatString = this.DataTextFormatString;
                field.DataValueField = this.DataValueField;
                field.CellPadding = this.CellPadding;
                field.CellSpacing = this.CellSpacing;
                field.RepeatColumns = this.RepeatColumns;
                field.RepeatDirection = this.RepeatDirection;
                field.RepeatLayout = this.RepeatLayout;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            CheckBoxList checkBoxList = control as CheckBoxList;
            return ((checkBoxList == null) ? string.Empty : checkBoxList.SelectedValue);
        }

        /// <summary>
        /// Initializes the specified System.Web.UI.WebControls.TableCell object to the specified row state.
        /// </summary>
        /// <param name="cell">The System.Web.UI.WebControls.TableCell to initialize.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            if (base.DesignMode)
            {
                if (cell != null)
                    cell.Text = (string)base.GetDesignTimeValue();
                return;
            }

            CopyProperties(m_InnerControl);
            m_InnerControl.Init += OnControlInit;

            if (!(this.EditMode || this.InsertMode))
                m_InnerControl.Enabled = m_InnerControl.Required = false;

            if (base.Visible)
                m_InnerControl.DataBinding += new EventHandler(this.OnBindingField);
            else
                m_InnerControl.Required = false;

            if (cell != null)
                cell.Controls.Add(m_InnerControl);
        }

        #endregion
    }
}
