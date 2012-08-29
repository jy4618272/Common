using System;
using System.Runtime.Serialization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>Occurrences of this rule repeat every given number of hours.</summary>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class HourlyRecurrenceRuleExample
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
    ///             // Creates a recurrence rule to repeat the appointment every 2 hours.
    ///             HourlyRecurrenceRule rrule = new HourlyRecurrenceRule(2, range);
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
    ///  1: 6/1/2007 3:30:00 PM
    ///  2: 6/1/2007 5:30:00 PM
    ///  3: 6/1/2007 7:30:00 PM
    ///  4: 6/1/2007 9:30:00 PM
    ///  5: 6/1/2007 11:30:00 PM
    ///  6: 6/2/2007 1:30:00 AM
    ///  7: 6/2/2007 3:30:00 AM
    ///  8: 6/2/2007 5:30:00 AM
    ///  9: 6/2/2007 7:30:00 AM
    /// 10: 6/2/2007 9:30:00 AM
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class HourlyRecurrenceRuleExample
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
    ///             ' Creates a recurrence rule to repeat the appointment every 2 hours.
    ///             Dim rrule As New HourlyRecurrenceRule(2, range)
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
    /// ' 1: 6/1/2007 3:30:00 PM
    /// ' 2: 6/1/2007 5:30:00 PM
    /// ' 3: 6/1/2007 7:30:00 PM
    /// ' 4: 6/1/2007 9:30:00 PM
    /// ' 5: 6/1/2007 11:30:00 PM
    /// ' 6: 6/2/2007 1:30:00 AM
    /// ' 7: 6/2/2007 3:30:00 AM
    /// ' 8: 6/2/2007 5:30:00 AM
    /// ' 9: 6/2/2007 7:30:00 AM
    /// '10: 6/2/2007 9:30:00 AM
    /// '
    ///     </code>
    /// </example>
    [Serializable]
    public class HourlyRecurrenceRule : RecurrenceRule
    {
        protected HourlyRecurrenceRule(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HourlyRecurrenceRule"/> class
        ///     with the specified interval (in hours) and <see cref="RecurrenceRange"/>.
        /// </summary>
        /// <param name="interval">The number of hours between the occurrences.</param>
        /// <param name="range">
        /// 	The <see cref="RecurrenceRange"/> instance that specifies the range of this
        ///     recurrence rule.
        /// </param>
        public HourlyRecurrenceRule(int interval, RecurrenceRange range)
        {
            rulePattern.Frequency = RecurrenceFrequency.Hourly;
            rulePattern.Interval = interval;
            rulePattern.DaysOfWeekMask = RecurrenceDay.None;
            rulePattern.DayOfMonth = 0;
            rulePattern.DayOrdinal = 0;
            rulePattern.Month = RecurrenceMonth.None;

            ruleRange = range;
        }

        /// <summary>Gets the interval (in hours) assigned to the current instance.</summary>
        /// <value>The interval (in hours) assigned to the current instance.</value>
        public int Interval
        {
            get { return rulePattern.Interval; }
        }

        protected override DateTime GetOccurrenceStart(int index)
        {
            return ruleRange.Start.Add(new TimeSpan(index * rulePattern.Interval, 0, 0));
        }

        protected override bool MatchAdvancedPattern(DateTime start)
        {
            return true;
        }
    }
}
