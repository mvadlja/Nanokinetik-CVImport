using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Ready.Model;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;

namespace xEVMPD
{
    /// <summary>
    /// xEVPRM - eXtended EudraVIgilance Product Report Message
    /// </summary>
    public partial class xEVPRMessage
    {
        static IAuthorisedProductOperations _authorizedProductOperations;
        static IProduct_PKOperations _productOperations;
        static IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        static IType_PKOperations _typeOperations = new Type_PKDAL();
        static IMeddra_ap_mn_PKOperations _indicationOperations = new Meddra_ap_mn_PKDAL();
        static IXevprm_log_PKOperations _logOperations = new Xevprm_log_PKDAL();
        static ISsi__cont_voc_PKOperations _ssiOperations = new Ssi__cont_voc_PKDAL();
        static IProduct_atc_mn_PKOperations _productATCMNOperations;
        static IProduct_indications_PKOperations _productIndicationOperations;
        static IProduct_mn_PKOperations _productMNOperations;
        static IXevprm_message_PKOperations _xevprm_message_PKOperation;
        static IProduct_pi_mn_PKOperations _productPIMNOperations;
        static IPp_ar_mn_PKOperations _pp_ar_mn_PKOperations;
        static IPp_md_mn_PKOperations _pp_md_mn_PKOperations;
        static IAdjuvant_PKOperations _adjuvant_PKOperations;
        static IExcipient_PKOperations _excipient_PKOPerations;
        static IActiveingredient_PKOperations _activeIngredieent_PKOperations;
        static ISubstance_PKOperations _substance_PKOperations;
        static IXevprm_ap_details_PKOperations _xevprm_ap_details_PKOperations;

        public static bool ValidateAP(int msg_PK, int ap_PK, XevprmOperationType operationType = XevprmOperationType.Insert)
        {
            _authorizedProductOperations = new AuthorisedProductDAL();
            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _productATCMNOperations = new Product_atc_mn_PKDAL();
            _productPIMNOperations = new Product_pi_mn_PKDAL();
            _adjuvant_PKOperations = new Adjuvant_PKDAL();
            _excipient_PKOPerations = new Excipient_PKDAL();

            AuthorisedProduct ap = _authorizedProductOperations.GetEntity(ap_PK);
            Product_PK product = _productOperations.GetEntity(ap.product_FK);

            List<Pharmaceutical_product_PK> pplist = new List<Pharmaceutical_product_PK>();
            if (ap != null && ap.product_FK != null)
                pplist = _pharmaceuticalProductOperations.GetPPForProduct(ap.product_FK);

            DataSet atcDS = _productATCMNOperations.GetATCByProduct(ap.product_FK);
            DataSet indicationDS = _indicationOperations.GetMEDDRAByAP(ap.ap_PK);
            Type_PK authStatus = _typeOperations.GetEntity(ap.authorisationstatus_FK);


            Type_PK authorizationProcedureType = product != null ? _typeOperations.GetEntity(product.authorisation_procedure) : null;
            string authorizationProcedureName = authorizationProcedureType != null ? authorizationProcedureType.name : null;

            bool statusOK = false;
            bool authTypeValidSuspended = false;
            bool authorizationProcedureFlag = true;

            if (authStatus != null)
            {
                if (authStatus.name != null && authStatus.name.Trim().ToLower().Equals("valid") || authStatus.name.Trim().ToLower() == "suspended")
                {
                    authTypeValidSuspended = true;
                    if (authStatus.name.Trim().ToLower() == "valid") //If  authstatus is suspended then withdrawn date must be present
                    {
                        if (ap.authorisationwithdrawndate == null)
                        {
                            authTypeValidSuspended = true;
                        }
                        else
                        {
                            authTypeValidSuspended = false;
                        }
                    }
                    else
                    {
                        if (ap.authorisationwithdrawndate != null)
                        {
                            authTypeValidSuspended = true;
                        }
                        else
                        {
                            authTypeValidSuspended = false;
                        }
                    }
                }
            }
            if (authorizationProcedureName != null)
            {
                if (authorizationProcedureName.ToLower().Contains("centralised") && !authorizationProcedureName.ToLower().Contains("decentralised"))
                {
                    if (string.IsNullOrWhiteSpace(ap.authorisationnumber))
                        authorizationProcedureFlag = false;
                }
                else if ((authorizationProcedureName.ToLower().Contains("mutual") || authorizationProcedureName.ToLower().Contains("decentralised")))
                {
                    if (string.IsNullOrWhiteSpace(product.product_number))
                        authorizationProcedureFlag = false;
                }
            }
            switch (operationType)
            {
                case XevprmOperationType.Insert:
                    if (ap.organizationmahcode_FK != null &&
                        ap.qppv_code_FK != null &&
                        atcDS.Tables[0].Rows.Count != 0 &&
                        indicationDS.Tables[0].Rows.Count != 0 &&
                        ap.authorisationcountrycode_FK != null &&
                        product.authorisation_procedure != null &&
                        ap.authorisationdate != null &&
                        ap.authorisationnumber != null &&
                        product.orphan_drug.HasValue &&
                        !string.IsNullOrWhiteSpace(ap.product_name) &&
                        pplist.Count != 0 &&
                        authTypeValidSuspended
                        && !string.IsNullOrWhiteSpace(ap.phv_email)
                        && ValidationHelper.IsValidEmail(ap.phv_email)
                        && !string.IsNullOrWhiteSpace(ap.phv_phone)
                        && authorizationProcedureFlag
                      )
                        statusOK = true;
                    if (statusOK && (string.IsNullOrWhiteSpace(ap.productshortname)) && string.IsNullOrWhiteSpace(ap.productgenericname))
                    {
                        statusOK = false;
                    }
                    if (statusOK && (string.IsNullOrWhiteSpace(ap.productshortname) && string.IsNullOrWhiteSpace(ap.productcompanyname)))
                    {
                        statusOK = false;
                    }
                    if (statusOK && pplist.Count != 0)
                    {
                        _activeIngredieent_PKOperations = new Activeingredient_PKDAL();
                        _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

                        foreach (Pharmaceutical_product_PK pp in pplist)
                        {
                            if (pp.Pharmform_FK == null)
                                statusOK = false;
                            List<Activeingredient_PK> ingredients = _activeIngredieent_PKOperations.GetIngredientsByPPPK(pp.pharmaceutical_product_PK);
                            if (ingredients.Count == 0)
                                statusOK = false;
                            else
                            {
                                foreach (Activeingredient_PK item in ingredients)
                                {
                                    if (item.concentrationtypecode == null)
                                    {
                                        statusOK = false;
                                    }
                                    if (item.lowamountnumervalue == null)
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                        statusOK = false;
                                    if (item.lowamountdenomvalue == null)
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                        statusOK = false;
                                    if (item.concentrationtypecode != null)
                                    {
                                        Ssi__cont_voc_PK concetrationType = _ssiOperations.GetEntity(item.concentrationtypecode);
                                        if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                        {
                                            if (concetrationType.term_name_english.ToLower().Contains("range"))
                                            {
                                                if (item.highamountnumervalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountnumerunit))
                                                    statusOK = false;
                                                if (item.highamountdenomvalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                    statusOK = false;
                                            }
                                        }
                                    }

                                }
                            }
                            List<Adjuvant_PK> adjuvantsInPP = _adjuvant_PKOperations.GetAdjuvantsByPPPK(pp.pharmaceutical_product_PK);
                            if (adjuvantsInPP.Count != 0)
                            {
                                foreach (Adjuvant_PK item in adjuvantsInPP)
                                {
                                    if (item.concentrationtypecode == null)
                                    {
                                        statusOK = false;
                                    }

                                    if (item.lowamountnumervalue == null)
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                        statusOK = false;
                                    if (item.lowamountdenomvalue == null)
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                        statusOK = false;
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                        statusOK = false;

                                    if (item.concentrationtypecode != null)
                                    {
                                        Ssi__cont_voc_PK concetrationType = _ssiOperations.GetEntity(item.concentrationtypecode);
                                        if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                        {
                                            if (concetrationType.term_name_english.ToLower().Contains("range"))
                                            {
                                                if (item.highamountnumervalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                                    statusOK = false;
                                                if (item.highamountdenomvalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                    statusOK = false;
                                            }
                                        }
                                    }

                                }
                            }

                            List<Excipient_PK> excipientsInPP = _excipient_PKOPerations.GetExcipientsByPPPK(pp.pharmaceutical_product_PK);
                            if (excipientsInPP.Count != 0)
                            {
                                foreach (Excipient_PK item in excipientsInPP)
                                {
                                    if (item.concentrationtypecode != null)
                                    {
                                        if (item.lowamountnumervalue == null)
                                            statusOK = false;
                                        if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                            statusOK = false;
                                        if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                            statusOK = false;
                                        if (item.lowamountdenomvalue == null)
                                            statusOK = false;
                                        if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                            statusOK = false;
                                        if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                            statusOK = false;

                                        Ssi__cont_voc_PK concetrationType = _ssiOperations.GetEntity(item.concentrationtypecode);
                                        if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                        {
                                            if (concetrationType.term_name_english.ToLower().Contains("range"))
                                            {
                                                if (item.highamountnumervalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                                    statusOK = false;
                                                if (item.highamountdenomvalue == null)
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                    statusOK = false;
                                                if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                    statusOK = false;
                                            }
                                        }
                                    }
                                }
                            }

                            List<Pp_ar_mn_PK> adminRoutesInPP = _pp_ar_mn_PKOperations.GetAdminRoutesByPPPK(pp.pharmaceutical_product_PK);
                            if (adminRoutesInPP.Count == 0)
                                statusOK = false;
                        }
                    }

                    IDocument_PKOperations _docOpertaion = new Document_PKDAL();
                    bool hasPPI = false;
                    try
                    {
                        var docList = _docOpertaion.GetDocumentsByAP((int)ap.ap_PK);
                        foreach (Document_PK document in docList)
                        {
                            Type_PK typeDoc = _typeOperations.GetEntity(document.type_FK);
                            if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
                            {
                                hasPPI = true;
                                if (document.version_date == null)
                                {
                                    hasPPI = false;
                                }

                                IAttachment_PKOperations _attachment_PKOperations = new Attachment_PKDAL();

                                List<Attachment_PK> attachments = _attachment_PKOperations.GetAttachmentsForDocument((int)document.document_PK);
                                if (attachments.Count > 0 && !String.IsNullOrWhiteSpace(attachments[0].attachmentname))
                                {
                                    string extension = Path.GetExtension(attachments[0].attachmentname);

                                    Type_PK type = _typeOperations.GetEntity(document.attachment_type_FK);

                                    if (type != null && !string.IsNullOrWhiteSpace(type.name))
                                    {
                                        if (string.IsNullOrWhiteSpace(extension) ||
                                           extension.ToLower().Trim() != "." + type.name.ToLower().Trim())
                                        {
                                            statusOK = false;
                                        }
                                    }
                                }
                            }
                        }

                        //if (!hasPPI && ap.product_FK != null)
                        //{
                        //    docList = _docOpertaion.GetDocumentsByProduct((int)ap.product_FK);

                        //    foreach (Document_PK document in docList)
                        //    {
                        //        Type_PK typeDoc = _typeOperations.GetEntity(document.type_FK);
                        //        if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
                        //        {
                        //            hasPPI = true;
                        //            if (document.version_date == null)
                        //            {
                        //                hasPPI = false;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    catch
                    {
                        hasPPI = false;
                    }

                    if (statusOK && hasPPI)
                    {
                        var _xevprm_message_PKOperation = new Xevprm_message_PKDAL();

                        Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msg_PK);
                        xEVPRMessage msg = new xEVPRMessage();
                        var xmlParsingError = msg.ConstructFrom(message);
                        if (xmlParsingError.Count == 0)
                            statusOK = true;
                        else
                            statusOK = false;
                    }
                    else
                    {
                        statusOK = false;
                    }
                    break;
                case XevprmOperationType.Nullify:
                    statusOK = IsEntityValid(ap, XevprmOperationType.Nullify);
                    var _xevprm_message_PKOperations = new Xevprm_message_PKDAL();

                    Xevprm_message_PK message1 = _xevprm_message_PKOperations.GetEntity(msg_PK);
                    xEVPRMessage msg1 = new xEVPRMessage();
                    var xmlParsingError1 = msg1.ConstructFrom(message1);
                    if (xmlParsingError1.Count == 0)
                        statusOK = true;
                    else
                        statusOK = false;
                    break;
                default:
                    statusOK = false;
                    break;
            }

            return statusOK;
        }

        public static Control ValidationErrorsAP(int msg_id, XevprmOperationType operationType)// = XevprmOperationType.Insert)
        {
            string errors = "";
            bool noError = true;
            Control errorsList = new Control();
            _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            _xevprm_ap_details_PKOperations = new Xevprm_ap_details_PKDAL();
            Xevprm_message_PK message = _xevprm_message_PKOperation.GetEntity(msg_id);

            if (message.status == XevprmStatus.NotReady || message.status == XevprmStatus.Created)
            {
                Label apErrors = new Label();
                //apErrors.Text = "Authorised product<br />";
                apErrors.Text = "Authorised product";
                errorsList.Controls.Add(apErrors);

                _authorizedProductOperations = new AuthorisedProductDAL();
                _productOperations = new Product_PKDAL();
                _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
                _productIndicationOperations = new Product_indications_PKDAL();
                _productATCMNOperations = new Product_atc_mn_PKDAL();
                _productMNOperations = new Product_mn_PKDAL();
                _productPIMNOperations = new Product_pi_mn_PKDAL();

                _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

                _activeIngredieent_PKOperations = new Activeingredient_PKDAL();
                _adjuvant_PKOperations = new Adjuvant_PKDAL();
                _excipient_PKOPerations = new Excipient_PKDAL();
                _substance_PKOperations = new Substance_PKDAL();


                Xevprm_ap_details_PK messageAPDetails = _xevprm_ap_details_PKOperations.GetEntityForXevprm(message.xevprm_message_PK);

                AuthorisedProduct ap = _authorizedProductOperations.GetEntity(messageAPDetails.ap_FK);

                switch (messageAPDetails.OperationType)
                {
                    case XevprmOperationType.Insert:

                        DataSet atcDS = _productATCMNOperations.GetATCByProduct(ap.product_FK);

                        DataSet indicationDS = _indicationOperations.GetMEDDRAByAP(ap.ap_PK);
                        List<Pharmaceutical_product_PK> pplist = new List<Pharmaceutical_product_PK>();
                        if (ap != null && ap.product_FK != null)
                            pplist = _pharmaceuticalProductOperations.GetPPForProduct(ap.product_FK);
                        string apLink = "../AuthorisedProductView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd=" + ap.ap_PK;
                        if (ap.authorisationstatus_FK == null)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.12.3.BR.1: Authorisation status can't be empty.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                        }

                        Type_PK authStatus = _typeOperations.GetEntity(ap.authorisationstatus_FK);
                        bool authTypeValidSuspended = false;
                        if (authStatus != null)
                        {
                            if (authStatus.name != null && authStatus.name.Trim().ToLower().Equals("valid") || authStatus.name.Trim().ToLower() == "suspended")
                                authTypeValidSuspended = true;
                        }


                        if (!authTypeValidSuspended)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.12.3.BR.3: Authorisation status must be “Valid” or “Suspended”.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                        }
                        if (authStatus != null && authStatus.name != null && authStatus.name.Trim().ToLower() == "valid")
                        {
                            if (ap.authorisationwithdrawndate != null)
                            {
                                HyperLink hl = new HyperLink()
                                {
                                    Text = "AP.12.12.BR.6: If the value of authorisation status is “Valid”, then withdrawn date must be empty.",
                                    NavigateUrl = apLink,
                                    Target = "_blank"
                                };
                                apErrors.Controls.Add(hl);
                            }
                        }
                        else//if (authStatus != null && authStatus.name != null && authStatus.name.Trim().ToLower() == "suspended")
                        {
                            if (ap.authorisationwithdrawndate == null)
                            {
                                HyperLink hl = new HyperLink()
                                {
                                    Text = "AP.12.12.BR.5: If the value of authorisation status is NOT “Valid”, then withdrawn date must be present.",
                                    NavigateUrl = apLink,
                                    Target = "_blank"
                                };
                                apErrors.Controls.Add(hl);
                            }
                        }
                        if (ap.organizationmahcode_FK == null)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.4.BR: Licence holder can't be empty.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                            noError = false;
                            errors += "Licence holder can't be empty.<br/>";
                        }

                        if (string.IsNullOrWhiteSpace(ap.authorisationnumber))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.12.4.BR.1: If the operation type is “insert”, “update”, “variation” or “withdrawal”, then Authorisation number must be present.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                        }

                        if (ap.qppv_code_FK == null && !(operationType == XevprmOperationType.Nullify || operationType == XevprmOperationType.Withdraw))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.5.BR.1: QPPV must be specified unless the operation type is “nullification” or “withdrawal”.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }

                        if (ap.authorisationcountrycode_FK == null)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.12.1.BR: Authorisation country code is mandatory.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                            noError = false;
                            errors += "Country can't be empty.<br/>";
                        }
                        if (indicationDS.Tables[0].Rows.Count == 0 && !(operationType == XevprmOperationType.Nullify || operationType == XevprmOperationType.Withdraw))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.INDs.BR.1: Indications are mandatory if the operation type is “insert”, “update” or “variation”.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                            noError = false;
                            errors += "Indications can't be empty.<br/>";
                        }

                        if (string.IsNullOrWhiteSpace(ap.productshortname) && string.IsNullOrWhiteSpace(ap.productgenericname))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.13.2.BR, AP.13.3.BR: Both product short name and product generic name can't be empty.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                            noError = false;
                            errors += "Both product short name and product generic name can 't be empty..<br/>";
                        }
                        if (string.IsNullOrWhiteSpace(ap.productshortname) && string.IsNullOrWhiteSpace(ap.productcompanyname))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.13.4.BR.2: Product company name must be present if product short name is absent.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }
                        if (ap.authorisationdate == null && !(operationType == XevprmOperationType.Withdraw || operationType == XevprmOperationType.Nullify))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.12.5.BR.1: Authorisation date is mandatory unless performing a nullification or withdrawal operation.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }
                        if (String.IsNullOrWhiteSpace(ap.product_name))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.13.1.BR: Full presentation name can't be empty.";
                            //hl.NavigateUrl = "../Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }

                        Label productErrors = new Label();
                        //productErrors.Text = "Product<br />";
                        productErrors.Text = "Product";
                        errorsList.Controls.Add(productErrors);

                        Product_PK product = _productOperations.GetEntity(ap.product_FK);
                        string pLink = "../ProductView/Form.aspx?EntityContext=Product&Action=Edit&error=1&idProd=" + ap.product_FK;
                        if (product != null)
                        {
                            if (product.authorisation_procedure == null)
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.12.2.BR: Authorisation procedure is mandatory.";
                                //hl.NavigateUrl = "~/Views/Business/ProductPKPPropertiesView.aspx?f=d&error=1&id=" + ap.product_FK;
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                                noError = false;
                            }
                            else
                            {
                                Type_PK authorizationProcedureType = _typeOperations.GetEntity(product.authorisation_procedure);
                                string authorizationProcedureName = authorizationProcedureType != null ? authorizationProcedureType.name : null;

                                if (authorizationProcedureName != null)
                                {
                                    if (authorizationProcedureName.ToLower().Contains("centralised") && !authorizationProcedureName.ToLower().Contains("decentralised"))
                                    {
                                        if (string.IsNullOrWhiteSpace(ap.authorisationnumber))
                                        {
                                            HyperLink hl = new HyperLink();
                                            hl.Text = "AP.12.8.BR.1: If the authorisation procedure is an “EU centralised procedure”, the \"EU Authorisation Number\", as assigned by the European Commission, must be specified.";
                                            hl.NavigateUrl = apLink;
                                            hl.Target = "_blank";
                                            apErrors.Controls.Add(hl);
                                            noError = false;
                                        }
                                    }
                                    else if (authorizationProcedureName.ToLower().Contains("mutual"))
                                    {
                                        if (string.IsNullOrWhiteSpace(product.product_number))
                                        {
                                            HyperLink hl = new HyperLink();
                                            hl.Text = "AP.12.7.BR.1: If the authorisation procedure follows the \"EU mutual recognition procedure\", the Mutual Recognition Procedure (MRP) Number must be specified.";
                                            hl.NavigateUrl = pLink;
                                            hl.Target = "_blank";
                                            productErrors.Controls.Add(hl);
                                            noError = false;
                                        }
                                    }
                                    else if (authorizationProcedureName.ToLower().Contains("decentralised"))
                                    {
                                        if (string.IsNullOrWhiteSpace(product.product_number))
                                        {
                                            HyperLink hl = new HyperLink();
                                            hl.Text = "AP.12.7.BR.1: If the authorisation procedure follows the \"EU decentralised procedure\", the EU Decentralised Procedure (DCP) number must be specified.";
                                            hl.NavigateUrl = pLink;
                                            hl.Target = "_blank";
                                            productErrors.Controls.Add(hl);
                                            noError = false;
                                        }
                                    }
                                }
                            }

                            if (!product.orphan_drug.HasValue && !(operationType == XevprmOperationType.Withdraw || operationType == XevprmOperationType.Nullify))
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.12.9.BR.1: Orphandrug is mandatory unless the operation type is “nullification” or “withdrawal”.";
                                //hl.NavigateUrl = "~/Views/Business/ProductPKPPropertiesView.aspx?f=d&error=1&id=" + ap.product_FK;
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                                noError = false;
                            }
                            if (atcDS.Tables[0].Rows.Count == 0)
                            {

                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.ATCs.BR: ATCs are mandatory.";
                                //hl.NavigateUrl = "~/Views/Business/ProductPKPPropertiesView.aspx?f=d&error=1&id=" + ap.product_FK;
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                                noError = false;
                            }

                            Label pPErrors = new Label();
                            //pPErrors.Text = "Pharmaceutical products<br />";
                            pPErrors.Text = "Pharmaceutical products";
                            errorsList.Controls.Add(pPErrors);



                            if (pplist.Count == 0)
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.PPs: Pharmaceutical products can't be empty.";
                                //hl.NavigateUrl = "~/Views/Business/ProductPKPPropertiesView.aspx?f=d&error=1&id=" + ap.product_FK;
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                                noError = false;
                            }
                            else
                            {
                                _pp_md_mn_PKOperations = new Pp_md_mn_PKDAL();
                                _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

                                foreach (Pharmaceutical_product_PK pp in pplist)
                                {
                                    string ppLink = "../PharmaceuticalProductView/Form.aspx?EntityContext=PharmaceuticalProduct&Action=Edit&error=1&idPharmProd=" + pp.pharmaceutical_product_PK;
                                    Label pProduct = new Label();
                                    pProduct.Text = pp.name;
                                    pPErrors.Controls.Add(pProduct);

                                    if (pp.Pharmform_FK == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "AuthProducts.PP.1.BR.1: Pharmaceutical form must be set.";
                                        //hl.NavigateUrl = "~/Views/Business/PharmaceuticalProductProperties.aspx?f=p&error=1&id=0&idpp=" + pp.pharmaceutical_product_PK;
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                        noError = false;
                                    }

                                    List<Pp_md_mn_PK> medDevicesInPP = _pp_md_mn_PKOperations.GetMedDevicesByPPPK(pp.pharmaceutical_product_PK);
                                    List<Pp_ar_mn_PK> adminRoutesInPP = _pp_ar_mn_PKOperations.GetAdminRoutesByPPPK(pp.pharmaceutical_product_PK);
                                    if (adminRoutesInPP.Count == 0)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ARs.BR: Administration routes can't be empty.";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                        noError = false;
                                    }

                                    List<Activeingredient_PK> ingredients = _activeIngredieent_PKOperations.GetIngredientsByPPPK(pp.pharmaceutical_product_PK);
                                    if (ingredients.Count == 0)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACTs.BR: Active ingredients can't be empty.";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                        noError = false;
                                    }
                                    else
                                    {
                                        foreach (var item in ingredients)
                                        {
                                            //Control ingredientsError = new Control();
                                            Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                            string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                            if (item.concentrationtypecode == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.2: Active ingredient - concentration type is mandatory" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                                noError = false;
                                            }
                                            else
                                            {
                                                if (item.lowamountnumervalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.3: The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.4: The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.5: The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (item.lowamountdenomvalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.6: The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.7: The low limit amount denominator prefix, or for non-range measurements, the amount denominator prefix must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ACT.8: The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }

                                                Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                                if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                                {
                                                    if (concetrationType.term_name_english.ToLower().Contains("range"))
                                                    {
                                                        if (item.highamountnumervalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.9: The high limit amount numerator value must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.10: The high limit amount numerator prefix must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountnumerunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.11: The high limit amount numerator unit must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (item.highamountdenomvalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.12: The high limit amount denominator value must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.13: The high limit amount denominator prefix must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ACT.14: The high limit amount denominator unit must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }

                                                        //pProduct.Controls.Add(ingredientsError);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //List<Excipient_PK> excipients = _excipient_PKOPerations.GetExcipientsByPPPK(pp.pharmaceutical_product_PK);
                                    List<Adjuvant_PK> adjuvants = _adjuvant_PKOperations.GetAdjuvantsByPPPK(pp.pharmaceutical_product_PK);
                                    if (adjuvants.Count != 0)
                                    {
                                        foreach (var item in adjuvants)
                                        {
                                            Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                            string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                            if (item.concentrationtypecode == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.2: Adjuvants - concentration type is mandatory" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                                noError = false;
                                            }
                                            else
                                            {
                                                if (item.lowamountnumervalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.3: The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.4: The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.5: The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (item.lowamountdenomvalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.6: The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.7: The low limit amount denominator prefix or, for non-range measurements, the amount denominator prefix must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.ADJ.8: The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }

                                                Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                                if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                                {
                                                    if (concetrationType.term_name_english.ToLower().Contains("range"))
                                                    {
                                                        if (item.highamountnumervalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.9: The high limit amount numerator value must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.10: The high limit amount numerator prefix must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.11: The high limit amount numerator unit must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (item.highamountdenomvalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.12: The high limit amount denominator value must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.13: The high limit amount denominator prefix must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.ADJ.14: The high limit amount denominator unit must be specified when a range is described" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    List<Excipient_PK> excipients = _excipient_PKOPerations.GetExcipientsByPPPK(pp.pharmaceutical_product_PK);
                                    if (excipients.Count != 0)
                                    {
                                        foreach (var item in excipients)
                                        {
                                            Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                            string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                            if (item.concentrationtypecode != null)
                                            {
                                                if (item.lowamountnumervalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.3 (Low limit numerator value)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.4 (Low limit numerator prefix)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.5 (Low limit numerator Unit)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (item.lowamountdenomvalue == null)
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.6 (Low limit denominator value)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.7 (Low limit denominator prefix)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }
                                                if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                                {
                                                    HyperLink hl = new HyperLink();
                                                    hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.8 (Low limit denominator Unit)" + substanceName + ".";
                                                    hl.NavigateUrl = ppLink;
                                                    hl.Target = "_blank";
                                                    pProduct.Controls.Add(hl);
                                                    noError = false;
                                                }

                                                Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                                if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                                {
                                                    if (concetrationType.term_name_english.ToLower().Contains("range"))
                                                    {
                                                        if (item.highamountnumervalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.9 (High limit numerator value)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.10 (High limit numerator prefix)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.11 (High limit numerator Unit)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (item.highamountdenomvalue == null)
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.12 (High limit denominator Value)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.13 (High limit denominator prefix)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                        if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                                        {
                                                            HyperLink hl = new HyperLink();
                                                            hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.14 (High limit denominator Unit)" + substanceName + ".";
                                                            hl.NavigateUrl = ppLink;
                                                            hl.Target = "_blank";
                                                            pProduct.Controls.Add(hl);
                                                            noError = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                bool noPPErrors = true;
                                foreach (Control control in pPErrors.Controls)
                                {
                                    if (control.Controls.Count > 0)
                                    {
                                        noPPErrors = false;
                                        break;
                                    }
                                }

                                if (noPPErrors)
                                    errorsList.Controls.Remove(pPErrors);
                            }
                        }
                        else
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "Related product can't be empty.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);

                            noError = false;
                        }

                        IDocument_PKOperations _docOpertaion = new Document_PKDAL();
                        bool hasPPI = false;
                        var docList = _docOpertaion.GetDocumentsByAP((int)ap.ap_PK);

                        foreach (Document_PK document in docList)
                        {
                            Type_PK typeDoc = _typeOperations.GetEntity(document.type_FK);
                            if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
                            {
                                hasPPI = true;
                                if (document.version_date == null)
                                {
                                    HyperLink hl = new HyperLink();
                                    hl.Text = "ATT.8: The date of the last update of the PPI document must be specified.";
                                    hl.NavigateUrl = "../DocumentView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd=" + messageAPDetails.ap_FK + "&idDoc=" + document.document_PK + "&error=1";
                                    hl.Target = "_blank";
                                    apErrors.Controls.Add(hl);
                                }

                                IAttachment_PKOperations _attachment_PKOperations = new Attachment_PKDAL();

                                List<Attachment_PK> attachments = _attachment_PKOperations.GetAttachmentsForDocument((int)document.document_PK);
                                if (attachments.Count > 0 && !String.IsNullOrWhiteSpace(attachments[0].attachmentname))
                                {
                                    string extension = Path.GetExtension(attachments[0].attachmentname);

                                    Type_PK type = _typeOperations.GetEntity(document.attachment_type_FK);

                                    if (type != null && !string.IsNullOrWhiteSpace(type.name))
                                    {
                                        if (string.IsNullOrWhiteSpace(extension) ||
                                           extension.ToLower().Trim() != "." + type.name.ToLower().Trim())
                                        {
                                            HyperLink hl = new HyperLink();
                                            hl.Text = "ATT.3: Specified attachment type doesn't match extension of uploaded attachment.";
                                            hl.NavigateUrl = "../DocumentView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd=" + messageAPDetails.ap_FK + "&idDoc=" + document.document_PK + "&error=1";
                                            hl.Target = "_blank";
                                            apErrors.Controls.Add(hl);
                                        }
                                    }
                                }
                            }
                        }

                        //if (!hasPPI & ap.product_FK != null)
                        //{
                        //    docList = _docOpertaion.GetDocumentsByProduct((int)ap.product_FK);

                        //    foreach (Document_PK document in docList)
                        //    {
                        //        Type_PK typeDoc = _typeOperations.GetEntity(document.type_FK);
                        //        if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
                        //        {
                        //            hasPPI = true;
                        //            if (document.version_date == null)
                        //            {
                        //                HyperLink hl = new HyperLink();
                        //                hl.Text = "ATT.8: The date of the last update of the PPI document must be specified.";
                        //                hl.NavigateUrl = "PDocumentsView.aspx?f=p&id=" + ap.product_FK + "&idDoc=" + document.document_PK + "&error=1";
                        //                hl.Target = "_blank";
                        //                productErrors.Controls.Add(hl);
                        //            }
                        //        }
                        //    }
                        //}
                        if (!hasPPI)
                        {
                            HyperLink hl = new HyperLink();
                            //hl.Text = "AP.PPI.1.BR: Either Authorised product or Product must have a PPI attachment.";
                            hl.Text = "AP.PPI.1.BR: Authorised product must have a PPI attachment.";
                            hl.NavigateUrl = "../DocumentView/List.aspx?EntityContext=AuthorisedProduct&error=1&idAuthProd=" + messageAPDetails.ap_FK;
                            //hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }

                        if (string.IsNullOrWhiteSpace(ap.phv_email) || !ValidationHelper.IsValidEmail(ap.phv_email))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.7.BR.1: If the operation type is “insert”, “update” or “variation”, then PhV email must be present and valid.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }

                        if (string.IsNullOrWhiteSpace(ap.phv_phone))
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AP.8.BR.1: If the operation type is “insert”, “update” or “variation”, then PhV phone number must be present.";
                            //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                            hl.NavigateUrl = apLink;
                            hl.Target = "_blank";
                            apErrors.Controls.Add(hl);
                            noError = false;
                        }

                        if (apErrors.Controls.Count == 0)
                            errorsList.Controls.Remove(apErrors);
                        if (productErrors.Controls.Count == 0)
                            errorsList.Controls.Remove(productErrors);


                        break;
                    case XevprmOperationType.Nullify:
                        noError = true;

                        Control apNullifyErrors = ValidateEntity(ap, XevprmOperationType.Nullify);
                        
                        
                        if (apNullifyErrors.Controls.Count > 0)
                        {
                            int indexStart = apErrors.Controls.Count;

                            for (int i = 0; i < apNullifyErrors.Controls.Count; i++)
                            {
                                apErrors.Controls.Add(new Control());
                            }

                            for (int i = apNullifyErrors.Controls.Count - 1; i >= 0; i--)
                            {
                                apErrors.Controls.AddAt(indexStart+i, apNullifyErrors.Controls[i]);
                            }
                        }

                        noError = apErrors.Controls.Count == 0;

                        if (apErrors.Controls.Count == 0)
                            errorsList.Controls.Remove(apErrors);
                        break;
                    default:
                        break;
                }
            }
            if (noError)
                _logOperations.AddNewEntry(msg_id, "Validation OK");

            return errorsList;
        }

        public static bool IsEntityValid(object entity, XevprmOperationType operationType)
        {
            if (entity == null) return false;

            if (entity.GetType() == typeof(AuthorisedProduct))
            {
                switch (operationType)
                {
                    case XevprmOperationType.Nullify:
                        return IsAuthorisedProductValidForNullify(entity as AuthorisedProduct);
                }
            }
            return true;
        }

        public static Control ValidateEntity(object entity, XevprmOperationType operationType)
        {
            Control errors = new Control();
            if (entity == null) return errors;

            if (entity.GetType() == typeof(AuthorisedProduct))
            {
                switch (operationType)
                {
                    case XevprmOperationType.Nullify:
                        return ValidateAuthorisedProductForNullify(entity as AuthorisedProduct);
                }
            }
            return errors;
        }

        #region Authorised Product Validation

        private static bool IsAuthorisedProductValidForNullify(AuthorisedProduct ap)
        {
            return ValidateAuthorisedProductForNullify(ap).Controls.Count == 0;
        }

        private static Control ValidateAuthorisedProductForNullify(AuthorisedProduct ap)
        {
            Control errorsList = new Control();
            if (ap == null) return errorsList;

            Label apErrors = new Label();
            apErrors.Text = "Authorised product";
            errorsList.Controls.Add(apErrors);

            _authorizedProductOperations = new AuthorisedProductDAL();
            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _productIndicationOperations = new Product_indications_PKDAL();
            _productATCMNOperations = new Product_atc_mn_PKDAL();
            _productMNOperations = new Product_mn_PKDAL();
            _productPIMNOperations = new Product_pi_mn_PKDAL();
            _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();
            _activeIngredieent_PKOperations = new Activeingredient_PKDAL();
            _adjuvant_PKOperations = new Adjuvant_PKDAL();
            _excipient_PKOPerations = new Excipient_PKDAL();
            _substance_PKOperations = new Substance_PKDAL();

            DataSet atcDS = _productATCMNOperations.GetATCByProduct(ap.product_FK);

            var pplist = new List<Pharmaceutical_product_PK>();
            if (ap != null && ap.product_FK != null)
                pplist = _pharmaceuticalProductOperations.GetPPForProduct(ap.product_FK);
            string apLink = "../AuthorisedProductView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd=" + ap.ap_PK;

            if (string.IsNullOrWhiteSpace(ap.ev_code))
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.2.BR: EV Code can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            if (string.IsNullOrWhiteSpace(ap.evprm_comments))
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.14.BR: EVPRM comment can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            if (ap.authorisationstatus_FK.HasValue)
            {
                Type_PK authStatus = _typeOperations.GetEntity(ap.authorisationstatus_FK);
                bool authTypeValidSuspended = false;
                if (authStatus != null)
                {
                    if (authStatus.name != null && authStatus.name.Trim().ToLower().Equals("valid") ||
                        authStatus.name.Trim().ToLower() == "suspended")
                        authTypeValidSuspended = true;
                }


                if (!authTypeValidSuspended)
                {
                    HyperLink hl = new HyperLink();
                    hl.Text = "AP.12.3.BR.3: Authorisation status must be “Valid” or “Suspended”.";
                    //hl.NavigateUrl = "~/Views/Business/APPropertiesView.aspx?f=d&error=1&id=" + ap.ap_PK;
                    hl.NavigateUrl = apLink;
                    hl.Target = "_blank";
                    apErrors.Controls.Add(hl);

                }
                if (authStatus != null && authStatus.name != null && authStatus.name.Trim().ToLower() == "valid")
                {
                    if (ap.authorisationwithdrawndate != null)
                    {
                        HyperLink hl = new HyperLink()
                                           {
                                               Text =
                                                   "AP.12.12.BR.6: If the value of authorisation status is “Valid”, then withdrawn date must be empty.",
                                               NavigateUrl = apLink,
                                               Target = "_blank"
                                           };
                        apErrors.Controls.Add(hl);
                    }
                }
                else
                    //if (authStatus != null && authStatus.name != null && authStatus.name.Trim().ToLower() == "suspended")
                {
                    if (ap.authorisationwithdrawndate == null)
                    {
                        HyperLink hl = new HyperLink()
                                           {
                                               Text =
                                                   "AP.12.12.BR.5: If the value of authorisation status is NOT “Valid”, then withdrawn date must be present.",
                                               NavigateUrl = apLink,
                                               Target = "_blank"
                                           };
                        apErrors.Controls.Add(hl);
                    }
                }
            }

            if (ap.organizationmahcode_FK == null)
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.4.BR: Licence holder can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            if (ap.authorisationcountrycode_FK == null)
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.12.1.BR: Authorisation country code is mandatory.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            if (string.IsNullOrWhiteSpace(ap.productshortname) && string.IsNullOrWhiteSpace(ap.productgenericname))
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.13.2.BR, AP.13.3.BR: Both product short name and product generic name can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }
            if (string.IsNullOrWhiteSpace(ap.productshortname) && string.IsNullOrWhiteSpace(ap.productcompanyname))
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.13.4.BR.2: Product company name must be present if product short name is absent.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            if (String.IsNullOrWhiteSpace(ap.product_name))
            {
                HyperLink hl = new HyperLink();
                hl.Text = "AP.13.1.BR: Full presentation name can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }

            #region Product
            Label productErrors = new Label();
            productErrors.Text = "Product";
            errorsList.Controls.Add(productErrors);

            Product_PK product = _productOperations.GetEntity(ap.product_FK);
            string pLink = "../ProductView/Form.aspx?EntityContext=Product&Action=Edit&error=1&idProd=" + ap.product_FK;
            if (product != null)
            {
                if (product.authorisation_procedure == null)
                {
                    HyperLink hl = new HyperLink();
                    hl.Text = "AP.12.2.BR: Authorisation procedure is mandatory.";
                    hl.NavigateUrl = pLink;
                    hl.Target = "_blank";
                    productErrors.Controls.Add(hl);
                }
                else
                {
                    Type_PK authorizationProcedureType = _typeOperations.GetEntity(product.authorisation_procedure);
                    string authorizationProcedureName = authorizationProcedureType != null ? authorizationProcedureType.name : null;

                    if (authorizationProcedureName != null)
                    {
                        if (authorizationProcedureName.ToLower().Contains("centralised") && !authorizationProcedureName.ToLower().Contains("decentralised"))
                        {
                            if (string.IsNullOrWhiteSpace(ap.authorisationnumber))
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.12.8.BR.1: If the authorisation procedure is an “EU centralised procedure”, the \"EU Authorisation Number\", as assigned by the European Commission, must be specified.";
                                hl.NavigateUrl = apLink;
                                hl.Target = "_blank";
                                apErrors.Controls.Add(hl);
                            }
                        }
                        else if (authorizationProcedureName.ToLower().Contains("mutual"))
                        {
                            if (string.IsNullOrWhiteSpace(product.product_number))
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.12.7.BR.1: If the authorisation procedure follows the \"EU mutual recognition procedure\", the Mutual Recognition Procedure (MRP) Number must be specified.";
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                            }
                        }
                        else if (authorizationProcedureName.ToLower().Contains("decentralised"))
                        {
                            if (string.IsNullOrWhiteSpace(product.product_number))
                            {
                                HyperLink hl = new HyperLink();
                                hl.Text = "AP.12.7.BR.1: If the authorisation procedure follows the \"EU decentralised procedure\", the EU Decentralised Procedure (DCP) number must be specified.";
                                hl.NavigateUrl = pLink;
                                hl.Target = "_blank";
                                productErrors.Controls.Add(hl);
                            }
                        }
                    }
                }

                if (atcDS.Tables[0].Rows.Count == 0)
                {
                    HyperLink hl = new HyperLink();
                    hl.Text = "AP.ATCs.BR: ATCs are mandatory.";
                    hl.NavigateUrl = pLink;
                    hl.Target = "_blank";
                    productErrors.Controls.Add(hl);
                }

                #region Pharmaceutical products
                Label pPErrors = new Label();
                pPErrors.Text = "Pharmaceutical products";
                errorsList.Controls.Add(pPErrors);

                if (pplist.Count == 0)
                {
                    HyperLink hl = new HyperLink();
                    hl.Text = "AP.PPs: Pharmaceutical products can't be empty.";
                    hl.NavigateUrl = pLink;
                    hl.Target = "_blank";
                    productErrors.Controls.Add(hl);
                }
                else
                {
                    _pp_md_mn_PKOperations = new Pp_md_mn_PKDAL();
                    _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

                    foreach (Pharmaceutical_product_PK pp in pplist)
                    {
                        string ppLink = "../PharmaceuticalProductView/Form.aspx?EntityContext=PharmaceuticalProduct&Action=Edit&error=1&idPharmProd=" + pp.pharmaceutical_product_PK;
                        Label pProduct = new Label();
                        pProduct.Text = pp.name;
                        pPErrors.Controls.Add(pProduct);

                        if (pp.Pharmform_FK == null)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "AuthProducts.PP.1.BR.1: Pharmaceutical form must be set.";
                            hl.NavigateUrl = ppLink;
                            hl.Target = "_blank";
                            pProduct.Controls.Add(hl);
                        }

                        List<Pp_ar_mn_PK> adminRoutesInPP = _pp_ar_mn_PKOperations.GetAdminRoutesByPPPK(pp.pharmaceutical_product_PK);
                        if (adminRoutesInPP.Count == 0)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "PP.ARs.BR: Administration routes can't be empty.";
                            hl.NavigateUrl = ppLink;
                            hl.Target = "_blank";
                            pProduct.Controls.Add(hl);
                        }

                        #region Active ingredients
                        List<Activeingredient_PK> ingredients = _activeIngredieent_PKOperations.GetIngredientsByPPPK(pp.pharmaceutical_product_PK);
                        if (ingredients.Count == 0)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = "PP.ACTs.BR: Active ingredients can't be empty.";
                            hl.NavigateUrl = ppLink;
                            hl.Target = "_blank";
                            pProduct.Controls.Add(hl);
                        }
                        else
                        {
                            foreach (var item in ingredients)
                            {
                                Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                if (item.concentrationtypecode == null)
                                {
                                    HyperLink hl = new HyperLink();
                                    hl.Text = "PP.ACT.2: Active ingredient - concentration type is mandatory" + substanceName + ".";
                                    hl.NavigateUrl = ppLink;
                                    hl.Target = "_blank";
                                    pProduct.Controls.Add(hl);
                                }
                                else
                                {
                                    if (item.lowamountnumervalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.3: The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.4: The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.5: The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (item.lowamountdenomvalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.6: The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.7: The low limit amount denominator prefix, or for non-range measurements, the amount denominator prefix must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ACT.8: The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }

                                    Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                    if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                    {
                                        if (concetrationType.term_name_english.ToLower().Contains("range"))
                                        {
                                            if (item.highamountnumervalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.9: The high limit amount numerator value must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.10: The high limit amount numerator prefix must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountnumerunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.11: The high limit amount numerator unit must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (item.highamountdenomvalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.12: The high limit amount denominator value must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.13: The high limit amount denominator prefix must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ACT.14: The high limit amount denominator unit must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Adjuvants
                        List<Adjuvant_PK> adjuvants = _adjuvant_PKOperations.GetAdjuvantsByPPPK(pp.pharmaceutical_product_PK);
                        if (adjuvants.Count != 0)
                        {
                            foreach (var item in adjuvants)
                            {
                                Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                if (item.concentrationtypecode == null)
                                {
                                    HyperLink hl = new HyperLink();
                                    hl.Text = "PP.ADJ.2: Adjuvants - concentration type is mandatory" + substanceName + ".";
                                    hl.NavigateUrl = ppLink;
                                    hl.Target = "_blank";
                                    pProduct.Controls.Add(hl);
                                }
                                else
                                {
                                    if (item.lowamountnumervalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.3: The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.4: The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.5: The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (item.lowamountdenomvalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.6: The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.7: The low limit amount denominator prefix or, for non-range measurements, the amount denominator prefix must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.ADJ.8: The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }

                                    Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                    if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                    {
                                        if (concetrationType.term_name_english.ToLower().Contains("range"))
                                        {
                                            if (item.highamountnumervalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.9: The high limit amount numerator value must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.10: The high limit amount numerator prefix must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.11: The high limit amount numerator unit must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (item.highamountdenomvalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.12: The high limit amount denominator value must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.13: The high limit amount denominator prefix must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.ADJ.14: The high limit amount denominator unit must be specified when a range is described" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Excipients
                        List<Excipient_PK> excipients = _excipient_PKOPerations.GetExcipientsByPPPK(pp.pharmaceutical_product_PK);
                        if (excipients.Count != 0)
                        {
                            foreach (var item in excipients)
                            {
                                Substance_PK substance = _substance_PKOperations.GetEntity(item.substancecode_FK);
                                string substanceName = (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name)) ? " (" + substance.substance_name + ")" : null;

                                if (item.concentrationtypecode != null)
                                {
                                    if (item.lowamountnumervalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.3 (Low limit numerator value)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.4 (Low limit numerator prefix)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountnumerunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.5 (Low limit numerator Unit)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (item.lowamountdenomvalue == null)
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.6 (Low limit denominator value)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomprefix))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.7 (Low limit denominator prefix)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }
                                    if (string.IsNullOrWhiteSpace(item.lowamountdenomunit))
                                    {
                                        HyperLink hl = new HyperLink();
                                        hl.Text = "PP.EXC.2.BR.2: If field PP.EXC.2 (Concentration type) is present the following field must be present: PP.EXC.8 (Low limit denominator Unit)" + substanceName + ".";
                                        hl.NavigateUrl = ppLink;
                                        hl.Target = "_blank";
                                        pProduct.Controls.Add(hl);
                                    }

                                    Ssi__cont_voc_PK concetrationType = item.concentrationtypecode != null ? _ssiOperations.GetEntity(item.concentrationtypecode) : new Ssi__cont_voc_PK();
                                    if (concetrationType != null && !string.IsNullOrWhiteSpace(concetrationType.term_name_english))
                                    {
                                        if (concetrationType.term_name_english.ToLower().Contains("range"))
                                        {
                                            if (item.highamountnumervalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.9 (High limit numerator value)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountnumerprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.10 (High limit numerator prefix)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.higamountnumerunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.11 (High limit numerator Unit)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (item.highamountdenomvalue == null)
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.12 (High limit denominator Value)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomprefix))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.13 (High limit denominator prefix)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                            if (string.IsNullOrWhiteSpace(item.highamountdenomunit))
                                            {
                                                HyperLink hl = new HyperLink();
                                                hl.Text = "PP.EXC.2.BR.3: If field PP.EXC.2 (Concentration type) is present and has a value of 2 (range) the following field must be present: PP.EXC.14 (High limit denominator Unit)" + substanceName + ".";
                                                hl.NavigateUrl = ppLink;
                                                hl.Target = "_blank";
                                                pProduct.Controls.Add(hl);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    bool noPPErrors = true;
                    foreach (Control control in pPErrors.Controls)
                    {
                        if (control.Controls.Count > 0)
                        {
                            noPPErrors = false;
                            break;
                        }
                    }

                    if (noPPErrors)
                        errorsList.Controls.Remove(pPErrors);
                }
                #endregion
            }
            else
            {
                HyperLink hl = new HyperLink();
                hl.Text = "Related product can't be empty.";
                hl.NavigateUrl = apLink;
                hl.Target = "_blank";
                apErrors.Controls.Add(hl);
            }
            #endregion

            if (apErrors.Controls.Count == 0)
                errorsList.Controls.Remove(apErrors);
            if (productErrors.Controls.Count == 0)
                errorsList.Controls.Remove(productErrors);

            return errorsList;
        }

        #endregion
    }

    struct XEVPRMError
    {
        public string Text { get; set; }
        public string NavigateUrl { get; set; }
    }
}
