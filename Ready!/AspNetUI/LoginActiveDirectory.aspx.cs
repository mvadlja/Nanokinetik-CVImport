using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using System.Threading;
using System.Collections.Generic;
using Ready.Model;
using System.DirectoryServices;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI
{
    public partial class LoginActiveDirectory : System.Web.UI.Page
    {
        private string _path;
        private string _filterAttribute;

        // Form initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Removes all session for current visitor
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
        }


        // Authenticating user
        protected void LoginForm_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string adPath = @"LDAP://zvjer"; //Path to your LDAP directory server
            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                if (true == adAuth.IsAuthenticated(LoginForm.UserName, LoginForm.Password))
                {
                    //string groups = adAuth.GetGroups();
                    //string groups = "Users";

                    ////Create the ticket, and add the groups.
                    ////bool isCookiePersistent = chkPersist.Checked;
                    //bool isCookiePersistent = true;
                    //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                    //          LoginForm.UserName, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);

                    ////Encrypt the ticket.
                    //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                    ////Create a cookie, and then add the encrypted ticket to the cookie as data.
                    //HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    //if (true == isCookiePersistent)
                    //    authCookie.Expires = authTicket.Expiration;

                    ////Add the cookie to the outgoing cookies collection.
                    //Response.Cookies.Add(authCookie);

                    //You can redirect now.
                    //Response.Redirect(FormsAuthentication.GetRedirectUrl(LoginForm.UserName, false));
                    FormsAuthentication.RedirectFromLoginPage(LoginForm.UserName, false);
                    
                    
                }
                else
                {
                    LoginForm.FailureText = "Authentication did not succeed. Check username and password.";
                }
            }
            catch (Exception ex)
            {
                LoginForm.FailureText = "Error authenticating. " + ex.Message;
            }

        }

        public bool IsAuthenticated(string username, string pwd)
        {
            DirectoryEntry entry = new DirectoryEntry(_path, username, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }
    }
}
