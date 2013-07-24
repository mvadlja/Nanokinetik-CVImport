using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI
{
    public partial class Login : System.Web.UI.Page
    {
        IUSEROperations _userOperations;
        IAd_domain_PKOperations _adDomainOperations;

        // Form initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _userOperations = new USERDAL();
            _adDomainOperations = new Ad_domain_PKDAL();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Removes all session for current visitor
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
        }

        protected void LoginForm_Authenticate(object sender, AuthenticateEventArgs e)
        {
            USER tmpUser = _userOperations.GetUserByUsername(LoginForm.UserName);
            string domain = "";
            if (tmpUser != null && tmpUser.AdDomain != null)
            {
                Ad_domain_PK d = _adDomainOperations.GetEntity(tmpUser.AdDomain);
                if (d != null) domain = d.domain_connection_string;
            }

            if (tmpUser != null && tmpUser.IsAdUser == true)
            {
                LoginAd(domain);
            }
            else
            {
                LoginClassic();
            }
        }

        protected void LoginClassic()
        {
            USER user = _userOperations.AuthenticateUser(LoginForm.UserName, FormsAuthentication.HashPasswordForStoringInConfigFile(LoginForm.Password, "SHA1"));

            if (user != null && user.Active != null && (bool)user.Active)
            {
                ClearUserData();

                FormsAuthentication.RedirectFromLoginPage(user.Username, LoginForm.RememberMeSet);
            }
        }

        protected void LoginAd(string domain)
        {
            string adPath;
            if (domain.StartsWith("LDAP://")) adPath = domain;
            else adPath = "LDAP://" + domain;

            var adAuth = new LdapAuthentication(adPath);
            try
            {
                if (adAuth.IsAuthenticated(LoginForm.UserName, LoginForm.Password))
                {
                    ClearUserData();
                    FormsAuthentication.RedirectFromLoginPage(LoginForm.UserName, false);
                }
            }
            catch (Exception ex)
            {
                //LoginForm.FailureText = ex.Message;
            }
        }

        private void ClearUserData()
        {
            var viewIdS = new[] { "AllTimeUnitGrid", "TimeUnitGrid", "AddAllNewGrids" };
            foreach (String s in viewIdS)
            {
                if (Request.Cookies["dragtable-" + s] != null)
                {
                    var c = new HttpCookie("dragtable-" + s, "prazno") { Expires = DateTime.Now.AddYears(-1), Path = "/" };

                    if (Response.Cookies["dragtable-" + s] != null) Response.SetCookie(c);
                    else Response.Cookies.Add(c);
                }

                if (Request.Cookies["column-sizes-" + s] != null)
                {
                    var c = new HttpCookie("column-sizes-" + s, "prazno") { Expires = DateTime.Now.AddYears(-1), Path = "/" };

                    if (Response.Cookies["column-sizes-" + s] != null) Response.SetCookie(c);
                    else Response.Cookies.Add(c);
                }

                if (Request.Cookies[s + "-rows-state"] != null)
                {
                    var c = new HttpCookie(s + "-rows-state", "prazno") { Expires = DateTime.Now.AddYears(-1), Path = "/" };

                    if (Response.Cookies[s + "-rows-state"] != null) Response.SetCookie(c);
                    else Response.Cookies.Add(c);
                }
            }
        }
    }
}
