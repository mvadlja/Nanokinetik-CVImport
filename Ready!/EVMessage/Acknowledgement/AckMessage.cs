using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Acknowledgement
{
    public class AckMessage
    {
        EVMessage.Acknowledgement.evprmack message;

        public evprmack Message
        {
            get { return (evprmack)message; }
            set { message = value; }
        }


        public AckMessage()
        {
            // create xsd-based message and set up default values
            Message = new evprmack();
        }

        public String MessageXML
        {
            get
            {
                String msgXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + this.message.ToString();
                msgXML = msgXML.Replace("<evprmack>", "<evprmack xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://eudravigilance.ema.europa.eu/schema/ackxevmpd.xsd\">");
                return msgXML;
            }
        }
        protected void GenAcknoledgementSection()
        {
            Message.acknowledgment = new acknowledgment();
            Message.acknowledgment.messageacknowledgment = new messageacknowledgment();
        }

        public void GenerateMessageError(string messageNr)
        {
            GenerateHeader(messageNr);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "03";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;
        }
        public void GenerateFieldError(string messageNr)
        {
            GenerateHeader(messageNr);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "02";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;
        }

        public void GenerateNoError(string messageNr)
        {
            GenerateHeader(messageNr);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "01";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;

            Message.acknowledgment.reportacknowledgment = new reportacknowledgment[3];

        }

        public void GenerateNoError(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg, String senderIdentifier, String receiverIdentifier)
        {
            if (msg == null)
            {
                GenerateErrors(msg, senderIdentifier, receiverIdentifier);
                return;
            }
            GenerateHeader(msg, senderIdentifier, receiverIdentifier);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "01";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagedateformat = "204";
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
            Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
            Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);

            var reportAckS = new List<reportacknowledgment>();

            if (msg.attachments != null && msg.attachments.attachment != null && msg.attachments.attachment.Count > 0)
            {
                foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.attachmentType attachment in msg.attachments.attachment)
                {
                    int operationType = attachment.operationtype;

                    var ack = new reportacknowledgment();
                    ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                    ack.localnumber = attachment.localnumber;
                    ack.operationresult = operationType == 1 ? 2 : operationType == 2 || operationType == 3 ? 4 : 3;
                    ack.operationtype = operationType;
                    string operation = operationType == 1 ? "inserted" : operationType == 2 || operationType == 3 ? "updated" : operationType == 4 ? "nullified" : "withdrawn";
                    ack.operationresultdesc = "Entity " + operation + " successfully.";
                    ack.reportname = "ATTACHMENT";
                    reportAckS.Add(ack);
                }
            }
            if (msg.authorisedproducts != null && msg.authorisedproducts.authorisedproduct != null && msg.authorisedproducts.authorisedproduct.Count > 0)
            {
                foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.authorisedproductType ap in msg.authorisedproducts.authorisedproduct)
                {
                    int operationType = ap.operationtype;
                    var ack = new reportacknowledgment();

                    if (operationType == 1)
                    {
                        ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                        ack.localnumber = ap.localnumber;
                    }
                    else
                    {
                        ack.ev_code = ap.ev_code;
                    }

                    ack.operationresult = operationType == 1 ? 2 : operationType == 2 || operationType == 3 ? 4 : 3;
                    ack.operationtype = operationType;
                    string operation = operationType == 1 ? "inserted" : operationType == 2 || operationType == 3 ? "updated" : operationType == 4 ? "nullified" : "withdrawn";
                    ack.operationresultdesc = "Entity " + operation + " successfully";
                    ack.reportname = "AUTHORISEDPRODUCT";
                    reportAckS.Add(ack);
                }
            }
            Message.acknowledgment.reportacknowledgment = reportAckS.ToArray();

        }


        public void GenerateFieldError(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg, String senderIdentifier, String receiverIdentifier)
        {
            if (msg == null)
            {
                GenerateErrors(msg, senderIdentifier, receiverIdentifier);
                return;
            }
            GenerateHeader(msg, senderIdentifier, receiverIdentifier);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "02";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagedateformat = "204";
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
            Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
            Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);


            List<reportacknowledgment> reportAckS = new List<reportacknowledgment>();
            bool errorImplemented = false;

            if (msg.attachments != null && msg.attachments.attachment != null && msg.attachments.attachment.Count > 0)
            {
                foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.attachmentType attachment in msg.attachments.attachment)
                {
                    reportacknowledgment ack = new reportacknowledgment();
                    ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                    ack.localnumber = attachment.localnumber;
                    if (!errorImplemented)
                    {
                        ack.operationresult = 13;
                        ack.operationresultdesc = "Error occured, constraint: UNKNOWN CONSTRAINT!";
                        errorImplemented = true;
                    }
                    else
                    {
                        ack.operationresult = 2;
                        ack.operationresultdesc = "Entity inserted successfully";
                    }
                    ack.operationtype = attachment.operationtype;

                    ack.reportname = "ATTACHMENT";
                    reportAckS.Add(ack);
                }
            }

            if (msg.authorisedproducts != null && msg.authorisedproducts.authorisedproduct != null && msg.authorisedproducts.authorisedproduct.Count > 0)
            {
                foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.authorisedproductType ap in msg.authorisedproducts.authorisedproduct)
                {
                    int operationType = ap.operationtype;

                    reportacknowledgment ack = new reportacknowledgment();
                    if (operationType == 1)
                    {
                        ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                        ack.localnumber = ap.localnumber;
                    }
                    else
                    {
                        ack.ev_code = ap.ev_code;
                    }

                    ack.operationtype = ap.operationtype;
                    if (!errorImplemented)
                    {
                        ack.operationresult = 13;
                        ack.operationresultdesc = "Error occured, constraint: UNKNOWN CONSTRAINT!";
                        errorImplemented = true;
                    }
                    else
                    {
                        ack.operationresult = 2;
                        ack.operationresultdesc = "Errors";
                    }
                    ack.reportname = "AUTHORISEDPRODUCT";
                    reportAckS.Add(ack);
                }
            }

            Message.acknowledgment.reportacknowledgment = reportAckS.ToArray();

        }

        public void GenerateErrors(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg, String senderIdentifier, String receiverIdentifier)
        {
            GenerateHeader(msg, senderIdentifier, receiverIdentifier);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = "03";
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagedateformat = "204";
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
            Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
            Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);
            Message.acknowledgment.messageacknowledgment.parsingerrormessage = "Message is not valid according to provided XSD schema!";



        }
        public void From(string xml)
        {

        }

        public void GenerateHeader(string messageNr)
        {
            Message.ichicsrmessageheader = new ichicsrmessageheader();
            Message.ichicsrmessageheader.messagetype = new EVMessage.Acknowledgement.messagetype() { TypedValue = "EVPRACK" };
            Message.ichicsrmessageheader.messagedate = new EVMessage.Acknowledgement.messagedate() { TypedValue = DateTime.Now.ToString("yyyyMMddHHmmss") };
            Message.ichicsrmessageheader.messagedateformat = new EVMessage.Acknowledgement.messagedateformat() { TypedValue = "204" };
            Message.ichicsrmessageheader.messagereceiveridentifier = new EVMessage.Acknowledgement.messagereceiveridentifier() { TypedValue = "BPET" };
            Message.ichicsrmessageheader.messagenumb = new EVMessage.Acknowledgement.messagenumb() { TypedValue = messageNr };
            Message.ichicsrmessageheader.messagesenderidentifier = new EVMessage.Acknowledgement.messagesenderidentifier() { TypedValue = "EVTEST" };
        }

        public void GenerateHeader(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg, String senderIdentifier, String receiverIdentifier)
        {


            Message.ichicsrmessageheader = new ichicsrmessageheader();
            Message.ichicsrmessageheader.messagetype = new EVMessage.Acknowledgement.messagetype() { TypedValue = "EVPRACK" };
            Message.ichicsrmessageheader.messagedate = new EVMessage.Acknowledgement.messagedate() { TypedValue = DateTime.Now.ToString("yyyyMMddHHmmss") };
            Message.ichicsrmessageheader.messagedateformat = new EVMessage.Acknowledgement.messagedateformat() { TypedValue = "204" };
            Message.ichicsrmessageheader.messagereceiveridentifier = new EVMessage.Acknowledgement.messagereceiveridentifier() { TypedValue = receiverIdentifier };
            Message.ichicsrmessageheader.messagenumb = new EVMessage.Acknowledgement.messagenumb() { TypedValue = Guid.NewGuid().ToString("N").Substring(16) };
            Message.ichicsrmessageheader.messagesenderidentifier = new EVMessage.Acknowledgement.messagesenderidentifier() { TypedValue = senderIdentifier };
        }
    }
}
