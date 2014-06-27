using System;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using Micajah.Common.Bll;
using System.Collections;

public partial class _Default : System.Web.UI.Page
{
    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        // Generates new RSA private + public keys to use in mc.config.
        //using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
        //{
        //    Response.Write(Server.HtmlEncode(provider.ToXmlString(true)));
        //    Response.Write("\r\n\r\n");
        //    Response.Write(Server.HtmlEncode(provider.ToXmlString(false)));
        //}

        //DateTime date = new DateTime(2012, 9, 26, 17, 56, 23);

        //Response.Write(Support.ToShortDateString(date) + "<br />");
        //Response.Write(Support.ToShortTimeString(date) + "<br />");
        //Response.Write(Support.ToLongDateTimeString(date) + "<br />");

        //Response.Write(Support.ToShortTimeString(date, 1) + "<br />");
        //Response.Write(Support.ToLongDateTimeString(date, 1) + "<br />");

        ArrayList list = new ArrayList();
        list.Add("1");
        list.Add("1");
        list.Add("1");
        list.Add("1");
        list.Add("2");
        list.Add("3");
        list.Add("2");
        list.Add("3");
        list.Add("4");

        list = Support.RemoveDuplicates(list);

        byte[] b12 = Support.GetBytes("lorem ipsum lorem ipsum lorem ipsum lorem ipsum");

        int b = 1;
    }

    #endregion
}
