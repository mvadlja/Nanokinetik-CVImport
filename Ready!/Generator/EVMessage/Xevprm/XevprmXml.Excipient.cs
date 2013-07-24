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
        public static bool AreExcipientsValid(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            return ValidateExcipients(dbPharmaceuticalProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateExcipients(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.excipientsLocalType evprmExcipients;
            return ConstructExcipients(dbPharmaceuticalProduct, operationType, out evprmExcipients);
        }

        public static bool IsExcipientValid(Excipient_PK dbExcipient, XevprmOperationType operationType)
        {
            return ValidateExcipient(dbExcipient, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateExcipient(Excipient_PK dbExcipient, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.excipientsLocalType.excipientLocalType evprmExcipient;
            return ConstructExcipient(dbExcipient, operationType, out evprmExcipient);
        }

        private static ValidationResult ConstructExcipients(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType, out pharmaceuticalproductType.excipientsLocalType evprmExcipients, string evprmPPLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmExcipients = null;

            if (dbPharmaceuticalProduct == null)
            {
                const string message = "ConstructExcipients: Pharmaceutical product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbPharmaceuticalProduct;

            IExcipient_PKOperations excipientOperations = new Excipient_PKDAL();

            string evprmExcipientsLocation = string.Format("{0}.excipients", evprmPPLocation);

            var dbExcipientList = excipientOperations.GetExcipientsByPPPK(dbPharmaceuticalProduct.pharmaceutical_product_PK);

            if (dbExcipientList != null && dbExcipientList.Count > 0)
            {
                var evprmExcipientList = new List<pharmaceuticalproductType.excipientsLocalType.excipientLocalType>();

                int excipientIndex = 0;

                foreach (Excipient_PK dbExcipient in dbExcipientList)
                {
                    string evprmExcipientLocation = string.Format("{0}.excipient[{1}]", evprmExcipientsLocation, excipientIndex);

                    pharmaceuticalproductType.excipientsLocalType.excipientLocalType evprmExcipient;

                    ValidationResult operationResult = ConstructExcipient(dbExcipient, operationType, out evprmExcipient, evprmExcipientLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                    if (evprmExcipient != null)
                    {
                        string readyPharProdNavigateUrl = NavigateUrl.PharmaceuticalProduct(dbExcipient.pp_FK, operationType);

                        if (evprmExcipient.substancecode != null && !string.IsNullOrWhiteSpace(evprmExcipient.substancecode.TypedValue))
                        {
                            if (evprmExcipientList.Any(excipient => excipient.substancecode != null && excipient.substancecode.TypedValue == evprmExcipient.substancecode.TypedValue))
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.BR1(), operationType);
                                Excipient_PK excipient = dbExcipient;
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => excipient.excipient_PK, () => excipient.substancecode_FK);
                                exception.AddEvprmDescription(evprmExcipientLocation, "substancecode", evprmExcipient.substancecode.TypedValue);
                                validationExceptions.Add(exception);
                            }
                        }

                        evprmExcipientList.Add(evprmExcipient);

                        excipientIndex++;
                    }
                }

                if (evprmExcipientList.Count > 0)
                {
                    evprmExcipients = new pharmaceuticalproductType.excipientsLocalType();
                    evprmExcipients.excipient = evprmExcipientList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmExcipientList.Count != dbExcipientList.Count)
                {
                    exceptions.Add(new Exception("Excipients: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructExcipient(Excipient_PK dbExcipient, XevprmOperationType operationType, out pharmaceuticalproductType.excipientsLocalType.excipientLocalType evprmExcipient, string evprmExcipientLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmExcipient = null;

            if (dbExcipient == null)
            {
                const string message = "ConstructExcipient: Excipient is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbExcipient;

            evprmExcipient = new pharmaceuticalproductType.excipientsLocalType.excipientLocalType();

            ISubstance_PKOperations substanceOperations = new Substance_PKDAL();
            ISsi__cont_voc_PKOperations ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            string readyPharProdNavigateUrl = NavigateUrl.PharmaceuticalProduct(dbExcipient.pp_FK, operationType);

            #region Substance

            var dbSubstance = dbExcipient.substancecode_FK.HasValue ? substanceOperations.GetEntity(dbExcipient.substancecode_FK) : null;
            if (dbSubstance != null)
            {
                if (!string.IsNullOrWhiteSpace(dbSubstance.ev_code))
                {
                    if (dbSubstance.ev_code.Trim().Length <= 60)
                    {
                        evprmExcipient.substancecode = new pharmaceuticalproductType.excipientsLocalType.excipientLocalType.substancecodeLocalType();
                        evprmExcipient.substancecode.resolutionmode = 2;
                        evprmExcipient.substancecode.TypedValue = dbSubstance.ev_code;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.substancecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.substancecode_FK);
                exception.AddEvprmDescription(evprmExcipientLocation, "substancecode", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Concentration type

            bool excipientHasConcentrationType = false;

            var dbConcetrationType = dbExcipient.concentrationtypecode.HasValue ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.concentrationtypecode) : null;
            if (dbConcetrationType != null)
            {
                int evcode;
                if (!string.IsNullOrWhiteSpace(dbConcetrationType.Evcode) && int.TryParse(dbConcetrationType.Evcode.Trim(), out evcode))
                {
                    evprmExcipient.concentrationtypecode = dbConcetrationType.Evcode.Trim();

                    excipientHasConcentrationType = true;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.concentrationtypecode.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbConcetrationType.ssi__cont_voc_PK, () => dbConcetrationType.Evcode);
                    validationExceptions.Add(exception);

                    validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                    return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, validationExceptionTree);
                }
            }

            #endregion

            if (excipientHasConcentrationType)
            {

                #region Low amount numerator value

                if (dbExcipient.lowamountnumervalue.HasValue)
                {
                    evprmExcipient.lowamountnumervalue = dbExcipient.lowamountnumervalue.Value;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumervalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountnumervalue);
                    exception.AddEvprmDescription(evprmExcipientLocation, "lowamountnumervalue", null);
                    validationExceptions.Add(exception);
                }

                #endregion

                #region Low amount numerator prefix

                {
                    var dbPrefix = IsValidInt(dbExcipient.lowamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.lowamountnumerprefix) : null;
                    if (dbPrefix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                        {
                            if (dbPrefix.term_name_english.Trim().Length <= 12)
                            {
                                evprmExcipient.lowamountnumerprefix = dbPrefix.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerprefix.BR1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerprefix.CustomBR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountnumerprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "lowamountnumerprefix", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region Low amount numerator unit

                bool isLowAmountNumerUnitValid = false;

                {
                    var dbUnit = IsValidInt(dbExcipient.lowamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.lowamountnumerunit) : null;
                    if (dbUnit != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                        {
                            if (dbUnit.term_name_english.Trim().Length <= 70)
                            {
                                isLowAmountNumerUnitValid = true;
                                evprmExcipient.lowamountnumerunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerunit.BR1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountnumerunit.CustomBR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountnumerunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "lowamountnumerunit", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region Low amount denominator value

                if (dbExcipient.lowamountdenomvalue.HasValue)
                {
                    evprmExcipient.lowamountdenomvalue = dbExcipient.lowamountdenomvalue.Value;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomvalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountdenomvalue);
                    exception.AddEvprmDescription(evprmExcipientLocation, "lowamountdenomvalue", null);
                    validationExceptions.Add(exception);
                }

                #endregion

                #region Low amount denominator prefix

                bool isLowAmountDenomPrefixValid = false;

                {
                    var dbPrefix = IsValidInt(dbExcipient.lowamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.lowamountdenomprefix) : null;
                    if (dbPrefix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                        {
                            if (dbPrefix.term_name_english.Trim().Length <= 12)
                            {
                                isLowAmountDenomPrefixValid = true;
                                evprmExcipient.lowamountdenomprefix = dbPrefix.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomprefix.BR1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomprefix.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountdenomprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "lowamountdenomprefix", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region Low amount denominator unit

                bool isLowAmountDenomUnitValid = false;

                {
                    var dbUnit = IsValidInt(dbExcipient.lowamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.lowamountdenomunit) : null;
                    if (dbUnit != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                        {
                            if (dbUnit.term_name_english.Trim().Length <= 70)
                            {
                                isLowAmountDenomUnitValid = true;
                                evprmExcipient.lowamountdenomunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomunit.BR1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.lowamountdenomunit.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.lowamountdenomunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "lowamountdenomunit", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount numerator value

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    if (dbExcipient.highamountnumervalue.HasValue)
                    {
                        evprmExcipient.highamountnumervalue = dbExcipient.highamountnumervalue.Value;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumervalue.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountnumervalue);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumervalue", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    if (dbExcipient.highamountnumervalue.HasValue)
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumervalue.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountnumervalue);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumervalue", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount numerator prefix

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    var dbPrefix = IsValidInt(dbExcipient.highamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.highamountnumerprefix) : null;
                    if (dbPrefix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                        {
                            if (dbPrefix.term_name_english.Trim().Length <= 12)
                            {
                                evprmExcipient.highamountnumerprefix = dbPrefix.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerprefix.BR3(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountnumerprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumerprefix", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    if (IsValidInt(dbExcipient.highamountnumerprefix))
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerprefix.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountnumerprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumerprefix", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount numerator unit

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    var dbUnit = IsValidInt(dbExcipient.higamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.higamountnumerunit) : null;
                    if (dbUnit != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                        {
                            if (dbUnit.term_name_english.Trim().Length <= 70)
                            {
                                if (isLowAmountNumerUnitValid && dbExcipient.higamountnumerunit == dbExcipient.lowamountnumerunit)
                                {
                                    evprmExcipient.highamountnumerunit = dbUnit.term_name_english.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerunit.BR3(), operationType);
                                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.higamountnumerunit);
                                    exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumerunit", null);
                                    validationExceptions.Add(exception);
                                }
                                evprmExcipient.highamountnumerunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerunit.BRCustom1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.higamountnumerunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumerunit", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    if (IsValidInt(dbExcipient.higamountnumerunit))
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountnumerunit.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.higamountnumerunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountnumerunit", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount denominator value

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    if (dbExcipient.highamountdenomvalue.HasValue)
                    {
                        if (dbExcipient.highamountdenomvalue == dbExcipient.lowamountdenomvalue)
                        {
                            evprmExcipient.highamountdenomvalue = dbExcipient.highamountdenomvalue.Value;
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomvalue.BR3(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomvalue);
                            exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomvalue", null);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomvalue.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomvalue);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomvalue", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    if (dbExcipient.highamountdenomvalue.HasValue)
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomvalue.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomvalue);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomvalue", dbExcipient.highamountdenomvalue.ToString());
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount denominator prefix

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    var dbPrefix = IsValidInt(dbExcipient.highamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.highamountdenomprefix) : null;
                    if (dbPrefix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                        {
                            if (dbPrefix.term_name_english.Trim().Length <= 12)
                            {
                                if (isLowAmountDenomPrefixValid && dbExcipient.highamountdenomprefix == dbExcipient.lowamountdenomprefix)
                                {
                                    evprmExcipient.highamountdenomprefix = dbPrefix.term_name_english.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomprefix.BR3(), operationType);
                                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomprefix);
                                    exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomprefix", null);
                                    validationExceptions.Add(exception);
                                }
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomprefix.BRCustom1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomprefix", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    if (IsValidInt(dbExcipient.highamountdenomprefix))
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomprefix.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomprefix);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomprefix", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion

                #region High amount denominator unit

                //If concentration type is 'range'
                if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
                {
                    var dbUnit = IsValidInt(dbExcipient.highamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbExcipient.highamountdenomunit) : null;
                    if (dbUnit != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                        {
                            if (dbUnit.term_name_english.Trim().Length <= 70)
                            {
                                if (isLowAmountDenomUnitValid && dbExcipient.highamountdenomunit == dbExcipient.lowamountdenomunit)
                                {
                                    evprmExcipient.highamountdenomunit = dbUnit.term_name_english.Trim();
                                }
                                else
                                {
                                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomunit.BR3(), operationType);
                                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomunit);
                                    exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomunit", null);
                                    validationExceptions.Add(exception);
                                }
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomunit.BRCustom1(), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomunit", null);
                        validationExceptions.Add(exception);
                    }

                }
                else
                {
                    if (IsValidInt(dbExcipient.highamountdenomunit))
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.EXC.highamountdenomunit.BR2(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbExcipient.excipient_PK, () => dbExcipient.highamountdenomunit);
                        exception.AddEvprmDescription(evprmExcipientLocation, "highamountdenomunit", null);
                        validationExceptions.Add(exception);
                    }
                }

                #endregion
            }

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
