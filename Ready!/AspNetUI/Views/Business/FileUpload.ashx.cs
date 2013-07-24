using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.IO;
using System.Configuration;
using AspNetUI.NanokinetikEDMS;
using AspNetUI.Support;
using AspNetUI.Views;
using AspNetUIFramework;
using CommonComponents;
using Ready.Model;

namespace AspNetUI
{
    public class FileUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if ((context.Request.HttpMethod == "POST" || context.Request.HttpMethod == "PUT") &&
               context.Request.QueryString.Count > 0)
            {
                try
                {
                    if (context.Request.QueryString["action"] == "checkin" && context.Request.QueryString["sessionId"] != null && context.Request.QueryString["attachmentId"] != null)
                    {
                        int attachmentPk;

                        if (int.TryParse(context.Request.QueryString["attachmentId"], out attachmentPk))
                        {
                            IAttachment_PKOperations attachmentOperations = new Attachment_PKDAL();
                            var attachment = attachmentOperations.GetEntity(attachmentPk);
                            if (attachment != null)
                            {
                                attachment.attachment_PK = null;
                                attachment.document_FK = null;
                                attachment.modified_date = DateTime.Now;
                                attachment.session_id = System.Guid.NewGuid();
                                attachment.disk_file = context.Request.BinaryRead(context.Request.TotalBytes);
                                attachment.check_in_for_attach_FK = attachmentPk;
                                attachment.check_in_session_id = Guid.Parse(context.Request.QueryString["sessionId"]);

                                attachmentOperations.Save(attachment);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}