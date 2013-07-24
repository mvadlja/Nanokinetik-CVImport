using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace EVGateway.AS2Handler
{
    public class Log
    {

        public static void Write(string s)
        {
            
            using (StreamWriter w = File.AppendText("log.txt"))
            {

            }
        }
    }
}