using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Organization table.
    /// </summary>
    internal class OrganizationTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OrganizationTableAdapter class.
        /// </summary>
        public OrganizationTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Organization;
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("PseudoId", "PseudoId");
            TableMapping.ColumnMappings.Add("Name", "Name");
            TableMapping.ColumnMappings.Add("Description", "Description");
            TableMapping.ColumnMappings.Add("WebsiteUrl", "WebsiteUrl");
            TableMapping.ColumnMappings.Add("DatabaseId", "DatabaseId");
            TableMapping.ColumnMappings.Add("FiscalYearStartMonth", "FiscalYearStartMonth");
            TableMapping.ColumnMappings.Add("FiscalYearStartDay", "FiscalYearStartDay");
            TableMapping.ColumnMappings.Add("WeekStartsDay", "WeekStartsDay");
            TableMapping.ColumnMappings.Add("LdapServerAddress", "LdapServerAddress");
            TableMapping.ColumnMappings.Add("LdapServerPort", "LdapServerPort");
            TableMapping.ColumnMappings.Add("LdapDomain", "LdapDomain");
            TableMapping.ColumnMappings.Add("LdapUserName", "LdapUserName");
            TableMapping.ColumnMappings.Add("LdapPassword", "LdapPassword");
            TableMapping.ColumnMappings.Add("LdapDomains", "LdapDomains");
            TableMapping.ColumnMappings.Add("ExpirationTime", "ExpirationTime");
            TableMapping.ColumnMappings.Add("GraceDays", "GraceDays");
            TableMapping.ColumnMappings.Add("Active", "Active");
            TableMapping.ColumnMappings.Add("Deleted", "Deleted");
            TableMapping.ColumnMappings.Add("ExternalId", "ExternalId");
            TableMapping.ColumnMappings.Add("CanceledTime", "CanceledTime");
            TableMapping.ColumnMappings.Add("Trial", "Trial");
            TableMapping.ColumnMappings.Add("Beta", "Beta");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");
            TableMapping.ColumnMappings.Add("Street", "Street");
            TableMapping.ColumnMappings.Add("Street2", "Street2");
            TableMapping.ColumnMappings.Add("City", "City");
            TableMapping.ColumnMappings.Add("State", "State");
            TableMapping.ColumnMappings.Add("PostalCode", "PostalCode");
            TableMapping.ColumnMappings.Add("Country", "Country");
            TableMapping.ColumnMappings.Add("Currency", "Currency");
            TableMapping.ColumnMappings.Add("HowYouHearAboutUs", "HowYouHearAboutUs");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertOrganization";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PseudoId", SqlDbType.VarChar, 6, ParameterDirection.Input, 0, 0, "PseudoId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@WebSiteUrl", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "WebSiteUrl", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@DatabaseId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "DatabaseId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FiscalYearStartMonth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "FiscalYearStartMonth", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FiscalYearStartDay", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "FiscalYearStartDay", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@WeekStartsDay", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "WeekStartsDay", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ExpirationTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "ExpirationTime", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@GraceDays", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "GraceDays", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CanceledTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CanceledTime", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Trial", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Trial", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Street2", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street2", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "City", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "State", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "PostalCode", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Country", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Currency", SqlDbType.Char, 3, ParameterDirection.Input, 0, 0, "Currency", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@HowYouHearAboutUs", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "HowYouHearAboutUs", DataRowVersion.Current, false, null, "", "", ""));


            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateOrganization";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PseudoId", SqlDbType.VarChar, 6, ParameterDirection.Input, 0, 0, "PseudoId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@WebsiteUrl", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "WebSiteUrl", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@DatabaseId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "DatabaseId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FiscalYearStartMonth", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "FiscalYearStartMonth", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@FiscalYearStartDay", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "FiscalYearStartDay", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@WeekStartsDay", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "WeekStartsDay", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapServerAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapServerAddress", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapServerPort", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LdapServerPort", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapDomain", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapDomain", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapUserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapUserName", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapPassword", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LdapPassword", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LdapDomains", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "LdapDomains", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ExpirationTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "ExpirationTime", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@GraceDays", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "GraceDays", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Active", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CanceledTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CanceledTime", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Trial", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Trial", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Beta", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Beta", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Deleted", SqlDbType.Bit, 1, ParameterDirection.Input, 1, 0, "Deleted", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Street2", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Street2", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "City", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "State", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Input, 0, 0, "PostalCode", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Country", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Currency", SqlDbType.Char, 3, ParameterDirection.Input, 0, 0, "Currency", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@HowYouHearAboutUs", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "HowYouHearAboutUs", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetOrganizations";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}