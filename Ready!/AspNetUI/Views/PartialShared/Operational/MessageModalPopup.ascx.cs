using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class MessageModalPopup : System.Web.UI.UserControl, IModalPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            messageModalPopupContainer.Style["display"] = "none";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            messageModalPopupContainer.Style["display"] = "none";
        }

        #region IModalPopup Members

        public string ModalPopupContainerWidth
        {
            get { return messageModalPopupContainer.Style["Width"]; }
            set { messageModalPopupContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return messageModalPopupContainer.Style["Height"]; }
            set { messageModalPopupContainer.Style["Height"] = value; }
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

            messageModalPopupContainer.Style["display"] = "inline";
        }

        #endregion
    }
}