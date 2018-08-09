using StrongerTogetherDAL.Models;
using System;
using System.Data.SqlClient;

namespace StrongerTogetherDAL.Mapping
{
    public class SupportLinksMapperDO
    {
        // Make appropriate comment
        public SupportLinksDO MapReaderToSingle(SqlDataReader reader)
        {
            SupportLinksDO result = new SupportLinksDO();

            // Make appropriate comment - Overview this section here
            if (reader["SupportId"] != DBNull.Value)
            {
                result.SupportId = (long)reader["SupportId"];
            }
            if (reader["Name"] != DBNull.Value)
            {
                result.Name = (string)reader["Name"];
            }
            if (reader["Address"] != DBNull.Value)
            {
                result.Address = (string)reader["Address"];
            }
            if (reader["Phone"] != DBNull.Value)
            {
                result.Phone = (string)reader["Phone"];
            }
            if (reader["Url"] != DBNull.Value)
            {
                result.Url = (string)reader["Url"];
            }
            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
            return result;
        }
    }
}
