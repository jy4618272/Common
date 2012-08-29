using System;
using System.Web.UI;
using System.Globalization;

public partial class DatePickerTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = ValidatedDatePicker1.SelectedDate.ToString(CultureInfo.CurrentCulture);
    }
}
