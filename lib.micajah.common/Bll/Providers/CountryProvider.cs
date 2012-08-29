using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with countries.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class CountryProvider
    {
        #region Public Methods

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <returns>The System.Data.DataView object that contains the countries.</returns>s
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetCountriesView()
        {
            DataView dv = WebApplication.CommonDataSet.Country.DefaultView;
            dv.Sort = "Name";
            return dv;
        }

        /// <summary>
        /// Creates new country.
        /// </summary>
        /// <param name="name">The name of the country.</param>
        /// <param name="throwOnError">true to throw an exception if an error occured; false to return false.</param>
        /// <returns>The identifier of the newly created country.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertCountry(string name, bool throwOnError)
        {
            CommonDataSet.CountryDataTable table = WebApplication.CommonDataSet.Country;
            CommonDataSet.CountryRow row = table.NewCountryRow();

            row.CountryId = Guid.NewGuid();

            try
            {
                row.Name = name;

                table.AddCountryRow(row);
                WebApplication.CommonDataSetTableAdapters.CountryTableAdapter.Update(row);
            }
            catch (ConstraintException ex)
            {
                string msg = ex.Message;
                if (msg.Contains(string.Concat("'", table.NameColumn.ColumnName, "'")))
                    msg = string.Format(CultureInfo.CurrentCulture, Resources.CountryProvider_ErrorMessage_CountryNameAlreadyExists, name);

                row.CountryId = Guid.Empty;

                if (throwOnError)
                    throw new ConstraintException(msg);
            }

            return row.CountryId;
        }

        #endregion
    }
}
