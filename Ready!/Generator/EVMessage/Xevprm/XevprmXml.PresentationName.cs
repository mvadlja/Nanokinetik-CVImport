using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool IsPresentationNameValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidatePresentationName(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        private static ValidationResult ValidatePresentationName(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.presentationnameLocalType evprmPresentationName;

            return ConstructPresentationName(dbAuthorisedProduct, operationType, out evprmPresentationName);
        }

        private static ValidationResult ConstructPresentationName(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType.presentationnameLocalType evprmPresentationName, string evprmAuthProdLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmPresentationName = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructPresentationName: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            evprmPresentationName = new authorisedproductType.presentationnameLocalType();

            string evprmPresentationNameLocation = string.Format("{0}.presentation", evprmAuthProdLocation);

            string readyAuthProdNavigateUrl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);

            //Full presentation name
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.product_name))
            {
                if (dbAuthorisedProduct.product_name.Trim().Length <= 2000)
                {
                    evprmPresentationName.productname = dbAuthorisedProduct.product_name.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productname.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_name);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productname.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_name);
                exception.AddEvprmDescription(evprmPresentationNameLocation, "productname", dbAuthorisedProduct.product_name);
                validationExceptions.Add(exception);
            }

            //Product short name
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.productshortname))
            {
                if (dbAuthorisedProduct.productshortname.Trim().Length <= 500)
                {
                    evprmPresentationName.productshortname = dbAuthorisedProduct.productshortname.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productshortname.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productshortname);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dbAuthorisedProduct.productgenericname))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productgenericname.BR2(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productgenericname);
                    exception.AddEvprmDescription(evprmPresentationNameLocation, "productgenericname", null);
                    validationExceptions.Add(exception);
                }

                if (string.IsNullOrWhiteSpace(dbAuthorisedProduct.productcompanyname))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productcompanyname.BR2(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productcompanyname);
                    exception.AddEvprmDescription(evprmPresentationNameLocation, "productcompanyname", null);
                    validationExceptions.Add(exception);
                }
            }

            //Product generic name
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.productgenericname))
            {
                if (dbAuthorisedProduct.productgenericname.Trim().Length <= 1000)
                {
                    evprmPresentationName.productgenericname = dbAuthorisedProduct.productgenericname.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productgenericname.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productgenericname);
                    validationExceptions.Add(exception);
                }
            }

            //Product company name
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.productcompanyname))
            {
                if (dbAuthorisedProduct.productcompanyname.Trim().Length <= 250)
                {
                    evprmPresentationName.productcompanyname = dbAuthorisedProduct.productcompanyname.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productcompanyname.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productcompanyname);
                    validationExceptions.Add(exception);
                }
            }

            //Product strength
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.productstrenght))
            {
                if (dbAuthorisedProduct.productstrenght.Trim().Length <= 250)
                {
                    evprmPresentationName.productstrength = dbAuthorisedProduct.productstrenght.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productstrenght.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productstrenght);
                    validationExceptions.Add(exception);
                }
            }

            //Product form
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.productform))
            {
                if (dbAuthorisedProduct.productform.Trim().Length <= 500)
                {
                    evprmPresentationName.productform = dbAuthorisedProduct.productform.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.productform.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.productform);
                    validationExceptions.Add(exception);
                }
            }

            //Package description
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.packagedesc))
            {
                if (dbAuthorisedProduct.packagedesc.Trim().Length <= 2000)
                {
                    evprmPresentationName.packagedesc = dbAuthorisedProduct.packagedesc.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.presentationname.packagedesc.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.packagedesc);
                    validationExceptions.Add(exception);
                }
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
