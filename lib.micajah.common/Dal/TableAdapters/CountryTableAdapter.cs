using System.Data;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Country table2.
    /// </summary>
    internal class CountryTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CountryTableAdapter class.
        /// </summary>
        public CountryTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Country;
            TableMapping.ColumnMappings.Add("CountryId", "CountryId");
            TableMapping.ColumnMappings.Add("Name", "Name");

            #endregion

            #region InsertCommand

            InsertCommand.CommandText = "dbo.Mc_InsertCountry";
            InsertCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, 0, 0, "CountryId", DataRowVersion.Current, false, null, "", "", ""));
            InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "dbo.Mc_GetCountries";
            SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Variant, 0, ParameterDirection.ReturnValue, 0, 0, null, DataRowVersion.Current, false, null, "", "", ""));

            #endregion
        }

        #endregion
    }
}