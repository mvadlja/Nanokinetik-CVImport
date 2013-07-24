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
        private static bool IsAuthorisationValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidateAuthorisation(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        private static ValidationResult ValidateAuthorisation(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.authorisationLocalType evprmAuthorisation;
            return ConstructAuthorisation(dbAuthorisedProduct, operationType, out evprmAuthorisation);
        }

        private static ValidationResult ConstructAuthorisation(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType.authorisationLocalType evprmAuthorisation, string evprmAPLocation = "")
        {
            var authProdValidationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var prodValidationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmAuthorisation = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructAuthorisation: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            authProdValidationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            evprmAuthorisation = new authorisedproductType.authorisationLocalType();

            ICountry_PKOperations countryOperations = new Country_PKDAL();
            IType_PKOperations typeOperations = new Type_PKDAL();
            IProduct_PKOperations productOperations = new Product_PKDAL();

            string evprmAuthorisationLocation = string.Format("{0}.authorisation", evprmAPLocation);

            var dbProduct = productOperations.GetEntity(dbAuthorisedProduct.product_FK);
            if (dbProduct == null)
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.BRCustom1(), operationType);
                exception.AddReadyDescription(NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType), () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_FK);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }
            else
            {
                prodValidationExceptionTree.Value.ReadyEntity = dbProduct;
            }


            string readyAuthProdNavigateUrl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);


            //Authorisation country code
            var dbCountry = dbAuthorisedProduct.authorisationcountrycode_FK.HasValue ? countryOperations.GetEntity(dbAuthorisedProduct.authorisationcountrycode_FK) : null;
            if (dbCountry != null)
            {
                if (!string.IsNullOrWhiteSpace(dbCountry.abbreviation))
                {
                    if (dbCountry.abbreviation.Trim().Length <= 2)
                    {
                        evprmAuthorisation.authorisationcountrycode = dbCountry.abbreviation.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationcountrycode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbCountry.country_PK, () => dbCountry.abbreviation);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationcountrycode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbCountry.country_PK, () => dbCountry.abbreviation);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationcountrycode.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationcountrycode_FK);
                exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationcountrycode", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }


            //Authorisation number
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.authorisationnumber))
            {
                if (dbAuthorisedProduct.authorisationnumber.Trim().Length <= 100)
                {
                    evprmAuthorisation.authorisationnumber = dbAuthorisedProduct.authorisationnumber.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationnumber);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation, XevprmOperationType.Withdraw))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationnumber.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationnumber);
                exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationnumber", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            if (dbProduct != null)
            {
                string readyProdNavigateUrl = NavigateUrl.Product(dbProduct.product_PK, dbAuthorisedProduct.ap_PK, operationType);

                //Authorisation procedure
                var dbAuthorisationProcedureType = dbProduct.authorisation_procedure.HasValue ? typeOperations.GetEntity(dbProduct.authorisation_procedure) : null;
                if (dbAuthorisationProcedureType != null)
                {
                    int authorisationProcedure = 0;
                    if (int.TryParse(dbAuthorisationProcedureType.ev_code, out authorisationProcedure))
                    {
                        if (authorisationProcedure >= 0 && authorisationProcedure < 100)
                        {
                            evprmAuthorisation.authorisationprocedure = authorisationProcedure;
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationprocedure.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                            exception.AddReadyDescription(readyProdNavigateUrl, () => dbAuthorisationProcedureType.type_PK, () => dbAuthorisationProcedureType.ev_code);
                            validationExceptions.Add(exception);
                            prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationprocedure.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                        exception.AddReadyDescription(readyProdNavigateUrl, () => dbAuthorisationProcedureType.type_PK, () => dbAuthorisationProcedureType.ev_code);
                        validationExceptions.Add(exception);
                        prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }

                    if (!string.IsNullOrWhiteSpace(dbAuthorisationProcedureType.name))
                    {
                        if (dbAuthorisationProcedureType.name.Trim().ToLower() == "centralised")
                        {
                            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.authorisationnumber))
                            {
                                if (dbAuthorisedProduct.authorisationnumber.Trim().Length <= 50)
                                {
                                    evprmAuthorisation.eunumber = dbAuthorisedProduct.authorisationnumber.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.eunumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationnumber);
                                    validationExceptions.Add(exception);
                                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                                }

                                //If the medicinal product follows the “EU centralised procedure”, the EMA procedure number may be specified.
                                if (!string.IsNullOrWhiteSpace(dbProduct.product_number))
                                {
                                    if (dbProduct.product_number.Trim().Length <= 50)
                                    {
                                        evprmAuthorisation.mrpnumber = dbProduct.product_number.Trim();
                                    }
                                    else
                                    {
                                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                        exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                        validationExceptions.Add(exception);
                                        prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                                    }
                                }
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.eunumber.BR1(), operationType);
                                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationnumber);
                                exception.AddEvprmDescription(evprmAuthorisationLocation, "eunumber", null);
                                validationExceptions.Add(exception);
                                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                            }
                        }
                        else if (dbAuthorisationProcedureType.name.Trim().ToLower() == "decentralised")
                        {
                            if (!string.IsNullOrWhiteSpace(dbProduct.product_number))
                            {
                                if (dbProduct.product_number.Trim().Length <= 50)
                                {
                                    evprmAuthorisation.mrpnumber = dbProduct.product_number.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                    validationExceptions.Add(exception);
                                    prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                                }
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.BR1(XevprmValidationRules.AP.RegistrationProcedure.Decentralised), operationType);
                                exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                exception.AddEvprmDescription(evprmAuthorisationLocation, "mrpnumber", null);
                                validationExceptions.Add(exception);
                                prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                            }
                        }
                        else if (dbAuthorisationProcedureType.name.Trim().ToLower().Contains("mutual"))
                        {
                            if (!string.IsNullOrWhiteSpace(dbProduct.product_number))
                            {
                                if (dbProduct.product_number.Trim().Length <= 50)
                                {
                                    evprmAuthorisation.mrpnumber = dbProduct.product_number.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                    validationExceptions.Add(exception);
                                    prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                                }
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.BR1(XevprmValidationRules.AP.RegistrationProcedure.Mutual), operationType);
                                exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                exception.AddEvprmDescription(evprmAuthorisationLocation, "mrpnumber", null);
                                validationExceptions.Add(exception);
                                prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                            }
                        }
                        else if (dbAuthorisationProcedureType.name.Trim().ToLower().Contains("national"))
                        {
                            if (!string.IsNullOrWhiteSpace(dbProduct.product_number))
                            {
                                if (dbProduct.product_number.Trim().Length <= 50)
                                {
                                    evprmAuthorisation.mrpnumber = dbProduct.product_number.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.mrpnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.product_number);
                                    validationExceptions.Add(exception);
                                    prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                                }
                            }
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exception(string.Format("Authorisation procedure with Id = '{0}' is missing name.", dbAuthorisationProcedureType.type_PK)));
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationprocedure.Cardinality(), operationType);
                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.authorisation_procedure);
                    exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationprocedure", null);
                    validationExceptions.Add(exception);
                    prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }

                if (dbProduct.orphan_drug != null)
                {
                    if (dbProduct.orphan_drug == true)
                    {
                        evprmAuthorisation.orphandrug = 1;
                    }
                    else if (dbProduct.orphan_drug == false)
                    {
                        evprmAuthorisation.orphandrug = 2;
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.orphandrug.BR1(), operationType);
                    exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.orphan_drug);
                    exception.AddEvprmDescription(evprmAuthorisationLocation, "orphandrug", null);
                    validationExceptions.Add(exception);
                    prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }

                if (dbProduct.intensive_monitoring.HasValue)
                {
                    if (dbProduct.intensive_monitoring.Value.In(1, 2))
                    {
                        evprmAuthorisation.intensivemonitoring = dbProduct.intensive_monitoring;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.intensivemonitoring.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyProdNavigateUrl, () => dbProduct.product_PK, () => dbProduct.intensive_monitoring);
                        validationExceptions.Add(exception);
                        prodValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
            }

            //Authorisation status
            Type_PK dbAuthorisationStatusType = dbAuthorisedProduct.authorisationstatus_FK.HasValue ? typeOperations.GetEntity(dbAuthorisedProduct.authorisationstatus_FK) : null;
            if (dbAuthorisationStatusType != null)
            {
                int authorisationStatus;
                if (int.TryParse(dbAuthorisationStatusType.ev_code, out authorisationStatus))
                {
                    if (authorisationStatus >= 0 && authorisationStatus < 100)
                    {
                        evprmAuthorisation.authorisationstatus = authorisationStatus;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationstatus.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisationStatusType.type_PK, () => dbAuthorisationStatusType.ev_code);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationstatus.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisationStatusType.type_PK, () => dbAuthorisationStatusType.ev_code);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }

                if (!string.IsNullOrWhiteSpace(dbAuthorisationStatusType.name))
                {
                    if (dbAuthorisationStatusType.name.Trim().ToLower() == "valid")
                    {
                        if (dbAuthorisedProduct.authorisationwithdrawndate.HasValue)
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.withdrawndate.BR6(), operationType);
                            exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationwithdrawndate);
                            exception.AddEvprmDescription(evprmAuthorisationLocation, "withdrawndate", dbAuthorisedProduct.authorisationwithdrawndate.Value.ToString("yyyyMMdd"));
                            validationExceptions.Add(exception);
                            authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        if (!dbAuthorisedProduct.authorisationwithdrawndate.HasValue)
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.withdrawndate.BR5(), operationType);
                            exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationwithdrawndate);
                            exception.AddEvprmDescription(evprmAuthorisationLocation, "withdrawndate", null);
                            validationExceptions.Add(exception);
                            authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }

                    if (dbAuthorisationStatusType.name.Trim().ToLower() == "valid" || dbAuthorisationStatusType.name.Trim().ToLower() == "suspended")
                    {
                        if (operationType == XevprmOperationType.Withdraw)
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationstatus.BR5(), operationType);
                            exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationstatus_FK.Value);
                            exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationstatus", dbAuthorisationStatusType.ev_code);
                            validationExceptions.Add(exception);
                            authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        if (operationType != XevprmOperationType.Withdraw)
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationstatus.BR3(), operationType);
                            exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationstatus_FK.Value);
                            exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationstatus", dbAuthorisationStatusType.ev_code);
                            validationExceptions.Add(exception);
                            authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }
                }
                else
                {
                    exceptions.Add(new Exception(string.Format("Authorisation status with Id = '{0}' is missing name.", dbAuthorisationStatusType.type_PK)));
                }
            }
            else if (operationType.NotIn(XevprmOperationType.Nullify))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationstatus.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationstatus_FK);
                exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationstatus", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            if (dbAuthorisedProduct.authorisationdate.HasValue)
            {
                if (dbAuthorisedProduct.authorisationdate.Value < DateTime.UtcNow.AddHours(12))
                {
                    evprmAuthorisation.authorisationdate = dbAuthorisedProduct.authorisationdate.Value.ToString("yyyyMMdd");
                    evprmAuthorisation.authorisationdateformat = "102";
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationdate.BR5(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationdate.Value);
                    exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationdate", dbAuthorisedProduct.authorisationdate.Value.ToString("yyyyMMdd"));
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation, XevprmOperationType.Withdraw))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.authorisationdate.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationdate);
                exception.AddEvprmDescription(evprmAuthorisationLocation, "authorisationdate", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            if (operationType == XevprmOperationType.Withdraw)
            {
                if (!dbAuthorisedProduct.authorisationwithdrawndate.HasValue)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.withdrawndate.BR1(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationwithdrawndate);
                    exception.AddEvprmDescription(evprmAuthorisationLocation, "withdrawndate", null);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }

            if (dbAuthorisedProduct.authorisationwithdrawndate.HasValue)
            {
                if (dbAuthorisedProduct.authorisationwithdrawndate.Value < DateTime.UtcNow.AddHours(12))
                {
                    evprmAuthorisation.withdrawndate = dbAuthorisedProduct.authorisationwithdrawndate.Value.ToString("yyyyMMdd");
                    evprmAuthorisation.withdrawndateformat = "102";
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.authorisation.withdrawndate.BR3(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateUrl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.authorisationwithdrawndate);
                    exception.AddEvprmDescription(evprmAuthorisationLocation, "withdrawndate", dbAuthorisedProduct.authorisationwithdrawndate.Value.ToString("yyyyMMdd"));
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }

            authProdValidationExceptionTree.Children.Add(prodValidationExceptionTree);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref authProdValidationExceptionTree);
        }
    }
}
