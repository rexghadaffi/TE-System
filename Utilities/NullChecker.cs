using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class NullChecker
    {
        public static bool IsNull(this object data)
        {
            if (data == null)
            {
                return true;
            }
            return false;
        }
        public static bool IsEmpty(this string data)
        {
            if (data == string.Empty || data.Trim() == "")
            {
                return true;
            }
            return false;
        }

        public static bool IsEmpty(this decimal data)
        {
            if (data == decimal.Zero)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this string data)
        {
            if (data == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this int data)
        {
            if (data == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this decimal data)
        {
            if (data == null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNull(this DateTime data)
        {
            if (data == null)
            {
                return true;
            }
            return false;
        } 
    }
}
