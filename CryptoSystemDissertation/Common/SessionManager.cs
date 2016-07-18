using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoSystemDissertation.Common
{
    public static class SessionManager
    {
        public static void RegisterSession(string key, object obj)
        {
            System.Web.HttpContext.Current.Session[key] = obj;
        }

        public static void FreeSession(string key)
        {
            System.Web.HttpContext.Current.Session[key] = null;
        }


        public static bool CheckSession(string key)
        {
            if (System.Web.HttpContext.Current.Session[key] != null)
                return true;
            else
                return false;
        }


        public static object ReturnSessionObject(string key)
        {
            if (CheckSession(key))
                return System.Web.HttpContext.Current.Session[key];
            else
                return null;
        }
    }
}