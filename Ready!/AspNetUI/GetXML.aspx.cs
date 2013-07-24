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

namespace AspNetUI
{
    public partial class GetXML : System.Web.UI.Page
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperation;

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(id);
            if (message == null) return;
            XmlDocument doc = new XmlDocument();

            if (message.xml == null)
                message.xml = "<evprm/>";
            byte[] encodedString = UnicodeEncoding.Unicode.GetBytes(message.xml);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
            XmlDocument xmlDoc = new XmlDocument();

            string errorMessage = string.Empty;
            try
            {
                doc.Load(ms);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


            //doc.LoadXml(message.XML);

            if (string.IsNullOrEmpty(errorMessage))
            {
                Response.Clear();
                Response.ContentType = "text/xml"; //must be 'text/xml'
                Response.ContentEncoding = System.Text.Encoding.UTF8; //we'd like UTF-8
                doc.Save(Response.Output); //save to the text-writer
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.ContentType = "text/html";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Output.WriteLine("<div style='color:red;margin:0 auto;padding-top:50px;text-align:center;width:100%;'>" + errorMessage + "</div>");
                Response.End();
            }
        }
    }
}