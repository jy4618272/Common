using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// Represents a set of command-related properties and methods that are used to fill the data table and 
    /// update a data sourceRow, and is implemented by .NET Framework data providers that access relational databases.
    /// </summary>
    public interface ITableAdapter : IDisposable, ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the table which the adapter relate to.
        /// </summary>
        TableName TableName { get; set; }

        /// <summary>
        /// Gets or sets the connection used by the SQL statements of this adapter.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets an SQL statement used to select records in the data sourceRow.
        /// </summary>
        SqlCommand SelectCommand { get; set; }

        IList<SqlCommand> SelectCommands { get; }

        /// <summary>
        /// Gets or sets an SQL statement used to insert new records into the data sourceRow.
        /// </summary>
        SqlCommand InsertCommand { get; set; }

        /// <summary>
        /// Gets or sets an SQL statement used to update records in the data sourceRow.
        /// </summary>
        SqlCommand UpdateCommand { get; set; }

        /// <summary>
        /// Gets or sets an SQL statement for deleting records from the data sourceRow.
        /// </summary>
        SqlCommand DeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the master mapping between a sourceRow table and a table which the adapter relate to.
        /// </summary>
        DataTableMapping TableMapping { get; set; }

        /// <summary>
        /// Gets or sets the value indicating the System.Data.DataTable should be cleared of all data before the filling operation.
        /// </summary>
        bool ClearBeforeFill { get; set; }

        #endregion

        #region Methods

        int Delete();

        int Delete(params object[] parametersValues);

        /// <summary>
        /// Adds or refreshes rows in a specified range in the specified System.Data.DataTable object.
        /// </summary>
        /// <param name="dataTable">The System.Data.DataTable object to fill in.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the System.Data.DataTable object.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        int Fill(DataTable dataTable);

        int Fill(DataTable dataTable, int selectCommandIndex);

        int Fill(DataTable dataTable, int selectCommandIndex, params object[] parametersValues);

        int Insert();

        int Insert(params object[] parametersValues);

        int Update();

        int Update(params object[] parametersValues);

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified System.Data.DataTable object.
        /// </summary>
        /// <param name="dataTable">The System.Data.DataTable object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        int Update(DataTable dataTable);

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified System.Data.DataSet object.
        /// </summary>
        /// <param name="dataSet">The System.Data.DataSet object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        int Update(DataSet dataSet);

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for specified System.Data.DataRow object.
        /// </summary>
        /// <param name="dataRow">An System.Data.DataRow object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        int Update(DataRow dataRow);

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row 
        /// in the specified array of System.Data.DataRow objects.
        /// </summary>
        /// <param name="dataRows">An array of System.Data.DataRow objects used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        int Update(DataRow[] dataRows);

        #endregion
    }
}
