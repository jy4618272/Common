using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage the values of the specified entity's field.
    /// </summary>
    public class EntityFieldListValuesControl : BaseControl
    {
        #region Members

        private Guid? m_EntityFieldId;

        #endregion

        #region Private Properties

        private Guid EntityFieldId
        {
            get
            {
                if (!m_EntityFieldId.HasValue)
                {
                    object obj = Support.ConvertStringToType(Request.QueryString["entityfieldid"], typeof(Guid));
                    m_EntityFieldId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_EntityFieldId.Value;
            }
        }

        #endregion

        #region Protected Methods

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["entityFieldId"] = this.EntityFieldId;
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Micajah.Common.Pages.MasterPage.InitializeAdminPage(this.Page);

            if (!this.IsPostBack)
            {
                ClientDataSet.EntityFieldDataTable table = EntityFieldProvider.GetEntityField(this.EntityFieldId);
                if (table.Count > 0)
                {
                    ClientDataSet.EntityFieldRow row = table[0];
                    if (row != null)
                    {
                        switch ((EntityFieldType)row.EntityFieldTypeId)
                        {
                            case EntityFieldType.SingleSelectList:
                            case EntityFieldType.MultipleSelectList:
                                this.MasterPage.CustomName = string.Format(CultureInfo.InvariantCulture, Resources.EntityFieldListValuesControl_CustomNameFormat, row.Name);
                                return;
                        }
                    }
                }

                List.ShowAddLink = false;
            }
        }

        #endregion
    }
}
