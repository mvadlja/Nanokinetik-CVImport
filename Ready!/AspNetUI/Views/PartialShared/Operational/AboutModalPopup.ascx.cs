using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using System.Configuration;

namespace AspNetUI.Support
{
    public partial class AboutModalPopup : System.Web.UI.UserControl, IModalPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                lblVersion.Text = ConfigurationManager.AppSettings["ApplicationVersion"].ToString();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            aboutModalPopupContainer.Style["display"] = "none";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            aboutModalPopupContainer.Style["display"] = "none";
        }

        #region IModalPopup Members

        public string ModalPopupContainerWidth
        {
            get { return aboutModalPopupContainer.Style["Width"]; }
            set { aboutModalPopupContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return aboutModalPopupContainer.Style["Height"]; }
            set { aboutModalPopupContainer.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        public void ShowModalPopup(string header, string message)
        {
            aboutModalPopupContainer.Style["display"] = "inline";
        }

        #endregion
    }
}