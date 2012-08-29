using System;
using System.Runtime.Serialization;
//using Micajah.Common.Bll.RecurringSchedule.Scheduling;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>Occurrences of this rule repeat on a weekly basis.</summary>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class WeeklyRecurrenceRuleExample
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
    ///             // Creates a recurrence rule to repeat the appointment every two weeks on Mondays and Tuesdays.
    ///             RecurrenceDay mask = RecurrenceDay.Monday | RecurrenceDay.Tuesday;
    ///             WeeklyRecurrenceRule rrule = new WeeklyRecurrenceRule(2, mask, range);
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
    ///  1: 6/4/2007 3:30:00 PM (Monday)
    ///  2: 6/5/2007 3:30:00 PM (Tuesday)
    ///  3: 6/18/2007 3:30:00 PM (Monday)
    ///  4: 6/19/2007 3:30:00 PM (Tuesday)
    ///  5: 7/2/2007 3:30:00 PM (Monday)
    ///  6: 7/3/2007 3:30:00 PM (Tuesday)
    ///  7: 7/16/2007 3:30:00 PM (Monday)
    ///  8: 7/17/2007 3:30:00 PM (Tuesday)
    ///  9: 7/30/2007 3:30:00 PM (Monday)
    /// 10: 7/31/2007 3:30:00 PM (Tuesday)
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class WeeklyRecurrenceRuleExample
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
    ///             ' Creates a recurrence rule to repeat the appointment every two weeks on Mondays and Tuesdays.
    ///             Dim mask As RecurrenceDay = RecurrenceDay.Monday Or RecurrenceDay.Tuesday
    ///             Dim rrule As New WeeklyRecurrenceRule(2, mask, range)
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
    /// ' 1: 6/4/2007 3:30:00 PM (Monday)
    /// ' 2: 6/5/2007 3:30:00 PM (Tuesday)
    /// ' 3: 6/18/2007 3:30:00 PM (Monday)
    /// ' 4: 6/19/2007 3:30:00 PM (Tuesday)
    /// ' 5: 7/2/2007 3:30:00 PM (Monday)
    /// ' 6: 7/3/2007 3:30:00 PM (Tuesday)
    /// ' 7: 7/16/2007 3:30:00 PM (Monday)
    /// ' 8: 7/17/2007 3:30:00 PM (Tuesday)
    /// ' 9: 7/30/2007 3:30:00 PM (Monday)
    /// '10: 7/31/2007 3:30:00 PM (Tuesday)
    /// '
    ///     </code>
    /// </example>
    [Serializable]
    public class WeeklyRecurrenceRule : RecurrenceRule
    {
        protected WeeklyRecurrenceRule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        ///     Initializes a new instance of <see cref="WeeklyRecurrenceRule"/> with the
        ///     specified interval, days of week bit mask and <see cref="RecurrenceRange"/>.
        /// </summary>
        /// <param name="interval">The number of weeks between the occurrences.</param>
        /// <param name="daysOfWeekMask">A bit mask that specifies the week days on which the event recurs.</param>
        /// <param name="range">
        /// 	The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.
        /// </param>
        public WeeklyRecurrenceRule(int interval, RecurrenceDay daysOfWeekMask, RecurrenceRange range)
        {
            rulePattern.Frequency = RecurrenceFrequency.Weekly;
            rulePattern.Interval = interval;
            rulePattern.DaysOfWeekMask = daysOfWeekMask;
            rulePattern.DayOfMonth = 0;
            rulePattern.DayOrdinal = 0;
            rulePattern.Month = RecurrenceMonth.None;

            ruleRange = range;
        }

		/// <summary>
		///     Initializes a new instance of <see cref="WeeklyRecurrenceRule"/> with the
		///     specified interval, days of week bit mask and <see cref="RecurrenceRange"/>.
		/// </summary>
		/// <param name="interval">The number of weeks between the occurrences.</param>
		/// <param name="daysOfWeekMask">A bit mask that specifies the week days on which the event recurs.</param>
		/// <param name="range">
		/// 	The <see cref="RecurrenceRange"/> instance that specifies the range of this rule.
		/// </param>
		/// <param name="firstDayOfWeek">
		///		The first day of week to use for calculations.
		/// </param>
		public WeeklyRecurrenceRule(int interval, RecurrenceDay daysOfWeekMask, RecurrenceRange range, DayOfWeek firstDayOfWeek)
			: this(interval, daysOfWeekMask, range)
		{
			Pattern.FirstDayOfWeek = firstDayOfWeek;
		}

        /// <summary>Gets the interval (in weeks) assigned to the current instance.</summary>
        /// <value>The interval (in weeks) assigned to the current instance.</value>
        public int Interval
        {
            get { return rulePattern.Interval; }
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
            get { return rulePattern.DaysOfWeekMask; }
        }

        protected override DateTime GetOccurrenceStart(int index)
        {
            return ruleRange.Start.Add(new TimeSpan(index, 0, 0, 0));
        }

        protected override bool MatchAdvancedPattern(DateTime start)
        {
            if ((GetWeekIndex(start) % rulePattern.Interval) != 0)
            {
                return false;
            }

            return MatchDayOfWeekMask(start);
        }

        private int GetWeekIndex(DateTime current)
        {
        	DateTime firstWeekStart = DateHelper.GetStartOfWeek(ruleRange.Start, rulePattern.FirstDayOfWeek);
			TimeSpan fromStart = current.Subtract(firstWeekStart);

            return (fromStart.Days / 7);
        }
    }
}
