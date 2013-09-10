using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_OAuthToken table.
    /// </summary>
    internal class OAuthTokenTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OAuthTokenTableAdapter class.
        /// </summary>
        public OAuthTokenTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Nonce;
            TableMapping.ColumnMappings.Add("TokenId", "TokenId");
            TableMapping.ColumnMappings.Add("Token", "Token");
            TableMapping.ColumnMappings.Add("TokenSecret", "TokenSecret");
            TableMapping.ColumnMappings.Add("TokenTypeId", "TokenTypeId");
            TableMapping.ColumnMappings.Add("ConsumerId", "ConsumerId");
            TableMapping.ColumnMappings.Add("ConsumerVersion", "ConsumerVersion");
            TableMapping.ColumnMappings.Add("Scope", "Scope");
            TableMapping.ColumnMappings.Add("LoginId", "LoginId");
            TableMapping.ColumnMappings.Add("RequestTokenVerifier", "RequestTokenVerifier");
            TableMapping.ColumnMappings.Add("RequestTokenCallback", "RequestTokenCallback");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");
            TableMapping.ColumnMappings.Add("PendingUserAuthorizationRequest", "PendingUserAuthorizationRequest");
            TableMapping.ColumnMappings.Add("OrganizationId", "OrganizationId");
            TableMapping.ColumnMappings.Add("InstanceId", "InstanceId");

            #endregion

            #region DeleteCommand

            DeleteCommand.CommandText = "dbo.Mc_DeleteOAuthTokenByToken";
            DeleteCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            DeleteCommand.Parameters.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Token", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertOAuthToken";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "TokenId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Token", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TokenSecret", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "TokenSecret", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@TokenTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "TokenTypeId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ConsumerId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ConsumerId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ConsumerVersion", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "ConsumerVersion", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Scope", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Scope", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RequestTokenVerifier", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "RequestTokenVerifier", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@RequestTokenCallback", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "RequestTokenCallback", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@PendingUserAuthorizationRequest", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "PendingUserAuthorizationRequest", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetOAuthTokenByToken";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Token", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region UpdateCommand

            UpdateCommand.CommandText = "dbo.Mc_UpdateOAuthToken";
            UpdateCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "TokenId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Token", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TokenSecret", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "TokenSecret", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@TokenTypeId", SqlDbType.Int, 4, ParameterDirection.Input, 10, 0, "TokenTypeId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ConsumerId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ConsumerId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@ConsumerVersion", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "ConsumerVersion", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@Scope", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Scope", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@LoginId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "LoginId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RequestTokenVerifier", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "RequestTokenVerifier", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@RequestTokenCallback", SqlDbType.NVarChar, 2048, ParameterDirection.Input, 0, 0, "RequestTokenCallback", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@PendingUserAuthorizationRequest", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "PendingUserAuthorizationRequest", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "OrganizationId", DataRowVersion.Current, false, null, "", "", ""));
            UpdateCommand.Parameters.Add(new SqlParameter("@InstanceId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "InstanceId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}