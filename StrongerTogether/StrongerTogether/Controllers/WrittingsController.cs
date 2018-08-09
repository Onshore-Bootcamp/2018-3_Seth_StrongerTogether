using StrongerTogether.Custom;
using StrongerTogether.Mapping;
using StrongerTogether.Models;
using StrongerTogetherBLL;
using StrongerTogetherBLL.Model;
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
    public class WrittingsController : Controller
    {
        // creating variables
        private static Logger logger;
        private WrittingsMapper mapper = new WrittingsMapper();
        private Logic logic;

        // creating a varaible
        private readonly WrittingsDAO WrittingsDataAccess;

        // calling on the config and instantiating the variables we created
        public WrittingsController()
        {
            string logpath = ConfigurationManager.AppSettings["ErrorLogPath"];
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            WrittingsDataAccess = new WrittingsDAO(connectionString, logpath);
            logger = new Logger(logpath);
            logic = new Logic();
        }

        //creating create writting view
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        [HttpGet]
        public ActionResult CreateWrittings()
        {
            return View();
        }

        // posting the createwritting information and params
        [HttpPost]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult CreateWrittings(Writtings form)
        {
            // creating the date for the date published
            form.DatePublished = DateTime.Now.Date;

            // creating our response variable
            ActionResult response;
            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {   
                    //pulling the user id from session
                    form.UserId = (long)Session["UserId"];
                    // using the logic layer for wordcount
                    form.WordCount = logic.CountWords(form.Content);
                    //mapping the writtings
                    WrittingsDO writtings = mapper.MapPoToDo(form);
                    WrittingsDataAccess.CreateWrittings(writtings);

                    response = RedirectToAction("AllWrittings", "Writtings");
                }
                else
                {
                    // if not valid 
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

        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        [HttpGet]
        public ActionResult DeleteWrittings(long writtingId)
        {
            // response variable
            ActionResult response;
            // checking to see if the writting ids match or admin
            if (writtingId != (long)Session["UserId"] && (long)Session["Role"] == 1)
            {
                response = RedirectToAction("AllUsers", "Account");
            }
            else
            {
                // creating variables for writtings and mapping them Do to PO
                WrittingsDO writtings = WrittingsDataAccess.ViewWrittingsById(writtingId);
                Writtings deletedWrittings = mapper.MapDoToPo(writtings);
                long writtingID = deletedWrittings.WrittingId;


                try
                {
                    //check to see if id is valid
                    if (writtingId > 0)
                    {
                        // id is valid
                        WrittingsDataAccess.DeleteWritting(writtingId);
                        response = RedirectToAction("AllWrittings", "Writtings");
                    }
                    else
                    {
                        // not valid
                        response = RedirectToAction("AllWrittings", "Writtings");
                    }
                }
                catch (SqlException Sqlex)
                {
                    //Log exception for sql 
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                    response = View(writtings);
                    TempData["Message"] = "Connection Error";
                }
                catch (Exception ex)
                {
                    // log exception for other exceptions
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = View(writtings);
                    TempData["Message"] = "Error Try Again";
                }
            }
            return response;
        }

        public ActionResult AllWrittings()
        {
            ActionResult response;

            // creating variable and instantiation
            List<WrittingsDO> allWrittings = WrittingsDataAccess.ViewAllWrittings();
            List<Writtings> mappedWrittings = new List<Writtings>();
            List<WrittingsBO> allWrittingsBO = new List<WrittingsBO>();
            // catching errors
            try
            {   // checking to see if dataoject is in allwrittings
                foreach (WrittingsDO dataObject in allWrittings)
                {
                    // writting to the list in the PO
                    mappedWrittings.Add(mapper.MapDoToPo(dataObject));
                }
                foreach (WrittingsDO dataObject in allWrittings)
                {
                    // writting to the list in the BO
                    allWrittingsBO.Add(mapper.MapDoToBo(dataObject));
                }
                    // creating the average and putting it into the view bag
                ViewBag.AverageWordCount = logic.AverageWordCount(allWrittingsBO);

            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(allWrittings);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(allWrittings);
                TempData["Message"] = "Error Try Again";
            }
            // shows the mapped writtings
            return View(mappedWrittings);
        }

        /// <summary>
        /// building the writting details
        /// </summary>
        /// <param name="writtingId"> the id accoiated with the users writtings</param>
        /// <returns> writting details</returns>
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult WrittingDetails(long writtingId)
        {
            // creating response variable
            ActionResult response;

            // viewing by writting id and mapping the writtings
            WrittingsDO writtings = WrittingsDataAccess.ViewWrittingsById(writtingId);
            Writtings detailWrittings = mapper.MapDoToPo(writtings);

            // writtingid variable
            long WrittingID = detailWrittings.WrittingId;

            try
            {
                //check if valid
                if (writtingId > 0)
                {
                    // if valid view details
                    response = View(detailWrittings);
                }
                else
                {
                    // if not valid redirect
                    response = RedirectToAction("AllWrittings", "Writtings");
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql 
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(writtings);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(writtings);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }

        /// <summary>
        /// updating writtings 
        /// </summary>
        /// <param name="writtingId"></param>
        /// <returns> updated writtings</returns>
        [HttpGet]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult UpdateWrittings(long writtingId)
        {
            // response variable
            ActionResult response;
            // checking if id matches or admin
            if (writtingId != (long)Session["UserId"] && (long)Session["Role"] == 1)
            {
                response = RedirectToAction("AllUsers", "Account");
            }
            else
            {
                // mapping what needs to be updated DO to PO
                WrittingsDO writtings = WrittingsDataAccess.ViewWrittingsById(writtingId);
                Writtings updateWrittings = mapper.MapDoToPo(writtings);
                long WrittingId = updateWrittings.WrittingId;

                try
                {
                    //check if valid
                    if (writtingId > 0)
                    {
                        // view update writtings
                        response = View(updateWrittings);
                    }
                    else
                    {
                        // redirect if not valid
                        response = RedirectToAction("AllWrittings", "Writtings");
                    }
                }
                catch (SqlException Sqlex)
                {
                    //Log exception for sql 
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                    response = View(writtings);
                    TempData["Message"] = "Connection Error";
                }
                catch (Exception ex)
                {
                    // log exception for other exceptions
                    logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = View(writtings);
                    TempData["Message"] = "Error Try Again";
                }
            }
            return response;
        }

        /// <summary>
        /// posting the updated information
        /// </summary>
        /// <param name="form"></param>
        /// <returns> the updated information </returns>
        [HttpPost]
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult UpdateWrittings(Writtings form)
        {
            // response variables
            ActionResult response;

            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {
                    //updates wordcount
                    form.WordCount = logic.CountWords(form.Content);
                    // mapping the updated writting PO to DO
                    WrittingsDO writtingsDO = mapper.MapPoToDo(form);
                    WrittingsDataAccess.UpdateWrittings(writtingsDO);

                    // redirect
                    response = RedirectToAction("AllWrittings", "Writtings");
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
    }
}