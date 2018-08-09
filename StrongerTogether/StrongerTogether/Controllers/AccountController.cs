using StrongerTogether.Custom;
using StrongerTogether.Mapping;
using StrongerTogether.Models;
using StrongerTogetherDAL;
using StrongerTogetherDAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Mvc;

namespace StrongerTogether.Controllers
{
    public class AccountController : Controller
    {
        // Creating a variable for the logger and mapper.
        private static Logger logger;
        UserMapper mapper = new UserMapper();

        // Creating a variable for the UserDAO.
        private readonly UserDAO UserDataAccess;

        // Constructor
        public AccountController()
        {
            // getting paths from config
            string logpath = ConfigurationManager.AppSettings["ErrorLogPath"];
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            // variables for the constructor and paths
            UserDataAccess = new UserDAO(connectionString, logpath);
            logger = new Logger(logpath);
        }

        // getting view for create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // Posting the create
        [HttpPost]
        public ActionResult Create(User form)
        {
            ActionResult response;

            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {
                    UserDO dataObject = new UserDO();

                    // Paramaters ofr users
                    dataObject.UserId = form.UserId;
                    dataObject.Username = form.Username;
                    dataObject.Password = form.Password;
                    dataObject.Email = form.Email;
                    dataObject.Bio = form.Bio;
                    dataObject.RoleId = 1;

                    // running the createuser method
                    UserDataAccess.CreateUser(dataObject);

                    // holds on to username
                    TempData["Username"] = dataObject.Username;
                    response = RedirectToAction("Login", "Account");
                }
                else
                {
                    response = View(form);
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql 
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(form);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(form);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }

        // Get login info
        [HttpGet]
        public ActionResult Login()
        {
            ActionResult response;

            Login login = new Login();
            
            try
            {   // checking for username
                if (TempData.ContainsKey("Username"))
                {
                    // if so log in
                    login.Username = (string)TempData["Username"];
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql 
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(login);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(login);
                TempData["Message"] = "Error Try Again";
            }
            return View(login);
        }

        // post login info
        [HttpPost]
        public ActionResult Login(Login form)
        {
            ActionResult response;

            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {
                    UserDO userDataObject = UserDataAccess.ViewUsersByUsername(form.Username);
                    // checking userId
                    if (userDataObject.UserId != default(int))
                    {
                        // if userid is valid check below
                        if (userDataObject.Password.Equals(form.Password))
                        {
                            Session["Username"] = userDataObject.Username;
                            Session["UserId"] = userDataObject.UserId;
                            Session["Role"] = userDataObject.RoleId;

                            response = RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            response = View(form);
                        }
                    }
                    else
                    {
                        response = View(form);
                    }
                }
                else
                {
                    response = View(form);
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(form);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(form);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }

        // getting info to delete
        [HttpGet]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult Delete(long userId)
        {
            
            ActionResult response;

            // checking to see if its the same user or an admin before deleteing
            if (userId != (long)Session["UserId"] && (long)Session["Role"] == 1)
            {
                response = RedirectToAction("AllUsers", "Account");
            }
            else
            {
                try
                {
                    // creating variables to use below
                    UserDO user = UserDataAccess.ViewUsersById(userId);
                    User deletedUser = mapper.MapDoToPo(user);
                    long userID = deletedUser.UserId;


                    // check to see if id is valid
                    if (userId > 0)
                    {
                        // if valid delete
                        UserDataAccess.DeleteUser(userId);
                        response = RedirectToAction("AllUsers", "Account");
                    }
                    else
                    {
                        // if not valid
                        response = RedirectToAction("Index", "Home");
                    }
                }
                catch (SqlException Sqlex)
                {
                    //Log exception for sql
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                    response = View(userId);
                    TempData["Message"] = "Connection Error";
                }
                catch (Exception ex)
                {
                    // log exception for other exceptions
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = View(userId);
                    TempData["Message"] = "Error Try Again";
                }
            }
            return response;
        }

        // view all users
        public ActionResult AllUsers()
        {
            // creating variables and mapping
            ActionResult response;
            List<UserDO> allUsers = UserDataAccess.ViewAllUsers();
            List<User> mappedUsers = new List<User>();

            try
            {
                // each object in users
                foreach (UserDO dataObject in allUsers)
                {
                    // mapping users
                    mappedUsers.Add(mapper.MapDoToPo(dataObject));
                }
                response = View(mappedUsers);
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql 
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(mappedUsers);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(mappedUsers);
                TempData["Message"] = "Error Try Again";
            }
            return response;

        }

        // builds user details
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult UserDetails(long userId)
        {
            // creates variable
            ActionResult response;
            UserDO user = UserDataAccess.ViewUsersById(userId);

            // uses variable and mapper
            User detailsUser = mapper.MapDoToPo(user);
            long userID = detailsUser.UserId;

            try
            {
                //check if valid
                if (userID > 0)
                {
                    // if valid
                    response = View(detailsUser);
                }
                else
                {
                    // if not valid
                    response = RedirectToAction("AllUsers", "Account");
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(detailsUser);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(detailsUser);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }

        // building updated users
        [HttpGet]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult UpdateUser(long UserId)
        {
            // creating and using variable and mapper
            ActionResult response;
            
            UserDO user = UserDataAccess.ViewUsersById(UserId);
            User updateUser = mapper.MapDoToPo(user);
            long userId = updateUser.UserId;

            // checking for same user id or admin
            if (userId != (long)Session["UserId"] && (long)Session["Role"] == 1)
            {
                response = RedirectToAction("AllUsers", "Account");
            }
            else
            {

                try
                {
                    //check if valid id
                    if (userId > 0)
                    {
                        // if valid
                        response = View(updateUser);
                    }
                    else
                    {
                        // if not valid
                        response = RedirectToAction("AllUsers", "Account");
                    }
                }
                catch (SqlException Sqlex)
                {
                    //Log exception for sql
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                    response = View(updateUser);
                    TempData["Message"] = "Connection Error";
                }
                catch (Exception ex)
                {
                    // log exception for other exceptions
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = View(updateUser);
                    TempData["Message"] = "Error Try Again";
                }
            }
            return response;
                
        }

        // updating user
        [HttpPost]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult UpdateUser(User form)
        {
            // creating a response
            ActionResult response;

            try
            {
                // checking if valid
                if (ModelState.IsValid)
                {
                    // variables and mapper
                    UserDO userDO = mapper.MapPoToDo(form);
                    UserDataAccess.UpdateUser(userDO);

                    // where to redirect
                    response = RedirectToAction("AllUsers", "Account");
                }
                else
                {
                    response = View(form);
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(form);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(form);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }
        [HttpGet]
        public ActionResult LogOff()
        {
            // abandon session and logoff
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}