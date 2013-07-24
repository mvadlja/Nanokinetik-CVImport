using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using AspNetUIFramework;
namespace AspNetUI.Support
{
    public class ReadySettings
    {
        //DEFAULT SETTINGS VALUES
        public static int XEVPRMMaxResubmit = 10;

        static ReadySettings() {

            if (ValidationHelper.IsValidInt(ConfigurationManager.AppSettings["XEVPRMMaxResubmit"]))
            {
                XEVPRMMaxResubmit = Convert.ToInt32(ConfigurationManager.AppSettings["XEVPRMMaxResubmit"]);
            }
        }
    }
}