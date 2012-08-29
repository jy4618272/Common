namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>Specifies the frequency of a recurrence.</summary>
    public enum RecurrenceFrequency
    {
        /// <summary>Indicates no recurrence.</summary>
        None,
        /// <summary>Indicates hourly recurrence.</summary>
        Hourly,
        /// <summary>Indicates daily recurrence.</summary>
        Daily,
        /// <summary>Indicates weekly recurrence.</summary>
        Weekly,
        /// <summary>Indicates monthly recurrence.</summary>
        Monthly,
        /// <summary>Indicates yearly recurrence.</summary>
        Yearly
    }
}
