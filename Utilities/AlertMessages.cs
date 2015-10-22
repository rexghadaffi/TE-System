using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class AlertMessages
    {
        public static string Error(string title, string content)
        {
            return string.Format("UserPrompt('{0}', '{1}', '{2}', '{3}');",
                                                title,
                                                content,
                                                "alert",
                                                "warning");
        }
        public static string Saved
        {
            get
            {
                return "$.notify('<strong>Entry Saved!</strong> " +
                       "You have sucessfully saved an entry.', { type: 'success' });";
            }
        }

        public static string Flagged
        {
            get
            {
                return "$.notify('<strong>Record Updated!</strong> " +
                       "You have sucessfully updated an entry.', { type: 'success' });";
            }
        }

        public static string InputError
        {
            get
            {
                return "$.notify('<strong>Unable to Save!</strong> " +
                      "Please fill in all the required fields.', { type: 'success' });";
            }
        }
        public static string Error(string exception)
        {
            return "$.notify('<strong>System Error!</strong> " +
                      exception.Replace("'", "") + "', { type: 'error' });";
        }

        public static string InvalidWeek
        {
            get { return "InvalidWeek();"; }
        }
        public static string PendingInvalidEntry
        {
            get
            {
                return "$.notify('<strong>Warning!</strong> One of your entries is invalid, Please modify immediately.', { type: 'danger' });";
            }
        }

    }
}
