using Micajah.Common.Bll;
using Micajah.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// Aids implementation of the ITableAdapter interface. The base class for tables adapters.
    /// </summary>
    public abstract class BaseTableAdapter : ITableAdapter
    {
        #region Members

        private TableName m_TableName;
        private DataTableMapping m_TableMapping;
        private string m_ConnectionString;
        private bool m_ClearBeforeFill;
        private SqlCommand m_SelectCommand;
        private SqlCommand m_InsertCommand;
        private SqlCommand m_UpdateCommand;
        private SqlCommand m_DeleteCommand;
        private List<SqlCommand> m_SelectCommands;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the table which the adapter relate to.
        /// </summary>
        public TableName TableName
        {
            get { return m_TableName; }
            set
            {
                m_TableName = value;
                this.TableMapping.DataSetTable = this.TableMapping.SourceTable = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the connection used by the SQL statements of this adapter.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(m_ConnectionString))
                    m_ConnectionString = FrameworkConfiguration.Current.WebApplication.ConnectionString;
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
                if (string.IsNullOrEmpty(value))
                    value = FrameworkConfiguration.Current.WebApplication.ConnectionString;
            }
        }

        /// <summary>
        /// Gets or sets an SQL statement used to select records in the data sourceRow.
        /// </summary>
        public SqlCommand SelectCommand
        {
            get
            {
                if (this.SelectCommands.Count == 0)
                {
                    m_SelectCommand = new SqlCommand();
                    m_SelectCommand.CommandType = CommandType.StoredProcedure;
                    this.SelectCommands.Add(m_SelectCommand);
                }
                return this.SelectCommands[0];
            }
            set
            {
                if (this.SelectCommands.Count > 0)
                    this.SelectCommands[0] = value;
                else
                    this.SelectCommands.Add(value);
                m_SelectCommand = value;
            }
        }

        public IList<SqlCommand> SelectCommands
        {
            get
            {
                if (m_SelectCommands == null) m_SelectCommands = new List<SqlCommand>();
                return m_SelectCommands;
            }
        }

        /// <summary>
        /// Gets or sets an SQL statement used to insert new records into the data sourceRow.
        /// </summary>
        public SqlCommand InsertCommand
        {
            get
            {
                if (m_InsertCommand == null)
                {
                    m_InsertCommand = new SqlCommand();
                    m_InsertCommand.CommandType = CommandType.StoredProcedure;
                }
                return m_InsertCommand;
            }
            set { m_InsertCommand = value; }
        }

        /// <summary>
        /// Gets or sets an SQL statement used to update records in the data sourceRow.
        /// </summary>
        public SqlCommand UpdateCommand
        {
            get
            {
                if (m_UpdateCommand == null)
                {
                    m_UpdateCommand = new SqlCommand();
                    m_UpdateCommand.CommandType = CommandType.StoredProcedure;
                }
                return m_UpdateCommand;
            }
            set { m_UpdateCommand = value; }
        }

        /// <summary>
        /// Gets or sets an SQL statement for deleting records from the data sourceRow.
        /// </summary>
        public SqlCommand DeleteCommand
        {
            get
            {
                if (m_DeleteCommand == null)
                {
                    m_DeleteCommand = new SqlCommand();
                    m_DeleteCommand.CommandType = CommandType.StoredProcedure;
                }
                return m_DeleteCommand;
            }
            set { m_DeleteCommand = value; }
        }

        /// <summary>
        /// Gets or sets the master mapping between a sourceRow table and a table which the adapter relate to.
        /// </summary>
        public DataTableMapping TableMapping
        {
            get
            {
                if (m_TableMapping == null) m_TableMapping = new DataTableMapping();
                return m_TableMapping;
            }
            set { m_TableMapping = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the System.Data.DataTable should be cleared of all data before the filling operation.
        /// </summary>
        public bool ClearBeforeFill
        {
            get { return m_ClearBeforeFill; }
            set { m_ClearBeforeFill = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected BaseTableAdapter()
        {
            ClearBeforeFill = true;
        }

        #endregion

        #region Private Methods

        private static void SetParametersValues(SqlCommand command, params object[] parametersValues)
        {
            if ((command != null) && (parametersValues != null))
            {
                int idx = 0;
                foreach (SqlParameter parameter in command.Parameters)
                {
                    if ((parameter.Direction != ParameterDirection.ReturnValue) && (parameter.Direction != ParameterDirection.Output))
                    {
                        object value = parametersValues[idx];
                        parameter.Value = ((value == null) ? (object)DBNull.Value : (object)value);
                        idx++;
                    }
                }
            }
        }

        private int ExecuteNonQuery(SqlCommand original—ommand, params object[] parametersValues)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {

                using (SqlCommand command = original—ommand.Clone())
                {
                    SetParametersValues(command, parametersValues);
                    command.Connection = connection;

                    return Support.ExecuteNonQuery(command);
                }
            }
        }

        private int ExecuteUpdate(object obj)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.TableMappings.Add(this.CloneTableMapping());
                    adapter.DeleteCommand = this.DeleteCommand.Clone();
                    adapter.InsertCommand = this.InsertCommand.Clone();
                    adapter.SelectCommand = this.SelectCommand.Clone();
                    adapter.UpdateCommand = this.UpdateCommand.Clone();
                    adapter.DeleteCommand.Connection = connection;
                    adapter.InsertCommand.Connection = connection;
                    adapter.SelectCommand.Connection = connection;
                    adapter.UpdateCommand.Connection = connection;

                    DataRow[] rows = obj as DataRow[];
                    if (rows != null)
                        return adapter.Update(rows);
                    else
                    {
                        DataTable table = obj as DataTable;
                        if (table != null)
                            return adapter.Update(table);
                    }
                    if (connection.State == ConnectionState.Open) connection.Close();
                    return 0;
                }
            }
        }

        private DataTableMapping CloneTableMapping()
        {
            DataTableMapping m = new DataTableMapping();

            m.DataSetTable = this.TableMapping.DataSetTable;
            m.SourceTable = this.TableMapping.SourceTable;

            foreach (DataColumnMapping cm in this.TableMapping.ColumnMappings)
            {
                m.ColumnMappings.Add(cm.SourceColumn, cm.DataSetColumn);
            }

            return m;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Releases all resources used by this adapter.
        /// </summary>
        /// <param name="disposing">true to releases all resources used by the adapter; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_DeleteCommand != null) m_DeleteCommand.Dispose();
                if (m_InsertCommand != null) m_InsertCommand.Dispose();
                if (m_UpdateCommand != null) m_UpdateCommand.Dispose();
                if (m_SelectCommand != null) m_SelectCommand.Dispose();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Releases all resources used by the adapter.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a new Micajah.Common.Dal.TableAdapters.BaseTableAdapter object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new Micajah.Common.Dal.TableAdapters.BaseTableAdapter object that is a copy of this instance.</returns>
        public BaseTableAdapter Clone()
        {
            BaseTableAdapter adapter = Assembly.GetExecutingAssembly().CreateInstance(this.GetType().FullName) as BaseTableAdapter;
            if (adapter != null)
            {
                adapter.ClearBeforeFill = this.ClearBeforeFill;
                adapter.ConnectionString = this.ConnectionString;
            }
            return adapter;
        }

        public int Delete()
        {
            return this.Delete(null);
        }

        public virtual int Delete(params object[] parametersValues)
        {
            return ExecuteNonQuery(this.DeleteCommand, parametersValues);
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the specified System.Data.DataTable object.
        /// </summary>
        /// <param name="dataTable">The System.Data.DataTable object to fill in.</param>
        /// <returns>
        /// The number of rows successfully added to or refreshed in the System.Data.DataTable object.
        /// This does not include rows affected by statements that do not return rows.
        /// </returns>
        public int Fill(DataTable dataTable)
        {
            return this.Fill(dataTable, 0);
        }

        public int Fill(DataTable dataTable, int selectCommandIndex)
        {
            return this.Fill(dataTable, selectCommandIndex, null);
        }

        public virtual int Fill(DataTable dataTable, int selectCommandIndex, params object[] parametersValues)
        {
            int returnValue = 0;
            if (dataTable != null)
            {
                if (this.ClearBeforeFill == true)
                    dataTable.Clear();

                if (this.SelectCommands.Count > selectCommandIndex)
                {
                    using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                    {

                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.TableMappings.Add(this.CloneTableMapping());

                            adapter.SelectCommand = this.SelectCommands[selectCommandIndex].Clone();
                            SetParametersValues(adapter.SelectCommand, parametersValues);
                            adapter.SelectCommand.Connection = connection;

                            returnValue = adapter.Fill(dataTable);
                            if (connection.State == ConnectionState.Open) connection.Close();
                        }
                    }
                }
            }
            return returnValue;
        }

        public int Insert()
        {
            return this.Insert(null);
        }

        public virtual int Insert(params object[] parametersValues)
        {
            return ExecuteNonQuery(this.InsertCommand, parametersValues);
        }

        public int Update()
        {
            return this.Update((object[])null);
        }

        public virtual int Update(params object[] parametersValues)
        {
            SetParametersValues(this.UpdateCommand, parametersValues);

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                this.UpdateCommand.Connection = connection;

                return Support.ExecuteNonQuery(this.UpdateCommand);
            }
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified System.Data.DataTable object.
        /// </summary>
        /// <param name="dataTable">The System.Data.DataTable object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        public virtual int Update(DataTable dataTable)
        {
            return this.ExecuteUpdate(dataTable);
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified System.Data.DataSet object.
        /// </summary>
        /// <param name="dataSet">The System.Data.DataSet object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        public virtual int Update(DataSet dataSet)
        {
            if (dataSet != null) return this.Update(dataSet.Tables[this.TableName.ToString()]);
            return 0;
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for specified System.Data.DataRow object.
        /// </summary>
        /// <param name="dataRow">An System.Data.DataRow object used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        public virtual int Update(DataRow dataRow)
        {
            return this.Update(new DataRow[] { dataRow });
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row 
        /// in the specified array of System.Data.DataRow objects.
        /// </summary>
        /// <param name="dataRows">An array of System.Data.DataRow objects used to update the data sourceRow.</param>
        /// <returns>The number of rows successfully updated.</returns>
        public virtual int Update(DataRow[] dataRows)
        {
            return this.ExecuteUpdate(dataRows);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
