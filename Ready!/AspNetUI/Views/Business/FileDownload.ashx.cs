using System;
using System.Web;
using System.IO;
using System.Configuration;
using AspNetUI.NanokinetikEDMS;
using AspNetUI.Support;
using AspNetUI.Views;
using AspNetUIFramework;
using Ready.Model;
using CacheManager = AspNetUI.Support.CacheManager;
using LocationManager = AspNetUI.Support.LocationManager;

namespace AspNetUI
{
    public class FileDownload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        IAttachment_PKOperations _attachmentOperations;

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            LocationManager.Instance.RetreiveApplicationLocationsFromDb();
            var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) return;

            string attachIDs = context.Request["attachID"];
            string file = context.Request["file"];

            if (!string.IsNullOrWhiteSpace(attachIDs))
            {
                var attachmentIdStr = string.Empty;
                var attachmentName = string.Empty;
                var attachmentData = new byte[1];

                var attachments = attachIDs.Split('|');
                if(attachments.Length > 1)
                {
                    if (!string.IsNullOrWhiteSpace(attachments[1]))
                    {
                        var EDMSDocument = attachments[1].Replace("{", "").Replace("}","").Split(';');
                        var username = SessionManager.Instance.CurrentUser.Username; 
                        var documentId = EDMSDocument[0];
                        var bindingRule = EDMSDocument[1];
                        var attachmentFormat = EDMSDocument[2];
                        var format = attachmentFormat != null && attachmentFormat.ToLower() == formatType.PDF.ToString().ToLower() ? formatType.PDF : formatType.ORIGINAL;

                        var edmsWsClient = new EDMS_WSClient();
                        try
                        {
                            var EDMSAttachment = edmsWsClient.getDocument(documentId, username, bindingRule, format);
                            attachmentName = EDMSAttachment.documentName + "." + attachmentFormat;
                            attachmentData = EDMSAttachment.content;
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }
                    else attachmentIdStr = attachments[0];
                } 
                else
                {
                    attachmentIdStr = attachments[0];
                }

                var attachmentId = ValidationHelper.IsValidInt(attachmentIdStr) ? (int?) Convert.ToInt32(attachmentIdStr) : null;
                if (attachmentId.HasValue)
                {
                    _attachmentOperations = new Attachment_PKDAL();
                    Attachment_PK attachment = _attachmentOperations.GetEntity(Int32.Parse(attachIDs));
                    attachmentName = attachment.attachmentname;
                    attachmentData = attachment.disk_file;
                }

                context.Response.Clear();
                context.Response.ContentType = "application/save";
                context.Response.AddHeader("Content-Disposition", @"attachment; filename=""" + HttpUtility.UrlDecode(HttpUtility.UrlEncode(attachmentName)) + @"""");

                if (attachmentData != null) context.Response.OutputStream.Write(attachmentData, 0, attachmentData.Length);
                context.Response.Flush();

            }
            else if (file == "AS2LogInbound")
            {
                if (File.Exists(ConfigurationManager.AppSettings["AS2HandlerInboundLogFullFilePath"]))
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/save";
                    context.Response.AddHeader("Content-Disposition", @"file; filename=""" + HttpUtility.UrlDecode(HttpUtility.UrlEncode("Log.txt")) + @"""");

                    byte[] fileData = File.ReadAllBytes(ConfigurationManager.AppSettings["AS2HandlerInboundLogFullFilePath"]);

                    context.Response.OutputStream.Write(fileData, 0, fileData.Length);
                    context.Response.Flush();
                }
            }
            else if (file == "EmailNotificationLog")
            {
                if (File.Exists(ConfigurationManager.AppSettings["LogFilePath"] + ConfigurationManager.AppSettings["EmailNotificationLogFileName"]))
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/save";
                    context.Response.AddHeader("Content-Disposition", @"file; filename=""" + HttpUtility.UrlDecode(HttpUtility.UrlEncode("EmailNotificationLog.txt")) + @"""");

                    byte[] fileData = File.ReadAllBytes(ConfigurationManager.AppSettings["LogFilePath"] + ConfigurationManager.AppSettings["EmailNotificationLogFileName"]);

                    context.Response.OutputStream.Write(fileData, 0, fileData.Length);
                    context.Response.Flush();
                }
            }
        }
    }
}