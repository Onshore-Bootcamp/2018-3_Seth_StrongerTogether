using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StrongerTogether.Models;
using StrongerTogetherDAL.Models;

namespace StrongerTogether.Mapping
{
    public class UserMapper
    {
        /// <summary>
        /// DO to PO
        /// </summary>
        /// <param name="from"></param>
        /// <returns> sends info out</returns>
        public User MapDoToPo(UserDO from)
        {
            User to = new User();
            to.UserId = from.UserId;
            to.Username = from.Username;
            to.Password = from.Password;
            to.Email = from.Email;
            to.Bio = from.Bio;
            to.RoleId = from.RoleId;
            to.RoleName = from.RoleName;
            to.Description = from.Description;

            return to;
        }

        /// <summary>
        /// PO to DO
        /// </summary>
        /// <param name="from"></param>
        /// <returns>mapps information</returns>
        public UserDO MapPoToDo(User from)
        {
            UserDO to = new UserDO();
            to.UserId = from.UserId;
            to.Username = from.Username;
            to.Password = from.Password;
            to.Email = from.Email;
            to.Bio = from.Bio;
            to.RoleId = from.RoleId;
            to.RoleName = from.RoleName;
            to.Description = from.Description;

            return to;
        }
    }
}