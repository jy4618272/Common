using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The helper class.
    /// </summary>
    public static class Support
    {
        #region Members

        /// <summary>
        /// The regular expression to match an e-mail address.
        /// </summary>
        internal const string EmailRegularExpression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// The regular expression to match an internet URL.
        /// </summary>
        internal const string UrlRegularExpression = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

        /// <summary>
        /// The regular expression to match the HTML tags.
        /// </summary>
        internal const string HtmlTagsRegularExpression = "<(.|\n)+?>";

        private const string ReservedChars = "$-_.+!*'(),@=&";
        private const string TrimCut = " ... ";

        private static CspParameters s_RsaCspParameters;
        private static CultureInfo s_UnitedStatesCulture;

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the parameters that are passed to the RSA cryptographic service provider.
        /// </summary>
        private static CspParameters RsaCspParameters
        {
            get
            {
                if (s_RsaCspParameters == null)
                {
                    s_RsaCspParameters = new CspParameters();
                    s_RsaCspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                }
                return s_RsaCspParameters;
            }
        }

        private static CultureInfo UnitedStatesCulture
        {
            get
            {
                if (s_UnitedStatesCulture == null)
                    s_UnitedStatesCulture = new CultureInfo("en-US");
                return s_UnitedStatesCulture;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using specified date and time formats.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <param name="omitUtc">Whether the UTC hours offset at the end of the result string is ommited.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone, int timeFormat, int? dateFormat, bool omitUtc)
        {
            string format = dateFormat.HasValue ? GetLongDateTimeFormatString(timeFormat, dateFormat.Value) : GetLongDateTimeFormatString(timeFormat);
            if (timeZone != null)
            {
                utcDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
                if (!omitUtc)
                {
                    format += " (UTC";
                    if (timeZone.BaseUtcOffset.TotalHours < 0)
                        format += timeZone.BaseUtcOffset.ToString().Substring(0, ((timeZone.BaseUtcOffset.TotalHours < 0) ? 6 : 5));
                    else if (timeZone.BaseUtcOffset.TotalHours > 0)
                        format += "+" + timeZone.BaseUtcOffset.ToString().Substring(0, ((timeZone.BaseUtcOffset.TotalHours < 0) ? 6 : 5));
                    format += ")";
                }
            }
            return string.Format(CultureInfo.CurrentCulture, format, utcDateTime);
        }

        /// <summary>
        /// Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="value">A Unicode character.</param>
        /// <returns>true if value needs to be converted; otherwise, false.</returns>
        private static bool NeedToEncode(char value)
        {
            if (value <= 127)
            {
                if (char.IsLetterOrDigit(value) || ReservedChars.IndexOf(value) >= 0)
                    return false;
            }
            return true;
        }

        #endregion

        #region Internal Methods

        internal static HtmlLink CreateStyleSheetLink(string styleSheetUrl)
        {
            return CreateStyleSheetLink(styleSheetUrl, null);
        }

        internal static HtmlLink CreateStyleSheetLink(string styleSheetUrl, string id)
        {
            using (HtmlLink link = new HtmlLink())
            {
                if (!string.IsNullOrEmpty(id)) link.ID = id;
                link.Attributes.Add("rel", "stylesheet");
                link.Attributes.Add("type", "text/css");
                link.Href = styleSheetUrl;
                return link;
            }
        }

        internal static Literal CreateJavaScriptLiteral(string scriptUrl, string id)
        {
            using (Literal script = new Literal())
            {
                script.ID = id;
                script.Text = ResourceProvider.GetJavaScript(scriptUrl);
                return script;
            }
        }

        internal static void SetPropertyValueSafe(PropertyInfo p, object obj, object value, object[] index)
        {
            if (p != null)
            {
                try
                {
                    p.SetValue(obj, value, index);
                }
                catch (MethodAccessException) { }
                catch (ArgumentException) { }
                catch (TargetException) { }
                catch (TargetParameterCountException) { }
            }
        }

        #endregion

        #region Public Methods

        #region Date and Time

        /// <summary>
        /// Returns the long date and time format: "MMM d, yyyy h:mm tt" (12 hours).
        /// </summary>
        /// <returns>The date and time format.</returns>
        public static string GetLongDateTimeFormat()
        {
            return GetLongDateTimeFormat(0);
        }

        /// <summary>
        /// Returns long date and time format: "MMM d, yyyy h:mm tt" (12 hours) or "MMM d, yyyy H:mm" (24 hours).
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>The date and time format.</returns>
        public static string GetLongDateTimeFormat(int timeFormat)
        {
            return "MMM d, yyyy " + GetTimeFormat(timeFormat);
        }

        /// <summary>
        /// Returns long date and time format.
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <returns>The date and time format.</returns>
        public static string GetLongDateTimeFormat(int timeFormat, int dateFormat)
        {
            return GetShortDateFormat(dateFormat) + GetTimeFormat(timeFormat);
        }

        /// <summary>
        /// Returns the long date and time format string: "{0:MMM d, yyyy h:mm tt}" (12 hours).
        /// </summary>
        /// <returns>The date and time format string.</returns>
        public static string GetLongDateTimeFormatString()
        {
            return GetLongDateTimeFormatString(0);
        }

        /// <summary>
        /// Returns long date and time format string: "{0:MMM d, yyyy h:mm tt}" (12 hours) or "{0:MMM d, yyyy H:mm}" (24 hours).
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>The date and time format string.</returns>
        public static string GetLongDateTimeFormatString(int timeFormat)
        {
            return "{0:" + GetLongDateTimeFormat(timeFormat) + "}";
        }

        /// <summary>
        /// Returns long date and time format string: "{0:MMM d, yyyy h:mm tt}" (12 hours) or "{0:MMM d, yyyy H:mm}" (24 hours).
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <returns>The date and time format string.</returns>
        public static string GetLongDateTimeFormatString(int timeFormat, int dateFormat)
        {
            return "{0:" + GetLongDateTimeFormat(timeFormat, dateFormat) + "}";
        }

        /// <summary>
        /// Returns the short date format: "d-MMM-yyyy".
        /// </summary>
        /// <returns>The date format.</returns>
        public static string GetShortDateFormat()
        {
            return "d-MMM-yyyy";
        }

        /// <summary>
        /// Returns short date format: "MM/dd/yyyy" or "dd/MM/yyyy".
        /// </summary>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <returns>The date format.</returns>
        public static string GetShortDateFormat(int dateFormat)
        {
            return ((dateFormat == 0) ? "MM/dd/yyyy" : "dd/MM/yyyy");
        }

        /// <summary>
        /// Returns the long date and time format string: "{0:d-MMM-yyyy}".
        /// </summary>
        /// <returns>The date format string.</returns>
        public static string GetShortDateFormatString()
        {
            return "{0:" + GetShortDateFormat() + "}";
        }

        /// <summary>
        /// Returns short date format string: "{0:MM/dd/yyyy}" or "{0:dd/MM/yyyy}".
        /// </summary>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "{0:MM/dd/yyyy}" format, 1 to use "{0:dd/MM/yyyy}" format.</param>
        /// <returns>The date format string.</returns>
        public static string GetShortDateFormatString(int dateFormat)
        {
            return "{0:" + GetShortDateFormat(dateFormat) + "}";
        }

        /// <summary>
        /// Returns the time zone by specified hours offset using the limited list of most popular time zones, or empty string if it is not found.
        /// </summary>
        /// <param name="hoursOffset">A number of hours.</param>
        /// <returns>Time zone identifier.</returns>
        public static string GetTimeZoneId(double hoursOffset)
        {
            switch (((hoursOffset > 0) ? "+" : string.Empty) + TimeSpan.FromHours(hoursOffset).ToString().Substring(0, ((hoursOffset < 0) ? 6 : 5)))
            {
                case "00:00":
                    return "UTC";
                case "+01:00":
                    return "Central Europe Standard Time";
                case "+02:00":
                    return "E. Europe Standard Time";
                case "+03:00":
                    return "Arabic Standard Time";
                case "+03:30":
                    return "Iran Standard Time";
                case "+04:00":
                    return "Russian Standard Time";
                case "+04:30":
                    return "Afghanistan Standard Time";
                case "+05:00":
                    return "West Asia Standard Time";
                case "+05:30":
                    return "India Standard Time";
                case "+05:45":
                    return "Nepal Standard Time";
                case "+06:00":
                    return "Central Asia Standard Time";
                case "+06:30":
                    return "Myanmar Standard Time";
                case "+07:00":
                    return "SE Asia Standard Time";
                case "+08:00":
                    return "North Asia Standard Time";
                case "+09:00":
                    return "North Asia East Standard Time";
                case "+09:30":
                    return "Cen. Australia Standard Time";
                case "+10:00":
                    return "West Pacific Standard Time";
                case "+11:00":
                    return "Central Pacific Standard Time";
                case "+12:00":
                    return "UTC+12";
                case "+13:00":
                    return "Samoa Standard Time";
                case "-01:00":
                    return "Azores Standard Time";
                case "-02:00":
                    return "UTC-02";
                case "-03:00":
                    return "E. South America Standard Time";
                case "-03:30":
                    return "Newfoundland Standard Time";
                case "-04:00":
                    return "Atlantic Standard Time";
                case "-04:30":
                    return "Venezuela Standard Time";
                case "-05:00":
                    return "Eastern Standard Time";
                case "-06:00":
                    return "Central Standard Time";
                case "-07:00":
                    return "Mountain Standard Time";
                case "-08:00":
                    return "Pacific Standard Time";
                case "-09:00":
                    return "Alaskan Standard Time";
                case "-10:00":
                    return "Hawaiian Standard Time";
                case "-11:00":
                    return "UTC-11";
                case "-12:00":
                    return "Dateline Standard Time";
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the time format.
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>The time format.</returns>
        public static string GetTimeFormat(int timeFormat)
        {
            return ((timeFormat == 0) ? " h:mm tt" : " H:mm");
        }

        /// <summary>
        /// Returns the time format string.
        /// </summary>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>The time format string.</returns>
        public static string GetTimeFormatString(int timeFormat)
        {
            return "{0:" + GetTimeFormat(timeFormat) + "}";
        }

        /// <summary>
        /// Converts the date to string in "d-MMM-yyyy" format.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>String that contains the formatted date.</returns>
        public static string ToShortDateString(DateTime date)
        {
            return ToShortDateString(date, null);
        }

        /// <summary>
        /// Converts the UTC date using specified time zone and return the result as string in "d-MMM-yyyy" format.
        /// </summary>
        /// <param name="utcDate">The UTC date to convert.</param>
        /// <param name="timeZone">The time zone to convert date to.</param>
        /// <returns>String that contains the formatted date.</returns>
        public static string ToShortDateString(DateTime utcDate, TimeZoneInfo timeZone)
        {
            if (timeZone != null)
                utcDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);
            return string.Format(CultureInfo.CurrentCulture, GetShortDateFormatString(), utcDate);
        }

        /// <summary>
        /// Converts the date to string in specified format.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "{0:MM/dd/yyyy}" format, 1 to use "{0:dd/MM/yyyy}" format.</param>
        /// <returns>String that contains the formatted date.</returns>
        public static string ToShortDateString(DateTime date, int dateFormat)
        {
            return ToShortDateString(date, null, dateFormat);
        }

        /// <summary>
        /// Converts the UTC date using specified time zone and return the result as string in specified format.
        /// </summary>
        /// <param name="utcDate">The UTC date to convert.</param>
        /// <param name="timeZone">The time zone to convert date to.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "{0:MM/dd/yyyy}" format, 1 to use "{0:dd/MM/yyyy}" format.</param>
        /// <returns>String that contains the formatted date.</returns>
        public static string ToShortDateString(DateTime utcDate, TimeZoneInfo timeZone, int dateFormat)
        {
            if (timeZone != null)
                utcDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);
            return string.Format(CultureInfo.CurrentCulture, GetShortDateFormatString(dateFormat), utcDate);
        }

        /// <summary>
        /// Converts the time to string using "h:mm tt" (12 hours) format.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns>String that contains the formatted time.</returns>
        public static string ToShortTimeString(DateTime time)
        {
            return ToShortTimeString(time, 0);
        }

        /// <summary>
        /// Converts the time to string using specified format.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>String that contains the formatted time.</returns>
        public static string ToShortTimeString(DateTime date, int timeFormat)
        {
            return ToShortTimeString(date, null, timeFormat);
        }

        /// <summary>
        /// Converts the UTC time using specified time zone and return the result as string in specified format.
        /// </summary>
        /// <param name="utcTime">The UTC time to convert.</param>
        /// <param name="timeZone">The time zone to convert time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>String that contains the formatted time.</returns>
        public static string ToShortTimeString(DateTime utcTime, TimeZoneInfo timeZone, int timeFormat)
        {
            if (timeZone != null)
                utcTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
            return string.Format(UnitedStatesCulture, GetTimeFormatString(timeFormat), utcTime);
        }

        /// <summary>
        /// Converts the date and time using specified time zone and return the result as formatted string using "MMM d, yyyy" date format and 12 hours time format.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime dateTime)
        {
            return ToLongDateTimeString(dateTime, null);
        }

        /// <summary>
        /// Converts the date and time using specified time zone and return the result as formatted string using "MMM d, yyyy" date format and 12 or 24 hours time format.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime dateTime, int timeFormat)
        {
            return ToLongDateTimeString(dateTime, null, timeFormat, false);
        }

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using "MMM d, yyyy" date format and 12 time format.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone)
        {
            return ToLongDateTimeString(utcDateTime, timeZone, 0, false);
        }

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using "MMM d, yyyy" date format and 12 or 24 hours time format.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone, int timeFormat)
        {
            return ToLongDateTimeString(utcDateTime, timeZone, timeFormat, false);
        }

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using "MMM d, yyyy" date format and 12 or 24 hours time format.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="omitUtc">Whether the UTC hours offset at the end of the result string is ommited.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone, int timeFormat, bool omitUtc)
        {
            return ToLongDateTimeString(utcDateTime, timeZone, timeFormat, null, omitUtc);
        }

        /// <summary>
        /// Converts the date and time using specified time zone and return the result as formatted string using specified date and time formats.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime dateTime, int timeFormat, int dateFormat)
        {
            return ToLongDateTimeString(dateTime, null, timeFormat, dateFormat, false);
        }

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using specified date and time formats.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone, int timeFormat, int dateFormat)
        {
            return ToLongDateTimeString(utcDateTime, timeZone, timeFormat, dateFormat, false);
        }

        /// <summary>
        /// Converts the UTC date and time using specified time zone and return the result as formatted string using specified date and time formats.
        /// </summary>
        /// <param name="utcDateTime">The UTC date and time to convert.</param>
        /// <param name="timeZone">The time zone to convert date and time to.</param>
        /// <param name="timeFormat">The format to convert time to. Possible values: 0 to use "h:mm tt" (12 hours) format, 1 to use "H:mm" (24 hours) format.</param>
        /// <param name="dateFormat">The format to convert date to. Possible values: 0 to use "MM/dd/yyyy" format, 1 to use "dd/MM/yyyy" format.</param>
        /// <param name="omitUtc">Whether the UTC hours offset at the end of the result string is ommited.</param>
        /// <returns>String that contains the formatted date and time.</returns>
        public static string ToLongDateTimeString(DateTime utcDateTime, TimeZoneInfo timeZone, int timeFormat, int dateFormat, bool omitUtc)
        {
            return ToLongDateTimeString(utcDateTime, timeZone, timeFormat, new int?(dateFormat), omitUtc);
        }

        #endregion

        #region Serialization

        public static string Serialize(object value)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                LosFormatter formatter = new LosFormatter();
                formatter.Serialize(writer, value);
            }
            return sb.ToString();
        }

        internal static object Deserialize(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            LosFormatter formatter = new LosFormatter();
            return formatter.Deserialize(value);
        }

        #endregion

        public static string Trim(string[] value, TrimSide trimSide, string delimiter, int maxLength, bool highlightLastElement)
        {
            string result = string.Empty;

            if (value == null) return result;

            if (highlightLastElement)
            {
                value[value.Length - 1] = "<b>" + value[value.Length - 1] + "</b>";
                maxLength += 7;
            }

            if (value.Length == 1)
                result = value.ToString();
            else
            {
                string minTrim = value[0] + TrimCut + delimiter + value[value.Length - 1];
                if (maxLength < minTrim.Length)
                    result = minTrim;
                else
                {
                    string source = string.Empty;
                    string leftPart = value[0] + delimiter;
                    string rightPart = TrimCut + delimiter + value[value.Length - 1];

                    for (int i = 1; i < value.Length; i++)
                    {
                        source += value[i];
                        if (i != (value.Length - 1))
                            source += delimiter;
                    }

                    if (maxLength > (leftPart.Length + source.Length))
                    {
                        maxLength = (leftPart.Length + source.Length);
                        result = leftPart + source;
                    }
                    else
                    {
                        leftPart = leftPart + TrimCut;
                        maxLength -= leftPart.Length;

                        switch (trimSide)
                        {
                            case TrimSide.Left:
                                result = leftPart + source.Substring(source.Length - maxLength, maxLength);
                                break;
                            case TrimSide.Center:
                                maxLength -= rightPart.Length;
                                string centerPart = source.Substring(0, source.Length - rightPart.Length);
                                if (maxLength > 0)
                                {
                                    double point = maxLength / 2;
                                    int centerPoint = Convert.ToInt32(Math.Ceiling(point));
                                    result = leftPart + centerPart.Substring(centerPoint, maxLength) + rightPart;
                                }
                                else
                                    result = leftPart + centerPart + rightPart;
                                break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the MD5 hash value for the specified string.
        /// </summary>
        /// <param name="value">The string to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        public static string CalculateMD5Hash(string value)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hash = null;

            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(inputBytes);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns an System.Object with the specified System.Type and whose value is equivalent to the specified value.
        /// </summary>
        /// <param name="value">A System.String.</param>
        /// <param name="conversionType">A System.Type.</param>
        /// <returns>
        /// An object whose System.Type is conversionType and whose value is equivalent to value.
        /// -or- value, if the System.Type of value and conversionType are equal.
        /// -or- null, if value is null and conversionType is not a value type.
        /// </returns>
        public static object ConvertStringToType(string value, Type conversionType)
        {
            object valueObj = null;

            if (value != null)
            {
                if (conversionType != null)
                {
                    if (conversionType == typeof(bool))
                    {
                        bool val = false;
                        if (bool.TryParse(value, out val))
                            valueObj = val;
                    }
                    else if (conversionType == typeof(int))
                    {
                        int val = 0;
                        if (int.TryParse(value, out val))
                            valueObj = val;
                    }
                    else
                    {
                        try
                        {
                            if (conversionType.IsEnum)
                                valueObj = Convert.ChangeType(Enum.Parse(conversionType, value), conversionType, CultureInfo.CurrentCulture);
                            else if (conversionType == typeof(Unit))
                                valueObj = Unit.Parse(value, CultureInfo.InvariantCulture);
                            else if (conversionType == typeof(Guid))
                                valueObj = new Guid(value);
                            else
                                valueObj = Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
                        }
                        catch (ArgumentException) { }
                        catch (FormatException) { }
                        catch (InvalidCastException) { }
                        catch (OverflowException) { }
                    }
                }
            }

            return valueObj;
        }

        public static object ConvertStringToType(string value, Type conversionType, TypeConverter converter)
        {
            object valueObj = null;

            if ((value != null) && (conversionType != null))
            {
                if (converter != null)
                {
                    try
                    {
                        if (converter.CanConvertFrom(typeof(string)))
                            valueObj = converter.ConvertFrom(value);
                        else if (converter.CanConvertTo(conversionType))
                            valueObj = converter.ConvertTo(value, conversionType);
                    }
                    catch (ArgumentException) { }
                    catch (FormatException) { }
                    catch (InvalidCastException) { }
                    catch (OverflowException) { }
                }
                else
                    valueObj = ConvertStringToType(value, conversionType);
            }

            return valueObj;
        }

        public static ICollection<Guid> ConvertStringToGuidList(string value)
        {
            return ConvertStringToGuidList(value, ",");
        }

        public static ICollection<Guid> ConvertStringToGuidList(string value, string separator)
        {
            List<Guid> list = null;
            if (value != null)
            {
                list = new List<Guid>();
                if (value.Length > 0)
                {
                    string[] parts = value.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                    Type type = typeof(Guid);
                    foreach (string id in parts)
                    {
                        object obj = ConvertStringToType(id, type);
                        if (obj != null)
                        {
                            Guid guid = (Guid)obj;
                            if (!list.Contains(guid))
                                list.Add(guid);
                        }
                    }
                }
            }
            return list;
        }

        public static string ConvertListToString(IEnumerable list)
        {
            return ConvertListToString(list, ",");
        }

        public static string ConvertListToString(IEnumerable list, string separator)
        {
            string groupIdString = null;
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object obj in list)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", separator, obj);
                }

                if (sb.Length > 0) sb.Remove(0, 1);
                groupIdString = sb.ToString();
            }
            return groupIdString;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="command">Specifies the SqlCommand that represents a Transact-SQL statement or stored procedure to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQuery(SqlCommand command)
        {
            int returnValue = 0;
            if (command != null)
            {
                command.Connection.Open();
                returnValue = command.ExecuteNonQuery();
                command.Connection.Close();
            }
            return returnValue;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        /// <param name="connection">The connection to an instance of SQL Server.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQuery(string commandText, SqlConnection connection)
        {
            int returnValue = 0;
            if (!string.IsNullOrEmpty(commandText))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    return ExecuteNonQuery(command);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="command">Specifies the SqlCommand that represents a Transact-SQL statement or stored procedure to execute.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(SqlCommand command)
        {
            object returnValue = null;
            if (command != null)
            {
                ConnectionState previousConnectionState = command.Connection.State;

                try
                {
                    command.Connection.Open();
                    returnValue = command.ExecuteScalar();
                }
                finally
                {
                    if (previousConnectionState == ConnectionState.Closed) command.Connection.Close();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="commandText">The text of the query.</param>
        /// <param name="connection">The connection to an instance of SQL Server.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string commandText, SqlConnection connection)
        {
            object returnValue = null;
            if (!string.IsNullOrEmpty(commandText))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    return ExecuteScalar(command);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Extracts the value of the specified parameter from specified query string or url.
        /// </summary>
        /// <param name="url">The url or query string.</param>
        /// <param name="parameterName">The name of the query string parameter.</param>
        /// <returns>The string that represents the value of the query string parameter.</returns>
        public static string ExtractQueryStringParameterValue(string url, string parameterName)
        {
            if (string.IsNullOrEmpty(url)) return null;
            if (string.IsNullOrEmpty(parameterName)) return null;

            string parameterValue = null;
            string queryString = url;
            string[] parts = url.Split('?');
            if (parts.Length > 1) queryString = parts[1];

            int startIndex = -1;
            int offset = 0;
            if (queryString.StartsWith(parameterName + "=", StringComparison.OrdinalIgnoreCase))
            {
                startIndex = 0;
                offset = parameterName.Length + 1;
            }
            else
            {
                startIndex = queryString.IndexOf("&" + parameterName + "=", StringComparison.OrdinalIgnoreCase);
                offset = parameterName.Length + 2;
            }
            if (startIndex > -1)
            {
                startIndex += offset;
                int endIndex = queryString.IndexOf("&", startIndex, StringComparison.OrdinalIgnoreCase);
                if (endIndex > -1)
                    parameterValue = queryString.Substring(startIndex, endIndex - startIndex);
                else
                    parameterValue = queryString.Substring(startIndex);
            }

            return parameterValue;
        }

        public static Control FindTargetControl(string controlId, Control control, bool searchNamingContainers)
        {
            if (string.IsNullOrEmpty(controlId)) return null;
            if (control == null) return null;

            if (searchNamingContainers)
            {
                Control namingContainer = control;
                Control control2 = null;
                while ((control2 == null) && (namingContainer != control.Page))
                {
                    namingContainer = namingContainer.NamingContainer;
                    if (namingContainer == null) return control2;
                    control2 = namingContainer.FindControl(controlId);
                }
                return control2;
            }

            return control.FindControl(controlId);
        }

        /// <summary>
        /// Generates a password with specified parameters.
        /// </summary>
        /// <param name="length">The length of the password.</param>
        /// <param name="numberOfNonAlphanumericCharacters">The number of special characters that must be present in the password.</param>
        /// <returns>An System.String object that represents the password with specified parameters.</returns>
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            byte[] pass_bytes = new byte[length];
            int i;
            int num_nonalpha = 0;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(pass_bytes);
            }

            for (i = 0; i < length; i++)
            {
                // Convert the random bytes to ascii values 33-126
                pass_bytes[i] = (byte)(pass_bytes[i] % 93 + 33);

                // Count the number of non-alphanumeric characters we have as we go
                if ((pass_bytes[i] >= 33 && pass_bytes[i] <= 47)
                    || (pass_bytes[i] >= 58 && pass_bytes[i] <= 64)
                    || (pass_bytes[i] >= 91 && pass_bytes[i] <= 96)
                    || (pass_bytes[i] >= 123 && pass_bytes[i] <= 126))
                    num_nonalpha++;

                // Get rid of any quotes, just in case they cause problems
                if (pass_bytes[i] == 34 || pass_bytes[i] == 39)
                    pass_bytes[i]++;
                else if (pass_bytes[i] == 96)
                    pass_bytes[i]--;
            }

            if (num_nonalpha < numberOfNonAlphanumericCharacters)
            {
                // Loop over the array, converting the least number of alphanumeric characters to non-alpha
                for (i = 0; i < length; i++)
                {
                    if (num_nonalpha == numberOfNonAlphanumericCharacters)
                        break;
                    if (pass_bytes[i] >= 48 && pass_bytes[i] <= 57)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] - 48 + 33);
                        num_nonalpha++;
                    }
                    else if (pass_bytes[i] >= 65 && pass_bytes[i] <= 90)
                    {
                        pass_bytes[i] = (byte)((pass_bytes[i] - 65) % 13 + 33);
                        num_nonalpha++;
                    }
                    else if (pass_bytes[i] >= 97 && pass_bytes[i] <= 122)
                    {
                        pass_bytes[i] = (byte)((pass_bytes[i] - 97) % 13 + 33);
                        num_nonalpha++;
                    }

                    // Make sure we don't end up with quote characters
                    if (pass_bytes[i] == 34 || pass_bytes[i] == 39)
                        pass_bytes[i]++;
                    else if (pass_bytes[i] == 96)
                        pass_bytes[i]--;
                }
            }
            else if (num_nonalpha > numberOfNonAlphanumericCharacters)
            {
                for (i = 0; i < length; i++)
                {
                    if (num_nonalpha == numberOfNonAlphanumericCharacters)
                        break;

                    if (pass_bytes[i] >= 33 && pass_bytes[i] <= 42)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] + 15);
                        num_nonalpha--;
                    }
                    else if (pass_bytes[i] >= 43 && pass_bytes[i] <= 47)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] + 22);
                        num_nonalpha--;
                    }
                    else if (pass_bytes[i] >= 58 && pass_bytes[i] <= 64)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] + 12);
                        num_nonalpha--;
                    }
                    else if (pass_bytes[i] >= 91 && pass_bytes[i] <= 96)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] - 14);
                        num_nonalpha--;
                    }
                    else if (pass_bytes[i] >= 123 && pass_bytes[i] <= 126)
                    {
                        pass_bytes[i] = (byte)(pass_bytes[i] - 4);
                        num_nonalpha--;
                    }
                }
            }

            for (i = 0; i < length; i++)
            {
                // Get rid of 0, 1, l, o, I, O in the password, just in case they cause problems
                if (pass_bytes[i] == 73 || pass_bytes[i] == 79 || pass_bytes[i] == 108 || pass_bytes[i] == 111)
                    pass_bytes[i]++;
                else if (pass_bytes[i] == 48)
                    pass_bytes[i] += 8;
                else if (pass_bytes[i] == 49)
                    pass_bytes[i] += 6;
            }

            return Encoding.ASCII.GetString(pass_bytes);
        }

        /// <summary>
        /// Generates the pseudo unique identifier.
        /// </summary>
        /// <returns>An System.String object that represents the pseudo unique identifier.</returns>
        public static string GeneratePseudoUnique()
        {
            return GeneratePseudoUnique(6);
        }

        /// <summary>
        /// Generates the pseudo unique identifier with specified length.
        /// </summary>
        /// <param name="length">The length of the identifier.</param>
        /// <returns>An System.String object that represents the pseudo unique identifier.</returns>
        public static string GeneratePseudoUnique(int length)
        {
            //"abcdefghijkmnopqrstuvwxyz0123456789"
            byte[] pass_bytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(pass_bytes);
            }

            for (int i = 0; i < length; i++)
            {
                // Convert the random bytes to ascii values 33-126
                pass_bytes[i] = (byte)(pass_bytes[i] % 93 + 33);

                if (pass_bytes[i] > 32 && pass_bytes[i] < 48)
                    pass_bytes[i] += 64;
                else if (pass_bytes[i] > 57 && pass_bytes[i] < 65)
                    pass_bytes[i] -= 10;
                else if (pass_bytes[i] > 64 && pass_bytes[i] < 91)
                    pass_bytes[i] += 32;
                else if (pass_bytes[i] > 90 && pass_bytes[i] < 97)
                    pass_bytes[i] += 11;
                else if (pass_bytes[i] > 122)
                    pass_bytes[i] -= 15;

                if (pass_bytes[i] == 108)
                    pass_bytes[i]++;
            }

            return Encoding.ASCII.GetString(pass_bytes);
        }

        /// <summary>
        /// Converts a string to an array of bytes.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The array of bytes.</returns>
        public static byte[] GetBytes(string value)
        {
            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(value);
                }
                bytes = stream.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Gets a new DataTable that is filled by SqlDataAdapter with specified SqlCommand.
        /// </summary>
        /// <param name="command">Specifies the SqlCommand that is used to fill a DataTable.</param>
        /// <returns>A new DataTable that is filled by SqlDataAdapter with specified SqlCommand.</returns>
        public static DataTable GetDataTable(SqlCommand command)
        {
            if (command == null) return null;

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {

                DataTable table = new DataTable();
                table.Locale = CultureInfo.CurrentCulture;

                adapter.Fill(table);

                if (command.Connection != null && command.Connection.State == ConnectionState.Open) command.Connection.Close();

                return table;
            }
        }

        /// <summary>
        /// Gets a DataRow object populated with current values from the data sourceRow.
        /// </summary>
        /// <param name="command">Specifies the SqlCommand.</param>
        /// <returns>a DataRow object populated with current values.</returns>
        public static DataRow GetDataRow(SqlCommand command)
        {
            DataTable table = GetDataTable(command);
            if (table != null)
            {
                if (table.Rows.Count > 0) return table.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// Gets a DataRowView object populated with current values from the data sourceRow.
        /// </summary>
        /// <param name="command">Specifies the SqlCommand.</param>
        /// <returns>a DataRowView object populated with current values.</returns>
        public static DataRowView GetDataRowView(SqlCommand command)
        {
            DataTable table = GetDataTable(command);
            if (table != null)
            {
                if (table.Rows.Count > 0) return table.DefaultView[0];
            }
            return null;
        }

        /// <summary>
        /// Returns the last element of the string array that contains the substrings in the specified string that are delimited by specified delimiter.
        /// </summary>
        /// <param name="value">The string to return the last part of.</param>
        /// <param name="delimiter">The string that delimit the substrings in the string.</param>
        /// <returns>The last element of the string array or null reference, if the delimiter is not found.</returns>
        public static string GetLastPartOfString(string value, string delimiter)
        {
            return GetLastPartOfString(value, delimiter, false);
        }

        /// <summary>
        /// Returns the last element of the string array that contains the substrings in the specified string that are delimited by specified delimiter.
        /// </summary>
        /// <param name="value">The string to return the last part of.</param>
        /// <param name="delimiter">The string that delimit the substrings in the string.</param>
        /// <param name="includeDelimiter">Specifies a value indicating whether the delimiter should be returned at the begin of the result string.</param>
        /// <returns>The last element of the string array or null reference, if the delimiter is not found.</returns>
        public static string GetLastPartOfString(string value, string delimiter, bool includeDelimiter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    string[] array1 = value.Split(new string[] { delimiter }, StringSplitOptions.None);
                    string lastPart = array1[array1.Length - 1];
                    if (includeDelimiter) lastPart = delimiter + lastPart;
                    return lastPart;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts an array of bytes to a string.
        /// </summary>
        /// <param name="bytes">The array of bytes to convert.</param>
        /// <returns>The string.</returns>
        public static string GetString(byte[] value)
        {
            if (value == null) return null;
            if (value.Length == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (byte b in value)
            {
                sb.Append((char)b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a value indicating whether the specified System.Object object is null reference or it represents a nonexistent value.
        /// </summary>
        /// <param name="value">A System.Object reference.</param>
        /// <returns>true if the value parameter is null reference or it represents a nonexistent value; otherwise, false.</returns>
        public static bool IsNullOrDBNull(object value)
        {
            return ((value == null) || (value.Equals(DBNull.Value)));
        }

        /// <summary>
        /// Escape double-quotation mark characters that are contained in a specified string.
        /// </summary>
        /// <param name="value">The string in which to preserve double-quotation marks.</param>
        /// <returns>A string after escaping double-quotation mark characters.</returns>
        public static string PreserveDoubleQuote(string value)
        {
            if (!string.IsNullOrEmpty(value)) return value.Replace("\"", "&quot;");
            return value;
        }

        /// <summary>
        /// Escape single-quotation mark characters that are contained in a specified string.
        /// </summary>
        /// <param name="value">The string in which to preserve single-quotation marks.</param>
        /// <returns>A string after escaping single-quotation mark characters.</returns>
        public static string PreserveSingleQuote(string value)
        {
            if (!string.IsNullOrEmpty(value)) return value.Replace("'", "''");
            return value;
        }

        /// <summary>
        /// Returns a string that contains a specified number of copies of the original string.
        /// </summary>
        /// <param name="value">A string to copy.</param>
        /// <param name="repeatCount">The number of times to copy value.</param>
        /// <param name="delimiter">The delimiter between copies of the string.</param>
        /// <returns>A string that contains a specified number of copies of the original string.</returns>
        public static string RepeatString(string value, int repeatCount, string delimiter)
        {
            if ((!string.IsNullOrEmpty(value)) && (repeatCount > 1))
            {
                if (delimiter == null) delimiter = string.Empty;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < repeatCount; i++)
                {
                    if (i > 0) sb.Append(delimiter);
                    sb.Append(value);
                }

                return sb.ToString();
            }

            return value;
        }

        /// <summary>
        /// Replaces the keys by the values in specified string.
        /// </summary>
        /// <param name="value">A string to replace in.</param>
        /// <param name="dictionary">The key and values to replace.</param>
        /// <returns>A result string.</returns>
        public static string ReplaceInString(string value, Dictionary<string, string> dictionary)
        {
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    value = value.Replace(key, dictionary[key]);
                }
            }
            return value;
        }

        public static ArrayList RemoveDuplicates(ArrayList value)
        {
            ArrayList list = null;

            if (value != null)
            {
                list = new ArrayList();

                foreach (object obj in value)
                {
                    if (!list.Contains(obj))
                    {
                        list.Add(obj);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Encryptes the string using RSA private key.
        /// </summary>
        /// <param name="str">The string to encrypt.</param>
        /// <returns>The encrypted string encoded with base 64 digits.</returns>
        public static string Encrypt(string value)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(RsaCspParameters))
            {
                provider.FromXmlString(FrameworkConfiguration.Current.Security.PrivateKey);
                return Convert.ToBase64String(provider.Encrypt(GetBytes(value), false));
            }
        }

        /// <summary>
        /// Decryptes the specified string, which encodes binary data as base 64 digits, using RSA public key.
        /// </summary>
        /// <param name="str">The string to decrypt.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string value)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(RsaCspParameters))
            {
                provider.FromXmlString(FrameworkConfiguration.Current.Security.PrivateKey);
                return GetString(provider.Decrypt(Convert.FromBase64String(value), false));
            }
        }

        /// <summary>
        /// Indicates whether the specified System.String object (in single-line or muliline modes) is null or an System.String.Empty string or contains only spaces.
        /// </summary>
        /// <param name="value">A System.String reference.</param>
        /// <returns>true if the value parameter is null or an empty string ("") or contains only spaces; otherwise, false.</returns>
        public static bool StringIsNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            return (!Regex.IsMatch(value, @"\S+", RegexOptions.Multiline));
        }

        /// <summary>
        /// Removes all HTML tags from the specified string.
        /// </summary>
        /// <param name="value">A System.String reference.</param>
        /// <returns>Returns the stripped string.</returns>
        public static string StripHtmlTags(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return Regex.Replace(value, HtmlTagsRegularExpression, string.Empty);
        }

        public static string SplitCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return Regex.Replace(value, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        /// <summary>
        /// Trims the specified string to specified length.
        /// </summary>
        /// <param name="value">The string to cut.</param>
        /// <param name="length">The new length of the string.</param>
        /// <returns>The trimmed string or original string if the specified length less than current length of the string.</returns>
        public static string TrimString(string value, int length)
        {
            if (value != null)
            {
                if (value.Length > length)
                    return value.Substring(0, length);
            }
            return value;
        }

        /// <summary>
        /// Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="value">A string to encode.</param>
        /// <returns>Encoded string.</returns>
        public static string ToHexString(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            UTF8Encoding utf8 = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();
            foreach (char chr in value)
            {
                if (NeedToEncode(chr))
                {
                    byte[] encodedBytes = utf8.GetBytes(chr.ToString());
                    for (int index = 0; index < encodedBytes.Length; index++)
                    {
                        sb.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
                    }
                }
                else
                    sb.Append(chr);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Sends the message with specified details to an SMTP server for delivery.
        /// </summary>
        /// <param name="sender">A System.String that contains the sender's address for this e-mail message.</param>
        /// <param name="from">A System.String that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A System.String that contains the addresses of the recipients of the e-mail message.</param>
        /// <param name="bcc">A System.String that contains the blind carbon copy (BCC) recipients for the e-mail message.</param>
        /// <param name="subject">The subject line for the e-mail message.</param>
        /// <param name="body">The message body.</param>
        /// <param name="isBodyHtml">true if the message body is in HTML; else false.</param>
        /// <param name="async">If it's true this method does not block the calling thread.</param>
        /// <param name="reason">A reason for email sending.</param>
        /// <returns>true if the message was sent successfully; otherwise, false.</returns>
        public static bool SendEmail(string sender, string from, string to, string bcc, string subject, string body, bool isBodyHtml, bool async, EmailSendingReason reason)
        {
            EmailSendingEventArgs args = new EmailSendingEventArgs() { Async = async, Reason = reason };
            return SendEmail(sender, from, to, bcc, subject, body, isBodyHtml, args);
        }

        /// <summary>
        /// Sends the message with specified details to an SMTP server for delivery.
        /// </summary>
        /// <param name="sender">A System.String that contains the sender's address for this e-mail message.</param>
        /// <param name="from">A System.String that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A System.String that contains the addresses of the recipients of the e-mail message.</param>
        /// <param name="bcc">A System.String that contains the blind carbon copy (BCC) recipients for the e-mail message.</param>
        /// <param name="subject">The subject line for the e-mail message.</param>
        /// <param name="body">The message body.</param>
        /// <param name="isBodyHtml">true if the message body is in HTML; else false.</param>
        /// <param name="async">If it's true this method does not block the calling thread.</param>
        /// <param name="args">An EmailSendingEventArgs that contains the Micajah.Common.Application.WebApplication.EmailSending event data.</param>
        /// <returns>true if the message was sent successfully; otherwise, false.</returns>
        public static bool SendEmail(string sender, string from, string to, string bcc, string subject, string body, bool isBodyHtml, EmailSendingEventArgs args)
        {
            bool sent = false;

            MailMessage msg = new MailMessage();
            if (!string.IsNullOrEmpty(sender))
                msg.Sender = new MailAddress(sender);
            if (!string.IsNullOrEmpty(from))
            {
                if (string.Compare(from, to, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (string.Compare(from, sender, StringComparison.OrdinalIgnoreCase) != 0)
                        msg.From = new MailAddress(from);
                }
            }
            else
            {
                msg.From = msg.Sender;
            }

            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = isBodyHtml;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            if (!string.IsNullOrEmpty(bcc))
                msg.Bcc.Add(bcc);

            if (args != null)
            {
                args.MailMessage = msg;

                WebApplication.RaiseEmailSending(args);

                if (!args.Cancel)
                {
                    using (SmtpClient client = new SmtpClient())
                    {
                        if (args.Async)
                            client.SendAsync(args.MailMessage, null);
                        else
                            client.Send(args.MailMessage);
                    }
                    sent = true;
                }
            }

            return sent;
        }

        /// <summary>
        /// Validates the specified e-mail address and generates an exception, if it is invalid.
        /// </summary>
        /// <param name="email">The e-mail address to validate.</param>
        public static void ValidateEmail(string email)
        {
            ValidateEmail(email, true);
        }

        /// <summary>
        /// Validates the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to validate.</param>
        /// <param name="throwOnError">true to throw an exception if an error occured; false to return false.</param>
        /// <returns>true if the e-mail address is valid; otherwise, false.</returns>
        public static bool ValidateEmail(string email, bool throwOnError)
        {
            bool isMatch = Regex.IsMatch(email, EmailRegularExpression, RegexOptions.IgnoreCase);
            if (!isMatch && throwOnError)
                throw new ArgumentException(Resources.Support_InvalidEmail);
            return isMatch;
        }

        /// <summary>
        /// Validates the specified URL and generates an exception, if it is invalid.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        public static void ValidateUrl(string url)
        {
            ValidateUrl(url, true);
        }

        /// <summary>
        /// Validates the specified URL.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        /// <param name="throwOnError">true to throw an exception if an error occured; false to return false.</param>
        /// <returns>true if URL is valid; otherwise, false.</returns>
        public static bool ValidateUrl(string url, bool throwOnError)
        {
            bool isMatch = Regex.IsMatch(url, UrlRegularExpression, RegexOptions.IgnoreCase);
            if (!isMatch && throwOnError)
                throw new ArgumentException(Resources.Support_InvalidUrl);
            return isMatch;
        }

        /// <summary>
        /// Returns DataView with specified number of rows
        /// </summary>
        /// <param name="dataView">Data View</param>
        /// <param name="limit">Number of rows</param>
        /// <returns></returns>
        public static DataView TrimDataView(DataView dataView, int numberOfRows)
        {
            if (dataView != null)
            {
                DataTable dt = dataView.Table.Clone();
                for (int i = 0; i < numberOfRows; i++)
                {
                    if (i >= dataView.Count)
                        break;
                    dt.ImportRow(dataView[i].Row);
                }

                return new DataView(dt, dataView.RowFilter, dataView.Sort, dataView.RowStateFilter);
            }

            return null;
        }

        #endregion
    }
}
