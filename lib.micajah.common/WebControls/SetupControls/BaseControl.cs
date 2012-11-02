using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.SetupControls
{
    public abstract class BaseControl : UserControl
    {
        #region Members

        protected CommonGridView List;
        protected MagicForm EditForm;
        protected ObjectDataSource EntityListDataSource;
        protected ObjectDataSource EntityDataSource;
        private HtmlGenericControl m_ErrorDiv;
        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private bool m_AddBreadcrumbs;

        #endregion

        #region Protected Properties

        protected HtmlGenericControl ErrorDiv
        {
            get
            {
                if (m_ErrorDiv == null) m_ErrorDiv = EditForm.FindControl("ErrorDiv") as HtmlGenericControl;
                return m_ErrorDiv;
            }
        }

        protected Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = Page.Master as Micajah.Common.Pages.MasterPage;
                return m_MasterPage;
            }
        }

        protected bool EnablePartialRendering
        {
            get
            {
                object obj = this.ViewState["EnablePartialRendering"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { this.ViewState["EnablePartialRendering"] = value; }
        }

        #endregion

        #region Protected Methods

        protected bool ShowError(Exception exception)
        {
            bool result = ShowError(exception, ErrorDiv);
            if (!result)
            {
                EditFormReset();
                List.DataBind();
                this.ResetBreadcrumbs();
            }
            return result;
        }

        protected virtual void AddBreadcrumbs()
        {
            if (this.MasterPage != null)
            {
                Micajah.Common.Bll.Action item = new Micajah.Common.Bll.Action();
                item.ActionId = Guid.NewGuid();
                item.Name = EditForm.Caption;
                item.ParentAction = this.MasterPage.ActiveAction;
                UserContext.Breadcrumbs.Add(item);

                if (this.EnablePartialRendering) this.MasterPage.UpdateBreadcrumbs();
            }
        }

        protected virtual void ResetBreadcrumbs()
        {
            UserContext.Breadcrumbs.RemoveLast();
            if (this.EnablePartialRendering) this.MasterPage.UpdateBreadcrumbs();
        }

        protected virtual void EntityListDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            ListAllowPaging(List, e.ReturnValue);
        }

        protected virtual void List_PageIndexChanged(object sender, EventArgs e)
        {
            EditFormReset();
        }

        protected virtual void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                    List.Visible = false;
                    EditForm.Visible = true;
                    EditForm.ChangeMode(DetailsViewMode.Insert);
                    m_AddBreadcrumbs = true;
                    break;
                case CommandActions.Edit:
                    List.SelectedIndex = e.RowIndex;
                    List.Visible = false;
                    EditForm.Visible = true;
                    EditForm.ChangeMode(DetailsViewMode.Edit);
                    m_AddBreadcrumbs = true;
                    break;
                case CommandActions.Delete:
                    EditFormReset();
                    break;
            }
        }

        protected virtual void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                EditFormReset();
                this.ResetBreadcrumbs();
            }
        }

        protected virtual void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e == null) return;

            if (ShowError(e.Exception))
            {
                e.KeepInInsertMode = true;
                e.ExceptionHandled = true;
            }
        }

        protected virtual void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (e == null) return;

            if (ShowError(e.Exception))
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
            }
        }

        protected virtual void ListInitialize()
        {
            Initialize(List);

            List.PageIndexChanged += new EventHandler(List_PageIndexChanged);
            List.Action += new EventHandler<CommonGridViewActionEventArgs>(List_Action);
        }

        protected virtual void EditFormInitialize()
        {
            Initialize(EditForm);

            EditForm.ItemCommand += new DetailsViewCommandEventHandler(EditForm_ItemCommand);
            EditForm.ItemInserted += new DetailsViewInsertedEventHandler(EditForm_ItemInserted);
            EditForm.ItemUpdated += new DetailsViewUpdatedEventHandler(EditForm_ItemUpdated);
        }

        protected virtual void LoadResources()
        {
            string className = this.GetType().BaseType.Name;
            LoadResources(List, className);
            LoadResources(EditForm, className);
        }

        protected virtual void EditFormReset()
        {
            List.SelectedIndex = -1;
            List.Visible = true;
            EditForm.ChangeMode(DetailsViewMode.ReadOnly);
            EditForm.Visible = false;
        }

        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            try
            {
                ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                if (sm != null) sm.EnablePartialRendering = this.EnablePartialRendering;
            }
            catch { }

            EntityListDataSource.Selected += new ObjectDataSourceStatusEventHandler(EntityListDataSource_Selected);

            ListInitialize();
            EditFormInitialize();

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack) LoadResources();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (m_AddBreadcrumbs)
                this.AddBreadcrumbs();
        }

        #endregion

        #region Internal Methods

        internal static void HideControls(ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                control.Visible = false;
            }
        }

        internal static void LoadResources(CommonGridView grid, string className)
        {
            LoadResources(grid, className, grid.ID);
        }

        internal static void LoadResources(CommonGridView grid, string className, string gridId)
        {
            if (grid == null) return;

            string listResourceName = string.Concat(className, "_", gridId, "_");

            grid.Caption = Resources.ResourceManager.GetString(listResourceName + "Caption");
            foreach (DataControlField field in grid.Columns)
            {
                BoundField boundField = field as BoundField;
                if (boundField != null)
                    boundField.HeaderText = Resources.ResourceManager.GetString(string.Concat(listResourceName, boundField.DataField, "Column_HeaderText"));
            }
        }

        internal static void LoadResources(MagicForm form, string className)
        {
            if (form == null) return;

            string editFormResourceName = string.Concat(className, "_", form.ID, "_");

            form.ObjectName = Resources.ResourceManager.GetString(editFormResourceName + "ObjectName");
            foreach (DataControlField field in form.Fields)
            {
                BoundField boundField = field as BoundField;
                if (boundField != null)
                {
                    boundField.HeaderText = Resources.ResourceManager.GetString(string.Concat(editFormResourceName, boundField.DataField, "Field_HeaderText"));
                    CheckBoxField checkBoxField = field as CheckBoxField;
                    if (checkBoxField != null)
                        checkBoxField.Text = Resources.ResourceManager.GetString(string.Concat(editFormResourceName, checkBoxField.DataField, "Field_Text"));
                }
            }
        }

        internal static void Initialize(CommonGridView grid)
        {
            if (grid == null) return;

            grid.AllowSorting = true;
            grid.AutoGenerateColumns = false;
            grid.AutoGenerateEditButton = true;
            grid.AutoGenerateDeleteButton = true;
            grid.ShowAddLink = true;
            grid.PageSize = 50;
        }

        internal static void Initialize(MagicForm form)
        {
            if (form == null) return;

            form.AutoGenerateRows = false;
            form.AutoGenerateInsertButton = true;
            form.AutoGenerateEditButton = true;
            form.DefaultMode = DetailsViewMode.ReadOnly;
            form.Visible = false;
        }

        internal static void ListAllowPaging(CommonGridView grid, object dataSource)
        {
            int count = 0;
            if (dataSource == null)
                dataSource = grid.DataSource;
            DataTable table = (dataSource as DataTable);
            if (table != null)
                count = table.Rows.Count;
            else
            {
                DataView dv = (dataSource as DataView);
                if (dv != null)
                    count = dv.Count;
                else
                {
                    ICollection coll = (dataSource as ICollection);
                    if (coll != null)
                        count = coll.Count;
                }
            }
            grid.AllowPaging = (count > grid.PageSize);
        }

        internal static bool ShowError(Exception exception, HtmlGenericControl errorDiv)
        {
            if (exception != null)
            {
                if (errorDiv != null)
                {
                    if (exception.InnerException != null)
                        errorDiv.InnerHtml = exception.InnerException.Message;
                    else
                        errorDiv.InnerHtml = exception.Message;
                    errorDiv.Visible = true;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Public Methods

        public static string GetHyperlink(string navigateUrl, string text)
        {
            return GetHyperlink(navigateUrl, text, null);
        }

        public static string GetHyperlink(string navigateUrl, string text, string toolTip)
        {
            return GetHyperlink(navigateUrl, text, toolTip, "_blank");
        }

        public static string GetHyperlink(string navigateUrl, string text, string toolTip, string target)
        {
            if (string.IsNullOrEmpty(navigateUrl))
                return text;
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "<a {2}href=\"{0}\"{3}>{1}</a>", navigateUrl, text
                    , (string.IsNullOrEmpty(target) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "target=\"{0}\" ", target))
                    , (string.IsNullOrEmpty(toolTip) ? string.Empty : string.Format(CultureInfo.InvariantCulture, " title=\"{0}\"", HttpUtility.HtmlAttributeEncode(toolTip))));
            }
        }

        public static void TimeZoneListDataBind(ListControl timeZoneList, string selectedValue, bool required)
        {
            if (timeZoneList == null) return;

            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                if (timeZoneInfo.BaseUtcOffset.TotalHours < 0)
                    timeZoneList.Items.Insert(0, new ListItem(timeZoneInfo.DisplayName, timeZoneInfo.Id));
            }

            foreach (TimeZoneInfo timeZoneInfo in timeZones)
            {
                if (timeZoneInfo.BaseUtcOffset.TotalHours >= 0)
                    timeZoneList.Items.Add(new ListItem(timeZoneInfo.DisplayName, timeZoneInfo.Id));
            }

            if (!required)
                timeZoneList.Items.Insert(0, string.Empty);

            ListItem item = timeZoneList.Items.FindByValue(selectedValue);
            if (item != null)
                item.Selected = true;
        }

        public static void TimeFormatListDataBind(ListControl list, string selectedValue, bool required)
        {
            if (list == null) return;

            if (!required)
                list.Items.Add(new ListItem(string.Empty, string.Empty));
            list.Items.Add(new ListItem(Resources.InstanceProfileControl_TimeFormat_12Hr, "0"));
            list.Items.Add(new ListItem(Resources.InstanceProfileControl_TimeFormat_24Hr, "1"));

            ListItem item = list.Items.FindByValue(selectedValue);
            if (item != null)
                item.Selected = true;
        }

        public static void DateFormatListDataBind(ListControl list, string selectedValue, bool required)
        {
            if (list == null) return;

            if (!required)
                list.Items.Add(new ListItem(string.Empty, string.Empty));
            list.Items.Add(new ListItem(Resources.InstanceProfileControl_DateFormat_0, "0"));
            list.Items.Add(new ListItem(Resources.InstanceProfileControl_DateFormat_1, "1"));

            ListItem item = list.Items.FindByValue(selectedValue);
            if (item != null)
                item.Selected = true;
        }

        public static void WorkingDaysListDataBind(ListControl list, string selectedValue)
        {
            if (list == null) return;

            string[] days = CultureInfo.CurrentUICulture.DateTimeFormat.DayNames;
            for (int x = 1; x < 8; x++)
            {
                int y = x % 7;
                list.Items.Add(new ListItem(days[y], y.ToString(CultureInfo.InvariantCulture)));
            }

            if (!string.IsNullOrEmpty(selectedValue))
            {
                if (selectedValue.Length == list.Items.Count)
                {
                    int i = 0;
                    foreach (ListItem day in list.Items)
                    {
                        day.Selected = selectedValue[i++] == '1';
                    }
                }
            }
        }

        public static string GetWorkingDays(ListControl list)
        {
            if (list == null) return string.Empty;

            string days = string.Empty;
            foreach (ListItem item in list.Items)
            {
                days = string.Concat(days, (item.Selected) ? "1" : "0");
            }

            return days;
        }

        #endregion
    }
}
