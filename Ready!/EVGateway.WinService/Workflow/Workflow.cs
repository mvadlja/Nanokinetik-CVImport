using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonComponents;
using Ready.Model;
using System.Configuration;

namespace EVGateway.WinService.Workflow
{
    public partial class Workflow
    {
        #region Declarations

        readonly IXevprm_message_PKOperations _xevprmMessageOperation;
        readonly IRecieved_message_PKOperations _receivedMessageOperations;
        readonly ISent_message_PKOperations _sentMessageOperations;
        readonly IAuthorisedProductOperations _authorisedProductOperations;
        readonly IAttachment_PKOperations _attachmentOperations;

        private string _AS2ExchangePointUri;
        private string _EMAMDNReceiptURL;

        private string _MDNReceiptURL;
        private int _AS2Timeout;

        private string _AS2GatewayID;
        private string _MessageReceiverID;

        private Dictionary<string, string> _senderThumbprint;
        private Dictionary<string, string> _gatewayThumbprint;

        private string _serviceLogFile;

        #endregion

        #region Properties

        public string AS2GatewayID
        {
            get { return _AS2GatewayID; }
            set { _AS2GatewayID = value; }
        }

        public string MessageReceiverId
        {
            get { return _MessageReceiverID; }
            set { _MessageReceiverID = value; }
        }

        public Dictionary<string, string> SenderThumbprint
        {
            get { return _senderThumbprint; }
            set { _senderThumbprint = value; }
        }

        public Dictionary<string, string> GatewayThumbprint
        {
            get { return _gatewayThumbprint; }
            set { _gatewayThumbprint = value; }
        }

        public string AS2ExchangePointURI
        {
            get { return _AS2ExchangePointUri; }
            set { _AS2ExchangePointUri = value; }
        }

        public string EMAMDNReceiptURL
        {
            get { return _EMAMDNReceiptURL; }
            set { _EMAMDNReceiptURL = value; }
        }

        public string MDNReceiptURL
        {
            get { return _MDNReceiptURL; }
            set { _MDNReceiptURL = value; }
        }

        public int AS2Timeout
        {
            get { return _AS2Timeout; }
            set { _AS2Timeout = value; }
        }

        #endregion

        #region Constructors

        public Workflow()
        {
            _xevprmMessageOperation = new Xevprm_message_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _receivedMessageOperations = new Recieved_message_PKDAL();
            _sentMessageOperations = new Sent_message_PKDAL();

            _AS2GatewayID = ConfigurationManager.AppSettings["AS2GatewayID"];
            _MessageReceiverID = ConfigurationManager.AppSettings["MessageReceiverID"];

            _AS2ExchangePointUri = ConfigurationManager.AppSettings["AS2ExchangePointURI"];
            _MDNReceiptURL = ConfigurationManager.AppSettings["MDNReceiptURL"];
            _EMAMDNReceiptURL = ConfigurationManager.AppSettings["EMAMDNReceiptURL"];

            string senderThumbprintConfig = ConfigurationManager.AppSettings["AS2SenderIDs"];

            var senderThumbprintConfigList = !string.IsNullOrWhiteSpace(senderThumbprintConfig) ? senderThumbprintConfig.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

            SenderThumbprint = new Dictionary<string, string>();

            foreach (string item in senderThumbprintConfigList)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string[] senderThumbrintArray = item.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (senderThumbrintArray.Count() == 2 &&
                        !string.IsNullOrWhiteSpace(senderThumbrintArray[0]) &&
                        !string.IsNullOrWhiteSpace(senderThumbrintArray[1]))
                    {
                        SenderThumbprint.Add(senderThumbrintArray[0].Trim(), senderThumbrintArray[1].Trim());
                    }
                }
            }

            string gatewayThumbprintConfig = ConfigurationManager.AppSettings["AS2GatewayIDs"];

            List<string> gatewayThumbprintConfigList = !string.IsNullOrWhiteSpace(gatewayThumbprintConfig) ? gatewayThumbprintConfig.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

            GatewayThumbprint = new Dictionary<string, string>();

            foreach (string item in gatewayThumbprintConfigList)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string[] gatewayThumbrintArray = item.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (gatewayThumbrintArray.Count() == 2 &&
                        !string.IsNullOrWhiteSpace(gatewayThumbrintArray[0]) &&
                        !string.IsNullOrWhiteSpace(gatewayThumbrintArray[1]))
                    {
                        GatewayThumbprint.Add(gatewayThumbrintArray[0].Trim(), gatewayThumbrintArray[1].Trim());
                    }
                }
            }

            string as2Timeout = ConfigurationManager.AppSettings["AS2TimeoutSec"];

            if (int.TryParse(as2Timeout, out _AS2Timeout))
            {
                if (AS2Timeout > 0)
                {
                    AS2Timeout *= 1000;
                }
                else
                {
                    AS2Timeout = 3600000;
                }
            }
            else
            {
                AS2Timeout = 3600000;
            }
        }

        #endregion

        #region Methods

        #region Process xevprm messages ready for submission

        internal void ProccessXevprmMessagesReadyForSubmission(object obj)
        {
            var xevprmMessagePkList = _xevprmMessageOperation.GetEntitiesPksReadyForSubmission();

            if (xevprmMessagePkList.Count > 0)
            {
                string description = string.Format("Submission of xevprm messages started. Total: {0}", xevprmMessagePkList.Count);
                Log.AS2Service.LogEvent(description);
            }

            int numberOfSuccessfullySubmitted = xevprmMessagePkList.Count(SubmitXevprmMessage);

            if (xevprmMessagePkList.Count > 0)
            {
                string description = "Submission of xevprm messages finished (Successfully submited: " + numberOfSuccessfullySubmitted + "/" + xevprmMessagePkList.Count + ")";
                Log.AS2Service.LogEvent(description);
            }
        }

        #endregion

        #region Process received messages

        internal void ProcessReceivedMessages(object obj)
        {
            var receivedMDNMessagePkList = _receivedMessageOperations.GetNotProcessedEntitiesPks(Ready.Model.ReceivedMessageType.MDN);

            if (receivedMDNMessagePkList.Count > 0)
            {
                string description = string.Format("Processing received MDN messages started. Total: {0}", receivedMDNMessagePkList.Count);
                Log.AS2Service.LogEvent(description);
            }

            int numberOfSuccessfullyProcessed = receivedMDNMessagePkList.Count(ProcessReceivedMDNMessage);

            if (receivedMDNMessagePkList.Count > 0)
            {
                string description = "Processing received MDN messages finished (Successfully processed: " + numberOfSuccessfullyProcessed + "/" + receivedMDNMessagePkList.Count + ")";
                Log.AS2Service.LogEvent(description);
            }

            var receivedACKMessagePkList = _receivedMessageOperations.GetNotProcessedEntitiesPks(Ready.Model.ReceivedMessageType.ACK);

            if (receivedACKMessagePkList.Count > 0)
            {
                string description = string.Format("Processing received ACK messages started. Total: {0}", receivedACKMessagePkList.Count);
                Log.AS2Service.LogEvent(description);
            }

            numberOfSuccessfullyProcessed = receivedACKMessagePkList.Count(ProcessReceivedACKMessage);

            if (receivedACKMessagePkList.Count > 0)
            {
                string description = "Processing received ACK messages finished (Successfully processed: " + numberOfSuccessfullyProcessed + "/" + receivedACKMessagePkList.Count + ")";
                Log.AS2Service.LogEvent(description);
            }
        }

        #endregion

        #region Process messages ready for MDN submission

        internal void ProcessXevprmMessagesReadyForMDNSubmission(object obj)
        {
            var xevprmMessagePkList = _xevprmMessageOperation.GetEntitiesPksReadyForMDNSubmission();

            if (xevprmMessagePkList.Count > 0)
            {
                string description = string.Format("Submission of MDN messages started. Total: {0}", xevprmMessagePkList.Count);
                Log.AS2Service.LogEvent(description);
            }

            int numberOfSuccessfullySubmitted = xevprmMessagePkList.Count(SubmitMDNMessage);

            if (xevprmMessagePkList.Count > 0)
            {
                string description = "Submission of MDN messages finished (Successfully submited: " + numberOfSuccessfullySubmitted + "/" + xevprmMessagePkList.Count + ")";
                Log.AS2Service.LogEvent(description);
            }
        }

        #endregion

        #endregion
    }
}
