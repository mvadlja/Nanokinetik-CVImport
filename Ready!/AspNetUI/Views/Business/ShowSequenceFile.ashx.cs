using System;
using System.Configuration;
using System.IO;
using System.Web;
using Ionic.Zip;
using Ready.Model;

namespace AspNetUI.Views.Business
{
    public class ShowSequenceFile : IHttpHandler
    {
        IAttachment_PKOperations _attachmentOperations;
        private string _sequence;
        private string _timestamp;
        private string _attachmentPk;
        private string _redirectFilePath;
        private string _sequenceFilePath;
        private string _physicalAttachTmpDirPath;
        private string _attachTmpDir;

        public void ProcessRequest(HttpContext context)
        {
            _attachmentOperations = new Attachment_PKDAL();

            _attachTmpDir = ConfigurationManager.AppSettings["AttachTmpDir"] ?? "AttachTmpDir";
            _physicalAttachTmpDirPath = context.Server.MapPath("~") + "\\" + _attachTmpDir;
            _attachmentPk = context.Request.QueryString["attachmentPk"];
            _timestamp = DateTime.Now.ToString("fff");

            Attachment_PK attachment = _attachmentOperations.GetEntity(Int32.Parse(_attachmentPk));
            _sequence = attachment.attachmentname.Substring(0, 4);

            _physicalAttachTmpDirPath = string.Format("{0}\\{1}\\{2}_{3}", context.Server.MapPath("~"), _attachTmpDir, _attachmentPk, _timestamp);
            _sequenceFilePath = string.Format("{0}\\{1}", _physicalAttachTmpDirPath, attachment.attachmentname);

            try
            {
                if (!Directory.Exists(_physicalAttachTmpDirPath)) Directory.CreateDirectory(_physicalAttachTmpDirPath);

                File.WriteAllBytes(_sequenceFilePath, attachment.disk_file);

                using (ZipFile uploadedZipFile = ZipFile.Read(_sequenceFilePath))
                {
                    foreach (ZipEntry zipEntry in uploadedZipFile)
                    {
                        string zippedFileName = zipEntry.FileName.Trim().ToLower();
                        zipEntry.Extract(_physicalAttachTmpDirPath, ExtractExistingFileAction.OverwriteSilently);

                        if (zippedFileName.Contains("index.xml")) _redirectFilePath = "index.xml";
                        else if (zippedFileName.Contains("ctd-toc.pdf")) _redirectFilePath = "ctd-toc.pdf";
                    }
                }

                try
                {
                    var filepathToDelete = string.Format("{0}/{1}.zip", _physicalAttachTmpDirPath, _sequence);
                    if (File.Exists(filepathToDelete)) File.Delete(filepathToDelete);
                }
                catch
                {
                    // Deletion failed. Will be deleted manually
                }

                var appVirtualPath = ConfigurationManager.AppSettings["AppVirtualPath"];
                var redirectUrl = string.Format("{0}/{1}/{2}_{3}/{4}/{5}", appVirtualPath, _attachTmpDir, _attachmentPk, _timestamp, _sequence, _redirectFilePath);
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
            catch
            {
                // Something went wrong with file/directory handling 
                // Redirect to previous location
            }

            if (context.Request.UrlReferrer != null) context.Response.Redirect(context.Request.UrlReferrer.AbsoluteUri);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}