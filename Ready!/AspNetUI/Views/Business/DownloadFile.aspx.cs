using System;
using System.Web.UI;
using System.Configuration;
using AspNetUI.Support;
using Ready.Model;
using System.IO;

namespace AspNetUI.Views
{
    public partial class DownloadFile : Page, System.Web.SessionState.IRequiresSessionState
    {
        IAttachment_PKOperations _attachmentOperations;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _attachmentOperations = new Attachment_PKDAL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LocationManager.Instance.RetreiveApplicationLocationsFromDb();
            var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) return;

            string fullPath = "";
            string attachID = Request.QueryString["attachID"];

            if (!string.IsNullOrWhiteSpace(attachID))
            {
                Attachment_PK attachment = _attachmentOperations.GetEntity(attachID);

                if (attachment != null)
                {
                    fullPath = Server.MapPath("~") + "\\" + ConfigurationManager.AppSettings["AttachTmpDir"] + "\\" + attachment.attachmentname;
                    File.WriteAllBytes(fullPath, attachment.disk_file);
                }
            }

            if (fullPath != "")
            {
                string imeFile = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);

                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + imeFile + "");
                Response.ContentType = "application/octet-stream";
                Response.TransmitFile(fullPath);
                Response.Flush();
                Response.Close();
                File.Delete(fullPath);                              
            }
        }
    }
}