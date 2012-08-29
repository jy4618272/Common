using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Provides UI elements for Hello note.
    /// </summary>
    [ToolboxData("<{0}:Hello runat=server></{0}:Hello>")]
    public sealed class Hello : Control, INamingContainer
    {
        #region Members

        private HtmlGenericControl m_HelloNote;
        private HyperLink m_ProfileLink;
        private HtmlGenericControl m_RoleLabel;

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            UserContext user = UserContext.Current;

            m_HelloNote = new HtmlGenericControl("div");
            m_HelloNote.ID = "HelloNoteLabel";
            m_HelloNote.Style.Add(HtmlTextWriterStyle.FontSize, "12px");
            m_HelloNote.InnerHtml = Resources.Hello_HelloNoteLabel_Text + "&nbsp;";
            if (user == null)
            {
                m_HelloNote.Style.Add(HtmlTextWriterStyle.PaddingBottom, "7px");
                m_HelloNote.InnerHtml += Resources.Hello_VisitorText;
            }
            else
                m_HelloNote.Style.Add(HtmlTextWriterStyle.Display, "inline");

            this.Controls.Add(m_HelloNote);

            if (user != null)
            {
                m_HelloNote.Style.Add(HtmlTextWriterStyle.Display, "inline");

                m_ProfileLink = new HyperLink();
                m_ProfileLink.ID = "ProfileLink";
                m_ProfileLink.Font.Size = new FontUnit(12, UnitType.Pixel);
                m_ProfileLink.Text = user.FirstName;

                Action action = ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.MyAccountGlobalNavigationLinkActionId);
                if (action != null) m_ProfileLink.NavigateUrl = action.AbsoluteNavigateUrl;

                this.Controls.Add(m_ProfileLink);

                string name = string.Empty;
                if (user.SelectedInstance != null)
                    name = user.SelectedInstance.Name;
                else if (user.SelectedOrganization != null)
                    name = user.SelectedOrganization.Name;

                m_RoleLabel = new HtmlGenericControl("div");
                m_RoleLabel.ID = "RoleLabel";
                m_RoleLabel.Style.Add(HtmlTextWriterStyle.Padding, "7px 0 7px 0");
                m_RoleLabel.Style.Add(HtmlTextWriterStyle.FontSize, "12px");
                m_RoleLabel.InnerHtml = string.Format(CultureInfo.CurrentCulture, Resources.Hello_RoleLabel_Text, name, RoleProvider.GetRoleName(user.RoleId));

                this.Controls.Add(m_RoleLabel);
            }
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write("<span style='font-size: 12px;'>Hello,</span>&nbsp;<a style='font-size: 12px;' href='#'>{First Name}</a><div style='padding: 7px 0 7px 0; font-size: 12px;'>You are logged in to {Organization/Instance Name} as {Role Name}</div>");
            else
                base.Render(writer);
        }

        #endregion
    }
}
