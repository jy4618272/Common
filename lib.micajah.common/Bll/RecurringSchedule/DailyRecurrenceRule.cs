using System;
using System.Runtime.Serialization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>Occurrences of this rule repeat on a daily basis.</summary>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class DailyRecurrenceRuleExample1
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
    ///             range.MaxOccurrences = 10;
    ///  
    ///             // Creates a recurrence rule to repeat the appointment every two days.
    ///             DailyRecurrenceRule rrule = new DailyRecurrenceRule(2, range);
    ///  
    ///             Console.WriteLine("Appointment occurrs at the following times: ");
    ///             int ix = 0;
    ///             foreach (DateTime occurrence in rrule.Occurrences)
    ///             {
    ///                 ix = ix + 1;
    ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek);
    ///             }
    ///         }
    ///     }
    /// }
    ///  
    /// /*
    /// This example produces the following results:
    ///  
    /// Appointment occurrs at the following times:
    ///  1: 6/1/2007 3:30:00 PM (Friday)
    ///  2: 6/3/2007 3:30:00 PM (Sunday)
    ///  3: 6/5/2007 3:30:00 PM (Tuesday)
    ///  4: 6/7/2007 3:30:00 PM (Thursday)
    ///  5: 6/9/2007 3:30:00 PM (Saturday)
    ///  6: 6/11/2007 3:30:00 PM (Monday)
    ///  7: 6/13/2007 3:30:00 PM (Wednesday)
    ///  8: 6/15/2007 3:30:00 PM (Friday)
    ///  9: 6/17/2007 3:30:00 PM (Sunday)
    /// 10: 6/19/2007 3:30:00 PM (Tuesday)
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class DailyRecurrenceRuleExample1
    ///         Shared Sub Main()
    ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
    ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
    ///  
    ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             Dim range As New RecurrenceRange()
    ///             range.Start = recurringAppointment.Start
    ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
    ///             range.MaxOccurrences = 10
    ///  
    ///             ' Creates a recurrence rule to repeat the appointment every two days.
    ///             Dim rrule As New DailyRecurrenceRule(2, range)
    ///  
    ///             Console.WriteLine("Appointment occurrs at the following times: ")
    ///             Dim ix As Integer = 0
    ///             For Each occurrence As DateTime In rrule.Occurrences
    ///                 ix = ix + 1
    ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek)
    ///             Next
    ///         End Sub
    ///     End Class
    /// End Namespace
    ///  
    /// '
    /// 'This example produces the following results:
    /// '
    /// 'Appointment occurrs at the following times:
    /// ' 1: 6/1/2007 3:30:00 PM (Friday)
    /// ' 2: 6/3/2007 3:30:00 PM (Sunday)
    /// ' 3: 6/5/2007 3:30:00 PM (Tuesday)
    /// ' 4: 6/7/2007 3:30:00 PM (Thursday)
    /// ' 5: 6/9/2007 3:30:00 PM (Saturday)
    /// ' 6: 6/11/2007 3:30:00 PM (Monday)
    /// ' 7: 6/13/2007 3:30:00 PM (Wednesday)
    /// ' 8: 6/15/2007 3:30:00 PM (Friday)
    /// ' 9: 6/17/2007 3:30:00 PM (Sunday)
    /// '10: 6/19/2007 3:30:00 PM (Tuesday)
    /// '
    ///     </code>
    /// </example>
    [Serializable]
    public class DailyRecurrenceRule : RecurrenceRule
    {
        protected DailyRecurrenceRule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class DailyRecurrenceRuleExample1
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
        ///             range.MaxOccurrences = 10;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment every two days.
        ///             DailyRecurrenceRule rrule = new DailyRecurrenceRule(2, range);
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ");
        ///             int ix = 0;
        ///             foreach (DateTime occurrence in rrule.Occurrences)
        ///             {
        ///                 ix = ix + 1;
        ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek);
        ///             }
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Appointment occurrs at the following times:
        ///  1: 6/1/2007 3:30:00 PM (Friday)
        ///  2: 6/3/2007 3:30:00 PM (Sunday)
        ///  3: 6/5/2007 3:30:00 PM (Tuesday)
        ///  4: 6/7/2007 3:30:00 PM (Thursday)
        ///  5: 6/9/2007 3:30:00 PM (Saturday)
        ///  6: 6/11/2007 3:30:00 PM (Monday)
        ///  7: 6/13/2007 3:30:00 PM (Wednesday)
        ///  8: 6/15/2007 3:30:00 PM (Friday)
        ///  9: 6/17/2007 3:30:00 PM (Sunday)
        /// 10: 6/19/2007 3:30:00 PM (Tuesday)
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class DailyRecurrenceRuleExample1
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 10
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment every two days.
        ///             Dim rrule As New DailyRecurrenceRule(2, range)
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ")
        ///             Dim ix As Integer = 0
        ///             For Each occurrence As DateTime In rrule.Occurrences
        ///                 ix = ix + 1
        ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek)
        ///             Next
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Appointment occurrs at the following times:
        /// ' 1: 6/1/2007 3:30:00 PM (Friday)
        /// ' 2: 6/3/2007 3:30:00 PM (Sunday)
        /// ' 3: 6/5/2007 3:30:00 PM (Tuesday)
        /// ' 4: 6/7/2007 3:30:00 PM (Thursday)
        /// ' 5: 6/9/2007 3:30:00 PM (Saturday)
        /// ' 6: 6/11/2007 3:30:00 PM (Monday)
        /// ' 7: 6/13/2007 3:30:00 PM (Wednesday)
        /// ' 8: 6/15/2007 3:30:00 PM (Friday)
        /// ' 9: 6/17/2007 3:30:00 PM (Sunday)
        /// '10: 6/19/2007 3:30:00 PM (Tuesday)
        /// '
        ///     </code>
        /// </example>
        /// <summary>
        ///     Initializes a new instance of <see cref="DailyRecurrenceRule"/> with the
        ///     specified interval (in days) and <see cref="RecurrenceRange"/>.
        /// </summary>
        /// <param name="interval">The number of days between the occurrences.</param>
        /// <param name="range">
        /// 	The <see cref="RecurrenceRange"/> instance that specifies the range of this
        ///     recurrence rule.
        /// </param>
        public DailyRecurrenceRule(int interval, RecurrenceRange range)
            : this(interval, RecurrenceDay.EveryDay, range)
        {
        }

        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Micajah.Common.Bll.RecurringSchedule;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class DailyRecurrenceRuleExample2
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
        ///             range.MaxOccurrences = 10;
        ///  
        ///             // Creates a recurrence rule to repeat the appointment every week day.
        ///             DailyRecurrenceRule rrule = new DailyRecurrenceRule(RecurrenceDay.WeekDays, range);
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ");
        ///             int ix = 0;
        ///             foreach (DateTime occurrence in rrule.Occurrences)
        ///             {
        ///                 ix = ix + 1;
        ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek);
        ///             }
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Appointment occurrs at the following times:
        ///  1: 6/1/2007 3:30:00 PM (Friday)
        ///  2: 6/4/2007 3:30:00 PM (Monday)
        ///  3: 6/5/2007 3:30:00 PM (Tuesday)
        ///  4: 6/6/2007 3:30:00 PM (Wednesday)
        ///  5: 6/7/2007 3:30:00 PM (Thursday)
        ///  6: 6/8/2007 3:30:00 PM (Friday)
        ///  7: 6/11/2007 3:30:00 PM (Monday)
        ///  8: 6/12/2007 3:30:00 PM (Tuesday)
        ///  9: 6/13/2007 3:30:00 PM (Wednesday)
        /// 10: 6/14/2007 3:30:00 PM (Thursday)
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Micajah.Common.Bll.RecurringSchedule
        ///  
        /// Namespace RecurrenceExamples
        ///     Class DailyRecurrenceRuleExample2
        ///         Shared Sub Main()
        ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM (local time) and lasts half an hour.
        ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
        ///  
        ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
        ///             Dim range As New RecurrenceRange()
        ///             range.Start = recurringAppointment.Start
        ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
        ///             range.MaxOccurrences = 10
        ///  
        ///             ' Creates a recurrence rule to repeat the appointment every week day.
        ///             Dim rrule As New DailyRecurrenceRule(RecurrenceDay.WeekDays, range)
        ///  
        ///             Console.WriteLine("Appointment occurrs at the following times: ")
        ///             Dim ix As Integer = 0
        ///             For Each occurrence As DateTime In rrule.Occurrences
        ///                 ix = ix + 1
        ///                 Console.WriteLine("{0,2}: {1} ({2})", ix, occurrence.ToLocalTime(), occurrence.DayOfWeek)
        ///             Next
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Appointment occurrs at the following times:
        /// ' 1: 6/1/2007 3:30:00 PM (Friday)
        /// ' 2: 6/4/2007 3:30:00 PM (Monday)
        /// ' 3: 6/5/2007 3:30:00 PM (Tuesday)
        /// ' 4: 6/6/2007 3:30:00 PM (Wednesday)
        /// ' 5: 6/7/2007 3:30:00 PM (Thursday)
        /// ' 6: 6/8/2007 3:30:00 PM (Friday)
        /// ' 7: 6/11/2007 3:30:00 PM (Monday)
        /// ' 8: 6/12/2007 3:30:00 PM (Tuesday)
        /// ' 9: 6/13/2007 3:30:00 PM (Wednesday)
        /// '10: 6/14/2007 3:30:00 PM (Thursday)
        /// '
        ///     </code>
        /// </example>
        /// <summary>
        ///     Initializes a new instance of <see cref="DailyRecurrenceRule"/> with the
        ///     specified days of week bit mask and <see cref="RecurrenceRange"/>.
        /// </summary>
        /// <param name="daysOfWeekMask">A bit mask that specifies the week days on which the event recurs.</param>
        /// <param name="range">
        /// 	The <see cref="RecurrenceRange"/> instance that specifies the range of this
        ///     recurrence rule.
        /// </param>
        public DailyRecurrenceRule(RecurrenceDay daysOfWeekMask, RecurrenceRange range)
            : this(1, daysOfWeekMask, range)
        {
        }

        private DailyRecurrenceRule(int interval, RecurrenceDay daysOfWeekMask, RecurrenceRange range)
        {
            rulePattern.Frequency = RecurrenceFrequency.Daily;
            rulePattern.Interval = interval;
            rulePattern.DaysOfWeekMask = daysOfWeekMask;
            rulePattern.DayOfMonth = 0;
            rulePattern.DayOrdinal = 0;
            rulePattern.Month = RecurrenceMonth.None;

            ruleRange = range;
        }

        /// <summary>Gets the interval (in days) between the occurrences.</summary>
        /// <value>The interval (in days) between the occurrences.</value>
        public int Interval
        {
            get { return rulePattern.Interval; }
        }

        /// <summary>
        /// Gets or sets the bit mask that specifies the week days on which the event
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
            get { return rulePattern.DaysOfWeekMask; }
            set { rulePattern.DaysOfWeekMask = value; }
        }

        protected override DateTime GetOccurrenceStart(int index)
        {
            return ruleRange.Start.Add(new TimeSpan(index * rulePattern.Interval, 0, 0, 0));
        }

        protected override bool MatchAdvancedPattern(DateTime start)
        {
            return MatchDayOfWeekMask(start);
        }
    }
}
