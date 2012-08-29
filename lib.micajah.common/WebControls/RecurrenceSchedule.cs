using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Bll.RecurringSchedule;

namespace Micajah.Common.WebControls
{
    [ToolboxData("<{0}:RecurrenceSchedule runat=server></{0}:RecurrenceSchedule>")]
    [DefaultEvent("Updated")]
    [ParseChildren(true)]
    public class RecurrenceSchedule : Control, INamingContainer
    {
        #region Members
        private RecurrenceScheduleInternalControl m_RecurrenceScheduleInternalControl;
        private PlaceHolder m_PlaceHolderMain;
        private bool m_ChildControlsCreated;
        #endregion

        #region Events
        public event EventHandler<EventArgs> Cancel;
        public event EventHandler<CancelEventArgs> Updating;
        public event EventHandler<RecurringScheduleEventArgs> Updated;
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the recurring schedule identifier.
        /// </summary>
        [Browsable(false)]
        public Guid RecurringScheduleId
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.RecurringScheduleId;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.RecurringScheduleId = value;

            }
        }

        /// <summary>
        /// Gets or sets the start date of the schedule.
        /// </summary>
        [Browsable(false)]
        public DateTime DateStart
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.Start;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.Start = value;
            }
        }

        /// <summary>
        /// Gets or sets the end date of the schedule.
        /// </summary>
        [Browsable(false)]
        public DateTime DateEnd
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.End;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.End = value;
            }
        }

        /// <summary>
        /// Gets or sets the duration of the schedule.
        /// </summary>
        [Browsable(false)]
        public TimeSpan Duration
        {
            get
            {
                return this.DateEnd - this.DateStart;
            }
        }

        /// <summary>
        /// Gets or sets the schedule name/description.
        /// </summary>
        [Category("Appearance")]
        [Description("The the schedule description.")]
        [DefaultValue("")]
        public string Description
        {
            get
            {
                if (this.DesignMode)
                    return string.Empty;
                else
                {
                    EnsureChildControls();
                    return m_RecurrenceScheduleInternalControl.Name;
                }
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the schedule recurrence rule.
        /// </summary>
        [Category("Appearance")]
        [Description("The schedule recurrence rule.")]
        [DefaultValue("")]
        public string RecurrenceRule
        {
            get
            {
                if (this.DesignMode)
                    return string.Empty;
                else
                {
                    EnsureChildControls();
                    return m_RecurrenceScheduleInternalControl.RecurrenceRule;
                }
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.RecurrenceRule = value;
            }
        }

        /// <summary>
        /// Gets or sets the local Entity type of the schedule recurrence.
        /// </summary>
        [Category("Appearance")]
        [Description("The local Entity Type.")]
        [DefaultValue("")]
        public string LocalEntityType
        {
            get
            {
                if (this.DesignMode)
                    return string.Empty;
                else
                {
                    EnsureChildControls();
                    return m_RecurrenceScheduleInternalControl.LocalEntityType;
                }
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.LocalEntityType = value;

            }
        }


        /// <summary>
        /// Gets or sets the local Entity type of the schedule recurrence.
        /// </summary>
        [Category("Appearance")]
        [Description("The local Entity Identifier.")]
        [DefaultValue("")]
        public string LocalEntityId
        {
            get
            {
                if (this.DesignMode)
                    return string.Empty;
                else
                {
                    EnsureChildControls();
                    return m_RecurrenceScheduleInternalControl.LocalEntityId;
                }
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.LocalEntityId = value;

            }
        }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        [Browsable(false)]
        public Guid OrganizationId
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.OrganizationId;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.OrganizationId = value;

            }
        }

        /// <summary>
        /// Gets or sets the instance identifier.
        /// </summary>
        [Browsable(false)]
        public Guid? InstanceId
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.InstanceId;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.InstanceId = value;
            }
        }

        /// <summary>
        /// Gets or sets the first day of week.
        /// </summary>
        [Browsable(false)]
        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.FirstDayOfWeek;
            }
            set
            {
                EnsureChildControls();
                m_RecurrenceScheduleInternalControl.FirstDayOfWeek = value;
            }
        }

        /// <summary>
        /// Gets or sets the frequency of the recurring schedule.
        /// </summary>
        [Browsable(false)]
        public RecurrenceFrequency Frequency
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.Frequency;
            }
        }

        /// <summary>
        /// Gets or sets the interval of the recurring schedule.
        /// </summary>
        [Browsable(false)]
        public int Interval
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.Interval;
            }
        }

        /// <summary>
        /// Gets or sets the mask of the days of week for the recurring schedule.
        /// </summary>
        [Browsable(false)]
        public RecurrenceDay DaysOfWeekMask
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.DaysOfWeekMask;
            }
        }

        /// <summary>
        /// Gets or sets the date range of the recurring schedule.
        /// </summary>
        [Browsable(false)]
        public RecurrenceRange Range
        {
            get
            {
                EnsureChildControls();
                return m_RecurrenceScheduleInternalControl.Range;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating that the cancel button is visibled.
        /// </summary>
        [Category("Behavior")]
        [Description("Visible state of the cancel button.")]
        [DefaultValue(false)]
        public bool VisibleCancelButton
        {
            get
            {
                object obj = ViewState["VisibleCancelButton"];
                return ((obj == null) ? false : (bool)obj);
            }
            set
            {
                ViewState["VisibleCancelButton"] = value;
            }
        }
        #endregion

        #region Private Methods
        private void RecurrenceSchedule_Cancel(object sender, EventArgs e)
        {
            if (this.Cancel != null) Cancel(sender, e);
        }
        private void RecurrenceSchedule_Updating(object sender, CancelEventArgs e)
        {
            if (this.Updating != null) Updating(sender, e);
        }
        private void RecurrenceSchedule_Updated(object sender, RecurringScheduleEventArgs e)
        {
            if (this.Updated != null) Updated(sender, e);
        }
        #endregion

        #region Override Methods

        /// <summary>
        /// Determines whether the server control contains child controls. If it does not, it creates child controls.
        /// </summary>
        protected override void EnsureChildControls()
        {
            if (!m_ChildControlsCreated) CreateChildControls();
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (this.Page != null)
            {
                Controls.Clear();
                m_PlaceHolderMain = new PlaceHolder();
                m_RecurrenceScheduleInternalControl = new RecurrenceScheduleInternalControl();
                m_RecurrenceScheduleInternalControl = (RecurrenceScheduleInternalControl)this.Page.LoadControl(ResourceProvider.RecurringScheduleControlVirtualPath);
                m_RecurrenceScheduleInternalControl.ID = "rs";
                m_RecurrenceScheduleInternalControl.VisibleCancelButton = this.VisibleCancelButton;
                m_RecurrenceScheduleInternalControl.Cancel += new EventHandler<EventArgs>(RecurrenceSchedule_Cancel);
                m_RecurrenceScheduleInternalControl.Updating += new EventHandler<CancelEventArgs>(RecurrenceSchedule_Updating);
                m_RecurrenceScheduleInternalControl.Updated += new EventHandler<RecurringScheduleEventArgs>(RecurrenceSchedule_Updated);
                m_PlaceHolderMain.Controls.Add(m_RecurrenceScheduleInternalControl);
                Controls.Add(m_PlaceHolderMain);
                m_ChildControlsCreated = true;
            }
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
            {
                //m_RecurrenceScheduleInternalControl.RenderControl(writer);
                writer.Write("<span style='font-size: 14px;'>The Recurring Schedule Appointment control</span>");
            }
            else
            {
                EnsureChildControls();
                base.Render(writer);
            }
        }
        #endregion
    }
}
