using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;

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
        public static MasterDataSet.CountryDataTable GetCountries()
        {
            using (CountryTableAdapter adapter = new CountryTableAdapter())
            {
                return adapter.GetCountries();
            }
        }

        /// <summary>
        /// Creates new country.
        /// </summary>
        /// <param name="name">The name of the country.</param>
        /// <returns>The identifier of the newly created country.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertCountry(string name)
        {
            using (CountryTableAdapter adapter = new CountryTableAdapter())
            {
                Guid countryId = Guid.NewGuid();
                adapter.Insert(countryId, name);
                return countryId;
            }
        }

        #endregion
    }
}
