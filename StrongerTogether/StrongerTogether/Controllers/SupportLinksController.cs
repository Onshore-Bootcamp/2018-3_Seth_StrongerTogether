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
    public class SupportLinksController : Controller
    {
        // variables
        private static Logger logger;
        SupportLinksMapper mapper = new SupportLinksMapper();

        //variables
        private readonly SupportLinksDAO SupportLinksDataAccess;

        public SupportLinksController()
        {
            // calling on conig
            string logpath = ConfigurationManager.AppSettings["ErrorLogPath"];
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            // using cariables config
            SupportLinksDataAccess = new SupportLinksDAO(connectionString, logpath);
            logger = new Logger(logpath);
        }

        
        [HttpGet]
        [Authorization("Role", "/Account/Login", 2, 3)]
        public ActionResult CreateSupportLinks()
        {
            return View();
        }

        [HttpPost]
        [Authorization("Role", "/Account/Login", 2, 3)]
        public ActionResult CreateSupportLinks(SupportLinks form)
        {
            
            ActionResult response;

            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {
                    // checking for http
                    if (!form.Url.StartsWith("https://"))
                    {
                        // adding http
                        form.Url = "https://" + form.Url;
                    }

                    // creating dataobject
                    SupportLinksDO dataObject = new SupportLinksDO()
                    {
                        //paramaters for support links
                        SupportId = form.SupportId,
                        Name = form.Name,
                        Address = form.Address,
                        Phone = form.Phone,
                        Url = form.Url,
                        UserId = (long)Session["UserId"]

                    };
                    // display param info
                    SupportLinksDataAccess.CreateSupportLinks(dataObject);

                    
                    TempData["SupportId"] = dataObject.SupportId;
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
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
        [Authorization("Role", "/Account/Login", 3)]
        public ActionResult DeleteSupportLinks(long supportId)
        {
            
            ActionResult response;

            // creating variables and mapping 
            SupportLinksDO supportLinks = SupportLinksDataAccess.ViewSupportLinksById(supportId);
            SupportLinks deleteSupportLinks = mapper.MapDoToPo(supportLinks);
            long supportID = deleteSupportLinks.SupportId;

            
            try
            {
                //check to see if id is valid
                if (supportId > 0)
                {
                    // if valid
                    SupportLinksDataAccess.DeleteSupportLinks(supportId);
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
                }
                else
                {
                    // if not valid
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(supportLinks);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(supportLinks);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }
        public ActionResult AllSupportLinks()
        {
            ActionResult response;

            //creating variables
            List<SupportLinksDO> allSupportLinks = SupportLinksDataAccess.ViewAllSupportLinks();
            List<SupportLinks> mappedSupportLinks = new List<SupportLinks>();

            
            try
            {   // looking for object in allsupportlinks
                foreach (SupportLinksDO dataObject in allSupportLinks)
                {
                    // mapping
                    mappedSupportLinks.Add(mapper.MapDoToPo(dataObject));
                }

            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(allSupportLinks);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(allSupportLinks);
                TempData["Message"] = "Error Try Again";
            }
            
            return View(mappedSupportLinks);
        }

        
        [Authorization("Role", "/Account/Login", 1, 2, 3)]
        public ActionResult SupportLinksDetails(long supportId)
        {
            // variables and response action
            ActionResult response;
            SupportLinksDO supportLinks = SupportLinksDataAccess.ViewSupportLinksById(supportId);

            // Id and mapping
            SupportLinks detailSupportLinks = mapper.MapDoToPo(supportLinks);
            long supportID = detailSupportLinks.SupportId;
            
            try
            {
                //check if valid
                if (supportID > 0)
                {
                    // if valid
                    response = View(detailSupportLinks);
                }
                else
                {
                    // if not valid
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql 
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(detailSupportLinks);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(detailSupportLinks);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }

        
        [HttpGet]
        [Authorization("Role", "/Account/Login", 2, 3)]
        public ActionResult UpdateSupportLinks(long supportId)
        {
            // response and supporlinks variable
            ActionResult response;
            SupportLinksDO supportLinks = SupportLinksDataAccess.ViewSupportLinksById(supportId);

            // mapping
            SupportLinks updateSupportLinks = mapper.MapDoToPo(supportLinks);
            long supportID = updateSupportLinks.SupportId;
            
            try
            {
                //check if valid
                if (supportID > 0)
                {
                    // if valid
                    response = View(updateSupportLinks);
                }
                else
                {
                    // if not valid
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
                }
            }
            catch (SqlException Sqlex)
            {
                //Log exception for sql
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, Sqlex);
                response = View(updateSupportLinks);
                TempData["Message"] = "Connection Error";
            }
            catch (Exception ex)
            {
                // log exception for other exceptions
                logger.ErrorLogger(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = View(updateSupportLinks);
                TempData["Message"] = "Error Try Again";
            }
            return response;
        }
        [HttpPost]
        [Authorization("Role", "/Account/Login", 2, 3)]
        public ActionResult UpdateSupportLinks(SupportLinks form)
        {
           
            ActionResult response;

            try
            {
                // checking to see if valid
                if (ModelState.IsValid)
                {
                    // mapping 
                    SupportLinksDO supportLinksDO = mapper.MapPoToDo(form);
                    SupportLinksDataAccess.UpdateSupportLinks(supportLinksDO);

                    // redirect
                    response = RedirectToAction("AllSupportLinks", "SupportLinks");
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

    }

}