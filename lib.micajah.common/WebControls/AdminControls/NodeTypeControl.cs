using System;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class NodeTypeControl : BaseControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected ObjectDataSource InstancesDataSource;

        private Guid? m_EntityId;
        private Entity m_Entity;
        private EntityNodeTypeCollection m_EntityCustomNodeTypes;

        #endregion

        #region Private Properties

        private bool EnableSwitchVisibility
        {
            get
            {
                object obj = this.ViewState["EnableSwitchVisibility"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { this.ViewState["EnableSwitchVisibility"] = value; }
        }

        private Guid EntityId
        {
            get
            {
                if (!m_EntityId.HasValue)
                {
                    object obj = Support.ConvertStringToType(this.Request.QueryString["entityid"], typeof(Guid));
                    m_EntityId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_EntityId.Value;
            }
        }

        private Entity Entity
        {
            get
            {
                if (m_Entity == null) m_Entity = WebApplication.Entities[this.EntityId.ToString()];
                return m_Entity;
            }
        }

        private EntityNodeTypeCollection EntityCustomNodeTypes
        {
            get
            {
                if (m_EntityCustomNodeTypes == null)
                {
                    if (this.Entity != null)
                    {
                        Guid? instanceId = null;
                        if (this.Entity.HierarchyStartLevel == EntityLevel.Instance)
                            instanceId = this.SelectedInstanceId;
                        m_EntityCustomNodeTypes = this.Entity.GetCustomNodeTypes(UserContext.SelectedOrganizationId, this.SelectedInstanceId);
                    }
                }
                return m_EntityCustomNodeTypes;
            }
        }

        private Guid? SelectedInstanceId
        {
            get
            {
                Guid? instanceId = new Guid?();
                if (InstanceList.Items.Count == 0)
                {
                    this.ApplyFilters();
                    InstanceList.DataBind();
                }
                object obj = Support.ConvertStringToType(InstanceList.SelectedValue, typeof(Guid));
                if (obj != null)
                    instanceId = (Guid)obj;
                return instanceId;
            }
        }

        #endregion

        #region Private Methods

        private void ApplyFilters()
        {
            InstancesDataSource.FilterExpression = (SearchPanel.Visible ? InstanceProvider.InstancesFilterExpression : "InstanceId IS NULL");
        }

        private void FillInputParameters(ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
            if (Entity.HierarchyStartLevel == EntityLevel.Instance)
                e.InputParameters["instanceId"] = this.SelectedInstanceId;
            e.InputParameters["entityId"] = EntityId;
        }

        #endregion

        #region Protected Methods

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            FillInputParameters(e);
        }

        protected void EntityListDataSource_Deleted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            this.Entity.Refresh();
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            FillInputParameters(e);
        }

        protected void EntityDataSource_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void InstancesDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.Items.Count == 1)
                    SearchPanel.Visible = comboBox.Visible = false;
            }
        }

        protected void InstanceList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.EditFormReset();
            List.DataBind();
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                if (this.Entity != null)
                    this.MasterPage.CustomName = Entity.Name + " " + this.MasterPage.ActiveAction.Name;

                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    if (this.Entity != null)
                        SearchPanel.Visible = (this.Entity.HierarchyStartLevel == EntityLevel.Instance);
                }
                else
                    SearchPanel.Visible = false;

                this.ApplyFilters();
                this.EnableSwitchVisibility = SearchPanel.Visible;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (List.Visible)
            {
                for (int x = 0; x < List.Rows.Count; x++)
                {
                    GridViewRow row = List.Rows[x];
                    Guid id = (Guid)List.DataKeys[x][0];
                    if (this.EntityCustomNodeTypes[id.ToString("N")] == null)
                    {
                        row.Cells[0].Controls.Clear();
                        int lastIndex = row.Cells.Count - 1;
                        row.Cells[lastIndex].Controls.Clear();
                    }
                }
            }
        }

        protected override void EditFormReset()
        {
            base.EditFormReset();
            if (this.EnableSwitchVisibility)
                SearchPanel.Visible = true;
        }

        protected override void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            base.EditForm_ItemInserted(sender, e);

            this.Entity.Refresh();
        }

        protected override void EditForm_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            base.EditForm_ItemUpdated(sender, e);

            this.Entity.Refresh();
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            base.List_Action(sender, e);

            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                case CommandActions.Edit:
                    SearchPanel.Visible = false;
                    break;
            }
        }

        #endregion
    }
}
