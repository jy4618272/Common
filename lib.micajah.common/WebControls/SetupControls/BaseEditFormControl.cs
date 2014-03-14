using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SetupControls
{
    public class BaseEditFormControl : MasterControl
    {
        #region Members

        protected MagicForm EditForm;
        protected ObjectDataSource EntityDataSource;

        private HtmlGenericControl m_ErrorPanel;
        private bool m_IsModernTheme;

        #endregion

        #region Protected Properties

        protected HtmlGenericControl ErrorPanel
        {
            get
            {
                if (m_ErrorPanel == null) m_ErrorPanel = EditForm.FindControl("ErrorPanel") as HtmlGenericControl;
                return m_ErrorPanel;
            }
        }

        #endregion

        #region Internal Methods

        internal static void Initialize(MagicForm form)
        {
            if (form == null) return;

            form.AutoGenerateRows = false;
            form.AutoGenerateInsertButton = true;
            form.AutoGenerateEditButton = true;
            form.DefaultMode = DetailsViewMode.Edit;
        }

        #endregion

        #region Protected Methods

        protected void RedirectToActionOrStartPage(Guid actionId)
        {
            Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId(actionId);
            Response.Redirect((action != null) ? action.CustomAbsoluteNavigateUrl : UserContext.Current.StartPageUrl);
        }

        protected void RedirectToConfigurationPage()
        {
            if (!m_IsModernTheme)
                RedirectToActionOrStartPage(ActionProvider.ConfigurationPageActionId);
        }

        protected virtual void EditFormInitialize()
        {
            Initialize(EditForm);

            if (m_IsModernTheme)
            {
                if (this.MasterPage != null)
                {
                    if (this.MasterPage.IsAdminPage)
                    {
                        EditForm.ShowCloseButton = CloseButtonVisibilityMode.None;
                        EditForm.ShowCancelButton = false;
                    }
                }
            }

            EditForm.ItemCommand += new DetailsViewCommandEventHandler(EditForm_ItemCommand);
            EditForm.ItemUpdated += new DetailsViewUpdatedEventHandler(EditForm_ItemUpdated);
        }

        #endregion

        #region Protected Methods

        protected virtual void LoadResources()
        {
            BaseControl.LoadResources(EditForm, this.GetType().BaseType.Name);
        }

        protected virtual void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (e == null) return;

            e.KeepInEditMode = true;
            if (BaseControl.ShowError(e.Exception, ErrorPanel))
                e.ExceptionHandled = true;
            else if (m_IsModernTheme)
            {
                if (this.MasterPage != null)
                {
                    this.MasterPage.MessageType = NoticeMessageType.Success;
                    this.MasterPage.Message = Resources.BaseEditFormControl_SuccessMessage;
                }
            }
        }

        protected virtual void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
        }

        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            m_IsModernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern);

            this.EditFormInitialize();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
                this.LoadResources();
        }

        #endregion
    }
}
