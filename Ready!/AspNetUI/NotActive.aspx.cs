using System;
using System.Collections.Generic;
using Kmis.Model;
using System.Threading;
using System.Configuration;
using Ready.Model;

namespace AspNetUI
{
    public partial class NotActive : System.Web.UI.Page
    {
        IUSEROperations _userOperations;
        IDowntimeOperations _downtimeOperations;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userOperations = new USERDAL();
            _downtimeOperations = new DowntimeDAL();

            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            if (user != null && user.Country_FK != null)
            {
                List<Downtime> downtimes = _downtimeOperations.GetCurrentActiveDowntimesByCountryID(user.Country_FK.Value);

                if (downtimes.Count > 0)
                {
                    Downtime relevantDowntime = downtimes[0];
                }
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                Label1.Text = string.Format("v{0} ", ConfigurationManager.AppSettings["ApplicationVersion"]);
        }
        }
    }
}