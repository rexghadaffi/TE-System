using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TE_System
{
    public static class CustomHtmlHelpers
    {

        public static IHtmlString DisplayForBoolean(this HtmlHelper html, bool value)
        {
            string htmlString = String.Format("<span class='fa fa-flag fa-fw text-success'></span>");

            if (!value)
            {
                htmlString = String.Format("<span class='fa fa-flag fa-fw text-danger'></span>");
            }
            return new HtmlString(htmlString);
        }

        public static IHtmlString DisplayForCheckbox(this HtmlHelper html, bool value)
        {
            string htmlString = String.Format("<span class='fa fa-check fa-fw text-success'></span>");

            if (!value)
            {
                htmlString = String.Format("<span class='fa fa-remove fa-fw text-danger'></span>");
            }
            return new HtmlString(htmlString);
        }

        public static void  ActionLinkWithBootstrap(this AjaxHelper ajaxHelper, string linkText, string actionName, AjaxOptions ajaxOptions)
        {

        }
    }
}