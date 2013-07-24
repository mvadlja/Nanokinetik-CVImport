using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool AreProductIndicationsValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidateProductIndications(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateProductIndications(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.productindicationsLocalType evprmProductIndications;
            return ConstructProductIndications(dbAuthorisedProduct, operationType, out evprmProductIndications);
        }

        public static bool IsProductIndicationValid(AuthorisedProduct dbAuthorisedProduct, Meddra_pk dbProductIndication, XevprmOperationType operationType)
        {
            return ValidateProductIndication(dbAuthorisedProduct, dbProductIndication, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateProductIndication(AuthorisedProduct dbAuthorisedProduct, Meddra_pk dbProductIndication, XevprmOperationType operationType)
        {
            authorisedproductType.productindicationsLocalType.productindicationLocalType evprmProductIndication;
            return ConstructProductIndication(dbAuthorisedProduct, dbProductIndication, operationType, out evprmProductIndication);
        }

        private static ValidationResult ConstructProductIndications(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType.productindicationsLocalType evprmProductIndications, string evprmAuthProdLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmProductIndications = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructProductAtcs: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            IMeddra_pkOperations meddraOperations = new Meddra_pkDAL();

            string evprmProductIndicationsLocation = string.Format("{0}.productindications", evprmAuthProdLocation);

            string readyAuthProdNavigateUrl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);

            var dbProductIndicationList = meddraOperations.GetMeddraByAp(dbAuthorisedProduct.ap_PK);

            if (dbProductIndicationList != null && dbProductIndicationList.Count > 0)
            {
                var evprmProductIndicationList = new List<authorisedproductType.productindicationsLocalType.productindicationLocalType>();

                int productIndicationIndex = 0;

                foreach (var dbProductIndication in dbProductIndicationList)
                {
                    string evprmProductIndicationLocation = string.Format("{0}.productindication[{1}]", evprmProductIndicationsLocation, productIndicationIndex);

                    authorisedproductType.productindicationsLocalType.productindicationLocalType evprmProductIndication;

                    ValidationResult operationResult = ConstructProductIndication(dbAuthorisedProduct, dbProductIndication, operationType, out evprmProductIndication, evprmProductIndicationLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                    if (evprmProductIndication != null)
                    {
                        if (evprmProductIndicationList.Any(productIndication => productIndication.meddracode == evprmProductIndication.meddracode))
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.BR2(), operationType);
                            Meddra_pk indication = dbProductIndication;
                            exception.AddReadyDescription(readyAuthProdNavigateUrl, () => indication.meddra_pk, () => indication.code);
                            exception.AddEvprmDescription(evprmProductIndicationLocation, "meddracode", dbProductIndication.code);
                            validationExceptions.Add(exception);
                        }

                        evprmProductIndicationList.Add(evprmProductIndication);

                        productIndicationIndex++;
                    }
                }

                if (evprmProductIndicationList.Count > 0)
                {
                    evprmProductIndications = new authorisedproductType.productindicationsLocalType();
                    evprmProductIndications.productindication = evprmProductIndicationList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmProductIndicationList.Count != dbProductIndicationList.Count)
                {
                    exceptions.Add(new Exception("Product indications: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, typeof(Meddra_ap_mn_PK), null, "meddra_ap_mn_PK", null);
                exception.AddEvprmDescription(evprmProductIndicationsLocation, "productindication", null);
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructProductIndication(AuthorisedProduct dbAuthorisedProduct, Meddra_pk dbProductIndication, XevprmOperationType operationType, out authorisedproductType.productindicationsLocalType.productindicationLocalType evprmProductIndication, string evprmProductIndicationLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmProductIndication = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructProductIndication: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            if (dbProductIndication == null)
            {
                const string message = "ConstructProductIndication: Product indication is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            string readyAuthProdNavigateUrl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);

            IType_PKOperations typeOperations = new Type_PKDAL();

            evprmProductIndication = new authorisedproductType.productindicationsLocalType.productindicationLocalType();

            if (!string.IsNullOrWhiteSpace(dbProductIndication.code))
            {
                if (IsValidInt(dbProductIndication.code.Trim()))
                {
                    int code = Convert.ToInt32(dbProductIndication.code.Trim());

                    if (Math.Abs(code).ToString().Length <= 8)
                    {
                        evprmProductIndication.meddracode = code;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddracode.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbProductIndication.meddra_pk, () => dbProductIndication.code);
                        exception.AddEvprmDescription(evprmProductIndicationLocation, "meddracode", dbProductIndication.code);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddracode.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbProductIndication.meddra_pk, () => dbProductIndication.code);
                    exception.AddEvprmDescription(evprmProductIndicationLocation, "meddracode", dbProductIndication.code);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddracode.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbProductIndication.meddra_pk, () => dbProductIndication.code);
                exception.AddEvprmDescription(evprmProductIndicationLocation, "meddracode", dbProductIndication.code);
                validationExceptions.Add(exception);
            }

            Type_PK dbLevelType = dbProductIndication.level_type_FK.HasValue ? typeOperations.GetEntity(dbProductIndication.level_type_FK) : null;
            if (dbLevelType != null)
            {
                if (!string.IsNullOrWhiteSpace(dbLevelType.name))
                {
                    if (dbLevelType.name.Trim().ToUpper().In("SOC", "HLGT", "HLT", "PT", "LLT"))
                    {
                        evprmProductIndication.meddralevel = dbLevelType.name.Trim().ToUpper();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddralevel.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbLevelType.type_PK, () => dbLevelType.name);
                        exception.AddEvprmDescription(evprmProductIndicationLocation, "meddralevel", dbLevelType.name);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddralevel.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbLevelType.type_PK, () => dbLevelType.name);
                    exception.AddEvprmDescription(evprmProductIndicationLocation, "meddralevel", dbLevelType.name);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddralevel.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbProductIndication.meddra_pk, () => dbProductIndication.level_type_FK);
                exception.AddEvprmDescription(evprmProductIndicationLocation, "meddralevel", null);
                validationExceptions.Add(exception);
            }

            Type_PK dbVersionType = dbProductIndication.version_type_FK.HasValue ? typeOperations.GetEntity(dbProductIndication.version_type_FK) : null;
            if (dbVersionType != null)
            {
                if (!string.IsNullOrWhiteSpace(dbVersionType.name))
                {
                    decimal version = 0;
                    if (Regex.IsMatch(dbVersionType.name.Trim(), "^[0-9]{1,2}\\.[0-9]{1}$") && decimal.TryParse(dbVersionType.name, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out version))
                    {
                        evprmProductIndication.meddraversion = version;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddraversion.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbVersionType.type_PK, () => dbVersionType.name);
                        exception.AddEvprmDescription(evprmProductIndicationLocation, "meddraversion", dbVersionType.name);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddraversion.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbVersionType.type_PK, () => dbVersionType.name);
                    exception.AddEvprmDescription(evprmProductIndicationLocation, "meddraversion", dbVersionType.name);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.IND.meddraversion.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbProductIndication.meddra_pk, () => dbProductIndication.version_type_FK);
                exception.AddEvprmDescription(evprmProductIndicationLocation, "meddraversion", null);
                validationExceptions.Add(exception);
            }

            if (validationExceptions.Count > 0)
            {
                evprmProductIndication = null;
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
