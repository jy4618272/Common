using System;
using System.ComponentModel;
using System.Globalization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    /// <summary>
    /// Provides a type converter to convert RecurrenceRule objects to and from string
    /// representation.
    /// </summary>
    public class RecurrenceRuleConverter : TypeConverter
    {
        /// <summary>
        /// Overloaded. Returns whether this converter can convert an object of one type to
        /// the type of this converter.
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Overloaded. Converts the given value to the type of this converter.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Micajah.Common.Bll.RecurringSchedule.RecurrenceRule.TryParse(System.String,Micajah.Common.Bll.RecurringSchedule.RecurrenceRule@)")]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw new InvalidOperationException("Cannot convert from null value.");
            }

            string rruleString = value as string;
            if (!string.IsNullOrEmpty(rruleString))
            {
                RecurrenceRule rrule;
                RecurrenceRule.TryParse(rruleString, out rrule);

                return rrule;
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Overloaded. Returns whether this converter can convert the object to the
        /// specified type.
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>Overloaded. Converts the given value object to the specified type.</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                RecurrenceRule rrule = value as RecurrenceRule;
                if (rrule != null)
                {
                    return rrule.ToString();
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Overloaded. Returns whether the given value object is valid for this type.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Micajah.Common.Bll.RecurringSchedule.RecurrenceRule.TryParse(System.String,Micajah.Common.Bll.RecurringSchedule.RecurrenceRule@)")]
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            string rruleString = value as string;
            if (!string.IsNullOrEmpty(rruleString))
            {
                RecurrenceRule rrule;
                RecurrenceRule.TryParse(rruleString, out rrule);

                return rrule != null;
            }

            return base.IsValid(context, value);
        }
    }
}
