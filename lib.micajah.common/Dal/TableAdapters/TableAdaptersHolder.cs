using System;
using System.Collections;
using System.Collections.Generic;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The base class for the the tables adapters holder.
    /// </summary>
    public abstract class TableAdaptersHolder : IDisposable
    {
        #region Members

        private Dictionary<TableName, ITableAdapter> m_TableAdapters;

        #endregion

        #region Protected Properties

        protected Dictionary<TableName, ITableAdapter> TableAdapters
        {
            get
            {
                if (m_TableAdapters == null) m_TableAdapters = new Dictionary<TableName, ITableAdapter>();
                return m_TableAdapters;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class with default adapters for the tables.
        /// </summary>
        protected TableAdaptersHolder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class with specified adapters for the tables.
        /// If the collection doesn't contain an adapter for some table then default adapter will be used.
        /// </summary>
        /// <param name="adapters">The collection of the adapters for the tables.</param>
        protected TableAdaptersHolder(ICollection adapters)
        {
            if (adapters != null)
            {
                foreach (ITableAdapter adapter in adapters)
                {
                    if (adapter != null) this.TableAdapters[adapter.TableName] = adapter;
                }
            }
        }

        #endregion

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (TableName tableName in this.TableAdapters.Keys)
                {
                    if (this.TableAdapters.ContainsKey(tableName))
                    {
                        ITableAdapter adapter = this.TableAdapters[tableName];
                        if (adapter != null) adapter.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        protected void AddAdapter(TableName tableName, Type adapterType)
        {
            if (adapterType == null) return;

            if (this.TableAdapters.ContainsKey(tableName))
            {
                if (this.TableAdapters[tableName] == null)
                    this.TableAdapters[tableName] = (ITableAdapter)adapterType.Assembly.CreateInstance(adapterType.FullName);
            }
            else
                this.TableAdapters.Add(tableName, (ITableAdapter)adapterType.Assembly.CreateInstance(adapterType.FullName));
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
