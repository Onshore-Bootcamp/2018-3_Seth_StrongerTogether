using StrongerTogether.Models;
using StrongerTogetherBLL.Model;
using StrongerTogetherDAL.Models;

namespace StrongerTogether.Mapping
{
    public class WrittingsMapper
    {
        /// <summary>
        /// DO to PO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> out information</returns>
        public Writtings MapDoToPo(WrittingsDO from)
        {
            Writtings to = new Writtings();
            to.WrittingId = from.WrittingId;
            to.Username = from.Username;
            to.Title = from.Title;
            to.DatePublished = from.DatePublished;
            // creates a summory of the content
            to.Content = from.Content;
            if (from.Content.Length > 64)
            {
                to.Blurb = from.Content.Substring(0, 64) + "...";
            } 
            else
            {
                to.Blurb = from.Content;
            }
            to.UserId = from.UserId;
            to.WordCount = from.WordCount;
            
            return to;
        }

        /// <summary>
        /// PO to DO
        /// </summary>
        /// <param name="from"></param>
        /// <returns>information out</returns>
        public WrittingsDO MapPoToDo(Writtings from)
        {
            WrittingsDO to = new WrittingsDO();
            to.WrittingId = from.WrittingId;
            to.Username = from.Username;
            to.Title = from.Title;
            to.DatePublished = from.DatePublished;
            to.Content = from.Content;
            to.UserId = from.UserId;
            to.WordCount = from.WordCount;
            
            return to;
        }
        /// <summary>
        /// DO to BO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> mapped info </returns>
        public WrittingsBO MapDoToBo(WrittingsDO from)
        {
            WrittingsBO to = new WrittingsBO();
            to.WrittingId = from.WrittingId;
            to.Username = from.Username;
            to.Title = from.Title;
            to.DatePublished = from.DatePublished;
            to.Content = from.Content;
            to.UserId = from.UserId;
            // number of words
            to.WordCount = from.WordCount;

            return to;
        }
        /// <summary>
        /// BO to DO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> mapped the info </returns>
        public WrittingsDO MapBoToDo(WrittingsBO from)
        {
            WrittingsDO to = new WrittingsDO();
            to.WrittingId = from.WrittingId;
            to.Username = from.Username;
            to.Title = from.Title;
            to.DatePublished = from.DatePublished;
            to.Content = from.Content;
            to.UserId = from.UserId;
            // number of words
            to.WordCount = from.WordCount;

            return to;
        }
    }
}