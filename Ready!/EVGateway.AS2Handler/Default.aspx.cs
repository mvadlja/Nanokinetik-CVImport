using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace EVGateway.AS2Handler
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Log("Default Page accessed...");
            string filename = Server.MapPath("~/Data/") + DateTime.Now.ToString("dd.MM.yyyy hh:mm");
            filename += "-" + System.IO.Path.GetRandomFileName() + ".dat";

        }

        void Log(string s)
        {
            string path = Server.MapPath("~/Data/log.txt");
            using (StreamWriter w = File.AppendText(path))
            {
                w.Write(DateTime.Now.ToString("dd.MM.yyyy hh:mm"));
                w.Write(" - ");
                w.WriteLine(s);
                // Update the underlying file.
                w.Flush();
            }
        }
    }
}