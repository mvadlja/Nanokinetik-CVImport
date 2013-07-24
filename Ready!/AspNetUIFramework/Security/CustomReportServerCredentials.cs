using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using System.Net;
using System.Configuration;

namespace AspNetUIFramework
{
    [Serializable]
    public class CustomReportServerCredentials : IReportServerCredentials
    {
        #region IReportServerCredentials Members

        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            return false;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(ConfigurationManager.AppSettings["ReportServerCredentialsUsername"], ConfigurationManager.AppSettings["ReportServerCredentialsPassword"], ConfigurationManager.AppSettings["ReportServerCredentialsDomain"]);
            }
        }

        #endregion
    }
}
