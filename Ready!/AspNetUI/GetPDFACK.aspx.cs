﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Ready.Model;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AspNetUI.Support;

namespace AspNetUI
{
    public partial class GetPDFACK : System.Web.UI.Page
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperation;
        List<string> linesHeader = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(id);

            if (message == null) return;

            if (message.ack == null)
                return;
            ParseXMLToHumanReadable(message.ack);

            // create PDF document
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            Document myDocument = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, m);
            myDocument.Open();
            foreach (string line in linesHeader)
                myDocument.Add(new Paragraph(line));
            myDocument.Close();
            writer.Flush();

            // output to response output stream
            Response.Clear();
            Response.ContentType = "application/pdf";

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();

        }

        private void ParseXMLToHumanReadable(string msgXML)
        {
            XmlDocument doc = new XmlDocument();

            byte[] encodedString = UnicodeEncoding.Unicode.GetBytes(msgXML);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();
            doc.Load(ms);
            string content;

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "      ";
            settings.NewLineChars = "\r";
            settings.NewLineHandling = NewLineHandling.Replace;
            XmlWriter xwriter = XmlWriter.Create(sb, settings);
            doc.Save(xwriter);
            xwriter.Close();
            content = sb.ToString();

            string[] lines = content.Split('\r');

            if (doc.ChildNodes.Count > 1)
            {
                ConvertToString(doc.ChildNodes[1].ChildNodes[0], 0);
                ConvertToString(doc.ChildNodes[1].ChildNodes[1], 0);
            }
        }


        private void ConvertToString(XmlNode xmlNode, int level)
        {
            if (xmlNode == null) return;
            string line = "";

            for (int i = 0; i < level; i++)
                line += "   ";
            if (xmlNode.Name.Contains('#'))
            {
                linesHeader[linesHeader.Count - 1] += " " + xmlNode.Value;
                //line += xmlNode.Value;
            }
            else
            {
                line += CBLoader.MapXmlTag(xmlNode.Name) + ":" + xmlNode.Value;
                linesHeader.Add(line);
            }
            

            foreach (XmlNode item in xmlNode.ChildNodes)
            {
                ConvertToString(item, level + 1);
            }
        }

    }
}