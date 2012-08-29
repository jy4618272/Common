using System;
using System.Runtime.Serialization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    /// Occurrences of this rule repeat on a yearly basis.
    /// </summary>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class YearlyRecurrenceRuleExample1
    ///     {
    ///         static void Main()
    ///         {
    ///             // Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
    ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"),
    ///                 Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment");
    ///  
    ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             RecurrenceRange range = new RecurrenceRange();
    ///             range.Start = recurringAppointment.Start;
    ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
    ///             range.MaxOccurrences = 5;
    ///  
    ///             // Creates a recurrence rule to repeat the appointment on the 1th of April each year.
    ///             YearlyRecurrenceRule rrule = new YearlyRecurrenceRule(RecurrenceMonth.April, 1, range);
    ///  
    ///             Console.WriteLine("Appointment occurrs at the following times: ");
    ///             int ix = 0;
    ///             foreach (DateTime occurrence in rrule.Occurrences)
    ///             {
    ///                 ix = ix + 1;
    ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime());
    ///             }
    ///         }
    ///     }
    /// }
    ///  
    /// /*
    /// This example produces the following results:
    ///  
    /// Appointment occurrs at the following times:
    ///  1: 4/1/2007 10:00:00 AM
    ///  2: 4/1/2008 10:00:00 AM
    ///  3: 4/1/2009 10:00:00 AM
    ///  4: 4/1/2010 10:00:00 AM
    ///  5: 4/1/2011 10:00:00 AM
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class YearlyRecurrenceRuleExample1
    ///         Shared Sub Main()
    ///             ' Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
    ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"), Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment")
    ///  
    ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             Dim range As New RecurrenceRange()
    ///             range.Start = recurringAppointment.Start
    ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
    ///             range.MaxOccurrences = 5
    ///  
    ///             ' Creates a recurrence rule to repeat the appointment on the 1th of April each year.
    ///             Dim rrule As New YearlyRecurrenceRule(RecurrenceMonth.April, 1, range)
    ///  
    ///             Console.WriteLine("Appointment occurrs at the following times: ")
    ///             Dim ix As Integer = 0
    ///             For Each occurrence As DateTime In rrule.Occurrences
    ///                 ix = ix + 1
    ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime())
    ///             Next
    ///         End Sub
    ///     End Class
    /// End Namespace
    ///  
    /// '
    /// 'This example produces the following results:
    /// '
    /// 'Appointment occurrs at the following times:
    /// ' 1: 4/1/2007 10:00:00 AM
    /// ' 2: 4/1/2008 10:00:00 AM
    /// ' 3: 4/1/2009 10:00:00 AM
    /// ' 4: 4/1/2010 10:00:00 AM
    /// ' 5: 4/1/2011 10:00:00 AM
    /// '
    ///     </code>
    /// </example>
    [Serializable]
    public class YearlyRecurrenceRule : RecurrenceRule
    {
        protected YearlyRecurrenceRule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YearlyRecurrenceRule"/> class.
        /// </summary>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class YearlyRecurrenceRuleExample1
        ///     {
        ///         static void Main()
        ///         {
        ///             // Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
        ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"),
        ///                 Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment");
        ///  
        ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             RecurrenceRange range = new RecurrenceRange();
        ///             range.Start = recurringAppointment.Start;
        ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
        ///             range.MaxOccurrences = 5;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment on the 1th of April each year.
        ///             YearlyRecurrenceRule rrule = new YearlyRecurrenceRule(RecurrenceMonth.April, 1, range);
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ");
        ///             int ix = 0;
        ///             foreach (DateTime occurrence in rrule.Occurrences)
        ///             {
        ///                 ix = ix + 1;
        ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime());
        ///             }
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Appointment occurrs at the following times:
        ///  1: 4/1/2007 10:00:00 AM
        ///  2: 4/1/2008 10:00:00 AM
        ///  3: 4/1/2009 10:00:00 AM
        ///  4: 4/1/2010 10:00:00 AM
        ///  5: 4/1/2011 10:00:00 AM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class YearlyRecurrenceRuleExample1
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"), Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 5
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment on the 1th of April each year.
        ///             Dim rrule As New YearlyRecurrenceRule(RecurrenceMonth.April, 1, range)
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ")
        ///             Dim ix As Integer = 0
        ///             For Each occurrence As DateTime In rrule.Occurrences
        ///                 ix = ix + 1
        ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime())
        ///             Next
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Appointment occurrs at the following times:
        /// ' 1: 4/1/2007 10:00:00 AM
        /// ' 2: 4/1/2008 10:00:00 AM
        /// ' 3: 4/1/2009 10:00:00 AM
        /// ' 4: 4/1/2010 10:00:00 AM
        /// ' 5: 4/1/2011 10:00:00 AM
        /// '
        ///     </code>
        /// </example>
        /// <param name="month">The month in which the event recurs.</param>
        /// <param name="dayOfMonth">The day of month on which the event recurs.</param>
        /// <param name="range">The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.</param>
        public YearlyRecurrenceRule(RecurrenceMonth month, int dayOfMonth, RecurrenceRange range)
            : this(month, dayOfMonth, 0, RecurrenceDay.EveryDay, range)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YearlyRecurrenceRule"/> class.
        /// </summary>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class YearlyRecurrenceRuleExample2
        ///     {
        ///         static void Main()
        ///         {
        ///             // Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
        ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"),
        ///                 Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment");
        ///  
        ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             RecurrenceRange range = new RecurrenceRange();
        ///             range.Start = recurringAppointment.Start;
        ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
        ///             range.MaxOccurrences = 5;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment on the second monday of April each year.
        ///             YearlyRecurrenceRule rrule = new YearlyRecurrenceRule(2, RecurrenceMonth.April, RecurrenceDay.Monday, range);
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ");
        ///             int ix = 0;
        ///             foreach (DateTime occurrence in rrule.Occurrences)
        ///             {
        ///                 ix = ix + 1;
        ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime());
        ///             }
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Appointment occurrs at the following times:
        ///  1: 4/9/2007 10:00:00 AM
        ///  2: 4/14/2008 10:00:00 AM
        ///  3: 4/13/2009 10:00:00 AM
        ///  4: 4/12/2010 10:00:00 AM
        ///  5: 4/11/2011 10:00:00 AM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class YearlyRecurrenceRuleExample2
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 4/1/2007 10:00 AM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("4/1/2007 10:00 AM"), Convert.ToDateTime("4/1/2007 10:30 AM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 5
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment on the second monday of April each year.
        ///             Dim rrule As New YearlyRecurrenceRule(2, RecurrenceMonth.April, RecurrenceDay.Monday, range)
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ")
        ///             Dim ix As Integer = 0
        ///             For Each occurrence As DateTime In rrule.Occurrences
        ///                 ix = ix + 1
        ///                 Console.WriteLine("{0,2}: {1}", ix, occurrence.ToLocalTime())
        ///             Next
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Appointment occurrs at the following times:
        /// ' 1: 4/9/2007 10:00:00 AM
        /// ' 2: 4/14/2008 10:00:00 AM
        /// ' 3: 4/13/2009 10:00:00 AM
        /// ' 4: 4/12/2010 10:00:00 AM
        /// ' 5: 4/11/2011 10:00:00 AM
        /// '
        ///     </code>
        /// </example>
        /// <param name="dayOrdinal">The day ordinal modifier. See <see cref="RecurrencePattern.DayOrdinal"/> for additional information.</param>
        /// <param name="month">The month in which the event recurs.</param>
        /// <param name="daysOfWeekMask">A bit mask that specifies the week days on which the event recurs.</param>
        /// <param name="range">The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.</param>
        public YearlyRecurrenceRule(int dayOrdinal, RecurrenceMonth month, RecurrenceDay daysOfWeekMask, RecurrenceRange range)
            : this(month, -1, dayOrdinal, daysOfWeekMask, range)
        {
        }

        private YearlyRecurrenceRule(RecurrenceMonth month, int dayOfMonth, int dayOrdinal, RecurrenceDay daysOfWeekMask, RecurrenceRange range)
        {
            rulePattern.Frequency = RecurrenceFrequency.Yearly;
            rulePattern.Interval = 1;
            rulePattern.DaysOfWeekMask = daysOfWeekMask;
            rulePattern.DayOfMonth = dayOfMonth;
            rulePattern.DayOrdinal = dayOrdinal;
            rulePattern.Month = month;

            ruleRange = range;
        }

        /// <summary>
        /// Gets the day of month on which the event recurs.
        /// </summary>
        /// <value>The day of month on which the event recurs.</value>
        public int DayOfMonth
        {
            get
            {
                return rulePattern.DayOfMonth;
            }
        }

        /// <summary>
        /// Gets the day ordinal modifier. See <see cref="RecurrencePattern.DayOrdinal"/> for additional information.
        /// </summary>
        /// <value>The day ordinal modifier.</value>
        public int DayOrdinal
        {
            get
            {
                return rulePattern.DayOrdinal;
            }
        }

        /// <summary>
        /// Gets the month in which the event recurs.
        /// </summary>
        /// <value>The month in which the event recurs.</value>
        public RecurrenceMonth Month
        {
            get
            {
                return rulePattern.Month;
            }
        }

        /// <summary>
        /// Gets the bit mask that specifies the week days on which the event
        /// recurs.
        /// </summary>
        /// <seealso cref="RecurrenceDay">RecurrenceDay Enumeration</seealso>
        /// <remarks>
        ///     For additional information on how to create masks see the
        ///     <see cref="RecurrenceDay"/> documentation.
        /// </remarks>
        /// <value>A bit mask that specifies the week days on which the event recurs.</value>
        public RecurrenceDay DaysOfWeekMask
        {
            get
            {
                return rulePattern.DaysOfWeekMask;
            }
        }

        protected override DateTime GetOccurrenceStart(int index)
        {
            int month = (int) rulePattern.Month;
            DateTime next = ruleRange.Start;

            if (next.Month != month)
            {
                int year = next.Month < month ? next.Year : next.Year + 1;
                next = new DateTime(year, month, 1, next.Hour, next.Minute, next.Second, 0, next.Kind);
            }

            for (int i = 0; i < index; i++)
            {
				int year = next.Year + 1;
                next = next.AddDays(1);

                if (next.Month != month)
                {
					next = new DateTime(year, month, 1, next.Hour, next.Minute, next.Second, 0, next.Kind);
                }
            }

            return next;
        }

        protected override bool MatchAdvancedPattern(DateTime start)
        {
            bool matchDayOfMonth = (0 < rulePattern.DayOfMonth);
            if (matchDayOfMonth && (start.Day != rulePattern.DayOfMonth))
            {
                return false;
            }

            if (!MatchDayOfWeekMask(start))
            {
                return false;
            }

            if (!MatchDayOrdinal(start))
            {
                return false;
            }

            return true;
        }
    }
}
