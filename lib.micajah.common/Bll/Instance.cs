using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The instance.
    /// </summary>
    [Serializable]
    public sealed class Instance : IComparable<Instance>
    {
        #region Members

        private Guid m_InstanceId;
        private string m_PseudoId;
        private Guid m_OrganizationId;
        private string m_Name;
        private string m_Description;
        private Guid? m_LogoImageResourceId;
        private bool m_EnableSignupUser;
        private string m_ExternalId;
        private string m_WorkingDays;
        private bool m_Active;
        private DateTime? m_CanceledTime;
        private bool m_Trial;
        private bool m_Beta;
        private DateTime? m_CreatedTime;
        private bool m_LogoImageResourceIdLoaded;
        private Organization m_Organization;
        private SettingCollection m_Settings;
        private SortedList m_GroupIdRoleIdList;
        private string m_EmailSuffixes;
        private Collection<string> m_EmailSuffixesList;

        // The objects which are used to synchronize access to the cached objects.
        private object m_SettingsSyncRoot = new object();
        private object m_EmailSuffixSyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Micajah.Common.Bll.Instance class.
        /// </summary>
        public Instance()
        {
            m_PseudoId = string.Empty;
            m_Name = string.Empty;
            m_Description = string.Empty;
            m_InstanceId = Guid.Empty;

            m_ExternalId = string.Empty;
            m_WorkingDays = "1111100";
            m_Active = true;
            this.TimeZoneId = string.Empty;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the collection of group/role identifiers pairs for this instance.
        /// </summary>
        public SortedList GroupIdRoleIdList
        {
            get
            {
                if (m_GroupIdRoleIdList == null)
                {
                    m_GroupIdRoleIdList = new SortedList();

                    CommonDataSet.RoleDataTable roleTable = WebApplication.CommonDataSet.Role;
                    OrganizationDataSet.GroupsInstancesRolesDataTable gdrTable = Organization.DataSet.GroupsInstancesRoles;

                    foreach (OrganizationDataSet.GroupsInstancesRolesRow gdrRow in gdrTable.Select(string.Concat(gdrTable.InstanceIdColumn.ColumnName, "='", InstanceId, "'")))
                    {
                        if ((roleTable.FindByRoleId(gdrRow.RoleId) != null) && (!m_GroupIdRoleIdList.Contains(gdrRow.GroupId)))
                            m_GroupIdRoleIdList.Add(gdrRow.GroupId, gdrRow.RoleId);
                    }
                }
                return m_GroupIdRoleIdList;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the instance.
        /// </summary>
        public Guid InstanceId
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        /// <summary>
        /// Gets or sets the pseudo unique identifier of the instance.
        /// </summary>
        public string PseudoId
        {
            get { return m_PseudoId; }
            set { m_PseudoId = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the organization which this instance belong to.
        /// </summary>
        public Guid OrganizationId
        {
            get { return m_OrganizationId; }
            set
            {
                m_OrganizationId = value;
                m_Organization = null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the instance.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Gets or sets the instance description.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the logo image.
        /// </summary>
        public Guid? LogoImageResourceId
        {
            get
            {
                if (!m_LogoImageResourceIdLoaded)
                {
                    CommonDataSet.ResourceRow resourceRow = ResourceProvider.GetResourceRow("InstanceLogo", m_InstanceId.ToString("N"));
                    if (resourceRow != null) m_LogoImageResourceId = resourceRow.ResourceId;
                    m_LogoImageResourceIdLoaded = true;
                }
                return m_LogoImageResourceId;
            }
            set
            {
                m_LogoImageResourceId = value;
                m_LogoImageResourceIdLoaded = false;
            }
        }

        /// <summary>
        /// Gets the URL of the logo image.
        /// </summary>
        public string LogoImageUrl
        {
            get { return (this.LogoImageResourceId.HasValue ? ResourceProvider.GetResourceUrl(this.LogoImageResourceId.Value, 300, 45, true) : null); }
        }

        /// <summary>
        /// Gets or sets a value indicating that the sign up user is enabled for this instance.
        /// </summary>
        public bool EnableSignupUser
        {
            get { return m_EnableSignupUser; }
            set { m_EnableSignupUser = value; }
        }

        /// <summary>
        /// Gets or sets the external id of the instance.
        /// </summary>
        public string ExternalId
        {
            get { return m_ExternalId; }
            set { m_ExternalId = value; }
        }

        /// <summary>
        /// Gets ot sets the time zone identifier.
        /// </summary>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets ot sets the time format.
        /// </summary>
        public int? TimeFormat { get; set; }

        /// <summary>
        /// Gets ot sets the date format.
        /// </summary>
        public int? DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the working days of the instance.
        /// </summary>
        public string WorkingDays
        {
            get { return m_WorkingDays; }
            set { m_WorkingDays = value; }
        }

        /// <summary>
        /// Gets or sets the active status of the instance.
        /// </summary>
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; }
        }

        /// <summary>
        /// Gets or sets the canceled time of the instance.
        /// </summary>
        public DateTime? CanceledTime
        {
            get { return m_CanceledTime; }
            set { m_CanceledTime = value; }
        }

        /// <summary>
        /// Gets or sets the trial status of the instance.
        /// </summary>
        public bool Trial
        {
            get { return m_Trial; }
            set { m_Trial = value; }
        }

        /// <summary>
        /// Gets or sets the beta status of the instance.
        /// </summary>
        public bool Beta
        {
            get { return m_Beta; }
            set { m_Beta = value; }
        }

        /// <summary>
        /// Gets or sets the created time of the instance.
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return m_CreatedTime; }
            set { m_CreatedTime = value; }
        }

        /// <summary>
        /// Gets the organization which this instance belong to.
        /// </summary>
        public Organization Organization
        {
            get
            {
                if (m_Organization == null)
                {
                    m_Organization = new Organization();
                    m_Organization.Load(OrganizationId);
                }
                return m_Organization;
            }
        }

        /// <summary>
        /// Gets or sets the email suffixes for the instance.
        /// </summary>
        public string EmailSuffixes
        {
            get
            {
                EnsureEmailSuffixesIsLoaded();
                return m_EmailSuffixes;
            }
            set
            {
                m_EmailSuffixes = value;
                m_EmailSuffixesList = null;
            }
        }

        /// <summary>
        /// Gets the list of email suffixes for the instance.
        /// </summary>
        public Collection<string> EmailSuffixesList
        {
            get
            {
                EnsureEmailSuffixesIsLoaded();
                return m_EmailSuffixesList;
            }
        }

        /// <summary>
        /// Gets a collection that contains the instance level settings.
        /// </summary>
        public SettingCollection Settings
        {
            get
            {
                if (m_Settings == null)
                {
                    lock (m_SettingsSyncRoot)
                    {
                        if (m_Settings == null)
                            m_Settings = SettingProvider.GetInstanceSettings(OrganizationId, InstanceId);
                    }
                }
                return m_Settings;
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(Instance instance1, Instance instance2)
        {
            if (object.ReferenceEquals(instance1, instance2))
                return true;

            if (((object)instance1 == null) || ((object)instance2 == null))
                return false;

            return instance1.Equals(instance2);
        }

        public static bool operator !=(Instance instance1, Instance instance2)
        {
            return (!(instance1 == instance2));
        }

        public static bool operator <(Instance instance1, Instance instance2)
        {
            return (instance1.CompareTo(instance2) < 0);
        }

        public static bool operator >(Instance instance1, Instance instance2)
        {
            return (instance1.CompareTo(instance2) > 0);
        }

        #endregion

        #region Private Methods

        private void EnsureEmailSuffixesIsLoaded()
        {
            if (m_EmailSuffixesList == null)
            {
                lock (m_EmailSuffixSyncRoot)
                {
                    if (m_EmailSuffixesList == null)
                    {
                        m_EmailSuffixesList = new Collection<string>();

                        if (m_EmailSuffixes == null)
                        {
                            DataTable table = EmailSuffixProvider.GetEmailSuffixesByInstanceId(this.InstanceId);
                            if (table.Rows.Count > 0)
                                m_EmailSuffixes = (string)table.Rows[0]["EmailSuffixName"];
                            else
                                m_EmailSuffixes = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(m_EmailSuffixes))
                        {
                            foreach (string suffix in m_EmailSuffixes.Split(','))
                            {
                                string val = suffix.Trim();
                                if (val.Length > 0) m_EmailSuffixesList.Add(val);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the data of specified instance into Micajah.Common.Bll.Instance class from CommonDataSet.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization that the instance belong to.</param>
        /// <param name="instanceId">The identifier of the instance to load.</param>
        /// <returns>true, if the specified instance is found; otherwise, false.</returns>
        internal bool Load(Guid organizationId, Guid instanceId)
        {
            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
            if (ds != null)
            {
                OrganizationDataSet.InstanceRow instanceRow = ds.Instance.FindByInstanceId(instanceId);
                if (instanceRow != null)
                {
                    Load(instanceRow);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads the data of specified instance into Micajah.Common.Bll.Instance class from CommonDataSet.
        /// </summary>
        /// <param name="row">The data row of the instance to load.</param>
        internal void Load(OrganizationDataSet.InstanceRow row)
        {
            if (row != null)
            {
                m_InstanceId = row.InstanceId;
                m_PseudoId = row.PseudoId;
                m_OrganizationId = row.OrganizationId;
                m_Name = row.Name;
                m_Description = row.Description;
                m_EnableSignupUser = row.EnableSignUpUser;

                m_ExternalId = row.ExternalId;
                this.TimeZoneId = (row.IsTimeZoneIdNull() ? null : row.TimeZoneId);
                this.TimeFormat = (row.IsTimeFormatNull() ? null : new int?(row.TimeFormat));
                this.DateFormat = (row.IsDateFormatNull() ? null : new int?(row.DateFormat));
                m_WorkingDays = row.WorkingDays;
                m_Active = row.Active;
                m_CanceledTime = (row.IsCanceledTimeNull() ? null : new DateTime?(row.CanceledTime));
                m_Trial = row.Trial;
                m_Beta = row.Beta;
                m_CreatedTime = (row.IsCreatedTimeNull() ? null : new DateTime?(row.CreatedTime));

                m_LogoImageResourceIdLoaded = false;
                m_LogoImageResourceId = null;
                m_Organization = null;
                m_Settings = null;
                m_GroupIdRoleIdList = null;
                m_EmailSuffixes = null;
                m_EmailSuffixesList = null;
            }
        }

        /// <summary>
        /// Refreshes the instance level settings.
        /// </summary>
        internal void Refresh()
        {
            m_Settings = null;
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            Instance instance = obj as Instance;
            if ((object)instance == null)
                return false;
            return (this.CompareTo(instance) == 0);
        }

        public override int GetHashCode()
        {
            return this.InstanceId.GetHashCode();
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <returns>A System.String that represents this object.</returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares the current instance with another.
        /// </summary>
        /// <param name="other">A instance to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the instances being compared.</returns>
        public int CompareTo(Instance other)
        {
            return (((object)other == null) ? 1 : string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <param name="delimiter">The string that delimit the properties/values in the string.</param>
        /// <returns>A System.String that represents this object.</returns>
        public string ToString(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("InstanceId={0}{1}", InstanceId, delimiter);
            sb.AppendFormat("PseudoId={0}{1}", PseudoId, delimiter);
            sb.AppendFormat("OrganizationId={0}{1}", OrganizationId, delimiter);
            sb.AppendFormat("Name={0}{1}", Name, delimiter);
            sb.AppendFormat("ExternalId={0}{1}", ExternalId, delimiter);
            sb.AppendFormat("Active={0}", Active);
            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// The collection of the instances.
    /// </summary>
    [Serializable]
    public sealed class InstanceCollection : List<Instance>
    {
        #region Private Methods

        private static int CompareByCreatedTime(Instance x, Instance y)
        {
            int result = 0;

            if (x == null)
            {
                result = ((y == null) ? 0 : -1);
            }
            else
            {
                if (y == null)
                    result = 1;
                else
                {
                    if (!x.CreatedTime.HasValue)
                        result = (y.CreatedTime.HasValue ? -1 : 0);
                    else
                    {
                        if (!y.CreatedTime.HasValue)
                            result = 1;
                        else
                            result = DateTime.Compare(x.CreatedTime.Value, y.CreatedTime.Value);
                    }
                }
            }
            return result;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Searches for an element that matches the specified identifier.
        /// </summary>
        /// <param name="instanceId">The identifier of the element to search for.</param>
        /// <returns>The first element that matches the specified identifier, if found; otherwise, the null reference.</returns>
        public Instance FindByInstanceId(Guid instanceId)
        {
            return this.Find(
                delegate(Instance instance)
                {
                    return instance.InstanceId == instanceId;
                });
        }

        public void SortByCreatedTime()
        {
            base.Sort(CompareByCreatedTime);
        }

        #endregion
    }
}
