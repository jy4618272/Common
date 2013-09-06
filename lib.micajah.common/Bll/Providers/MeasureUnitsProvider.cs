using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
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

        private static Guid InsertMeasureUnit(
            string singularName,
            string singularAbbreviation,
            string pluralName,
            string pluralAbbreviation,
            string groupName,
            string localName,
            Guid organizationId,
            bool refreshData)
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return Guid.Empty;

            CommonDataSet.UnitsOfMeasureDataTable table = ds.UnitsOfMeasure;
            CommonDataSet.UnitsOfMeasureRow row = table.NewUnitsOfMeasureRow();
            Guid newId = Guid.NewGuid();
            row.UnitsOfMeasureId = newId;
            row.SingularName = singularName;
            row.SingularAbbrv = singularAbbreviation;
            row.PluralName = pluralName;
            row.PluralAbbrv = pluralAbbreviation;
            row.GroupName = groupName;
            row.LocalName = localName;
            row.OrganizationId = organizationId;
            table.AddUnitsOfMeasureRow(row);

            MasterDataSetTableAdapters adapters = MasterDataSetTableAdapters.Current;
            if (adapters != null) adapters.UnitsOfMeasureAdapter.Update(row);
            if (refreshData) WebApplication.RefreshMeasureUnits();
            return newId;
        }

        private static void UpdateMeasureUnitsConversion(
            Guid measureUnitFrom,
            Guid measureUnitTo,
            Guid organizationId,
            double factor,
            bool refreshData,
            bool deleted)
        {
            if (double.IsNaN(factor) || factor <= 0.0) throw new ArithmeticException(Resources.MeasureUnitsProvider_ErrorMessage_FactorCannotBeZero);

            if (measureUnitFrom.Equals(Guid.Empty) ||
                measureUnitTo.Equals(Guid.Empty) ||
                measureUnitFrom.Equals(measureUnitTo))
                throw new ArgumentException(Resources.MeasureUnitsProvider_ErrorMessage_IncorrectIdentifier);

            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return;

            CommonDataSet.UnitsOfMeasureConversionDataTable table = ds.UnitsOfMeasureConversion;
            bool isNew = true;
            CommonDataSet.UnitsOfMeasureConversionRow row = null;

            foreach (CommonDataSet.UnitsOfMeasureConversionRow r in table)
            {
                if (r.UnitOfMeasureFrom.Equals(measureUnitFrom) &&
                    r.UnitOfMeasureTo.Equals(measureUnitTo) &&
                    r.OrganizationId.Equals(organizationId))
                {
                    row = r;
                    isNew = false;
                }
            }

            if (isNew && row == null)
            {
                row = table.NewUnitsOfMeasureConversionRow();
                row.UnitOfMeasureFrom = measureUnitFrom;
                row.UnitOfMeasureTo = measureUnitTo;
                row.OrganizationId = organizationId;
            }
            row.Factor = factor;
            if (isNew)
                table.AddUnitsOfMeasureConversionRow(row);
            else if (deleted)
                row.Delete();
            //throw new ConstraintException(Resources.MeasureUnitsProvider_ErrorMessage_ConversionAlreadyExists, ex);

            MasterDataSetTableAdapters adapters = MasterDataSetTableAdapters.Current;
            if (adapters != null) adapters.UnitsOfMeasureConversionAdapter.Update(row);
            if (refreshData) WebApplication.RefreshMeasureUnits();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all group names for Measure Units
        /// </summary>
        /// <returns>String array of groups</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), DataObjectMethod(DataObjectMethodType.Select)]
        public static ReadOnlyCollection<string> GetUnitGroups()
        {
            List<string> groups = new List<string>();
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;
            foreach (CommonDataSet.UnitsOfMeasureRow row in ds.UnitsOfMeasure)
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), DataObjectMethod(DataObjectMethodType.Select)]
        public static ReadOnlyCollection<string> GetUnitTypeNames()
        {
            List<string> locals = new List<string>();
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;
            locals.Add("English");
            locals.Add("Metric");
            foreach (CommonDataSet.UnitsOfMeasureRow row in ds.UnitsOfMeasure)
            {
                if (!locals.Contains(row.LocalName) &&
                    !locals.Contains("English") &&
                    !locals.Contains("Metric"))
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.UnitsOfMeasureDataTable GetBuiltInMeasureUnits()
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;

            CommonDataSet.UnitsOfMeasureDataTable table = null;
            try
            {
                table = new CommonDataSet.UnitsOfMeasureDataTable();
                foreach (CommonDataSet.UnitsOfMeasureRow row in ds.UnitsOfMeasure)
                {
                    if (row.OrganizationId.Equals(Guid.Empty))
                    {
                        CommonDataSet.UnitsOfMeasureRow newrow = table.NewUnitsOfMeasureRow();
                        newrow.UnitsOfMeasureId = row.UnitsOfMeasureId;
                        newrow.OrganizationId = Guid.Empty;
                        newrow.SingularName = row.SingularName;
                        newrow.SingularAbbrv = row.SingularAbbrv;
                        newrow.SingularFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.SingularName, row.LocalName);
                        newrow.PluralName = row.PluralName;
                        newrow.PluralAbbrv = row.PluralAbbrv;
                        newrow.PluralFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.PluralName, row.LocalName);
                        newrow.GroupName = row.GroupName;
                        newrow.LocalName = row.LocalName;
                        table.AddUnitsOfMeasureRow(newrow);
                    }
                }
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Gets only local measure units without global units
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <returns>UnitsOfMeasureDataTable object that contains measure units</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.UnitsOfMeasureDataTable GetMeasureUnits(Guid organizationId)
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;

            CommonDataSet.UnitsOfMeasureDataTable table = null;
            try
            {
                table = new CommonDataSet.UnitsOfMeasureDataTable();
                foreach (CommonDataSet.UnitsOfMeasureRow row in ds.UnitsOfMeasure)
                {
                    if (row.OrganizationId.Equals(organizationId))
                    {
                        CommonDataSet.UnitsOfMeasureRow newrow = table.NewUnitsOfMeasureRow();
                        newrow.UnitsOfMeasureId = row.UnitsOfMeasureId;
                        newrow.OrganizationId = organizationId;
                        newrow.SingularName = row.SingularName;
                        newrow.SingularAbbrv = row.SingularAbbrv;
                        newrow.SingularFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.SingularName, row.LocalName);
                        newrow.PluralName = row.PluralName;
                        newrow.PluralAbbrv = row.PluralAbbrv;
                        newrow.PluralFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.PluralName, row.LocalName);
                        newrow.GroupName = row.GroupName;
                        newrow.LocalName = row.LocalName;
                        table.AddUnitsOfMeasureRow(newrow);
                    }
                }
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Gets only local measure units without global units
        /// </summary>
        /// <param name="organizationId">Organization Identifier</param>
        /// <returns>UnitsOfMeasureDataTable object that contains measure units</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.UnitsOfMeasureDataTable GetMeasureUnits()
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
        public static CommonDataSet.UnitsOfMeasureRow GetMeasureUnitRow(Guid unitsOfMeasureId, Guid organizationId)
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;

            CommonDataSet.UnitsOfMeasureRow row = ds.UnitsOfMeasure.FindByUnitsOfMeasureIdOrganizationId(unitsOfMeasureId, organizationId);
            if (row == null) row = ds.UnitsOfMeasure.FindByUnitsOfMeasureIdOrganizationId(unitsOfMeasureId, Guid.Empty);
            if (row != null)
            {
                row.SingularFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.SingularName, row.LocalName);
                row.PluralFullName = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.PluralName, row.LocalName);
            }
            return row;
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
        public static CommonDataSet.UnitsOfMeasureRow GetMeasureUnitRow(Guid unitsOfMeasureId)
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
            CommonDataSet.UnitsOfMeasureRow row = GetMeasureUnitRow(unitsOfMeasureId);
            if (row == null) return string.Empty;
            switch (unitName)
            {
                case MeasureUnitName.SingularName:
                    return row.SingularName;
                case MeasureUnitName.SingularAbbreviation:
                    return row.SingularAbbrv;
                case MeasureUnitName.SingularFullName:
                    return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.SingularName, row.LocalName);
                case MeasureUnitName.PluralName:
                    return row.PluralName;
                case MeasureUnitName.PluralAbbreviation:
                    return row.PluralAbbrv;
                case MeasureUnitName.PluralFullName:
                    return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", row.PluralName, row.LocalName);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertMeasureUnit(
            Guid organizationId,
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv,
            string groupName,
            string localName)
        {
            return InsertMeasureUnit(singularName, singularAbbrv, pluralName, pluralAbbrv, groupName, localName, organizationId, true);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertMeasureUnit(
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv,
            string groupName,
            string localName)
        {
            return InsertMeasureUnit(singularName, singularAbbrv, pluralName, pluralAbbrv, groupName, localName, UserContext.Current.SelectedOrganizationId, true);
        }

        /// <summary>
        /// Creates a new conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId,
            Guid organizationId,
            double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, factor, true, false);
        }

        /// <summary>
        /// Creates a new conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId,
            double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, factor, true, false);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnit(
            Guid unitsOfMeasureId,
            Guid organizationId,
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv,
            string groupName,
            string localName)
        {
            CommonDataSet.UnitsOfMeasureRow row = GetMeasureUnitRow(unitsOfMeasureId, organizationId);
            if (row == null) throw new ArgumentException(Resources.MeasureUnitsProvider_ErrorMessage_CannotFindByMeasureUnitId);
            row.SingularName = singularName;
            row.SingularAbbrv = singularAbbrv;
            row.PluralName = pluralName;
            row.PluralAbbrv = pluralAbbrv;
            if (groupName != null) row.GroupName = groupName;
            if (localName != null) row.LocalName = localName;
            MasterDataSetTableAdapters adapters = MasterDataSetTableAdapters.Current;
            if (adapters != null) adapters.UnitsOfMeasureAdapter.Update(row);
            WebApplication.RefreshMeasureUnits();
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnit(
            Guid unitsOfMeasureId,
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv,
            string groupName,
            string localName)
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnit(
            Guid unitsOfMeasureId,
            Guid organizationId,
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv)
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abbrv"), DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnit(
            Guid unitsOfMeasureId,
            string singularName,
            string singularAbbrv,
            string pluralName,
            string pluralAbbrv)
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
        public static void UpdateMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId,
            Guid organizationId,
            double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, factor, true, false);
        }

        /// <summary>
        /// Updates the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        /// <param name="factor">Conversion factor. Must be more zero.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId,
            double factor)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, factor, true, false);
        }

        /// <summary>
        /// Deletes the Measure Unit
        /// </summary>
        /// <param name="measureUnitId">The identifier of the Measure Unit.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnits(Guid unitsOfMeasureId, Guid organizationId)
        {
            CommonDataSet.UnitsOfMeasureRow row = GetMeasureUnitRow(unitsOfMeasureId, organizationId);
            if (row == null) throw new ArgumentException(Resources.MeasureUnitsProvider_ErrorMessage_CannotFindByMeasureUnitId);
            row.Delete();
            MasterDataSetTableAdapters adapters = MasterDataSetTableAdapters.Current;
            if (adapters != null) adapters.UnitsOfMeasureAdapter.Update(row);
            WebApplication.RefreshMeasureUnits();
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
        public static void DeleteMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, UserContext.Current.SelectedOrganizationId, 1, true, true);
        }


        /// <summary>
        /// Deletes the conversion of measure units
        /// </summary>
        /// <param name="measureUnitFrom">Measure Unit Identifier of conversion sourceRow</param>
        /// <param name="measureUnitTo">Measure Unit Identifier of conversion target</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteMeasureUnitsConversion(
            Guid sourceUnitsOfMeasureId,
            Guid targetUnitsOfMeasureId,
            Guid organizationId)
        {
            UpdateMeasureUnitsConversion(sourceUnitsOfMeasureId, targetUnitsOfMeasureId, organizationId, 1, true, true);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetConvertedMeasureUnits(Guid unitsOfMeasureId)
        {
            return GetConvertedMeasureUnits(unitsOfMeasureId, UserContext.Current.SelectedOrganizationId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetConvertedMeasureUnits(Guid unitsOfMeasureId, Guid organizationId)
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return null;

            DataTable table = null;
            try
            {
                table = new DataTable();
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("SourceUnitsOfMeasureId", typeof(Guid));
                table.Columns.Add("SourceSingularName", typeof(string));
                table.Columns.Add("SourcePluralName", typeof(string));
                table.Columns.Add("TargetUnitsOfMeasureId", typeof(Guid));
                table.Columns.Add("TargetSingularName", typeof(string));
                table.Columns.Add("TargetPluralName", typeof(string));
                table.Columns.Add("Factor", typeof(float));
                CommonDataSet.UnitsOfMeasureRow sourceRow = GetMeasureUnitRow(unitsOfMeasureId, organizationId);
                if (sourceRow == null) return null;

                foreach (CommonDataSet.UnitsOfMeasureConversionRow r in ds.UnitsOfMeasureConversion)
                {
                    if (r.UnitOfMeasureFrom.Equals(unitsOfMeasureId) && r.OrganizationId.Equals(organizationId))
                    {
                        CommonDataSet.UnitsOfMeasureRow targetRow = GetMeasureUnitRow(r.UnitOfMeasureTo, organizationId);
                        if (targetRow == null) continue;
                        DataRow newRow = table.NewRow();
                        newRow["SourceUnitsOfMeasureId"] = sourceRow.UnitsOfMeasureId;
                        newRow["SourceSingularName"] = sourceRow.SingularFullName;
                        newRow["SourcePluralName"] = sourceRow.PluralFullName;
                        newRow["TargetUnitsOfMeasureId"] = targetRow.UnitsOfMeasureId;
                        newRow["TargetSingularName"] = targetRow.SingularFullName;
                        newRow["TargetPluralName"] = targetRow.PluralFullName;
                        newRow["Factor"] = r.Factor;
                        table.Rows.Add(newRow);
                    }
                }
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetMeasureUnitsExceptCurrent(Guid unitsOfMeasureId)
        {
            CommonDataSet.UnitsOfMeasureDataTable table = GetMeasureUnits();
            DataView view = table.DefaultView;
            view.RowFilter = string.Format(CultureInfo.CurrentCulture, "UnitsOfMeasureId <> '{0}'", unitsOfMeasureId.ToString());
            return view;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetMeasureUnitsByGroupExceptCurrent(string groupName, Guid unitsOfMeasureId)
        {
            CommonDataSet.UnitsOfMeasureDataTable table = GetMeasureUnits();
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
            CommonDataSet ds = WebApplication.CommonDataSet;
            if (ds == null) return 0;

            CommonDataSet.UnitsOfMeasureConversionRow row = ds.UnitsOfMeasureConversion.FindByOrganizationIdUnitOfMeasureToUnitOfMeasureFrom(organizationId, to, from);
            if (row != null)
            {
                return value * row.Factor;
            }
            return 0;
        }

        public static bool OverrideMeasureUnit(Guid unitsOfMeasureId, Guid organizationId)
        {
            UnitsOfMeasureAdapter adapter = MasterDataSetTableAdapters.Current.UnitsOfMeasureAdapter as UnitsOfMeasureAdapter;
            if (adapter != null && adapter.OverrideUnit(unitsOfMeasureId, organizationId) > 0)
            {
                WebApplication.RefreshMeasureUnits();
                return true;
            }
            return false;
        }
        #endregion
    }
}
