using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Ready.Model;
using System.Data;

namespace xEVMPD
{
    
    //public class evprmack 
    //{
    //    ichicsrmessageheaderType ichicsrmessageheader;
    //    acknowledgmentType acknowledgment;
    //}

    //public class acknowledgmentType  
    //{
    //    messageacknowledgmentType messageacknowledgment;
    //    reportacknowledgmentType[] reportacknowledgments;
        
    //}

    //public class messageacknowledgmentType
    //{
    //    public string evmessagenumb { get; set; }
    //    public string originalevmessagenumb { get; set; }
    //    public string originalevmessagesenderidentifier { get; set; }
    //    public string originalevmessagereceiveridentifier { get; set; }
    //    public string originalevmessagedateformat { get; set; }
    //    public string originalevmessagedate { get; set; }
    //    public string transmissionacknoledgementcode { get; set; }
    //    public string parsingerrormessage { get; set; }
    //}

    //public class reportacknowledgmentType
    //{
    //    public string reportname { get; set; }
    //    public string localnumber { get; set; }
    //    public string EV_code { get; set; }
    //    public int operationtype { get; set; }
    //    public int operationresult { get; set; }
    //    public string operationresultdesc { get; set; }
    //    public reportcommentType reportcomments;
    //}

    //public class reportcommentType
    //{
    //    public int severity { get; set; }
    //    public string sectionname { get; set; }
    //    public string fieldname { get; set; }
    //    public string fieldvalue { get; set; }
    //    public int commentcode { get; set; }
    //    public string commenttext { get; set; }
    //}

    
}
