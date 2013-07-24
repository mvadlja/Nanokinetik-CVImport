using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AspNetUI.NanokinetikEDMS;

namespace AspNetUI.Support
{
    public class EDMS
    {
        public static DataSet EdmsGetFolderContent(string folderId, string username, Dictionary<string, string> filters, out int itemsCount)
        {
            itemsCount = 0;

            var ds = new DataSet();
            var dt = ds.Tables.Add("FolderContent");

            var documentId = dt.Columns.Add("DocumentId", typeof(string));
            dt.Columns.Add("DocumentName", typeof(string));
            dt.Columns.Add("VersionLabel", typeof(string));
            dt.Columns.Add("VersionNumber", typeof(string));
            dt.Columns.Add("AttachmentType", typeof(string));
            dt.Columns.Add("BindingRule", typeof(string));
            dt.Columns.Add("ModifyDate", typeof(DateTime));
            dt.Columns.Add("ContentSize", typeof(string));
            dt.Columns.Add("ModifiyDateSpecified", typeof(bool));
            dt.Columns.Add("DosExtension", typeof(string));
            dt.Columns.Add("DocumentFormat", typeof(string));
            dt.Columns.Add("Download", typeof(string));
            dt.PrimaryKey = new [] { documentId };

            var edmsWsClient = new EDMS_WSClient();

            EDMSDocument[] folderContent;

            if (string.IsNullOrWhiteSpace(folderId) || string.IsNullOrWhiteSpace(username)) return ds;

            try
            {
                folderContent = edmsWsClient.getFolderContent(folderId, username);
            }
            catch (Exception ex)
            {
                return ds;
            }

            if (folderContent == null || !folderContent.Any()) return ds;

            foreach (var content in folderContent)
            {
                var dr = ds.Tables["FolderContent"].NewRow();

                dr["DocumentId"] = content.documentID;
                dr["DocumentName"] = content.documentName;
                dr["VersionLabel"] = content.versionLabel;
                dr["VersionNumber"] = content.versionNumber;
                dr["AttachmentType"] = content.mimeType;
                dr["BindingRule"] = content.currentVersion;
                dr["ModifyDate"] = content.modifyDate.ToShortDateString();
                dr["ContentSize"] = content.contentSize;
                dr["ModifiyDateSpecified"] = content.modifyDateSpecified;
                dr["DosExtension"] = content.dosExtension;
                dr["DocumentFormat"] = content.format;
                dr["Download"] = string.Empty;

                ds.Tables["FolderContent"].Rows.Add(dr);
            }

            itemsCount = ds.Tables["FolderContent"].Rows.Count;

            return ds;
        }

    }
}