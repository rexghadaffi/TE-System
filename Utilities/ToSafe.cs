using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ToSafe
    {
        public static decimal ToDecimal(this object data)
        {
            try
            {
                return decimal.Parse(data.ToString());
            }
            catch { return decimal.Zero; }
        }
        public static int ToInteger(this object data)
        {
            try
            {
                return int.Parse(data.ToString());
            }
            catch { return 0; }
        }
        public static bool ToBoolean(this object data)
        {
            try
            {
                return Convert.ToBoolean(data);
            }
            catch { return false; }
        }
        public static bool CbxToBoolean(this object data)
        {
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public static int ToSafeWeek(this int data)
        {
            if (data > 52 || data <= 0)
            {
                return DateExtension.CurrentWeek;
            }
            return data;
        }      

        public static int ToSafeWeek(this string data)
        {
            try
            {
                int result = int.Parse(data);

                if (result > 52 || result <= 0)
                {
                    return DateExtension.CurrentWeek;
                }
                return result;
            }
            catch { return 0; }
        }
        public static string ToSafeWeekAsString(this int data)
        {
            if (data > 52 || data <= 0)
            {
                return DateExtension.CurrentWeek.ToString();
            }
            return data.ToString();
        }      
    }
}
