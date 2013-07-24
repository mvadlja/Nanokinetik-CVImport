using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Lesnikowski.Client.IMAP;
using Lesnikowski.Mail;
using Lesnikowski.Mail.Headers;

namespace AspNetUI.Support
{
    public class GMailHelper
    {
        //private const string CSHelpdeskMail = "info.hr@kolektiva.net";
        //private const string CSHelpdeskMailPassword = "!nf0.Ko1a";
        private const string CSHelpdeskMail = "cs.helpdesk.test@gmail.com";
        private const string CSHelpdeskMailPassword = "helpdesk123";

        private const string GMailSmtp = "smtp.gmail.com";
        private const int GMailSmtpPort = 587;

        private const string GMailIMAPServer = "imap.gmail.com";
        private const int GMailIMAPPortSSL = 993;

        private static Imap _imap = null;

        private static Imap IMAP
        {
            get { return _imap ?? (_imap = CreateNewIMAPClient("[Gmail]/All Mail")); }
        }

        private static Imap CreateNewIMAPClient(string folder)
        {
            Imap imap = new Imap();

            imap.ConnectSSL(GMailIMAPServer, GMailIMAPPortSSL);
            imap.Login(CSHelpdeskMail, CSHelpdeskMailPassword);
            imap.Select(!String.IsNullOrEmpty(folder) ? folder : "[Gmail]/All Mail");

            return imap;
        }

        public static List<IMAP_EMail> GetNewMessages(int currentPage, int pageSize, ref int currentIndex)
        {
            List<IMAP_EMail> newMessages = new List<IMAP_EMail>();

            try
            {
                IMAP.Select("INBOX");

                List<long> unreadMsgUIDs = IMAP.SearchFlag(Flag.Unseen);

                int numberOfMsgsToFetch = unreadMsgUIDs.Count >= (((currentPage - 1) * pageSize) + pageSize)
                                              ? pageSize
                                              : unreadMsgUIDs.Count - ((currentPage - 1) * pageSize);

                while (newMessages.Count < numberOfMsgsToFetch && currentIndex < unreadMsgUIDs.Count)
                {

                    IMAP_EMail message = FetchMessageInfo(unreadMsgUIDs[unreadMsgUIDs.Count - currentIndex - 1]);

                    if (message.ThreadID > 0 && !(newMessages.Any(m => m.ThreadID == message.ThreadID)))
                    {
                        message.ThreadMsgsCount = ThreadMsgsCount(message.ThreadID);

                        if (message.ThreadMsgsCount > 1)
                        {
                            message.Sender += " (" + message.ThreadMsgsCount + ")";
                        }

                        newMessages.Add(message);
                    }

                    currentIndex++;
                }
            }
            finally
            {
                IMAP.Select("[Gmail]/All Mail");
            }

            return newMessages;
        }

        public static List<IMAP_EMail> GetReadMessages(int currentPage, int pageSize, ref int currentIndex)
        {
            List<IMAP_EMail> readMessages = new List<IMAP_EMail>();

            List<long> readMsgsUIDs = IMAP.SearchFlag(Flag.Seen);

            if (readMsgsUIDs == null)
                return readMessages;

            int numberOfMsgsToFetch = readMsgsUIDs.Count >= (((currentPage - 1) * pageSize) + pageSize)
                                          ? pageSize
                                          : readMsgsUIDs.Count - ((currentPage - 1) * pageSize);

            while (readMessages.Count < numberOfMsgsToFetch && currentIndex < readMsgsUIDs.Count)
            {
                IMAP_EMail message = FetchMessageInfo(readMsgsUIDs[readMsgsUIDs.Count - currentIndex - 1]);

                if (message.ThreadID > 0 && !(readMessages.Any(m => m.ThreadID == message.ThreadID)))
                {
                    message.ThreadMsgsCount = ThreadMsgsCount(message.ThreadID);

                    if (message.ThreadMsgsCount > 1)
                    {
                        message.Sender += " (" + message.ThreadMsgsCount + ")";
                    }

                    readMessages.Add(message);
                }

                currentIndex++;
            }

            return readMessages;
        }

        public static List<IMAP_EMail> GetThreadMessages(ulong threadID, int currentPage, int pageSize, out int threadMsgsCount)
        {
            List<IMAP_EMail> threadMessages = new List<IMAP_EMail>();

            List<long> threadUIDs = IMAP.Search().Where(Expression.GmailThreadId(threadID.ToString()));

            threadMsgsCount = threadUIDs != null ? threadUIDs.Count : 0;

            if (threadUIDs == null)
                return threadMessages;
            
            for (int i = (currentPage - 1) * pageSize; i < (((currentPage - 1) * pageSize) + pageSize) && i < threadUIDs.Count; i++)
            {
                threadMessages.Add(FetchMessage(threadUIDs[threadUIDs.Count - i - 1]));
            }
            
            return threadMessages;
        }

        public static List<IMAP_EMail> GetMessagesByThreadIDs(HashSet<ulong> threadIDs)
        {
            List<IMAP_EMail> messages = new List<IMAP_EMail>();

            foreach (ulong threadID in threadIDs)
            {
                List<long> threadMsgsUIDs = IMAP.Search().Where(Expression.GmailThreadId(threadID.ToString()));
                if (threadMsgsUIDs != null && threadMsgsUIDs.Count > 0)
                {
                    IMAP_EMail email = FetchMessage(threadMsgsUIDs[threadMsgsUIDs.Count - 1]);
                    if (email != null)
                    {
                        email.Sender += " (" + threadMsgsUIDs.Count + ")";
                        messages.Add(email);
                    }
                }
            }

            return messages;
        }

        public static IMAP_EMail GetLastThreadMessage(ulong threadID)
        {
            List<long> uids = IMAP.Search().Where(Expression.GmailThreadId(threadID.ToString()));

            if (uids == null || uids.Count == 0)
                return null;

            IMAP_EMail message = FetchMessage(uids[uids.Count - 1]);

            return message;
        }

        private static IMAP_EMail FetchMessageInfo(long uid)
        {
            IMAP_EMail message = new IMAP_EMail();

            Envelope envelope = IMAP.GetEnvelopeByUID(uid);
            if (envelope != null)
            {
                message.Subject = envelope.Subject;
                message.Sender = JoinAddresses(envelope.Sender);
                message.Date = envelope.Date;
                message.InReplyTo = envelope.InReplyTo;
                message.ThreadID = !String.IsNullOrEmpty(envelope.GmailThreadId)
                                       ? Convert.ToUInt64(envelope.GmailThreadId)
                                       : 0;
                message.MessageID = !String.IsNullOrEmpty(envelope.GmailMessageId)
                                        ? Convert.ToUInt64(envelope.GmailMessageId)
                                        : 0;
            }

            return message;
        }

        private static IMAP_EMail FetchMessage(long uid)
        {
            IMAP_EMail message = new IMAP_EMail();

            IMail email = new MailBuilder().CreateFromEml(IMAP.GetMessageByUID(uid));

            if (email != null)
            {
                message.Subject = email.Subject;
                message.Sender = JoinAddresses(email.From);
                message.Date = email.Date;
                message.InReplyTo = email.InReplyTo;
                message.Body = email.TextDataString;

                Envelope envelope = IMAP.GetEnvelopeByUID(uid);
                if (envelope != null)
                {
                    message.ThreadID = !String.IsNullOrEmpty(envelope.GmailThreadId)
                                           ? Convert.ToUInt64(envelope.GmailThreadId)
                                           : 0;
                    message.MessageID = !String.IsNullOrEmpty(envelope.GmailMessageId)
                                            ? Convert.ToUInt64(envelope.GmailMessageId)
                                            : 0;
                }
            }

            return message;
        }

        public static int ThreadMsgsCount(ulong threadID)
        {
            List<long> uids = IMAP.Search().Where(Expression.GmailThreadId(threadID.ToString()));

            return uids != null ? uids.Count : 0;
        }

        public static void MarkMessageAsUnread(ulong messageID)
        {
            List<long> uids = IMAP.Search().Where(Expression.GmailMessageId(messageID.ToString()));

            if (uids.Count > 0)
            {
                IMAP.MarkMessageUnseenByUID(uids[0]);
            }
        }

        public static void DisposeIMAPClient()
        {
            if (_imap == null) return;

            _imap.Close();
            _imap.Dispose();
            _imap = null;
        }

        public static void SendMail(string mailTo, string subject, string body, string originalMessageID)
        {
            using (SmtpClient smtpClient = new SmtpClient(GMailSmtp, GMailSmtpPort))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(CSHelpdeskMail, CSHelpdeskMailPassword);

                using (MailMessage message = new MailMessage(CSHelpdeskMail, mailTo))
                {
                    var plainView = AlternateView.CreateAlternateViewFromString("<html><body>" + body + "</body></html>", new System.Net.Mime.ContentType("text/html; charset=UTF-8"));

                    message.AlternateViews.Add(plainView);

                    message.Body = "<html><body>" + body + "</body></html>";
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;

                    if (!String.IsNullOrEmpty(originalMessageID))
                    {
                        message.Subject = "Re: " + subject;
                        message.Headers.Add("In-Reply-To", originalMessageID);
                    }
                    else
                    {
                        message.Subject = subject;
                    }

                    smtpClient.Send(message);
                }
            }
        }

        private static string JoinAddresses(IMailBoxList addresses)
        {
            return string.Join(",",
                addresses.ConvertAll(m => string.Format("{0} <{1}>", m.Name, m.Address))
                .ToArray());
        }
    }

    public class IMAP_EMail
    {
        public string Subject { get; set; }
        public string Sender { get; set; }
        public DateTime? Date { get; set; }
        public string Body { get; set; }
        public long UID { get; set; }
        public ulong MessageID { get; set; }
        public ulong ThreadID { get; set; }
        public int ThreadMsgsCount { get; set; }
        public string InReplyTo { get; set; }

        public IMAP_EMail()
        {
        }

        public IMAP_EMail(string subject, string sender, DateTime date, string body, long uid, ulong messageID, ulong threadID, int threadMsgsCount, string inReplyTo)
        {
            Subject = subject;
            Sender = sender;
            Date = date;
            Body = body;
            UID = uid;
            MessageID = messageID;
            ThreadID = threadID;
            ThreadMsgsCount = threadMsgsCount;
            InReplyTo = inReplyTo;
        }
    }
}