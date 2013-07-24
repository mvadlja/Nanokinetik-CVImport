using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using CommonTypes;

namespace AspNetUIFramework
{
    public class SessionManager
    {
        public static readonly SessionManager Instance = new SessionManager();
        private SessionManager() { }

        public Exception UserException
        {
            get { return HttpContext.Current.Session["UserException"] == null ? null : (Exception)HttpContext.Current.Session["UserException"]; }
            set { HttpContext.Current.Session["UserException"] = value; }
        }

        public AppUser CurrentUser
        {
            get { return HttpContext.Current.Session["CurrentUser"] == null ? null : (AppUser)HttpContext.Current.Session["CurrentUser"]; }
            set { HttpContext.Current.Session["CurrentUser"] = value; }
        }
    }
}
