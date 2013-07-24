using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using xEVMPD;
using Ready.Model;
using System.Configuration;
using System.IO;
using EVMessage.Xevprm;

namespace AspNetUI.Support
{
    public partial class MessageErrorPopup : System.Web.UI.UserControl, IModalPopup
    {
        private List<string> xmlParsingError
        {
            get { return (List<string>)ViewState["MessageErrorPopup_xmlParsingError"]; }
            set { ViewState["MessageErrorPopup_xmlParsingError"] = value; }
        }

        private int msgid
        {
            get { return (int)ViewState["MessageErrorPopup_msgid"]; }
            set { ViewState["MessageErrorPopup_msgid"] = value; }
        }

        private XevprmOperationType operationType
        {
            get { return (XevprmOperationType)ViewState["MessageErrorPopup_operationType"]; }
            set { ViewState["MessageErrorPopup_operationType"] = value; }
        }

        public virtual event EventHandler<FormDetailsEventArgs> OnNoMoreErrors;
        //public virtual event EventHandler<FormDetailsEventArgs> onExportClick;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnValidate.Visible = false;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (xmlParsingError == null)
            {
                xmlParsingError = new List<string>();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            var _xevprm_message_PKOperation = new Xevprm_message_PKDAL();

            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msgid);
            xEVPRMessage msg = new xEVPRMessage();
            xmlParsingError = msg.ConstructFrom(message);
            message.xml = msg.ToXmlString();
            _xevprm_message_PKOperation.Save(message);

            messageModalPopupContainer.Style["display"] = "none";

            ScriptManager sm = (ScriptManager)Page.Master.FindControl("smMain");
            sm.RegisterPostBackControl(btnExport);

        }


        protected void btnValidate_Click(object sender, EventArgs e)
        {
            btnValidate.Visible = false;
            divMessage.Controls.Clear();

            var _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msgid);

            IXevprm_ap_details_PKOperations _xevprm_ap_details_PKOperations = new Xevprm_ap_details_PKDAL();
            Xevprm_ap_details_PK messageAPDetails = _xevprm_ap_details_PKOperations.GetEntityForXevprm(message.xevprm_message_PK);

            Control errors = xEVPRMessage.ValidationErrorsAP(msgid, messageAPDetails.OperationType);
            ShowModalPopup(divHeader.InnerHtml, errors, msgid);
        }

        protected void btnExportToExcel(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Business/ValidationReport.aspx?msgID=" + msgid, "_blank", "");
            btnValidate_Click(null, null);

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            var _xevprm_message_PKOperation = new Xevprm_message_PKDAL();

            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msgid);

            
            //xEVPRMessage msg = new xEVPRMessage();
            //msg.ConstructFrom(message);
            //message.XML = msg.ToXmlString();
            //_xevprm_message_PKOperation.Save(message);


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

        public void ShowModalPopup(string header, Control control, int msgID)
        {
            bool isValidationOK = control.Controls.Count == 0 ? true : false;
            int stringLenght = 500;
            msgid = msgID;
            divHeader.InnerHtml = header;

            divMessage.Controls.Add(new LiteralControl("<br />"));
            divMessage.Controls.Add(new LiteralControl("<table border=1 style=\"width:100%;border-spacing:0px;border-collapse;\">")); //margin-left: auto; margin-right: auto;border-spacing:0px;border-collapse;\">"));

            foreach (Control levelOne in control.Controls)
            {
                divMessage.Controls.Add(new LiteralControl("<tr>"));
                if (((levelOne as Label).Text).Trim().ToLower().Contains("pharmaceutical products"))
                {
                    bool noErrors = true;
                    foreach (Control item in levelOne.Controls)
                    {
                        if (item.Controls.Count != 0)
                        {
                            noErrors = false;
                            break;
                        }
                    }
                    if (noErrors)//(levelOne.Controls.Count > 0 && levelOne.Controls.Count == 0)
                        continue;

                    Label parent = new Label();
                    if ((levelOne as Label).Text.Length > stringLenght)
                    {
                        parent.ToolTip = (levelOne as Label).Text;
                        parent.Text = (levelOne as Label).Text.Substring(0, stringLenght - 4) + "..." + "<br />";
                    }
                    else
                        parent.Text = "<div style=\"margin-top:4px;\">" + (levelOne as Label).Text + "<br />" + "</div>"; ;

                    divMessage.Controls.Add(new LiteralControl("<td>"));
                    divMessage.Controls.Add(parent);
                }
                else
                {
                    //Display AP,P
                    Label parent = new Label();
                    if ((levelOne as Label).Text.Length > stringLenght)
                    {
                        parent.ToolTip = (levelOne as Label).Text;
                        parent.Text = (levelOne as Label).Text.Substring(0, stringLenght - 4) + "...";
                    }
                    else
                        parent.Text = "<div style=\"margin-top:4px;\">" + (levelOne as Label).Text + "</div>";


                    divMessage.Controls.Add(new LiteralControl("<td>"));
                    divMessage.Controls.Add(parent);
                }

                divMessage.Controls.Add(new LiteralControl("<table border=0 style=\"margin-left: auto; margin-right: auto;text-align: left;width:100%;margin-top: 2px;\">"));
                foreach (Control levelTwo in levelOne.Controls)
                {
                    divMessage.Controls.Add(new LiteralControl("<tr>"));
                    //Display AP and P errors
                    //divMessage.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    //divMessage.Controls.Add(new LiteralControl("<td>"));

                    if (levelTwo is HyperLink)
                    {
                        HyperLink hl = new HyperLink();
                        hl.Text = (levelTwo as HyperLink).Text;
                        hl.NavigateUrl = (levelTwo as HyperLink).NavigateUrl;
                        hl.Target = (levelTwo as HyperLink).Target;

                        if (hl.Text.Length > stringLenght)
                        {
                            hl.ToolTip = hl.Text;
                            hl.Text = hl.Text.Substring(0, stringLenght - 4) + "...";
                        }

                        if (hl.Text.Contains(':'))
                        {
                            string[] hlText = hl.Text.Split(':');
                            //hl.Text = "<td>" + hlText[0] + "</td><td>" + hlText[1] + "</td>";
                            divMessage.Controls.Add(new LiteralControl("<td valign=\"top\"  style=\"width:20%\">" + hlText[0] + "</td><td valign=\"top\" ><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hlText[1] + "</a></td>"));
                        }
                        else
                        {
                            divMessage.Controls.Add(new LiteralControl("<td valign=\"top\"  style=\"width:20%\">&nbsp;</td><td valign=\"top\" ><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hl.Text + "</a></td>"));
                        }


                        //divMessage.Controls.Add(hl);
                        //divMessage.Controls.Add(new LiteralControl("<br />"));
                    }
                    else if (levelTwo is LiteralControl)
                    {
                        String literalControl = (levelTwo as LiteralControl).Text;
                        Label lc = new Label();
                        if (literalControl.Length > stringLenght)
                        {
                            lc.ToolTip = literalControl;
                            lc.Text = literalControl.Substring(0, stringLenght - 4) + "...";
                        }
                        else
                            lc.Text = literalControl;


                        divMessage.Controls.Add(lc);
                        divMessage.Controls.Add(new LiteralControl("<br />"));
                    }
                    else if (levelTwo is Label)
                    {
                        if (levelTwo.Controls.Count != 0)
                        {


                            Label lvlThreeLabel = new Label();
                            if ((levelTwo as Label).Text.Length > stringLenght)
                            {
                                lvlThreeLabel.ToolTip = (levelTwo as Label).Text;
                                lvlThreeLabel.Text = (levelTwo as Label).Text.Substring(0, stringLenght - 4) + "...";
                            }
                            else
                                lvlThreeLabel.Text = "<td>" + (levelTwo as Label).Text;
                            //lvlThreeLabel.Text = "<div style=\"margin-top:4px;\">" + (levelTwo as Label).Text + "</div>";

                            divMessage.Controls.Add(new LiteralControl(lvlThreeLabel.Text));
                        }
                        //divMessage.Controls.Add(lvlThreeLabel);
                        //divMessage.Controls.Add(new LiteralControl("<br />"));
                    }
                    if (levelTwo.Controls.Count != 0)
                    {
                        divMessage.Controls.Add(new LiteralControl("<table border=0 style=\"margin-left: auto; margin-right: auto;text-align: left;width:100%;margin-top: 2px;\">"));
                        divMessage.Controls.Add(new LiteralControl("<tr>"));
                    }
                    //Display PP errors
                    foreach (Control levelThree in levelTwo.Controls)
                    {
                        if (levelTwo.Controls.Count != 0)
                        {
                            divMessage.Controls.Add(new LiteralControl("<table border=0 style=\"margin-left: auto; margin-right: auto;text-align: left;width:100%;margin-top: 2px;\">"));
                            divMessage.Controls.Add(new LiteralControl("<tr>"));
                        }
                        //divMessage.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));

                        if (levelThree is HyperLink)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = (levelThree as HyperLink).Text;
                            if (hl.Text.Length > stringLenght)
                            {
                                hl.ToolTip = hl.Text;
                                hl.Text = hl.Text.Substring(0, stringLenght - 4) + "...";
                            }
                            hl.NavigateUrl = (levelThree as HyperLink).NavigateUrl;
                            hl.Target = (levelThree as HyperLink).Target;
                            if (hl.Text.Contains(':'))
                            {
                                string[] hlText = hl.Text.Split(':');
                                //hl.Text = "<td>" + hlText[0] + "</td><td>" + hlText[1] + "</td>";
                                divMessage.Controls.Add(new LiteralControl("<td valign=\"top\"  style=\"width:20%\">" + hlText[0] + "</td><td valign=\"top\" ><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hlText[1] + "</a></td>"));
                            }
                            else
                            {
                                divMessage.Controls.Add(new LiteralControl("<td valign=\"top\"  style=\"width:20%\">&nbsp;</td><td valign=\"top\" ><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hl.Text + "</a></td>"));
                            }
                            //divMessage.Controls.Add(hl);
                            //divMessage.Controls.Add(new LiteralControl("<br />"));
                        }
                        else if (levelThree is LiteralControl)
                        {
                            String literalControl = (levelThree as LiteralControl).Text;
                            Label lc = new Label();
                            if (literalControl.Length > stringLenght)
                            {
                                lc.ToolTip = literalControl;
                                lc.Text = literalControl.Substring(0, stringLenght - 4) + "...";
                            }
                            else
                                lc.Text = literalControl;

                            divMessage.Controls.Add(lc);
                            //divMessage.Controls.Add(new LiteralControl("<br />"));
                        }
                        else if (levelThree is Label)
                        {
                            Label lvlThreeLabel = new Label();
                            if ((levelThree as Label).Text.Length > stringLenght)
                            {
                                lvlThreeLabel.ToolTip = (levelThree as Label).Text;
                                lvlThreeLabel.Text = (levelThree as Label).Text.Substring(0, stringLenght - 4) + "...";
                            }
                            else
                                lvlThreeLabel.Text = "<div style=\"margin-top:2px;\">" + (levelThree as Label).Text + "</div>";

                            divMessage.Controls.Add(new LiteralControl(lvlThreeLabel.Text));

                            //divMessage.Controls.Add(lvlThreeLabel);
                            //divMessage.Controls.Add(new LiteralControl("<br />"));
                        }
                        foreach (var levelFour in levelThree.Controls)
                        {
                            if (levelFour is HyperLink)
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = (levelFour as HyperLink).Text;
                                if (hl.Text.Length > stringLenght)
                                {
                                    hl.ToolTip = hl.Text;
                                    hl.Text = hl.Text.Substring(0, stringLenght - 4) + "...";
                                }
                                hl.NavigateUrl = (levelFour as HyperLink).NavigateUrl;
                                hl.Target = (levelFour as HyperLink).Target;

                                
                                if (hl.Text.Contains(':'))
                                {
                                    string[] hlText = hl.Text.Split(':');
                                    //hl.Text = "<td>" + hlText[0] + "</td><td>" + hlText[1] + "</td>";
                                    divMessage.Controls.Add(new LiteralControl("<td style=\"width:20%\">" + hlText[0] + "</td><td><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hlText[1] + "</a></td>"));
                                }
                                else
                                {
                                    divMessage.Controls.Add(new LiteralControl("<td style=\"width:20%\">&nbsp;</td><td><a target=\"_blank\" href=\"" + hl.NavigateUrl + "\">" + hl.Text + "</a></td>"));
                                }
                                divMessage.Controls.Add(new LiteralControl("</tr>"));
                                divMessage.Controls.Add(new LiteralControl("<tr>"));
                            }
                        }
                        if (levelTwo.Controls.Count != 0)
                        {
                            divMessage.Controls.Add(new LiteralControl("</tr>"));
                            divMessage.Controls.Add(new LiteralControl("</table>"));
                        }
                    }

                    divMessage.Controls.Add(new LiteralControl("</td>"));
                    divMessage.Controls.Add(new LiteralControl("</tr>"));
                }
                divMessage.Controls.Add(new LiteralControl("</table>"));
                divMessage.Controls.Add(new LiteralControl("</td>"));
                divMessage.Controls.Add(new LiteralControl("</tr>"));
            }
            divMessage.Controls.Add(new LiteralControl("</table>"));
            divMessage.Controls.Add(new LiteralControl("<br />"));
            if (isValidationOK) //if only a table and it's elements remaings that means that there is no error
            {
                divMessage.Controls.Clear();

                btnExport.Visible = false;

                var _xevprm_message_PKOperation = new Xevprm_message_PKDAL();

                Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msgid);
                xEVPRMessage msg = new xEVPRMessage();
                xmlParsingError = msg.ConstructFrom(message);

                if (xmlParsingError.Count == 0)
                {
                    divMessage.Controls.Add(new LiteralControl("Validation OK"));
                    btnValidate.Visible = false;
                }
                else
                {
                    btnValidate.Visible = true;
                }

                if (OnNoMoreErrors != null && xmlParsingError.Count == 0)
                {
                    if (message.status == XevprmStatus.Created ||
                        message.status == XevprmStatus.NotReady)
                    {
                        Xevprm.UpdateXevprmEntityDetailsTablesWithCurrentData(message);
                        message.status = XevprmStatus.Ready;

                        message.xml = msg.ToXmlString();
                        message.xml_hash = XevprmHelper.ComputeHash(message.xml);

                        _xevprm_message_PKOperation.Save(message);
                    }

                    OnNoMoreErrors(null, new FormDetailsEventArgs(msgid));
                }
                //Display xml parsing errors
                else if (xmlParsingError.Count != 0)
                {
                    divMessage.Controls.Add(new LiteralControl("<table border=1 style=\"width:100%;border-spacing:0px;border-collapse;\">"));
                    divMessage.Controls.Add(new LiteralControl("<tr>"));
                    divMessage.Controls.Add(new LiteralControl("<td>"));
                    divMessage.Controls.Add(new LiteralControl("XML parsing errors"));
                    divMessage.Controls.Add(new LiteralControl("</td>"));
                    divMessage.Controls.Add(new LiteralControl("</tr>"));
                    divMessage.Controls.Add(new LiteralControl("<tr>"));
                    divMessage.Controls.Add(new LiteralControl("<td>"));
                    divMessage.Controls.Add(new LiteralControl("<table border=0 style=\"margin-left: auto; margin-right: auto;text-align: left;width:100%;margin-top: 2px;\">"));
                    foreach (string error in xmlParsingError)
                    {
                        string[] errorComponents = error.Split('|');
                        divMessage.Controls.Add(new LiteralControl("<tr>"));
                        divMessage.Controls.Add(new LiteralControl("<td valign=\"top\" align=\"center\" ><a target=\"_blank\" href=\"" + errorComponents[1] + "\">" + errorComponents[0] + "</a></td>"));
                        //divMessage.Controls.Add(new LiteralControl("<td>" + error + "</td>"));
                        divMessage.Controls.Add(new LiteralControl("</tr>"));

                    }
                    divMessage.Controls.Add(new LiteralControl("</table>"));
                    divMessage.Controls.Add(new LiteralControl("</td>"));
                    divMessage.Controls.Add(new LiteralControl("</tr>"));
                    divMessage.Controls.Add(new LiteralControl("</table>"));
                }
            }
            else
            {
                btnValidate.Visible = true;
            }
            messageModalPopupContainer.Style["display"] = "inline";
        }

        #endregion


        public void ShowModalPopup(string header, string message)
        {
            divHeader.InnerHtml = header;
            divMessage.InnerHtml = message;
            messageModalPopupContainer.Style["display"] = "inline";
        }
    }
}