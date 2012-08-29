using System;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    /// 	<para>Specifies the days of the week. Members might be combined using bitwise
    ///     operations to specify multiple days.</para>
    /// </summary>
    /// <remarks>
    ///     The constants in the <see cref="RecurrenceDay"/> enumeration might be combined
    ///     with bitwise operations to represent any combination of days. It is designed to be
    ///     used in conjunction with the <see cref="RecurrencePattern"/> class to filter
    ///     the days of the week for which the recurrence pattern applies.
    /// </remarks>
    /// <example>
    /// 	<para>Consider the following example that demonstrates the basic usage pattern of
    ///     RecurrenceDay. The most common operators used for manipulating bit fields
    ///     are:</para>
    /// 	<list type="bullet">
    /// 		<item>Bitwise OR: Turns a flag on.</item>
    /// 		<item>Bitwise XOR: Toggles a flag.</item>
    /// 		<item>Bitwise AND: Checks if a flag is turned on.</item>
    /// 		<item>Bitwise NOT: Turns a flag off.</item>
    /// 	</list>
    /// 	<code lang="CS">
    /// using System;
    /// using Micajah.Common.Bll.RecurringSchedule;
    ///  
    /// namespace RecurrenceExamples
    /// {
    ///     class RecurrenceDayExample
    ///     {
    ///         static void Main()
    ///         {
    ///             // Selects Friday, Saturday and Sunday.
    ///             RecurrenceDay dayMask = RecurrenceDay.Friday | RecurrenceDay.WeekendDays;
    ///             PrintSelectedDays(dayMask);
    ///  
    ///             // Selects all days, except Thursday.
    ///             dayMask = RecurrenceDay.EveryDay ^ RecurrenceDay.Thursday;
    ///             PrintSelectedDays(dayMask);
    ///         }
    ///  
    ///         static void PrintSelectedDays(RecurrenceDay dayMask)
    ///         {
    ///             Console.WriteLine("Value: {0,3} - {1}", (int) dayMask, dayMask);
    ///         }
    ///     }
    /// }
    ///  
    /// /*
    /// This example produces the following results:
    ///  
    /// Value: 112 - Friday, WeekendDays
    /// Value: 119 - Monday, Tuesday, Wednesday, Friday, WeekendDays
    /// */
    ///     </code>
    /// 	<code lang="VB">
    /// Imports System
    /// Imports Micajah.Common.Bll.RecurringSchedule
    ///  
    /// Namespace RecurrenceExamples
    ///     Class RecurrenceDayExample
    ///         Shared Sub Main()
    ///             ' Selects Friday, Saturday and Sunday.
    ///             Dim dayMask As RecurrenceDay = RecurrenceDay.Friday Or RecurrenceDay.WeekendDays
    ///             PrintSelectedDays(dayMask)
    ///  
    ///             ' Selects all days, except Thursday.
    ///             dayMask = RecurrenceDay.EveryDay Xor RecurrenceDay.Thursday
    ///             PrintSelectedDays(dayMask)
    ///         End Sub
    ///  
    ///         Shared Sub PrintSelectedDays(ByVal dayMask As RecurrenceDay)
    ///             Console.WriteLine("Value: {0,3} - {1}", DirectCast(dayMask, Integer), dayMask)
    ///         End Sub
    ///     End Class
    /// End Namespace
    ///  
    /// '
    /// 'This example produces the following results:
    /// '
    /// 'Value: 112 - Friday, WeekendDays
    /// 'Value: 119 - Monday, Tuesday, Wednesday, Friday, WeekendDays
    /// '
    ///     </code>
    /// </example>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames"), Flags()]
	public enum RecurrenceDay
	{
		/// <summary>Indicates no selected day.</summary>
		None = 0,
		/// <summary>Indicates Monday.</summary>
		Sunday = 1,
		/// <summary>Indicates Tuesday.</summary>
		Monday = 1 << 1,
		/// <summary>Indicates Wednesday.</summary>
		Tuesday = 1 << 2,
		/// <summary>Indicates Thursday.</summary>
		Wednesday = 1 << 3,
		/// <summary>Indicates Friday.</summary>
		Thursday = 1 << 4,
		/// <summary>Indicates Saturday.</summary>
		Friday = 1 << 5,
		/// <summary>Indicates Sunday.</summary>
		Saturday = 1 << 6,
		/// <summary><para>Indicates the range from Sunday to Saturday inclusive.</para></summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "EveryDay")]
        EveryDay = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
		/// <summary>Indicates the range from Monday to Friday inclusive.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WeekDays")]
        WeekDays = Monday | Tuesday | Wednesday | Thursday | Friday,
		/// <summary>Indicates the range from Saturday to Sunday inclusive.</summary>
		WeekendDays = Saturday | Sunday
	}
}
