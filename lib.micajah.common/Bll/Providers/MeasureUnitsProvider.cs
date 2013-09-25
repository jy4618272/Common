using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with Units fo Measure.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class MeasureUnitsProvider
    {
        #region Private Methods

        private static MasterDataSet.UnitsOfMeasureDataTable GetAllMeasureUnits()
        {
            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                return adapter.GetUnitsOfMeasure();
            }
        }

        private static MasterDataSet.UnitsOfMeasureConversionRow GetUnitsOfMeasureConversionRow(Guid from, Guid to, Guid organizationId)
        {
            using (UnitsOfMeasureConversionTableAdapter adapter = new UnitsOfMeasureConversionTableAdapter())
            {
                MasterDataSet.UnitsOfMeasureConversionDataTable table = adapter.GetUnitOfMeasureConversionByOrganizationId(from, to, organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        private static Guid InsertMeasureUnit(string singularName, string singularAbbreviation, string pluralName, string pluralAbbreviation, string groupName, string localName, Guid organizationId)
        {
            Guid newId = Guid.NewGuid();

            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                adapter.Insert(newId, organizationId, singularName, singularAbbreviation, pluralName, pluralAbbreviation, groupName, localName);
            }

            return newId;
        }

        private static void UpdateMeasureUnitsConversion(Guid measureUnitFrom, Guid measureUnitTo, Guid organizationId, double factor, bool deleted)
        {
            if (double.IsNaN(factor) || factor <= 0.0)
                throw new ArithmeticException(Resources.MeasureUnitsProvider_ErrorMessage_FactorCannotBeZero);

            if (measureUnitFrom.Equals(Guid.Empty) || measureUnitTo.Equals(Guid.Empty) || measureUnitFrom.Equals(measureUnitTo))
                throw new ArgumentException(Resources.MeasureUnitsProvider_ErrorMessage_IncorrectIdentifier);

            MasterDataSet.UnitsOfMeasureConversionRow row = GetUnitsOfMeasureConversionRow(measureUnitFrom, measureUnitTo, organizationId);

            if (row == null)
            {
                MasterDataSet.UnitsOfMeasureConversionDataTable table = new MasterDataSet.UnitsOfMeasureConversionDataTable();
                row = table.NewUnitsOfMeasureConversionRow();
                row.UnitOfMeasureFrom = measureUnitFrom;
                row.UnitOfMeasureTo = measureUnitTo;
                row.OrganizationId = organizationId;
                row.Factor = factor;
                table.AddUnitsOfMeasureConversionRow(row);
            }
            else
            {
                if (deleted)
                    row.Delete();
                else
                    row.Factor = factor;
            }

            using (UnitsOfMeasureConversionTableAdapter adapter = new UnitsOfMeasureConversionTableAdapter())
            {
                adapter.Update(row);
            }
        }

        #endregion

        #region Internal Methods

        internal static MasterDataSet.UnitsOfMeasureConversionDataTable GetUnitOfMeasureConversionFromByOrganizationId(Guid unitsOfMeasureId, Guid organizationId)
        {
            using (UnitsOfMeasureConversionTableAdapter adapter = new UnitsOfMeasureConversionTableAdapter())
            {
                return adapter.GetUnitOfMeasureConversionFromByOrganizationId(unitsOfMeasureId, organizationId);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all group names for Measure Units
        /// </summary>
        /// <returns>String array of groups</returns>
        public static ReadOnlyCollection<string> GetUnitGroups()
        {
            List<string> groups = new List<string>();
            foreach (MasterDataSet.UnitsOfMeasureRow row in GetAllMeasureUnits())
            {
                if (!groups.Contains(row.GroupName))
                    groups.Add(row.GroupName);
            }
            groups.Sort();
            return new ReadOnlyCollection<string>(groups);
        }

        /// <summary>
        /// Gets all local names 
        /// </summary>
        /// <returns>String array of groups</returns>
        public static ReadOnlyCollection<string> GetUnitTypeNames()
        {
            List<string> locals = new List<string>();
            locals.Add("English");
            locals.Add("Metric");
            foreach (MasterDataSet.UnitsOfMeasureRow row in GetAllMeasureUnits())
            {
                if (!locals.Contains(row.LocalName) && !locals.Contains("English") && !locals.Contains("Metric"))
                {
                    locals.Add(row.GroupName);
                }
            }
            locals.Sort();
            return new ReadOnlyCollection<string>(locals);
        }

        /// <summary>
        /// Gets only global measure units
        /// </summary>
        /// <returns>UnitsOfMeasureDataTable object that contains measure units</returns>
        public static MasterDataSet.UnitsOfMeasureDataTable GetBuiltInMeasureUnits()
        {
            return GetMeasureUnits(Guid.Empty);
        }

        /// <summary>
        /// Gets only local measure units without global units
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <returns>UnitsOfMeasureDataTable object that contains measure units</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.UnitsOfMeasureDataTable GetMeasureUnits(Guid organizationId)
        {
            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                return adapter.GetUnitsOfMeasureByOrganizationId(organizationId);
            }
        }

        /// <summary>
        /// Gets only local measure units without global units
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <returns>UnitsOfMeasureDataTable object that contains measure units</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.UnitsOfMeasureDataTable GetMeasureUnits()
        {
            return GetMeasureUnits(UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Gets an object populated with information of the specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">Measure Unit Identifier</param>
        /// <returns>
        /// The object populated with information of the specified measure unit. 
        /// If the unit is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.UnitsOfMeasureRow GetMeasureUnitRow(Guid unitsOfMeasureId, Guid organizationId)
        {
            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                MasterDataSet.UnitsOfMeasureDataTable table = adapter.GetUnitOfMeasure(unitsOfMeasureId, organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Gets an object populated with information of the specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">Measure Unit Identifier</param>
        /// <returns>
        /// The object populated with information of the specified measure unit. 
        /// If the unit is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.UnitsOfMeasureRow GetMeasureUnitRow(Guid unitsOfMeasureId)
        {
            return GetMeasureUnitRow(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Gets an object populated with unit name of the specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">Measure Unit Identifier</param>
        /// /// <param name="unitName">Measure Unit Name target</param>
        /// <returns>
        /// The object populated with information of the specified measure unit. 
        /// If the unit is not found, the method returns string.Empty.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static string GetMeasureUnitName(Guid unitsOfMeasureId, MeasureUnitName unitName)
        {
            MasterDataSet.UnitsOfMeasureRow row = GetMeasureUnitRow(unitsOfMeasureId);
            if (row == null)
                return string.Empty;

            switch (unitName)
            {
                case MeasureUnitName.SingularName:
                    return row.SingularName;
                case MeasureUnitName.SingularAbbreviation:
                    return row.SingularAbbrv;
                case MeasureUnitName.SingularFullName:
                    return row.SingularFullName;
                case MeasureUnitName.PluralName:
                    return row.PluralName;
                case MeasureUnitName.PluralAbbreviation:
                    return row.PluralAbbrv;
                case MeasureUnitName.PluralFullName:
                    return row.PluralFullName;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Creates a new organization measure unit 
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        /// <param name="groupName">Group Name</param>
        /// <returns>The System.Guid that represents the identifier of the newly created measure unit.</returns>
        public static Guid InsertMeasureUnit(Guid organizationId, string singularName, string singularAbbrv, string pluralName, string pluralAbbrv, string groupName, string localName)
        {
            return InsertMeasureUnit(singularName, singularAbbrv, pluralName, pluralAbbrv, groupName, localName, organizationId);
        }

        /// <summary>
        /// Creates a new organization measure unit 
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        /// <param name="groupName">Group Name</param>
        /// <returns>The System.Guid that represents the identifier of the newly created measure unit.</returns>
        public static Guid InsertMeasureUnit(string singularName, string singularAbbrv, string pluralName, string pluralAbbrv, string groupName, string localName)
        {
            return InsertMeasureUnit(singularName, singularAbbrv, pluralName, pluralAbbrv, groupName, localName, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Creates a new conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId, Guid organizationId, double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, factor, false);
        }

        /// <summary>
        /// Creates a new conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId, double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, factor, false);
        }

        /// <summary>
        /// Updates the details of specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        /// <param name="groupName">Group Name</param>
        public static void UpdateMeasureUnit(Guid unitsOfMeasureId, Guid organizationId, string singularName, string singularAbbrv, string pluralName, string pluralAbbrv, string groupName, string localName)
        {
            MasterDataSet.UnitsOfMeasureRow row = GetMeasureUnitRow(unitsOfMeasureId, organizationId);
            if (row == null)
                throw new ArgumentException(Resources.MeasureUnitsProvider_ErrorMessage_CannotFindByMeasureUnitId);

            row.SingularName = singularName;
            row.SingularAbbrv = singularAbbrv;
            row.PluralName = pluralName;
            row.PluralAbbrv = pluralAbbrv;
            if (groupName != null) row.GroupName = groupName;
            if (localName != null) row.LocalName = localName;

            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                adapter.Update(row);
            }
        }

        /// <summary>
        /// Updates the details of specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        /// <param name="groupName">Group Name</param>
        public static void UpdateMeasureUnit(Guid unitsOfMeasureId, string singularName, string singularAbbrv, string pluralName, string pluralAbbrv, string groupName, string localName)
        {
            UpdateMeasureUnit(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId, singularName, singularAbbrv, pluralName, pluralAbbrv, groupName, localName);
        }

        /// <summary>
        /// Updates the details of specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        public static void UpdateMeasureUnit(Guid unitsOfMeasureId, Guid organizationId, string singularName, string singularAbbrv, string pluralName, string pluralAbbrv)
        {
            UpdateMeasureUnit(unitsOfMeasureId, organizationId, singularName, singularAbbrv, pluralName, pluralAbbrv, null, null);
        }

        /// <summary>
        /// Updates the details of specified measure unit.
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        /// <param name="singularName">Singular Name</param>
        /// <param name="singularAbbreviation">Singular Abbreviation</param>
        /// <param name="pluralName">Plural Name</param>
        /// <param name="pluralAbbreviation">Plural  Abbreviation</param>
        public static void UpdateMeasureUnit(Guid unitsOfMeasureId, string singularName, string singularAbbrv, string pluralName, string pluralAbbrv)
        {
            UpdateMeasureUnit(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId, singularName, singularAbbrv, pluralName, pluralAbbrv, null, null);
        }

        /// <summary>
        /// Updates the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId, Guid organizationId, double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, factor, false);
        }

        /// <summary>
        /// Updates the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId, double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, factor, false);
        }

        /// <summary>
        /// Deletes the Measure Unit
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnits(Guid unitsOfMeasureId, Guid organizationId)
        {
            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                adapter.Delete(unitsOfMeasureId, organizationId);
            }
        }

        /// <summary>
        /// Deletes the Measure Unit
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnits(Guid unitsOfMeasureId)
        {
            DeleteMeasureUnits(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId);
        }

        /// <summary>
        /// Deletes the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, 1, true);
        }

        /// <summary>
        /// Deletes the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnitsConversion(Guid sourceUnitsOfMeasureId, Guid targetUnitsOfMeasureId, Guid organizationId)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, 1, true);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetConvertedMeasureUnits(Guid unitsOfMeasureId)
        {
            return GetConvertedMeasureUnits(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetConvertedMeasureUnits(Guid unitsOfMeasureId, Guid organizationId)
        {
            using (DataTable resultTable = new DataTable())
            {
                resultTable.Locale = CultureInfo.CurrentCulture;
                resultTable.Columns.Add("SourceUnitsOfMeasureId", typeof(Guid));
                resultTable.Columns.Add("SourceSingularName", typeof(string));
                resultTable.Columns.Add("SourcePluralName", typeof(string));
                resultTable.Columns.Add("TargetUnitsOfMeasureId", typeof(Guid));
                resultTable.Columns.Add("TargetSingularName", typeof(string));
                resultTable.Columns.Add("TargetPluralName", typeof(string));
                resultTable.Columns.Add("Factor", typeof(float));

                MasterDataSet.UnitsOfMeasureRow sourceRow = GetMeasureUnitRow(unitsOfMeasureId, organizationId);
                if (sourceRow == null)
                    return null;

                foreach (MasterDataSet.UnitsOfMeasureConversionRow r in GetUnitOfMeasureConversionFromByOrganizationId(unitsOfMeasureId, organizationId))
                {
                    if (r.UnitOfMeasureFrom.Equals(unitsOfMeasureId) && r.OrganizationId.Equals(organizationId))
                    {
                        MasterDataSet.UnitsOfMeasureRow targetRow = GetMeasureUnitRow(r.UnitOfMeasureTo, organizationId);
                        if (targetRow == null)
                            continue;

                        DataRow newRow = resultTable.NewRow();
                        newRow["SourceUnitsOfMeasureId"] = sourceRow.UnitsOfMeasureId;
                        newRow["SourceSingularName"] = sourceRow.SingularFullName;
                        newRow["SourcePluralName"] = sourceRow.PluralFullName;
                        newRow["TargetUnitsOfMeasureId"] = targetRow.UnitsOfMeasureId;
                        newRow["TargetSingularName"] = targetRow.SingularFullName;
                        newRow["TargetPluralName"] = targetRow.PluralFullName;
                        newRow["Factor"] = r.Factor;
                        resultTable.Rows.Add(newRow);
                    }
                }

                return resultTable;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetMeasureUnitsExceptCurrent(Guid unitsOfMeasureId)
        {
            MasterDataSet.UnitsOfMeasureDataTable table = GetMeasureUnits();
            DataView view = table.DefaultView;
            view.RowFilter = string.Format(CultureInfo.CurrentCulture, "UnitsOfMeasureId <> '{0}'", unitsOfMeasureId.ToString());
            return view;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetMeasureUnitsByGroupExceptCurrent(string groupName, Guid unitsOfMeasureId)
        {
            MasterDataSet.UnitsOfMeasureDataTable table = GetMeasureUnits();
            DataView view = table.DefaultView;
            view.RowFilter = string.Format(CultureInfo.CurrentCulture, "UnitsOfMeasureId <> '{0}' AND GroupName = '{1}'", unitsOfMeasureId.ToString(), groupName);
            return view;
        }

        /// <summary>
        /// Converts value from one Measure Unit to another Unit.
        /// </summary>
        /// <param name="from">Source Measure Unit</param>
        /// <param name="to">Target Measure Unit</param>
        /// <param name="value">Converted value</param>
        /// <returns>Returns a converted value; otherwise return 0 (zero).</returns>
        public static double ConvertValue(Guid from, Guid to, Guid organizationId, double value)
        {
            MasterDataSet.UnitsOfMeasureConversionRow row = GetUnitsOfMeasureConversionRow(from, to, organizationId);
            if (row != null)
                return value * row.Factor;
            return 0;
        }

        public static bool OverrideMeasureUnit(Guid unitsOfMeasureId, Guid organizationId)
        {
            using (UnitsOfMeasureTableAdapter adapter = new UnitsOfMeasureTableAdapter())
            {
                return (adapter.UpdateUnitsOfMeasureOverride(unitsOfMeasureId, organizationId) > 0);
            }
        }

        #endregion
    }
}
