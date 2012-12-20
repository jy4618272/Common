namespace Micajah.Common.Bll.Handlers
{
    /// <summary>
    /// The class that provides a mechanism to get the name and description of the settings.
    /// </summary>
    public class SettingHandler
    {
        #region Members

        private static SettingHandler s_Instance;
        private static SettingHandler s_Current;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the instance of this class.
        /// </summary>
        internal static SettingHandler Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new SettingHandler();
                return s_Instance;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the instance of the class.
        /// </summary>
        public static SettingHandler Current
        {
            get { return ((s_Current == null) ? Instance : s_Current); }
            set { s_Current = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the name of the setting.
        /// </summary>
        /// <param name="setting">The setting to get name of.</param>
        /// <returns>The System.String that represents the name of the setting.</returns>
        public virtual string GetName(Setting setting)
        {
            if (setting == null)
                return null;
            return setting.Name;
        }

        /// <summary>
        /// Returns the description of the setting.
        /// </summary>
        /// <param name="setting">The setting to get decription of.</param>
        /// <returns>The System.String that represents the description of the setting.</returns>
        public virtual string GetDescription(Setting setting)
        {
            if (setting == null)
                return null;
            return setting.Description;
        }

        /// <summary>
        /// Returns the used items count.
        /// </summary>
        /// <param name="setting">The setting to get decription of.</param>
        /// <returns>The System.String that represents the description of the setting.</returns>
        public virtual int GetUsedItemsCount(Setting setting, System.Guid OrganizationId, System.Guid InstanceId)
        {
            return -1;
        }


        #endregion
    }
}
