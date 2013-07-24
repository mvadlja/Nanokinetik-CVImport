using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public class XevprmHelper
    {
        public static string ComputeHash(string data)
        {
            data = PrepareXMLForHash(data);
            string result = string.Empty;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(UnicodeEncoding.Unicode.GetBytes(data));
                result = Convert.ToBase64String(hash);
            }
            return result;
        }

        private static string PrepareXMLForHash(string data)
        {
            List<string> elementsToRemove = new List<string>() { "messagedate", "localnumber", "messagenumb" };

            return RemoveXMLElements(data, elementsToRemove); ;
        }

        public static string RemoveXMLElements(string data, List<string> tags)
        {
            foreach (var tag in tags)
            {
                string openTag = "<" + tag + ">";
                string closeTag = "</" + tag + ">";
                int openTagIndex = -1;
                int closeTagIndex = -1;

                int numMatches = Regex.Matches(data, openTag).Count;
                for (int counter = 0; counter < numMatches; counter++)
                {
                    openTagIndex = -1;
                    closeTagIndex = -1;

                    openTagIndex = data.LastIndexOf(openTag);
                    closeTagIndex = data.IndexOf(closeTag, openTagIndex);

                    if (openTagIndex > -1 && closeTagIndex > -1)
                    {
                        //include empty spaces before element
                        int startIndex = openTagIndex;
                        int emptySpacesLength = Regex.Match(data.Substring(0, startIndex), "[^\\r\\n>]*$").Length;
                        startIndex -= emptySpacesLength;

                        //include empty spaces after element
                        int endIndex = closeTagIndex + closeTag.Length;
                        emptySpacesLength = Regex.Match(data.Substring(endIndex), "^[^\\r\\n<]*[\\r\\n]*").Length;
                        endIndex += emptySpacesLength;

                        //remove element
                        data = data.Remove(startIndex, endIndex - startIndex);
                    }
                }

                //remove all selfclosing tags, empty elements
                data = Regex.Replace(data, "[^\\r\\n>]*<" + tag + "\\s*/>[^\\r\\n<]*[\\r\\n]*", "");
            }

            return data;
        }

        public static string AvailableGridActionForXevprmStatus(XevprmStatus xevprmStatus)
        {
            string availableAction = "-";

            switch (xevprmStatus)
            {
                case XevprmStatus.Created:
                    availableAction = "Validate";
                    break;
                case XevprmStatus.ValidationFailed:
                    availableAction = "Validate";
                    break;
                case XevprmStatus.ValidationSuccessful:
                    availableAction = "Submit";
                    break;
                case XevprmStatus.ReadyToSubmit:
                    availableAction = "Abort";
                    break;
                case XevprmStatus.SubmittingMessage:
                    break;
                case XevprmStatus.SubmissionFailed:
                    availableAction = "Resubmit";
                    break;
                case XevprmStatus.SubmissionAborted:
                    availableAction = "Resubmit";
                    break;
                case XevprmStatus.MDNPending:
                    availableAction = "Abort";
                    break;
                case XevprmStatus.MDNReceivedError:
                    availableAction = "Resubmit";
                    break;
                case XevprmStatus.MDNReceivedSuccessful:
                    availableAction = "Abort";
                    break;
                case XevprmStatus.ACKReceived:
                    break;
                case XevprmStatus.SubmittingMDN:
                    break;
                case XevprmStatus.ACKDeliveryFailed:
                    availableAction = "Resubmit";
                    break;
                case XevprmStatus.ACKDelivered:
                    break;
            }

            return availableAction;
        }
    }
}
