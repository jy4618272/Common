using System;
using System.Web.UI.WebControls;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    ///     Specifies the pattern that <see cref="RecurrenceRule"/> uses to evaluate the
    ///     recurrence dates set.
    /// </summary>
    /// <remarks>
    /// 	<para>
    ///         The properties of the <see cref="RecurrencePattern"/> class work together
    ///         to define a complete pattern definition to be used by the
    ///         <see cref="RecurrenceRule"/> engine.
    ///     </para>
    /// 	<para>
    ///         You should not need to work with it directly as specialized
    ///         <see cref="RecurrenceRule"/> classes are provided for the supported modes
    ///         of recurrence. They take care of constructing appropriate
    ///         <see cref="RecurrencePattern"/> objects.
    ///     </para>
    /// </remarks>
    /// <example>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class RecurrencePatternExample
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
    ///             // Creates a recurrence rule for the appointment.
    ///             DailyRecurrenceRule rrule = new DailyRecurrenceRule(1, range);
    ///  
    ///             // Displays the relevant parts of the generated pattern:
    ///             Console.WriteLine("The active recurrence pattern is:");
    ///             Console.WriteLine("  Frequency: {0}", rrule.Pattern.Frequency);
    ///             Console.WriteLine("  Interval: {0}", rrule.Pattern.Interval);
    ///             Console.WriteLine("  Days of week: {0}\n", rrule.Pattern.DaysOfWeekMask);
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
    /// The active recurrence pattern is:
    ///   Frequency: Daily
    ///   Interval: 1
    ///   Days of week: EveryDay
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
    ///     Class RecurrencePatternExample
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
    ///             ' Creates a recurrence rule for the appointment.
    ///             Dim rrule As New DailyRecurrenceRule(1, range)
    ///  
    ///             ' Displays the relevant parts of the generated pattern:
    ///             Console.WriteLine("The active recurrence pattern is:")
    ///             Console.WriteLine("  Frequency: {0}", rrule.Pattern.Frequency)
    ///             Console.WriteLine("  Interval: {0}", rrule.Pattern.Interval)
    ///             Console.WriteLine("  Days of week: {0}" &amp; Chr(10) &amp; "", rrule.Pattern.DaysOfWeekMask)
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
    /// 'The active recurrence pattern is:
    /// '  Frequency: Daily
    /// '  Interval: 1
    /// '  Days of week: EveryDay
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
    public class RecurrencePattern : IEquatable<RecurrencePattern>
    {
        private RecurrenceFrequency _frequency = RecurrenceFrequency.None;
        private int _interval;
        private RecurrenceDay _daysOfWeekMask = RecurrenceDay.None;
        private int _dayOfMonth;
        private int _dayOrdinal;
        private RecurrenceMonth _month = RecurrenceMonth.None;
    	private DayOfWeek _firstDayOfWeek = DayOfWeek.Sunday;

    	/// <value>
        /// 	<para>
        ///         A <see cref="RecurrenceFrequency"/> enumerated constant that indicates the
        ///         frequency of recurrence.
        ///     </para>
        /// </value>
        /// <summary>Gets or sets the frequency of recurrence.</summary>
        /// <remarks>The default value is <see cref="RecurrenceFrequency.None"/>.</remarks>
        /// <seealso cref="RecurrenceFrequency">RecurrenceFrequency Enumeration</seealso>
        public RecurrenceFrequency Frequency
        {
            get
            {
                 return _frequency;
            }

            set
            {
                 _frequency = value;
            }
        }

        /// <summary>Gets or sets the interval of recurrence.</summary>
        /// <value>
        /// 	<para>
        ///         A positive integer representing how often the recurrence rule repeats,
        ///         expressed in <see cref="RecurrenceFrequency"/> units.
        ///     </para>
        /// </value>
        /// <remarks>The default value is 1.</remarks>
        public int Interval
        {
            get
            {
                 return _interval;
            }

            set
            {
                 _interval = value;
            }
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
            get
            {
                return _daysOfWeekMask;
            }

            set
            {
                 _daysOfWeekMask = value;
            }
        }

        /// <summary>Gets or sets the day month on which the event recurs.</summary>
        /// <value>The day month on which the event recurs.</value>
        public int DayOfMonth
        {
            get
            {
                 return _dayOfMonth;
            }

            set
            {
                _dayOfMonth = value;
            }
        }

        /// <remarks>
        /// 	<para>
        ///         This property is meaningful only when <see cref="RecurrenceFrequency"/> is
        ///         set to <see cref="RecurrenceFrequency.Monthly"/> or
        ///         <see cref="RecurrenceFrequency.Yearly"/> and <see cref="DayOfMonth"/>
        ///         is not set.
        ///     </para>
        /// 	<para>In such scenario it selects the n-th occurrence within the set of events
        ///     specified by the rule. Valid values are from -31 to +31, 0 is ignored.</para>
        /// 	<para>For example with RecurrenceFrequency set to Monthly and DaysOfWeekMask set to
        ///     Monday DayOfMonth is interpreted in the following way:</para>
        /// 	<list type="bullet">
        /// 		<item>
        /// 			<ul class="noindent">
        /// 				<li>1: Selects the first monday of the month.</li>
        /// 				<li>3: Selects the third monday of the month.</li>
        /// 				<li>-1: Selects the last monday of the month.</li>
        /// 			</ul>
        /// 		</item>
        /// 	</list>
        /// 	<para>
        ///         For detailed examples see the documentation of the
        ///         <see cref="MonthlyRecurrenceRule"/> class.
        ///     </para>
        /// </remarks>
        /// <seealso cref="MonthlyRecurrenceRule">MonthlyRecurrenceRule Class</seealso>
        public int DayOrdinal
        {
            get
            {
                return _dayOrdinal;
            }

            set
            {
                 _dayOrdinal = value;
            }
        }

        /// <summary>Gets or sets the month on which the event recurs.</summary>
        /// <value>
        ///     This property is only meaningful when <see cref="RecurrenceFrequency"/> is set
        ///     to <see cref="RecurrenceFrequency.Yearly"/>.
        /// </value>
        public RecurrenceMonth Month
        {
            get
            {
                 return _month;
            }

            set
            {
                _month = value;
            }
        }

		/// <summary>Gets or sets the day on which the week starts.</summary>
		/// <value>
		///     This property is only meaningful when <see cref="RecurrenceFrequency"/> is set
		///     to <see cref="RecurrenceFrequency.Weekly"/> and <see cref="Interval"/> is greater than 1.
		/// </value>
		public DayOfWeek FirstDayOfWeek
    	{
    		get
    		{
    			return _firstDayOfWeek;
    		}

			set
			{
				_firstDayOfWeek = value;
			}
    	}


        /// <summary>
        /// Overloaded. Overridden. Returns a value indicating whether this instance is equal
        /// to a specified object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> is an instance of
        ///     <see cref="RecurrencePattern"/> and equals the value of this instance;
        ///     otherwise, <b>false</b>.
        /// </returns>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            RecurrencePattern other = obj as RecurrencePattern;

            if (obj == null)
            {
                return false;
            }

            return Equals(other);
        }

        /// <summary>Overriden. Returns the hash code for this instance.</summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Frequency.GetHashCode() ^ Interval.GetHashCode() ^ DaysOfWeekMask.GetHashCode() ^
                DayOfMonth.GetHashCode() ^ DayOrdinal.GetHashCode() ^ Month.GetHashCode();
        }

        /// <summary>
        ///     Overloaded. Overridden. Returns a value indicating whether this instance is equal
        ///     to a specified <see cref="RecurrencePattern"/> object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> equals the value of this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        /// <param name="other">An <see cref="RecurrencePattern"/> object to compare with this instance.</param>
        public bool Equals(RecurrencePattern other)
        {
            if (other == null)
            {
                return false;
            }

            return  (Frequency == other.Frequency) &&
                    (Interval == other.Interval) &&
                    (DaysOfWeekMask == other.DaysOfWeekMask) &&
                    (DayOfMonth == other.DayOfMonth) &&
                    (DayOrdinal == other.DayOrdinal) &&
                    (Month == other.Month);
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrencePattern"/> objects have the
        ///     same value.
        /// </summary>
        public static bool operator ==(RecurrencePattern pat1, RecurrencePattern pat2)
        {
            return (object) pat1 == null ? (object) pat2 == null : pat1.Equals(pat2);
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrencePattern"/> objects have
        ///     different values.
        /// </summary>
        public static bool operator !=(RecurrencePattern pat1, RecurrencePattern pat2)
        {
            return (object)pat1 == null ? (object)pat2 != null : !pat1.Equals(pat2);
        }
    }
}
