using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TE_System.AppContext;
using TE_System.Models;
using Utilities;

namespace TE_System.Controllers
{
    [Authorize]
    [UserCredentials]
    public class EntryController : BaseController
    {
        /* 
            Summary:
            This controller handles the request for creating / editing
            time entries and viewing of remarks.
        */
        public ActionResult Index(string notify)
        {
            /* 
             Checks if a request came from a create/update event
             */
          
            if (notify != null)
            {
                // Tell user that create/update was successful
                ViewBag.Saved = AlertMessages.Saved;                
            }            
            return SetFormLoad();
        }  
   
        public ActionResult IndexData()
        {            
            //ViewBag.TotalHours = entries.Entries.Sum(t => t.TotalWeekHours);
            return PartialView("_IndexData", FillViewModel());
        }
        [HttpPost]
        public ActionResult Create(FormCollection formData)
        {
            /* Adds new entry
              - Model State needs to be checked here for server-side validation            
             */            
            dbEntry.AddNewRecord(User.Identity.Name,
                                 Session["week"].ToInteger(),
                                 base.dbEntry.ExtractFormCollection(formData));
            return RedirectToAction("Index", new { notify = "true" });
        }
        private ActionResult SetFormLoad()
        {
            /* 
              Get all data for display in the Index  
            */
         
           
            base.DisplayUserDetails();
           
            ViewBag.Flagged = "warning";
            return View("Index");
        }       
        [HttpPost]
        public ActionResult Edit(TimeEntry entry, string tid)
        {
            /* 
               Update Entry..
             */
            base.dbEntry.Update(entry, tid);
            return RedirectToAction("Index", new { notify = "true" });          
        }

        #region -- Methods Called By Ajax --  
        [IsEntryLocked] 
        public ActionResult RenderModal()
        {
            /* 
               Renders the Time Entry Form (adding)
            */            
            base.SetDropDowns();
            return PartialView("_AddEntry");
        }
        [IsEntryLocked]
        public ActionResult Edit(string tid)
        {
            /* 
               Renders the Time Entry Form (updating)
            */
            // Get db data.
            TimeEntry entry = base.dbEntry.FetchRecordDetails(new Guid(tid));
            base.SelectDropDowns(entry);
            return PartialView("_EditForm", FillViewModel(entry));
        }        
        public ActionResult GetRemarksForEntry(string id)
        {
            /* 
               Renders the Left Sidebar for viewing of remarks.
            */
            // Get db data.
            return PartialView("_Sidebar", dbAnnotation.GetNotesForEntry(id));
        }
        public ActionResult DisplayProhibitedError()
        {
            /* 
               Renders a message that the user cannot modifiy an entry
            */
            return PartialView("~/Views/Shared/ErrorMessage.cshtml");
        }
        #endregion
    }
}