using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using Micajah.Common.Configuration;

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
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);
                connection.Open();

                command = new SqlCommand("[dbo].[Mc_DeleteViewState]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Now", SqlDbType.DateTime).Value = DateTime.Now;
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
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

            Pair statePair = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;
            StreamReader sr = null;
            MemoryStream stream = null;

            try
            {
                if (viewStateId != Guid.Empty)
                {

                    connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);
                    connection.Open();

                    command = new SqlCommand("[dbo].[Mc_GetViewState]", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ViewStateId", SqlDbType.UniqueIdentifier).Value = viewStateId;

                    reader = command.ExecuteReader();
                    if (reader.Read()) stream = new MemoryStream(reader[0] as byte[]);

                    if (stream != null)
                    {
                        sr = new StreamReader(stream);
                        statePair = (stateFormatter.Deserialize(sr.ReadToEnd()) as Pair);
                    }
                }
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
                if (stream != null) stream.Dispose();
                if (reader != null) reader.Dispose();
            }

            return statePair;
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

            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                byte[] bytes = Support.GetBytes(stateFormatter.Serialize(statePair));

                connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString);
                connection.Open();

                command = new SqlCommand("[dbo].[Mc_InsertViewState]", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ViewStateId", SqlDbType.UniqueIdentifier).Value = viewStateId;
                command.Parameters.Add("@ViewState", SqlDbType.VarBinary).Value = bytes;
                command.Parameters.Add("@ExpirationTime", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(FrameworkConfiguration.Current.WebApplication.ViewStateExpirationTimeout);
                command.ExecuteNonQuery();
            }
            finally
            {
                if (connection != null) connection.Dispose();
                if (command != null) command.Dispose();
            }
        }

        #endregion
    }
}
