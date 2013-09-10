using System;
using System.Web;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll.Handlers
{
    /// <summary>
    /// Outputs the embedded resources or binary resources from Mc_Rsource table to the outgoing HTTP content body.
    /// </summary>
    public sealed class ResourceHandler : IHttpHandler
    {
        #region Public Properties

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        #region Public Methods

        public void ProcessRequest(HttpContext context)
        {
            if (context == null) return;

            byte[] content = null;
            if (ResourceProvider.IsResourceUrl(context.Request.FilePath) && (context.Request.QueryString["d"] != null))
            {
                string contentType = null;
                string name = null;
                bool cacheable = true;
                ResourceProvider.GetResource(context.Request.QueryString["d"], ref content, ref contentType, ref name, ref cacheable);

                if (content != null)
                {
                    context.Response.Clear();
                    if (!string.IsNullOrEmpty(contentType)) context.Response.ContentType = contentType;
                    if (!string.IsNullOrEmpty(name))
                    {
                        string contentDisposition = string.Empty;
                        if (context.Request.Browser.IsBrowser("IE"))
                            contentDisposition = "filename=\"" + Support.ToHexString(name) + "\";";
                        else
                            contentDisposition = "filename*=utf-8''" + HttpUtility.UrlPathEncode(name) + ";";
                        context.Response.AddHeader("Content-Disposition", contentDisposition);
                    }

                    if (content.Length > 0)
                    {
                        if (cacheable)
                        {
                            context.Response.Cache.SetExpires(DateTime.UtcNow.AddMonths(1));
                            context.Response.Cache.SetCacheability(HttpCacheability.Public);
                        }
                        context.Response.OutputStream.Write(content, 0, content.Length);
                    }
                }
            }

            if (content == null)
                throw new HttpException(404, Resources.Error_404);
        }

        #endregion
    }
}
