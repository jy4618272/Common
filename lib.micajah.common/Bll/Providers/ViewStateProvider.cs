using Micajah.Common.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with view state.
    /// </summary>
    public static class ViewStateProvider
    {
        #region Public Methods

        /// <summary>
        /// Deletes the view and control states which are expired.
        /// </summary>
        public static void DeleteExpiredViewState()
        {
            using (SqlConnection connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[Mc_DeleteViewState]", connection))
                {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ViewState.ConnectionString);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Now", SqlDbType.DateTime).Value = DateTime.UtcNow;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Loads the view and control states.
        /// </summary>
        /// <param name="viewStateId">The identifier of the state.</param>
        /// <param name="stateFormatter">An instance of System.Web.UI.IStateFormatter that is used to serialize and deserialize object state.</param>
        /// <returns>The object that stores the view and control states.</returns>
        public static Pair GetViewState(Guid viewStateId, IStateFormatter stateFormatter)
        {
            if (stateFormatter == null) return null;

            if (viewStateId == Guid.Empty) return null;

            using (SqlConnection connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[Mc_GetViewState]", connection))
                {
                    connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ViewState.ConnectionString);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ViewStateId", SqlDbType.UniqueIdentifier).Value = viewStateId;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Pair statePair = null;
                        StreamReader sr = null;
                        MemoryStream stream = null;

                        if (reader.Read()) stream = new MemoryStream(reader[0] as byte[]);

                        if (stream != null)
                        {
                            sr = new StreamReader(stream);
                            statePair = (stateFormatter.Deserialize(sr.ReadToEnd()) as Pair);
                        }
                        connection.Close();
                        return statePair;
                    }
                }
            }
        }

        /// <summary>
        /// Stores the view and control states.
        /// </summary>
        /// <param name="viewStateId">The identifier of the state.</param>
        /// <param name="stateFormatter">An instance of System.Web.UI.IStateFormatter that is used to serialize and deserialize object state.</param>
        /// <param name="statePair">The object that stores the view and control states.</param>
        public static void InsertViewState(Guid viewStateId, IStateFormatter stateFormatter, Pair statePair)
        {
            if (stateFormatter == null) return;

            byte[] bytes = Support.GetBytes(stateFormatter.Serialize(statePair));

                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ViewState.ConnectionString);
            {
                using (SqlCommand command = new SqlCommand("[dbo].[Mc_InsertViewState]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ViewStateId", SqlDbType.UniqueIdentifier).Value = viewStateId;
                    command.Parameters.Add("@ViewState", SqlDbType.VarBinary).Value = bytes;
                command.Parameters.Add("@ExpirationTime", SqlDbType.DateTime).Value = DateTime.UtcNow.AddMinutes(FrameworkConfiguration.Current.WebApplication.ViewState.ExpirationTimeout);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        #endregion
    }
}
