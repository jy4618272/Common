using Micajah.Common.Dal.TableAdapters;

namespace Micajah.Common.Dal
{
    public partial class ConfigurationDataSet
    {
        #region Members

        private static object s_ConfigurationDataSetSyncRoot = new object();

        private static ConfigurationDataSet s_Current;

        #endregion

        #region Public Properties

        public static ConfigurationDataSet Current
        {
            get
            {
                if (s_Current == null)
                {
                    lock (s_ConfigurationDataSetSyncRoot)
                    {
                        if (s_Current == null)
                        {
                            ConfigurationDataSet ds = new ConfigurationDataSet();
                            MasterTableAdapters.Current.Fill(ds);
                            s_Current = ds;
                        }
                    }
                }
                return s_Current;
            }
        }

        #endregion
    }
}
