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
    public partial class GetACK : System.Web.UI.Page
    {
        IXevprm_message_PKOperations _xevprm_message_PKOperation;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
                Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(id);

                XmlDocument doc = new XmlDocument();

                //byte[] encodedString = UTF8Encoding.UTF8.GetBytes(message.ACK);
                if (message.ack == null)
                    message.ack = "<evprm/>";
                byte[] encodedString = UnicodeEncoding.Unicode.GetBytes(message.ack);


                // Put the byte array into a stream and rewind it to the beginning
                MemoryStream ms = new MemoryStream(encodedString);
                ms.Flush();
                ms.Position = 0;

                // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
                XmlDocument xmlDoc = new XmlDocument();
                doc.Load(ms);


                //doc.LoadXml(message.XML);

                Response.Clear();
                Response.ContentType = "text/xml"; //must be 'text/xml'
                Response.ContentEncoding = System.Text.Encoding.UTF8; //we'd like UTF-8
                doc.Save(Response.Output); //save to the text-writer
                Response.End();
            }
        }
    }
}