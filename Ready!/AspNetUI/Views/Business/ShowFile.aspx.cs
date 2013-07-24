using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Threading;
using AspNetUI.Support;
using Ionic.Zip;
using Ready.Model;
using System.Xml;
using System.Web.UI;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace AspNetUI.Views.Business
{
    public partial class ShowFile : Page
    {
        private const int BUFFER_SIZE = 32 * 1024;
        IAttachment_PKOperations _attachmentOperations;
        string sequence = String.Empty;
        string fileNamePart1 = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            FileStream inStream = null;

            string rootPath = Server.MapPath("~") + "\\" + ConfigurationManager.AppSettings["AttachTmpDir"] + "\\";
            string fileName = !String.IsNullOrWhiteSpace(Request.QueryString["fileName"]) ? Request.QueryString["fileName"] : "";
            string fullPath = String.Empty;
            string fullPathXsl = String.Empty;
            string contentType = !String.IsNullOrWhiteSpace(fileName) ? AttachmentHelper.GetMIMEType(fileName) : String.Empty;

            SequenceType sequenceType = SequenceType.Other;
            Attachment_PK attachment = null;

            Response.ContentType = contentType;

            Stream outStream = Response.OutputStream;

            if (Request.QueryString["isSUAttachment"] != null && Convert.ToBoolean(Request.QueryString["isSUAttachment"]))
            {
                string attachID = "";

                _attachmentOperations = new Attachment_PKDAL();

                attachID = Request.QueryString["attachID"];

                attachment = _attachmentOperations.GetEntity(Int32.Parse(attachID));
                sequence = attachment.attachmentname.Substring(0, 4);

                fullPath = rootPath + "//" + attachment.attachmentname;
                File.WriteAllBytes(fullPath, attachment.disk_file);

                using (ZipFile uploadedZipFile = ZipFile.Read(fullPath))
                {
                    foreach (ZipEntry item in uploadedZipFile)
                    {
                        string zippedFileName = item.FileName.Trim().ToLower();
                        if (zippedFileName.Contains("index.xml"))
                        {
                            item.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                            fullPath = rootPath + sequence + "\\index.xml";
                            Response.ContentType = "text/html";
                            sequenceType = SequenceType.eCTD;
                        }
                        else if (zippedFileName.Contains("ectd-2-0.xsl"))
                        {
                            item.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                            fullPathXsl = rootPath + sequence + "\\util\\style\\ectd-2-0.xsl";
                        }
                        else if (zippedFileName.Contains("/ctd-toc.pdf"))
                        {
                            item.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                            fullPath = rootPath + sequence + "//ctd-toc.pdf";
                            Response.ContentType = "application/pdf";
                            sequenceType = SequenceType.NeeS;
                        }
                        else if (zippedFileName.Contains("/ich-ectd-3-2.dtd")) 
                        {
                            item.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                        }
                        else
                        {
                            item.Extract(rootPath, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }

                    // xml
                    if (sequenceType == SequenceType.eCTD)
                    {
                        //Response.Clear();
                        //Response.ContentType = "text/xml";
                        ////doc.Load(inStream);
                        ////doc.Save(Response.Output);
                        //Response.Flush();
                        //Response.Close();

                        try
                        {
                            XPathDocument xmlDoc = new XPathDocument(fullPath);

                            fileNamePart1 = DateTime.Now.Millisecond.ToString();
                            using (XmlTextWriter xmlTxtWriter = new XmlTextWriter(rootPath + "//" + sequence + "//" + fileNamePart1 + "_index.html", null))
                            {
                                XslTransform xsl = new XslTransform();
                                xsl.Load(fullPathXsl);
                                xsl.Transform(xmlDoc, null, xmlTxtWriter);
                            }

                            XmlDocument html = new XmlDocument();
                            html.Load(rootPath + "//" + sequence + "//" + fileNamePart1 + "_index.html");

                            changeLinkPaths(html.ChildNodes[0]);
                            html.Save(Response.Output);


                            //Response.ContentType = "text/html";
                            //Response.WriteFile(rootPath + "//index.html");
                            Response.Flush();
                            Response.Close();
                        }
                        catch (Exception ex) {
                            // No Xls defined
                            inStream = new FileStream(fullPath, FileMode.Open);
                            XmlDocument doc = new XmlDocument();

                            Response.Clear();
                            Response.ContentType = "text/xml";
                            doc.Load(inStream);
                            doc.Save(Response.Output);
                            Response.Flush();
                            Response.Close();
                        }
                    }
                }

            }
            else
            {
                if (!String.IsNullOrWhiteSpace(fileName))
                {
                    fullPath = rootPath + fileName;
                }
            }

            if (sequenceType != SequenceType.eCTD)
            {
                inStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);

                byte[] buffer = new byte[BUFFER_SIZE];
                int len;
                while ((len = inStream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    outStream.Write(buffer, 0, len);
                }
                outStream.Flush();
                inStream.Close();
            }

            if (sequenceType != SequenceType.Other)
            {
                //if (sequenceType == SequenceType.NEES) File.Delete(rootPath + "//" + attachment.attachmentname.Substring(0, 4) + "//ctd-toc.pdf");
                if (sequenceType == SequenceType.eCTD)
                {
                    File.Delete(rootPath + "//" + sequence + "//" + fileNamePart1 + "_index.html");
                //    File.Delete(rootPath + "//index.html");
                }

                //Directory.Delete(rootPath + "//" + attachment.attachmentname.Substring(0, 4), true);
                File.Delete(rootPath + "//" + attachment.attachmentname);
            }
            else
            {
               File.Delete(fullPath);
            }
        }

        private void changeLinkPaths(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "a" && item.Attributes["href"].ToString() != String.Empty) {
                        XmlAttribute attribute = item.Attributes["href"];
                        //attribute.Value = Request.ApplicationPath + "/" + ConfigurationManager.AppSettings["AttachTmpDir"] + "/" + sequence + "/" + attribute.Value;
                        attribute.Value = GetSiteRoot() + "/" + ConfigurationManager.AppSettings["AttachTmpDir"] + "/" + sequence + "/" + attribute.Value;
                        item.Attributes.Append(attribute);
                    }
                    changeLinkPaths(item);
                }
            }
            else {
                return;
            }
        }

        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string sOut = protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + System.Web.HttpContext.Current.Request.ApplicationPath;

            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }

            return sOut;
        }
    }
}