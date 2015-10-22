using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilities;
using TE_System.Models;
using TE_System.AppContext;
using System.Web.Routing;
using Newtonsoft.Json;

namespace TE_System.Controllers
{
    // Some controllers will be inheriting this base controller for code-reuse  
    public abstract class BaseController : Controller
    {
        //
        // GET: /Base/
        #region - Properties and Private Members -
        // Sales Order Data Access Object
        protected ProjectContext dbProject = new ProjectContext();
        // Activity Type Data Access Object
        protected ActivityTypeContext dbActivity = new ActivityTypeContext();
        // Staff Type Data Access Object
        protected StaffTypeContext dbStaff = new StaffTypeContext();
        // Practice Data Access Object
        protected PracticeContext dbPractice = new PracticeContext();
        // TimeEntry Data Access Object
        protected TimeEntryContext dbEntry = new TimeEntryContext();
        // CRM User Data Access Object
        protected SystemUserContext dbUser = new SystemUserContext();
        // CRM Annotation Data Access Object
        protected AnnotationContext dbAnnotation = new AnnotationContext();



        #endregion

        #region -- Method Helpers --
        // Stores the fullname of the user to a viewbag to display in the view
        protected void DisplayUserDetails()
        {
            ViewBag.CurrentWeek = Session["week"];
            ViewBag.FullName = dbUser.GetFullNameForUser(new Guid(User.Identity.Name)).Fullname;
        }

        //Reduces the value of Session["week"] to 1
        [UserCredentials]
        public ActionResult PreviousWeek()
        {
            int currentWeek = Convert.ToInt32(Session["week"]);
            currentWeek--;
            Session["week"] = currentWeek.ToSafeWeek();
            return RedirectToAction("Index");           
            //return PartialView("~/Views/Entry/_TableRows.cshtml", FillViewModel());
        }
        //Increases the value of Session["week"] to 1        
        [UserCredentials]
        public ActionResult NextWeek()
        {
            int currentWeek = Convert.ToInt32(Session["week"]);
            currentWeek++;
            Session["week"] = currentWeek.ToSafeWeek();
            return RedirectToAction("Index");
        }
        // Parses the query string from the search text to a date
        // and converts it to week #
        [UserCredentials]
        public ActionResult SearchWeek(string date)
        {
            Session["week"] = DateExtension.GetWeekNumber(date).ToSafeWeek();
            return RedirectToAction("Index");
        }
        // A Method Helper that fills a Model
        protected TimeEntryViewModel FillViewModel(TimeEntry entry = null)
        {
            TimeEntryViewModel vm = new TimeEntryViewModel
            {
                Entries = dbEntry.FetchForUser(new Guid(User.Identity.Name),
                                                Session["week"].ToInteger()).ToList(),
                Entry = entry
            };
            return vm;
        }
        // Creates a Select list item <option></option> in html
        // and  pre selects it before rendering in the view
        protected void SelectDropDowns(TimeEntry entry)
        {
            ViewBag.Project = new SelectList(dbProject.FetchAll(),
                                               "ProjectID",
                                               "ProjectName", entry.ProjectID);
            ViewBag.Activity = new SelectList(dbActivity.FetchAll(),
                                              "ActivityTypeID",
                                              "ActivityTypeName", entry.ActivityTypeID);

            ViewBag.StaffType = new SelectList(dbStaff.FetchAll(),
                                              "StaffTypeID",
                                              "StaffTypeName", entry.StaffTypeID);
            ViewBag.Practice = new SelectList(dbPractice.FetchAll(),
                                            "PracticeID",
                                            "PracticeName", entry.PracticeID);
        }
        // Caches the rendered dropdowns.
        [OutputCache(CacheProfile = "cache3mins")]
        // Creates a dropdownlist in the view
        protected void SetDropDowns()
        {
            ViewBag.Project = new SelectList(dbProject.FetchAll(),
                                               "ProjectID",
                                               "ProjectName");
            ViewBag.Activity = new SelectList(dbActivity.FetchAll(),
                                              "ActivityTypeID",
                                              "ActivityTypeName");

            ViewBag.StaffType = new SelectList(dbStaff.FetchAll(),
                                              "StaffTypeID",
                                              "StaffTypeName");
            ViewBag.Practice = new SelectList(dbPractice.FetchAll(),
                                            "PracticeID",
                                            "PracticeName");
        }

        //protected string GetWeekCookieValue()
        //{
        //    //if (Request.Cookies["UserSettings"]["week"].IsNull())
        //    //{
        //    //    return string.Empty;
        //    //}
        //    return Request.Cookies["UserSettings"]["week"];
        //}

        //protected void SetCookieValue(HttpCookie cookie, string index, object value)
        //{
        //    if (cookie.IsNull())
        //    {
        //        return;
        //    }
        //    cookie[index] = value.ToString();
        //}
        #endregion

    }
}