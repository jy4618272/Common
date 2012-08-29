using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays the messages list.
    /// </summary>
    [ToolboxData("<{0}:MessageList runat=server></{0}:MessageList>")]
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class MessageList : WebControl
    {
        #region Members

        protected Repeater MessageRepeater;
        protected ObjectDataSource MessageListDataSource;

        #endregion

        #region Overriden Properties

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the culture in which the date and time will be formatted.
        /// </summary>
        [Category("Appearance")]
        [Description("The culture in which the date and time will be formatted.")]
        [DefaultValue(typeof(CultureInfo), "en-US")]
        public CultureInfo Culture
        {
            get
            {
                object obj = this.ViewState["Culture"];
                return ((obj == null) ? CultureInfo.CurrentCulture : (CultureInfo)obj);
            }
            set { this.ViewState["Culture"] = value; }
        }

        /// <summary>
        /// Gets or sets the string that specifies the display format for the date.
        /// </summary>
        [Category("Appearance")]
        [Description("The string that specifies the display format for the date.")]
        [DefaultValue("{0:g}")]
        public string DateTimeFormatString
        {
            get
            {
                object obj = this.ViewState["DateTimeFormatString"];
                return ((obj == null) ? "{0:g}" : (string)obj);
            }
            set { this.ViewState["DateTimeFormatString"] = value; }
        }

        /// <summary>
        /// Gets or set the number of hours that will be added to the date and time values.
        /// </summary>
        [Category("Appearance")]
        [Description("The number of hours that will be added to the date and time values.")]
        [DefaultValue(0.0)]
        public double DateTimeHoursOffset
        {
            get
            {
                object obj = ViewState["DateTimeHoursOffset"];
                return ((obj == null) ? 0 : (double)obj);
            }
            set { ViewState["DateTimeHoursOffset"] = value; }
        }

        /// <summary>
        /// Gets the style of the header section of the message in the list.
        /// </summary>
        [Category("Appearance")]
        [Description("The style of the header section of the message in the list.")]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style HeaderStyle
        {
            get
            {
                object obj = this.ViewState["HeaderStyle"];
                if (obj == null) this.ViewState["HeaderStyle"] = obj = new Style();
                return (Style)obj;
            }
        }

        /// <summary>
        /// Gets a value indicating that the control is empty.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty
        {
            get { return (this.MessagesCount == 0); }
        }

        /// <summary>
        /// Gets or sets the type of the object which the uploaded files are associated with.
        /// </summary>
        [Category("Data")]
        [Description("The type of the object which the uploaded files are associated with.")]
        [DefaultValue("")]
        public string LocalObjectType
        {
            get
            {
                object obj = this.ViewState["LocalObjectType"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { this.ViewState["LocalObjectType"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the object which the uploaded files are associated with.
        /// </summary>
        [Category("Data")]
        [Description("The unique identifier of the object which the uploaded files are associated with.")]
        [DefaultValue("")]
        public string LocalObjectId
        {
            get
            {
                object obj = this.ViewState["LocalObjectId"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { this.ViewState["LocalObjectId"] = value; }
        }

        /// <summary>
        /// Gets the messages count in the control.
        /// </summary>
        [Browsable(false)]
        public int MessagesCount
        {
            get
            {
                object obj = ViewState["MessagesCount"];
                return ((obj == null) ? 0 : (int)obj);
            }
            private set { ViewState["MessagesCount"] = value; }
        }

        /// <summary>
        /// Gets the style of the subject section of the message in the list.
        /// </summary>
        [Category("Appearance")]
        [Description("The style of the subject section of the message in the list.")]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style SubjectStyle
        {
            get
            {
                object obj = this.ViewState["SubjectStyle"];
                if (obj == null) this.ViewState["SubjectStyle"] = obj = new Style();
                return (Style)obj;
            }
        }

        /// <summary>
        /// Gets the style of the text section of the message in the list.
        /// </summary>
        [Category("Appearance")]
        [Description("The style of the text section of the message in the list.")]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style TextStyle
        {
            get
            {
                object obj = this.ViewState["TextStyle"];
                if (obj == null) this.ViewState["TextStyle"] = obj = new Style();
                return (Style)obj;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the initial message.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid? ParentMessageId
        {
            get
            {
                object obj = this.ViewState["ParentMessageId"];
                return ((obj == null) ? null : (Guid?)obj);
            }
            private set { this.ViewState["ParentMessageId"] = value; }
        }

        #endregion

        #region Private Methods

        private void MessageListDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (e == null) return;

            if (e.InputParameters.Contains("LocalObjectId")) e.InputParameters["LocalObjectId"] = this.LocalObjectId;
            if (e.InputParameters.Contains("LocalObjectType")) e.InputParameters["LocalObjectType"] = this.LocalObjectType;
        }

        private void MessageRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (!((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))) return;

            this.MessagesCount = this.MessagesCount + 1;

            DataRowView drv = e.Item.DataItem as DataRowView;
            if (drv == null) return;

            OrganizationDataSet.MessageRow messageRow = drv.Row as OrganizationDataSet.MessageRow;
            if (messageRow == null)
                return;

            if (e.Item.ItemIndex == 0)
                this.ParentMessageId = messageRow.MessageId;

            Panel subjectPanel = null;
            Literal subjectLiteral = null;
            Panel headerPanel = null;
            Panel textPanel = null;
            Literal textLiteral = null;

            try
            {

                if (!string.IsNullOrEmpty(messageRow.Subject))
                {
                    subjectPanel = new Panel();
                    subjectPanel.ApplyStyle(this.SubjectStyle);
                    subjectPanel.Width = Unit.Percentage(100);
                    e.Item.Controls.Add(subjectPanel);

                    subjectLiteral = new Literal();
                    subjectLiteral.Text = messageRow.Subject;
                    subjectPanel.Controls.Add(subjectLiteral);
                }

                headerPanel = new Panel();
                headerPanel.ApplyStyle(this.HeaderStyle);
                headerPanel.HorizontalAlign = HorizontalAlign.Right;
                headerPanel.Width = Unit.Percentage(100);
                e.Item.Controls.Add(headerPanel);

                string headerText = string.Empty;
                OrganizationDataSet.UserRow userRow = UserProvider.GetUserRow(messageRow.FromUserId, false);
                if (userRow != null) headerText = userRow.FullName;

                if (!messageRow.IsToUserIdNull())
                {
                    userRow = UserProvider.GetUserRow(messageRow.ToUserId, false);
                    if (userRow != null)
                    {
                        if (!string.IsNullOrEmpty(headerText)) headerText += Resources.MessageList_FromToSeparator;
                        headerText += userRow.FullName;
                    }
                }

                string createdTimeText = string.Format(this.Culture, this.DateTimeFormatString, messageRow.CreatedTime.AddHours(this.DateTimeHoursOffset));
                if (!string.IsNullOrEmpty(headerText))
                    headerText = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tr><td style=\"text-align: left;\">" + headerText + "</td><td style=\"text-align: right;\">" + createdTimeText + "</tr></table>";
                else
                    headerText = createdTimeText;

                headerPanel.Controls.Add(new LiteralControl(headerText));

                if (!string.IsNullOrEmpty(messageRow.Text))
                {
                    textPanel = new Panel();
                    textPanel.Width = Unit.Percentage(100);
                    textPanel.ApplyStyle(this.TextStyle);
                    e.Item.Controls.Add(textPanel);

                    textLiteral = new Literal();
                    textPanel.Controls.Add(textLiteral);
                    textLiteral.Text = messageRow.Text;
                }
            }
            finally
            {
                if (subjectPanel != null) subjectPanel.Dispose();
                if (subjectLiteral != null) subjectLiteral.Dispose();
                if (headerPanel != null) headerPanel.Dispose();
                if (textPanel != null) textPanel.Dispose();
                if (textLiteral != null) textLiteral.Dispose();
            }
        }

        #endregion

        #region Overriden Methods

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            MessageListDataSource = new ObjectDataSource();
            MessageListDataSource.ID = "MessageListDataSource";
            MessageListDataSource.TypeName = typeof(MessageProvider).FullName;
            MessageListDataSource.SelectMethod = "GetMessages";
            MessageListDataSource.SelectParameters.Add("LocalObjectId", TypeCode.String, string.Empty);
            MessageListDataSource.SelectParameters.Add("LocalObjectType", TypeCode.String, string.Empty);
            MessageListDataSource.Selecting += new ObjectDataSourceSelectingEventHandler(MessageListDataSource_Selecting);
            this.Controls.Add(MessageListDataSource);

            MessageRepeater = new Repeater();
            MessageRepeater.ID = "MessageRepeater";
            MessageRepeater.DataSourceID = MessageListDataSource.ID;
            MessageRepeater.ItemDataBound += new RepeaterItemEventHandler(MessageRepeater_ItemDataBound);
            this.Controls.Add(MessageRepeater);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            base.DataBind();
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write(this.ID);
            else
                base.RenderControl(writer);
        }

        #endregion
    }
}
