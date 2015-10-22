using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utilities;
using TE_System.AppContext;
using TE_System.Models;
using System.Threading.Tasks;

namespace TE_System.Controllers
{
    public class LoginController : Controller
    {
        private CrmAuthentication auth = new CrmAuthentication();
        private SystemUserContext dbUser = new SystemUserContext();
        //
        // GET: /Login/
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {                               
                return RedirectToAction("Index", "Entry");
            }          
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(PortalUser userInput)
        {
            try
            {
                bool result = await auth.IsAuthenticated(userInput);

                if (result)
                {
                    SystemUser userDetails = await dbUser.GetUserDetails(userInput.Username);
                    if (userDetails != null)
                    {
                        FormsAuthentication.SetAuthCookie(userDetails.ID, false);
                        SetCookies(userInput);
                        Session["week"] = DateExtension.CurrentWeek;
                        return RedirectToAction("Index", "Entry");
                    }
                    ViewBag.ErrorMessage = AlertMessages.Error("Error!",
                        "Invalid Username/Password");
                    return View();
                }
                ViewBag.ErrorMessage = AlertMessages.Error("Error!",
                    "Your account doesn't exist in our records.");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = AlertMessages.Error("Error!",
                        "Invalid Username/Password");
                return View();
            }
        }
        public void SetCookies(PortalUser data)
        {
            HttpCookie myCookie = new HttpCookie("UserSettings");
            myCookie["week"] = DateExtension.CurrentWeek.ToString();
            myCookie["domain"] = data.Username.Encrypt();
            myCookie["user"] = data.Password.Encrypt();
            Response.Cookies.Add(myCookie);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Entry");
        }
    }
}