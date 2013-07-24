using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ready.Model;
using System.IO;
using System.Text;
using AspNetUI.Support;
using ESCommon;
using ESCommon.Rtf;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing; 

namespace AspNetUI
{
    public partial class GetRTF : System.Web.UI.Page
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperation;
        List<string> linesHeader = new List<string>();

        RtfWriter writer;
        RtfDocument mainRtfDocument;
        //PdfDocument myDocument;
        PdfPTable tempAuthorisedTable = new PdfPTable(2);
        PdfPTable tempAttachments = new PdfPTable(2);

        
        
        

        RtfTable authorisedProductSubtable = new RtfTable(RtfTableAlign.Center);
        List<RtfTable> ATCcodeList = new List<RtfTable>();
        List<RtfTable> indicationsList = new List<RtfTable>();

        RtfTable pharmaceuticalProductsSubtable = new RtfTable(RtfTableAlign.Center);
        List<RtfTable> administrationRoutesList = new List<RtfTable>();
        List<RtfTable> activeIngreedientsList = new List<RtfTable>();
        List<RtfTable> excepiantsList = new List<RtfTable>();
        List<RtfTable> adjuvantsList = new List<RtfTable>();
        List<RtfDocument> medicalDevices = new List<RtfDocument>();
        RtfDocument ppDocument = new RtfDocument();

        RtfTable attachmentsSubtable = new RtfTable(RtfTableAlign.Center);
        RtfDocument attachmentsDocument = new RtfDocument();


        BaseFont bF;
        iTextSharp.text.Font font;
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(id);

            if (message == null) return;

            if (message.xml == null)
                return;

            // create RTF document
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            mainRtfDocument = new RtfDocument();

            Bitmap bmp = new System.Drawing.Bitmap(MapPath("~/Images/report_logo.png"));
            Bitmap bmp2 = new Bitmap(bmp.Width,bmp.Height);
            Graphics  g  = Graphics.FromImage(bmp2);
            g.DrawImage(bmp,new Point(0,0));
            RtfImage picture = new RtfImage(bmp2, RtfImageFormat.Wmf);
            picture.Width = TwipConverter.ToTwip(19, MetricUnit.Centimeter);
            picture.Height = (int)(picture.OriginalHeight / (picture.OriginalWidth / (float)picture.Width));
            
            RtfFormattedParagraph imgParagraph = NewFormattedParagraph(null, 0.0f, 0.5f);
            imgParagraph.AppendParagraph(picture);
            imgParagraph.Formatting.IndentLeft = TwipConverter.ToTwip(-1.2f, MetricUnit.Centimeter);
            mainRtfDocument.Contents.Add(imgParagraph);
        
            try
            {
                mainRtfDocument.FontTable.Add(new RtfFont("Arial"));
                mainRtfDocument.DefaultFont = 0;
            } catch (Exception ignorable) { }
            
     


            /// TESTING CODE
            //StreamReader reader = new StreamReader(new FileStream(@"c:\\xml.xml", FileMode.OpenOrCreate, FileAccess.Read));
            //string xmlString = reader.ReadToEnd();
            //extractACKElements(xmlString);

            string errorMessage = string.Empty;
            try
            {
                extractACKElements(message.xml);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message; 
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                // Building a document from parts
                mainRtfDocument.Contents.Add(NewFormattedParagraph("Authorized product", 0.2f, 0.1f));
                mainRtfDocument.Contents.Add(authorisedProductSubtable);

                mainRtfDocument.Contents.Add(NewFormattedParagraph("Product ATCs", 0.2f, 0.1f));
                foreach (var table in ATCcodeList)
                {
                    mainRtfDocument.Contents.Add(NewFormattedParagraph("Product ATC", 0.1f, 0.0f));
                    mainRtfDocument.Contents.Add(table);
                }

                mainRtfDocument.Contents.Add(NewFormattedParagraph("Product indications", 0.2f, 0.1f));
                foreach (var table in indicationsList)
                {
                    mainRtfDocument.Contents.Add(NewFormattedParagraph("Product indication", 0.1f, 0.0f));
                    mainRtfDocument.Contents.Add(table);
                }


                mainRtfDocument.Contents.Add(NewFormattedParagraph("Pharmaceutical products", 0.2f, 0.1f));
                foreach (var item in ppDocument.Contents) mainRtfDocument.Contents.Add(item);

                mainRtfDocument.Contents.Add(NewFormattedParagraph("Attachments", 0.2f, 0.1f));
                foreach (var item in attachmentsDocument.Contents) mainRtfDocument.Contents.Add(item);
            }
            else
            {
                mainRtfDocument.Contents.Add(NewFormattedParagraph(errorMessage, 0.2f,0.2f));
            }

            // output to response output stream
            Response.Clear();
            Response.ContentType = "application/rtf";
            RtfWriter writer = new RtfWriter();
            TextWriter wr = new StreamWriter(Response.OutputStream);
            writer.Write(wr, mainRtfDocument);
            wr.Flush();
            wr.Close();
            string enCodeFileName = Server.UrlEncode("message.rtf");
            Response.AddHeader("content-disposition", "attachment; filename=" + enCodeFileName);
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

            foreach (XmlNode item in doc.ChildNodes)
            {
                ConvertToString(item, 0);
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
                //This will add indent
                //ConvertToString(item, level + 1);

                //This is without indent
                ConvertToString(item, level);
            }
        }

        private void extractACKElements(string xml)
        {
            if (xml == null)
                xml = "<evprm/>";
            byte[] encodedString = UnicodeEncoding.Unicode.GetBytes(xml);

            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            XmlDocument doc = new XmlDocument();
            doc.Load(ms);

            XmlNode xmlEVPRM;
            if (doc.ChildNodes.Count >= 1 && doc.ChildNodes[0].Name != null && doc.ChildNodes[0].Name.Contains("evprm"))
                xmlEVPRM = doc.ChildNodes[0];
            else
                xmlEVPRM = doc.ChildNodes[1];

            mainRtfDocument.Contents.Add(NewFormattedParagraph("EVPR Message",0.0f,0.1f));
            RtfTable table = new RtfTable(RtfTableAlign.Center);

            RtfTableRow row = new RtfTableRow(2); 
            table.Rows.Add(row);
            
            table[0, 0].AppendText("Message number");

            try
            {
                XmlNode xmlEVPRMHeader = xmlEVPRM["ichicsrmessageheader"];
                XmlNode xmlEVPRMHeaderMessageNum = xmlEVPRMHeader["messagenumb"];

                 table[1,0].AppendText(xmlEVPRMHeaderMessageNum.FirstChild.Value);

            }
            catch
            {
                table[1,0].AppendText("N/A");
            }
            RtfFormattedParagraph pg = new RtfFormattedParagraph();
            mainRtfDocument.Contents.Add(table);


            foreach (XmlNode mainElement in xmlEVPRM.ChildNodes)
            {
                switch (mainElement.Name)
                {
                    case "authorisedproducts":
                        HandleAuthorizationProducts(mainElement);
                        break;
                    case "attachments":
                        HandleAttachments(mainElement);
                        break;

                    default:
                        break;
                }
            }
            
        }

        RtfFormattedParagraph NewFormattedParagraph(String text, float spaceBeforeInCm,float spaceAfterInCm)
        {
            RtfFormattedParagraph paragraph = String.IsNullOrEmpty(text)?new RtfFormattedParagraph():new RtfFormattedParagraph(text);
            paragraph.Formatting.SpaceBefore = TwipConverter.ToTwip(spaceBeforeInCm, MetricUnit.Centimeter);
            paragraph.Formatting.SpaceAfter = TwipConverter.ToTwip(spaceAfterInCm, MetricUnit.Centimeter);
            paragraph.Formatting.IndentLeft = TwipConverter.ToTwip(-0.25f, MetricUnit.Centimeter);
            return paragraph;
        }

        private void HandleAttachments(XmlNode mainElement)
        {
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                attachmentsDocument.Contents.Add(NewFormattedParagraph("Attachment", 0.1f, 0.0f));
                HandleAttachment(child);
            }
        }

        private void HandleAttachment(XmlNode mainElement)
        {
            RtfTable table = new RtfTable(RtfTableAlign.Center);
            
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText( CBLoader.MapXmlTag(child.Name));
             
                try
                {
                     row[1].AppendText(child.FirstChild.Value);
                }
                catch
                {
                     row[1].AppendText("N/A");
                }
                table.Rows.Add(row);
            }
            attachmentsDocument.Contents.Add(table);
        }

        private void HandleAuthorizationProducts(XmlNode mainElement)
        {
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                HandleAuthorizationProduct(child);
            }
        }
        private void HandleAuthorizationProduct(XmlNode mainElement)
        {
            RtfTable bufferTable;
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                switch (child.Name)
                {
                    case "localnumber":
                        HandleKeyValuePair("Local number", child, ref authorisedProductSubtable);
                        break;
                    case "mahcode":
                        HandleMAH(child);
                        break;
                    case "qppvcode":
                        HandleKeyValuePair("QPPV", child, ref authorisedProductSubtable);
                        break;
                    case "enquiryemail":
                        HandleKeyValuePair("PHV EMail", child, ref authorisedProductSubtable);
                        break;
                    case "enquiryphone":
                        HandleKeyValuePair("PHV Phone", child, ref authorisedProductSubtable);
                        break;
                    case "authorisation":
                        HandleAuthorization(child);
                        break;
                    case "presentationname":
                        HandlePresentationName(child);
                        break;
                    case "productatcs":
                        HandleProductAtcs(child);
                        break;
                    case "productindications":
                        HandleProductIndications(child);
                        break;
                    case "pharmaceuticalproducts":
                        HandlePharmaceuticalProducts(child);
                        break;
                    case "ev_code":
                        HandleKeyValuePair("EV Code", child, ref authorisedProductSubtable);
                        break;
                    case "mflcode":
                        HandleMFL(child);
                        break;
                    case "comments":
                        //HandleKeyValuePair("Comments", child, ref bufferTable);
                        break;
                    case "infodateformat":
                        HandleKeyValuePair("Info Date Format", child, ref authorisedProductSubtable);
                        break;
                    case "infodate":
                        HandleKeyValuePair("Info Date", child, ref authorisedProductSubtable);
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandleMFL(XmlNode mainElement)
        {
            //RtfTable table = new RtfTable(RtfTableAlign.Center);
            //RtfTableRow row = new RtfTableRow(1);
            //row[0].AppendText("MFL");
            //table.Rows.Add(row);
            //if (mainElement.ChildNodes.Count == 0)
            //{
            //    cell = new PdfPCell(new Phrase("MISSING", font));
            //    cell.Colspan = 1;
            //    //cell.HorizontalAlignment = 1;
            //    tempAuthorisedTable.AddCell(cell);
            //}
            //else
            //{

            //    foreach (XmlNode item in mainElement.ChildNodes)
            //    {
            //        cell = new PdfPCell(new Phrase(item.InnerText, font));
            //        cell.Colspan = 1;
            //        //cell.HorizontalAlignment = 1;
            //        tempAuthorisedTable.AddCell(cell);
            //    }

            //}
            //myDocument.Add(table);
        }

        private void HandleComments(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Comments", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }
            //myDocument.Add(table);
        }


        private void HandleProductIndications(XmlNode child)
        {
            foreach (XmlNode item in child.ChildNodes)
            {
                HandleProductIndication(item);
            }
        }

        private void HandleProductIndication(XmlNode mainElement)
        {
            RtfTable indTable = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                indTable.Rows.Add(row);
            }
            indicationsList.Add(indTable);
        }
        private void HandlePharmaceuticalProducts(XmlNode mainElement)
        {
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Pharmaceutical product", 0.1f, 0.0f));
                HandlePharmaceuticalProduct(item);
            }
        }

        private void HandlePharmaceuticalProduct(XmlNode mainElement)
        {
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                switch (item.Name)
                {
                    case "pharmformcode":
                        HandlePharmFormCode(item);
                        break;
                    case "adminroutes":
                        HandleAdminRoutes(item);
                        break;
                    case "activeingredients":
                        HandleActiveIngredients(item);
                        break;
                    case "excipients":
                        HandleExcipients(item);
                        break;
                    case "adjuvants":
                        HandleAdjuvants(item);
                        break;
                    case "medicaldevices":
                        HandleMedicalDevices(item);
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandleActiveIngredients(XmlNode mainElement)
        {
            ppDocument.Contents.Add(NewFormattedParagraph("Active ingredients", 0.2f, 0.1f));
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Active ingredient", 0.0f, 0.0f));
                HandleActiveIngredient(item);
            }
        }

        private void HandleActiveIngredient(XmlNode mainElement)
        {
            RtfTable table = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                table.Rows.Add(row);
            }
            ppDocument.Contents.Add(table);
        }

        private void HandleExcipients(XmlNode mainElement)
        {
            ppDocument.Contents.Add(NewFormattedParagraph("Excipients", 0.2f, 0.1f));
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Excipient", 0.0f, 0.0f));
                HandleExcipient(item);
            }
        }

        private void HandleExcipient(XmlNode mainElement)
        {
            RtfTable table = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                table.Rows.Add(row);
            }
            ppDocument.Contents.Add(table);
        }

        private void HandleAdjuvants(XmlNode mainElement)
        {
            ppDocument.Contents.Add(NewFormattedParagraph("Adjuvants", 0.2f, 0.1f));

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Adjuvant", 0.0f, 0.0f));
                HandleAdjuvant(item);
            }
        }

        private void HandleAdjuvant(XmlNode mainElement)
        {
            RtfTable table = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                table.Rows.Add(row);
            }
            ppDocument.Contents.Add(table);
        }

        private void HandleAdminRoutes(XmlNode mainElement)
        {
            ppDocument.Contents.Add(NewFormattedParagraph("Administrations routes", 0.2f, 0.1f));
            
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Administration route", 0.0f, 0.0f));
                HandleAdminRoute(item);
            }
        }

        private void HandleAdminRoute(XmlNode mainElement)
        {
            RtfTable table = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                table.Rows.Add(row);
            }
            ppDocument.Contents.Add(table);
        }

        private void HandleMedicalDevices(XmlNode mainElement)
        {
            ppDocument.Contents.Add(NewFormattedParagraph("Medical devices", 0.2f, 0.1f));

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                ppDocument.Contents.Add(NewFormattedParagraph("Medical device", 0.0f, 0.0f));
                HandleMedicalDevice(item);
            }
        }

        private void HandleMedicalDevice(XmlNode mainElement)
        {
            RtfTable table = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                table.Rows.Add(row);
            }
            ppDocument.Contents.Add(table);
        }

        private void HandleProductAtcs(XmlNode mainElement)
        {

            //RtfTableRow titleRow = CreateTitleRow();
            //titleRow[0].AppendParagraph("Product ACTs");
            //authorisedProductSubtable.Rows.Add(titleRow);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleProductAtc(item);
            }
        }

        private void HandleProductAtc(XmlNode mainElement)
        {
            //RtfTableRow titleRow = new RtfTableRow(1);
            
            //titleRow[0].AppendText("Product ATC");
            //authorisedProductSubtable.Rows.Add(titleRow);
            RtfTable atcTable = new RtfTable();
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                atcTable.Rows.Add(row);
                //authorisedProductSubtable.Rows.Add(row);
                
            }
            ATCcodeList.Add(atcTable);
        }

        private void HandleAuthorization(XmlNode mainElement)
        {
            RtfTable tempTable = new RtfTable(RtfTableAlign.Center);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow tempRow = new RtfTableRow(2);
                tempRow[0].AppendText(CBLoader.MapXmlTag(item.Name));
                tempRow[1].AppendText(item.InnerText);
                authorisedProductSubtable.Rows.Add(tempRow);
            }
        }

        private void HandleMAH(XmlNode mainElement)
        {
            RtfTableRow row = new RtfTableRow(2);
            row[0].AppendText("MAH");
            if (mainElement.ChildNodes.Count == 0)
            {
                row[1].AppendText("MISSING");
            }
            else
            {

                foreach (XmlNode item in mainElement.ChildNodes)
                {
                    row[1].AppendText(item.InnerText);
                }
            }
            authorisedProductSubtable.Rows.Add(row);
        }

        private void HandlePharmFormCode(XmlNode mainElement)
        {
            RtfTable tempTable = new RtfTable();
            RtfTableRow row = new RtfTableRow(2);
            row[0].AppendText("Pharmaceutical form");
           
            if (mainElement.ChildNodes.Count == 0)
            {
                row[1].AppendText("MISSING");
                tempTable.Rows.Add(row);
            }
            else
            {

                foreach (XmlNode item in mainElement.ChildNodes)
                {
                    row[1].AppendText(item.InnerText);
                    tempTable.Rows.Add(row);
                }

            }
            ppDocument.Contents.Add(tempTable);
        }

        private void HandlePresentationName(XmlNode mainElement)
        {
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                RtfTableRow row = new RtfTableRow(2);
                row[0].AppendText(CBLoader.MapXmlTag(item.Name));
                row[1].AppendText(item.InnerText);
                authorisedProductSubtable.Rows.Add(row);
            }
        }

        public void HandleKeyValuePair(string key, XmlNode node, ref RtfTable destTable)
        {
            RtfTableRow tableRow = new RtfTableRow(2);
            tableRow[0].AppendText(key);

            foreach (XmlNode item in node.ChildNodes)
            {
                tableRow[1].AppendText(item.InnerText);
                tableRow[1].Formatting.SpaceBefore = 10;
                destTable.Rows.Add(tableRow);
            }
        }

        private RtfTableRow CreateTitleRow()
        {
            RtfTableRow row = new RtfTableRow(1);
            row[0].Definition.Style.Borders.Left.Width = 0;
            row[0].Definition.Style.Borders.Right.Width = 0;
            row[0].Formatting.SpaceBefore = TwipConverter.ToTwip(0.3f, MetricUnit.Centimeter);
            row[0].Formatting.SpaceAfter = TwipConverter.ToTwip(0.1f, MetricUnit.Centimeter);
            return row;
        }

    }
}