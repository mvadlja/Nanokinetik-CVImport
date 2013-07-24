using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Ionic.Zip;
using EVMessage;
using EVMessage.AS2;
using xEVMPD;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;

namespace xEVMPD
{
    public class EVMessageBase
    {
        protected object message;
        protected virtual Type MyType { get { return null; } }

        public string ToXmlString()
        {
            try
            {
                StringBuilder xmlSB = new StringBuilder();
                string evprm = (message as evprm).ToString();
                evprm = evprm.Replace("<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\">",
                        "<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\" xmlns:ssi=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd_ssi\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd http://eudravigilance.ema.europa.eu/schema/emaxevmpd.xsd\">");
                
                xmlSB.Append(evprm);
                return xmlSB.ToString();
            }
            catch 
            {
                return String.Empty;
            }
        }

    }
}
