using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.WebControls;

public partial class Controls_EmbeddedIcons : UserControl
{
    #region Members

    private object m_IconSize;

    #endregion

    #region Public Properties

    public IconSize IconSize
    {
        get
        {
            if (m_IconSize == null)
            {
                object obj = Support.ConvertStringToType(this.Request.QueryString["IconSize"], typeof(IconSize));
                m_IconSize = ((obj == null) ? (object)IconSize.NotSet : obj);
            }
            return (IconSize)m_IconSize;
        }
        set { m_IconSize = value; }
    }

    #endregion

    #region Protected Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            IconsList.DataSource = ResourceProvider.GetIconImageFileNameList(this.IconSize);
            IconsList.DataBind();
        }
    }

    protected void IconsList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Item:
            case ListItemType.AlternatingItem:
                Image img = e.Item.FindControl("Icon") as Image;
                if (img != null)
                {
                    string iconFileName = (string)e.Item.DataItem;
                    img.ImageUrl = ResourceProvider.GetIconImageUrl(iconFileName, this.IconSize, true);
                    img.ToolTip = iconFileName;
                }
                break;
        }
    }

    #endregion
}