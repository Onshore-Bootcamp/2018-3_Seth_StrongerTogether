using StrongerTogetherDAL.Mapping;
using StrongerTogetherDAL.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace StrongerTogetherDAL
{
    public class SupportLinksDAO
    {
        // calls on config
        private static Logger logger;
        private static SupportLinksMapperDO mapper = new SupportLinksMapperDO();

        // config
        private readonly string ConnectionString;

        // Connection for sql and logger
        public SupportLinksDAO(string connectionString, string logpath)
        {
            ConnectionString = connectionString;
            logger = new Logger(logpath);
        }

        public SupportLinksDO ViewSupportLinksById(long supportId)
        {
            // instatiate support
            SupportLinksDO support = new SupportLinksDO();

            try
            {
                // sql connection
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("View_Support_Links_By_Id", connection))
                {
                    // sql commands
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    // paramaters
                    command.Parameters.AddWithValue("@SupportId", supportId);

                    // open connection
                    connection.Open();

                    // mapping from sql
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        if (reader.Read())
                        {
                            //info from sql
                            support.SupportId = reader["SupportId"] != DBNull.Value ? (long)reader["SupportId"] : 0;
                            support.Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : null;
                            support.Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : null;
                            support.Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : null;
                            support.Url = reader["Url"] != DBNull.Value ? (string)reader["Url"] : null;
                            support.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
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
            return support;
        }

        
        public void CreateSupportLinks(SupportLinksDO createSupportLinks)
        {

            try
            {
                // calling on sql
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand("Create_Support_Links", connection))
                {
                    // calls on stored procedures
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    // params from sql
                    command.Parameters.AddWithValue("@Name", createSupportLinks.Name);
                    command.Parameters.AddWithValue("@Address", createSupportLinks.Address);
                    command.Parameters.AddWithValue("@Phone", createSupportLinks.Phone);
                    command.Parameters.AddWithValue("@Url", createSupportLinks.Url);
                    command.Parameters.AddWithValue("@UserId", createSupportLinks.UserId);

                    // executes sql
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        
        public void UpdateSupportLinks(SupportLinksDO supportLinks)
        {

            try
            {
                // sql connection
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand updateSupportLinks = new SqlCommand("Update_Support_Links", connectionStrongerTogether))
                {
                    // stored procedures
                    updateSupportLinks.CommandType = CommandType.StoredProcedure;
                    updateSupportLinks.CommandTimeout = 60;

                    // stroed paramaters
                    updateSupportLinks.Parameters.AddWithValue("@SupportId", supportLinks.SupportId);
                    updateSupportLinks.Parameters.AddWithValue("@Name", supportLinks.Name);
                    updateSupportLinks.Parameters.AddWithValue("@Address", supportLinks.Address);
                    updateSupportLinks.Parameters.AddWithValue("@Phone", supportLinks.Phone);
                    updateSupportLinks.Parameters.AddWithValue("@Url", supportLinks.Url);
                    updateSupportLinks.Parameters.AddWithValue("@UserId", supportLinks.UserId);

                    // executes
                    connectionStrongerTogether.Open();
                    updateSupportLinks.ExecuteNonQuery();
                    connectionStrongerTogether.Close();
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        
        public void DeleteSupportLinks(long supportId)
        {
            try
            {
                // sql connection
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand deleteSupportLinks = new SqlCommand("Delete_Support_Links", connectionStrongerTogether))
                {
                    // calls on stored porcedures
                    deleteSupportLinks.CommandType = CommandType.StoredProcedure;
                    deleteSupportLinks.CommandTimeout = 60;
                    deleteSupportLinks.Parameters.AddWithValue("@SupportId", supportId);

                    //executes
                    connectionStrongerTogether.Open();
                    deleteSupportLinks.ExecuteNonQuery();
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        
        public List<SupportLinksDO> ViewAllSupportLinks()
        {
            // instatiates support links
            List<SupportLinksDO> displaySupportLinks = new List<SupportLinksDO>();

            try
            {
                // sql connection
                using (SqlConnection connectionStrongerTogether = new SqlConnection(ConnectionString))
                using (SqlCommand viewSupportLinks = new SqlCommand("View_All_Support_Links", connectionStrongerTogether))
                {
                    // stored procedures
                    viewSupportLinks.CommandType = CommandType.StoredProcedure;
                    connectionStrongerTogether.Open();

                    // view and execute from sql
                    using (SqlDataReader sqlDataReader = viewSupportLinks.ExecuteReader())
                    {

                        
                        while (sqlDataReader.Read())
                        {
                            // mapping the links
                            SupportLinksDO supportLinks = mapper.MapReaderToSingle(sqlDataReader);
                            displaySupportLinks.Add(supportLinks);
                        }
                    }

                    // close connection
                    connectionStrongerTogether.Close();
                }
            }
            catch (SqlException sqlex)
            {
                // log sql exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                // log other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return displaySupportLinks;
        }
    }
}
