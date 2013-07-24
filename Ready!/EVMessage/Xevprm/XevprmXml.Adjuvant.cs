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
        public static bool AreAdjuvantsValid(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            return ValidateAdjuvants(dbPharmaceuticalProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateAdjuvants(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.adjuvantsLocalType evprmAdjuvants;
            return ConstructAdjuvants(dbPharmaceuticalProduct, operationType, out evprmAdjuvants);
        }

        public static bool IsAdjuvantValid(Adjuvant_PK dbAdjuvant, XevprmOperationType operationType)
        {
            return ValidateAdjuvant(dbAdjuvant, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateAdjuvant(Adjuvant_PK dbAdjuvant, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType evprmAdjuvant;
            return ConstructAdjuvant(dbAdjuvant, operationType, out evprmAdjuvant);
        }

        private static ValidationResult ConstructAdjuvants(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType, out pharmaceuticalproductType.adjuvantsLocalType evprmAdjuvants, string evprmPPLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmAdjuvants = null;

            if (dbPharmaceuticalProduct == null)
            {
                const string message = "ConstructAdjuvants: Pharmaceutical product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbPharmaceuticalProduct;

            IAdjuvant_PKOperations adjuvantOperations = new Adjuvant_PKDAL();

            string evprmAdjuvantsLocation = string.Format("{0}.adjuvants", evprmPPLocation);

            List<Adjuvant_PK> dbAdjuvantList = adjuvantOperations.GetAdjuvantsByPPPK(dbPharmaceuticalProduct.pharmaceutical_product_PK);

            if (dbAdjuvantList != null && dbAdjuvantList.Count > 0)
            {
                var evprmAdjuvantList = new List<pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType>();

                int adjuvantIndex = 0;

                foreach (Adjuvant_PK dbAdjuvant in dbAdjuvantList)
                {
                    string evprmAdjuvantLocation = string.Format("{0}.adjuvant[{1}]", evprmAdjuvantsLocation, adjuvantIndex);

                    pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType evprmAdjuvant;

                    ValidationResult operationResult = ConstructAdjuvant(dbAdjuvant, operationType, out evprmAdjuvant, evprmAdjuvantLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                    if (evprmAdjuvant != null)
                    {
                        string readyPharProdNavigateUrl = NavigateUrl.PharmaceuticalProduct(dbAdjuvant.pp_FK, operationType);

                        if (evprmAdjuvant.substancecode != null && !string.IsNullOrWhiteSpace(evprmAdjuvant.substancecode.TypedValue))
                        {
                            if (evprmAdjuvantList.Any(adjuvant => adjuvant.substancecode != null && adjuvant.substancecode.TypedValue == evprmAdjuvant.substancecode.TypedValue))
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.BR1(), operationType);
                                Adjuvant_PK adjuvant = dbAdjuvant;
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => adjuvant.adjuvant_PK, () => adjuvant.substancecode_FK);
                                exception.AddEvprmDescription(evprmAdjuvantLocation, "substancecode", evprmAdjuvant.substancecode.TypedValue);
                                validationExceptions.Add(exception);
                            }
                        }

                        evprmAdjuvantList.Add(evprmAdjuvant);

                        adjuvantIndex++;
                    }
                }

                if (evprmAdjuvantList.Count > 0)
                {
                    evprmAdjuvants = new pharmaceuticalproductType.adjuvantsLocalType();
                    evprmAdjuvants.adjuvant = evprmAdjuvantList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmAdjuvantList.Count != dbAdjuvantList.Count)
                {
                    exceptions.Add(new Exception("Adjuvants: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructAdjuvant(Adjuvant_PK dbAdjuvant, XevprmOperationType operationType, out pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType evprmAdjuvant, string evprmAdjuvantLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmAdjuvant = null;

            if (dbAdjuvant == null)
            {
                const string message = "ConstructAdjuvant: Adjuvant is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAdjuvant;

            evprmAdjuvant = new pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType();

            ISubstance_PKOperations substanceOperations = new Substance_PKDAL();
            ISsi__cont_voc_PKOperations ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            string readyPharProdNavigateUrl = NavigateUrl.PharmaceuticalProduct(dbAdjuvant.pp_FK, operationType);

            #region Substance

            Substance_PK dbSubstance = dbAdjuvant.substancecode_FK.HasValue ? substanceOperations.GetEntity(dbAdjuvant.substancecode_FK) : null;
            if (dbSubstance != null)
            {
                if (!string.IsNullOrWhiteSpace(dbSubstance.ev_code))
                {
                    if (dbSubstance.ev_code.Trim().Length <= 60)
                    {
                        evprmAdjuvant.substancecode = new pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType.substancecodeLocalType();
                        evprmAdjuvant.substancecode.resolutionmode = 2;
                        evprmAdjuvant.substancecode.TypedValue = dbSubstance.ev_code;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                        validationExceptions.Add(exception); ;
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                    validationExceptions.Add(exception); ;
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.substancecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.substancecode_FK);
                exception.AddEvprmDescription(evprmAdjuvantLocation, "substancecode", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Concentration type

            Ssi__cont_voc_PK dbConcetrationType = dbAdjuvant.concentrationtypecode.HasValue ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.concentrationtypecode) : null;
            if (dbConcetrationType != null)
            {
                int evcode;
                if (!string.IsNullOrWhiteSpace(dbConcetrationType.Evcode) && int.TryParse(dbConcetrationType.Evcode.Trim(), out evcode))
                {
                    evprmAdjuvant.concentrationtypecode = dbConcetrationType.Evcode.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.concentrationtypecode.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbConcetrationType.ssi__cont_voc_PK, () => dbConcetrationType.Evcode);
                    validationExceptions.Add(exception);

                    validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                    return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, validationExceptionTree);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.concentrationtypecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.concentrationtypecode);
                exception.AddEvprmDescription(evprmAdjuvantLocation, "concentrationtypecode", null);
                validationExceptions.Add(exception);

                validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, validationExceptionTree);
            }

            #endregion

            #region Low amount numerator value

            if (dbAdjuvant.lowamountnumervalue.HasValue)
            {
                evprmAdjuvant.lowamountnumervalue = dbAdjuvant.lowamountnumervalue.Value;
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumervalue.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountnumervalue);
                exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountnumervalue", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Low amount numerator prefix

            {
                Ssi__cont_voc_PK dbPrefix = IsValidInt(dbAdjuvant.lowamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.lowamountnumerprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            evprmAdjuvant.lowamountnumerprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerprefix.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountnumerprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount numerator unit

            bool isLowAmountNumerUnitValid = false;

            {
                Ssi__cont_voc_PK dbUnit = IsValidInt(dbAdjuvant.lowamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.lowamountnumerunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            isLowAmountNumerUnitValid = true;
                            evprmAdjuvant.lowamountnumerunit = dbUnit.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountnumerunit.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountnumerunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount denominator value

            if (dbAdjuvant.lowamountdenomvalue.HasValue)
            {
                evprmAdjuvant.lowamountdenomvalue = dbAdjuvant.lowamountdenomvalue.Value;
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomvalue.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountdenomvalue);
                exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountdenomvalue", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Low amount denominator prefix

            bool isLowAmountDenomPrefixValid = false;

            {
                var dbPrefix = IsValidInt(dbAdjuvant.lowamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.lowamountdenomprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            isLowAmountDenomPrefixValid = true;
                            evprmAdjuvant.lowamountdenomprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomprefix.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountdenomprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount denominator unit

            bool isLowAmountDenomUnitValid = false;

            {
                var dbUnit = IsValidInt(dbAdjuvant.lowamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.lowamountdenomunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            isLowAmountDenomUnitValid = true;
                            evprmAdjuvant.lowamountdenomunit = dbUnit.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.lowamountdenomunit.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.lowamountdenomunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "lowamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator value

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                if (dbAdjuvant.highamountnumervalue.HasValue)
                {
                    evprmAdjuvant.highamountnumervalue = dbAdjuvant.highamountnumervalue.Value;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumervalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountnumervalue);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumervalue", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (dbAdjuvant.highamountnumervalue.HasValue)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumervalue.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountnumervalue);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumervalue", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator prefix

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbPrefix = IsValidInt(dbAdjuvant.highamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.highamountnumerprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            evprmAdjuvant.highamountnumerprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR3(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountnumerprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbAdjuvant.highamountnumerprefix))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerprefix.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountnumerprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator unit

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbUnit = IsValidInt(dbAdjuvant.higamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.higamountnumerunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            if (isLowAmountNumerUnitValid && dbAdjuvant.higamountnumerunit == dbAdjuvant.lowamountnumerunit)
                            {
                                evprmAdjuvant.highamountnumerunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerunit.BR3(), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.higamountnumerunit);
                                exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumerunit", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerunit.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerunit.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.higamountnumerunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbAdjuvant.higamountnumerunit))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountnumerunit.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.higamountnumerunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator value

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                if (dbAdjuvant.highamountdenomvalue.HasValue)
                {
                    if (dbAdjuvant.highamountdenomvalue == dbAdjuvant.lowamountdenomvalue)
                    {
                        evprmAdjuvant.highamountdenomvalue = dbAdjuvant.highamountdenomvalue.Value;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR3(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomvalue);
                        exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomvalue", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomvalue);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomvalue", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (dbAdjuvant.highamountdenomvalue.HasValue)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomvalue.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomvalue);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomvalue", dbAdjuvant.highamountdenomvalue.ToString());
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator prefix

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbPrefix = IsValidInt(dbAdjuvant.highamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.highamountdenomprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            if (isLowAmountDenomPrefixValid && dbAdjuvant.highamountdenomprefix == dbAdjuvant.lowamountdenomprefix)
                            {
                                evprmAdjuvant.highamountdenomprefix = dbPrefix.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR3(), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomprefix);
                                exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomprefix", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomprefix.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbAdjuvant.highamountdenomprefix))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomprefix.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomprefix);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator unit

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbUnit = IsValidInt(dbAdjuvant.highamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbAdjuvant.highamountdenomunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            if (isLowAmountDenomUnitValid && dbAdjuvant.highamountdenomunit == dbAdjuvant.lowamountdenomunit)
                            {
                                evprmAdjuvant.highamountdenomunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomunit.BR3(), operationType);
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomunit);
                                exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomunit", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomunit.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomunit.BR1(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbAdjuvant.highamountdenomunit))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ADJ.highamountdenomunit.BR2(), operationType);
                    exception.AddReadyDescription(readyPharProdNavigateUrl, () => dbAdjuvant.adjuvant_PK, () => dbAdjuvant.highamountdenomunit);
                    exception.AddEvprmDescription(evprmAdjuvantLocation, "highamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
