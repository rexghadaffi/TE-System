using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TE_System.AppContext;

namespace TE_System.Controllers
{
    [Authorize()]
    public class GuidelinesController : Controller
    {
        //
        // GET: /Guidelines/     
        [UserCredentials]
        public ActionResult Index()
        {            
            DisplayForFullName();
            return View();
        }
        [UserCredentials]
        protected void DisplayForFullName()
        {
              // CRM User Data Access Object
            SystemUserContext dbUser = new SystemUserContext();
            // Display Fullname for User
            ViewBag.FullName = dbUser.GetFullNameForUser(new Guid(User.Identity.Name)).Fullname;
        } 
	}
}