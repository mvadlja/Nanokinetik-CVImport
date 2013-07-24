using System;
using System.Collections.Generic;
using System.Linq;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool AreActiveIngredientsValid(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            return ValidateActiveIngredients(dbPharmaceuticalProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateActiveIngredients(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.activeingredientsLocalType evprmActiveIngredients;
            return ConstructActiveIngredients(dbPharmaceuticalProduct, operationType, out evprmActiveIngredients);
        }

        public static bool IsActiveIngredientValid(Activeingredient_PK dbActiveIngredient, XevprmOperationType operationType)
        {
            return ValidateActiveIngredient(dbActiveIngredient, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateActiveIngredient(Activeingredient_PK dbActiveIngredient, XevprmOperationType operationType)
        {
            pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType evprmActiveIngredient;
            return ConstructActiveIngredient(dbActiveIngredient, operationType, out evprmActiveIngredient);
        }

        private static ValidationResult ConstructActiveIngredients(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType, out pharmaceuticalproductType.activeingredientsLocalType evprmActiveIngredients, string evprmPPLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmActiveIngredients = null;

            if (dbPharmaceuticalProduct == null)
            {
                const string message = "ConstructActiveIngredients: Pharmaceutical product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbPharmaceuticalProduct;

            IActiveingredient_PKOperations activeIngredientOperations = new Activeingredient_PKDAL();

            string evprmActiveIngredientsLocation = string.Format("{0}.activeingredients", evprmPPLocation);

            var dbActiveIngredientList = activeIngredientOperations.GetIngredientsByPPPK(dbPharmaceuticalProduct.pharmaceutical_product_PK);

            string readyPharProdNavigateUrl = NavigateUrl.PharmaceuticalProduct(dbPharmaceuticalProduct.pharmaceutical_product_PK, operationType);

            if (dbActiveIngredientList != null && dbActiveIngredientList.Count > 0)
            {
                var evprmActiveIngredientList = new List<pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType>();

                int activeIngredientIndex = 0;

                foreach (Activeingredient_PK dbActiveIngredient in dbActiveIngredientList)
                {
                    string evprmActiveIngredientLocation = string.Format("{0}.activeingredient[{1}]", evprmActiveIngredientsLocation, activeIngredientIndex);

                    pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType evprmActiveIngredient;

                    var operationResult = ConstructActiveIngredient(dbActiveIngredient, operationType, out evprmActiveIngredient, evprmActiveIngredientsLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                    if (evprmActiveIngredient != null)
                    {
                        if (evprmActiveIngredient.substancecode != null && !string.IsNullOrWhiteSpace(evprmActiveIngredient.substancecode.TypedValue))
                        {
                            if (evprmActiveIngredientList.Any(activeIng => activeIng.substancecode != null && activeIng.substancecode.TypedValue == evprmActiveIngredient.substancecode.TypedValue))
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.BR1(), operationType);
                                Activeingredient_PK ingredient = dbActiveIngredient;
                                exception.AddReadyDescription(readyPharProdNavigateUrl, () => ingredient.activeingredient_PK, () => ingredient.substancecode_FK);
                                exception.AddEvprmDescription(evprmActiveIngredientLocation, "substancecode", evprmActiveIngredient.substancecode.TypedValue);
                                validationExceptions.Add(exception);
                            }
                        }

                        evprmActiveIngredientList.Add(evprmActiveIngredient);

                        activeIngredientIndex++;
                    }
                }

                if (evprmActiveIngredientList.Count > 0)
                {
                    evprmActiveIngredients = new pharmaceuticalproductType.activeingredientsLocalType();
                    evprmActiveIngredients.activeingredient = evprmActiveIngredientList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmActiveIngredientList.Count != dbActiveIngredientList.Count)
                {
                    exceptions.Add(new Exception("Active ingredients: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.Cardinality(), operationType);
                exception.AddReadyDescription(readyPharProdNavigateUrl, typeof(Activeingredient_PK), null, "activeingredient_PK", null);
                exception.AddEvprmDescription(evprmActiveIngredientsLocation, "activeingredient", null);
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructActiveIngredient(Activeingredient_PK dbActiveIngredient, XevprmOperationType operationType, out pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType evprmActiveIngredient, string evprmActiveIngredientLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmActiveIngredient = null;

            if (dbActiveIngredient == null)
            {
                const string message = "ConstructActiveIngredient: Active ingredient is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbActiveIngredient;

            evprmActiveIngredient = new pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType();

            ISubstance_PKOperations substanceOperations = new Substance_PKDAL();
            ISsi__cont_voc_PKOperations ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            string readyPPUrl = NavigateUrl.PharmaceuticalProduct(dbActiveIngredient.pp_FK, operationType);

            #region Substance

            var dbSubstance = dbActiveIngredient.substancecode_FK.HasValue ? substanceOperations.GetEntity(dbActiveIngredient.substancecode_FK) : null;
            if (dbSubstance != null)
            {
                if (!string.IsNullOrWhiteSpace(dbSubstance.ev_code))
                {
                    if (dbSubstance.ev_code.Trim().Length <= 60)
                    {
                        evprmActiveIngredient.substancecode = new pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType.substancecodeLocalType { resolutionmode = 2, TypedValue = dbSubstance.ev_code };
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.substancecode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbSubstance.substance_PK, () => dbSubstance.ev_code);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.substancecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.substancecode_FK);
                exception.AddEvprmDescription(evprmActiveIngredientLocation, "substancecode", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Concentration type

            var dbConcetrationType = dbActiveIngredient.concentrationtypecode.HasValue ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.concentrationtypecode) : null;
            if (dbConcetrationType != null)
            {
                int evcode;
                if (!string.IsNullOrWhiteSpace(dbConcetrationType.Evcode) && int.TryParse(dbConcetrationType.Evcode.Trim(), out evcode))
                {
                    evprmActiveIngredient.concentrationtypecode = dbConcetrationType.Evcode.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.concentrationtypecode.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbConcetrationType.ssi__cont_voc_PK, () => dbConcetrationType.Evcode);
                    validationExceptions.Add(exception);

                    validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                    return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, validationExceptionTree);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.concentrationtypecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.concentrationtypecode);
                exception.AddEvprmDescription(evprmActiveIngredientLocation, "concentrationtypecode", null);
                validationExceptions.Add(exception);

                validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, validationExceptionTree);
            }

            #endregion

            #region Low amount numerator value

            if (dbActiveIngredient.lowamountnumervalue.HasValue)
            {
                evprmActiveIngredient.lowamountnumervalue = dbActiveIngredient.lowamountnumervalue.Value;
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumervalue.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountnumervalue);
                exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountnumervalue", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Low amount numerator prefix

            {
                var dbPrefix = IsValidInt(dbActiveIngredient.lowamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.lowamountnumerprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            evprmActiveIngredient.lowamountnumerprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerprefix.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountnumerprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount numerator unit

            bool isLowAmountNumerUnitValid = false;

            {
                var dbUnit = IsValidInt(dbActiveIngredient.lowamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.lowamountnumerunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            isLowAmountNumerUnitValid = true;
                            evprmActiveIngredient.lowamountnumerunit = dbUnit.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountnumerunit.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountnumerunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount denominator value

            if (dbActiveIngredient.lowamountdenomvalue.HasValue)
            {
                evprmActiveIngredient.lowamountdenomvalue = dbActiveIngredient.lowamountdenomvalue.Value;
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomvalue.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountdenomvalue);
                exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountdenomvalue", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Low amount denominator prefix

            bool isLowAmountDenomPrefixValid = false;

            {
                var dbPrefix = IsValidInt(dbActiveIngredient.lowamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.lowamountdenomprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            isLowAmountDenomPrefixValid = true;
                            evprmActiveIngredient.lowamountdenomprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomprefix.BR1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomprefix.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountdenomprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region Low amount denominator unit

            bool isLowAmountDenomUnitValid = false;

            {
                var dbUnit = IsValidInt(dbActiveIngredient.lowamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.lowamountdenomunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            isLowAmountDenomUnitValid = true;
                            evprmActiveIngredient.lowamountdenomunit = dbUnit.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomunit.BR1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.lowamountdenomunit.Cardinality(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.lowamountdenomunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "lowamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator value

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                if (dbActiveIngredient.highamountnumervalue.HasValue)
                {
                    evprmActiveIngredient.highamountnumervalue = dbActiveIngredient.highamountnumervalue.Value;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumervalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumervalue);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumervalue", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (dbActiveIngredient.highamountnumervalue.HasValue)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumervalue.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumervalue);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumervalue", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator prefix

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbPrefix = IsValidInt(dbActiveIngredient.highamountnumerprefix) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.highamountnumerprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            evprmActiveIngredient.highamountnumerprefix = dbPrefix.term_name_english.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerprefix.BR3(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerprefix.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumerprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbActiveIngredient.highamountnumerprefix))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerprefix.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumerprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumerprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount numerator unit

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbUnit = IsValidInt(dbActiveIngredient.highamountnumerunit) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.highamountnumerunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            if (isLowAmountNumerUnitValid && dbActiveIngredient.highamountnumerunit == dbActiveIngredient.lowamountnumerunit)
                            {
                                evprmActiveIngredient.highamountnumerunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerunit.BR3(), operationType);
                                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumerunit);
                                exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumerunit", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerunit.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerunit.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumerunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbActiveIngredient.highamountnumerunit))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountnumerunit.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountnumerunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountnumerunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator value

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                if (dbActiveIngredient.highamountdenomvalue.HasValue)
                {
                    if (dbActiveIngredient.highamountdenomvalue == dbActiveIngredient.lowamountdenomvalue)
                    {
                        evprmActiveIngredient.highamountdenomvalue = dbActiveIngredient.highamountdenomvalue.Value;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomvalue.BR3(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomvalue);
                        exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomvalue", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomvalue.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomvalue);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomvalue", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (dbActiveIngredient.highamountdenomvalue.HasValue)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomvalue.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomvalue);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomvalue", dbActiveIngredient.highamountdenomvalue.ToString());
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator prefix

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbPrefix = IsValidInt(dbActiveIngredient.highamountdenomprefix) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.highamountdenomprefix) : null;
                if (dbPrefix != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbPrefix.term_name_english))
                    {
                        if (dbPrefix.term_name_english.Trim().Length <= 12)
                        {
                            if (isLowAmountDenomPrefixValid && dbActiveIngredient.highamountdenomprefix == dbActiveIngredient.lowamountdenomprefix)
                            {
                                evprmActiveIngredient.highamountdenomprefix = dbPrefix.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomprefix.BR3(), operationType);
                                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomprefix);
                                exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomprefix", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomprefix.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomprefix.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbPrefix.ssi__cont_voc_PK, () => dbPrefix.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomprefix.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbActiveIngredient.highamountdenomprefix))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomprefix.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomprefix);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomprefix", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            #region High amount denominator unit

            //If concentration type is 'range'
            if (dbConcetrationType != null && dbConcetrationType.Evcode != null && dbConcetrationType.Evcode.Trim() == "2")
            {
                var dbUnit = IsValidInt(dbActiveIngredient.highamountdenomunit) ? ssiControlledVocabularyOperations.GetEntity(dbActiveIngredient.highamountdenomunit) : null;
                if (dbUnit != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbUnit.term_name_english))
                    {
                        if (dbUnit.term_name_english.Trim().Length <= 70)
                        {
                            if (isLowAmountDenomUnitValid && dbActiveIngredient.highamountdenomunit == dbActiveIngredient.lowamountdenomunit)
                            {
                                evprmActiveIngredient.highamountdenomunit = dbUnit.term_name_english.Trim();
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomunit.BR3(), operationType);
                                exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomunit);
                                exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomunit", null);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomunit.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomunit.BRCustom1(), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbUnit.ssi__cont_voc_PK, () => dbUnit.term_name_english);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomunit.BR1(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                if (IsValidInt(dbActiveIngredient.highamountdenomunit))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.ACT.highamountdenomunit.BR2(), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbActiveIngredient.activeingredient_PK, () => dbActiveIngredient.highamountdenomunit);
                    exception.AddEvprmDescription(evprmActiveIngredientLocation, "highamountdenomunit", null);
                    validationExceptions.Add(exception);
                }
            }

            #endregion

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
