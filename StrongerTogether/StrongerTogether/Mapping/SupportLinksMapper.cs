using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using StrongerTogether.Models;
using StrongerTogetherDAL.Models;

namespace StrongerTogether.Mapping
{
    public class SupportLinksMapper
    {
        /// <summary>
        /// DO to PO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> sends information out</returns>
        public SupportLinks MapDoToPo(SupportLinksDO from)
        {
            SupportLinks to = new SupportLinks();
            to.SupportId = from.SupportId;
            to.Name = from.Name;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.Url = from.Url;
            to.UserId = from.UserId;

            return to;
        }

        /// <summary>
        ///  mapping the PO to DO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> send it out</returns>
        public SupportLinksDO MapPoToDo(SupportLinks from)
        {
            SupportLinksDO to = new SupportLinksDO();
            to.SupportId = from.SupportId;
            to.Name = from.Name;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.Url = from.Url;
            to.UserId = from.UserId;

            return to;
        }
    }
}