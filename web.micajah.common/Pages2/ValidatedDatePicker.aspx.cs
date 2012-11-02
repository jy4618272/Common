using System;
using System.Globalization;
using System.Web.UI;
using Micajah.Common.Bll;
using Micajah.Common.Security;

public partial class DatePickerTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidatedDatePicker1.DateFormat = Support.GetLongDateTimeFormat(UserContext.Current.TimeFormat, UserContext.Current.DateFormat);
        Label1.Text = Support.ToLongDateTimeString(ValidatedDatePicker1.SelectedDate, UserContext.Current.TimeFormat);
    }
}
