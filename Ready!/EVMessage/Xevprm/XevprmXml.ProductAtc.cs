using System;
using System.Collections.Generic;
using System.Linq;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool AreProductAtcsValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidateProductAtcs(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateProductAtcs(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.productatcsLocalType evprmProductAtcs;
            return ConstructProductAtcs(dbAuthorisedProduct, operationType, out evprmProductAtcs);
        }

        public static bool IsProductAtcValid(AuthorisedProduct dbAuthorisedProduct, Atc_PK dbAtc, XevprmOperationType operationType)
        {
            return ValidateProductAtc(dbAuthorisedProduct, dbAtc, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateProductAtc(AuthorisedProduct dbAuthorisedProduct, Atc_PK dbAtc, XevprmOperationType operationType)
        {
            authorisedproductType.productatcsLocalType.productatcLocalType evprmProductAtc;
            return ConstructProductAtc(dbAuthorisedProduct, dbAtc, operationType, out evprmProductAtc);
        }

        private static ValidationResult ConstructProductAtcs(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType.productatcsLocalType evprmProductAtcs, string evprmAuthProdLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmProductAtcs = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructProductAtcs: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            IAtc_PKOperations atcOperations = new Atc_PKDAL();
            IProduct_PKOperations productOperations = new Product_PKDAL();

            var dbProduct = productOperations.GetEntity(dbAuthorisedProduct.product_FK);
            if (dbProduct == null)
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.BRCustom1(), operationType);
                exception.AddReadyDescription(NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType), () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_FK);
                validationExceptions.Add(exception);
                validationExceptionTree.Value.XevprmValidationExceptions.Add(exception);

                return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
            }

            string evprmProductAtcsLocation = string.Format("{0}.productatcs", evprmAuthProdLocation);

            var dbProductAtcList = dbProduct.product_PK.HasValue ? atcOperations.GetEntitiesByProduct(dbProduct.product_PK.Value) : new List<Atc_PK>();

            if (dbProductAtcList != null && dbProductAtcList.Count > 0)
            {
                var evprmProductAtcList = new List<authorisedproductType.productatcsLocalType.productatcLocalType>();

                int productAtcIndex = 0;

                foreach (var dbProductAtc in dbProductAtcList)
                {
                    string evprmProductAtcLocation = string.Format("{0}.productatc[{1}]", evprmProductAtcsLocation, productAtcIndex);

                    authorisedproductType.productatcsLocalType.productatcLocalType evprmProductAtc;
                    ValidationResult operationResult = ConstructProductAtc(dbAuthorisedProduct, dbProductAtc, operationType, out evprmProductAtc, evprmProductAtcLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                    if (evprmProductAtc != null)
                    {
                        if (evprmProductAtc.atccode != null && !string.IsNullOrWhiteSpace(evprmProductAtc.atccode.TypedValue))
                        {
                            if (evprmProductAtcList.Any(atc => atc.atccode != null && !string.IsNullOrWhiteSpace(atc.atccode.TypedValue) && atc.atccode.TypedValue == evprmProductAtc.atccode.TypedValue))
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.AP.ATC.BR1(), operationType);
                                Atc_PK atc = dbProductAtc;
                                exception.AddReadyDescription(NavigateUrl.Product(dbProduct.product_PK, dbAuthorisedProduct.ap_PK, operationType), () => atc.atc_PK, () => atc.atccode);
                                exception.AddEvprmDescription(evprmProductAtcLocation, "atccode", evprmProductAtc.atccode.TypedValue);
                                validationExceptions.Add(exception);
                            }
                        }

                        evprmProductAtcList.Add(evprmProductAtc);

                        productAtcIndex++;
                    }
                }

                if (evprmProductAtcList.Count > 0)
                {
                    evprmProductAtcs = new authorisedproductType.productatcsLocalType();
                    evprmProductAtcs.productatc = evprmProductAtcList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmProductAtcList.Count != dbProductAtcList.Count)
                {
                    exceptions.Add(new Exception("Product atcs: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.ATC.Cardinality(), operationType);
                exception.AddReadyDescription(NavigateUrl.Product(dbProduct.product_PK, dbAuthorisedProduct.ap_PK, operationType), typeof(Product_atc_mn_PK), null, "product_atc_mn_PK", null);
                exception.AddEvprmDescription(evprmProductAtcsLocation, "productatc", null);
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructProductAtc(AuthorisedProduct dbAuthorisedProduct, Atc_PK dbProductAtc, XevprmOperationType operationType, out authorisedproductType.productatcsLocalType.productatcLocalType evprmProductAtc, string evprmProductAtcLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmProductAtc = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructProductAtcs: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            string readyAuthProdNavigateUrl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);

            if (dbProductAtc == null)
            {
                const string message = "ConstructProductAtc: Product ATC is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            IProduct_PKOperations productOperations = new Product_PKDAL();

            var dbProduct = productOperations.GetEntity(dbAuthorisedProduct.product_FK);
            if (dbProduct == null)
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.BRCustom1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_FK);
                validationExceptions.Add(exception);
                validationExceptionTree.Value.XevprmValidationExceptions.Add(exception);

                return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
            }

            string readyProdNavigateUrl = NavigateUrl.Product(dbProduct.product_PK, dbAuthorisedProduct.ap_PK, operationType);

            if (!string.IsNullOrWhiteSpace(dbProductAtc.atccode))
            {

                if (dbProductAtc.atccode.Trim().Length <= 60)
                {
                    evprmProductAtc = new authorisedproductType.productatcsLocalType.productatcLocalType();
                    evprmProductAtc.atccode = new authorisedproductType.productatcsLocalType.productatcLocalType.atccodeLocalType();

                    evprmProductAtc.atccode.TypedValue = dbProductAtc.atccode.Trim();
                    evprmProductAtc.atccode.resolutionmode = 2;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.ATC.atccode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProductAtc.atc_PK, () => dbProductAtc.atccode);
                    exception.AddEvprmDescription(evprmProductAtcLocation, "atccode", dbProductAtc.atccode.Trim());
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.ATC.atccode.Cardinality(), operationType);
                exception.AddReadyDescription(readyProdNavigateUrl, () => dbProductAtc.atc_PK, () => dbProductAtc.atccode);
                exception.AddEvprmDescription(evprmProductAtcLocation, "atccode", dbProductAtc.atccode.Trim());
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
