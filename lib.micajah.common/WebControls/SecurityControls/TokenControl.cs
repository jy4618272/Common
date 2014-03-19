using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class TokenControl : UserControl
    {
        #region Members

        protected System.Web.UI.WebControls.TextBox TokenTextBox;
        protected LinkButton ResetTokenButton;

        #endregion

        #region Public Properties

        public Guid LoginId
        {
            get
            {
                object obj = ViewState["LoginId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set
            {
                ViewState["LoginId"] = value;

                TokenTextBox.Text = LoginProvider.Current.GetToken(value);
            }
        }

        public int Columns
        {
            get { return TokenTextBox.Columns; }
            set { TokenTextBox.Columns = value; }
        }

        public Unit Width
        {
            get { return TokenTextBox.Width; }
            set { TokenTextBox.Width = value; }
        }

        public string Token
        {
            get { return TokenTextBox.Text; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ResetTokenButton.Text = Resources.TokenControl_ResetTokenButton_Text;
            ResetTokenButton.OnClientClick = string.Concat("return confirm('", Resources.TokenControl_ResetTokenButton_ConfirmText, "');");
        }

        protected void ResetTokenButton_Click(object sender, EventArgs e)
        {
            TokenTextBox.Text = LoginProvider.Current.ResetToken(this.LoginId);
        }

        #endregion
    }
}
