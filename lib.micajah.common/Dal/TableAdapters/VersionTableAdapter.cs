using System.Data;
using System.Globalization;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The adapter for the Mc_Version table.
    /// </summary>
    internal class VersionTableAdapter : BaseTableAdapter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the VersionTableAdapter class.
        /// </summary>
        public VersionTableAdapter()
        {
            #region TableMapping

            TableName = TableName.Version;
            TableMapping.ColumnMappings.Add("Version", "Version");

            #endregion

            #region SelectCommand

            SelectCommand.CommandText = "SELECT Version FROM dbo.Mc_Version";
            SelectCommand.CommandType = CommandType.Text;

            #endregion
        }

        #endregion

        #region Public Methods

        public static int GetVersion()
        {
            using (DataTable table = new DataTable())
            {
                table.Locale = CultureInfo.InvariantCulture;
                using (VersionTableAdapter ta = new VersionTableAdapter())
                {
                    ta.Fill(table);
                    if (table.Rows.Count > 0)
                        return (int)table.Rows[0][0];
                    return 0;
                }
            }
        }

        #endregion
    }
}
