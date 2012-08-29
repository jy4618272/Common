using System;
using System.Collections.Generic;
using System.Globalization;

namespace Micajah.Common.Bll.RecurringSchedule
{
    public static class DateHelper
    {
        public static string[] DayOrdinalValues() { return new string[] { "1", "2", "3", "4", "-1" }; }

        public static string[] DayMaskValues()
        {
            return new string[] {
                ((int) RecurrenceDay.EveryDay).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.WeekDays).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.WeekendDays).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Sunday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Monday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Tuesday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Wednesday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Thursday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Friday).ToString(CultureInfo.InvariantCulture),
                ((int) RecurrenceDay.Saturday).ToString(CultureInfo.InvariantCulture) };
        }

        public static DateTime GetStartOfWeek(DateTime selectedDate, DayOfWeek weekStart)
        {
            return GetStartOfWeek(selectedDate, weekStart, 7);
        }

        public static DateTime GetStartOfWeek(DateTime selectedDate, DayOfWeek weekStart, int numberDays)
        {
            int selectedDay = (int)selectedDate.DayOfWeek;
            if (selectedDay < (int)weekStart)
            {
                selectedDay += numberDays;
            }

            int DaysToSubtract = selectedDay - (int)weekStart;
            DateTime result = selectedDate.Subtract(TimeSpan.FromDays(DaysToSubtract));
            return new DateTime(result.Ticks, selectedDate.Kind);
        }

        public static DateTime GetEndOfWeek(DateTime selectedDate, DayOfWeek weekStart, int numberDays)
        {
            DateTime result = GetStartOfWeek(selectedDate, weekStart).AddDays(numberDays);
            return new DateTime(result.Ticks, selectedDate.Kind);
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            DateTime result = new DateTime(date.Year, date.Month, 1);
            return new DateTime(result.Ticks, date.Kind);
        }

        public static DateTime GetFirstDayOfMonth(DateTime selectedDate, DayOfWeek firstDayOfWeek, int weekLength)
        {
            DateTime day = GetFirstDayOfMonth(selectedDate);

            List<DayOfWeek> validDays = new List<DayOfWeek>(weekLength);
            for (int i = 0; i < weekLength; i++)
            {
                int validDay = ((int)firstDayOfWeek + i) % 7;
                validDays.Add((DayOfWeek)validDay);
            }

            while (!validDays.Contains(day.DayOfWeek))
            {
                day = day.AddDays(1);
            }

            return day;
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            DateTime result = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            return new DateTime(result.Ticks, date.Kind);
        }

        public static int GetNumberOfDaysInMonth(DateTime date)
        {
            TimeSpan month = GetLastDayOfMonth(date) - GetFirstDayOfMonth(date);
            return (int)month.TotalDays;
        }

        public static DateTime GetStartOfDay(DateTime day)
        {
            DateTime result = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
            return new DateTime(result.Ticks, day.Kind);
        }

        public static DateTime GetEndOfDay(DateTime day)
        {
            DateTime result = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);
            return new DateTime(result.Ticks, day.Kind);
        }

        public static DateTime GetStartOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(int year, int month)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
        }

        //public static bool RangeIsInsideAppointment(DateTime start, DateTime end, Appointment appointment)
        //{
        //    return appointment.Start <= start && appointment.End >= end;
        //}

        public static DateTime ToUtc(DateTime date)
        {
            return (date.Kind != DateTimeKind.Utc) ? date.ToUniversalTime() : date;
        }

        public static DateTime AssumeUtc(DateTime date)
        {
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        public static int GetWeekLength(DateTime date, DayOfWeek firstDayOfWeek, DayOfWeek lastDayOfWeek)
        {
            DateTime firstDay = GetStartOfWeek(date, firstDayOfWeek);
            DateTime weekEnd = firstDay;

            while (weekEnd.DayOfWeek != lastDayOfWeek)
            {
                weekEnd = weekEnd.AddDays(1);
            }

            return (int)((TimeSpan)(weekEnd - firstDay)).TotalDays + 1;
        }

    }
}
