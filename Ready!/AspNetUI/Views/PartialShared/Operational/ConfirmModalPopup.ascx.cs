using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class ConfirmModalPopup : System.Web.UI.UserControl, IModalPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IModalPopup Members

        public string ModalPopupContainerWidth
        {
            get { return confirmModalPopupContainer.Style["Width"]; }
            set { confirmModalPopupContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return confirmModalPopupContainer.Style["Height"]; }
            set { confirmModalPopupContainer.Style["Height"] = value; }
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

            confirmModalPopupContainer.Style["display"] = "inline";
        }

        #endregion
    }
}