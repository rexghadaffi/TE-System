using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TE_System;
using TE_System.Models;
using Utilities;

namespace TE_System.Controllers
{
     [UserCredentials]
    public class ValidateEntryController : BaseController
    {
        //
        // GET: /ValidateEntry/Index       
        public ActionResult Index(string notify)
        {
            if (notify != null)
            {
                // Tell user that create/update was successful
                ViewBag.Saved = AlertMessages.Flagged;
            } 
            ViewBag.CurrentWeek = Session["week"];
            base.DisplayUserDetails();           
            return View();
        }      
        private List<string> GetProjectIdForUser()
        {
            /*
             * A method helper that returns all 
             * the GUIDs associated with the current user
             */
            List<string> ids = new List<string>();

            foreach (Project item in dbProject.FindProjectForUser(User.Identity.Name))
            {
                ids.Add(item.ProjectID);
            }

            return ids;
        }
        public string Data(int pageSize, int pageNumber, string sortOrder, string searchText = null)
        {
            var skip = (pageNumber - 1) * pageSize;
            int skipResult = Convert.ToInt32(skip);

            IQueryable<TimeEntry> entries = IsSearching(searchText);
            //--Fetch Data
            var listCount = entries.Count();
            var list = entries.ToList().Skip(skipResult).Take(pageSize);

            dynamic foo = new ExpandoObject();
            foo.total = listCount;
            foo.rows = list;

            return JsonConvert.SerializeObject(foo);
        }
        private IQueryable<TimeEntry> IsSearching(string searchText)
        {
            /*
            * A method helper that checks
            * if user is searching from the bootstrap table
            * returns data accordingly 
            */
            IQueryable<TimeEntry> entries = dbEntry.FetchPartialDetails(GetProjectIdForUser(), (int?)Session["week"]);
            if (searchText != null)
            {
                return entries.Where(e => e.ActivityTypeID.Contains(searchText) ||
                                          e.StaffFullname.Contains(searchText) ||
                                          e.ProjectID.Contains(searchText) ||
                                          e.PracticeID.Contains(searchText) ||
                                          e.StaffTypeID.Contains(searchText));
            }
            return entries;
        }
        public ActionResult GetEntryDetail(string id)
        {
            /*
           * A ViewResult that returns the details of the entry 
           * of a particular user and renders it in the sidebar           
           */
            Guid recordID = Guid.Empty;
            recordID = new Guid(id);
            TimeEntry entry = dbEntry.FetchRecordDetails(recordID);

            return PartialView("~/Views/ValidateEntry/_Sidebar.cshtml", entry);
        }
        // Renders Partial View for Remarks 
        public ActionResult RemarksForm(string recordID)
        {
            /*
           * A ViewResult that renders a modal remarks form      
           */
            return PartialView("_RemarksForm");
        }
        [HttpPost]
        public ActionResult AddRemarks(Annotation data, string recordID)
        {
            /*
             * An ActionResult that process the post request for adding of remarks     
             */
            if (Request.Cookies["UserSettings"] != null)
            {
                string username;
                string password;
                if (Request.Cookies["UserSettings"]["domain"] != null &&
                    Request.Cookies["UserSettings"]["user"] != null)
                { 
                    username = Request.Cookies["UserSettings"]["domain"].Decrypt();
                    password = Request.Cookies["UserSettings"]["user"].Decrypt();
                    dbAnnotation.AddRemarksForEntry(new Guid(recordID), data, username, password);
                }               
            }
            return RedirectToAction("Index", new { notify = true });
        }
        // Dim Entry Information as Invalid
        public string FlagInfo(string entry)
        {
            try
            {
                Guid entryID = new Guid(entry);
                dbEntry.FlagInfo(entryID);
                return "Update Successful";
            }
            catch (Exception ex) { return ex.Message; }
        }
        // Dim Billability as Invalid
        public string FlagBillability(string entry)
        {
            try
            {
                Guid entryID = new Guid(entry);
                dbEntry.FlagBillability(entryID);
                return "Update Successful";
            }
            catch (Exception ex) { return ex.Message; }
        }

        // Dim Entry as Valid
        public string AcceptEntry(string recordID)
        {
            try
            {
                Guid entryID = new Guid(recordID);
                dbEntry.EntryValid(entryID);
                return "Update Successful";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}