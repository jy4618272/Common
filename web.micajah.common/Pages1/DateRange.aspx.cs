using System;
using System.Web.UI;
using System.Globalization;

public partial class DateRangeTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = DateRange1.DateStart.ToString(CultureInfo.CurrentCulture);
        Label2.Text = DateRange1.DateEnd.ToString(CultureInfo.CurrentCulture);
    }
}
