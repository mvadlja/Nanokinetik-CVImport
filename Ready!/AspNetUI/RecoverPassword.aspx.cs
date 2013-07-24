using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using AspNetUIFramework;
using System.Net.Mail;
using Ready.Model;
using CommonComponents;

namespace AspNetUI
{
    public partial class RecoverPassword : System.Web.UI.Page
    {
        IUSEROperations _userOperations;
        USER _user;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _userOperations = new USERDAL();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lblMsg.Text = "";

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                Label1.Text = "v" + ConfigurationManager.AppSettings["ApplicationVersion"].ToString() + " ";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // E-mail Message
                string newPass = GenerateRandomPassword();
                string message = "";
                message = "<br />Please use this new randomly generated password to login to the Ready application:<br /><hr><br />";
                message += "Username: <b>" + _user.Username + "</b><br/ >";
                message += "Password: <b>" + newPass + "</b><br />";
                message += "<hr><br />";

                try
                {
                    // Change password
                    _user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass, "SHA1");
                    _userOperations.Save(_user);

                    // Send validation mail

                    List<string> recipients = new List<string>() {ctlEmail.ControlValue.ToString()};
                    EmailOperations.SendEmail("Ready - Your new generated password", message, true, recipients, "smtp_1", MailPriority.High);

                    lblMsg.Text = "An email with a new password has been sent to you.<br />";
                    ctlEmail.ControlValue = "";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Password recovery failed.<br />";
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }


        private bool ValidateForm()
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlEmail.ControlValue.ToString())) errorMessage += ctlEmail.ControlEmptyErrorMessage + "<br />";
            else if (!ValidationHelper.IsValidEmail(ctlEmail.ControlValue.ToString())) errorMessage += ctlEmail.ControlErrorMessage + "<br />";
            else
            {
                _user = _userOperations.GetUserByEmail(ctlEmail.ControlValue == null ? "" : ctlEmail.ControlValue.ToString());

                if (_user == null)
                    errorMessage += "Password recovery failed.<br />";
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                lblMsg.Text = errorMessage;
                return false;
            }
            else return true;
        }

        private string GenerateRandomPassword()
        {
            char[] randomPassword = new char[6];

            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                randomPassword[i] = (char)r.Next(97, 122);
            }

            return (new string(randomPassword));
        }
    }
}
