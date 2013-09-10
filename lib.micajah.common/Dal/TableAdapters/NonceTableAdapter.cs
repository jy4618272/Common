using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Nonce table.
    /// </summary>
    internal class NonceTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NonceTableAdapter class.
        /// </summary>
        public NonceTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Nonce;
            TableMapping.ColumnMappings.Add("Context", "Context");
            TableMapping.ColumnMappings.Add("Code", "Code");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertNonce";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Context", SqlDbType.NVarChar, 100, ParameterDirection.Input, 0, 0, "Context", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "Code", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}