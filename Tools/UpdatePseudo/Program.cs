using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace UpdatePseudo
{
    class Program
    {
        private static string SelectCommandText
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture
                    , "SELECT {3} {0}, {1} FROM {2} WHERE LEN(ISNULL({0}, '')) = 0"
                    , Properties.Settings.Default.TableColumnName
                    , Properties.Settings.Default.TablePrimaryKeyColumnName
                    , Properties.Settings.Default.TableName
                    , ((Properties.Settings.Default.TopRowsCount > 0) ? "TOP " + Properties.Settings.Default.TopRowsCount.ToString(CultureInfo.InvariantCulture) : string.Empty));
            }
        }

        private static string UpdateCommandText
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture
                    , "UPDATE {0} SET {1} = @P1 WHERE {2} = @P2"
                    , Properties.Settings.Default.TableName, Properties.Settings.Default.TableColumnName, Properties.Settings.Default.TablePrimaryKeyColumnName);
            }
        }

        private static DataTable Table
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add(Properties.Settings.Default.TableColumnName);
                table.Columns.Add(Properties.Settings.Default.TablePrimaryKeyColumnName);
                table.PrimaryKey = new DataColumn[] { table.Columns[1] };
                return table;
            }
        }

        private static SqlCommand CreateUpdateCommand(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand(UpdateCommandText, connection);

            SqlParameter par = new SqlParameter();
            par.ParameterName = "@P1";
            par.SourceColumn = Properties.Settings.Default.TableColumnName;
            updateCmd.Parameters.Add(par);

            par = new SqlParameter();
            par.ParameterName = "@P2";
            par.SourceColumn = Properties.Settings.Default.TablePrimaryKeyColumnName;
            updateCmd.Parameters.Add(par);

            return updateCmd;
        }

        /// <summary>
        /// Generates the pseudo unique identifier.
        /// </summary>
        /// <returns>An System.String object that represents the pseudo unique identifier.</returns>
        public static string GenereatePseudoUnique()
        {
            //"abcdefghijkmnopqrstuvwxyz0123456789"
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] pass_bytes = new byte[6];
            rng.GetBytes(pass_bytes);

            for (int i = 0; i < 6; i++)
            {
                // Convert the random bytes to ascii values 33-126
                pass_bytes[i] = (byte)(pass_bytes[i] % 93 + 33);

                if (pass_bytes[i] > 32 && pass_bytes[i] < 48)
                    pass_bytes[i] += 64;
                else if (pass_bytes[i] > 57 && pass_bytes[i] < 65)
                    pass_bytes[i] -= 10;
                else if (pass_bytes[i] > 64 && pass_bytes[i] < 91)
                    pass_bytes[i] += 32;
                else if (pass_bytes[i] > 90 && pass_bytes[i] < 97)
                    pass_bytes[i] += 11;
                else if (pass_bytes[i] > 122)
                    pass_bytes[i] -= 15;

                if (pass_bytes[i] == 108)
                    pass_bytes[i]++;
            }

            return Encoding.ASCII.GetString(pass_bytes);
        }

        static void Main(string[] args)
        {
            SqlConnection conn = null;
            SqlDataAdapter adapt = null;
            SqlCommand selectCmd = null;
            SqlCommand updateCmd = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString);

                Console.WriteLine("DB Server: {0}", conn.DataSource);
                Console.WriteLine("DB Name: {0}", conn.Database);
                Console.WriteLine("DB Table Name: {0}", Properties.Settings.Default.TableName);
                Console.WriteLine("DB Table Column Name: {0}", Properties.Settings.Default.TableColumnName);
                Console.WriteLine("DB Table Primary Key: {0}\r\n", Properties.Settings.Default.TablePrimaryKeyColumnName);

                selectCmd = new SqlCommand(SelectCommandText, conn);
                adapt = new SqlDataAdapter(selectCmd);
                updateCmd = CreateUpdateCommand(conn);
                adapt.UpdateCommand = updateCmd;
                DataTable table = Table;

                int count = 1;
                while (count > 0)
                {
                    if (Properties.Settings.Default.TopRowsCount == 0)
                        Console.Write("Selecting all the rows with null or empty value of the {0} column... ", Properties.Settings.Default.TableColumnName);
                    else
                        Console.Write("Selecting {1} rows with null or empty value of the {0} column... ", Properties.Settings.Default.TableColumnName, Properties.Settings.Default.TopRowsCount.ToString(CultureInfo.InvariantCulture));

                    adapt.Fill(table);

                    count = table.Rows.Count;
                    if (count > 0)
                    {
                        Console.Write("Done. ");

                        if ((Properties.Settings.Default.TopRowsCount == 0) || (Properties.Settings.Default.TopRowsCount != count))
                            Console.Write("Rows found: {0}", count);

                        Console.Write("\r\nUpdating...");

                        foreach (DataRow row in table.Rows)
                        {
                            row[Properties.Settings.Default.TableColumnName] = GenereatePseudoUnique();
                        }

                        adapt.Update(table);

                        Console.WriteLine("Done.");

                        if ((Properties.Settings.Default.TopRowsCount == 0) || (Properties.Settings.Default.TopRowsCount != count))
                            break;

                        table.Clear();
                    }
                    else
                        Console.WriteLine("No rows found.");
                }

                Console.WriteLine("\r\nFinished!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\r\nAn error occured: {0}\r\n", ex.ToString());
                Console.WriteLine("Process failed!");
            }
            finally
            {
                if (updateCmd != null) updateCmd.Dispose();
                if (selectCmd != null) selectCmd.Dispose();
                if (adapt != null) adapt.Dispose();
                if (conn != null) conn.Dispose();
            }

            Console.ReadKey();
        }
    }
}
