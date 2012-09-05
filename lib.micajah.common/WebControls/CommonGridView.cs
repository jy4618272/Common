using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// GridView implementation with using custom paging functionality and supporting standard color schemes.
    /// </summary>
    public class CommonGridView : GridView, IThemeable
    {
        #region Members

        public const string CollapseCommandName = "Collapse";
        public const string ExpandCommandName = "Expand";

        internal EventHandler<CommonGridViewActionEventArgs> ActionInternal;

        private WebControl oPagesTop; //current page (top)
        private WebControl oPagesBottom; //current page (bottom)
        private Literal oLabel1Top;  //pages: (top)
        private Literal oLabel2Top; //from 1000 (top)
        private WebControl oButton2Top; //< (top)
        private WebControl oButton3Top; //> (top)
        private Literal oLabel1Bottom; //pages: (bottom)
        private Literal oLabel2Bottom; //from 1000 (bottom)
        private WebControl oButton2Bottom; //< (bottom)
        private WebControl oButton3Bottom; //> (bottom)
        private Table oTableTop; //top line state
        private Table oTableBottom; //bottom line state
        private CustomPagerSettings m_CustomPagerSettings;
        private SchemeColorSet m_SchemeColorSet;
        private int? m_MaxColSpan;
        private bool? m_HeaderGroupExists;
        private Control m_ChildControl;
        private LinkButton m_AddLink;
        private ITemplate m_CaptionControls;
        private PlaceHolder m_CaptionControlsContainer;
        private TextBox m_SearchTextBox;
        private Button m_SearchButton;
        private ITemplate m_Filter;
        private PlaceHolder m_FilterContainer;
        private GridViewRow m_HeaderRow;
        private GridViewRow m_TopPagerRow;
        private GridViewRow m_BottomPagerRow;

        #endregion

        #region Private Properties

        private bool HeaderGroupExists
        {
            get
            {
                if (!m_HeaderGroupExists.HasValue)
                {
                    m_HeaderGroupExists = false;
                    for (int Index = 0; Index < this.Columns.Count; Index++)
                    {
                        if (this.Columns[Index].ShowHeader && this.Columns[Index].Visible)
                        {
                            ISpanned field = this.Columns[Index] as ISpanned;
                            if (field != null)
                            {
                                if (!string.IsNullOrEmpty(field.HeaderGroup))
                                {
                                    m_HeaderGroupExists = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                return m_HeaderGroupExists.Value;
            }
        }

        private int MaxColSpan
        {
            get
            {
                if (!m_MaxColSpan.HasValue) m_MaxColSpan = this.GetMaxColSpan(0);
                return m_MaxColSpan.Value;
            }
        }

        private SchemeColorSet SchemeColorSet
        {
            get
            {
                if (m_SchemeColorSet == null) m_SchemeColorSet = new SchemeColorSet(ColorScheme);
                return m_SchemeColorSet;
            }
        }

        private bool ShowPager
        {
            get { return (AllowPaging && m_CustomPagerSettings.Visible && base.PageCount > 1); }
        }

        private bool ShowTopPagerRow
        {
            get { return (ShowPager && (m_CustomPagerSettings.Position == PagerPosition.Top || m_CustomPagerSettings.Position == PagerPosition.TopAndBottom)); }
        }

        private bool ShowBottomPagerRow
        {
            get { return (ShowPager && (m_CustomPagerSettings.Position == PagerPosition.Bottom || m_CustomPagerSettings.Position == PagerPosition.TopAndBottom)); }
        }

        private int StartCellsCount
        {
            get
            {
                int startCellsCount = 0;
                if (this.AutoGenerateEditButton)
                    startCellsCount++;
                if (!string.IsNullOrEmpty(this.ChildControl))
                    startCellsCount++;
                return startCellsCount;
            }
        }

        private bool Focused { get; set; }

        private GridViewRow EmptyDataRow { get; set; }

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets or sets the gridline style for a control.
        /// </summary>
        [DefaultValue(GridLines.None)]
        public override GridLines GridLines
        {
            get { return base.GridLines; }
            set { base.GridLines = value; }
        }

        [DefaultValue(false)]
        public new bool AutoGenerateDeleteButton
        {
            get
            {
                object obj = ViewState["NewAutoGenerateDeleteButton"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["NewAutoGenerateDeleteButton"] = value; }
        }

        [DefaultValue(false)]
        public new bool AutoGenerateEditButton
        {
            get
            {
                object obj = ViewState["NewAutoGenerateEditButton"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["NewAutoGenerateEditButton"] = value; }
        }

        [ResourceDefaultValue("CommonGridView_EmptyDataText")]
        public override string EmptyDataText
        {
            get { return base.EmptyDataText; }
            set { base.EmptyDataText = value; }
        }

        public new GridViewRow HeaderRow
        {
            get { return ((m_HeaderRow == null) ? base.HeaderRow : m_HeaderRow); }
            private set { m_HeaderRow = value; }
        }

        public new GridViewRow TopPagerRow
        {
            get { return ((m_TopPagerRow == null) ? base.TopPagerRow : m_TopPagerRow); }
            private set { m_TopPagerRow = value; }
        }

        public new GridViewRow BottomPagerRow
        {
            get { return ((m_BottomPagerRow == null) ? base.BottomPagerRow : m_BottomPagerRow); }
            private set { m_BottomPagerRow = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public GridViewRow CaptionRow { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public GridViewRow FilterRow { get; private set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The caption of the add hyperlink.
        /// </summary>
        [Category("Appearance")]
        [Description("The caption of the add hyperlink.")]
        [ResourceDefaultValue("CommonGridView_AddLink_Text")]
        public string AddLinkCaption
        {
            get
            {
                object obj = ViewState["AddLinkCaption"];
                return ((obj == null) ? Resources.CommonGridView_AddLink_Text : (string)obj);
            }
            set { ViewState["AddLinkCaption"] = value; }
        }

        ///<summary>
        /// Gets Custom Pager Class for customizing paging.
        ///</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        NotifyParentProperty(true),
        PersistenceMode(PersistenceMode.InnerProperty),
        Category("Paging"),
        Description("Defines properties of a line state for CustomPaging.")]
        public CustomPagerSettings CustomPagerSettings
        {
            get { return m_CustomPagerSettings; }
        }

        ///<summary>
        /// Gets or sets predefined color schemes.
        ///</summary>
        [DefaultValue(ColorScheme.White),
        Category("Appearance"),
        Description("Defines standart color scheme.")]
        public ColorScheme ColorScheme
        {
            get
            {
                object obj = ViewState["ColorScheme"];
                return ((obj == null) ? ColorScheme.White : (ColorScheme)obj);
            }
            set
            {
                ViewState["ColorScheme"] = value;
                m_SchemeColorSet = null;
            }
        }

        ///<summary>
        /// Gets or sets caption.
        ///</summary>
        [Category("Appearance")]
        [Description("Gets or sets grid caption.")]
        [DefaultValue("")]
        public new string Caption
        {
            get
            {
                object obj = ViewState["NewCaption"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["NewCaption"] = value; }
        }

        /// <summary>
        /// Gets or sets the content which is displayed in the caption section of the control at the left of the add hyperlink.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate CaptionControls
        {
            get { return m_CaptionControls; }
            set
            {
                m_CaptionControls = value;
                this.EnsureCaptionControls();
            }
        }

        /// <summary>
        /// Gets or sets the content which is displayed above the table with the data and under the caption section.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate Filter
        {
            get { return m_Filter; }
            set
            {
                m_Filter = value;
                this.EnsureFilter();
            }
        }

        [Themeable(false)]
        [TypeConverter(typeof(ControlIDConverter))]
        [DefaultValue(""), IDReferenceProperty]
        public string ChildControl
        {
            get
            {
                object obj = ViewState["ChildControl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ChildControl"] = value; }
        }

        ///<summary>
        /// Gets or sets a value indicating whether the add hyperlink is visible and rendered.
        ///</summary>
        [DefaultValue(false)]
        [Category("Appearance")]
        [Description("Whether the add hyperlink is visible and rendered.")]
        public bool ShowAddLink
        {
            get
            {
                object obj = ViewState["ShowAddLink"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["ShowAddLink"] = value; }
        }

        ///<summary>
        /// Gets or sets a value indicating whether the search is visible and rendered.
        ///</summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Whether the search is visible and rendered.")]
        public bool EnableSearch
        {
            get
            {
                object obj = ViewState["EnableSearch"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableSearch"] = value; }
        }

        /// <summary>
        /// Gets or set value indicating that the row's select action is enabled.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool EnableSelect
        {
            get
            {
                object obj = ViewState["EnableSelect"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableSelect"] = value; }
        }

        /// <summary>
        /// Gets or set value indicating that the delete action requires confirmation.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool EnableDeleteConfirmation
        {
            get
            {
                object obj = ViewState["EnableDeleteConfirmation"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableDeleteConfirmation"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the collapse image.
        /// </summary>
        [Category("Appearance")]
        [Description("The URL to the collapse image.")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [UrlProperty]
        public string CollapseImageUrl
        {
            get
            {
                object obj = ViewState["CollapseImageUrl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["CollapseImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the expand image.
        /// </summary>
        [Category("Appearance")]
        [Description("The URL to the expand image.")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [UrlProperty]
        public string ExpandImageUrl
        {
            get
            {
                object obj = ViewState["ExpandImageUrl"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ExpandImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the width of the expand/collapse image.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(typeof(Unit), "19px")]
        [Description("The width of the expand/collapse image.")]
        public Unit ExpandCollapseImageWidth
        {
            get
            {
                object obj = ViewState["ExpandCollapseImageWidth"];
                return ((obj == null) ? Unit.Pixel(19) : (Unit)obj);
            }
            set { ViewState["ExpandCollapseImageWidth"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the expand/collapse image.
        /// </summary>
        [Category("Layout")]
        [DefaultValue(typeof(Unit), "19px")]
        [Description("The height of the expand/collapse image.")]
        public Unit ExpandCollapseImageHeight
        {
            get
            {
                object obj = ViewState["ExpandCollapseImageHeight"];
                return ((obj == null) ? Unit.Pixel(19) : (Unit)obj);
            }
            set { ViewState["ExpandCollapseImageHeight"] = value; }
        }

        /// <summary>
        /// The caption type of the delete button.
        /// </summary>
        [Category("Appearance")]
        [Description("The caption type of the delete button.")]
        [DefaultValue(DeleteButtonCaptionType.Delete)]
        public DeleteButtonCaptionType DeleteButtonCaption
        {
            get
            {
                object obj = ViewState["DeleteButtonCaption"];
                return ((obj == null) ? DeleteButtonCaptionType.Delete : (DeleteButtonCaptionType)obj);
            }
            set { ViewState["DeleteButtonCaption"] = value; }
        }

        /// <summary>
        /// Gets the System.Web.UI.WebControls.DataKey object that contains the data key value for the expanded row in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataKey ExpandedDataKey
        {
            get
            {
                DataKeyArray dataKeys = this.DataKeys;
                int expandedIndex = this.ExpandedIndex;
                if (((dataKeys != null) && (expandedIndex < dataKeys.Count)) && (expandedIndex > -1))
                {
                    return dataKeys[expandedIndex];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the index of the expanded row in a control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ExpandedIndex
        {
            get
            {
                object obj = ViewState["ExpandedIndex"];
                return ((obj == null) ? -1 : (int)obj);
            }
            set { this.ChangeExpandedIndex(((value > -1) ? ExpandCommandName : CollapseCommandName), value); }
        }

        /// <summary>
        /// Gets a reference to a System.Web.UI.WebControls.GridViewRow object that represents the expanded row in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GridViewRow ExpandedRow
        {
            get
            {
                int expandedIndex = this.ExpandedIndex;
                GridViewRow row = null;
                if (expandedIndex != -1) row = this.Rows[expandedIndex];
                return row;
            }
        }

        /// <summary>
        ///  Gets the data key value of the expanded row in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object ExpandedValue
        {
            get
            {
                if (this.ExpandedDataKey != null)
                    return this.ExpandedDataKey.Value;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the value which the search text box displays when it does not have focus.
        /// </summary>
        [Category("Appearance")]
        [Description("The value which the search text box displays when it does not have focus.")]
        [ResourceDefaultValue("CommonGridView_SearchEmptyText")]
        public string SearchEmptyText
        {
            get
            {
                this.EnsureSearch();
                return ((m_SearchTextBox.EmptyText == null) ? Resources.CommonGridView_SearchEmptyText : m_SearchTextBox.EmptyText);
            }
            set
            {

                this.EnsureSearch();
                m_SearchTextBox.EmptyText = value;
            }
        }

        /// <summary>
        /// Gets or sets the text content of search text box.
        /// </summary>
        [Category("Appearance")]
        [Description("The text content of search text box.")]
        [DefaultValue("")]
        public string SearchText
        {
            get
            {
                this.EnsureSearch();
                return m_SearchTextBox.Text;
            }
            set
            {

                this.EnsureSearch();
                m_SearchTextBox.Text = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MasterPageTheme Theme
        {
            get
            {
                object obj = ViewState["Theme"];
                return ((obj == null) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Theme : (MasterPageTheme)obj);
            }
            set { ViewState["Theme"] = value; }
        }

        #endregion

        #region Events

        public event EventHandler<CommonGridViewActionEventArgs> Action;

        /// <summary>
        /// Occurs when one of the pager buttons is clicked, but after the control handles the paging operation.
        /// </summary>
        public new event EventHandler PageIndexChanged;

        /// <summary>
        /// Occurs when one of the pager buttons is clicked, but before the control handles the paging operation.
        /// </summary>
        public new event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Occurs when a row's Edit button is clicked, but before the control enters edit mode.
        /// </summary>
        public new event GridViewEditEventHandler RowEditing;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CommonGridView class. 
        /// </summary>
        public CommonGridView()
        {
            this.ActionInternal = new EventHandler<CommonGridViewActionEventArgs>(this.OnAction);

            m_CustomPagerSettings = new CustomPagerSettings();
        }

        #endregion

        #region Private Methods

        private static void ApplyStyle(GridView grid, SchemeColorSet schemeColorSet, MasterPageTheme theme, bool enableSelect, int selectedRowIndex)
        {
            if (grid == null) return;
            if (schemeColorSet == null) return;

            grid.CellPadding = 0;
            grid.CellSpacing = 0;
            grid.GridLines = GridLines.None;
            grid.CssClass = "Cgv_T";
            grid.HeaderStyle.CssClass = "Cgv_H";
            grid.EmptyDataRowStyle.CssClass = "Cgv_Er";
            grid.FooterStyle.CssClass = "Cgv_F";
            grid.PagerStyle.CssClass = "Cgv_P";

            System.Drawing.Color borderColor = System.Drawing.Color.Empty;

            if (theme == MasterPageTheme.Modern)
                grid.BorderWidth = Unit.Pixel(0);
            else
            {
                grid.EmptyDataRowStyle.BorderColor = borderColor = schemeColorSet.Dark;
                grid.Style["border-top"] = "0 solid transparent";
                grid.Style["border-left"] = grid.Style["border-right"] = grid.Style["border-bottom"] = "0 solid " + schemeColorSet.DarkHtml;

                if (grid.GetType() == typeof(GridView))
                {
                    if (grid.Rows.Count == 0)
                    {
                        Table child = grid.Controls[0] as Table;
                        if (child != null)
                        {
                            if (child.Rows.Count > 0)
                                ProcessRowCells(child.Rows[0], grid.EmptyDataRowStyle.CssClass, borderColor);
                        }
                    }
                }
            }

            string cssClass = null;
            foreach (GridViewRow row in grid.Rows)
            {
                cssClass = "Cgv_C";
                if (theme == MasterPageTheme.Modern)
                    row.CssClass = "Cgv_R";
                else
                {
                    DataControlRowState rowState = row.RowState;
                    if ((rowState & DataControlRowState.Alternate) == DataControlRowState.Alternate)
                    {
                        cssClass = "Cgv_A";
                        row.BackColor = schemeColorSet.Light;
                    }
                    else if ((rowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                        cssClass = "Cgv_E";
                }

                if (row.RowIndex != selectedRowIndex)
                {
                    if (enableSelect && (string.Compare(row.Attributes["enableSelect"], bool.FalseString, StringComparison.OrdinalIgnoreCase) != 0))
                    {
                        row.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                        if (string.IsNullOrEmpty(row.Attributes["onclick"]))
                        {
                            row.Attributes["onclick"] = "Cgv_Select(event, '"
                                + grid.Page.ClientScript.GetPostBackEventReference(grid, string.Concat(CommandActions.Select, "$", row.RowIndex)).Replace("'", "\\'") + "');";
                        }
                    }
                }
                else
                {
                    if (theme == MasterPageTheme.Modern)
                        row.CssClass += " Cgv_Sr";
                    else
                        row.BackColor = schemeColorSet.Highlight;
                }

                ProcessRowCells(row, cssClass, borderColor);
            }

            if (grid.HeaderRow != null)
                ProcessRowCells(grid.HeaderRow, grid.HeaderStyle.CssClass, borderColor);

            if (grid.TopPagerRow != null)
            {
                grid.TopPagerRow.BorderColor = borderColor;
                ProcessRowCells(grid.TopPagerRow, grid.PagerStyle.CssClass, borderColor);
            }

            if (grid.BottomPagerRow != null)
            {
                grid.BottomPagerRow.BorderColor = borderColor;
                ProcessRowCells(grid.BottomPagerRow, grid.PagerStyle.CssClass, borderColor);
            }

            RegisterStyleSheet(grid, theme);
        }

        private static void ApplyStyle(Table table, SchemeColorSet schemeColorSet, MasterPageTheme theme)
        {
            if (table == null) return;

            table.CellSpacing = table.CellPadding = 0;
            table.Style[HtmlTextWriterStyle.BorderCollapse] = "collapse";
            table.CssClass = "Cgv_T";

            TableRow row = null;
            int rowsCount = table.Rows.Count;
            string cssClass = null;

            System.Drawing.Color borderColor = System.Drawing.Color.Empty;
            if (theme == MasterPageTheme.Modern)
                table.BorderWidth = Unit.Pixel(0);
            else
                table.BorderColor = borderColor = schemeColorSet.Dark;

            for (int idx = 0; idx < rowsCount; idx++)
            {
                row = table.Rows[idx];

                cssClass = "Cgv_C";
                if (row.CssClass == "Caption")
                {
                    cssClass = "Cgv_Cpt";
                    row.CssClass = null;
                }
                else if ((row.CssClass == "Header") || (row is TableHeaderRow))
                    row.CssClass = cssClass = "Cgv_H";
                else if (theme == MasterPageTheme.Modern)
                    row.CssClass = "Cgv_R";
                else if (idx % 2 != 0)
                {
                    cssClass = "Cgv_A";
                    row.BackColor = schemeColorSet.Light;
                }

                ProcessRowCells(row, cssClass, borderColor);
            }

            RegisterStyleSheet(table, theme);
        }

        private static void ApplyStyle(HtmlTable table, SchemeColorSet schemeColorSet, MasterPageTheme theme)
        {
            if (table == null) return;

            table.CellSpacing = table.CellPadding = 0;
            table.Style[HtmlTextWriterStyle.BorderCollapse] = "collapse";
            table.Attributes["class"] = "Cgv_T";

            HtmlTableRow row = null;
            int rowsCount = table.Rows.Count;
            int dataRowIndex = -1;
            string cssClass = null;

            System.Drawing.Color borderColor = System.Drawing.Color.Empty;
            if (theme == MasterPageTheme.Modern)
                table.Border = 0;
            else
            {
                borderColor = schemeColorSet.Dark;
                table.Style["border-top"] = "0 solid transparent";
                table.Style["border-left"] = table.Style["border-right"] = table.Style["border-bottom"] = "0 solid " + schemeColorSet.DarkHtml;
            }

            for (int idx = 0; idx < rowsCount; idx++)
            {
                row = table.Rows[idx];

                cssClass = "Cgv_C";
                if (row.Attributes["class"] == "Caption")
                {
                    cssClass = "Cgv_Cpt";
                    row.Attributes.Remove("class");
                }
                else if (row.Attributes["class"] == "Header")
                    row.Attributes["class"] = cssClass = "Cgv_H";
                else if (theme == MasterPageTheme.Modern)
                    row.Attributes["class"] = "Cgv_R";
                else
                {
                    if (dataRowIndex % 2 == 0)
                    {
                        cssClass = "Cgv_A";
                        row.Style[HtmlTextWriterStyle.BackgroundColor] = schemeColorSet.LightHtml;
                    }
                    dataRowIndex++;
                }

                ProcessRowCells(row, cssClass, borderColor);
            }

            RegisterStyleSheet(table, theme);
        }

        private static void ProcessRowCells(TableRow row, string cssClass, System.Drawing.Color borderColor)
        {
            foreach (TableCell cell in row.Cells)
            {
                MagicForm.ApplyCssClassToCell(cell, cssClass);

                if (!borderColor.IsEmpty)
                    cell.BorderColor = borderColor;
            }
        }

        private static void ProcessRowCells(HtmlTableRow row, string cssClass, System.Drawing.Color borderColor)
        {
            foreach (HtmlTableCell cell in row.Cells)
            {
                MagicForm.ApplyCssClassToCell(cell, cssClass);

                if (!borderColor.IsEmpty)
                    cell.Style[HtmlTextWriterStyle.BorderColor] = System.Drawing.ColorTranslator.ToHtml(borderColor);
            }
        }

        private void ChangeExpandedIndex(string commandName, int rowIndex)
        {
            bool expand = (string.Compare(commandName, ExpandCommandName, StringComparison.Ordinal) == 0);

            if (expand && (this.ExpandedIndex > -1)) this.ChangeExpandedIndex(CollapseCommandName, this.ExpandedIndex);

            this.ViewState["ExpandedIndex"] = (expand ? rowIndex : -1);

            if ((rowIndex > -1) && (rowIndex < this.Rows.Count))
            {
                GridViewRow row = this.Rows[rowIndex];
                if (row.Cells[0].Controls.Count > 0)
                {
                    ImageButton imageButton = row.Cells[0].Controls[0] as ImageButton;
                    if (imageButton != null)
                    {
                        imageButton.CommandName = (expand ? CollapseCommandName : ExpandCommandName);
                        imageButton.ImageUrl = this.GetExpandCollapseImageUrl(expand);
                    }
                }
            }

            if (!expand)
            {
                this.EnsureChildControl();

                if (m_ChildControl != null)
                {
                    foreach (Control ctl in m_ChildControl.Controls)
                    {
                        CommonGridView grid = ctl as CommonGridView;
                        if (grid != null) grid.ChangeExpandedIndex(CollapseCommandName, grid.ExpandedIndex);
                    }
                }
            }
        }

        private ImageButton CreatePreviousImageButton(string id)
        {
            ImageButton imgBtn = new ImageButton();
            imgBtn.ID = id;
            if (PageIndex > 0)
                imgBtn.ImageUrl = m_CustomPagerSettings.LeftImageUrl;
            else
            {
                imgBtn.ImageUrl = m_CustomPagerSettings.LeftDisableImageUrl;
                imgBtn.Enabled = false;
            }
            imgBtn.Click += new ImageClickEventHandler(PreviousButton_Click);
            if (!string.IsNullOrEmpty(m_CustomPagerSettings.LeftText))
                imgBtn.AlternateText = m_CustomPagerSettings.LeftText;

            return (string.IsNullOrEmpty(imgBtn.ImageUrl) ? null : imgBtn);
        }

        private ImageButton CreateNextImageButton(string id)
        {
            ImageButton imgBtn = new ImageButton();
            imgBtn.ID = id;
            if (PageIndex > 0)
                imgBtn.ImageUrl = m_CustomPagerSettings.RightImageUrl;
            else
            {
                imgBtn.ImageUrl = m_CustomPagerSettings.RightDisableImageUrl;
                imgBtn.Enabled = false;
            }
            imgBtn.Click += new ImageClickEventHandler(NextButton_Click);
            if (!string.IsNullOrEmpty(m_CustomPagerSettings.RightText))
                imgBtn.AlternateText = m_CustomPagerSettings.RightText;

            return (string.IsNullOrEmpty(imgBtn.ImageUrl) ? null : imgBtn);
        }

        private LinkButton CreatePreviousLinkButton(string id)
        {
            if (string.IsNullOrEmpty(m_CustomPagerSettings.LeftText))
                return null;

            LinkButton lnkBtn = new LinkButton();

            lnkBtn.ID = id;
            lnkBtn.Text = m_CustomPagerSettings.LeftText;
            lnkBtn.Click += new EventHandler(PreviousButton_Click);
            if (PageIndex <= 0)
                lnkBtn.Enabled = false;

            return lnkBtn;
        }

        private LinkButton CreateNextLinkButton(string id)
        {
            if (string.IsNullOrEmpty(m_CustomPagerSettings.RightText))
                return null;

            LinkButton lnkBtn = new LinkButton();

            lnkBtn.ID = id;
            lnkBtn.Text = m_CustomPagerSettings.RightText;
            lnkBtn.Click += new EventHandler(NextButton_Click);
            if (PageIndex >= (PageCount - 1))
                lnkBtn.Enabled = false;

            return lnkBtn;
        }

        /// <summary>
        /// Creates the buttons for custom pager depending on button style.
        /// </summary>
        private void CreateCustomPagerButtons()
        {
            switch (m_CustomPagerSettings.ButtonsStyle)
            {
                case PagerButtonsStyle.Image:
                    oButton2Top = CreatePreviousImageButton("PreviousTopButton");
                    oButton3Top = CreateNextImageButton("NextTopButton");
                    oButton2Bottom = CreatePreviousImageButton("PreviousBottomButton");
                    oButton3Bottom = CreateNextImageButton("NextBottomButton");
                    break;
                case PagerButtonsStyle.Link:
                    oButton2Top = CreatePreviousLinkButton("PreviousTopButton");
                    oButton3Top = CreateNextLinkButton("NextTopButton");
                    oButton2Bottom = CreatePreviousLinkButton("PreviousBottomButton");
                    oButton3Bottom = CreateNextLinkButton("NextBottomButton");
                    break;
            }
        }

        private DropDownList CreatePageList(string id)
        {
            DropDownList dropdown = new DropDownList();

            for (int i = 1; i <= this.PageCount; i++)
            {
                dropdown.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            dropdown.SelectedIndex = this.PageIndex;
            dropdown.ID = id;
            dropdown.AutoPostBack = true;
            dropdown.SelectedIndexChanged += new EventHandler(PageList_SelectedIndexChanged);

            return dropdown;
        }

        private System.Web.UI.WebControls.TextBox CreatePageTextBox(string id)
        {
            System.Web.UI.WebControls.TextBox textBox = new System.Web.UI.WebControls.TextBox();

            textBox.ID = id;
            textBox.AutoPostBack = true;
            textBox.Text = Convert.ToString((PageIndex + 1), CultureInfo.InvariantCulture);
            textBox.TextChanged += new EventHandler(PageTextBox_TextChanged);

            return textBox;
        }

        /// <summary>
        /// Creates a selector of the custom pager depending on mode.
        /// </summary>
        private void CreateCustomPagerSelector()
        {
            switch (m_CustomPagerSettings.Mode)
            {
                case PagingMode.TextBox:
                    oPagesTop = this.CreatePageTextBox("PageTopTextBox");
                    oPagesBottom = this.CreatePageTextBox("PageBottomTextBox");
                    break;
                case PagingMode.DropDownList:
                    oPagesTop = CreatePageList("PageTopList");
                    oPagesBottom = CreatePageList("PageBottomList");
                    break;
            }
        }

        private GridViewRow CreateHeaderRow(bool dataBind, TableRowCollection rows)
        {
            GridViewRow row = this.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            GridViewRowEventArgs e = new GridViewRowEventArgs(row);

            ICollection columns = CreateColumns(null, false);
            DataControlField[] fields = new DataControlField[columns.Count];
            columns.CopyTo(fields, 0);
            this.InitializeRow(row, fields);

            this.OnRowCreated(e);

            rows.Add(row);

            if (dataBind)
            {
                row.DataBind();
                this.OnRowDataBound(e);
            }

            return row;
        }

        private GridViewRow CreateCaptionRow()
        {
            GridViewRow row = null;
            TableCell cell = null;
            Panel pnl1 = null;
            Panel pnl2 = null;

            try
            {
                bool addCaptionControls = ((m_CaptionControlsContainer != null) && m_CaptionControlsContainer.HasControls());

                if (this.EnableSearch || (!string.IsNullOrEmpty(this.Caption)) || addCaptionControls || this.ShowAddLink)
                {
                    row = this.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.CssClass = "Cgv_Cpt";
                    this.ProcessCell(cell);

                    if (this.EnableSearch || (!string.IsNullOrEmpty(this.Caption)))
                    {
                        pnl1 = new Panel();
                        pnl1.ID = "SearchPanel";
                        cell.Controls.Add(pnl1);

                        if (this.EnableSearch)
                        {
                            pnl1.CssClass = "Cgv_Search";

                            this.EnsureSearch();
                            pnl1.Controls.Add(m_SearchTextBox);
                            pnl1.Controls.Add(m_SearchButton);
                        }
                        else if (!string.IsNullOrEmpty(this.Caption))
                        {
                            pnl1.CssClass = "Cgv_CptTxt";
                            if (this.Theme != MasterPageTheme.Modern)
                                pnl1.ForeColor = this.SchemeColorSet.Dark;
                            pnl1.Controls.Add(new LiteralControl(this.Caption));
                        }
                    }

                    if (addCaptionControls || this.ShowAddLink)
                    {
                        pnl2 = new Panel();
                        pnl2.ID = "CaptionControlsPanel";
                        pnl2.CssClass = "Cgv_CptCtrls";
                        cell.Controls.Add(pnl2);

                        if (addCaptionControls)
                            pnl2.Controls.Add(m_CaptionControlsContainer);

                        if (this.ShowAddLink)
                        {
                            m_AddLink = new LinkButton();
                            m_AddLink.ID = "btnAdd";
                            m_AddLink.Text = this.AddLinkCaption;
                            m_AddLink.CausesValidation = false;
                            m_AddLink.CssClass = ((this.Theme == MasterPageTheme.Modern) ? "Button Green Large" : "Cgv_AddNew");
                            m_AddLink.Click += new EventHandler(AddLink_Click);
                            pnl2.Controls.Add(m_AddLink);
                        }
                    }
                }

                return row;
            }
            finally
            {
                if (pnl1 != null) pnl1.Dispose();
                if (pnl2 != null) pnl2.Dispose();
                if (cell != null) cell.Dispose();
                if (row != null) row.Dispose();
            }
        }

        private GridViewRow CreateFilterRow()
        {
            GridViewRow row = null;
            TableCell cell = null;

            try
            {
                if ((m_FilterContainer != null) && m_FilterContainer.HasControls())
                {
                    row = this.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
                    cell = new TableCell();
                    cell.CssClass = "Cgv_Filter";
                    row.Cells.Add(cell);
                    this.ProcessCell(cell);

                    cell.Controls.Add(m_FilterContainer);
                }

                return row;
            }
            finally
            {
                if (cell != null) cell.Dispose();
                if (row != null) row.Dispose();
            }
        }

        private GridViewRow CreatePagerRow(PagerPosition position)
        {
            GridViewRow row = this.CreateRow(-1, -1, DataControlRowType.Pager, DataControlRowState.Normal);
            row.CssClass = "Cgv_P";
            if (this.Theme != MasterPageTheme.Modern)
                row.BorderColor = this.SchemeColorSet.Dark;

            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            this.ProcessCell(cell);
            cell.ApplyStyle(this.PagerStyle);
            if (this.Theme != MasterPageTheme.Modern)
                cell.BorderColor = this.SchemeColorSet.Dark;

            Table table = null;
            if (position == PagerPosition.Top)
                table = oTableTop;
            else if (position == PagerPosition.Bottom)
                table = oTableBottom;

            if (table != null)
            {
                if (!this.PagerStyle.ForeColor.IsEmpty) table.ForeColor = this.PagerStyle.ForeColor;
                table.Font.CopyFrom(this.PagerStyle.Font);
                cell.Controls.Add(table);
            }

            return row;
        }

        private void EnsureSearch()
        {
            if (m_SearchTextBox != null) return;

            m_SearchTextBox = new TextBox();
            m_SearchTextBox.ID = "SearchTextBox";
            m_SearchTextBox.Theme = MasterPageTheme.Modern;
            m_SearchTextBox.CausesValidation = false;

            m_SearchButton = new Button();
            m_SearchButton.ID = "SearchButton";
            m_SearchButton.CausesValidation = false;
            m_SearchButton.Click += new EventHandler(SearchButton_Click);
            if (this.Theme != MasterPageTheme.Modern)
                m_SearchButton.Text = Resources.CommonGridView_SearchButtonText;
        }

        private void EnsureChildControl()
        {
            if (m_ChildControl == null)
                m_ChildControl = Support.FindTargetControl(this.ChildControl, this, true);
        }

        private void EnsureCaptionControls()
        {
            if (m_CaptionControls != null)
            {
                if (m_CaptionControlsContainer == null)
                {
                    m_CaptionControlsContainer = new PlaceHolder();
                    m_CaptionControlsContainer.ID = "CaptionControlsContainer";
                }
                m_CaptionControls.InstantiateIn(m_CaptionControlsContainer);
            }
        }

        private void EnsureFilter()
        {
            if (m_Filter != null)
            {
                if (m_FilterContainer == null)
                {
                    m_FilterContainer = new PlaceHolder();
                    m_FilterContainer.ID = "FilterContainer";
                }
                m_Filter.InstantiateIn(m_FilterContainer);
            }
        }

        private static string GetHeaderGroupName(TableCell cell)
        {
            DataControlFieldHeaderCell fieldCell = cell as DataControlFieldHeaderCell;
            if (fieldCell != null)
            {
                ISpanned field = fieldCell.ContainingField as ISpanned;
                if ((field != null) && (!string.IsNullOrEmpty(field.HeaderGroup)) && fieldCell.ContainingField.ShowHeader && fieldCell.ContainingField.Visible)
                    return field.HeaderGroup;
            }
            return null;
        }

        private string GetExpandCollapseImageUrl(bool expand)
        {
            if (expand)
            {
                if (string.IsNullOrEmpty(this.CollapseImageUrl))
                    return ResourceProvider.GetImageUrl(typeof(CommonGridView), "Minus.gif", true);
                else
                    return this.CollapseImageUrl;
            }
            else
            {
                if (string.IsNullOrEmpty(this.ExpandImageUrl))
                    return ResourceProvider.GetImageUrl(typeof(CommonGridView), "Plus.gif", true);
                else
                    return this.ExpandImageUrl;
            }
        }

        private int GetMaxColSpan(int startIndex)
        {
            int colSpan = 0;
            for (int i = startIndex; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                {
                    colSpan += 1;
                    ISpanned f = this.Columns[i] as ISpanned;
                    if (f != null)
                    {
                        if (f.ColumnSpan > 1) colSpan += (f.ColumnSpan - 1);
                        if (f.CreateNewRow)
                        {
                            int currentColSpan = GetMaxColSpan(i + 1);
                            if (colSpan < currentColSpan) colSpan = currentColSpan;
                            break;
                        }
                    }
                }
            }
            return colSpan;
        }

        private void HandlePage(int newPageIndex)
        {
            if (this.AllowPaging)
            {
                GridViewPageEventArgs args = new GridViewPageEventArgs(newPageIndex);
                OnPageIndexChanging(args);
                if (!args.Cancel)
                {
                    if ((args.NewPageIndex <= -1) || ((base.IsViewStateEnabled && (args.NewPageIndex >= this.PageCount)) && (this.PageIndex == (this.PageCount - 1))))
                        return;
                    PageIndex = args.NewPageIndex;
                    OnPageIndexChanged(EventArgs.Empty);
                    this.RequiresDataBinding = true;
                }
            }
        }

        /// <summary>
        /// Initialize custom pager child controls.
        /// </summary>
        private void InitCustomPager()
        {
            //top
            oLabel1Top = new Literal();
            oLabel2Top = new Literal();
            oLabel1Bottom = new Literal();
            oLabel2Bottom = new Literal();
            oTableTop = new Table();
            oTableBottom = new Table();
            oTableTop.HorizontalAlign = m_CustomPagerSettings.HorizontalAlign;
            oTableBottom.HorizontalAlign = m_CustomPagerSettings.HorizontalAlign;

            this.CreateCustomPagerSelector();

            oTableTop.Rows.Clear();
            oTableTop.ID = "oTableTop";
            if (m_CustomPagerSettings.FirstTitle.Length > 0) oLabel1Top.Text = m_CustomPagerSettings.FirstTitle + HtmlTextWriter.SpaceChar;
            if (m_CustomPagerSettings.LastTitle.Length > 0)
                oLabel2Top.Text = string.Concat(HtmlTextWriter.SpaceChar, m_CustomPagerSettings.LastTitle, HtmlTextWriter.SpaceChar, PageCount.ToString(CultureInfo.CurrentCulture));
            TableCell oCell1Top = new TableCell();
            TableCell oCell2Top = new TableCell();
            TableCell oCell3Top = new TableCell();
            TableCell oCell4Top = new TableCell();
            TableCell oCell5Top = new TableCell();
            TableCell oCell6Top = new TableCell();
            TableCell oCell7Top = new TableCell();
            TableCell oCell8Top = new TableCell();
            oCell1Top.Wrap = false;
            oCell5Top.Wrap = false;
            oCell7Top.Wrap = false;
            oCell8Top.Width = Unit.Percentage(100);
            if (oLabel1Top.Text.Length > 0) oCell1Top.Controls.Add(oLabel1Top);

            this.CreateCustomPagerButtons();

            if (oButton2Top != null) oCell3Top.Controls.Add(oButton2Top);

            oCell4Top.Controls.Add(oPagesTop);
            if (oLabel2Top.Text.Length > 0) oCell5Top.Controls.Add(oLabel2Top);
            if (oButton3Top != null) oCell6Top.Controls.Add(oButton3Top);

            TableRow oRowTop = new TableRow();
            oRowTop.HorizontalAlign = HorizontalAlign.Center;
            oRowTop.Cells.Add(oCell1Top);
            oRowTop.Cells.Add(oCell2Top);
            oRowTop.Cells.Add(oCell3Top);
            oRowTop.Cells.Add(oCell4Top);
            oRowTop.Cells.Add(oCell5Top);
            oRowTop.Cells.Add(oCell6Top);
            oRowTop.Cells.Add(oCell7Top);
            if (m_CustomPagerSettings.Width.IsEmpty && (!Width.IsEmpty)) oRowTop.Cells.Add(oCell8Top);
            oTableTop.Rows.Add(oRowTop);
            oTableTop.Width = Width;

            //bottom
            oTableBottom.Rows.Clear();
            oTableBottom.ID = "oTableBottom";
            if (m_CustomPagerSettings.FirstTitle.Length > 0)
                oLabel1Bottom.Text = m_CustomPagerSettings.FirstTitle + HtmlTextWriter.SpaceChar;
            if (m_CustomPagerSettings.LastTitle.Length > 0)
                oLabel2Bottom.Text = string.Concat(HtmlTextWriter.SpaceChar, m_CustomPagerSettings.LastTitle, HtmlTextWriter.SpaceChar, PageCount.ToString(CultureInfo.CurrentCulture));
            TableCell oCell1Bottom = new TableCell();
            TableCell oCell2Bottom = new TableCell();
            TableCell oCell3Bottom = new TableCell();
            TableCell oCell4Bottom = new TableCell();
            TableCell oCell5Bottom = new TableCell();
            TableCell oCell6Bottom = new TableCell();
            TableCell oCell7Bottom = new TableCell();
            TableCell oCell8Bottom = new TableCell();
            oCell1Bottom.Wrap = false;
            oCell5Bottom.Wrap = false;
            oCell7Bottom.Wrap = false;
            oCell8Bottom.Width = Unit.Percentage(100);
            if (oLabel1Bottom.Text.Length > 0) oCell1Bottom.Controls.Add(oLabel1Bottom);
            if (oButton2Bottom != null) oCell3Bottom.Controls.Add(oButton2Bottom);

            oCell4Bottom.Controls.Add(oPagesBottom);
            if (oLabel2Bottom.Text.Length > 0) oCell5Bottom.Controls.Add(oLabel2Bottom);
            if (oButton3Bottom != null) oCell6Bottom.Controls.Add(oButton3Bottom);

            TableRow oRowBottom = new TableRow();
            oRowBottom.HorizontalAlign = HorizontalAlign.Center;
            oRowBottom.Cells.Add(oCell1Bottom);
            oRowBottom.Cells.Add(oCell2Bottom);
            oRowBottom.Cells.Add(oCell3Bottom);
            oRowBottom.Cells.Add(oCell4Bottom);
            oRowBottom.Cells.Add(oCell5Bottom);
            oRowBottom.Cells.Add(oCell6Bottom);
            oRowBottom.Cells.Add(oCell7Bottom);
            if (m_CustomPagerSettings.Width.IsEmpty && (!Width.IsEmpty)) oRowBottom.Cells.Add(oCell8Bottom);
            oTableBottom.Rows.Add(oRowBottom);
            oTableBottom.Width = Width;

            oPagesTop.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            oPagesBottom.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        }

        private static bool NeedCreateNewRow(TableCell cell)
        {
            return NeedCreateNewRow(cell as DataControlFieldCell);
        }

        private static bool NeedCreateNewRow(DataControlFieldCell fieldCell)
        {
            if (fieldCell != null) return NeedCreateNewRow(fieldCell.ContainingField);
            return false;
        }

        private static bool NeedCreateNewRow(DataControlField field)
        {
            if (field != null)
            {
                ISpanned f = field as ISpanned;
                if (f != null) return f.CreateNewRow;
            }
            return false;
        }

        private void OnAction(CommandActions action, int rowIndex)
        {
            this.OnAction(this, new CommonGridViewActionEventArgs(action, rowIndex));
        }

        private void OnAction(object sender, CommonGridViewActionEventArgs e)
        {
            if (e.Action == CommandActions.Edit) OnRowEditing(new GridViewEditEventArgs(e.RowIndex));
            if (this.Action != null) this.Action(sender, e);
        }

        private void PagePreRender(object sender, EventArgs e)
        {
            this.EnsureChildControl();
            if (m_ChildControl != null)
            {
                int rowIndex = this.ExpandedIndex;
                m_ChildControl.Visible = ((rowIndex > -1) && (rowIndex < this.Rows.Count));
            }
        }

        private void ProcessCell(TableCell cell)
        {
            int colSpan = this.MaxColSpan + this.StartCellsCount;
            if (this.AutoGenerateDeleteButton) colSpan++;
            if (colSpan > 1)
                cell.ColumnSpan = colSpan;
        }

        private void RegisterClientScripts()
        {
            int rowIndex = this.ExpandedIndex;
            if ((rowIndex > -1) && (rowIndex < this.Rows.Count))
            {
                this.EnsureChildControl();
                if (m_ChildControl != null)
                {
                    int m = 1;
                    int columnsCount = this.Columns.Count;

                    for (int i = 0; i < columnsCount; i++)
                    {
                        if (this.Columns[i].Visible)
                        {
                            if (NeedCreateNewRow(this.Columns[i]) && (i < (columnsCount - 1))) m++;
                        }
                    }

                    int idx = (this.ExpandedIndex + 1) * m;
                    if (this.ShowHeader)
                    {
                        idx++;
                        if (this.HeaderGroupExists) idx++;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID + "_InsertChildToTableRow"
                        , string.Format(CultureInfo.InvariantCulture, "Cgv_InsertChildToTableRow('{0}','{1}',{2});\r\n", m_ChildControl.ClientID, this.ClientID, idx)
                        , true);
                }
            }
        }

        /// <summary>
        /// Registers the CommonGridView stylesheet file on the page.
        /// </summary>
        /// <param name="control">The control that is registering the stylesheet file.</param>
        /// <param name="theme">The theme of the control.</param>
        private static void RegisterStyleSheet(Control control, MasterPageTheme theme)
        {
            if (theme == MasterPageTheme.Modern)
                ResourceProvider.RegisterStyleSheetResource(control, ResourceProvider.CommonGridViewModernStyleSheet, "CommonGridViewModernStyleSheet", true);
            else
                ResourceProvider.RegisterStyleSheetResource(control, "Styles.CommonGridView.css", "CommonGridViewStyleSheet", true);
        }

        private static void RenderHeaderCell(HtmlTextWriter writer, ref TableCell cell, int rowSpan)
        {
            DataControlFieldHeaderCell fieldCell = cell as DataControlFieldHeaderCell;
            if (!fieldCell.ContainingField.Visible) return;

            string cellText = null;
            cell.Attributes.Remove("SpannedCell");
            cell.RowSpan = ((rowSpan > 1) ? rowSpan : 0);
            if (!fieldCell.ContainingField.ShowHeader)
            {
                cellText = cell.Text;
                cell.Text = string.Empty;
            }
            cell.RenderControl(writer);
            cell.Visible = false;
            if (cellText != null) cell.Text = cellText;
        }

        private static void RenderHeaderGroupCell(HtmlTextWriter writer, string headerGroupName, int columnSpan, string borderColor)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Scope, "col");
            if (columnSpan > 1) writer.AddAttribute(HtmlTextWriterAttribute.Colspan, columnSpan.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(borderColor)) writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, borderColor);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Cgv_H");
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(headerGroupName);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders the header row.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        private void RenderHeaderRow(HtmlTextWriter writer)
        {
            if ((this.HeaderRow != null) && this.ShowHeader)
            {
                this.HeaderRow.MergeStyle(this.HeaderStyle);
                this.HeaderRow.RenderBeginTag(writer);
                this.RenderHeaderRow(writer, this.HeaderRow);
                this.HeaderRow.RenderEndTag(writer);
            }
        }

        /// <summary>
        /// Renders the header row.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        /// <param name="container">The Control to render.</param>
        private void RenderHeaderRow(HtmlTextWriter writer, Control container)
        {
            if ((writer == null) || (container == null) || (!this.ShowHeader)) return;

            GridViewRow headerRow = container as GridViewRow;
            if (headerRow == null) return;

            if (this.HeaderGroupExists)
            {
                int cellsCount = headerRow.Cells.Count;
                string previousHeaderGroupName = null;
                int columnSpan = 1;
                int cellIndex = 0;

                for (cellIndex = 0; cellIndex <= cellsCount; cellIndex++)
                {
                    string headerGroupName = null;

                    if (cellIndex < cellsCount)
                    {
                        headerGroupName = GetHeaderGroupName(headerRow.Cells[cellIndex]);
                        if ((previousHeaderGroupName != null) && (headerGroupName != null) && (string.Compare(headerGroupName, previousHeaderGroupName, StringComparison.CurrentCulture) == 0))
                        {
                            columnSpan++;
                            continue;
                        }
                    }

                    if (cellIndex > 0)
                    {
                        if ((columnSpan > 1) || (!string.IsNullOrEmpty(previousHeaderGroupName)))
                            RenderHeaderGroupCell(writer, previousHeaderGroupName, columnSpan, (this.Theme == MasterPageTheme.Modern ? null : this.SchemeColorSet.DarkHtml));
                        else
                        {
                            TableCell cell = headerRow.Cells[cellIndex - 1];
                            if (this.Theme != MasterPageTheme.Modern)
                                cell.BorderColor = this.SchemeColorSet.Dark;
                            RenderHeaderCell(writer, ref cell, 2);
                            if (NeedCreateNewRow(cell)) break;
                        }
                    }

                    previousHeaderGroupName = headerGroupName;
                    columnSpan = 1;
                }

                cellsCount--;
                if (this.AutoGenerateDeleteButton && (cellIndex < cellsCount))
                {
                    TableCell cell = headerRow.Cells[cellsCount];
                    if (this.Theme != MasterPageTheme.Modern)
                        cell.BorderColor = this.SchemeColorSet.Dark;
                    RenderHeaderCell(writer, ref cell, 2);
                }

                writer.RenderEndTag();

                if (!string.IsNullOrEmpty(base.HeaderStyle.CssClass)) writer.AddAttribute(HtmlTextWriterAttribute.Class, base.HeaderStyle.CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            }

            this.RenderHeaderRow(writer, headerRow, this.HeaderGroupExists);
        }

        private void RenderHeaderRow(HtmlTextWriter writer, GridViewRow headerRow, bool headerGroupExists)
        {
            int cellsCount = headerRow.Cells.Count;
            int cellIndex = 0;
            string borderColor = (this.Theme == MasterPageTheme.Modern ? null : this.SchemeColorSet.DarkHtml);

            for (cellIndex = 0; cellIndex < cellsCount; cellIndex++)
            {
                TableCell cell = headerRow.Cells[cellIndex];
                if (this.Theme != MasterPageTheme.Modern)
                    cell.BorderColor = this.SchemeColorSet.Dark;
                RenderHeaderCell(writer, ref cell, 0);
                if (NeedCreateNewRow(cell))
                {
                    int additionalCellsCount = (this.MaxColSpan - cellIndex - ((cell.ColumnSpan > 1) ? cell.ColumnSpan : 1) + this.StartCellsCount);
                    RenderEmptyCells(writer, additionalCellsCount, cell.CssClass, borderColor, HtmlTextWriterTag.Th);
                    break;
                }
            }

            if (!headerGroupExists)
            {
                cellsCount--;
                if (this.AutoGenerateDeleteButton && (cellIndex < cellsCount))
                    headerRow.Cells[cellsCount].RenderControl(writer);
            }
        }

        private static void RenderRow(HtmlTextWriter writer, GridViewRow row, int startCellsCount, int maxColSpan, bool autoGenerateDeleteButton, string borderColor, ref bool firstRow)
        {
            int cellsCount = row.Cells.Count;

            HtmlTextWriterTag tag = ((row.RowType == DataControlRowType.Header) ? HtmlTextWriterTag.Th : HtmlTextWriterTag.Td);
            bool deleteButtonCellIsRendered = false;
            int cellIndex = 0;
            int colSpan = 0;

            row.RenderBeginTag(writer);

            for (int columnIndex = 0; columnIndex < cellsCount; columnIndex++)
            {
                TableCell cell = row.Cells[columnIndex];
                if (!cell.Visible)
                    continue;

                bool createNewRow = false;
                DataControlFieldCell fieldCell = cell as DataControlFieldCell;
                if (fieldCell != null)
                {
                    DataControlField field = fieldCell.ContainingField;
                    if (field != null)
                    {
                        if (!field.Visible)
                            continue;

                        createNewRow = NeedCreateNewRow(field);
                    }
                }

                int additionalCellsCount = 0;
                bool last = (columnIndex == (cellsCount - 1 - (autoGenerateDeleteButton ? 1 : 0)));

                if (createNewRow && (!last))
                {
                    if (cell.ColumnSpan >= maxColSpan)
                        cell.ColumnSpan = maxColSpan;
                    else
                        additionalCellsCount = (maxColSpan - cellIndex - ((cell.ColumnSpan > 1) ? cell.ColumnSpan : 1) + (firstRow ? startCellsCount : 0));

                    cell.RenderControl(writer);

                    RenderEmptyCells(writer, additionalCellsCount, cell.CssClass, borderColor, tag);

                    if (autoGenerateDeleteButton)
                    {
                        if (deleteButtonCellIsRendered)
                            RenderEmptyCells(writer, 1, cell.CssClass, borderColor, tag);
                        else
                        {
                            TableCell deleteButtonCell = row.Cells[cellsCount - 1];
                            deleteButtonCell.RenderControl(writer);
                            deleteButtonCell.Visible = false;
                            deleteButtonCellIsRendered = true;
                        }
                    }

                    row.RenderEndTag(writer);
                    row.RenderBeginTag(writer);

                    RenderEmptyCells(writer, startCellsCount, cell.CssClass, borderColor, tag);

                    cellIndex = 0;
                    firstRow = false;
                }
                else
                {
                    additionalCellsCount = ((last && autoGenerateDeleteButton && deleteButtonCellIsRendered) ? 1 : 0);
                    if (last)
                    {
                        int c1 = (maxColSpan - cellIndex - ((cell.ColumnSpan > 1) ? cell.ColumnSpan : 1) - colSpan);
                        if (c1 > 0)
                            additionalCellsCount += c1;
                        else if (cell.ColumnSpan > maxColSpan)
                            cell.ColumnSpan = maxColSpan;
                    }
                    else
                        colSpan += ((cell.ColumnSpan > 1) ? (cell.ColumnSpan - 1) : 0);

                    cell.RenderControl(writer);

                    RenderEmptyCells(writer, additionalCellsCount, cell.CssClass, borderColor, tag);

                    cellIndex++;
                }
            }

            row.RenderEndTag(writer);
        }

        private void RenderRows(HtmlTextWriter writer)
        {
            if ((writer == null) || (this.Rows.Count == 0)) return;

            int startCellsCount = this.StartCellsCount;
            bool firstRow = true;
            string borderColor = (this.Theme == MasterPageTheme.Modern ? null : this.SchemeColorSet.DarkHtml);

            for (int rowIndex = 0; rowIndex < this.Rows.Count; rowIndex++)
            {
                firstRow = true;
                RenderRow(writer, this.Rows[rowIndex], startCellsCount, this.MaxColSpan, this.AutoGenerateDeleteButton, borderColor, ref firstRow);
            }
        }

        private static void RenderEmptyCells(HtmlTextWriter writer, int cellsCount, string cssClass, string borderColor, HtmlTextWriterTag tag)
        {
            if (cellsCount > 0)
            {
                for (int i = 0; i < cellsCount; i++)
                {
                    if (!string.IsNullOrEmpty(cssClass)) writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
                    if (!string.IsNullOrEmpty(borderColor)) writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, borderColor);
                    writer.RenderBeginTag(tag);
                    writer.Write("&nbsp;");
                    writer.RenderEndTag();
                }
            }
        }

        private void RenderFilterRow(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.FilterRow != null)
            {
                if (m_FilterContainer != null)
                {
                    StringBuilder sb = new StringBuilder();
                    using (StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture))
                    {
                        using (HtmlTextWriter wr = new HtmlTextWriter(sw))
                        {
                            m_FilterContainer.RenderControl(wr);
                        }
                    }

                    if (Support.StringIsNullOrEmpty(sb.ToString().Replace("&nbsp;", string.Empty)))
                        return;
                }

                this.FilterRow.RenderControl(writer);
            }
        }

        private void AddLink_Click(object sender, EventArgs e)
        {
            this.OnAction(CommandActions.Add, -1);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            this.OnAction(CommandActions.Search, -1);
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (PageIndex > 0) HandlePage(PageIndex - 1);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (PageIndex < (PageCount - 1)) HandlePage(PageIndex + 1);
        }

        private void PageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropdown = (DropDownList)sender;
            this.ChangePageIndex(dropdown.SelectedValue);
        }

        private void PageTextBox_TextChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)sender;
            if (!this.ChangePageIndex(textBox.Text))
                textBox.Text = (this.PageIndex + 1).ToString(CultureInfo.InvariantCulture);
        }

        private bool ChangePageIndex(string pageIndex)
        {
            int index = -1;
            if (!Int32.TryParse(pageIndex, out index)) index = -1;
            if (index > 0 && index <= PageCount)
            {
                HandlePage(index - 1);
                return true;
            }
            return false;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates the set of column fields used to build the control hierarchy.
        /// </summary>
        /// <param name="dataSource">A PagedDataSource that represents the data source.</param>
        /// <param name="useDataSource">true to use the data source specified by the dataSource parameter; otherwise, false.</param>
        /// <returns>A System.Collections.ICollection that contains the fields used to build the control hierarchy.</returns>
        protected override ICollection CreateColumns(PagedDataSource dataSource, bool useDataSource)
        {
            ICollection fields = base.CreateColumns(dataSource, useDataSource);
            ArrayList list = new ArrayList(fields);

            AutoGeneratedButtonsField autoGeneratedButtonsField = null;

            if (this.AutoGenerateDeleteButton)
            {
                autoGeneratedButtonsField = new AutoGeneratedButtonsField(this, false, this.AutoGenerateDeleteButton, false);
                list.Add(autoGeneratedButtonsField);
            }

            if (this.AutoGenerateEditButton)
            {
                autoGeneratedButtonsField = new AutoGeneratedButtonsField(this, this.AutoGenerateEditButton, false, false);
                list.Insert(0, autoGeneratedButtonsField);
            }

            if (!string.IsNullOrEmpty(this.ChildControl))
            {
                ButtonField bf = new ButtonField();
                bf.ButtonType = ButtonType.Image;
                bf.HeaderStyle.Width = this.ExpandCollapseImageWidth;
                bf.HeaderStyle.Wrap = false;
                bf.ItemStyle.Width = this.ExpandCollapseImageWidth;
                bf.ItemStyle.Wrap = false;
                bf.ControlStyle.Width = this.ExpandCollapseImageWidth;
                bf.ControlStyle.Height = this.ExpandCollapseImageHeight;
                bf.CommandName = "Expand";
                if (!this.DesignMode) bf.ImageUrl = this.GetExpandCollapseImageUrl(false);
                list.Insert(0, bf);
            }

            fields = list;

            foreach (object field in fields)
            {
                IThemeable f = field as IThemeable;
                if (f != null)
                    f.Theme = this.Theme;
            }

            return fields;
        }

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            this.PagerSettings.Visible = false;

            int rowsCount = base.CreateChildControls(dataSource, dataBinding);

            if (this.HasControls())
            {
                Table table = this.Controls[0] as Table;
                if (table != null)
                {
                    m_MaxColSpan = null;

                    if (rowsCount == 0)
                        this.HeaderRow = this.CreateHeaderRow(dataBinding, table.Rows);

                    if (this.ShowTopPagerRow || this.ShowBottomPagerRow)
                        this.InitCustomPager();

                    if (this.ShowTopPagerRow)
                    {
                        this.TopPagerRow = CreatePagerRow(PagerPosition.Top);
                        if (this.TopPagerRow != null)
                        {
                            table.Rows.AddAt(0, this.TopPagerRow);
                            if (dataBinding) this.TopPagerRow.DataBind();
                        }
                    }

                    this.FilterRow = this.CreateFilterRow();
                    if (this.FilterRow != null)
                    {
                        table.Rows.AddAt(0, this.FilterRow);
                        if (dataBinding) this.FilterRow.DataBind();
                    }

                    this.CaptionRow = this.CreateCaptionRow();
                    if (this.CaptionRow != null)
                    {
                        table.Rows.AddAt(0, this.CaptionRow);
                        if (m_SearchTextBox != null)
                            m_SearchTextBox.DefaultButtonUniqueId = m_SearchButton.UniqueID; // It's important to call this after the controls are added to the collection.
                        if (dataBinding) this.CaptionRow.DataBind();
                    }

                    if (this.ShowBottomPagerRow)
                    {
                        this.BottomPagerRow = CreatePagerRow(PagerPosition.Bottom);
                        if (this.BottomPagerRow != null)
                        {
                            table.Rows.Add(this.BottomPagerRow);
                            if (dataBinding) this.BottomPagerRow.DataBind();
                        }
                    }
                }
            }

            return rowsCount;
        }

        protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            if (rowType == DataControlRowType.EmptyDataRow)
            {
                this.EmptyDataRow = base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
                return this.EmptyDataRow;
            }
            return base.CreateRow(rowIndex, dataSourceIndex, rowType, rowState);
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Init event. Registers the required client script include.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            base.EmptyDataText = Resources.CommonGridView_EmptyDataText;

            if (!this.DesignMode)
            {
                Type type = this.GetType();
                if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(type, "CommonGridViewClientScripts"))
                    this.Page.ClientScript.RegisterClientScriptInclude(type, "CommonGridViewClientScripts", ResourceProvider.GetResourceUrl("Scripts.CommonGridView.js", true));
            }

            this.Page.PreRender += new EventHandler(PagePreRender);
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.ExpandedIndex > -1) this.ChangeExpandedIndex(ExpandCommandName, this.ExpandedIndex);
            base.OnDataBound(e);
        }

        /// <summary>
        /// Raises the PageIndexChanging event.
        /// </summary>
        /// <param name="e">A GridViewPageEventArgs that contains the event data.</param>
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null) PageIndexChanging(this, e);
        }

        /// <summary>
        /// Raises the PageIndexChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPageIndexChanged(EventArgs e)
        {
            if (PageIndexChanged != null) PageIndexChanged(this, e);
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event and registers the style sheet of the control.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            RegisterStyleSheet(this, this.Theme);

            if (this.Focused)
            {
                if (this.EnableSearch)
                    m_SearchTextBox.Focus();
                else
                    base.Focus();
            }
        }

        /// <summary>
        /// Raises the SelectedIndexChanged event.
        /// </summary>
        /// <param name="e">An EventArgs that contains event data.</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            OnAction(CommandActions.Select, this.SelectedIndex);
        }

        /// <summary>
        /// Raises the System.Web.UI.WebControls.GridView.RowCommand event.
        /// </summary>
        /// <param name="e">A System.Web.UI.WebControls.GridViewCommandEventArgs that contains event data.</param>
        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            if (e == null) return;

            switch (e.CommandName)
            {
                case CollapseCommandName:
                case ExpandCommandName:
                    this.ChangeExpandedIndex(e.CommandName, Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture));
                    break;
            }
            base.OnRowCommand(e);
        }

        /// <summary>
        /// Raises the RowEditing event.
        /// </summary>
        /// <param name="e">A GridViewEditEventArgs that contains event data.</param>
        protected override void OnRowEditing(GridViewEditEventArgs e)
        {
            if (RowEditing != null) RowEditing(this, e);
        }

        /// <summary>
        /// Override Render method for show custom pager and use standard color schemes
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (writer == null || (!this.Visible)) return;

            ApplyStyle(this, this.SchemeColorSet, this.Theme, this.EnableSelect, this.SelectedIndex);

            bool isNotEmpty = (this.Rows.Count > 0);

            this.RenderBeginTag(writer);

            if (this.CaptionRow != null)
                this.CaptionRow.RenderControl(writer);

            this.RenderFilterRow(writer);

            if (isNotEmpty && this.ShowTopPagerRow && (this.TopPagerRow != null))
                this.TopPagerRow.RenderControl(writer);

            this.RenderHeaderRow(writer);

            if (isNotEmpty)
                this.RenderRows(writer);
            else if (this.EmptyDataRow != null)
            {
                this.EmptyDataRow.MergeStyle(this.EmptyDataRowStyle);
                if (this.EmptyDataRow.Cells.Count > 0)
                    ProcessCell(this.EmptyDataRow.Cells[0]);
                ProcessRowCells(this.EmptyDataRow, null, this.EmptyDataRowStyle.BorderColor);
                this.EmptyDataRow.RenderControl(writer);
            }

            if (this.FooterRow != null)
                this.FooterRow.RenderControl(writer);

            if (isNotEmpty && this.ShowBottomPagerRow && (this.BottomPagerRow != null))
                this.BottomPagerRow.RenderControl(writer);

            this.RenderEndTag(writer);

            this.RegisterClientScripts();
        }

        public override void Focus()
        {
            this.Focused = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Applies the CommonGridView's styles to the grid.
        /// </summary>
        /// <param name="grid">The System.Web.UI.WebControls.GridView object to apply the styles to.</param>
        public static void ApplyStyle(GridView grid)
        {
            ApplyStyle(grid, ColorScheme.White);
        }

        /// <summary>
        ///  Applies the CommonGridView's styles to the grid.
        /// </summary>
        /// <param name="grid">The System.Web.UI.WebControls.GridView object to apply the styles to.</param>
        /// <param name="colorScheme">The Micajah.Common.Style.ColorScheme object that specifies the color scheme to apply.</param>
        public static void ApplyStyle(GridView grid, ColorScheme colorScheme)
        {
            ApplyStyle(grid, new SchemeColorSet(colorScheme), FrameworkConfiguration.Current.WebApplication.MasterPage.Theme, false, -1);
        }

        /// <summary>
        /// Applies the CommonGridView's styles to the table.
        /// </summary>
        /// <param name="table">The System.Web.UI.HtmlControls.HtmlTable object to apply the styles to.</param>
        public static void ApplyStyle(HtmlTable table)
        {
            ApplyStyle(table, ColorScheme.White);
        }

        /// <summary>
        /// Applies the CommonGridView's styles to the table.
        /// </summary>
        /// <param name="table">The System.Web.UI.HtmlControls.HtmlTable object to apply the styles to.</param>
        /// <param name="colorScheme">The Micajah.Common.Style.ColorScheme object that specifies the color scheme to apply.</param>
        public static void ApplyStyle(HtmlTable table, ColorScheme colorScheme)
        {
            ApplyStyle(table, new SchemeColorSet(colorScheme), FrameworkConfiguration.Current.WebApplication.MasterPage.Theme);
        }

        /// <summary>
        /// Applies the CommonGridView's styles to the table.
        /// </summary>
        /// <param name="table">The System.Web.UI.WebControls.Table object to apply the styles to.</param>
        public static void ApplyStyle(Table table)
        {
            ApplyStyle(table, ColorScheme.White);
        }

        /// <summary>
        /// Applies the CommonGridView's styles to the table.
        /// </summary>
        /// <param name="table">The System.Web.UI.WebControls.Table object to apply the styles to.</param>
        /// <param name="colorScheme">The Micajah.Common.Style.ColorScheme object that specifies the color scheme to apply.</param>
        public static void ApplyStyle(Table table, ColorScheme colorScheme)
        {
            ApplyStyle(table, new SchemeColorSet(colorScheme), FrameworkConfiguration.Current.WebApplication.MasterPage.Theme);
        }

        #endregion
    }
}
