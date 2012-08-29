using System;
using System.Runtime.Serialization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    /// Occurrences of this rule repeat on a monthly basis.
    /// </summary>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class MonthlyRecurrenceRuleExample1
    ///     {
    ///         static void Main()
    ///         {
    ///             // Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
    ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"),
    ///                 Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment");
    ///  
    ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             RecurrenceRange range = new RecurrenceRange();
    ///             range.Start = recurringAppointment.Start;
    ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
    ///             range.MaxOccurrences = 5;
    ///  
    ///             // Creates a recurrence rule to repeat the appointment on the 5th day of every month.
    ///             MonthlyRecurrenceRule rrule = new MonthlyRecurrenceRule(5, 1, range);
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
    ///  1: 6/5/2007 3:30:00 PM
    ///  2: 7/5/2007 3:30:00 PM
    ///  3: 8/5/2007 3:30:00 PM
    ///  4: 9/5/2007 3:30:00 PM
    ///  5: 10/5/2007 3:30:00 PM
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class MonthlyRecurrenceRuleExample1
    ///         Shared Sub Main()
    ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
    ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
    ///  
    ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             Dim range As New RecurrenceRange()
    ///             range.Start = recurringAppointment.Start
    ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
    ///             range.MaxOccurrences = 5
    ///  
    ///             ' Creates a recurrence rule to repeat the appointment on the 5th day of every month.
    ///             Dim rrule As New MonthlyRecurrenceRule(5, 1, range)
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
    /// ' 1: 6/5/2007 3:30:00 PM
    /// ' 2: 7/5/2007 3:30:00 PM
    /// ' 3: 8/5/2007 3:30:00 PM
    /// ' 4: 9/5/2007 3:30:00 PM
    /// ' 5: 10/5/2007 3:30:00 PM
    /// '
    ///     </code>
    /// </example>
    [Serializable]
    public class MonthlyRecurrenceRule : RecurrenceRule
    {
        protected MonthlyRecurrenceRule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyRecurrenceRule"/> class.
        /// </summary>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class MonthlyRecurrenceRuleExample1
        ///     {
        ///         static void Main()
        ///         {
        ///             // Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"),
        ///                 Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment");
        ///  
        ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             RecurrenceRange range = new RecurrenceRange();
        ///             range.Start = recurringAppointment.Start;
        ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
        ///             range.MaxOccurrences = 5;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment on the 5th day of every month.
        ///             MonthlyRecurrenceRule rrule = new MonthlyRecurrenceRule(5, 1, range);
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
        ///  1: 6/5/2007 3:30:00 PM
        ///  2: 7/5/2007 3:30:00 PM
        ///  3: 8/5/2007 3:30:00 PM
        ///  4: 9/5/2007 3:30:00 PM
        ///  5: 10/5/2007 3:30:00 PM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class MonthlyRecurrenceRuleExample1
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 5
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment on the 5th day of every month.
        ///             Dim rrule As New MonthlyRecurrenceRule(5, 1, range)
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
        /// ' 1: 6/5/2007 3:30:00 PM
        /// ' 2: 7/5/2007 3:30:00 PM
        /// ' 3: 8/5/2007 3:30:00 PM
        /// ' 4: 9/5/2007 3:30:00 PM
        /// ' 5: 10/5/2007 3:30:00 PM
        /// '
        ///     </code>
        /// </example>
        /// <param name="dayOfMonth">The day of month on which the event recurs.</param>
        /// <param name="interval">The interval (in months) between the occurrences.</param>
        /// <param name="range">The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.</param>
        public MonthlyRecurrenceRule(int dayOfMonth, int interval, RecurrenceRange range)
            : this(dayOfMonth, interval, 0, RecurrenceDay.None, range)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyRecurrenceRule"/> class.
        /// </summary>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class MonthlyRecurrenceRuleExample2
        ///     {
        ///         static void Main()
        ///         {
        ///             // Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"),
        ///                 Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment");
        ///  
        ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             RecurrenceRange range = new RecurrenceRange();
        ///             range.Start = recurringAppointment.Start;
        ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
        ///             range.MaxOccurrences = 5;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment on the last monday of every two months.
        ///             MonthlyRecurrenceRule rrule = new MonthlyRecurrenceRule(-1, RecurrenceDay.Monday, 2, range);
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
        ///  1: 6/25/2007 3:30:00 PM
        ///  2: 8/27/2007 3:30:00 PM
        ///  3: 10/29/2007 2:30:00 PM
        ///  4: 12/31/2007 2:30:00 PM
        ///  5: 2/25/2008 2:30:00 PM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class MonthlyRecurrenceRuleExample2
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 5
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment on the last monday of every two months.
        ///             Dim rrule As New MonthlyRecurrenceRule(-1, RecurrenceDay.Monday, 2, range)
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
        /// ' 1: 6/25/2007 3:30:00 PM
        /// ' 2: 8/27/2007 3:30:00 PM
        /// ' 3: 10/29/2007 2:30:00 PM
        /// ' 4: 12/31/2007 2:30:00 PM
        /// ' 5: 2/25/2008 2:30:00 PM
        /// '
        ///     </code>
        /// </example>
        /// <param name="dayOrdinal">The day ordinal modifier. See <see cref="RecurrencePattern.DayOrdinal"/> for additional information.</param>
        /// <param name="daysOfWeekMask">A bit mask that specifies the week days on which the event recurs.</param>
        /// <param name="interval">The interval (in months) between the occurrences.</param>
        /// <param name="range">The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.</param>
        public MonthlyRecurrenceRule(int dayOrdinal, RecurrenceDay daysOfWeekMask, int interval, RecurrenceRange range)
            : this(0, interval, dayOrdinal, daysOfWeekMask, range)
        {
        }

        private MonthlyRecurrenceRule(int dayOfMonth, int interval, int dayOrdinal, RecurrenceDay daysOfWeekMask, RecurrenceRange range)
        {
            rulePattern.Frequency = RecurrenceFrequency.Monthly;
            rulePattern.Interval = interval;
            rulePattern.DaysOfWeekMask = daysOfWeekMask;
            rulePattern.DayOfMonth = dayOfMonth;
            rulePattern.DayOrdinal = dayOrdinal;
            rulePattern.Month = RecurrenceMonth.None;

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
        /// <seealso cref="RecurrencePattern.DayOrdinal"/>
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

        /// <summary>Gets the interval (in months) between the occurrences.</summary>
        /// <value>The interval (in months) between the occurrences.</value>
        public int Interval
        {
            get { return rulePattern.Interval; }
        }

        protected override DateTime GetOccurrenceStart(int index)
        {
            return ruleRange.Start.AddDays(index);
        }

        protected override bool MatchAdvancedPattern(DateTime start)
        {
            if ((GetMonthIndex(start) % rulePattern.Interval) != 0)
            {
                return false;
            }

            bool matchDayOfMonth = (0 < rulePattern.DayOfMonth);
            if (matchDayOfMonth)
            {
                return start.Day == rulePattern.DayOfMonth;
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

        private int GetMonthIndex(DateTime start)
        {
            return (ruleRange.Start.Month - start.Month);
        }
    }
}
