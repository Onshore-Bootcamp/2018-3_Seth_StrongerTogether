using StrongerTogetherDAL.Mapping;
using StrongerTogetherDAL.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace StrongerTogetherDAL
{
    public class WrittingsDAO
    {
        // instatiating logger and mapper
        private static Logger logger;
        private static WrittingsMapperDO Mapper = new WrittingsMapperDO();
        
        private readonly string ConnectionString;
        // creates and initalize the variable
        public WrittingsDAO(string connectionString, string logpath)
        {
            ConnectionString = connectionString;
            logger = new Logger(logpath);
        }
        /// <summary>
        /// view by writting id
        /// </summary>
        /// <param name="writtingId"></param>
        /// <returns> the completed writtings </returns>
        public WrittingsDO ViewWrittingsById(long writtingId)
        {
            // instatiating writtingsDO
            WrittingsDO writtings = new WrittingsDO();

            try
            {   // instantiating sql connection and command
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("View_Writtings_By_ID", connection))
                {
                    // calling on the stored procedures and setting a timeout value
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                    // adding value to the paramater WrittingId
                    command.Parameters.AddWithValue("@WrittingId", writtingId);
                    // opening sql connection
                    connection.Open();
                    // Execute the command and pull the data from sql
                    using (SqlDataReader reader = command.ExecuteReader())
                    {   
                        // reads entries from database
                        if (reader.Read())
                        {
                            // pulls the info from the database and maps it out
                            writtings.WrittingId = reader["WrittingId"] != DBNull.Value ? (long)reader["WrittingId"] : 0;
                            writtings.Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : null;
                            writtings.DatePublished = reader["DatePublished"] != DBNull.Value ? (DateTime)reader["DatePublished"] : DateTime.MinValue;
                            writtings.Content = reader["Content"] != DBNull.Value ? (string)reader["Content"] : null;
                            writtings.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
                            writtings.WordCount = reader["WordCount"] != DBNull.Value ? (int)reader["WordCount"] : 0;
                        }


                    }
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return writtings;
        }
        /// <summary>
        /// viewing writtings by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns> writtings </returns>
        public WrittingsDO ViewWrittingsByUsername(string username)
        {   // instantiatie writtings
            WrittingsDO writtings = new WrittingsDO();

            try
            {   // instantiating sql connection and command
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("View_Writtings_By_Username", connection))
                {   
                    //calling on stored procedures and setting timeout length
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                    // adding value for the stored paramaters
                    command.Parameters.AddWithValue("@Username", username);
                    // open conection
                    connection.Open();
                    // Execute the command and pull the data from sql
                    using (SqlDataReader reader = command.ExecuteReader())
                    {   
                        //read entries from the database
                        if (reader.Read())
                        {
                            // pulling the info from the database and mapping it out
                            writtings.WrittingId = reader["WrittingId"] != DBNull.Value ? (long)reader["WrittingId"] : 0;
                            writtings.Username = reader["Username"] != DBNull.Value ? (string)reader["Username"] : null;
                            writtings.Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : null;
                            writtings.DatePublished = reader["DatePublished"] != DBNull.Value ? (DateTime)reader["DatePublished"] : DateTime.MinValue;
                            writtings.Content = reader["Content"] != DBNull.Value ? (string)reader["Content"] : null;

                        }
                    }
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return writtings;
        }
        /// <summary>
        /// creating a writting
        /// </summary>
        /// <param name="createWritting"></param>
        public void CreateWrittings(WrittingsDO createWritting)
        {
            // instantiating the sql connection and command
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand("Create_Writtings", connection))
            {   
                // calling on stored procedures and setting the timeout length
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;
                // adding value for the stored paramaters
                command.Parameters.AddWithValue("@Title", createWritting.Title);
                command.Parameters.AddWithValue("@DatePublished", createWritting.DatePublished);
                command.Parameters.AddWithValue("@Content", createWritting.Content);
                command.Parameters.AddWithValue("@UserId", createWritting.UserId);
                command.Parameters.AddWithValue("@WordCount", createWritting.WordCount);
                // opening conection executing and closing the connection
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }
        /// <summary>
        /// updating the users writting
        /// </summary>
        /// <param name="writtings"></param>
        public void UpdateWrittings(WrittingsDO writtings)
        {

            try
            {
                // connectiong to sql
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand updateWrittings = new SqlCommand("Update_Writtings", connectionStrongerTogether))
                {
                    // calling on the stored procedures and setting the timeout
                    updateWrittings.CommandType = CommandType.StoredProcedure;
                    updateWrittings.CommandTimeout = 60;
                    // adding value for the stored paramaters
                    updateWrittings.Parameters.AddWithValue("@WrittingId", writtings.WrittingId);
                    updateWrittings.Parameters.AddWithValue("@Title", writtings.Title);
                    updateWrittings.Parameters.AddWithValue("@Content", writtings.Content);
                    updateWrittings.Parameters.AddWithValue("@UserId", writtings.UserId);
                    updateWrittings.Parameters.AddWithValue("@WordCount", writtings.WordCount);
                    // open the connection and execute
                    connectionStrongerTogether.Open();
                    updateWrittings.ExecuteNonQuery();

                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }

        }
        /// <summary>
        /// delete by writting id
        /// </summary>
        /// <param name="writtingID"></param>
        public void DeleteWritting(long writtingID)
        {
            try
            {   
                // instantiate sql connection and command
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand deleteWritting = new SqlCommand("Delete_Writting", connectionStrongerTogether))
                {   // calling on the stored procedures and setting the timeout
                    deleteWritting.CommandType = CommandType.StoredProcedure;
                    deleteWritting.CommandTimeout = 60;
                    // Adding value for the stored paramaters
                    deleteWritting.Parameters.AddWithValue("@WrittingId", writtingID);
                    // open connection and execute
                    connectionStrongerTogether.Open();
                    deleteWritting.ExecuteNonQuery();
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        /// <summary>
        /// view all writtings
        /// </summary>
        /// <returns> writtings </returns>
        public List<WrittingsDO> ViewAllWrittings()
        {   
            // instantiate writtings
            List<WrittingsDO> returnWrittings = new List<WrittingsDO>();

            try
            {
                // instantiate sql connection and command
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand viewWrittings = new SqlCommand("View_All_Writtings", connectionStrongerTogether))
                {
                    // calling on stored procedures with default 30sec timeout
                    viewWrittings.CommandType = CommandType.StoredProcedure;

                    //opening the connection
                    connectionStrongerTogether.Open();

                    // Execute the command and pull the data from sql
                    using (SqlDataReader sqlDataReader = viewWrittings.ExecuteReader())
                    {
                        //read entries from the database
                        while (sqlDataReader.Read())
                        {  
                            // map each entry and adding to the return list
                            WrittingsDO writting = Mapper.MapReaderToSingle(sqlDataReader);
                            returnWrittings.Add(writting);
                        }
                    }
                    // attempt to close the connection
                    connectionStrongerTogether.Close();
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return returnWrittings;
        }
    }
}
