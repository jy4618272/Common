using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Represents the list of the controls separated by specified delimiter.
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class ControlList : System.Web.UI.Control
    {
        #region Members

        private const string DefaultDelimiter = " &nbsp;|&nbsp; ";

        private ArrayList m_InnerList;
        private string m_Delimiter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Micajah.Common.WebControls.ControlList class.
        /// </summary>
        public ControlList()
        {
            m_InnerList = new ArrayList();
            m_Delimiter = DefaultDelimiter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the list delimiter.
        /// </summary>
        [DefaultValue(DefaultDelimiter)]
        public string Delimiter
        {
            get { return m_Delimiter; }
            set { m_Delimiter = value; }
        }

        /// <summary>
        /// Gets a items count of the links list.
        /// </summary>
        [DefaultValue(0)]
        public int Count
        {
            get { return m_InnerList.Count; }
        }

        /// <summary>
        /// Gets or sets whether the controls are displayed vertically or horizontally.
        /// One of the System.Web.UI.WebControls.RepeatDirection values. The default is System.Web.UI.WebControls.Horizontal.
        /// </summary>
        [DefaultValue(RepeatDirection.Horizontal)]
        public RepeatDirection RepeatDirection
        {
            get
            {
                object obj = ViewState["RepeatDirection"];
                return ((obj == null) ? RepeatDirection.Horizontal : (RepeatDirection)obj);
            }
            set { ViewState["RepeatDirection"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating that the order number of the controls are rendered,
        /// if the RepeatDirection is System.Web.UI.WebControls.Horizontal.
        /// </summary>
        [DefaultValue(false)]
        public bool ShowOrderNumber
        {
            get
            {
                object obj = ViewState["ShowOrderNumber"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["ShowOrderNumber"] = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a control to the end of the list.
        /// </summary>
        /// <param name="value">The control to add.</param>
        public void Add(Control value)
        {
            if (value != null) m_InnerList.Add(value);
        }

        /// <summary>
        /// Inserts a control into the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The control to insert.</param>
        public void Insert(int index, Control value)
        {
            if (value != null) m_InnerList.Insert(index, value);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (m_InnerList.Count == 0) return;

            RepeatDirection direction = this.RepeatDirection;
            int Index = 0;
            bool orderNumber = ShowOrderNumber;

            HtmlGenericControl ctl = null;
            try
            {
                if (direction == RepeatDirection.Vertical)
                    ctl = new HtmlGenericControl("ol");

                foreach (System.Web.UI.Control link in m_InnerList)
                {
                    if (link.Visible)
                    {
                        if (direction == RepeatDirection.Horizontal)
                        {
                            if (Index > 0) Controls.Add(new LiteralControl(m_Delimiter));
                            if (orderNumber) Controls.Add(new LiteralControl(string.Concat(Index + 1, ".&nbsp;")));
                            Controls.Add(link);
                        }
                        else if (direction == RepeatDirection.Vertical)
                        {
                            using (HtmlGenericControl li = new HtmlGenericControl("li"))
                            {
                                li.Controls.Add(link);
                                ctl.Controls.Add(li);
                            }
                        }
                    }
                    Index++;
                }
            }
            finally
            {
                if (ctl != null)
                {
                    Controls.Add(ctl);
                    ctl.Dispose();
                }
            }
        }

        #endregion
    }
}
