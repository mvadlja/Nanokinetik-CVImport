using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Ready.Model;
using System.Data;

namespace xEVMPD
{
    /// <summary>
    /// ACK - Acknowledgement message
    /// </summary>
    public partial class ACKMessage: EVMessageBase
    {
        public evprmack Message
        {
            get { return (evprmack)message; }
            set { message = value; }
        }

        protected override Type MyType
        {
            get { return typeof(evprmack); }
        }

        public ACKMessage_Status MessageStatus {
            get { return (ACKMessage_Status)(int)Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode; }
        }

        public ACKMessage()
        {
            // create xsd-based message and set up default values
            Message = new evprmack();            
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
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item03;
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;
        }
        public void GenerateFieldError(string messageNr) 
        {
            GenerateHeader(messageNr);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item02;
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;
        }

        public void GenerateNoError(string messageNr)
        {
            GenerateHeader(messageNr);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item01;
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = messageNr;
         
            Message.acknowledgment.reportacknowledgment = new reportacknowledgment[3];
           
        }
        
         public void GenerateNoError(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg)
        {
            GenerateHeader(msg);
            GenAcknoledgementSection();
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item01;
            Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagedateformat = messageacknowledgmentOriginalmessagedateformat.Item204;
            Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
            Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
            Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
            Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);
            Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item01;

            List<reportacknowledgment> reportAckS = new List<reportacknowledgment>();

            foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.attachmentType attachment in msg.attachments.attachment)
            {
                reportacknowledgment ack = new reportacknowledgment();
                ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                ack.localnumber = attachment.localnumber;
                ack.operationresult = "2";
                ack.operationtype = attachment.operationtype.ToString();
                ack.operationresultdesc = "Entity inserted successfully";
                ack.reportname = reportacknowledgmentReportname.ATTACHMENT;
                reportAckS.Add(ack);
            }
            foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.authorisedproductType ap in msg.authorisedproducts.authorisedproduct)
            {
                reportacknowledgment ack = new reportacknowledgment();
                ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                ack.localnumber = ap.localnumber;
                ack.operationresult = "2";
                ack.operationtype = ap.operationtype.ToString();
                ack.operationresultdesc = "Entity inserted successfully";
                ack.reportname = reportacknowledgmentReportname.AUTHORISEDPRODUCT;
                reportAckS.Add(ack);
            }

            Message.acknowledgment.reportacknowledgment = reportAckS.ToArray();
           
        }


         public void GenerateFieldError(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg)
         {
             GenerateHeader(msg);
             GenAcknoledgementSection();
             Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item02;
             Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
             Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
             Message.acknowledgment.messageacknowledgment.originalmessagedateformat = messageacknowledgmentOriginalmessagedateformat.Item204;
             Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
             Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
             Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
             Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);


             List<reportacknowledgment> reportAckS = new List<reportacknowledgment>();
             bool errorImplemented = false;
             foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.attachmentType attachment in msg.attachments.attachment)
             {
                 reportacknowledgment ack = new reportacknowledgment();
                 ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                 ack.localnumber = attachment.localnumber;
                 if (!errorImplemented)
                 {
                     ack.operationresult = "13";
                     ack.operationresultdesc = "Error occured, constraint: UNKNOWN CONSTRAINT!";
                     errorImplemented = true;
                 }
                 else
                 {
                     ack.operationresult = "2";
                     ack.operationresultdesc = "Entity inserted successfully";
                 }
                 ack.operationtype = attachment.operationtype.ToString();

                 ack.reportname = reportacknowledgmentReportname.ATTACHMENT;
                 reportAckS.Add(ack);
             }
             foreach (eudravigilance.ema.europa.eu.schema.emaxevmpd.authorisedproductType ap in msg.authorisedproducts.authorisedproduct)
             {
                 reportacknowledgment ack = new reportacknowledgment();
                 ack.ev_code = Guid.NewGuid().ToString("N").Substring(10);
                 ack.localnumber = ap.localnumber;
         
                 ack.operationtype = ap.operationtype.ToString();
                 if (!errorImplemented)
                 {
                     ack.operationresult = "13";
                     ack.operationresultdesc = "Error occured, constraint: UNKNOWN CONSTRAINT!";
                     errorImplemented = true;
                 }
                 else
                 {
                     ack.operationresult = "2";
                     ack.operationresultdesc = "Entity inserted successfully";
                 }
                 ack.reportname = reportacknowledgmentReportname.AUTHORISEDPRODUCT;
                 reportAckS.Add(ack);
             }

             Message.acknowledgment.reportacknowledgment = reportAckS.ToArray();

         }

         public void GenerateErrors(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg)
         {
             GenerateHeader(msg);
             GenAcknoledgementSection();
             Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode = messageacknowledgmentTransmissionacknowledgmentcode.Item03;
             Message.acknowledgment.messageacknowledgment.originalmessagenumb = msg.ichicsrmessageheader.messagenumb;
             Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
             Message.acknowledgment.messageacknowledgment.originalmessagedateformat = messageacknowledgmentOriginalmessagedateformat.Item204;
             Message.acknowledgment.messageacknowledgment.originalmessagedate = msg.ichicsrmessageheader.messagedate;
             Message.acknowledgment.messageacknowledgment.originalmessagereceiveridentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
             Message.acknowledgment.messageacknowledgment.originalmessagesenderidentifier = msg.ichicsrmessageheader.messagesenderidentifier;
             Message.acknowledgment.messageacknowledgment.evmessagenumb = Guid.NewGuid().ToString("N").Substring(6);
             Message.acknowledgment.messageacknowledgment.parsingerrormessage = "Message is not valid according to provided XSD schema!";

      

         }
        public void From(string xml)
        {
            XmlSerializer ser;
            ser = new XmlSerializer(typeof(evprmack));

            UnicodeEncoding encoding = new UnicodeEncoding();
            Byte[] byteArray = encoding.GetBytes(xml);
            Stream s = new MemoryStream(byteArray);
                        
            XmlTextReader xmlReader = new XmlTextReader(s);                       
            message = ser.Deserialize(xmlReader);
            xmlReader.Close();            
        }

        public void GenerateHeader(string messageNr) {
            Message.ichicsrmessageheader = new ichicsrmessageheader();
            Message.ichicsrmessageheader.messagetype = messagetype.EVPRACK;
            Message.ichicsrmessageheader.messagedate = DateTime.Now.ToString("MM-dd-yyyy");
            Message.ichicsrmessageheader.messagedateformat = messagedateformat.Item204;
            Message.ichicsrmessageheader.messagereceiveridentifier = "BPET";            
            Message.ichicsrmessageheader.messagenumb = messageNr;
            Message.ichicsrmessageheader.messagesenderidentifier = "EVTEST";
        }

        public void GenerateHeader(eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg)
        {
            Message.ichicsrmessageheader = new ichicsrmessageheader();
            Message.ichicsrmessageheader.messagetype = messagetype.EVPRACK;
            Message.ichicsrmessageheader.messagedate = DateTime.Now.ToString("MM-dd-yyyy");
            Message.ichicsrmessageheader.messagedateformat = messagedateformat.Item204;
            Message.ichicsrmessageheader.messagereceiveridentifier = msg.ichicsrmessageheader.messagesenderidentifier;
            Message.ichicsrmessageheader.messagenumb = Guid.NewGuid().ToString("N").Substring(16);
            Message.ichicsrmessageheader.messagesenderidentifier = msg.ichicsrmessageheader.messagereceiveridentifier;
        }
        
        public  String ToXMLString {
            get {
                try
                {
                    StringBuilder xmlSB = new StringBuilder();
                    string evprm = Message.ToString();
                    evprm = evprm.Replace("<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\">",
                            "<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\" xmlns:ssi=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd_ssi\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd http://eudravigilance.ema.europa.eu/schema/emaxevmpd.xsd\">");

                    xmlSB.Append(evprm);
                    return xmlSB.ToString();
                }
                catch
                {
                    return String.Empty;
                }
            }
        }
    }

    public enum ACKMessage_Status
    {
        NoErrors = 1,
        FieldErrors,
        CompleteMessageRejected
    }

       
}
