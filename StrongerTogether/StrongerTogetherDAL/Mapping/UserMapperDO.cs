using StrongerTogetherDAL.Models;
using System;
using System.Data.SqlClient;

namespace StrongerTogetherDAL.Mapping
{
    public class UserMapperDO
    {
        // Make appropriate comment
        public UserDO MapReaderToSingle(SqlDataReader reader)
        {
            UserDO result = new UserDO();

            // Make appropriate comment - Overview of this
            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
            if (reader["Username"] != DBNull.Value)
            {
                result.Username = (string)reader["Username"];
            }
            if (reader["Password"] != DBNull.Value)
            {
                result.Password = (string)reader["Password"];
            }
            if (reader["Email"] != DBNull.Value)
            {
                result.Email= (string)reader["Email"];
            }
            if (reader["Bio"] != DBNull.Value)
            {
                result.Bio = (string)reader["Bio"];
            }
            if (reader["Role"] != DBNull.Value)
            {
                result.RoleId = (long)reader["Role"];
            }
            if (reader["RoleName"] != DBNull.Value)
            {
                result.RoleName = (string)reader["RoleName"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                result.Description = (string)reader["Description"];
            }

            return result;
        }
    }
}
