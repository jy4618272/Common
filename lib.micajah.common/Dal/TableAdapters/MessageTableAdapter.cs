using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Message table.
    /// </summary>
    internal class MessageTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MessageTableAdapter class.
        /// </summary>
        public MessageTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Message;
            TableMapping.ColumnMappings.Add("MessageId", "MessageId");
            TableMapping.ColumnMappings.Add("ParentMessageId", "ParentMessageId");
            TableMapping.ColumnMappings.Add("LocalObjectType", "LocalObjectType");
            TableMapping.ColumnMappings.Add("LocalObjectId", "LocalObjectId");
            TableMapping.ColumnMappings.Add("FromUserId", "FromUserId");
            TableMapping.ColumnMappings.Add("ToUserId", "ToUserId");
            TableMapping.ColumnMappings.Add("Subject", "Subject");
            TableMapping.ColumnMappings.Add("Text", "Text");
            TableMapping.ColumnMappings.Add("CreatedTime", "CreatedTime");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertMessage";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@MessageId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "MessageId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ParentMessageId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ParentMessageId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalObjectType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalObjectType", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@LocalObjectId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalObjectId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@FromUserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "FromUserId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@ToUserId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "ToUserId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Subject", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Text", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Text", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, 23, 3, "CreatedTime", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetMessages";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@LocalObjectType", SqlDbType.NVarChar, 50, ParameterDirection.Input, 0, 0, "LocalObjectType", DataRowVersion.Current, false, null, "", "", ""));
            SelectCommand.Parameters.Add(new SqlParameter("@LocalObjectId", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "LocalObjectId", DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}
