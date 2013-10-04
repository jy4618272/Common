using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Micajah.Common.Bll
{
    public enum MeasureUnitName
    {
        None = 0,
        SingularName,
        SingularAbbreviation,
        SingularFullName,
        PluralName,
        PluralAbbreviation,
        PluralFullName
    }

    [Serializable]
    public class MeasureUnit
    {
        #region Members

        private static MeasureUnit s_Empty;

        #endregion

        #region Properties

        public Guid MeasureUnitId { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string SingularName { get; private set; }

        public string SingularAbbreviation { get; private set; }

        public string SingularFullName { get; private set; }

        public string PluralName { get; private set; }

        public string PluralAbbreviation { get; private set; }

        public string PluralFullName { get; private set; }

        public string GroupName { get; private set; }

        public string UnitType { get; private set; }

        public double ConversionFactor { get; private set; }

        public int Level { get; private set; }

        public MeasureUnitCollection RelatedUnits
        {
            get
            {
                MeasureUnitCollection list = new MeasureUnitCollection();
                foreach (MasterDataSet.UnitsOfMeasureConversionRow r in MeasureUnitsProvider.GetUnitOfMeasureConversionFromByOrganizationId(this.MeasureUnitId, this.OrganizationId))
                {
                    MeasureUnit unit = MeasureUnit.Create(r.UnitOfMeasureTo, this.OrganizationId);
                    if (unit != null)
                    {
                        unit.SetConversionFactor(r.Factor);
                        unit.SetLevel(this.Level + 1);
                        list.Add(unit);
                    }
                }
                return list;
            }
        }

        public static MeasureUnit Empty
        {
            get
            {
                if (s_Empty == null) s_Empty = new MeasureUnit();
                return s_Empty;
            }
        }

        #endregion

        #region Constructors

        private MeasureUnit()
        {
            this.MeasureUnitId = Guid.Empty;
            this.OrganizationId = Guid.Empty;
            this.SingularName = string.Empty;
            this.SingularFullName = string.Empty;
            this.SingularAbbreviation = string.Empty;
            this.PluralName = string.Empty;
            this.PluralFullName = string.Empty;
            this.PluralAbbreviation = string.Empty;
            this.UnitType = string.Empty;
            this.GroupName = string.Empty;
            this.Level = 0;
            this.ConversionFactor = 1;
        }

        #endregion

        #region Fabric Methods

        public static MeasureUnit Create(MasterDataSet.UnitsOfMeasureRow row)
        {
            MeasureUnit unit = new MeasureUnit();
            if (row != null)
            {
                unit.MeasureUnitId = row.UnitsOfMeasureId;
                unit.OrganizationId = row.OrganizationId;
                unit.SingularName = row.SingularName;
                unit.SingularFullName = row.SingularFullName;
                unit.SingularAbbreviation = row.SingularAbbrv;
                unit.PluralName = row.PluralName;
                unit.PluralFullName = row.PluralFullName;
                unit.PluralAbbreviation = row.PluralAbbrv;
                unit.GroupName = row.GroupName;
                unit.UnitType = row.LocalName;
                unit.ConversionFactor = 1;
                unit.Level = 0;
            }
            return unit;
        }

        public static MeasureUnit Create(Guid measureUnitId, Guid organizationId)
        {
            return Create(MeasureUnitsProvider.GetMeasureUnitRow(measureUnitId, organizationId));
        }

        #endregion

        #region Static Methods

        public static ReadOnlyCollection<string> GetGroupNames()
        {
            return MeasureUnitsProvider.GetUnitGroups();
        }

        public static ReadOnlyCollection<string> GetUnitTypeNames()
        {
            return MeasureUnitsProvider.GetUnitTypeNames();
        }

        public static string GetMeasureUnitName(Guid measureUnitsId, MeasureUnitName unitName)
        {
            return MeasureUnitsProvider.GetMeasureUnitName(measureUnitsId, unitName);
        }

        #endregion

        #region Public Methods

        private void FillRelatedUnits(ref MeasureUnitCollection units)
        {
            if (this.Level > 10) return;
            MeasureUnitCollection tmp = new MeasureUnitCollection();
            foreach (MeasureUnit unit in this.RelatedUnits)
            {
                if (!units.Contains(unit))
                {
                    unit.SetLevel(this.Level + 1);
                    unit.SetConversionFactor(unit.ConversionFactor * this.ConversionFactor);
                    units.Add(unit);
                    tmp.Add(unit);
                }
            }
            foreach (MeasureUnit unit in tmp)
                unit.FillRelatedUnits(ref units);
        }

        public MeasureUnitCollection GetConvertUnits()
        {
            MeasureUnitCollection units = new MeasureUnitCollection();
            units.Add(this);
            this.FillRelatedUnits(ref units);
            units.Remove(this);
            //units.SortByLevel();
            return units;
        }

        public void SetConversionFactor(double factor)
        {
            this.ConversionFactor = factor;
        }

        public void SetLevel(int level)
        {
            this.Level = level;
        }

        #endregion

        #region Override Methods

        public override bool Equals(object obj)
        {
            MeasureUnit unit = obj as MeasureUnit;
            if (obj != null)
                return (this.MeasureUnitId == unit.MeasureUnitId && this.OrganizationId == unit.OrganizationId);
            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
    [Serializable]
    [DataObjectAttribute(true)]
    public class MeasureUnitCollection : Collection<MeasureUnit>
    {
        #region Private Properties

        private List<MeasureUnit> ItemList
        {
            get { return base.Items as List<MeasureUnit>; }
        }

        #endregion

        #region Public Properties

        public MeasureUnit this[string name]
        {
            get
            {
                int index = this.FindIndexByIdOrName(name);
                return (((index < 0) || (index >= this.Count)) ? null : base[index]);
            }
            set
            {
                int index = this.FindIndexByIdOrName(name);
                if (index > -1)
                    base[index] = value;
                else
                    base.Add(value);
            }
        }

        #endregion

        #region Private Methods

        private int FindIndexByIdOrName(string value)
        {
            int index = this.ItemList.FindIndex(
                delegate(MeasureUnit measureUnit)
                {
                    if (measureUnit.SingularName.Equals(value, StringComparison.Ordinal))
                        return true;
                    if (measureUnit.SingularAbbreviation.Equals(value, StringComparison.Ordinal))
                        return true;
                    if (measureUnit.PluralName.Equals(value, StringComparison.Ordinal))
                        return true;
                    if (measureUnit.PluralAbbreviation.Equals(value, StringComparison.Ordinal))
                        return true;
                    return false;
                });

            if (index == -1)
            {
                object obj = Support.ConvertStringToType(value, typeof(Guid));
                if (obj != null)
                {
                    Guid id = (Guid)obj;

                    index = this.ItemList.FindIndex(
                        delegate(MeasureUnit measureUnit)
                        {
                            return (measureUnit.MeasureUnitId == id);
                        });
                }
            }

            return index;
        }

        private int SortByName(MeasureUnit x, MeasureUnit y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else
                if (y == null)
                    return 1;
                else
                    return string.Compare(x.SingularFullName, y.SingularFullName, StringComparison.Ordinal);
        }

        private int SortByType(MeasureUnit x, MeasureUnit y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else
                if (y == null)
                    return 1;
                else
                    return string.Compare(x.UnitType, y.UnitType, StringComparison.Ordinal);
        }

        private int SortByGroup(MeasureUnit x, MeasureUnit y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else
                if (y == null)
                    return 1;
                else
                    return string.Compare(x.GroupName, y.GroupName, StringComparison.Ordinal);
        }

        private int SortByLevel(MeasureUnit x, MeasureUnit y)
        {
            if (x == null)
                if (y == null)
                    return 0;
                else
                    return -1;
            else
                if (y == null)
                    return 1;
                else
                    return x.Level.CompareTo(y.Level);
        }

        #endregion

        #region Public Methods

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MeasureUnitCollection GetUnits()
        {
            Guid orgId = Micajah.Common.Security.UserContext.Current.OrganizationId;
            if (orgId.Equals(Guid.Empty))
                return new MeasureUnitCollection();
            else return MeasureUnitCollection.GetUnits(orgId);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MeasureUnitCollection GetUnits(Guid organizationId)
        {
            MeasureUnitCollection list = new MeasureUnitCollection();

            foreach (MasterDataSet.UnitsOfMeasureRow row in MeasureUnitsProvider.GetBuiltInMeasureUnits())
                list.Add(MeasureUnit.Create(row));

            foreach (MasterDataSet.UnitsOfMeasureRow row in MeasureUnitsProvider.GetMeasureUnits(organizationId))
                list.Add(MeasureUnit.Create(row));

            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MeasureUnitCollection GetUnitsByGroup(Guid organizationId, string groupName)
        {
            MeasureUnitCollection list = new MeasureUnitCollection();
            MasterDataSet.UnitsOfMeasureDataTable table = MeasureUnitsProvider.GetMeasureUnits(organizationId);
            foreach (MasterDataSet.UnitsOfMeasureRow row in table)
                if (row.GroupName.Equals(groupName))
                    list.Add(MeasureUnit.Create(row));
            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MeasureUnitCollection GetUnitsByGroup(string groupName)
        {
            Guid orgId = Micajah.Common.Security.UserContext.Current.OrganizationId;
            if (orgId.Equals(Guid.Empty))
                return new MeasureUnitCollection();
            else return MeasureUnitCollection.GetUnitsByGroup(orgId, groupName);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MeasureUnitCollection GetUnitsByUnitType(Guid organizationId, string unitType)
        {
            MeasureUnitCollection list = new MeasureUnitCollection();
            MasterDataSet.UnitsOfMeasureDataTable table = MeasureUnitsProvider.GetMeasureUnits(organizationId);
            foreach (MasterDataSet.UnitsOfMeasureRow row in table)
                if (row.LocalName.Equals(unitType))
                    list.Add(MeasureUnit.Create(row));
            return list;
        }

        public static MeasureUnitCollection GetUnitsByUnitType(string unitType)
        {
            Guid orgId = Micajah.Common.Security.UserContext.Current.OrganizationId;
            if (orgId.Equals(Guid.Empty))
                return new MeasureUnitCollection();
            else return MeasureUnitCollection.GetUnitsByUnitType(orgId, unitType);
        }

        public void SortByName()
        {
            this.ItemList.Sort(SortByName);
        }

        public void SortByType()
        {
            this.ItemList.Sort(SortByType);
        }

        public void SortByGroup()
        {
            this.ItemList.Sort(SortByGroup);
        }

        public void SortByLevel()
        {
            this.ItemList.Sort(SortByLevel);
        }

        #endregion
    }
}
