using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using AspNetUI.Model.XlsImport;
using SpreadsheetLight;
using System.Text;
using System.Web.UI;
using Ready.Model.Business;

namespace AspNetUI.Views.XlsImport
{
    public partial class XlsImport : FormPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdatePanel updatePanel = Page.Master.FindControl("upCommon") as UpdatePanel;
            UpdatePanelControlTrigger trigger = new PostBackTrigger();
            trigger.ControlID = btnImport.UniqueID;
            updatePanel.Triggers.Add(trigger);
        }


        public void btnImport_OnClick(object sender, EventArgs e)
        {
            ImportFile();
        }

        public void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ActivityView/List.aspx?EntityContext=Activity");
        }

        private void ImportFile()
        {
            if (fileUpload.HasFile)
            {
                XlsData data = SerializeExcel(fileUpload.PostedFile.InputStream);

                XlsImportDAL xlsImportDAL = new XlsImportDAL();

                string validationMessage = xlsImportDAL.ValidateData(data.ValidationData);

                // if there is no validation message than everything is ok
                if (String.IsNullOrEmpty(validationMessage))
                {
                    bool importSuccessful = xlsImportDAL.ImportData(data.Data, SessionManager.Instance.CurrentUser.UserID);
                    if (importSuccessful)
                    {
                        MasterPage.ModalPopup.ShowModalPopup("Import", "Import successful!");
                    }
                    else
                    {
                        MasterPage.ModalPopup.ShowModalPopup("Error", "Import failed!");
                    }
                }
                else
                {
                    //show valididation message
                    MasterPage.ModalPopup.ShowModalPopup("Import validation error!", validationMessage.Replace(";", "<br/>"));
                }
            }
        }
        public static XlsData SerializeExcel(Stream stream)
        {
            List<XlsProduct> listProducts = new List<XlsProduct>();
            List<XlsManufacturer> listManufacturer = new List<XlsManufacturer>();
            List<XlsAuthorisedProduct> listAuthProducts = new List<XlsAuthorisedProduct>();
            List<XlsMFDocument> listMFDocuments = new List<XlsMFDocument>();
            List<XlsPackagingMaterial> listPackagingMaterial = new List<XlsPackagingMaterial>();
            List<string> listSerializedRows = new List<string>();

            using (SLDocument doc = new SLDocument(stream))
            {
                int rowIndex = 2;
                while (IsXlsRowAvailable(doc, rowIndex))
                {
                    Dictionary<string, object> rowValues = GetXlsRowValues(doc, rowIndex);

                    XlsProduct product = AddProductToList(rowValues, ref listProducts);
                    XlsAuthorisedProduct authProduct = AddAuthProductToList(rowValues, ref listAuthProducts, product.ProductNumber);
                    AddManufacturersToList(rowValues, ref listManufacturer, product.ProductNumber);
                    AddMfDocumentsToList(rowValues, ref listMFDocuments, authProduct.RegistrationNumber);
                    AddPackagingMaterialToList(rowValues, ref listPackagingMaterial, product.ProductNumber);

                    AddSerializedRows(rowValues, ref listSerializedRows, rowIndex);

                    rowIndex++;
                }
            }

            string validationData = SerializeForValidation(listSerializedRows);
            string serizalizedExcel = SerializeForSubmission(listProducts, listAuthProducts, listManufacturer, listPackagingMaterial, listMFDocuments);


            return new XlsData { Data = serizalizedExcel, ValidationData = validationData };
        }

        private static bool IsXlsRowAvailable(SLDocument doc, int rowIndex)
        {
            return !String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 1));
        }

        private static Dictionary<string, object> GetXlsRowValues(SLDocument doc, int rowIndex)
        {
            Dictionary<string, object> rowValues = new Dictionary<string, object>();

            rowValues["mainProductName"] = doc.GetCellValueAsString(rowIndex, 1);

            rowValues["status"] = doc.GetCellValueAsString(rowIndex, 2);
            rowValues["form"] = doc.GetCellValueAsString(rowIndex, 3);
            rowValues["region"] = doc.GetCellValueAsString(rowIndex, 4);
            rowValues["nameOfProduct"] = doc.GetCellValueAsString(rowIndex, 5);
            rowValues["strength"] = doc.GetCellValueAsString(rowIndex, 7);
            rowValues["presentationDevice"] = doc.GetCellValueAsString(rowIndex, 8);
            rowValues["legalStatus"] = doc.GetCellValueAsString(rowIndex, 9);
            rowValues["MAH"] = doc.GetCellValueAsString(rowIndex, 10);
            rowValues["MAHGroup"] = doc.GetCellValueAsString(rowIndex, 11);
            rowValues["reservationConfirmed"] = doc.GetCellValueAsString(rowIndex, 13);
            rowValues["reservedTo"] = doc.GetCellValueAsString(rowIndex, 14);
            rowValues["customerGroupReservedTo"] = doc.GetCellValueAsString(rowIndex, 17);
            rowValues["country"] = doc.GetCellValueAsString(rowIndex, 19);
            rowValues["nationalDcpMrp"] = doc.GetCellValueAsString(rowIndex, 20);
            rowValues["procedureNumber"] = doc.GetCellValueAsString(rowIndex, 21);
            rowValues["registrationNumber"] = doc.GetCellValueAsString(rowIndex, 22);
            rowValues["localCodes"] = doc.GetCellValueAsString(rowIndex, 23);
            rowValues["prApproval"] = doc.GetCellValueAsString(rowIndex, 38);

            rowValues["deliveryDateToCustomer"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 39)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 39);
            rowValues["estimatedSubmissionDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 40)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 40);
            rowValues["submissionDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 41)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 41);
            rowValues["day0"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 42)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 42);
            rowValues["estimatedProcedureApprovalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 43)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 43);
            rowValues["procedureApprovalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 44)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 44);
            rowValues["submissionPhaseNational"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 45)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 45);
            rowValues["estimatedNationalApprovalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 46)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 46);
            rowValues["nationalApprovalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 47)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 47);
            rowValues["effectivelyMarketedDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 48)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 48);
            rowValues["sunsetClauseDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 49)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 49);
            rowValues["patentExpire"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 50)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 50);

            rowValues["comments"] = doc.GetCellValueAsString(rowIndex, 51);
            rowValues["manufacturers"] = doc.GetCellValueAsString(rowIndex, 52);
            rowValues["firstPackagers"] = doc.GetCellValueAsString(rowIndex, 53);
            rowValues["secondPackagers"] = doc.GetCellValueAsString(rowIndex, 54);
            rowValues["releasers"] = doc.GetCellValueAsString(rowIndex, 55);

            rowValues["firstAPISource"] = doc.GetCellValueAsString(rowIndex, 56);
            rowValues["firstDmfCep"] = doc.GetCellValueAsString(rowIndex, 57);
            rowValues["secondApiSource"] = doc.GetCellValueAsString(rowIndex, 58);
            rowValues["secondDmfCep"] = doc.GetCellValueAsString(rowIndex, 59);
            rowValues["thirdApiSource"] = doc.GetCellValueAsString(rowIndex, 60);
            rowValues["thirdDmfCep"] = doc.GetCellValueAsString(rowIndex, 61);


            rowValues["batchSizeFinishProduct"] = doc.GetCellValueAsDecimal(rowIndex, 63);
            rowValues["packagingMaterial"] = doc.GetCellValueAsString(rowIndex, 64);
            rowValues["packagingSizeProcedure"] = doc.GetCellValueAsString(rowIndex, 65);
            rowValues["packagingSizeNational"] = doc.GetCellValueAsString(rowIndex, 66);
            rowValues["submissionOfRenewalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 67)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 67);
            rowValues["renewalDate"] = String.IsNullOrEmpty(doc.GetCellValueAsString(rowIndex, 68)) ? null : (DateTime?)doc.GetCellValueAsDateTime(rowIndex, 68);
            rowValues["storageConditions"] = doc.GetCellValueAsString(rowIndex, 69);
            rowValues["shelfLife"] = doc.GetCellValueAsString(rowIndex, 70);

            return rowValues;
        }

        private static XlsProduct AddProductToList(Dictionary<string, object> rowValues, ref List<XlsProduct> listProducts)
        {
            XlsProduct product = null;

            if (listProducts.Exists(x => x.Name == (string)rowValues["mainProductName"] && x.ProductNumber == (string)rowValues["procedureNumber"]))
            {
                product = listProducts.Find(x => x.Name == (string)rowValues["mainProductName"] && x.ProductNumber == (string)rowValues["procedureNumber"]);
            }
            else
            {
                product = new XlsProduct(
                                (string)rowValues["mainProductName"],
                                (string)rowValues["procedureNumber"],
                                (string)rowValues["region"],
                                (string)rowValues["customerGroupReservedTo"],
                                (decimal)rowValues["batchSizeFinishProduct"],
                                (string)rowValues["packagingSizeProcedure"],
                                (string)rowValues["storageConditions"]);

                listProducts.Add(product);
            }

            return product;
        }

        private static void AddManufacturersToList(Dictionary<string, object> rowValues, ref List<XlsManufacturer> listManufacturer, string productNumber)
        {
            string manufacturers = (string)rowValues["manufacturers"];
            string firstPackagers = (string)rowValues["firstPackagers"];
            string secondPackagers = (string)rowValues["secondPackagers"];
            string releasers = (string)rowValues["releasers"];


            foreach (string man in manufacturers.Split(';'))
            {
                if (!String.IsNullOrEmpty(man.Trim()))
                {
                    if (!listManufacturer.Exists(x => x.ProductNumber == productNumber &&
                                     x.Name == man.Trim() &&
                                     x.Type == "Manufacturer"))
                    {
                        listManufacturer.Add(new XlsManufacturer { ProductNumber = productNumber, Name = man.Trim(), Type = "Manufacturer" });
                    }
                }
            }

            foreach (string firstPack in firstPackagers.Split(';'))
            {
                if (!String.IsNullOrEmpty(firstPack.Trim()))
                {
                    if (!listManufacturer.Exists(x => x.ProductNumber == productNumber &&
                                                      x.Name == firstPack.Trim() &&
                                                      x.Type == "PrimaryPackager"))
                    {
                        listManufacturer.Add(new XlsManufacturer { ProductNumber = productNumber, Name = firstPack.Trim(), Type = "PrimaryPackager" });
                    }
                }
            }

            foreach (string secondPack in secondPackagers.Split(';'))
            {
                if (!String.IsNullOrEmpty(secondPack.Trim()))
                {
                    if (!listManufacturer.Exists(x => x.ProductNumber == productNumber &&
                                                      x.Name == secondPack.Trim() &&
                                                      x.Type == "SecondaryPackager"))
                    {
                        listManufacturer.Add(new XlsManufacturer { ProductNumber = productNumber, Name = secondPack.Trim(), Type = "SecondaryPackager" });
                    }
                }
            }

            foreach (string releaser in releasers.Split(';'))
            {
                if (!String.IsNullOrEmpty(releaser.Trim()))
                {
                    if (!listManufacturer.Exists(x => x.ProductNumber == productNumber &&
                                                      x.Name == releaser.Trim() &&
                                                      x.Type == "Releaser"))
                    {
                        listManufacturer.Add(new XlsManufacturer { ProductNumber = productNumber, Name = releaser.Trim(), Type = "Releaser" });
                    }
                }
            }
        }

        private static XlsAuthorisedProduct AddAuthProductToList(Dictionary<string, object> rowValues, ref List<XlsAuthorisedProduct> listAuthProducts, string productNumber)
        {
            XlsAuthorisedProduct authProduct = new XlsAuthorisedProduct
            {
                ProductNumber = productNumber,

                Status = (string)rowValues["status"],
                NameOfProduct = (string)rowValues["nameOfProduct"],
                Strength = (string)rowValues["strength"],
                ProductDescription = (string)rowValues["presentationDevice"],
                LegalStatus = (string)rowValues["legalStatus"],
                LicenceHolder = (string)rowValues["MAH"],
                LicenceHolderGroup = (string)rowValues["MAHGroup"],
                ReservationConfirmed = (string)rowValues["reservationConfirmed"],
                ReservedTo = (string)rowValues["reservedTo"],
                Country = (string)rowValues["country"],
                ProcedureType = (string)rowValues["nationalDcpMrp"],
                RegistrationNumber = (string)rowValues["registrationNumber"],
                LocalCodes = (string)rowValues["localCodes"],
                DeliveryDateToCustomer = (DateTime?)rowValues["deliveryDateToCustomer"],
                EstimatedSubmissionDate = (DateTime?)rowValues["estimatedSubmissionDate"],
                SubmissionDate = (DateTime?)rowValues["submissionDate"],
                Day0 = (DateTime?)rowValues["day0"],
                EstimatedProcedureApprovalDate = (DateTime?)rowValues["estimatedProcedureApprovalDate"],
                ProcedureApprovalDate = (DateTime?)rowValues["procedureApprovalDate"],
                SubmissionPhaseNational = (DateTime?)rowValues["submissionPhaseNational"],
                EstimatedNationalApprovalDate = (DateTime?)rowValues["estimatedNationalApprovalDate"],
                NationalApprovalDate = (DateTime?)rowValues["nationalApprovalDate"],
                EffectivelyMarketedDate = (DateTime?)rowValues["effectivelyMarketedDate"],
                SunsetClauseDate = (DateTime?)rowValues["sunsetClauseDate"],
                PatentExpire = (DateTime?)rowValues["patentExpire"],
                SubmissionOfRenewalDate = (DateTime?)rowValues["submissionOfRenewalDate"],
                RenewalDate = (DateTime?)rowValues["renewalDate"],
                Comments = (string)rowValues["comments"],
                PackSize = (string)rowValues["packagingSizeNational"],
                ShelfLife = (string)rowValues["shelfLife"]
            };

            listAuthProducts.Add(authProduct);

            return authProduct;
        }

        private static void AddMfDocumentsToList(Dictionary<string, object> rowValues, ref List<XlsMFDocument> listMFDocuments, string registrationNumber)
        {
            string firstDmfCep = (string)rowValues["firstDmfCep"];
            string firstAPISource = (string)rowValues["firstAPISource"];

            string secondDmfCep = (string)rowValues["secondDmfCep"];
            string secondApiSource = (string)rowValues["secondApiSource"];

            string thirdDmfCep = (string)rowValues["thirdDmfCep"];
            string thirdApiSource = (string)rowValues["thirdApiSource"];


            for (int i = 0; i < firstDmfCep.Split('\n').Length; i++)
            {
                if (!String.IsNullOrEmpty(firstDmfCep.Split('\n')[i]))
                {
                    listMFDocuments.Add(new XlsMFDocument
                    {
                        RegistrationNumber = registrationNumber,
                        Name = firstDmfCep.Split('\n')[i],
                        IsEffective = i == 0 ? true : false,
                        ApiSource = firstAPISource
                    });
                }
            }

            for (int i = 0; i < secondDmfCep.Split('\n').Length; i++)
            {
                if (!String.IsNullOrEmpty(secondDmfCep.Split('\n')[i]))
                {
                    listMFDocuments.Add(new XlsMFDocument
                    {
                        RegistrationNumber = registrationNumber,
                        Name = secondDmfCep.Split('\n')[i],
                        IsEffective = i == 0 ? true : false,
                        ApiSource = secondApiSource
                    });
                }
            }

            for (int i = 0; i < thirdDmfCep.Split('\n').Length; i++)
            {
                if (!String.IsNullOrEmpty(thirdDmfCep.Split('\n')[i]))
                {
                    listMFDocuments.Add(new XlsMFDocument
                    {
                        RegistrationNumber = registrationNumber,
                        Name = thirdDmfCep.Split('\n')[i],
                        IsEffective = i == 0 ? true : false,
                        ApiSource = thirdApiSource
                    });
                }
            }


        }
        private static void AddPackagingMaterialToList(Dictionary<string, object> rowValues, ref List<XlsPackagingMaterial> listPackagingMaterial, string productNumber)
        {
            string packMaterialString = (string)rowValues["packagingMaterial"];


            foreach (string packMaterial in packMaterialString.Split(';'))
            {
                if (!String.IsNullOrEmpty(packMaterial.Trim()))
                {
                    if (!listPackagingMaterial.Exists(x => x.ProductNumber == productNumber &&
                                     x.Name == packMaterial.Trim()))
                    {
                        listPackagingMaterial.Add(new XlsPackagingMaterial { ProductNumber = productNumber, Name = packMaterial.Trim() });
                    }
                }
            }
        }

        private static string SerializeForSubmission(List<XlsProduct> listProducts, List<XlsAuthorisedProduct> listAuthProducts,
                                                  List<XlsManufacturer> listManufacturers, List<XlsPackagingMaterial> listPackagingMaterial,
                                                  List<XlsMFDocument> listMFDocuments)
        {
            string serizalizedExcel = "<Data>\n";

            serizalizedExcel = String.Format("{0}\t<Products>\n", serizalizedExcel);

            foreach (XlsProduct product in listProducts)
            {
                serizalizedExcel += "\t\t" + product.Serizalize();
            }

            serizalizedExcel = String.Format("{0}\t</Products>\n", serizalizedExcel);

            serizalizedExcel = String.Format("{0}\t<Manufacturers>\n", serizalizedExcel);

            foreach (XlsManufacturer manufacturer in listManufacturers)
            {
                serizalizedExcel += "\t\t" + manufacturer.Serizalize();
            }

            serizalizedExcel = String.Format("{0}\t</Manufacturers>\n", serizalizedExcel);

            serizalizedExcel = String.Format("{0}\t<PackagingMaterial>\n", serizalizedExcel);

            foreach (XlsPackagingMaterial packMaterial in listPackagingMaterial)
            {
                serizalizedExcel += "\t\t" + packMaterial.Serizalize();
            }

            serizalizedExcel = String.Format("{0}\t</PackagingMaterial>\n", serizalizedExcel);

            serizalizedExcel = String.Format("{0}\t<MFDocuments>\n", serizalizedExcel);

            foreach (XlsMFDocument document in listMFDocuments)
            {
                serizalizedExcel += "\t\t" + document.Serizalize();
            }

            serizalizedExcel = String.Format("{0}\t</MFDocuments>\n", serizalizedExcel);

            serizalizedExcel = String.Format("{0}\t<AuthorisedProducts>\n", serizalizedExcel);

            foreach (XlsAuthorisedProduct authProds in listAuthProducts)
            {
                serizalizedExcel += "\t\t" + authProds.Serizalize();
            }

            serizalizedExcel = String.Format("{0}\t</AuthorisedProducts>\n", serizalizedExcel);
            serizalizedExcel += "</Data>";

            return serizalizedExcel;
        }


        private static void AddSerializedRows(Dictionary<string, object> rowValues, ref List<string> listSerializedRows, int rowIndex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Row ");

            XMLSerializationHelper.AppendIfNotNull(sb, "RowIndex", rowIndex);
            XMLSerializationHelper.AppendIfNotNull(sb, "MainProductName", (string)rowValues["mainProductName"]);

            XMLSerializationHelper.AppendIfNotNull(sb, "Status", (string)rowValues["status"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Form", (string)rowValues["form"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Region", (string)rowValues["region"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "NameOfProduct", (string)rowValues["nameOfProduct"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Strength", (string)rowValues["strength"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductDescription", (string)rowValues["presentationDevice"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "LegalStatus", (string)rowValues["legalStatus"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "MAH", (string)rowValues["MAH"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "MAHGroup", (string)rowValues["MAHGroup"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ReservationConfirmed", (string)rowValues["reservationConfirmed"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ReservedTo", (string)rowValues["reservedTo"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "CustomerGroupReservedTo", (string)rowValues["customerGroupReservedTo"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Country", (string)rowValues["country"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ProcedureType", (string)rowValues["nationalDcpMrp"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ProcedureNumber", (string)rowValues["procedureNumber"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "RegistrationNumber", (string)rowValues["registrationNumber"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "LocalCodes", (string)rowValues["localCodes"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "PrApproval", (string)rowValues["prApproval"]);

            XMLSerializationHelper.AppendDateIfNotNull(sb, "DeliveryDateToCustomer", (DateTime?)rowValues["deliveryDateToCustomer"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedSubmissionDate", (DateTime?)rowValues["estimatedSubmissionDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionDate", (DateTime?)rowValues["submissionDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "Day0", (DateTime?)rowValues["day0"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedProcedureApprovalDate", (DateTime?)rowValues["estimatedProcedureApprovalDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "ProcedureApprovalDate", (DateTime?)rowValues["procedureApprovalDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionPhaseNational", (DateTime?)rowValues["submissionPhaseNational"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedNationalApprovalDate", (DateTime?)rowValues["estimatedNationalApprovalDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "NationalApprovalDate", (DateTime?)rowValues["nationalApprovalDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EffectivelyMarketedDate", (DateTime?)rowValues["effectivelyMarketedDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SunsetClauseDate", (DateTime?)rowValues["sunsetClauseDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "PatentExpire", (DateTime?)rowValues["patentExpire"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionOfRenewalDate", (DateTime?)rowValues["submissionOfRenewalDate"]);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "RenewalDate", (DateTime?)rowValues["renewalDate"]);

            XMLSerializationHelper.AppendIfNotNull(sb, "Comments", (string)rowValues["comments"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Manufacturers", (string)rowValues["manufacturers"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "FirstPackagers", (string)rowValues["firstPackagers"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "SecondPackagers", (string)rowValues["secondPackagers"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "Releasers", (string)rowValues["releasers"]);

            XMLSerializationHelper.AppendIfNotNull(sb, "FirstAPISource", (string)rowValues["firstAPISource"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "FirstDmfCep", (string)rowValues["firstDmfCep"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "SecondApiSource", (string)rowValues["secondApiSource"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "SecondDmfCep", (string)rowValues["secondDmfCep"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ThirdApiSource", (string)rowValues["thirdApiSource"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ThirdDmfCep", (string)rowValues["thirdDmfCep"]);

            XMLSerializationHelper.AppendDecimalIfNotNull(sb, "BatchSizeFinishProduct", (decimal?)rowValues["batchSizeFinishProduct"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "PackagingMaterial", (string)rowValues["packagingMaterial"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "PackagingSizeProcedure", (string)rowValues["packagingSizeProcedure"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "PackagingSizeNational", (string)rowValues["packagingSizeNational"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "StorageConditions", (string)rowValues["storageConditions"]);
            XMLSerializationHelper.AppendIfNotNull(sb, "ShelfLife", (string)rowValues["shelfLife"]);

            sb.Append("/>");
            listSerializedRows.Add(sb.ToString().Replace("\n", "; ") + "\n");
        }


        private static string SerializeForValidation(List<string> listSerializedRows)
        {
            string serializedValues = "<Data>\n";

            foreach (String row in listSerializedRows)
            {
                serializedValues += "\t" + row;
            }

            serializedValues += "</Data>";

            return serializedValues;
        }
    }
}