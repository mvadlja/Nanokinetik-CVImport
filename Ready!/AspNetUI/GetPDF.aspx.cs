using System;
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
    public partial class GetPDF : System.Web.UI.Page
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperation;
        List<string> linesHeader = new List<string>();

        PdfWriter writer;
        Document myDocument;

        PdfPTable tempAuthorisedTable = new PdfPTable(2);
        PdfPTable tempAttachments = new PdfPTable(2);

        BaseFont bF;
        Font font;
        protected void Page_Load(object sender, EventArgs e)
        {
            bF = BaseFont.CreateFont(Server.MapPath("~/Fonts/arial.ttf"), BaseFont.IDENTITY_H, true);
            font = new Font(bF, 12f, Font.NORMAL);
            int id = Convert.ToInt32(Request.QueryString["id"]);
            _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(id);

            if (message == null) return;

            if (message.xml == null)
                return;
            //ParseXMLToHumanReadable(message.XML);

            // create PDF document
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            myDocument = new Document(PageSize.A4);

            writer = PdfWriter.GetInstance(myDocument, m);


            myDocument.Open();

            iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(MapPath("~/Images/report_logo.png"));// "report_logo.png");
            logoImage.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            logoImage.ScaleToFit(500, 480);
            myDocument.Add(logoImage);
            logoImage = null;

            //PdfPTable table = new PdfPTable(2);
            //PdfPCell cell = new PdfPCell();
            ////cell.Colspan = 2;
            ////cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            ////table.AddCell(cell);


            //foreach (string line in linesHeader)
            //{
            //    foreach (var item in line.Split(':'))
            //    {
            //        cell = new PdfPCell(new Phrase(item));
            //        table.AddCell(cell);
            //    }

            //}
            string errorMessage = string.Empty;
            try
            {
                extractACKElements(message.xml);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Font f = new Font();
                f.Color = new BaseColor(System.Drawing.Color.Red);
                myDocument.Add(new Phrase(""));
                myDocument.Add(new Phrase(errorMessage, f));
            }

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

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("EVPR Message", font));
            cell.Colspan = 2;
            cell.Border = 0;

            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Message number", font));
            table.AddCell(cell);

            try
            {
                XmlNode xmlEVPRMHeader = xmlEVPRM["ichicsrmessageheader"];
                XmlNode xmlEVPRMHeaderMessageNum = xmlEVPRMHeader["messagenumb"];

                cell = new PdfPCell(new Phrase(xmlEVPRMHeaderMessageNum.FirstChild.Value, font));
                table.AddCell(cell);
            }
            catch
            {
                cell = new PdfPCell(new Phrase("N/A", font));
                table.AddCell(cell);
            }
            myDocument.Add(table);

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
            myDocument.Add(tempAttachments);
        }

        private void HandleAttachments(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Attachments", font));
            cell.Colspan = 2;
            cell.Border = 0;
            tempAttachments.AddCell(cell);
            //myDocument.Add(table);
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                HandleAttachment(child);
            }
        }

        private void HandleAttachment(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Attachment", font));
            cell.Colspan = 2;
            cell.Border = 0;
            tempAttachments.AddCell(cell);
            //myDocument.Add(table);
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(new Phrase(CBLoader.MapXmlTag(child.Name), font)));
                tempAttachments.AddCell(cell);

                try
                {
                    cell = new PdfPCell(new Phrase(child.FirstChild.Value, font));
                    tempAttachments.AddCell(cell);
                }
                catch
                {
                    cell = new PdfPCell(new Phrase("N/A", font));
                    tempAttachments.AddCell(cell);
                }



            }
            //myDocument.Add(table);
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
            foreach (XmlNode child in mainElement.ChildNodes)
            {
                switch (child.Name)
                {
                    case "localnumber":
                        HandleLocalNumber(child);
                        break;
                    case "mahcode":
                        HandleMAH(child);
                        break;
                    case "qppvcode":
                        HandleQPPV(child);
                        break;
                    case "enquiryemail":
                        HandleEmail(child);
                        break;
                    case "enquiryphone":
                        HandlePhone(child);
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
                        HandleEVCode(child);
                        break;
                    case "mflcode":
                        HandleMFL(child);
                        break;
                    case "comments":
                        HandleComments(child);
                        break;
                    case "infodateformat":
                        HandleInfoDateFormat(child);
                        break;
                    case "infodate":
                        HandleInfoDate(child);
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandleMFL(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("MFL"));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            if (mainElement.ChildNodes.Count == 0)
            {
                cell = new PdfPCell(new Phrase("MISSING", font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            else
            {

                foreach (XmlNode item in mainElement.ChildNodes)
                {
                    cell = new PdfPCell(new Phrase(item.InnerText, font));
                    cell.Colspan = 1;
                    //cell.HorizontalAlignment = 1;
                    tempAuthorisedTable.AddCell(cell);
                }

            }
            myDocument.Add(table);
        }

        private void HandleEVCode(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("EV Code", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleInfoDateFormat(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Info Date Format", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
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
            myDocument.Add(table);
        }

        private void HandleInfoDate(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Info Date", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandlePhone(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("PHV Phone", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleEmail(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("PHV EMail", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in child.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleProductIndications(XmlNode child)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Product indications", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            myDocument.Add(table);
            foreach (XmlNode item in child.ChildNodes)
            {
                HandleProductIndication(item);
            }
        }

        private void HandleProductIndication(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Product indication", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }

            myDocument.Add(table);
        }
        private void HandlePharmaceuticalProducts(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Pharmaceutical Products", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            myDocument.Add(table);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandlePharmaceuticalProduct(item);
            }
        }

        private void HandlePharmaceuticalProduct(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Pharmaceutical Product", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

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
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Active Ingredients", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleActiveIngredient(item);
            }
        }

        private void HandleActiveIngredient(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Active Ingredient", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleExcipients(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Excipients", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleExcipient(item);
            }
        }

        private void HandleExcipient(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Excipient", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText));
                table.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleAdjuvants(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Adjuvants", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleAdjuvant(item);
            }
        }

        private void HandleAdjuvant(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Adjuvant", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleAdminRoutes(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Administration routes", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleAdminRoute(item);
            }
        }

        private void HandleAdminRoute(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Administration route", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);



            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleMedicalDevices(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Medical devices", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            myDocument.Add(table);

            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleMedicalDevice(item);
            }
        }

        private void HandleMedicalDevice(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Medical device", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);



            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandleProductAtcs(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Product ATCs", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            myDocument.Add(table);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                HandleProductAtc(item);
            }
        }

        private void HandleProductAtc(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Product ATC", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText));
                table.AddCell(cell);
            }

            myDocument.Add(table);
        }

        private void HandlePresentationName(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Presentation Name", font));
            //cell.Colspan = 2;
            //cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            //table.AddCell(cell);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }

            myDocument.Add(table);
        }

        private void HandleAuthorization(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Authorized product", font));
            cell.Colspan = 2;
            cell.Border = 0;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            myDocument.Add(table);
            table = new PdfPTable(2);
            myDocument.Add(tempAuthorisedTable);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(CBLoader.MapXmlTag(item.Name), font));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                table.AddCell(cell);
            }

            myDocument.Add(table);
        }

        private void HandleMAH(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("MAH", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            if (mainElement.ChildNodes.Count == 0)
            {
                cell = new PdfPCell(new Phrase("MISSING", font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            else
            {

                foreach (XmlNode item in mainElement.ChildNodes)
                {
                    cell = new PdfPCell(new Phrase(item.InnerText, font));
                    cell.Colspan = 1;
                    //cell.HorizontalAlignment = 1;
                    tempAuthorisedTable.AddCell(cell);
                }

            }
            myDocument.Add(table);
        }
        private void HandleLocalNumber(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Local number", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }

            myDocument.Add(table);
        }
        private void HandleQPPV(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("QPPV", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            tempAuthorisedTable.AddCell(cell);
            foreach (XmlNode item in mainElement.ChildNodes)
            {
                cell = new PdfPCell(new Phrase(item.InnerText, font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                tempAuthorisedTable.AddCell(cell);
            }
            myDocument.Add(table);
        }

        private void HandlePharmFormCode(XmlNode mainElement)
        {
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Pharmaceutical form", font));
            cell.Colspan = 1;
            //cell.HorizontalAlignment = 1;
            table.AddCell(cell);
            if (mainElement.ChildNodes.Count == 0)
            {
                cell = new PdfPCell(new Phrase("MISSING", font));
                cell.Colspan = 1;
                //cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }
            else
            {

                foreach (XmlNode item in mainElement.ChildNodes)
                {
                    cell = new PdfPCell(new Phrase(item.InnerText, font));
                    cell.Colspan = 1;
                    //cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

            }
            myDocument.Add(table);
        }
    }
}