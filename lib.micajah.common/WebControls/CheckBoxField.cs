using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents a Boolean field that is displayed as a check box in a data-bound control.
    /// </summary>
    public class CheckBoxField : BaseValidatedField, ICheckBox
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the text label associated with the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated shown with the control.")]
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
        /// Gets or sets the alignment of the text label associated with the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The alignment of the text label associated with the control.")]
        [DefaultValue(TextAlign.Right)]
        public TextAlign TextAlign
        {
            get
            {
                object obj = ViewState["TextAlign"];
                return ((obj == null) ? TextAlign.Right : (TextAlign)obj);
            }
            set { ViewState["TextAlign"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control state automatically posts back to the server when clicked.
        /// </summary>
        [Category("Behavior")]
        [Description("Automatically posts back to the server when the control is clicked.")]
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
        /// Gets or sets a value indicating whether the control is checked by default in insert mode.
        /// </summary>
        [Category("Behavior")]
        [Description("The checked state of the control in insert mode.")]
        [DefaultValue(false)]
        public bool DefaultChecked
        {
            get
            {
                object obj = ViewState["DefaultChecked"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["DefaultChecked"] = value; }
        }

        /// <summary>
        /// Gets or sets the rendering mode for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The rendering mode.")]
        [DefaultValue(CheckBoxRenderingMode.CheckBox)]
        public CheckBoxRenderingMode RenderingMode
        {
            get
            {
                object obj = this.ViewState["RenderingMode"];
                return ((obj == null) ? CheckBoxRenderingMode.CheckBox : (CheckBoxRenderingMode)obj);
            }
            set { this.ViewState["RenderingMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the text label associated with the control in checked state.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated with the control in checked state.")]
        [ResourceDefaultValue("CheckBox_CheckedText")]
        public string CheckedText
        {
            get
            {
                object obj = this.ViewState["CheckedText"];
                return (obj == null) ? Resources.CheckBox_CheckedText : (string)obj;
            }
            set { this.ViewState["CheckedText"] = value; }
        }

        /// <summary>
        /// Gets or sets the text label associated with the control in unchecked state.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated with the control in unchecked state.")]
        [ResourceDefaultValue("CheckBox_UncheckedText")]
        public string UncheckedText
        {
            get
            {
                object obj = this.ViewState["UncheckedText"];
                return (obj == null) ? Resources.CheckBox_UncheckedText : (string)obj;
            }
            set { this.ViewState["UncheckedText"] = value; }
        }

        #endregion

        #region Private Methods

        private void CopyProperties(CheckBox control)
        {
            CopyProperties(this, control);

            control.Text = this.Text;
            control.TextAlign = this.TextAlign;
            control.AutoPostBack = this.AutoPostBack;
            control.RenderingMode = this.RenderingMode;
            control.CheckedText = this.CheckedText;
            control.UncheckedText = this.UncheckedText;
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null) checkBox.Checked = LookupBooleanValue(checkBox);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.CheckBoxField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.CheckBoxField.</returns>
        protected override DataControlField CreateField()
        {
            return new CheckBoxField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.CheckBoxField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.CheckBoxField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            CheckBoxField field = newField as CheckBoxField;
            if (field != null)
            {
                field.Text = this.Text;
                field.TextAlign = this.TextAlign;
                field.AutoPostBack = this.AutoPostBack;
                field.DefaultChecked = this.DefaultChecked;
                field.RenderingMode = this.RenderingMode;
                field.CheckedText = this.CheckedText;
                field.UncheckedText = this.UncheckedText;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            CheckBox checkBox = control as CheckBox;
            return ((checkBox == null) ? false : checkBox.Checked);
        }

        /// <summary>
        /// Initializes the specified System.Web.UI.WebControls.TableCell object to the specified row state.
        /// </summary>
        /// <param name="cell">The System.Web.UI.WebControls.TableCell to initialize.</param>
        /// <param name="rowState">One of the System.Web.UI.WebControls.DataControlRowState values.</param>
        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            CheckBox control = new CheckBox();
            CopyProperties(control);
            control.Init += OnControlInit;

            if (this.InsertMode)
                control.Checked = DefaultChecked;
            else if (!this.EditMode)
                control.Enabled = control.Required = false;

            if (base.Visible)
            {
                if (!this.InsertMode) control.DataBinding += new EventHandler(this.OnBindingField);
            }
            else
                control.Required = false;

            if (cell != null)
                cell.Controls.Add(control);
        }

        #endregion
    }
}
