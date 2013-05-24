using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Security;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a bread crumbs in a Web Forms page.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class Breadcrumbs : Control
    {
        #region Members

        private HtmlGenericControl m_Container;
        private Table m_Table;
        private Unit m_ColumnWidth;
        private bool m_ShowBreadCrumbs;
        private Micajah.Common.Pages.MasterPage m_MicajahMasterPage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BreadCrumbs class.
        /// </summary>
        public Breadcrumbs()
        {
            m_ColumnWidth = new Unit(-1);
            m_ShowBreadCrumbs = true;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the width of the table's columns.
        /// </summary>
        private Unit ColumnWidth
        {
            get
            {
                if (m_ColumnWidth.Value == -1)
                {
                    int columnCount = 0;
                    if (UserContext.Breadcrumbs.Count > 0) columnCount++;
                    if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.CenterHtml)) columnCount++;
                    if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.RightHtml)) columnCount++;
                    m_ColumnWidth = ((columnCount > 1) ? Unit.Percentage(Convert.ToInt32(100 / columnCount)) : Unit.Empty);
                }
                return m_ColumnWidth;
            }
        }

        private Micajah.Common.Pages.MasterPage MicajahMasterPage
        {
            get
            {
                if (m_MicajahMasterPage == null)
                {
                    System.Web.UI.MasterPage master = this.Page.Master;
                    while (master != null)
                    {
                        if (master is Micajah.Common.Pages.MasterPage)
                        {
                            m_MicajahMasterPage = (master as Micajah.Common.Pages.MasterPage);
                            return m_MicajahMasterPage;
                        }
                        master = master.Master;
                    }
                }
                return m_MicajahMasterPage;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the left cell which contains the breadcrumbs itself.
        /// </summary>
        private TableCell CreateLeftCell()
        {
            TableCell td = null;
            Label lbl = null;

            try
            {
                td = new TableCell();
                td.Width = ColumnWidth;
                int i = 0;
                int count = UserContext.Breadcrumbs.Count;

                foreach (Micajah.Common.Bll.Action item in UserContext.Breadcrumbs)
                {
                    if (i == (count - 1))
                    {
                        lbl = new Label();
                        lbl.CssClass = "L";
                        lbl.Text = item.CustomName;
                        td.Controls.Add(lbl);
                    }
                    else
                    {
                        td.Controls.Add(new Link(item.CustomName, item.CustomAbsoluteNavigateUrl, item.CustomDescription));
                        td.Controls.Add(new LiteralControl("&nbsp;&nbsp;|&nbsp;&nbsp;"));
                    }
                    i++;
                }

                return td;
            }
            finally
            {
                if (td != null) td.Dispose();
                if (lbl != null) lbl.Dispose();
            }
        }

        private void EnsureBreadcrumbs()
        {
            if (this.HasControls()) return;

            TableRow tr = null;

            try
            {
                m_Container = new HtmlGenericControl("div");
                m_Container.Attributes["id"] = "Mp_B";
                m_Container.Attributes["class"] = "Mp_B";

                if (this.ShowBreadcrumbs)
                {
                    m_Table = new Table();
                    m_Table.CellPadding = m_Table.CellSpacing = 0;
                    tr = new TableRow();

                    if (UserContext.Breadcrumbs.Count > 0)
                    {
                        using (TableCell td = CreateLeftCell())
                        {
                            tr.Cells.Add(td);
                        }
                    }

                    // Creates the center cell.
                    if (!string.IsNullOrEmpty(this.MicajahMasterPage.BreadcrumbsCenterHtml))
                    {
                        using (TableCell td = new TableCell())
                        {
                            td.Width = ColumnWidth;
                            td.HorizontalAlign = HorizontalAlign.Center;
                            td.Controls.Add(new LiteralControl(this.MicajahMasterPage.BreadcrumbsCenterHtml));
                            tr.Cells.Add(td);
                        }
                    }

                    // Creates the right cell.
                    if (!string.IsNullOrEmpty(this.MicajahMasterPage.BreadcrumbsRightHtml))
                    {
                        using (TableCell td = new TableCell())
                        {
                            td.Width = ColumnWidth;
                            td.HorizontalAlign = HorizontalAlign.Right;
                            td.Controls.Add(new LiteralControl(this.MicajahMasterPage.BreadcrumbsRightHtml));
                            tr.Cells.Add(td);
                        }
                    }

                    m_Table.Rows.Add(tr);
                    m_Container.Controls.Add(m_Table);

                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                    {
                        if (this.MicajahMasterPage.VisibleHelpLink)
                            m_Table.Rows[0].Cells[m_Table.Rows[0].Cells.Count - 1].Style[HtmlTextWriterStyle.PaddingRight] = "85px";
                    }
                }
                else
                    m_Container.Style[HtmlTextWriterStyle.Display] = "none";

                Controls.Add(m_Container);
            }
            finally
            {
                if (tr != null) tr.Dispose();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value that indicates whether the bread crumbs are rendered.
        /// </summary>
        public bool ShowBreadcrumbs
        {
            get
            {
                return m_ShowBreadCrumbs && (UserContext.Breadcrumbs.Count > 0
                    || (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.CenterHtml))
                    || (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.RightHtml)));
            }
            set { m_ShowBreadCrumbs = value; }
        }

        #endregion

        #region Override Methods

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                this.EnsureBreadcrumbs();
                m_Container.RenderControl(writer);
            }
        }

        #endregion

        #region Public Methods

        public string RenderContent()
        {
            this.EnsureBreadcrumbs();
            if (m_Table != null)
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture))
                {
                    HtmlTextWriter w = new HtmlTextWriter(sw);
                    m_Table.RenderControl(w);
                    return sb.ToString();
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
