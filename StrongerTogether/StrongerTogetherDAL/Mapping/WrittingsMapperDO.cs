using StrongerTogetherDAL.Models;
using System;
using System.Data.SqlClient;

namespace StrongerTogetherDAL.Mapping
{
    public class WrittingsMapperDO
    {
        // Make appropriate comment
        public WrittingsDO MapReaderToSingle(SqlDataReader reader)
        {
            WrittingsDO result = new WrittingsDO();

            // Make appropriate comment - Overview of this
            if (reader["WrittingId"] != DBNull.Value)
            {
                result.WrittingId = (long)reader["WrittingId"];
            }
            if (reader["Username"] != DBNull.Value)
            {
                result.Username = (string)reader["Username"];
            }
            if (reader["Title"] != DBNull.Value)
            {
                result.Title = (string)reader["Title"];
            }
            if (reader["DatePublished"] != DBNull.Value)
            {
                result.DatePublished = (DateTime)reader["DatePublished"];
            }
            if (reader["Content"] != DBNull.Value)
            {
                result.Content = (string)reader["Content"];
            }
            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
            if (reader["WordCount"] != DBNull.Value)
            {
                result.WordCount = (int)reader["WordCount"];
            }
            return result;
        }
    }
}
