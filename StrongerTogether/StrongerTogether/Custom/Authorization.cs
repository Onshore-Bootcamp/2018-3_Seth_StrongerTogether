using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrongerTogetherDAL.Models;
using StrongerTogetherDAL.Mapping;
using StrongerTogetherDAL;

namespace StrongerTogether.Custom
{
    public class Authorization : ActionFilterAttribute
    {  
        // creating the attributes
        private readonly string _Key;
        private readonly long[] _AllowedValues;
        private readonly string _DirectPath;
        //passing in the attributes and creating variables
        public Authorization(string Key, string Redirect, params long[] AllowedValues)
        {
            _Key = Key;
            _DirectPath = Redirect;
            _AllowedValues = AllowedValues;
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {   
            // checking session
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            //role id
            long role = 0;
            // if not null
            if (session[_Key] != null)
            {   
                // they have the correct role
                long.TryParse(session[_Key].ToString(), out role);
            }
                // if allowed valuse does not contain role
            if (!_AllowedValues.Contains(role))
            {   
                // redirect somewhere else
                filterContext.Result = new RedirectResult(_DirectPath, false);
            }
            //on executing
            base.OnActionExecuting(filterContext);
        }

    }
}