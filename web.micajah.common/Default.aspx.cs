using System;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using Micajah.Common.Bll;

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
    }

    #endregion
}
