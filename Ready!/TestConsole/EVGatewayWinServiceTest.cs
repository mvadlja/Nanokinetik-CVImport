using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EVGateway.WinService.Workflow;

namespace TestConsole
{
    public static class EVGatewayWinServiceTest
    {
        static readonly Workflow WorkflowBillev = new Workflow()
        {
            MessageReceiverId = "EVTEST",
            AS2GatewayID = "EVMPDHVAL",
            SenderThumbprint = new Dictionary<string, string>() { { "BPET", "F0384F0CD0B91BE8B88026EA348E998235184934" } },
            GatewayThumbprint = new Dictionary<string, string>() { { "EVMPDHVAL", "2BFC2DE0B850CCC919AF128ADF2DB526A5850198" } },
            AS2Timeout = 3600,
            AS2ExchangePointURI = "http://vgateway.ema.europa.eu:8080/exchange/EVMPDHVAL",
            EMAMDNReceiptURL = "http://vgateway.ema.europa.eu:8080/exchange/EVMPDHVAL",
            MDNReceiptURL = "http://office.possimusit.com:4080/AS2Listener.ashx",
            XevprmMessageSubmissionDelay = 0
        };

        public static void SubmitXevprmMessages(int xevprmMessagePk)
        {
            WorkflowBillev.SubmitXevprmMessage(xevprmMessagePk);
        }

        public static void ProcessReceivedMessages(int receivedMessagePk)
        {
            WorkflowBillev.ProcessReceivedMDNMessage(receivedMessagePk);
        }
    }
}
