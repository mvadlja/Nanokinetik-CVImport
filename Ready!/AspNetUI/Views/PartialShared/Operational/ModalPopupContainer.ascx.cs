using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class ModalPopupContainer : System.Web.UI.UserControl, IModalPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            modalPopupContainer.Style["display"] = "none";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            modalPopupContainer.Style["display"] = "none";
        }

        #region IModalPopup Members

        public string ModalPopupContainerWidth
        {
            get { return modalPopupContainer.Style["Width"]; }
            set { modalPopupContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return modalPopupContainer.Style["Height"]; }
            set { modalPopupContainer.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        public void ShowModalPopup(string header, string message)
        {
            divHeader.InnerHtml = header;
            divMessage.InnerHtml = message;

            modalPopupContainer.Style["display"] = "inline";
        }

        #endregion
    }
}