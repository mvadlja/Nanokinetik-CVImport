using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using EVMessage.MarketingAuthorisation;
using Ionic.Zip;
using Ready.Model;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;

using EVMessage.XmlValidator;
using EVMessage.AS2;

namespace EVMessage
{
    public class ConsoleTest
    {
        public static void Main()
        {
            TestMAValidationModules();

            //    SendMessage();

            //    //IRecieved_message_PKOperations _recieved_message_PKOperations = new Recieved_message_PKDAL();
            //    //Recieved_message_PK recMsg = _recieved_message_PKOperations.GetEntity(40096);
            //    ////File.WriteAllBytes(@"C:\ack.xml",recMsg.msg_data);
            //    ////File.WriteAllText(@"C:\ack1.xml", ASCIIEncoding.ASCII.GetString(recMsg.msg_data), ASCIIEncoding.ASCII);
            //    ////File.WriteAllText(@"C:\ack2.xml", UTF8Encoding.UTF8.GetString(recMsg.msg_data), UTF8Encoding.UTF8);

            //    ////string xmlStr = ExtractACK(recMsg.msg_data, UTF8Encoding.UTF8);
            //    //string xmlStr1 = ExtractACK(recMsg.msg_data, ASCIIEncoding.ASCII);

            //    ////xEVMPD.ACKMessage ack = new xEVMPD.ACKMessage();
            //    ////ack.From(xmlStr);

            //    //xEVMPD.ACKMessage ack1 = new xEVMPD.ACKMessage();
            //    //ack1.From(xmlStr1);

            //    //ISent_message_PKOperations op = new Sent_message_PKDAL();
            //    //Sent_message_PK msg = op.GetEntity(71);
            //    //File.WriteAllBytes("D:\\mdn.txt", msg.msg_data);
            //    ////XMLValidatorTest();


            //    //SaveMAAttachmentToDB(@"D:\Downloads\Gliclazida_Billev_SPC_PT.DOC");

            //    //MADataExporterTest();

            //    //TestXevprmValidationModule();

            //    //TestMAValidationModules();
        }

        //private static void SendMessage()
        //{
        //    string messageNum = "201210260918";
        //    string testMessagePath = @"D:\Downloads\";
        //    byte[] xmsgZipData = File.ReadAllBytes(testMessagePath + messageNum + ".zip");
        //    AS2SendSettings as2SendSettings;

        //    // Billev RACKSPACE test environment
        //    as2SendSettings = new AS2SendSettings()
        //    {
        //        Uri = new Uri("http://195.144.18.238:8080/exchange/EVMPDHVAL"),
        //        MsgData = xmsgZipData,
        //        Filename = messageNum + ".zip",
        //        MessageID = messageNum,
        //        From = "BPET",
        //        To = "EVMPDHVAL",
        //        TimeoutMs = 10000, // 10 sec
        //        signingCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415",
        //        recipientCertThumbPrint = "2BFC2DE0B850CCC919AF128ADF2DB526A5850198",
        //        MDNReceiptURL = "http://5.79.32.172:4080/AS2Listener.ashx"
        //    };

        //    // EMA ACK simulation test environment
        //    as2SendSettings = new AS2SendSettings()
        //    {
        //        Uri = new Uri("http://5.79.32.172:4080/AS2Listener.ashx"),
        //        MsgData = xmsgZipData,
        //        Filename = messageNum + ".zip",
        //        MessageID = messageNum,
        //        From = "EVMPDHVAL",
        //        To = "BPET",
        //        TimeoutMs = 10000, // 10 sec
        //        signingCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPET sign, not relevant
        //        recipientCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPET enc
        //        MDNReceiptURL = ""
        //    };

        //    AS2Send.SendFile(as2SendSettings);

        //    // EMA MDN simulation test environment
        //    as2SendSettings = new AS2SendSettings()
        //    {
        //        Uri = new Uri("http://5.79.32.172:4080/AS2Listener.ashx"),
        //        MsgData = xmsgZipData,
        //        Filename = messageNum + ".xml",
        //        MessageID = messageNum,
        //        From = "EVMPDHVAL",
        //        To = "BPET",
        //        TimeoutMs = 10000, // 10 sec
        //        signingCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPET sign, not relevant
        //        recipientCertThumbPrint = "", //
        //        MDNReceiptURL = ""
        //    };

        //    AS2Send.SendFile(as2SendSettings);

        //    // EMA ACK simulation prod environment
        //    as2SendSettings = new AS2SendSettings()
        //    {
        //        Uri = new Uri("http://5.79.32.172:4080/AS2Listener.ashx"),
        //        MsgData = xmsgZipData,
        //        Filename = messageNum + ".zip",
        //        MessageID = messageNum,
        //        From = "EVMPDH",
        //        To = "BPEP",
        //        TimeoutMs = 10000, // 10 sec
        //        signingCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPEP sign, not relevant
        //        recipientCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPEP enc
        //        MDNReceiptURL = ""
        //    };

        //    AS2Send.SendFile(as2SendSettings);

        //    // EMA MDN simulation prod environment
        //    as2SendSettings = new AS2SendSettings()
        //    {
        //        Uri = new Uri("http://5.79.32.172:4080/AS2Listener.ashx"),
        //        MsgData = xmsgZipData,
        //        Filename = messageNum + ".xml",
        //        MessageID = messageNum,
        //        From = "EVMPDH",
        //        To = "BPEP",
        //        TimeoutMs = 10000, // 10 sec
        //        signingCertThumbPrint = "A68DA138AA65D1DD9663CA798CF7894E79944415", // BPEP sign, not relevant
        //        recipientCertThumbPrint = "",
        //        MDNReceiptURL = ""
        //    };

        //    AS2Send.SendFile(as2SendSettings);
        //}

        //public static string ExtractACK(byte[] data, Encoding encoding)
        //{
        //    string ACK = string.Empty;

        //    string messageStr = encoding.GetString(data).Replace("\0", "");

        //    int ACKstart = messageStr.IndexOf("<?xml");
        //    int ACKend = messageStr.IndexOf("</evprmack>") + ("</evprmack>").Length;

        //    if (ACKstart != -1 && ACKend != -1)
        //    {
        //        ACK = messageStr.Substring(ACKstart, ACKend - ACKstart);
        //        ACK = ACK.Replace("UTF-8", "UTF-16");
        //    }
        //    return ACK;
        //}

        //public static void XMLValidatorTest()
        //{
        //    string filename = @"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\ma.xml";

        //    byte[] data = File.ReadAllBytes(filename);

        //    XmlValidator.XmlValidator xmlValidator = new XmlValidator.XmlValidator();

        //    bool isValid = xmlValidator.Validate(data, ASCIIEncoding.ASCII.GetBytes(EVMessage.Properties.Resources.MarketingAuthorisationXSD));

        //    MarketingAuthorisation.Schema.messageheaderType messageheader;

        //    if (isValid)
        //    {
        //        //MarketingAuthorisation.Schema.XRoot xroot = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(File.ReadAllText(filename));
        //        //xEVMPD.ACKMessage msg = new xEVMPD.ACKMessage();
        //        //msg.From( File.ReadAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\ack.xml",Encoding.UTF8));

        //        //xroot = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(File.ReadAllText(filename,Encoding.UTF8));
        //        //messageheader = xroot.marketingauthorisation.messageheader;

        //        //StatusReport.StatusReportMessage statusReportFactory = new StatusReport.StatusReportMessage();

        //        //statusReportFactory.CreateStatusReportFor_ACKReceivedFromEMA(messageheader, msg,"ready_id_example");

        //        //string xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\testACKReceived.xml", xml);

        //        //statusReportFactory.CreateStatusReportFor_MAReceived(messageheader);
        //        //xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\testMessageReceived.xml", xml);

        //        //statusReportFactory.CreateStatusReportFor_MAValidationSuccessfull(messageheader,"dsfsdf");
        //        //xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\testMAValidationSucessfull.xml", xml);

        //        //statusReportFactory.CreateStatusReportFor_MAValidationFailed(messageheader, "dsfsdf");
        //        //xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\testMAValidationFailed.xml", xml);

        //        //statusReportFactory.CreateStatusReportFor_MASentToEMA(messageheader, "dsfsdf");
        //        //xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\testMASentToEma.xml", xml);

        //    }
        //    else
        //    {
        //        //List<string> validationErrors = xmlValidator.GetValidationErrors();
        //        //List<string> validationWarnings = xmlValidator.GetValidationWarnings();

        //        //XmlValidator.XmlValidator xmlReportValidator = new XmlValidator.XmlValidator();
        //        //bool isreportValid;

        //        //xmlValidator.GetValidationExceptions().ForEach(delegate(string item) { Console.WriteLine(item); });

        //        // messageheader = null;

        //        //try
        //        //{
        //        //    MarketingAuthorisation.Schema.XRoot xroot = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(File.ReadAllText(filename));
        //        //    messageheader = xroot.marketingauthorisation.messageheader;
        //        //}
        //        //catch
        //        //{
        //        //    //messageheader = new MarketingAuthorisation.Schema.messageheaderType();
        //        //}

        //        //StatusReport.StatusReportMessage statusReportFactory = new StatusReport.StatusReportMessage();

        //        //statusReportFactory.CreateStatusReportFor_MAXmlValidationFailure(messageheader,xmlValidator,Encoding.UTF8.GetString(data));


        //        //string xml = statusReportFactory.StatusReport.ToString();
        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\validationFailed1.xml", xml);

        //        //isreportValid = xmlReportValidator.Validate(xml, EVMessage.Properties.Resources.StatusReportXSD);

        //        //statusReportFactory.CreateStatusReportFor_MAXmlValidationFailure(null, xmlValidator, "sdfsdf");

        //        //xml = statusReportFactory.StatusReport.ToString();

        //        //isreportValid = xmlReportValidator.Validate(xml, EVMessage.Properties.Resources.StatusReportXSD);

        //        //statusReportFactory.CreateStatusReportFor_MAXmlValidationFailure(messageheader, null, "sdfsdf");

        //        //xml = statusReportFactory.StatusReport.ToString();

        //        //isreportValid = xmlReportValidator.Validate(xml, EVMessage.Properties.Resources.StatusReportXSD);

        //        //statusReportFactory.CreateStatusReportFor_MAXmlValidationFailure(null, null, "sdfsdf");

        //        //xml = statusReportFactory.StatusReport.ToString();

        //        //isreportValid = xmlReportValidator.Validate(xml, EVMessage.Properties.Resources.StatusReportXSD);

        //        //File.WriteAllText(@"C:\Users\Tomo\Desktop\PossimusIT\SANDOZ schemas\validationFailes.xml", xml);

        //    }
        //}

        //public static void MADataExporterTest()
        //{
        //    string filename = @"C:\ma.xml"; //ma xml
        //    int userPK = 3;                 //ready user PK
        //    int responsibleUserPK = 23;     //ready person PK
        //    int maPK = 1;                   //marketing authorisation PK

        //    byte[] data = File.ReadAllBytes(filename);

        //    XmlValidator.XmlValidator xmlValidator = new XmlValidator.XmlValidator();

        //    bool isValid = xmlValidator.Validate(data, ASCIIEncoding.ASCII.GetBytes(EVMessage.Properties.Resources.MarketingAuthorisationXSD));

        //    if (isValid)
        //    {
        //        MarketingAuthorisation.Schema.XRoot xroot = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(File.ReadAllText(filename));

        //        IMarketing_authorisation_PKOperations _marketing_authorisation_PKOperations = new Marketing_authorisation_PKDAL();
        //        IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
        //        Marketing_authorisation_PK dbMarketingAuthorisation = _marketing_authorisation_PKOperations.GetEntity(maPK);

        //        if (dbMarketingAuthorisation != null)
        //        {
        //            var maDataExporter = new MADataExporter();

        //            maDataExporter.MADataExporterMode = MarketingAuthorisation.MADataExporter.MADataExporterModeType.Validate;
        //            maDataExporter.ResponisbleUserPK = responsibleUserPK;
        //            maDataExporter.MASaveOptions.RollbackAllAtSaveException = true;

        //            bool isSuccessful = maDataExporter.ExportMAToDB(xroot.marketingauthorisation, dbMarketingAuthorisation.marketing_authorisation_PK);

        //            if (isSuccessful)
        //            {
        //                if (maDataExporter.MADataExporterMode == MarketingAuthorisation.MADataExporter.MADataExporterModeType.Validate)
        //                {
        //                    Console.WriteLine("MA data translation validation succeeded.");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("MA successfully exported to database.");

        //                    List<Xevprm_message_PK> insertedXevprmMessageList = new List<Xevprm_message_PK>(maDataExporter.InsertedAuthorisedProducts.Count);

        //                    foreach (AuthorisedProduct authorisedProduct in maDataExporter.InsertedAuthorisedProducts)
        //                    {
        //                        Xevprm.OperationResult insertResult = Xevprm.Xevprm.CreateNewMessage(authorisedProduct, XevprmEntityType.AuthorisedProduct, XevprmOperationType.Insert, userPK, null, 7);
        //                        if (insertResult.IsSuccess && insertResult.Result != null && insertResult.Result is Xevprm_message_PK)
        //                        {
        //                            Xevprm_message_PK message = insertResult.Result as Xevprm_message_PK;

        //                            message.message_number = dbMarketingAuthorisation.ready_id + "_" + message.message_number;

        //                            message = _xevprm_message_PKOperations.Save(message);

        //                            insertedXevprmMessageList.Add(message);
        //                        }
        //                    }

        //                    Console.WriteLine(string.Format("{0}/{1} xevprm messages successfully inserted.", insertedXevprmMessageList.Count, maDataExporter.InsertedAuthorisedProducts.Count));

        //                    for (int xevprmMessageIndex = 0; xevprmMessageIndex < insertedXevprmMessageList.Count; xevprmMessageIndex++)
        //                    {
        //                        Xevprm_message_PK xevprmMessage = insertedXevprmMessageList[xevprmMessageIndex];

        //                        Xevprm.XevprmXml xevprmXml = new Xevprm.XevprmXml();
        //                        Xevprm.OperationResult constructResult = xevprmXml.ConstructXevprm(xevprmMessage);

        //                        if (constructResult.IsSuccess)
        //                        {
        //                            xevprmMessage.xml = xevprmXml.Xml;

        //                            xevprmMessage = _xevprm_message_PKOperations.Save(xevprmMessage);

        //                            Xevprm.Xevprm.UpdateXevprmEntityDetailsTablesWithCurrentData(xevprmMessage);
        //                        }
        //                        else
        //                        {
        //                            if (xevprmXml.ValidationExceptions.Count > 0)
        //                            {
        //                                Console.WriteLine("Validation Exceptions:\n");

        //                                xevprmXml.ValidationExceptions.ForEach(delegate(Xevprm.XevprmValidationException item)
        //                                {
        //                                    Console.WriteLine("-------------------------------------------------------------------");
        //                                    Console.WriteLine(item.Severity == Xevprm.SeverityType.Error ? "\nError:" : "\nWarning:");
        //                                    Console.WriteLine("\nReady:");
        //                                    Console.WriteLine(string.Format("Ready details form entity type: {0} ID = '{1}'\n", item.ReadyRootEntityType.Name, item.ReadyRootEntityPk));
        //                                    Console.WriteLine(string.Format("Ready entity type with exception: {0} ID = '{1}'\n", item.ReadyEntityType.Name, item.ReadyEntityPk));
        //                                    Console.WriteLine(string.Format("Navigation URL: ApplicationURL/Views/{0}\n", item.NavigateUrl));
        //                                    Console.WriteLine(string.Format("Message:{0}\n", item.ReadyMessage));


        //                                    Console.WriteLine("\nEvprm:");
        //                                    string evprmMessage = string.Format("{0}.{1} '{2}' : {3}", item.EvprmLocation, item.EvprmPropertyName, item.EvprmPropertyValue, item.EvprmMessage);
        //                                    Console.WriteLine(evprmMessage);
        //                                });
        //                            }

        //                            if (xevprmXml.Exceptions.Count > 0)
        //                            {
        //                                Console.WriteLine("Exceptions:\n");

        //                                xevprmXml.Exceptions.ForEach(delegate(Exception item)
        //                                {
        //                                    Console.WriteLine("-------------------------------------------------------------------");
        //                                    string message = string.Format("Message: {0} \n\nInner Exception: {1} \n\nStack Trace: {2}\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
        //                                    Console.WriteLine(message);
        //                                });
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("Exporting MA ended with errors!\n");

        //                if (maDataExporter.ValidationExceptions.Count > 0)
        //                {
        //                    Console.WriteLine("Validation Exceptions:\n");

        //                    maDataExporter.ValidationExceptions.ForEach(delegate(MarketingAuthorisation.ValidationException item)
        //                    {
        //                        Console.WriteLine(item.Severity == MarketingAuthorisation.SeverityType.Error ? "\nError:" : "\nWarning:");

        //                        string message = string.Format("{0}.{1} '{2}' : {3}", item.Location, item.PropertyName, item.PropertyValue, item.Message);
        //                        Console.WriteLine(message);
        //                    });
        //                }

        //                if (maDataExporter.Exceptions.Count > 0)
        //                {
        //                    Console.WriteLine("Exceptions:\n");

        //                    maDataExporter.Exceptions.ForEach(delegate(Exception item)
        //                    {
        //                        Console.WriteLine("-------------------------------------------------------------------");
        //                        string message = string.Format("Message: {0} \n\nInner Exception: {1} \n\nStack Trace: {2}\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
        //                        Console.WriteLine(message);
        //                    });
        //                }

        //                if (maDataExporter.SaveExceptions.Count > 0)
        //                {
        //                    Console.WriteLine("Save Exceptions:\n");

        //                    maDataExporter.SaveExceptions.ForEach(delegate(Exception item)
        //                    {
        //                        Console.WriteLine("-------------------------------------------------------------------");
        //                        string message = string.Format("Message: {0} \n\nInner Exception: {1} \n\nStack Trace: {2}\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
        //                        Console.WriteLine(message);
        //                    });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine(string.Format("MA with ID = '{0}' can't be found in database.\n", maPK));
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("XML validation failed!\n\nExceptions:\n");

        //        xmlValidator.GetValidationExceptions().ForEach(delegate(string item) { Console.WriteLine("\n" + item); });
        //    }

        //    Console.ReadLine();
        //}

        //public static void SaveMAAttachmentToDB(string filename)
        //{
        //    IMa_attachment_PKOperations _ma_attachment_PKOperations = new Ma_attachment_PKDAL();

        //    if (File.Exists(filename))
        //    {
        //        byte[] fileData = File.ReadAllBytes(filename);

        //        Ma_attachment_PK maAttachment = new Ma_attachment_PK();
        //        maAttachment.file_data = fileData;
        //        maAttachment.file_name = Path.GetFileName(filename);
        //        maAttachment.file_path = Path.GetDirectoryName(filename);
        //        maAttachment.last_change = DateTime.Now;

        //        _ma_attachment_PKOperations.Save(maAttachment);
        //    }
        //}

        /// <summary>
        /// Validation test
        /// </summary>
        /// <param name="mode">>1 = XML XSD validation; 2 = (1) and MA translation validation; 3 = (1)(2) and Xevprm business rules validation</param>
        public static void TestMAValidationModules()
        {
            string startIndex = System.Configuration.ConfigurationManager.AppSettings["TestMAStartIndex"];
            string defaultFilePath = System.Configuration.ConfigurationManager.AppSettings["TestMADefaultFilePath"];
            string defaultFilename = System.Configuration.ConfigurationManager.AppSettings["TestMADefaultFileName"];

            int userPK = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TestMAUserPK"]);
            int responsibleUserPK = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TestMAResponsibleUserPK"]);
            int maPK = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TestMAMAPK"]);

            int mode = 1;
            int index = 0;
            int.TryParse(startIndex, out index);

            //Console.WriteLine("Test user:");
            //string testUser = Console.ReadLine();

            while (true)
            {
                index++;


                //string filepath = @"C:\";
                //string filename = @"C:\ma.xml";

                string filepath = defaultFilePath;
                string filename = defaultFilePath + defaultFilename;

                bool fileExists = false;
                do
                {
                    Console.Clear();
                    bool isMode = false;
                    do
                    {
                        Console.WriteLine("\tq = QUIT\n");
                        Console.WriteLine("\t1 = XML XSD validation");
                        Console.WriteLine("\t2 = (1) and MA translation validation");
                        Console.WriteLine("\t3 = (2) and Xevprm business rules validation");
                        Console.Write("\nMODE:   ");
                        string inputMode = Console.ReadLine();

                        if (inputMode == "q") return;

                        if (int.TryParse(inputMode, out mode) && (mode == 1 || mode == 2 || mode == 3))
                        {
                            isMode = true;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid mode.\n");
                        }

                    } while (!isMode);

                    Console.Clear();
                    Console.WriteLine("q = QUIT\n");
                    Console.WriteLine("Default MA xml file: {0}\n", filename);
                    Console.WriteLine("Press <enter> to use default.\n");
                    Console.Write("MA xml file: ");
                    string input = Console.ReadLine();

                    if (input == "q") return;

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        input = filename;
                    }

                    if (!input.Contains(@":\"))
                    {
                        filename = filepath + input;
                    }
                    else
                    {
                        filename = input;
                    }

                    if (File.Exists(filename))
                    {
                        fileExists = true;

                        Console.WriteLine("\nMA xml used: {0}\n", filename);
                    }
                    else
                    {
                        Console.WriteLine("File doesn't exists.");
                        Console.ReadLine();
                    }

                } while (fileExists == false);

                Console.WriteLine("Tested validation rule/s:");
                Console.WriteLine();
                string validationRule = Console.ReadLine();
                Console.WriteLine();

                string testUser = System.Configuration.ConfigurationManager.AppSettings["TestMATestUser"];
                var timeFormat = "{3:" + System.Configuration.ConfigurationManager.AppSettings["TestMAOuputFileDateFormat"] + "}";
                var time = DateTime.Now;

                string inFile = String.Format("{0}{1}_{2}_" + timeFormat + ".xml", filepath, validationRule, testUser, time);
                string outFile = String.Format("{0}{1}_{2}_" + timeFormat + "_result.txt", filepath, validationRule, testUser, time);
                TextWriter defaultConsoleOut = Console.Out;

                using (var streamWriter = new StreamWriter(outFile))
                {
                    Console.SetOut(streamWriter);

                    //Console.WriteLine("-----------------------------------------------------------------------");
                    //Console.WriteLine("Date: {0} {1}\r\n", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                    //Console.WriteLine("Results:\r\r\n");

                    byte[] data = File.ReadAllBytes(filename);
                    File.WriteAllBytes(inFile, data);

                    #region XML validation

                    var xmlValidator = new XmlValidator.XmlValidator();

                    bool isValid = xmlValidator.Validate(data, Encoding.UTF8.GetBytes(Properties.Resources.MarketingAuthorisationXSD));

                    if (isValid)
                    {
                        MarketingAuthorisation.Schema.XRoot xroot = null;

                        try
                        {
                            xroot = MarketingAuthorisation.Schema.XRoot.Parse(File.ReadAllText(filename));
                            Console.WriteLine("XML validation succeeded!\r\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("XML validation failed! Error while trying to parse MA XML.");
                            Console.WriteLine("\r\nException:\r\n\r\nMessage: {0}\r\nStackTrace: {1}\r\n", ex.Message, ex.StackTrace);

                            if (ex.InnerException != null)
                            {
                                Console.WriteLine("\r\nInner Exception:\r\n\r\nMessage: {0}\r\nStackTrace: {1}\r\n", ex.InnerException.Message, ex.InnerException.StackTrace);
                            }

                            isValid = false;
                        }

                        #region MADataTranslation
                        if (isValid && (mode == 2 || mode == 3))
                        {
                            IMarketing_authorisation_PKOperations _marketing_authorisation_PKOperations = new Marketing_authorisation_PKDAL();
                            IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
                            Marketing_authorisation_PK dbMarketingAuthorisation = _marketing_authorisation_PKOperations.GetEntity(maPK);

                            if (dbMarketingAuthorisation != null)
                            {
                                var maDataExporter = new MADataExporter();
                                if (mode == 2)
                                {
                                    maDataExporter.MADataExporterMode = MADataExporter.MADataExporterModeType.Validate;
                                }
                                else if (mode == 3)
                                {
                                    maDataExporter.MADataExporterMode = MADataExporter.MADataExporterModeType.ValidateAndExportToDB;
                                    maDataExporter.MASaveOptions.IgnoreValidationExceptions = false;
                                    maDataExporter.MASaveOptions.IgnoreErrorsInValidationProcess = false;
                                    maDataExporter.MASaveOptions.RollbackAllAtSaveException = true;
                                    maDataExporter.MASaveOptions.PopulateMAEntityMNTable = true;
                                    maDataExporter.MASaveOptions.UpdateExistingEntities = true;
                                }

                                maDataExporter.ResponisbleUserPK = responsibleUserPK;

                                bool isSuccessful = maDataExporter.ExportMAToDB(xroot.marketingauthorisation, dbMarketingAuthorisation.marketing_authorisation_PK);

                                if (isSuccessful)
                                {
                                    if (maDataExporter.MADataExporterMode == MADataExporter.MADataExporterModeType.Validate)
                                    {
                                        Console.WriteLine("MA data translation validation succeeded.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("MA successfully exported to database.");

                                        var createdXevprmMessageList = new List<Xevprm_message_PK>(maDataExporter.ExportedAuthorisedProducts.Count);

                                        foreach (var exportedAuthorisedProduct in maDataExporter.ExportedAuthorisedProducts)
                                        {
                                            string messageNumber = EVMessage.Xevprm.Xevprm.GenerateMessageNumber(dbMarketingAuthorisation.ready_id, 7);
                                            var createResult = EVMessage.Xevprm.Xevprm.CreateNewMessage(exportedAuthorisedProduct.Item1, XevprmEntityType.AuthorisedProduct, exportedAuthorisedProduct.Item2, 3, messageNumber);
                                            if (createResult.IsSuccess && createResult.Result != null)
                                            {
                                                createdXevprmMessageList.Add(createResult.Result);
                                            }
                                            else
                                            {
                                                Console.WriteLine(createResult.Description);
                                                if (createResult.Exception != null)
                                                {
                                                    Console.WriteLine("\r\nException:\r\n\r\nMessage: {0}\r\nStackTrace: {1}\r\n", createResult.Exception.Message, createResult.Exception.StackTrace);

                                                    if (createResult.Exception.InnerException != null)
                                                    {
                                                        Console.WriteLine("\r\nInner Exception:\r\n\r\nMessage: {0}\r\nStackTrace: {1}\r\n", createResult.Exception.InnerException.Message, createResult.Exception.InnerException.StackTrace);
                                                    }
                                                }
                                            }
                                        }

                                        Console.WriteLine(string.Format("{0}/{1} xevprm messages successfully created.", createdXevprmMessageList.Count, maDataExporter.InsertedAuthorisedProducts.Count));

                                        for (int xevprmMessageIndex = 0; xevprmMessageIndex < createdXevprmMessageList.Count; xevprmMessageIndex++)
                                        {
                                            Xevprm_message_PK xevprmMessage = createdXevprmMessageList[xevprmMessageIndex];

                                            var xevprmXml = new Xevprm.XevprmXml();
                                            var constructResult = xevprmXml.ConstructXevprm(xevprmMessage);

                                            if (constructResult.IsSuccess)
                                            {
                                                xevprmMessage.xml = xevprmXml.Xml;
                                                xevprmMessage.XevprmStatus = XevprmStatus.ReadyToSubmit;
                                                xevprmMessage = _xevprm_message_PKOperations.Save(xevprmMessage);

                                                Xevprm.Xevprm.UpdateXevprmEntityDetailsTables(xevprmMessage);
                                            }
                                            else
                                            {
                                                xevprmMessage.XevprmStatus = XevprmStatus.ValidationFailed;
                                                xevprmMessage = _xevprm_message_PKOperations.Save(xevprmMessage);

                                                if (xevprmXml.ValidationExceptions.Count > 0)
                                                {
                                                    Console.WriteLine("Validation Exceptions:\r\n");

                                                    xevprmXml.ValidationExceptions.ForEach(delegate(Xevprm.XevprmValidationException item)
                                                    {
                                                        Console.WriteLine("-------------------------------------------------------------------");
                                                        Console.WriteLine(item.Severity == Xevprm.SeverityType.Error ? "\r\nError:" : "\r\nWarning:");
                                                        Console.WriteLine(string.Format("Business rule: '{0}'\r\n", item.XevprmBusinessRule));

                                                        Console.WriteLine("\r\nReady:");
                                                        Console.WriteLine(string.Format("Type: '{0}' PK = '{1}'\r\n", item.ReadyEntityType.FullName, item.ReadyEntityPk));
                                                        Console.WriteLine(string.Format("Property: '{0}' Value = '{1}'\r\n", item.ReadyEntityPropertyName, item.ReadyEntityPropertyValue));
                                                        Console.WriteLine(string.Format("Navigation URL: {0}\r\n", item.NavigateUrl));
                                                        Console.WriteLine(string.Format("Message: {0}\r\n", item.ReadyMessage));


                                                        Console.WriteLine("\r\nEvprm:");
                                                        string evprmMessage = string.Format("{0}.{1} '{2}' : {3}", item.EvprmLocation, item.EvprmPropertyName, item.EvprmPropertyValue, item.EvprmMessage);
                                                        Console.WriteLine(evprmMessage);
                                                    });
                                                }

                                                if (xevprmXml.Exceptions.Count > 0)
                                                {
                                                    Console.WriteLine("Exceptions:\r\n");

                                                    xevprmXml.Exceptions.ForEach(delegate(Exception item)
                                                    {
                                                        Console.WriteLine("-------------------------------------------------------------------");
                                                        string message = string.Format("Message: {0} \r\n\r\nInner Exception: {1} \r\n\r\nStack Trace: {2}\r\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
                                                        Console.WriteLine(message);
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Exporting MA ended with errors!\r\n");

                                    if (maDataExporter.ValidationExceptions.Count > 0)
                                    {
                                        Console.WriteLine("Validation Exceptions:\r\n");

                                        maDataExporter.ValidationExceptions.ForEach(delegate(MarketingAuthorisation.ValidationException item)
                                        {
                                            Console.WriteLine(item.Severity == MarketingAuthorisation.SeverityType.Error ? "\r\nError:" : "\r\nWarning:");

                                            string message = string.Format("{0}.{1} '{2}' : {3}", item.Location, item.PropertyName, item.PropertyValue, item.Message);
                                            Console.WriteLine(message);
                                        });
                                    }

                                    if (maDataExporter.Exceptions.Count > 0)
                                    {
                                        Console.WriteLine("Exceptions:\r\n");

                                        maDataExporter.Exceptions.ForEach(delegate(Exception item)
                                        {
                                            Console.WriteLine("-------------------------------------------------------------------");
                                            string message = string.Format("Message: {0} \r\n\r\nInner Exception: {1} \r\n\r\nStack Trace: {2}\r\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
                                            Console.WriteLine(message);
                                        });
                                    }

                                    if (maDataExporter.SaveExceptions.Count > 0)
                                    {
                                        Console.WriteLine("Save Exceptions:\r\n");

                                        maDataExporter.SaveExceptions.ForEach(delegate(Exception item)
                                        {
                                            Console.WriteLine("-------------------------------------------------------------------");
                                            string message = string.Format("Message: {0} \r\n\r\nInner Exception: {1} \r\n\r\nStack Trace: {2}\r\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
                                            Console.WriteLine(message);
                                        });
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(string.Format("MA with ID = '{0}' can't be found in database.\r\n", maPK));
                            }

                        #endregion

                        }
                    }
                    else
                    {
                        Console.WriteLine("XML validation failed!\r\n\r\nExceptions:\r\n");

                        xmlValidator.GetValidationExceptions().ForEach(item => Console.WriteLine("\r\n" + item));
                    }

                    #endregion
                }

                Console.SetOut(defaultConsoleOut);
                Console.WriteLine("Output saved to: {0}", outFile);

                System.Diagnostics.Process.Start(outFile);

                Console.ReadLine();
            }
        }

        //public static void TestXevprmValidationModule()
        //{
        //    int xevprmMessagePK = 0;

        //    while (true)
        //    {
        //        Console.Clear();
        //        bool isValid = false;
        //        do
        //        {

        //            Console.Write("Validate Xevprm message with ID: ");
        //            string input = Console.ReadLine();

        //            if (input == "q") return;

        //            if (int.TryParse(input, out xevprmMessagePK))
        //            {
        //                isValid = true;
        //            }
        //            else
        //            {
        //                Console.WriteLine("Invalid ID.\n");
        //            }

        //        } while (!isValid);



        //        IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
        //        Xevprm_message_PK dbXevprmMessage = _xevprm_message_PKOperations.GetEntity(xevprmMessagePK);
        //        if (dbXevprmMessage != null)
        //        {
        //            Xevprm.XevprmXml xevprmXml = new Xevprm.XevprmXml();
        //            Xevprm.OperationResult constructResult = xevprmXml.ConstructXevprm(dbXevprmMessage);

        //            if (constructResult.IsSuccess)
        //            {
        //                dbXevprmMessage.xml = xevprmXml.Xml;
        //                dbXevprmMessage = _xevprm_message_PKOperations.Save(dbXevprmMessage);

        //                Xevprm.Xevprm.UpdateXevprmEntityDetailsTablesWithCurrentData(dbXevprmMessage);

        //                Console.WriteLine(string.Format("Xevprm message ID = '{0}': Validation succeeded. Message is 'Ready!'\n", dbXevprmMessage.xevprm_message_PK));
        //            }
        //            else
        //            {
        //                if (xevprmXml.ValidationExceptions.Count > 0)
        //                {
        //                    Console.WriteLine("Validation Exceptions:\n");

        //                    xevprmXml.ValidationExceptions.ForEach(delegate(Xevprm.XevprmValidationException item)
        //                    {
        //                        StringBuilder sb = new StringBuilder();
        //                        item.RelatedBusinessRuleList.ForEach(br => sb.Append(", " + br));
        //                        string businessRules = sb.ToString();
        //                        businessRules = businessRules.StartsWith(", ") ? businessRules.Substring(2) + ": " : string.Empty;

        //                        Console.WriteLine("-------------------------------------------------------------------");
        //                        Console.WriteLine(item.Severity == Xevprm.SeverityType.Error ? "\nError:" : "\nWarning:");
        //                        Console.WriteLine("\nReady:");
        //                        Console.WriteLine(string.Format("Ready details form entity type: {0} ID = '{1}'\n", item.ReadyRootEntityType.Name, item.ReadyRootEntityPk));
        //                        Console.WriteLine(string.Format("Ready entity type with exception: {0} ID = '{1}'\n", item.ReadyEntityType.Name, item.ReadyEntityPk));
        //                        Console.WriteLine(string.Format("Navigation URL: ApplicationURL/Views/{0}\n", item.NavigateUrl));
        //                        Console.WriteLine(string.Format("Message: {0}\t{1}\n",businessRules, item.ReadyMessage));


        //                        Console.WriteLine("\nEvprm:");
        //                        string evprmMessage = string.Format("{0}.{1} '{2}' : {3} {4}\n", item.EvprmLocation, item.EvprmPropertyName, item.EvprmPropertyValue,businessRules, item.EvprmMessage);
        //                        Console.WriteLine(evprmMessage);
        //                    });
        //                }

        //                if (xevprmXml.Exceptions.Count > 0)
        //                {
        //                    Console.WriteLine("Exceptions:\n");

        //                    xevprmXml.Exceptions.ForEach(delegate(Exception item)
        //                    {
        //                        Console.WriteLine("-------------------------------------------------------------------");
        //                        string message = string.Format("Message: {0} \n\nInner Exception: {1} \n\nStack Trace: {2}\n", item.Message, item.InnerException != null ? item.InnerException.Message : " - ", item.StackTrace);
        //                        Console.WriteLine(message);
        //                    });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine(string.Format("Xevprm message with ID = '{0}' can't be found in database.\n", xevprmMessagePK));
        //        }
        //        Console.ReadLine();
        //    }


        //}
    }
}
