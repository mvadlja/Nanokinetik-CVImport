using System;
using System.Configuration;
using System.Web.UI;
using AspNetUI.Support;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.RestrictedAreaView
{
    public partial class ErrorInfo : Page
    {
        private Uri RestrictedAreaUrlRefferer
        {
            get { return (Uri)ViewState["RestrictedAreaUrlRefferer"]; }
            set { ViewState["RestrictedAreaUrlRefferer"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lblInfo.Text = "<center>You've tried to access restricted area for which you don't have sufficient privileges!</center>";
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                lblAppVersion.Text = string.Format("v{0} ", ConfigurationManager.AppSettings["ApplicationVersion"]);
            }

            if (!IsPostBack && Request.UrlReferrer != null) RestrictedAreaUrlRefferer = Request.UrlReferrer;

            IPerson_PKOperations personOperations = new Person_PKDAL();
            if (SessionManager.Instance.CurrentUser != null)
            {
                Person_PK person = personOperations.GetPersonByUserID(SessionManager.Instance.CurrentUser.UserID);
                if (person != null)
                {
                    lblLoginName.Text = person.FullName;
                    lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                    lbtLogOut.Visible = true;
                }
                else
                {
                    lblLoginName.Text = SessionManager.Instance.CurrentUser == null? String.Empty: SessionManager.Instance.CurrentUser.Username;
                    lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                    lbtLogOut.Visible = true;
                }
            }
            else
            {
                if (ValidationHelper.IsValidInt(Convert.ToString(Session["UserID"])))
                {
                    int? userID = Convert.ToInt32(Session["UserID"]);
                    Person_PK person = personOperations.GetPersonByUserID(userID);
                    if (person != null)
                    {
                        lblLoginName.Text = person.FullName;
                        lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                        lbtLogOut.Visible = true;
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            btnBack.Visible = RestrictedAreaUrlRefferer != null && !string.IsNullOrWhiteSpace(RestrictedAreaUrlRefferer.AbsoluteUri);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RestrictedAreaUrlRefferer.AbsoluteUri))
            {
                Session["OneTimePermissionToken"] = Permission.View;
                Response.Redirect(RestrictedAreaUrlRefferer.AbsoluteUri);
            }
            else SecurityHelper.RedirectToLoginPage();
        }

        protected void lbtLogOut_Click(object sender, EventArgs e)
        {
            SecurityHelper.RedirectToLoginPage();
        }       

        protected void lbtAbout_Click(object sender, EventArgs e)
        {
            aboutModalPopup.ShowModalPopup("", "");
        }
    }
}
