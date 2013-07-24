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
        public static bool ArePharmaceuticalProductsValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidatePharmaceuticalProducts(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidatePharmaceuticalProducts(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.pharmaceuticalproductsLocalType evprmPharmaceuticalProducts;
            return ConstructPharmaceuticalProducts(dbAuthorisedProduct, operationType, out evprmPharmaceuticalProducts);
        }

        public static bool IsPharmaceuticalProductValid(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            return ValidatePharmaceuticalProduct(dbPharmaceuticalProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidatePharmaceuticalProduct(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType)
        {
            pharmaceuticalproductType evprmPharmaceuticalProduct;
            return ConstructPharmaceuticalProduct(dbPharmaceuticalProduct, operationType, out evprmPharmaceuticalProduct);
        }

        private static ValidationResult ConstructPharmaceuticalProducts(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType.pharmaceuticalproductsLocalType evprmPharmaceuticalProducts, string evprmAuthProdLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmPharmaceuticalProducts = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructPharmaceuticalProducts: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            IProduct_PKOperations productOperations = new Product_PKDAL();

            var dbProduct = productOperations.GetEntity(dbAuthorisedProduct.product_FK);
            if (dbProduct == null)
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.BRCustom1(), operationType);
                exception.AddReadyDescription(NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType), () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_FK);
                validationExceptions.Add(exception);

                validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;
                validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

                return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
            }

            validationExceptionTree.Value.ReadyEntity = dbProduct;

            IPharmaceutical_product_PKOperations pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();

            string evprmPPsLocation = string.Format("{0}.pharmaceuticalproducts", evprmAuthProdLocation);

            List<Pharmaceutical_product_PK> dbPharmaceuticalProductList = dbProduct.product_PK.HasValue ? pharmaceuticalProductOperations.GetEntitiesByProduct(dbProduct.product_PK.Value) : new List<Pharmaceutical_product_PK>();

            var evprmPharmaceuticalProductList = new List<pharmaceuticalproductType>();

            if (dbPharmaceuticalProductList != null && dbPharmaceuticalProductList.Count > 0)
            {
                int ppIndex = 0;
                foreach (Pharmaceutical_product_PK dbPharmaceuticalProduct in dbPharmaceuticalProductList)
                {
                    var ppValidationExceptionTree = new Tree<XevprmValidationTreeNode>();
                    ppValidationExceptionTree.Value.ReadyEntity = dbPharmaceuticalProduct;

                    string evprmPharmProdLocation = string.Format("{0}.pharmaceuticalproduct[{1}]", evprmPPsLocation, ppIndex);

                    pharmaceuticalproductType evprmPharmaceuticalProduct;
                    var validationResult = ConstructPharmaceuticalProduct(dbPharmaceuticalProduct, operationType, out evprmPharmaceuticalProduct, evprmPharmProdLocation);
                    UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                    ppValidationExceptionTree.Value.XevprmValidationExceptions = validationResult.XevprmValidationExceptions;
                    validationExceptionTree.Children.Add(ppValidationExceptionTree);

                    evprmPharmaceuticalProductList.Add(evprmPharmaceuticalProduct);

                    ppIndex++;
                }

                if (evprmPharmaceuticalProductList.Count > 0)
                {
                    evprmPharmaceuticalProducts = new authorisedproductType.pharmaceuticalproductsLocalType();
                    evprmPharmaceuticalProducts.pharmaceuticalproduct = evprmPharmaceuticalProductList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmPharmaceuticalProductList.Count != dbPharmaceuticalProductList.Count)
                {
                    exceptions.Add(new Exception("Pharmaceutical products: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.PP.Cardinality(), operationType);
                exception.AddReadyDescription(NavigateUrl.Product(dbProduct.product_PK, dbAuthorisedProduct.ap_PK, operationType), typeof(Product_mn_PK), null, "product_mn_PK", null);
                exception.AddEvprmDescription(evprmPPsLocation, "pharmaceuticalproduct", null);
                validationExceptions.Add(exception);
                validationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructPharmaceuticalProduct(Pharmaceutical_product_PK dbPharmaceuticalProduct, XevprmOperationType operationType, out pharmaceuticalproductType evprmPharmaceuticalProduct, string evprmPPLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmPharmaceuticalProduct = null;

            if (dbPharmaceuticalProduct == null)
            {
                const string message = "ConstructPharmaceuticalProduct: Pharmaceutical product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbPharmaceuticalProduct;

            evprmPharmaceuticalProduct = new pharmaceuticalproductType();

            IPharmaceutical_form_PKOperations pharmaceuticalFormOperations = new Pharmaceutical_form_PKDAL();
            IPp_ar_mn_PKOperations pharmaceuticalProductAdministrationRouteMnOperations = new Pp_ar_mn_PKDAL();
            IAdminroute_PKOperations administrationRouteOperations = new Adminroute_PKDAL();
            IPp_md_mn_PKOperations pharmaceuticalProductMedicalDeviceMnOperations = new Pp_md_mn_PKDAL();
            IMedicaldevice_PKOperations medicalDeviceOperations = new Medicaldevice_PKDAL();

            string readyPPUrl = NavigateUrl.PharmaceuticalProduct(dbPharmaceuticalProduct.pharmaceutical_product_PK, operationType);

            #region Pharmaceutical form

            //Pharmaceutical form
            //Note: Insert for pharmaceutical form is not supported
            Pharmaceutical_form_PK dbPharmaceuticalForm = dbPharmaceuticalProduct.Pharmform_FK.HasValue ? pharmaceuticalFormOperations.GetEntity(dbPharmaceuticalProduct.Pharmform_FK) : null;
            if (dbPharmaceuticalForm != null)
            {
                if (!string.IsNullOrWhiteSpace(dbPharmaceuticalForm.ev_code))
                {
                    if (dbPharmaceuticalForm.ev_code.Trim().Length <= 60)
                    {
                        evprmPharmaceuticalProduct.pharmformcode = new pharmaceuticalproductType.pharmformcodeLocalType();

                        evprmPharmaceuticalProduct.pharmformcode.TypedValue = dbPharmaceuticalForm.ev_code.Trim();
                        evprmPharmaceuticalProduct.pharmformcode.resolutionmode = 2;
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.pharmformcode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyPPUrl, () => dbPharmaceuticalForm.pharmaceutical_form_PK, () => dbPharmaceuticalForm.ev_code);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.PP.pharmformcode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyPPUrl, () => dbPharmaceuticalForm.pharmaceutical_form_PK, () => dbPharmaceuticalForm.ev_code);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.pharmformcode.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => dbPharmaceuticalProduct.pharmaceutical_product_PK, () => dbPharmaceuticalProduct.Pharmform_FK);
                exception.AddEvprmDescription(evprmPPLocation, "pharmformcode", null);
                validationExceptions.Add(exception);
            }

            #endregion

            #region Administration routes


            string evprmAdminRoutesLocation = string.Format("{0}.adminroutes", evprmPPLocation);

            var dbAdminRoutePpMnList = pharmaceuticalProductAdministrationRouteMnOperations.GetAdminRoutesByPPPK(dbPharmaceuticalProduct.pharmaceutical_product_PK);

            if (dbAdminRoutePpMnList != null && dbAdminRoutePpMnList.Count > 0)
            {
                var evprmAdminRouteList = new List<pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType>();

                int adminRouteIndex = 0;

                foreach (var dbAdminRoutePpMn in dbAdminRoutePpMnList)
                {
                    var dbAdminRoute = dbAdminRoutePpMn.admin_route_FK.HasValue ? administrationRouteOperations.GetEntity(dbAdminRoutePpMn.admin_route_FK) : null;

                    if (dbAdminRoute != null)
                    {
                        string evprmAdminRouteLocation = string.Format("{0}.adminroute[{1}]", evprmPPLocation, adminRouteIndex);

                        if (!string.IsNullOrWhiteSpace(dbAdminRoute.ev_code))
                        {
                            if (dbAdminRoute.ev_code.Trim().Length <= 60)
                            {
                                var evprmAdminRoute = new pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType();

                                evprmAdminRoute.adminroutecode = new pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType.adminroutecodeLocalType();
                                evprmAdminRoute.adminroutecode.TypedValue = dbAdminRoute.ev_code.Trim();
                                evprmAdminRoute.adminroutecode.resolutionmode = 2;

                                if (evprmAdminRoute.adminroutecode != null && !string.IsNullOrWhiteSpace(evprmAdminRoute.adminroutecode.TypedValue))
                                {
                                    if (evprmAdminRouteList.Any(adminRoute => adminRoute.adminroutecode != null && adminRoute.adminroutecode.TypedValue == evprmAdminRoute.adminroutecode.TypedValue))
                                    {
                                        var exception = new XevprmValidationException(new XevprmValidationRules.PP.AR.BR1(), operationType);
                                        exception.AddReadyDescription(readyPPUrl, () => dbAdminRoute.adminroute_PK, () => dbAdminRoute.ev_code);
                                        exception.AddEvprmDescription(evprmAdminRouteLocation, "adminroutecode", dbAdminRoute.ev_code.Trim());
                                        validationExceptions.Add(exception);
                                    }
                                }

                                adminRouteIndex++;

                                evprmAdminRouteList.Add(evprmAdminRoute);
                            }
                            else
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.AR.adminroutecode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                                exception.AddReadyDescription(readyPPUrl, () => dbAdminRoute.adminroute_PK, () => dbAdminRoute.ev_code);
                                validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.AR.adminroutecode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbAdminRoute.adminroute_PK, () => dbAdminRoute.ev_code);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exception(string.Format("Admin route with ID={0} can't be found in database. Pharmaceutical product and Admin route MN relationship is incorrect.", dbAdminRoutePpMn.admin_route_FK)));
                    }

                }

                if (evprmAdminRouteList.Count > 0)
                {
                    evprmPharmaceuticalProduct.adminroutes = new pharmaceuticalproductType.adminroutesLocalType();
                    evprmPharmaceuticalProduct.adminroutes.adminroute = evprmAdminRouteList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmAdminRouteList.Count != dbAdminRoutePpMnList.Count)
                {
                    exceptions.Add(new Exception("Administration routes: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.PP.AR.Cardinality(), operationType);
                exception.AddReadyDescription(readyPPUrl, () => new Pp_ar_mn_PK().pp_ar_mn_PK, () => new Pp_ar_mn_PK().pp_ar_mn_PK);
                exception.AddEvprmDescription(evprmAdminRoutesLocation, "adminroute", null);
                validationExceptions.Add(exception);
            }


            #endregion

            #region Medical devices

            string evprmMedicalDevicesLocation = string.Format("{0}.medicaldevices", evprmPPLocation);

            List<Pp_md_mn_PK> dbMedicalDevicePPMmList = pharmaceuticalProductMedicalDeviceMnOperations.GetMedDevicesByPPPK(dbPharmaceuticalProduct.pharmaceutical_product_PK);

            if (dbMedicalDevicePPMmList != null && dbMedicalDevicePPMmList.Count > 0)
            {
                var evprmMedicalDeviceList = new List<pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType>();

                int medicalDeviceIndex = 0;

                foreach (Pp_md_mn_PK dbMedicalDevicePPMn in dbMedicalDevicePPMmList)
                {
                    Medicaldevice_PK dbMedicalDevice = medicalDeviceOperations.GetEntity(dbMedicalDevicePPMn.pp_medical_device_FK);

                    if (dbMedicalDevice != null)
                    {

                        string evprmMedicalDeviceLocation = string.Format("{0}.medicaldevice[{1}]", evprmMedicalDevicesLocation, medicalDeviceIndex);

                        if (IsValidInt(dbMedicalDevice.ev_code))
                        {
                            var evprmMedicalDevice = new pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType();

                            evprmMedicalDevice.medicaldevicecode = Int32.Parse(dbMedicalDevice.ev_code);


                            if (evprmMedicalDeviceList.Any(medDevice => medDevice.medicaldevicecode == evprmMedicalDevice.medicaldevicecode))
                            {
                                var exception = new XevprmValidationException(new XevprmValidationRules.PP.MD.BR1(), operationType);
                                exception.AddReadyDescription(readyPPUrl, () => dbMedicalDevice.medicaldevice_PK, () => dbMedicalDevice.ev_code);
                                exception.AddEvprmDescription(evprmMedicalDeviceLocation, "medicaldevicecode", dbMedicalDevice.ev_code.Trim());
                                validationExceptions.Add(exception);
                            }

                            evprmMedicalDeviceList.Add(evprmMedicalDevice);

                            medicalDeviceIndex++;
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.PP.MD.medicaldevicecode.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                            exception.AddReadyDescription(readyPPUrl, () => dbMedicalDevice.medicaldevice_PK, () => dbMedicalDevice.ev_code);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        exceptions.Add(new Exception(string.Format("Medical device with ID={0} can't be found in database. Pharmaceutical product and Medical device MN relationship is incorrect.", dbMedicalDevicePPMn.pp_medical_device_FK)));
                    }
                }

                if (evprmMedicalDeviceList.Count > 0)
                {
                    evprmPharmaceuticalProduct.medicaldevices = new pharmaceuticalproductType.medicaldevicesLocalType();
                    evprmPharmaceuticalProduct.medicaldevices.medicaldevice = evprmMedicalDeviceList.ToArray();
                }

                if (validationExceptions.Count == 0 && exceptions.Count == 0 && evprmMedicalDeviceList.Count != dbMedicalDevicePPMmList.Count)
                {
                    exceptions.Add(new Exception("Medical devices: Validation was successful but number of items added to xml is different than actual number of items!"));
                }
            }

            #endregion

            #region Active ingredients

            {
                pharmaceuticalproductType.activeingredientsLocalType evprmActiveIngredients;
                ValidationResult operationResult = ConstructActiveIngredients(dbPharmaceuticalProduct, operationType, out evprmActiveIngredients, evprmPPLocation);
                UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                if (evprmActiveIngredients != null)
                {
                    evprmPharmaceuticalProduct.activeingredients = evprmActiveIngredients;
                }
            }

            #endregion

            #region Excipients

            {
                pharmaceuticalproductType.excipientsLocalType evprmExcipients;
                ValidationResult operationResult = ConstructExcipients(dbPharmaceuticalProduct, operationType, out evprmExcipients, evprmPPLocation);
                UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                if (evprmExcipients != null)
                {
                    evprmPharmaceuticalProduct.excipients = evprmExcipients;
                }
            }

            #endregion

            #region Adjuvants

            {
                pharmaceuticalproductType.adjuvantsLocalType evprmAdjuvants;
                ValidationResult operationResult = ConstructAdjuvants(dbPharmaceuticalProduct, operationType, out evprmAdjuvants, evprmPPLocation);
                UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                if (evprmAdjuvants != null)
                {
                    evprmPharmaceuticalProduct.adjuvants = evprmAdjuvants;
                }
            }

            #endregion

            validationExceptionTree.Value.XevprmValidationExceptions = validationExceptions;

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
