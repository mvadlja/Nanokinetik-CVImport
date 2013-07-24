using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using System.Data;
using System.Text;

namespace AspNetUI
{
    public partial class GetReceivedMessageZIP : System.Web.UI.Page
    {
        IRecieved_message_PKOperations _recieved_message_PKOPerations;
        ISent_message_PKOperations _sent_message_PKOPerations;
        protected void Page_Load(object sender, EventArgs e)
        {
            String messageType = Request.QueryString["type"];
            if (messageType == "received")
            {
                _recieved_message_PKOPerations = new Recieved_message_PKDAL();
                int id = Convert.ToInt32(Request.QueryString["id"]);
                Recieved_message_PK message = _recieved_message_PKOPerations.GetEntity(id);
                if (message == null || message.msg_data == null) return;

                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Type", "text/plain");
                Response.Write(Encoding.UTF8.GetString(message.msg_data));
                Response.Flush();
                Response.End();

            }

            if (messageType == "sent")
            {
                _sent_message_PKOPerations = new Sent_message_PKDAL();
                int id = Convert.ToInt32(Request.QueryString["id"]);
                Sent_message_PK message = _sent_message_PKOPerations.GetEntity(id);
                if (message == null || message.msg_data == null) return;

                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("Content-Type", "text/plain");
                Response.Write(Encoding.UTF8.GetString(message.msg_data));
                Response.Flush();
                Response.End();

            }

        }
    }
}