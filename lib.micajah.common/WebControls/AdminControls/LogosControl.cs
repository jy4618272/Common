using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage logos.
    /// </summary>
    public class LogosControl : BaseControl
    {
        #region Members

        protected UpdatePanel UpdatePanelLogos;

        #endregion

        #region Private Properties

        private Guid SelectedObjectId
        {
            get { return ((List.SelectedIndex > -1) ? new Guid(List.SelectedDataKey["ObjectId"].ToString()) : Guid.Empty); }
        }

        private string SelectedObjectType
        {
            get { return ((List.SelectedIndex > -1) ? List.SelectedDataKey["ObjectType"].ToString() : string.Empty); }
        }

        #endregion

        #region Private Methods

        private void BindData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ObjectId");
            dt.Columns.Add("Name");
            dt.Columns.Add("Type");
            dt.Columns.Add("Logo");
            dt.Columns.Add("ObjectType");

            UserContext user = UserContext.Current;
            Organization org = Micajah.Common.Bll.Providers.OrganizationProvider.GetOrganization(user.SelectedOrganizationId);
            InstanceCollection instances = Micajah.Common.Bll.Providers.InstanceProvider.GetInstances(user.SelectedOrganizationId, false);

            if (instances.Count == 1)
            {
                DataRow dr = dt.NewRow();
                dr["ObjectId"] = instances[0].InstanceId;
                dr["Name"] = instances[0].Name;
                dr["Type"] = Resources.LogosControl_List_InstanceType_Text;
                dr["Logo"] = ResourceProvider.GetInstanceLogoImageUrl(instances[0].InstanceId);
                dr["ObjectType"] = ResourceProvider.InstanceLogoLocalObjectType;
                dt.Rows.Add(dr);
            }
            else
            {
                if (user.IsOrganizationAdministrator)
                {
                    DataRow dr = dt.NewRow();
                    dr["ObjectId"] = org.OrganizationId;
                    dr["Name"] = org.Name;
                    dr["Type"] = Resources.LogosControl_List_OrganisationType_Text;
                    dr["Logo"] = ResourceProvider.GetOrganizationLogoImageUrl(org.OrganizationId);
                    dr["ObjectType"] = ResourceProvider.OrganizationLogoLocalObjectType;
                    dt.Rows.Add(dr);
                }

                foreach (Instance instance in instances)
                {
                    DataRow dr = dt.NewRow();
                    dr["ObjectId"] = instance.InstanceId;
                    dr["Name"] = instance.Name;
                    dr["Type"] = Resources.LogosControl_List_InstanceType_Text;
                    dr["Logo"] = ResourceProvider.GetInstanceLogoImageUrl(instance.InstanceId);
                    dr["ObjectType"] = ResourceProvider.InstanceLogoLocalObjectType;
                    dt.Rows.Add(dr);
                }

                List.EditIndex = -1;
            }
            List.DataSource = dt;
            List.DataBind();

            if (instances.Count == 1)
            {
                List.EditIndex = 0;
                List.SelectedIndex = 0;
                List.Visible = false;
                EditForm.Visible = true;
                EditForm.ChangeMode(DetailsViewMode.Edit);
                EditForm.ShowCloseButton = CloseButtonVisibilityMode.None;
                BindEditForm();
            }
        }

        private void BackToList()
        {
            List.SelectedIndex = -1;
            List.Visible = true;
            EditForm.Visible = false;
            BindData();
        }

        private void BindEditForm()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ObjectId");
            dt.Columns.Add("ObjectType");

            DataRow dr = dt.NewRow();
            dr["ObjectId"] = string.Format(CultureInfo.InvariantCulture, "{0:N}", this.SelectedObjectId);
            dr["ObjectType"] = this.SelectedObjectType;
            dt.Rows.Add(dr);

            EditForm.DataSource = dt;
            EditForm.DataBind();
        }

        #endregion

        #region Protected Methods

        protected void EditForm_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
        }

        protected void EditForm_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
        }

        protected void EditForm_DataBinding(object sender, EventArgs e)
        {
            if (List.Rows.Count == 1)
                EditForm.ObjectName = Resources.LogosControl_EditForm_ObjectName;
            else if (this.SelectedObjectType == ResourceProvider.OrganizationLogoLocalObjectType)
                EditForm.ObjectName = Resources.LogosControl_EditForm_ObjectName_Organzation;
            else if (this.SelectedObjectType == ResourceProvider.InstanceLogoLocalObjectType)
                EditForm.ObjectName = Resources.LogosControl_EditForm_ObjectName_Instance;
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            List.Columns[2].HeaderText = Resources.LogosControl_List_LogoColumn_HeaderText;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Initialize(List);

            List.Visible = true;
            List.ShowAddLink = false;
            List.AutoGenerateDeleteButton = false;
            List.AllowSorting = false;

            EditForm.DefaultMode = DetailsViewMode.Edit;
            EditForm.Visible = false;
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e.Action == CommandActions.Edit)
            {
                List.SelectedIndex = e.RowIndex;
                List.Visible = false;
                EditForm.Visible = true;
                EditForm.ChangeMode(DetailsViewMode.Edit);
                BindEditForm();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
                BindData();
        }

        protected override void EditFormInitialize()
        {
            base.EditFormInitialize();
            EditForm.ShowCloseButton = CloseButtonVisibilityMode.Always;
        }

        protected override void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                ImageUpload logoImageUpload = (ImageUpload)EditForm.FindControl("LogoImageUpload");
                if (logoImageUpload != null)
                {
                    if (e.CommandName == "Update")
                    {
                        logoImageUpload.AcceptChanges();
                        this.BackToList();

                        if (List.Rows.Count == 1)
                        {
                            if (this.MasterPage != null)
                            {
                                this.MasterPage.MessageType = NoticeMessageType.Success;
                                this.MasterPage.Message = Resources.BaseEditFormControl_SuccessMessage;
                            }
                        }
                    }

                    if ((e != null) && (e.CommandName == "Cancel"))
                    {
                        logoImageUpload.RejectChanges();
                        this.BackToList();
                    }

                    if (this.SelectedObjectType == ResourceProvider.OrganizationLogoLocalObjectType)
                        ResourceProvider.RemoveOrganizationLogoImageUrlFromCache(this.SelectedObjectId);
                    else if (this.SelectedObjectType == ResourceProvider.InstanceLogoLocalObjectType)
                        ResourceProvider.RemoveInstanceLogoImageUrlFromCache(this.SelectedObjectId);
                }
            }
        }

        #endregion
    }
}
