using Micajah.Common.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>Provides the <strong>abstract</strong> base class for recurrence rules.</summary>
    /// <seealso cref="HourlyRecurrenceRule">HourlyRecurrenceRule Class</seealso>
    /// <seealso cref="DailyRecurrenceRule">DailyRecurrenceRule Class</seealso>
    /// <seealso cref="WeeklyRecurrenceRule">WeeklyRecurrenceRule Class</seealso>
    /// <seealso cref="MonthlyRecurrenceRule">MonthlyRecurrenceRule Class</seealso>
    /// <seealso cref="YearlyRecurrenceRule">YearlyRecurrenceRule Class</seealso>
    /// <remarks>
    /// 	<strong>Notes to implementers:</strong> This base class is provided to make it
    /// easier for implementers to create a recurrence rule. Implementers are encouraged to
    /// extend this base class instead of creating their own.
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(RecurrenceRuleConverter))]
    public abstract class RecurrenceRule : ISerializable, IEquatable<RecurrenceRule>
    {
        #region Private fields

        private DateTime _effectiveStart = DateTime.MinValue;
        private DateTime _effectiveEnd = DateTime.MaxValue;
        private IList<DateTime> _exceptions = new List<DateTime>();
        private int _maximumCandidates = 1000;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected RecurrencePattern rulePattern = new RecurrencePattern();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected RecurrenceRange ruleRange = new RecurrenceRange();

        #endregion

        #region Constructors

        protected RecurrenceRule(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;

            RecurrenceRule parsedRule;
            if (!TryParse(info.GetString("RRULE"), out parsedRule))
            {
                throw new InvalidOperationException(Resources.RecurrenceRule_DeserializationFailed);
            }

            rulePattern = parsedRule.Pattern;
            ruleRange = parsedRule.Range;
        }

        protected RecurrenceRule()
        {

        }

        #endregion

        #region Public properties

        /// <summary>Gets the <see cref="RecurrenceRange"/> associated with this recurrence rule.</summary>
        /// <value>The <see cref="RecurrenceRange"/> associated with this recurrence rule.</value>
        /// <remarks>
        ///     By calling <see cref="SetEffectiveRange"/> the range of the generated
        ///     occurrences can be narrowed.
        /// </remarks>
        public RecurrenceRange Range
        {
            get { return ruleRange; }
        }

        /// <summary>Gets the <see cref="RecurrencePattern"/> associated with this recurrence rule.</summary>
        /// <value>The <see cref="RecurrencePattern"/> associated with this recurrence rule.</value>
        public RecurrencePattern Pattern
        {
            get { return rulePattern; }
        }

        /// <remarks>Occurrence times are in UTC.</remarks>
        /// <summary>Gets the evaluated occurrence times of this recurrence rule.</summary>
        /// <value>The evaluated occurrence times of this recurrence rule.</value>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Telerik.Web.UI;
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
        /// Imports Telerik.Web.UI
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
        public virtual IEnumerable<DateTime> Occurrences
        {
            get
            {
                int candidateIndex = 0;
                int occurrencesCount = 0;
                while (occurrencesCount < ruleRange.MaxOccurrences)
                {
                    DateTime nextStart = GetOccurrenceStart(candidateIndex++);

                    if ((ruleRange.RecursUntil < nextStart) || (_effectiveEnd < nextStart))
                    {
                        yield break;
                    }

                    if (MaximumCandidates < candidateIndex)
                    {
                        yield break;
                    }

                    if (!MatchAdvancedPattern(nextStart))
                    {
                        continue;
                    }

                    occurrencesCount++;

                    if (nextStart < _effectiveStart)
                    {
                        continue;
                    }

                    if (_exceptions.Contains(nextStart))
                    {
                        continue;
                    }

                    yield return nextStart;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this recurrence rule yields any
        /// occurrences.
        /// </summary>
        /// <value>True this recurrence rule yields any occurrences, false otherwise.</value>
        public bool HasOccurrences
        {
            get
            {
                return Occurrences.GetEnumerator().MoveNext();
            }
        }

        /// <summary>Gets or sets a list of the exception dates associated with this recurrence rule.</summary>
        /// <value>A list of the exception dates associated with this recurrence rule.</value>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Telerik.Web.UI;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class RecurrenceExceptionsExample
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
        ///             // Creates a recurrence exception for 5:30 PM (local time).
        ///             // Note that exception dates must be in universal time.
        ///             rrule.Exceptions.Add(Convert.ToDateTime("6/1/2007 5:30 PM").ToUniversalTime());
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
        ///  2: 6/1/2007 7:30:00 PM
        ///  3: 6/1/2007 9:30:00 PM
        ///  4: 6/1/2007 11:30:00 PM
        ///  5: 6/2/2007 1:30:00 AM
        ///  6: 6/2/2007 3:30:00 AM
        ///  7: 6/2/2007 5:30:00 AM
        ///  8: 6/2/2007 7:30:00 AM
        ///  9: 6/2/2007 9:30:00 AM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Telerik.Web.UI
        ///  
        /// Namespace RecurrenceExamples
        ///     Class RecurrenceExceptionsExample
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
        ///             ' Creates a recurrence exception for 5:30 PM (local time).
        ///             ' Note that exception dates must be in universal time.
        ///             rrule.Exceptions.Add(Convert.ToDateTime("6/1/2007 5:30 PM").ToUniversalTime())
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
        /// ' 2: 6/1/2007 7:30:00 PM
        /// ' 3: 6/1/2007 9:30:00 PM
        /// ' 4: 6/1/2007 11:30:00 PM
        /// ' 5: 6/2/2007 1:30:00 AM
        /// ' 6: 6/2/2007 3:30:00 AM
        /// ' 7: 6/2/2007 5:30:00 AM
        /// ' 8: 6/2/2007 7:30:00 AM
        /// ' 9: 6/2/2007 9:30:00 AM
        /// '
        ///     </code>
        /// </example>
        /// <remarks>
        /// Any date placed in the list will be considered a recurrence exception, i.e. an
        /// occurrence will not be generated for that date. The dates must be in <strong>universal
        /// time</strong>.
        /// </remarks>
        public virtual IList<DateTime> Exceptions
        {
            get { return _exceptions; }
            private set { _exceptions = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this recurrence rule has associated
        /// exceptions.
        /// </summary>
        /// <value>True if this recurrence rule has associated exceptions, false otherwise.</value>
        public bool HasExceptions
        {
            get
            {
                return (0 < _exceptions.Count);
            }
        }

        /// <summary>
        /// Gets or sets the maximum candidates limit.
        /// </summary>
        /// <remarks>
        ///	This limit is used to prevent lockups when evaluating infinite rules without using SetEffectiveRange.
        /// The default value should not be changed under normal conditions.
        /// </remarks>
        /// <value>The maximum candidates limit.</value>
        public int MaximumCandidates
        {
            get
            {
                return _maximumCandidates;
            }

            set
            {
                _maximumCandidates = value;
            }
        }

        #endregion

        #region Protected properties

        protected DateTime EffectiveStart
        {
            get
            {
                if (_effectiveStart < ruleRange.Start)
                {
                    return ruleRange.Start;
                }
                else
                {
                    return _effectiveStart;
                }
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates a recurrence rule with the specified pattern and range.
        /// </summary>
        /// <param name="pattern">The recurrence pattern.</param>
        /// <param name="range">The recurrence range.</param>
        /// <returns>The constructed recurrence rule.</returns>
        public static RecurrenceRule FromPatternAndRange(RecurrencePattern pattern, RecurrenceRange range)
        {
            if (pattern == null || range == null)
            {
                return null;
            }

            switch (pattern.Frequency)
            {
                case RecurrenceFrequency.Hourly:
                    return new HourlyRecurrenceRule(pattern.Interval, range);

                case RecurrenceFrequency.Daily:
                    if (pattern.DaysOfWeekMask != RecurrenceDay.EveryDay)
                    {
                        return new DailyRecurrenceRule(pattern.DaysOfWeekMask, range);
                    }
                    else
                        if (0 < pattern.Interval)
                        {
                            return new DailyRecurrenceRule(pattern.Interval, range);
                        }
                    break;

                case RecurrenceFrequency.Weekly:
                    if (0 < pattern.Interval && pattern.DaysOfWeekMask != RecurrenceDay.None)
                    {
                        return new WeeklyRecurrenceRule(pattern.Interval, pattern.DaysOfWeekMask, range, pattern.FirstDayOfWeek);
                    }
                    break;

                case RecurrenceFrequency.Monthly:
                    if (0 < pattern.DayOfMonth && 0 < pattern.Interval)
                    {
                        return new MonthlyRecurrenceRule(pattern.DayOfMonth, pattern.Interval, range);
                    }

                    if (pattern.DayOrdinal != 0 && pattern.DaysOfWeekMask != RecurrenceDay.None && 0 < pattern.Interval)
                    {
                        return new MonthlyRecurrenceRule(pattern.DayOrdinal, pattern.DaysOfWeekMask, pattern.Interval, range);
                    }
                    break;

                case RecurrenceFrequency.Yearly:
                    if (pattern.Month != RecurrenceMonth.None && 0 < pattern.DayOfMonth)
                    {
                        return new YearlyRecurrenceRule(pattern.Month, pattern.DayOfMonth, range);
                    }

                    if (pattern.DayOrdinal != 0 && pattern.Month != RecurrenceMonth.None && pattern.DaysOfWeekMask != RecurrenceDay.None)
                    {
                        return new YearlyRecurrenceRule(pattern.DayOrdinal, pattern.Month, pattern.DaysOfWeekMask, range);
                    }

                    if (pattern.Month != RecurrenceMonth.None)
                    {
                        return new YearlyRecurrenceRule(pattern.Month, range.Start.Day, range);
                    }
                    break;
            }

            return null;
        }

        /// <summary>Creates a recurrence rule instance from it's string representation.</summary>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Telerik.Web.UI;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class ParsingExample
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
        ///             // Prints the string representation of the recurrence rule:
        ///             string rruleAsString = rrule.ToString();
        ///             Console.WriteLine("Recurrence rule:\n\n{0}\n", rruleAsString);
        ///  
        ///             // The string representation can be stored in a database, etc.
        ///             // ...
        ///  
        ///             // Then it can be reconstructed using TryParse method:
        ///             RecurrenceRule parsedRule;
        ///             RecurrenceRule.TryParse(rruleAsString, out parsedRule);
        ///             Console.WriteLine("After parsing (should be the same):\n\n{0}", parsedRule);
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Recurrence rule:
        ///  
        /// DTSTART:20070601T123000Z
        /// DTEND:20070601T130000Z
        /// RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        ///  
        ///  
        /// After parsing (should be the same):
        ///  
        /// DTSTART:20070601T123000Z
        /// DTEND:20070601T130000Z
        /// RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Telerik.Web.UI
        ///  
        /// Namespace RecurrenceExamples
        ///     Class ParsingExample
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
        ///             ' Prints the string representation of the recurrence rule:
        ///             Dim rruleAsString As String = rrule.ToString()
        ///             Console.WriteLine("Recurrence rule:" &amp; Chr(10) &amp; "" &amp; Chr(10) &amp; "{0}" &amp; Chr(10) &amp; "", rruleAsString)
        ///  
        ///             ' The string representation can be stored in a database, etc.
        ///             ' ...
        ///  
        ///             ' Then it can be reconstructed using TryParse method:
        ///             Dim parsedRule As RecurrenceRule
        ///             RecurrenceRule.TryParse(rruleAsString, parsedRule)
        ///             Console.WriteLine("After parsing (should be the same):" &amp; Chr(10) &amp; "" &amp; Chr(10) &amp; "{0}", parsedRule)
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Recurrence rule:
        /// '
        /// 'DTSTART:20070601T123000Z
        /// 'DTEND:20070601T130000Z
        /// 'RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// '
        /// '
        /// 'After parsing (should be the same):
        /// '
        /// 'DTSTART:20070601T123000Z
        /// 'DTEND:20070601T130000Z
        /// 'RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// '
        ///     </code>
        /// </example>
        /// <returns>True if <em>input</em> was converted successfully, false otherwise.</returns>
        /// <param name="input">The string representation to parse.</param>
        /// <param name="rrule">
        /// When this method returns, contains the recurrence rule instance, if the
        /// conversion succeeded, or null if the conversion failed. The conversion fails if the
        /// <em>value</em> parameter is a null reference (<strong>Nothing</strong> in Visual Basic)
        /// or represents invalid recurrence rule.
        /// </param>
        public static bool TryParse(string input, out RecurrenceRule rule)
        {
            if (string.IsNullOrEmpty(input))
            {
                rule = null;
                return false;
            }

            RecurrenceRange parsedRange = new RecurrenceRange();
            RecurrencePattern parsedPattern = new RecurrencePattern();
            List<DateTime> parsedExceptions = new List<DateTime>();

            DateTime? dtStart = null;
            DateTime? dtEnd = null;

            input = input.Trim();
            foreach (string line in input.Split('\n'))
            {
                string trimmedLine = line.Trim();
                Match dateHeaderMatch = Regex.Match(trimmedLine, @"^(DTSTART|DTEND):(.*)$", RegexOptions.IgnoreCase);
                if (dateHeaderMatch.Success)
                {
                    DateTime date;
                    if (TryParseDateTime(dateHeaderMatch.Groups[2].Value, out date))
                    {
                        if (dateHeaderMatch.Groups[1].Value == "DTSTART")
                        {
                            dtStart = date;
                        }
                        else
                        {
                            dtEnd = date;
                        }
                    }
                }

                ParseRRule(trimmedLine, parsedRange, parsedPattern);
                ParseExceptions(trimmedLine, parsedExceptions);
            }

            if (dtStart.HasValue && dtEnd.HasValue)
            {
                parsedRange.Start = dtStart.Value;
                parsedRange.EventDuration = dtEnd.Value.Subtract(dtStart.Value);
                rule = FromPatternAndRange(parsedPattern, parsedRange);
                rule.Exceptions = parsedExceptions;

                return (rule != null);
            }

            rule = null;
            return false;
        }

        private static void ParseRRule(string line, RecurrenceRange parsedRange, RecurrencePattern parsedPattern)
        {
            Match rruleHeaderMatch = Regex.Match(line, @"^(RRULE:)(.*)$", RegexOptions.IgnoreCase);
            if (rruleHeaderMatch.Success)
            {
                string rules = rruleHeaderMatch.Groups[2].Value;

                Match frequencyMatch = Regex.Match(rules, @"FREQ=(HOURLY|DAILY|WEEKLY|MONTHLY|YEARLY)", RegexOptions.IgnoreCase);
                if (frequencyMatch.Success)
                {
                    parsedPattern.Frequency = (RecurrenceFrequency)Enum.Parse(typeof(RecurrenceFrequency), frequencyMatch.Groups[1].Value, true);
                }

                Match maxOccurrencesMatch = Regex.Match(rules, @"COUNT=(\d{1,4})", RegexOptions.IgnoreCase);
                if (maxOccurrencesMatch.Success)
                {
                    parsedRange.MaxOccurrences = int.Parse(maxOccurrencesMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                }

                Match recurUntilMatch = Regex.Match(rules, @"UNTIL=([\w\d]*)", RegexOptions.IgnoreCase);
                if (recurUntilMatch.Success)
                {
                    DateTime date;
                    if (TryParseDateTime(recurUntilMatch.Groups[1].Value, out date))
                    {
                        parsedRange.RecursUntil = date;
                    }
                }

                Match intervalMatch = Regex.Match(rules, @"INTERVAL=(\d{1,2})", RegexOptions.IgnoreCase);
                if (intervalMatch.Success)
                {
                    parsedPattern.Interval = int.Parse(intervalMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                }

                Match dayOrdinalMatch = Regex.Match(rules, @"BYSETPOS=(-?\d{1})", RegexOptions.IgnoreCase);
                if (dayOrdinalMatch.Success)
                {
                    parsedPattern.DayOrdinal = int.Parse(dayOrdinalMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                }

                Match dayOfMonthMatch = Regex.Match(rules, @"BYMONTHDAY=(\d{1,2})", RegexOptions.IgnoreCase);
                if (dayOfMonthMatch.Success)
                {
                    parsedPattern.DayOfMonth = int.Parse(dayOfMonthMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                }

                Match dayOfWeekMaskMatch = Regex.Match(rules, @"BYDAY=(-?\d{1})?([\w,]*)", RegexOptions.IgnoreCase);
                if (dayOfWeekMaskMatch.Success)
                {
                    // The BYDAY rule might also contain a day ordinal value.
                    if (!string.IsNullOrEmpty(dayOfWeekMaskMatch.Groups[1].Value))
                    {
                        parsedPattern.DayOrdinal = int.Parse(dayOfWeekMaskMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                    }

                    RecurrenceDay mask;
                    if (TryParseDayOfWeekMask(dayOfWeekMaskMatch.Groups[2].Value, out mask))
                    {
                        parsedPattern.DaysOfWeekMask = mask;
                    }
                }

                Match monthMatch = Regex.Match(rules, @"BYMONTH=(\d{1,2})", RegexOptions.IgnoreCase);
                if (monthMatch.Success)
                {
                    parsedPattern.Month = (RecurrenceMonth)Enum.Parse(typeof(RecurrenceMonth), monthMatch.Groups[1].Value, true);
                }

                Match weekStartMatch = Regex.Match(rules, @"WKST=([\w,]*)", RegexOptions.IgnoreCase);
                if (weekStartMatch.Success)
                {
                    parsedPattern.FirstDayOfWeek = ParseDayOfWeek(weekStartMatch.Groups[1].Value);
                }
            }
        }

        private static void ParseExceptions(string line, List<DateTime> parsedExceptions)
        {
            Match exDateHeaderMatch = Regex.Match(line, @"^(EXDATE):(.*)$", RegexOptions.IgnoreCase);
            if (exDateHeaderMatch.Success)
            {
                foreach (string exDate in exDateHeaderMatch.Groups[2].Value.Split(','))
                {
                    DateTime parsedExDate;
                    if (TryParseDateTime(exDate, out parsedExDate))
                    {
                        parsedExceptions.Add(parsedExDate);
                    }
                }
            }
        }

        protected static bool IsValidValue<T>(T value, T minValue, T maxValue)
            where T : IComparable
        {
            return ((value.CompareTo(minValue) >= 0) || (value.CompareTo(maxValue) <= 0));
        }

        protected static void ValidateValue<T>(string name, T value, T minValue, T maxValue)
            where T : IComparable
        {
            if ((value.CompareTo(minValue) <= 0) || (value.CompareTo(maxValue) >= 0))
            {
                throw new ArgumentOutOfRangeException(
                    String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} is out of range. Actual value is {1}, allowed range is [{2} - {3}]",
                    name, value, minValue, maxValue));
            }
        }

        private static bool TryParseDateTime(string input, out DateTime date)
        {
            Match dateMatch = Regex.Match(input, @"^(\d{4})(\d{2})(\d{2})T(\d{2})(\d{2})(\d{2})(Z)(.*)$", RegexOptions.IgnoreCase);
            if (dateMatch.Success)
            {
                int year = int.Parse(dateMatch.Groups[1].Value, System.Globalization.CultureInfo.CurrentCulture);
                int month = int.Parse(dateMatch.Groups[2].Value, System.Globalization.CultureInfo.CurrentCulture);
                int day = int.Parse(dateMatch.Groups[3].Value, System.Globalization.CultureInfo.CurrentCulture);
                int hour = int.Parse(dateMatch.Groups[4].Value, System.Globalization.CultureInfo.CurrentCulture);
                int minute = int.Parse(dateMatch.Groups[5].Value, System.Globalization.CultureInfo.CurrentCulture);
                int second = int.Parse(dateMatch.Groups[6].Value, System.Globalization.CultureInfo.CurrentCulture);

                bool valid = true;
                valid &= IsValidValue(year, 1900, 2900);
                valid &= IsValidValue(month, 1, 12);
                valid &= IsValidValue(day, 1, 31);
                valid &= IsValidValue(hour, 0, 23);
                valid &= IsValidValue(minute, 0, 59);
                valid &= IsValidValue(second, 0, 59);

                if (valid)
                {
                    date = new DateTime(year, month, day, hour, minute, second, 0, DateTimeKind.Utc);
                    return true;
                }
            }

            date = DateTime.MinValue;
            return false;
        }

        private static bool TryParseDayOfWeekMask(string input, out RecurrenceDay mask)
        {
            Dictionary<string, RecurrenceDay> abbrevToRecurrenceDay = new Dictionary<string, RecurrenceDay>();
            abbrevToRecurrenceDay.Add("MO", RecurrenceDay.Monday);
            abbrevToRecurrenceDay.Add("TU", RecurrenceDay.Tuesday);
            abbrevToRecurrenceDay.Add("WE", RecurrenceDay.Wednesday);
            abbrevToRecurrenceDay.Add("TH", RecurrenceDay.Thursday);
            abbrevToRecurrenceDay.Add("FR", RecurrenceDay.Friday);
            abbrevToRecurrenceDay.Add("SA", RecurrenceDay.Saturday);
            abbrevToRecurrenceDay.Add("SU", RecurrenceDay.Sunday);

            mask = RecurrenceDay.None;
            foreach (string dayAbbrev in input.Split(','))
            {
                if (abbrevToRecurrenceDay.ContainsKey(dayAbbrev))
                {
                    mask |= abbrevToRecurrenceDay[dayAbbrev];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static DayOfWeek ParseDayOfWeek(string input)
        {
            Dictionary<string, DayOfWeek> abbrevToDayOfWeek = new Dictionary<string, DayOfWeek>();
            abbrevToDayOfWeek.Add("MO", DayOfWeek.Monday);
            abbrevToDayOfWeek.Add("TU", DayOfWeek.Tuesday);
            abbrevToDayOfWeek.Add("WE", DayOfWeek.Wednesday);
            abbrevToDayOfWeek.Add("TH", DayOfWeek.Thursday);
            abbrevToDayOfWeek.Add("FR", DayOfWeek.Friday);
            abbrevToDayOfWeek.Add("SA", DayOfWeek.Saturday);
            abbrevToDayOfWeek.Add("SU", DayOfWeek.Sunday);

            if (abbrevToDayOfWeek.ContainsKey(input))
            {
                return abbrevToDayOfWeek[input];
            }

            return DayOfWeek.Sunday;
        }

        #endregion

        #region Public methods

        /// <summary>Specifies the effective range for evaluating occurrences.</summary>
        /// <exception cref="System.ArgumentException" caption="">End date is before Start date.</exception>
        /// <remarks>
        ///     The range is inclusive. To clear the effective range call
        ///     <see cref="ClearEffectiveRange"/>.
        /// </remarks>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Telerik.Web.UI;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class EffectiveRangeExample
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
        ///             // Limits the effective range.
        ///             rrule.SetEffectiveRange(Convert.ToDateTime("6/1/2007 5:00 PM"), Convert.ToDateTime("6/1/2007 8:00 PM"));
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
        ///  1: 6/1/2007 5:30:00 PM
        ///  2: 6/1/2007 7:30:00 PM
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Telerik.Web.UI
        ///  
        /// Namespace RecurrenceExamples
        ///     Class EffectiveRangeExample
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
        ///             ' Limits the effective range.
        ///             rrule.SetEffectiveRange(Convert.ToDateTime("6/1/2007 5:00 PM"), Convert.ToDateTime("6/1/2007 8:00 PM"))
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
        /// ' 1: 6/1/2007 5:30:00 PM
        /// ' 2: 6/1/2007 7:30:00 PM
        /// '
        ///     </code>
        /// </example>
        /// <seealso cref="ClearEffectiveRange">ClearEffectiveRange Method</seealso>
        /// <param name="start">The starting date of the effective range.</param>
        /// <param name="end">The ending date of the effective range.</param>
        public void SetEffectiveRange(DateTime start, DateTime end)
        {
            if (_effectiveEnd < _effectiveStart)
            {
                throw new ArgumentException("The end date is before the start date.");
            }

            _effectiveStart = DateHelper.AssumeUtc(start);
            _effectiveEnd = DateHelper.AssumeUtc(end);
        }

        /// <summary>Clears the effective range set by calling <see cref="SetEffectiveRange"/>.</summary>
        /// <remarks>If no effective range was set, calling this method has no effect.</remarks>
        /// <seealso cref="SetEffectiveRange">SetEffectiveRange Method</seealso>
        public void ClearEffectiveRange()
        {
            _effectiveStart = DateTime.MinValue;
            _effectiveEnd = DateTime.MaxValue;
        }

        /// <summary>Converts the recurrence rule to it's equivalent string representation.</summary>
        /// <returns>The string representation of this recurrence rule.</returns>
        /// <example>
        /// 	<code lang="CS">
        /// using System;
        /// using Telerik.Web.UI;
        ///  
        /// namespace RecurrenceExamples
        /// {
        ///     class ParsingExample
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
        ///             // Prints the string representation of the recurrence rule:
        ///             string rruleAsString = rrule.ToString();
        ///             Console.WriteLine("Recurrence rule:\n\n{0}\n", rruleAsString);
        ///  
        ///             // The string representation can be stored in a database, etc.
        ///             // ...
        ///  
        ///             // Then it can be reconstructed using TryParse method:
        ///             RecurrenceRule parsedRule;
        ///             RecurrenceRule.TryParse(rruleAsString, out parsedRule);
        ///             Console.WriteLine("After parsing (should be the same):\n\n{0}", parsedRule);
        ///         }
        ///     }
        /// }
        ///  
        /// /*
        /// This example produces the following results:
        ///  
        /// Recurrence rule:
        ///  
        /// DTSTART:20070601T123000Z
        /// DTEND:20070601T130000Z
        /// RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        ///  
        ///  
        /// After parsing (should be the same):
        ///  
        /// DTSTART:20070601T123000Z
        /// DTEND:20070601T130000Z
        /// RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// */
        ///     </code>
        /// 	<code lang="VB">
        /// Imports System
        /// Imports Telerik.Web.UI
        ///  
        /// Namespace RecurrenceExamples
        ///     Class ParsingExample
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
        ///             ' Prints the string representation of the recurrence rule:
        ///             Dim rruleAsString As String = rrule.ToString()
        ///             Console.WriteLine("Recurrence rule:" &amp; Chr(10) &amp; "" &amp; Chr(10) &amp; "{0}" &amp; Chr(10) &amp; "", rruleAsString)
        ///  
        ///             ' The string representation can be stored in a database, etc.
        ///             ' ...
        ///  
        ///             ' Then it can be reconstructed using TryParse method:
        ///             Dim parsedRule As RecurrenceRule
        ///             RecurrenceRule.TryParse(rruleAsString, parsedRule)
        ///             Console.WriteLine("After parsing (should be the same):" &amp; Chr(10) &amp; "" &amp; Chr(10) &amp; "{0}", parsedRule)
        ///         End Sub
        ///     End Class
        /// End Namespace
        ///  
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'Recurrence rule:
        /// '
        /// 'DTSTART:20070601T123000Z
        /// 'DTEND:20070601T130000Z
        /// 'RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// '
        /// '
        /// 'After parsing (should be the same):
        /// '
        /// 'DTSTART:20070601T123000Z
        /// 'DTEND:20070601T130000Z
        /// 'RRULE:FREQ=HOURLY;COUNT=10;INTERVAL=2;
        /// '
        ///     </code>
        /// </example>
        /// <remarks>
        /// The string representation is based on the iCalendar data format (RFC
        /// 2445).
        /// </remarks>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            DateTime dtEnd = ruleRange.Start.Add(ruleRange.EventDuration);

            output.AppendFormat("DTSTART:{0}\r\n", FormatDateTime(ruleRange.Start, true));
            output.AppendFormat("DTEND:{0}\r\n", FormatDateTime(dtEnd, true));
            output.AppendFormat("RRULE:{0}\r\n", FormatRRule());
            output.Append(FormatExceptions());

            return output.ToString();
        }

        /// <summary>Overriden. Returns the hash code for this instance.</summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (Pattern.GetHashCode() ^ Range.GetHashCode());
        }

        /// <summary>
        /// Overloaded. Overridden. Returns a value indicating whether this instance is equal
        /// to a specified object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> is an instance of
        ///     <see cref="RecurrenceRule"/> and equals the value of this instance;
        ///     otherwise, <b>false</b>.
        /// </returns>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            RecurrenceRule rule = obj as RecurrenceRule;

            if (obj == null)
            {
                return false;
            }

            return Equals(rule);
        }

        /// <summary>
        ///     Overloaded. Overridden. Returns a value indicating whether this instance is equal
        ///     to a specified <see cref="RecurrenceRule"/> object.
        /// </summary>
        /// <returns>
        /// 	<strong>true</strong> if <i>value</i> equals the value of this instance;
        /// otherwise, <b>false</b>.
        /// </returns>
        /// <param name="other">An <see cref="RecurrenceRule"/> object to compare with this instance.</param>
        public bool Equals(RecurrenceRule other)
        {
            if (other == null)
            {
                return false;
            }

            return (Pattern == other.Pattern && Range == other.Range);
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrenceRule"/> objects have the
        ///     same value.
        /// </summary>
        public static bool operator ==(RecurrenceRule rule1, RecurrenceRule rule2)
        {
            return ((object)rule1 == null ? (object)rule2 == null : rule1.Equals(rule2));
        }

        /// <summary>
        ///     Determines whether two specified <see cref="RecurrenceRule"/> objects have
        ///     different values.
        /// </summary>
        public static bool operator !=(RecurrenceRule rule1, RecurrenceRule rule2)
        {
            return ((object)rule1 == null ? (object)rule2 != null : !rule1.Equals(rule2));
        }

        #endregion

        #region Protected methods

        protected virtual DateTime GetOccurrenceStart(int index)
        {
            throw new NotImplementedException();
        }

        protected virtual bool MatchAdvancedPattern(DateTime start)
        {
            throw new NotImplementedException();
        }

        protected bool MatchDayOfWeekMask(DateTime start)
        {
            RecurrenceDay day = (RecurrenceDay)Enum.Parse(typeof(RecurrenceDay), start.DayOfWeek.ToString());
            return ((day & rulePattern.DaysOfWeekMask) == day);
        }

        protected bool MatchDayOrdinal(DateTime date)
        {
            if (0 == rulePattern.DayOrdinal)
            {
                return true;
            }

            // TODO: Caching.

            return ((0 < rulePattern.DayOrdinal) ? MatchDayOrdinalPositive(date) : MatchDayOrdinalNegative(date));
        }

        #endregion

        #region Private methods

        private static string FormatDateTime(DateTime date, bool containsTime)
        {
            StringBuilder output = new StringBuilder();
            DateTime utcDate = date.ToUniversalTime();

            output.AppendFormat("{0:00}{1:00}{2:00}", utcDate.Year, utcDate.Month, utcDate.Day);

            if (containsTime)
            {
                output.AppendFormat("T{0:00}{1:00}{2:00}", utcDate.Hour, utcDate.Minute, utcDate.Second);
            }

            output.Append("Z");

            return output.ToString();
        }

        private string FormatRRule()
        {
            StringBuilder output = new StringBuilder();

            output.AppendFormat(System.Globalization.CultureInfo.CurrentCulture, "FREQ={0};", rulePattern.Frequency.ToString().ToUpper(System.Globalization.CultureInfo.CurrentCulture));

            if ((0 < ruleRange.MaxOccurrences) && (ruleRange.MaxOccurrences < int.MaxValue))
            {
                output.AppendFormat("COUNT={0};", ruleRange.MaxOccurrences);
            }
            else
                if ((DateTime.MinValue < ruleRange.RecursUntil) && (ruleRange.RecursUntil < DateTime.MaxValue))
                {
                    output.AppendFormat("UNTIL={0};", FormatDateTime(ruleRange.RecursUntil, true));
                }

            if (0 < rulePattern.Interval)
            {
                output.AppendFormat("INTERVAL={0};", rulePattern.Interval);
            }

            if (rulePattern.DayOrdinal != 0)
            {
                output.AppendFormat("BYSETPOS={0};", rulePattern.DayOrdinal);
            }

            if (0 < rulePattern.DayOfMonth)
            {
                output.AppendFormat("BYMONTHDAY={0};", rulePattern.DayOfMonth);
            }

            if (rulePattern.DaysOfWeekMask != RecurrenceDay.None)
            {
                Dictionary<RecurrenceDay, string> recurrenceDayToAbbrev = new Dictionary<RecurrenceDay, string>();
                recurrenceDayToAbbrev.Add(RecurrenceDay.Monday, "MO");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Tuesday, "TU");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Wednesday, "WE");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Thursday, "TH");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Friday, "FR");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Saturday, "SA");
                recurrenceDayToAbbrev.Add(RecurrenceDay.Sunday, "SU");

                List<string> days = new List<string>();
                foreach (RecurrenceDay day in recurrenceDayToAbbrev.Keys)
                {
                    if ((rulePattern.DaysOfWeekMask & day) == day)
                    {
                        days.Add(recurrenceDayToAbbrev[day]);
                    }
                }

                output.AppendFormat("BYDAY={0};", string.Join(",", days.ToArray()));
            }

            if (rulePattern.Month != RecurrenceMonth.None)
            {
                output.AppendFormat("BYMONTH={0};", (int)rulePattern.Month);
            }

            if (rulePattern.FirstDayOfWeek != DayOfWeek.Sunday)
            {
                string shortDayName = rulePattern.FirstDayOfWeek.ToString().ToUpperInvariant().Substring(0, 2);
                output.AppendFormat("WKST={0};", shortDayName);
            }

            string result = output.ToString();

            // Remove the trailing semicolon - We should avoid inserting it in the first place.
            if (result[result.Length - 1] == ';')
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        private string FormatExceptions()
        {
            if (_exceptions.Count == 0)
            {
                return string.Empty;
            }

            string[] exceptions = new string[_exceptions.Count];
            for (int i = 0; i < _exceptions.Count; i++)
            {
                exceptions[i] = FormatDateTime(_exceptions[i], true);
            }

            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "EXDATE:{0}\r\n", string.Join(",", exceptions));
        }

        private bool MatchDayOrdinalPositive(DateTime date)
        {
            DateTime currentDate = DateHelper.GetStartOfMonth(date.Year, date.Month);
            int dayOrdinal = 0;
            while (currentDate <= date)
            {
                if (MatchDayOfWeekMask(currentDate))
                {
                    dayOrdinal++;
                }

                currentDate = currentDate.AddDays(1);
            }

            return (dayOrdinal == rulePattern.DayOrdinal);
        }

        private bool MatchDayOrdinalNegative(DateTime date)
        {
            DateTime currentDate = DateHelper.GetEndOfMonth(date.Year, date.Month);
            int dayOrdinal = 0;
            while (date < currentDate)
            {
                if (MatchDayOfWeekMask(currentDate))
                {
                    dayOrdinal--;
                }

                currentDate = currentDate.AddDays(-1);
            }

            return (dayOrdinal == rulePattern.DayOrdinal);
        }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Populates a <b>SerializationInfo</b> with the data needed to serialize this
        /// object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
                info.AddValue("RRULE", ToString());
        }

        #endregion
    }
}
