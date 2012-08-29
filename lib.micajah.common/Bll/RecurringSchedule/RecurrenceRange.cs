using System;
//using Micajah.Common.Bll.RecurringSchedule.Scheduling;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    /// 	<para>
    ///         Specifies the time frame for which given <see cref="RecurrenceRule"/> is
    ///         active. It consists of the start time of the event, it's duration and optional
    ///         limits.
    ///     </para>
    /// </summary>
    /// <remarks>
    /// 	<para>
    ///         Limits for both occurrence count and end date can be specified via the
    ///         <see cref="MaxOccurrences"/> and <see cref="RecursUntil"/>
    ///         properties.
    ///     </para>
    /// 	<para>
    ///         Start and EventDuration properties refer to the recurring event's start and
    ///         duration. In the context of <see cref="RadScheduler"/> they are usually
    ///         derived from <see cref="Appointment.Start"/> and <see cref="Appointment.End"/>.
    ///     </para>
    /// </remarks>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class RecurrenceRangeExample
    ///     {
    ///         static void Main()
    ///         {
    ///             // Creates a sample appointment that starts at 6/1/2007 3:30 PM and lasts half an hour.
    ///             Appointment recurringAppointment = new Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"),
    ///                 Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment");
    ///  
    ///             // Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             RecurrenceRange range = new RecurrenceRange();
    ///             range.Start = recurringAppointment.Start;
    ///             range.EventDuration = recurringAppointment.End - recurringAppointment.Start;
    ///             range.MaxOccurrences = 10;
    ///  
    ///             // Creates a daily recurrence rule for the appointment.
    ///             DailyRecurrenceRule rrule = new DailyRecurrenceRule(1, range);
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
    ///  2: 6/2/2007 3:30:00 PM
    ///  3: 6/3/2007 3:30:00 PM
    ///  4: 6/4/2007 3:30:00 PM
    ///  5: 6/5/2007 3:30:00 PM
    ///  6: 6/6/2007 3:30:00 PM
    ///  7: 6/7/2007 3:30:00 PM
    ///  8: 6/8/2007 3:30:00 PM
    ///  9: 6/9/2007 3:30:00 PM
    /// 10: 6/10/2007 3:30:00 PM
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class RecurrenceRangeExample
    ///         Shared Sub Main()
    ///             ' Creates a sample appointment that starts at 6/1/2007 3:30 PM and lasts half an hour.
    ///             Dim recurringAppointment As New Appointment("1", Convert.ToDateTime("6/1/2007 3:30 PM"), Convert.ToDateTime("6/1/2007 4:00 PM"), "Sample appointment")
    ///  
    ///             ' Creates a recurrence range, that specifies a limit of 10 occurrences for the appointment.
    ///             Dim range As New RecurrenceRange()
    ///             range.Start = recurringAppointment.Start
    ///             range.EventDuration = recurringAppointment.[End] - recurringAppointment.Start
    ///             range.MaxOccurrences = 10
    ///  
    ///             ' Creates a daily recurrence rule for the appointment.
    ///             Dim rrule As New DailyRecurrenceRule(1, range)
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
    /// ' 2: 6/2/2007 3:30:00 PM
    /// ' 3: 6/3/2007 3:30:00 PM
    /// ' 4: 6/4/2007 3:30:00 PM
    /// ' 5: 6/5/2007 3:30:00 PM
    /// ' 6: 6/6/2007 3:30:00 PM
    /// ' 7: 6/7/2007 3:30:00 PM
    /// ' 8: 6/8/2007 3:30:00 PM
    /// ' 9: 6/9/2007 3:30:00 PM
    /// '10: 6/10/2007 3:30:00 PM
    /// '
    ///     </code>
    /// </example>
    public class RecurrenceRange : IEquatable<RecurrenceRange>
    {
        private DateTime _start = DateTime.MinValue;
        private TimeSpan _eventDuration = TimeSpan.Zero;
        private DateTime _recursUntil = DateTime.MaxValue;
        private int _maxOccurrences = int.MaxValue;


        /// <summary>The start of the recurring event.</summary>
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
				_start = DateHelper.AssumeUtc(value);
            }
        }

        /// <summary>The duration of the recurring event.</summary>
        public TimeSpan EventDuration
        {
            get
            {
                return _eventDuration;
            }
            set
            {
                _eventDuration = value;
            }
        }

        /// <summary>
        /// Optional end date for the recurring appointment. Defaults to no end date
        /// (DateTime.MaxValue).
        /// </summary>
        public DateTime RecursUntil
        {
            get
            {
                return _recursUntil;
            }
            set
            {
				_recursUntil = value < DateTime.MaxValue ? DateHelper.AssumeUtc(value) : DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Optional limit for the number of occurrences. Defaults to no limit
        /// (Int32.MaxInt).
        /// </summary>
        public int MaxOccurrences
        {
            get
            {
                return _maxOccurrences;
            }
            set
            {
                _maxOccurrences = value;
            }
        }


        /// <summary>
        ///     Overloaded. Initializes a new instance of the <see cref="RecurrenceRange"/>
        ///     class.
        /// </summary>
        public RecurrenceRange()
        {
        
        }

        /// <summary>
        ///     Overloaded. Initializes a new instance of the <see cref="RecurrenceRange"/>
        ///     class with to the specified Start, EventDuration, RecursUntil and MaxOccurrences
        ///     values.
        /// </summary>
        /// <param name="start">The start of the recurring event.</param>
        /// <param name="duration">The duration of the recurring event.</param>
        /// <param name="recursUntil">
        /// Optional end date for the recurring appointment. Defaults to no end date
        /// (DateTime.MaxValue).
        /// </param>
        /// <param name="maxOccurrences">
        /// Optional limit for the number of occurrences. Defaults to no limit
        /// (Int32.MaxInt).
        /// </param>
        public RecurrenceRange(DateTime start, TimeSpan duration, DateTime recursUntil, int maxOccurrences)
        {
            _start = start;
            _eventDuration = duration;
            _recursUntil = recursUntil;
            _maxOccurrences = maxOccurrences;
        }


        /// <summary>
        /// Overloaded. Overridden. Returns a value indicating whether this instance is equal
        /// to a specified object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> is an instance of
        ///     <see cref="RecurrenceRange"/> and equals the value of this instance;
        ///     otherwise, <b>false</b>.
        /// </returns>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            RecurrenceRange other = obj as RecurrenceRange;

            if (obj == null)
            {
                return false;
            }

            return Equals(other);
        }

        /// <summary>Overriden. Returns the hash code for this instance.</summary>
        public override int GetHashCode()
        {
            return _start.GetHashCode() ^ _eventDuration.GetHashCode() ^ _recursUntil.GetHashCode() ^ _maxOccurrences.GetHashCode();
        }

        /// <summary>
        ///     Overloaded. Overridden. Returns a value indicating whether this instance is equal
        ///     to a specified <see cref="RecurrenceRange"/> object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> equals the value of this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        /// <param name="other">An <see cref="RecurrenceRange"/> object to compare with this instance.</param>
        public bool Equals(RecurrenceRange other)
        {
            if (other == null)
            {
                return false;
            }

            return (DatesAreEqualIgnoringMillis(_start, other.Start) &&
                    (_eventDuration == other.EventDuration) &&
                    DatesAreEqualIgnoringMillis(_recursUntil, other.RecursUntil) &&
                    (_maxOccurrences == other.MaxOccurrences));
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrenceRange"/> objects have the
        ///     same value.
        /// </summary>
        public static bool operator ==(RecurrenceRange range1, RecurrenceRange range2)
        {
            return ((object)range1 == null ? (object)range2 == null : range1.Equals(range2));
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrenceRange"/> objects have
        ///     different values.
        /// </summary>
        public static bool operator !=(RecurrenceRange range1, RecurrenceRange range2)
        {
            return ((object)range1 == null ? (object)range2 != null : !range1.Equals(range2));
        }

        private static bool DatesAreEqualIgnoringMillis(DateTime first, DateTime second)
        {
            return ((first.Year == second.Year) &&
                    (first.Month == second.Month) &&
                    (first.Hour == second.Hour) &&
                    (first.Minute == second.Minute) &&
                    (first.Second == second.Second));
        }
    }
}
