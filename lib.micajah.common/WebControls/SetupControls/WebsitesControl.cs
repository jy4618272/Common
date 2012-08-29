using System.Text;
using System.Web.UI;
using Micajah.Common.Properties;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage web sites.
    /// </summary>
    public class WebsitesControl : BaseControl
    {
        #region Members

        private TextBox m_UrlTextBox;
        private CustomValidator m_UrlValidator;

        #endregion

        #region Private Properties

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (UrlTextBox != null)
                {
                    sb.Append("function UrlValidation(source, arguments) { arguments.IsValid = true; ");
                    sb.AppendFormat("var Elem = document.getElementById('{0}_txt'); ", UrlTextBox.ClientID);
                    sb.Append("if (Elem) { var ElemValue = Elem.value; if (!StringIsEmpty(ElemValue)) { arguments.IsValid = StringIsUrlList(ElemValue); } } }\r\n");
                }

                return sb.ToString();
            }
        }

        private TextBox UrlTextBox
        {
            get
            {
                if (m_UrlTextBox == null)
                    m_UrlTextBox = (EditForm.FindControl("UrlTextBox") as TextBox);
                return m_UrlTextBox;
            }
        }

        private CustomValidator UrlValidator
        {
            get
            {
                if (m_UrlValidator == null)
                    m_UrlValidator = (EditForm.FindControl("UrlValidator") as CustomValidator);
                return m_UrlValidator;
            }
        }

        #endregion

        #region Protected Properties

        protected static string UrlValidatorErrorMessage
        {
            get { return Resources.TextBox_RegularExpressionValidator_ErrorMessage; }
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();
            List.Columns[1].HeaderText = Resources.WebsitesControl_List_NameColumn_HeaderText;
            EditForm.Fields[1].HeaderText = Resources.WebsitesControl_EditForm_UrlField_HeaderText;
        }

        protected override void EditFormReset()
        {
            base.EditFormReset();
            if (UrlTextBox != null) UrlTextBox.Text = string.Empty;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string scripts = ClientScripts;
            if (!string.IsNullOrEmpty(scripts)) ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "UrlValidationScript", scripts, true);

            base.Render(writer);
        }

        #endregion

        #region Protected Methods

        protected void EditForm_DataBound(object sender, System.EventArgs e)
        {
            if (EditForm.CurrentMode != DetailsViewMode.ReadOnly)
            {
                if (UrlValidator != null && UrlTextBox != null)
                    m_UrlValidator.Attributes["controltovalidate2"] = m_UrlTextBox.ClientID;
            }
        }

        #endregion
    }
}
