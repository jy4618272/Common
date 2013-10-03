using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// Provides user interface (UI) elements for the managing and shows the information of the Micajah.Common framework.
    /// </summary>
    public class FrameworkControl : MasterControl
    {
        #region Members

        protected Label TitleLabel3;
        protected Label DbInfoLabel;
        protected PlaceHolder DbNeedUpgradePanel;
        protected Label DbNeedUpgradeLabel;
        protected Label DbNeedUpgradeNotesLabel;
        protected LinkButton UpgradeLink;
        protected CommonGridView AssemblyList;

        private bool m_IsFrameworkAdministrator;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            TitleLabel3.Text = Resources.FrameworkControl_TitleLabel3_Text;

            BaseControl.LoadResources(AssemblyList, this.GetType().BaseType.Name);
        }

        private void CheckDbVersion()
        {
            int version = VersionProvider.GetVersion();

            DbNeedUpgradePanel.Visible = false;
            if (version > 0)
            {
                DbInfoLabel.Text = string.Format(CultureInfo.CurrentCulture, Resources.FrameworkControl_DbInfoLabel_Text, version);

                if (version != WebApplicationElement.RequiredDatabaseVersion)
                {
                    DbNeedUpgradePanel.Visible = true;
                    DbNeedUpgradeLabel.Text = string.Format(CultureInfo.CurrentCulture, Resources.FrameworkControl_DbNeedUpgradeLabel_Text, WebApplicationElement.RequiredDatabaseVersion);

                    if (m_IsFrameworkAdministrator || (string.Compare(this.Request.QueryString["dbupdate"], bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        DbNeedUpgradeNotesLabel.Text = Resources.FrameworkControl_DbNeedUpgradeNotesLabel_Text;

                        UpgradeLink.Text = Resources.FrameworkControl_UpgradeLink_Text;
                        UpgradeLink.Attributes["onclick"] = string.Format(CultureInfo.CurrentCulture, "return confirm('{0}');", Resources.FrameworkControl_UpgradeLink_ConfirmText.Replace("'", "\\'"));
                        UpgradeLink.Visible = true;
                    }
                }
            }
            else
            {
                DbInfoLabel.Text = Resources.FrameworkControl_ErrorMesage_InvalidDatabaseVersion;
            }
        }

        private void AssemblyListDataBind()
        {
            if (AssemblyList.Rows.Count == 0)
            {
                AssemblyName[] names = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
                int length = names.Length;
                Array.Resize(ref names, length + 1);
                names[length] = Assembly.GetExecutingAssembly().GetName();

                using (DataTable table = new DataTable())
                {
                    table.Locale = CultureInfo.CurrentCulture;
                    table.Columns.Add("Name", typeof(string));
                    table.Columns.Add("Version", typeof(string));
                    table.Columns.Add("Culture", typeof(string));
                    table.Columns.Add("PublicKeyToken", typeof(string));

                    DataRow row = null;
                    foreach (AssemblyName name in names)
                    {
                        row = table.NewRow();
                        row["Name"] = name.Name;
                        row["Version"] = name.Version;
                        row["Culture"] = name.CultureInfo.DisplayName;
                        foreach (byte b in name.GetPublicKeyToken())
                        {
                            row["PublicKeyToken"] += string.Format(CultureInfo.CurrentCulture, "{0:x}", b);
                        }

                        table.Rows.Add(row);
                    }

                    using (DataView dv = new DataView(table))
                    {
                        dv.Sort = "Name";

                        AssemblyList.DataSource = dv;
                        AssemblyList.DataBind();
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            LoadResources();
            AssemblyListDataBind();

            UserContext user = UserContext.Current;
            if (user != null) m_IsFrameworkAdministrator = user.IsFrameworkAdministrator;

            if (!m_IsFrameworkAdministrator)
            {
                this.MasterPage.VisibleBreadcrumbs = this.MasterPage.VisibleHeader = this.MasterPage.VisibleFooter
                    = this.MasterPage.VisibleLeftArea = this.MasterPage.VisibleMainMenu = this.MasterPage.VisibleHelpLink
                    = this.MasterPage.VisibleApplicationLogo
                    = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            CheckDbVersion();
        }

        protected void UpgradeLink_Click(object sender, EventArgs e)
        {
            string sqlScript = null;
            string preUpgradeSqlScript = ResourceProvider.GetSqlScript("pre", "Common");
            string postUpgradeSqlScript = ResourceProvider.GetSqlScript("post", "Common");
            string versionUpgradeSqlScriptOriginal = ResourceProvider.GetSqlScript("v", "Common");

            using (SqlConnection masterConnection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString))
            {
                // Upgrades the databases.
                for (int v = (VersionProvider.GetVersion() + 1); v <= WebApplicationElement.RequiredDatabaseVersion; v++)
                {
                    string versionUpgradeSqlScript = versionUpgradeSqlScriptOriginal.Replace("$Input$", v.ToString(CultureInfo.InvariantCulture));
                    string outputSqlScript = string.Empty;

                    sqlScript = ResourceProvider.GetSqlScript(v, "Master");
                    if (!string.IsNullOrEmpty(sqlScript))
                    {
                        Support.ExecuteNonQuery(preUpgradeSqlScript, masterConnection);
                        object obj = Support.ExecuteScalar(sqlScript, masterConnection);
                        if (!Support.IsNullOrDBNull(obj)) outputSqlScript = obj.ToString();
                        Support.ExecuteNonQuery(postUpgradeSqlScript, masterConnection);
                    }
                    Support.ExecuteNonQuery(versionUpgradeSqlScript, masterConnection);

                    sqlScript = ResourceProvider.GetSqlScript(v, "Client");
                    if (!string.IsNullOrEmpty(sqlScript))
                        sqlScript = sqlScript.Replace("$Input$", outputSqlScript);

                    // Gets the unique connection strings to the databases.
                    ArrayList connectionStrings = new ArrayList();
                    foreach (Organization organization in OrganizationProvider.CreateOrganizationCollection(OrganizationProvider.GetOrganizations()))
                    {
                        // TODO: Should we throw exception there?
                        string connStr = OrganizationProvider.GetConnectionString(organization.OrganizationId);
                        if (!(string.IsNullOrEmpty(connStr) || connectionStrings.Contains(connStr)))
                            connectionStrings.Add(connStr);
                    }

                    foreach (string connStr in connectionStrings)
                    {
                        using (SqlConnection clientConnection = new SqlConnection(connStr))
                        {
                            if (!string.IsNullOrEmpty(sqlScript))
                            {
                                Support.ExecuteNonQuery(preUpgradeSqlScript, clientConnection);
                                Support.ExecuteNonQuery(sqlScript, clientConnection);
                                Support.ExecuteNonQuery(postUpgradeSqlScript, clientConnection);
                            }
                            Support.ExecuteNonQuery(versionUpgradeSqlScript, clientConnection);
                        }
                    }

                    if (v == 77 || v == 78)
                        OrganizationProvider.UpdateOrganizationsPseudoId();
                }
            }
        }

        #endregion
    }
}
