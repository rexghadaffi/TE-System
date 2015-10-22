using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TE_System.AppContext;
using TE_System.Models;
using Utilities;

namespace TE_System.Controllers
{
    [UserCredentials]
    public class ValidateTimeController : BaseController
    {
        private TimekeepingContext dbTimekeeping = new TimekeepingContext();

        public ActionResult Index(string notify)
        {
            if (notify != null)
            {
                // Tell user that create/update was successful
                ViewBag.Saved = AlertMessages.Saved;
            } 
            ViewBag.CurrentWeek = Session["week"];
            base.DisplayUserDetails();
            return View("Index");
        }
        //--Fetch Data
        public string Data(int pageSize, int pageNumber, string sortOrder)
        {
            var skip = (pageNumber - 1) * pageSize;
            int skipResult = Convert.ToInt32(skip);
            

            // counts the lists that was fetched
            var listCount = base.dbEntry.FetchAll.Count();
            //Fetch all entries that was flagged by Project Managers
            var list = base.dbEntry.FetchAll.Where(e => e.isValidBillability == false).ToList().Skip(skipResult).Take(pageSize);

            //-- Splits data for the bootstrap table parameters
            dynamic foo = new ExpandoObject();
            foo.total = listCount;
            foo.rows = list;

            string json = JsonConvert.SerializeObject(foo);

            return json;
        }
        public ActionResult ViewFlaggedUserEntry(string userID)
        {
            /*
             * A ViewResult that returns the entries of a user
             */
            List<TimeEntry> entries = base.dbEntry.FetchForUser(new Guid(userID),
                                                Session["week"].ToInteger()).ToList();
            // Display Total Hours
            ViewBag.TotalHours = entries.Sum(t => t.TotalWeekHours);
            ViewBag.Warning = "warning";
            return PartialView("_StaffEntries", entries);
        }
        public ActionResult FlaggedUsers()
        {
            /*
             * A ViewResult that returns the all the flagged user
             */
            List<SystemUser> users = this.dbTimekeeping.FetchFlaggedUsers.ToList();
            return PartialView("_FlaggedStaff", users);
        }
        public ActionResult Edit(string tid)
        {
            /*
             * A ViewResult that returns the edit form for an entry
             */
            TimeEntry entry = base.dbEntry.FetchRecordDetails(new Guid(tid));
            SelectDropDowns(entry);
            return PartialView("_EditForm", FillViewModel(entry));
        }
        [HttpPost]       
        public ActionResult Edit(TimeEntry entry, string tid)
        {
            /*
             * An ActionResult that process the updating of an entry
             */
            base.dbEntry.Update(entry, tid, true);
            return RedirectToAction("Index", new { notify = "true" });
        }
    }
}