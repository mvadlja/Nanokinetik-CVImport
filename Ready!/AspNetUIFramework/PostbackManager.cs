using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace AspNetUIFramework
{
    public class PostbackManager
    {
        public static readonly PostbackManager Instance = new PostbackManager();
        private PostbackManager() { }

        public string PostbackControlID
        {
            get { return HttpContext.Current.Request.Params["__EVENTTARGET"] == null ? null : HttpContext.Current.Request.Params["__EVENTTARGET"].ToString(); }
        }

        public string PostbackArgument
        {
            get { return HttpContext.Current.Request.Params["__EVENTARGUMENT"] == null ? null : HttpContext.Current.Request.Params["__EVENTARGUMENT"].ToString(); }
        }
    }
}
