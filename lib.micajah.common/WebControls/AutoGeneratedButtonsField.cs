using System;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Pages;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    public class AutoGeneratedButtonsField : ButtonFieldBase
    {
        #region Members

        private bool m_ShowEditButton;
        private bool m_ShowDeleteButton;
        private bool m_ShowSelectButton;
        private bool m_ShowInsertButton;
        private CommonGridView m_Grid;
        private MagicForm m_MagicForm;

        #endregion

        #region Constructors

        private AutoGeneratedButtonsField() { }

        public AutoGeneratedButtonsField(CommonGridView grid, bool showEditButton, bool showDeleteButton, bool showSelectButton)
        {
            this.m_Grid = grid;
            this.m_ShowEditButton = showEditButton;
            this.m_ShowDeleteButton = showDeleteButton;
            this.m_ShowSelectButton = showSelectButton;
        }

        public AutoGeneratedButtonsField(MagicForm magicForm)
        {
            if (magicForm != null)
            {
                m_MagicForm = magicForm;
                m_ShowEditButton = magicForm.AutoGenerateEditButton;
                m_ShowInsertButton = magicForm.AutoGenerateInsertButton;
                m_ShowDeleteButton = magicForm.AutoGenerateDeleteButton;
            }
        }

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets or sets a value indicating whether the header section is displayed.
        /// </summary>
        public override bool ShowHeader
        {
            get
            {
                if (m_MagicForm != null) return true;
                return base.ShowHeader;
            }
            set { base.ShowHeader = value; }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.AutoGeneratedButtonsField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.AutoGeneratedButtonsField.</returns>
        protected override DataControlField CreateField()
        {
            return new AutoGeneratedButtonsField();
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            BaseValidatedField.InitializeSpannedCell(cell, cellType);

            if (cellType != DataControlCellType.DataCell) return;

            if (m_Grid != null)
                InitializeCommonGridViewCell(cell, rowIndex);
            else if (m_MagicForm != null)
                InitializeMagicFormCell(cell, rowIndex);
        }

        #endregion

        #region Private Methods

        private void InsertButton(Control container, CommandActions commandAction, int rowIndex, string cssClass)
        {
            this.InsertButton(container, commandAction, rowIndex, ButtonType.Link, Color.Empty, false, cssClass);
        }

        private void InsertButton(Control container, CommandActions commandAction, int rowIndex, Color buttonColor)
        {
            this.InsertButton(container, commandAction, rowIndex, ButtonType.Link, buttonColor);
        }

        private void InsertButton(Control container, CommandActions commandAction, int rowIndex, ButtonType buttonType, Color buttonColor)
        {
            InsertButton(container, commandAction, rowIndex, buttonType, buttonColor, false);
        }

        private void InsertButton(Control container, CommandActions commandAction, int rowIndex, ButtonType buttonType, Color buttonColor, bool skipObjectName)
        {
            InsertButton(container, commandAction, rowIndex, buttonType, buttonColor, skipObjectName, null);
        }

        private void InsertButton(Control container, CommandActions commandAction, int rowIndex, ButtonType buttonType, Color buttonColor, bool skipObjectName, string cssClass)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (buttonType == ButtonType.Image) throw new NotSupportedException(Resources.ExceptionMessage_ImageButtonTypeIsNotSupported);

            IButtonControl button = null;

            string OnClientClick = string.Empty;
            if ((commandAction | CommandActions.Delete) == commandAction)
            {
                string caption = null;
                if (m_Grid != null)
                {
                    if (m_Grid.EnableDeleteConfirmation)
                        caption = m_Grid.DeleteButtonCaption.ToString();
                }
                else if (m_MagicForm != null)
                {
                    if (m_MagicForm.EnableDeleteConfirmation)
                        caption = m_MagicForm.DeleteButtonCaption.ToString();
                }
                if (caption != null)
                    OnClientClick = string.Concat("return confirm('", Resources.ResourceManager.GetString(string.Concat("AutoGeneratedButtonsField_", caption, "Button_ConfirmText")), "');");
            }

            if (buttonType == ButtonType.Link)
            {
                LinkButton ctrl = new LinkButton();
                ctrl.OnClientClick = OnClientClick;
                button = ctrl;
            }
            else if (buttonType == ButtonType.Button)
            {
                Button ctrl = new Button();
                ctrl.OnClientClick = OnClientClick;
                button = ctrl;
            }

            string cmdString = commandAction.ToString().Replace(" ", string.Empty);

            WebControl ctl = (button as WebControl);
            ctl.ID = string.Concat("btn", cmdString.Replace(",", string.Empty));
            if (!buttonColor.IsEmpty) ctl.ForeColor = buttonColor;
            if (!string.IsNullOrEmpty(cssClass)) ctl.CssClass = cssClass;

            button.Text = GetActionString(commandAction, skipObjectName);
            button.CommandName = cmdString.Split(',')[0];
            button.CommandArgument = string.Concat(rowIndex, ",", cmdString);
            button.Command += new CommandEventHandler(Button_Command);
            button.CausesValidation = false;

            if (m_MagicForm != null)
            {
                if (((commandAction | CommandActions.Update) == commandAction) || (commandAction | CommandActions.Insert) == commandAction)
                {
                    button.ValidationGroup = m_MagicForm.ValidationGroupInternal;
                    button.CausesValidation = true;
                }
            }

            container.Controls.Add(ctl);
        }

        private void InitializeMagicFormCell(DataControlFieldCell cell, int rowIndex)
        {
            bool modernTheme = (m_MagicForm.Theme == MasterPageTheme.Modern);

            cell.CssClass = "Mf_B";

            Control container = cell;
            if (this.m_MagicForm.ShowRequiredTable)
                container = MagicForm.AddRequiredTable(cell);

            CommandActions action = 0;
            bool showCloseButton = false;
            if (m_MagicForm.CurrentMode == DetailsViewMode.Edit && m_ShowEditButton)
            {
                action = CommandActions.Update;
                showCloseButton = ((m_MagicForm.ShowCloseButton == CloseButtonVisibilityMode.Always)
                    || (m_MagicForm.ShowCloseButton == CloseButtonVisibilityMode.Edit));

            }
            else if (m_MagicForm.CurrentMode == DetailsViewMode.Insert && m_ShowInsertButton)
            {
                action = CommandActions.Insert;
                showCloseButton = ((m_MagicForm.ShowCloseButton == CloseButtonVisibilityMode.Always)
                    || (m_MagicForm.ShowCloseButton == CloseButtonVisibilityMode.Insert));
            }

            if (action > 0)
            {
                ButtonType type = ButtonType.Button;
                Color color = Color.Black;
                if (showCloseButton)
                {
                    if (m_MagicForm.ShowCloseButtonSeparate)
                    {
                        InsertButton(container, action, rowIndex, type, (modernTheme ? Color.Empty : color));
                        InsertButtonSeparator(container);
                        type = ButtonType.Link;
                        color = Color.Blue;
                    }
                    InsertButton(container, (action | CommandActions.Close), rowIndex, type, (modernTheme ? Color.Empty : color), m_MagicForm.ShowCloseButtonSeparate, null);
                }
                else InsertButton(container, action, rowIndex, type, (modernTheme ? Color.Empty : color));
            }

            if (m_MagicForm.ShowCancelButton)
            {
                InsertButtonSeparator(container);
                InsertButton(container, CommandActions.Cancel, rowIndex, "Cancel");
            }

            if (m_ShowDeleteButton && m_MagicForm.CurrentMode != DetailsViewMode.Insert)
            {
                InsertButtonSeparator(container);
                InsertButton(container, CommandActions.Delete, rowIndex, "Delete");
            }
        }

        private void InitializeCommonGridViewCell(DataControlFieldCell cell, int rowIndex)
        {
            cell.CssClass = "Cgv_B";
            cell.Attributes.Remove("SpannedCell");
            if (this.m_Grid.EditIndex != rowIndex)
            {
                if (m_ShowEditButton)
                    this.InsertButton(cell, CommandActions.Edit, rowIndex, "Edit");

                if (m_ShowDeleteButton)
                    this.InsertButton(cell, CommandActions.Delete, rowIndex, "Delete");

                if (m_ShowSelectButton)
                {
                    InsertSeparator(cell);
                    this.InsertButton(cell, CommandActions.Select, rowIndex, Color.Empty);
                }
            }
            else
            {
                if (m_ShowEditButton)
                {
                    this.InsertButton(cell, CommandActions.Update, rowIndex, Color.Empty);
                    InsertSeparator(cell);
                    this.InsertButton(cell, CommandActions.Cancel, rowIndex, Color.Empty);
                }
            }
        }

        private string GetActionString(CommandActions commandAction, bool skipObjectName)
        {
            StringBuilder sb = new StringBuilder();
            string actionName = null;

            string key = string.Empty;
            CommandActions keyAction = 0;
            if (m_MagicForm != null)
            {
                if (m_MagicForm.CurrentMode == DetailsViewMode.Insert)
                {
                    if ((commandAction & CommandActions.Insert) == CommandActions.Insert)
                    {
                        key = m_MagicForm.InsertButtonCaption.ToString();
                        keyAction = CommandActions.Insert;
                    }
                }
                else if (m_MagicForm.CurrentMode == DetailsViewMode.Edit)
                {
                    if ((commandAction & CommandActions.Update) == CommandActions.Update)
                    {
                        key = m_MagicForm.UpdateButtonCaption.ToString();
                        keyAction = CommandActions.Update;
                    }
                }
            }
            else if (m_Grid != null)
            {
                if ((commandAction & CommandActions.Delete) == CommandActions.Delete)
                {
                    key = m_Grid.DeleteButtonCaption.ToString();
                    keyAction = CommandActions.Delete;
                }
            }

            foreach (CommandActions cmdAction in Enum.GetValues(typeof(CommandActions)))
            {
                if ((commandAction | cmdAction) == commandAction)
                {
                    bool useKey = false;
                    actionName = cmdAction.ToString();
                    if (keyAction > 0)
                    {
                        useKey = (((cmdAction & keyAction) == keyAction));
                        if (useKey) actionName = key;
                    }

                    if (!DesignMode) actionName = Resources.ResourceManager.GetString(string.Concat("AutoGeneratedButtonsField_", actionName, "Button_Text"));

                    if (sb.Length == 0)
                    {
                        sb.Append(actionName);
                        if (m_MagicForm != null)
                        {
                            if ((!skipObjectName) && useKey && (!string.IsNullOrEmpty(m_MagicForm.ObjectName))) sb.AppendFormat(" {0}", m_MagicForm.ObjectName);
                        }
                    }
                    else
                        sb.AppendFormat(" & {0}", actionName);
                }
            }
            return sb.ToString();
        }

        private void Button_Command(object sender, CommandEventArgs e)
        {
            int rowIndex = -1;
            CommandActions commandAction = (CommandActions)Enum.Parse(typeof(CommandActions), e.CommandName);

            string[] args = e.CommandArgument.ToString().Split(',');
            if (args.Length > 0)
            {
                if (!Int32.TryParse(args[0], out rowIndex)) rowIndex = -1;
                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        commandAction = (commandAction | (CommandActions)Enum.Parse(typeof(CommandActions), args[i]));
                    }
                }
            }

            if (this.m_Grid != null)
            {
                switch (commandAction)
                {
                    case CommandActions.Edit:
                        //this.m_grid.EditIndex = rowIndex;
                        break;
                    case CommandActions.Select:
                        this.m_Grid.SelectedIndex = rowIndex;
                        break;
                    case CommandActions.Delete:
                        this.m_Grid.DeleteRow(rowIndex);
                        break;
                    case CommandActions.Cancel:
                    case CommandActions.Update:
                        this.m_Grid.EditIndex = -1;
                        break;
                }
                this.m_Grid.ActionInternal(this.m_Grid, new CommonGridViewActionEventArgs(commandAction, rowIndex));
                this.m_Grid.DataBind();
            }
            else if (m_MagicForm != null)
            {
                this.m_MagicForm.ActionInternal(this.m_MagicForm, new MagicFormActionEventArgs(commandAction));
            }
        }

        #endregion

        #region Public Methods

        public static void InsertSeparator(Control container)
        {
            InsertSeparator(container, "|", 1);
        }

        public static void InsertSeparator(Control container, string separator, int noBreakCharsCount)
        {
            if (container == null) throw new ArgumentNullException("container");

            string noBreakChars = Support.RepeatString("&nbsp;", noBreakCharsCount, string.Empty);
            LiteralControl literalControl = new LiteralControl(noBreakChars + separator + noBreakChars);
            container.Controls.Add(literalControl);
        }

        public static void InsertButtonSeparator(Control container)
        {
            InsertSeparator(container, Resources.AutoGeneratedButtonsField_ButtonsSeparator, 2);
        }

        #endregion
    }
}
