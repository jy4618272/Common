using System;
using System.Globalization;
using System.Web.UI;
using Micajah.Common.Bll;
using Micajah.Common.Security;

public partial class DateRangeTestPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateRange1.DateFormat = Support.GetShortDateFormat(UserContext.Current.DateFormat);
        Label1.Text = DateRange1.DateStart.ToString(CultureInfo.CurrentCulture);
        Label2.Text = DateRange1.DateEnd.ToString(CultureInfo.CurrentCulture);
    }
}
