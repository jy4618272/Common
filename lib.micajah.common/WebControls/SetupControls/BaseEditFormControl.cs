using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.SetupControls
{
    public class BaseEditFormControl : UserControl
    {
        #region Members

        protected MagicForm EditForm;
        protected ObjectDataSource EntityDataSource;

        private HtmlGenericControl m_ErrorDiv;

        #endregion

        #region Protected Properties

        protected HtmlGenericControl ErrorDiv
        {
            get
            {
                if (m_ErrorDiv == null) m_ErrorDiv = EditForm.FindControl("ErrorDiv") as HtmlGenericControl;
                return m_ErrorDiv;
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

        protected virtual void EditFormInitialize()
        {
            Initialize(EditForm);

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
            if (BaseControl.ShowError(e.Exception, ErrorDiv))
                e.ExceptionHandled = true;
        }

        protected virtual void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
        }

        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
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
