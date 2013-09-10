using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_OAuthConsumer table.
    /// </summary>
    internal class OAuthConsumerTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OAuthConsumerTableAdapter class.
        /// </summary>
        public OAuthConsumerTableAdapter()
        {
            #region TableMapping

            TableName = TableName.OAuthConsumer;
            TableMapping.ColumnMappings.Add("ConsumerId", "ConsumerId");
            TableMapping.ColumnMappings.Add("ConsumerKey", "ConsumerKey");
            TableMapping.ColumnMappings.Add("ConsumerSecret", "ConsumerSecret");
            TableMapping.ColumnMappings.Add("Callback", "Callback");
            TableMapping.ColumnMappings.Add("VerificationCodeFormat", "VerificationCodeFormat");
            TableMapping.ColumnMappings.Add("VerificationCodeLength", "VerificationCodeLength");

            #endregion

            #region SelectCommands

            SelectCommand.CommandText = "dbo.Mc_GetOAuthConsumer";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@ConsumerId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ConsumerId", DataRowVersion.Current, false, null, "", "", ""));

            using (SqlCommand command = new SqlCommand("dbo.Mc_GetOAuthConsumerByConsumerKey"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.Mc_GetOAuthConsumerByConsumerKey";
                command.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
                command.Parameters.Add(new SqlParameter("@ConsumerKey", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "ConsumerKey", DataRowVersion.Current, false, null, "", "", ""));
                SelectCommands.Add(command);
            }

            #endregion
        }

        #endregion
    }
}