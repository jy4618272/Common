using System;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

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
    }

    #endregion
}
