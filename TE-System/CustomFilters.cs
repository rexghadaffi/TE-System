using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Utilities;

namespace TE_System
{
    public class UserCredentials : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {          
            /* This custom filter checks whether [week] session is existing. 
               If not, we destroy all the user's credential in the cookie and all stored
               sessions
             */
            if (context.HttpContext.Session["week"] == null)
            {
                // destroy authentication cookie
                FormsAuthentication.SignOut();
                context.HttpContext.Session.Abandon();
                // move the user to login page.
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Login");             
                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
            base.OnActionExecuting(context);
        }
    }

    public class IsEntryLocked : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            /* This custom filter validates the current time if 
             * it's still valid to create/update an entry.
             */
            int currentWeek = context.HttpContext.Session["week"].ToInteger();
            if (currentWeek < DateExtension.CurrentWeek ||
                currentWeek > (DateExtension.CurrentWeek + 1) ||
                DateExtension.IsEntryModificationLocked)
            {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "DisplayProhibitedError");
                redirectTargetDictionary.Add("controller", "Entry");
                context.Result = new RedirectToRouteResult(redirectTargetDictionary);                 
            }
            base.OnActionExecuting(context);
        }
    }
}