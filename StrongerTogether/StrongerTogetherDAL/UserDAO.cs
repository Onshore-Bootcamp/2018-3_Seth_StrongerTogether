using StrongerTogetherDAL.Mapping;
using StrongerTogetherDAL.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace StrongerTogetherDAL
{
    public class UserDAO
    {
        // calling on and creating variables
        private static Logger logger;
        private static UserMapperDO Mapper = new UserMapperDO();

        // calling on the connection string
        private readonly string ConnectionString;

        // using the conection and logpath
        public UserDAO(string connectionString, string logpath)
        {
            // Make appropriate comment
            ConnectionString = connectionString;
            logger = new Logger(logpath);
        }

        public UserDO ViewUsersById(long userId)
        {
            // instantiate the variable
            UserDO user = new UserDO();

            try
            {
                // call on the sql connection
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("View_Users_By_ID", connection))
                {
                    // pull the stored procedures and set a timeout
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    // paramaters
                    command.Parameters.AddWithValue("@UserId", userId);

                    // open connection
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // if the reader executes and reads each reccord
                        if (reader.Read())
                        {
                            // map the reccord
                            user = Mapper.MapReaderToSingle(reader);
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
            return user;
        }

        /// <summary>
        /// viewing user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns> the users info </returns>
        public UserDO ViewUsersByUsername(string username)
        {
            // instatiate user
            UserDO user = new UserDO();

            try
            {
                // calling on sql
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("View_Users_By_Username", connection))
                {
                    // calling on stored procedures
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    // calling on the paramaters
                    command.Parameters.AddWithValue("@Username", username);

                    // open the connection
                    connection.Open();
                    // execute the command
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // if the reader reads the record
                        if (reader.Read())
                        {
                            // calling on the paramaters
                            user.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
                            user.Username = reader["Username"] != DBNull.Value ? (string)reader["Username"] : null;
                            user.Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : null;
                            user.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null;
                            user.Bio = reader["Bio"] != DBNull.Value ? (string)reader["Bio"] : null;
                            user.RoleId = reader["Role"] != DBNull.Value ? (long)reader["Role"] : 0;
                            user.RoleName = reader["RoleName"] != DBNull.Value ? (string)reader["RoleName"] : null;
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
            return user;
        }

        // creating a user
        public void CreateUser(UserDO createUser)
        {
            // calling on sql
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand("Create_User", connection))
            {
                //calling on the stored procedures
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;
                // calling on the paramaters
                command.Parameters.AddWithValue("@Username", createUser.Username);
                command.Parameters.AddWithValue("@Password", createUser.Password);
                command.Parameters.AddWithValue("@Email", createUser.Email);
                command.Parameters.AddWithValue("@Bio", createUser.Bio);
                command.Parameters.AddWithValue("@RoleId", createUser.RoleId);
                // open the connection then execute and close connection
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        /// <summary>
        /// updating the user
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(UserDO user)
        {
            try
            {
                // calling on sql
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand updateUser = new SqlCommand("Update_User", connectionStrongerTogether))
                {
                    // calling on the stored procedures and checking for timeout
                    updateUser.CommandType = CommandType.StoredProcedure;
                    updateUser.CommandTimeout = 60;

                    // calling on the paramaters
                    updateUser.Parameters.AddWithValue("@UserId", user.UserId);
                    updateUser.Parameters.AddWithValue("@Username", user.Username);
                    updateUser.Parameters.AddWithValue("@Password", user.Password);
                    updateUser.Parameters.AddWithValue("@Email", user.Email);
                    updateUser.Parameters.AddWithValue("@Bio", user.Bio);
                    updateUser.Parameters.AddWithValue("@RoleId", user.RoleId);

                    // open the connection then execute and close connectiion
                    connectionStrongerTogether.Open();
                    updateUser.ExecuteNonQuery();
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

        }

        /// <summary>
        /// delete user by id
        /// </summary>
        /// <param name="userID"></param>
        public void DeleteUser(long userID)
        {
            try
            {
                // calling on sql
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand deleteUser = new SqlCommand("Delete_User", connectionStrongerTogether))
                {
                    // calling on the stored procedures
                    deleteUser.CommandType = CommandType.StoredProcedure;
                    deleteUser.CommandTimeout = 60;
                    // checking paramater
                    deleteUser.Parameters.AddWithValue("@UserId", userID);

                    // open and execute 
                    connectionStrongerTogether.Open();
                    deleteUser.ExecuteNonQuery();
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
        /// view and dispaly users
        /// </summary>
        /// <returns></returns>
        public List<UserDO> ViewAllUsers()
        {
            // instantiate the display users
            List<UserDO> displayUsers = new List<UserDO>();

            try
            {
                // calling on sql
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand viewUsers = new SqlCommand("View_All_Users", connectionStrongerTogether))
                {
                    // calling on stored procedures
                    viewUsers.CommandType = CommandType.StoredProcedure;
                    connectionStrongerTogether.Open();

                    // executing sql data reader
                    using (SqlDataReader sqlDataReader = viewUsers.ExecuteReader())
                    {
                        //while its reading the sql records
                        while (sqlDataReader.Read())
                        {
                            // map and display users
                            UserDO user = Mapper.MapReaderToSingle(sqlDataReader);
                            displayUsers.Add(user);
                        }
                    }
                    // close sql connection
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
            return displayUsers;
        }
    }
}
