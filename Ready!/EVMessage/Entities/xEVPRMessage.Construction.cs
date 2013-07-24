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
using System.Globalization;
using System.Configuration;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using AspNetUIFramework;
using System.Text.RegularExpressions;

namespace xEVMPD
{
    /// <summary>
    /// xEVPRM - eXtended EudraVIgilance Product Report Message
    /// </summary>
    public partial class xEVPRMessage
    {
        Xevprm_message_PK xmsg = null;

        List<string> constructionErrors = new List<string>();

        IOrganization_PKOperations _organizationOperations;
        IDocument_PKOperations _documentOperations;
        IDocument_language_mn_PKOperations _documentLanguageMNOperations;
        //IType_PKOperations _typeOperations;
        ICountry_PKOperations _countryOperations;
        ILanguagecode_PKOperations _languageCodeOperations;
        IAttachment_PKOperations _attachmentOperations;
        IPerson_PKOperations _person_PKOperations;
        IQppv_code_PKOperations _qppv_code_PKOperations;

        Organization_PK organization = null;
        Document_PK document = null;
        Document_language_mn_PK documentLanguageMN = null;
        Type_PK type = null;
        Country_PK countryHolder = null;
        Languagecode_PK languageCode = null;
        Attachment_PK attachment = null;
        Product_PK product = null;
        List<Document_PK> docList = null;

        string apLink = "../AuthorisedProductView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd="; //error=0 for parsing errors
        string organisationLink = "../OrganizationView/Form.aspx?EntityContext=Organisation&Action=Edit&error=1&idOrg=";
        string personLink = "../PersonView/Form.aspx?EntityContext=Person&Action=Edit&error=1&idPerson=";
        string apDocumentLink = "../DocumentView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&error=1&idAuthProd=";
        string PPLink = "../PharmaceuticalProductView/Form.aspx?EntityContext=PharmaceuticalProduct&Action=Edit&error=1&idPharmProd=";
        string ProductLink = "../ProductView/Form.aspx?EntityContext=Product&Action=Edit&error=1&idProd=";
        public void ConstructFrom(int xevprm_message_PK)
        {
            IXevprm_message_PKOperations _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
            Xevprm_message_PK xmessage = _xevprm_message_PKOperation.GetEntity(xevprm_message_PK);

            ConstructFrom(xmessage);
        }

        public List<string> ConstructFrom(Xevprm_message_PK xmessage)
        {
            this.xmsg = xmessage;

            GenerateHeader(xmsg.message_number);

            IXevprm_ap_details_PKOperations _xevprm_ap_details_PKOperations = new Xevprm_ap_details_PKDAL();
            Xevprm_ap_details_PK messageAPDetails = _xevprm_ap_details_PKOperations.GetEntityForXevprm(xmessage.xevprm_message_PK);

            ConstructFromAuthorizedProduct((int)messageAPDetails.ap_FK, messageAPDetails.OperationType);

            return constructionErrors;//This will return errors in parsing
        }

        private void ConstructFromAuthorizedProduct(int ap_fk, XevprmOperationType operationType)
        {
            _authorizedProductOperations = new AuthorisedProductDAL();
            _organizationOperations = new Organization_PKDAL();
            _documentOperations = new Document_PKDAL();
            _documentLanguageMNOperations = new Document_language_mn_PKDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _languageCodeOperations = new Languagecode_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _productOperations = new Product_PKDAL();
            _person_PKOperations = new Person_PKDAL();
            _qppv_code_PKOperations = new Qppv_code_PKDAL();

            apDocumentLink += ap_fk + "&idDoc=";
            
            AuthorisedProduct ap = _authorizedProductOperations.GetEntity(ap_fk);

            if (ap.organizationmahcode_FK != null) // MAH
                organization = _organizationOperations.GetEntity(ap.organizationmahcode_FK);

            //constructOrganisation(ap, operationType);
            ConstructAPT(ap, operationType);

            //masterfilelocationType mfl = new masterfilelocationType();
            //Message.masterfilelocations.masterfilelocation = new masterfilelocationType[] { mfl };

            //attachmentType attachment = new attachmentType();
            //Message.attachments.attachment = new attachmentType[] { attachment };

            //sourceType src = new sourceType();
            //Message.sources.source = new sourceType[] { src };
        }

        // check operationtype for approved substance

        public void ConstructAPT(AuthorisedProduct ap, XevprmOperationType operationType)
        {
            authorisedproductType apt = new authorisedproductType();

            constructAuthorisation(apt, ap);
            constructPresentation(apt, ap);
            constructProduct(apt, ap);
            ConstructPPT(apt, ap);

            if (operationType == XevprmOperationType.Insert)
            {
                constructAttachment(apt, ap, operationType);
            }
            //constructOrganisation(ap, operationType);
            string mflEvcodeError = string.Empty;
            if (ap.mflcode_FK != null)
            {
                organization = _organizationOperations.GetEntity(ap.mflcode_FK);

                apt.mflcode = new authorisedproductType.mflcodeLocalType();
                apt.mflcode.resolutionmode = 2;
                if (organization != null && !string.IsNullOrWhiteSpace(organization.mfl_evcode))
                {
                    if (organization.mfl_evcode.Length <= 60)
                    {
                        apt.mflcode.TypedValue = organization.mfl_evcode;
                    }
                    else
                    {
                        mflEvcodeError = "Master file location EV Code can't be longer than 60 characters|" + organisationLink + ap.mflcode_FK + "&mfl=true";
                        constructionErrors.Add(mflEvcodeError);
                    }
                }
                else
                {
                    mflEvcodeError = "Master file location is missing EV Code|" + organisationLink + ap.mflcode_FK + "&mfl=true";
                    constructionErrors.Add(mflEvcodeError);
                }
                //organization = _organizationOperations.GetEntity(ap.mflcode_FK);
                //masterfilelocationType  mflType = constructMFL(organization, operationType, true);

                //if (mflType != null)
                //{
                //    Message.masterfilelocations = new evprm.masterfilelocationsLocalType();
                //    Message.masterfilelocations.masterfilelocation = new masterfilelocationType[] { mflType };

                //    apt.mflcode = new authorisedproductType.mflcodeLocalType();
                //    apt.mflcode.resolutionmode = 2;
                //    apt.mflcode.TypedValue = mflType.ev_code;
                //}
            }

            apt.operationtype = (int)operationType;

            if (operationType == XevprmOperationType.Insert)
            {
                if (ap.ap_PK.ToString().Length <= 60)
                {
                    apt.localnumber = ap.ap_PK.ToString();// Guid.NewGuid().ToString();
                }
                else
                {
                    constructionErrors.Add("Authorised product type local number can't be longer than 60 characters|" + apLink + ap.ap_PK);
                }
            }
            else if (operationType == XevprmOperationType.Nullify)
            {
                if (!string.IsNullOrWhiteSpace(ap.ev_code))
                {
                    if (ap.ev_code.Length <= 60)
                    {
                        apt.ev_code = ap.ev_code;
                    }
                    else
                    {
                        constructionErrors.Add("EVCCODE can't be longer than 60 characters.|" + apLink + ap.ap_PK);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(ap.evprm_comments))
            {
                if (ap.evprm_comments.Length <= 500)
                    apt.comments = ap.evprm_comments;
                else
                {
                    constructionErrors.Add("Comment(EVPRM) can't be longer than 500 characters.|" + apLink + ap.ap_PK);
                }
            }


            organization = _organizationOperations.GetEntity(ap.organizationmahcode_FK);
            if (organization != null)
            {
                apt.mahcode = new authorisedproductType.mahcodeLocalType();
                apt.mahcode.resolutionmode = 2;
                if (!string.IsNullOrWhiteSpace(organization.ev_code))
                {
                    if (organization.ev_code.Length <= 60)
                    {
                        apt.mahcode.TypedValue = organization.ev_code;
                    }
                    else 
                    {
                        if (!string.IsNullOrEmpty(mflEvcodeError))
                        {
                            if (constructionErrors.Remove(mflEvcodeError))
                                constructionErrors.Add(mflEvcodeError + "&mah=true");

                            constructionErrors.Add("Licence holder EV Code can't be longer than 60 characters|" + organisationLink + organization.organization_PK + "&mah=true&mfl=true");
                        }
                        else
                        {
                            constructionErrors.Add("Licence holder EV Code can't be longer than 60 characters|" + organisationLink + organization.organization_PK + "&mah=true");
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(mflEvcodeError))
                    {
                        if (constructionErrors.Remove(mflEvcodeError))
                            constructionErrors.Add(mflEvcodeError + "&mah=true");

                        constructionErrors.Add("Licence holder is missing EV Code|" + organisationLink + organization.organization_PK + "&mah=true&mfl=true");
                    }
                    else
                    {
                        constructionErrors.Add("Licence holder is missing EV Code|" + organisationLink + organization.organization_PK + "&mah=true");
                    }

                }
                if (!string.IsNullOrWhiteSpace(organization.organizationsenderid_EMEA) && organization.organizationsenderid_EMEA.Trim().Length>=3)
                {
                    Message.ichicsrmessageheader.messagesenderidentifier = organization.organizationsenderid_EMEA.Trim();
                }
                else
                {
                    String errorMsg = string.IsNullOrWhiteSpace(organization.organizationsenderid_EMEA) ? "Organisation sender ID is missing|" : "Organisation sender ID must be at least 3 characters long|";
                    if (!string.IsNullOrEmpty(mflEvcodeError))
                    {
                        if (constructionErrors.Remove(mflEvcodeError))
                            constructionErrors.Add(mflEvcodeError + "&mah=true");

                        constructionErrors.Add(errorMsg + organisationLink + organization.organization_PK + "&mah=true&mfl=true");
                    }
                    else
                    {
                        constructionErrors.Add(errorMsg + organisationLink + organization.organization_PK + "&mah=true");
                    }
                }
            }

            if (ap.qppv_code_FK != null)
            {
                Qppv_code_PK qppv = _qppv_code_PKOperations.GetEntity(ap.qppv_code_FK);

                if (qppv != null && qppv.qppv_code != null)
                {
                    if (qppv.qppv_code.Length <= 10)
                    {
                        if (ValidationHelper.IsValidInt(qppv.qppv_code) && int.Parse(qppv.qppv_code) >= 0)
                        {
                            apt.qppvcode = new authorisedproductType.qppvcodeLocalType();
                            apt.qppvcode.TypedValue = qppv.qppv_code;
                        }
                        else
                        {
                            constructionErrors.Add("Person QPPV Code must be valid positive integer|" + personLink + qppv.person_FK + "&QppvCodeId=" + qppv.qppv_code_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Person QPPV Code can't be longer than 10 characters|" + personLink + qppv.person_FK + "&QppvCodeId=" + qppv.qppv_code_PK);
                    }

                }
                else if (qppv != null && qppv.qppv_code == null)
                {
                    constructionErrors.Add("Person is missing QPPV Code|" + personLink + qppv.person_FK + "&QppvCodeId=" + qppv.qppv_code_PK);
                }
                else
                {
                    constructionErrors.Add("QPPV is missing|" + apLink + ap.ap_PK);
                }
            }
            else
            {
                constructionErrors.Add("QPPV is missing|" + apLink + ap.ap_PK);
            }

            if (!string.IsNullOrWhiteSpace(ap.phv_email)) 
                if (ap.phv_email.Length <= 100) apt.enquiryemail = ap.phv_email; else constructionErrors.Add("Enquiry Email can't be longer than 100 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.phv_phone)) 
                if (ap.phv_phone.Length <= 50) apt.enquiryphone = ap.phv_phone; else constructionErrors.Add("Enquiry Phone number can't be longer than 50 characters|" + apLink + ap.ap_PK);

            if (ap.infodate.HasValue)
            {
                apt.infodate = ap.infodate.Value.ToString("yyyyMMdd");
                apt.infodateformat = "102"; // YYYYMMDD
            }

            //apt.mflcode

            //authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType ppi = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType();
            //apt.ppiattachments.ppiattachment = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType[] { ppi };            

            Message.authorisedproducts = new evprm.authorisedproductsLocalType();
            Message.authorisedproducts.authorisedproduct = new authorisedproductType[] { apt };
        }

        masterfilelocationType constructMFL(Organization_PK org, XevprmOperationType operationType, bool isAssignedToAuthProd = false)
        {
            masterfilelocationType masterfilelocationType = null;

            if (org != null)
            {
                masterfilelocationType = new masterfilelocationType();

                if (isAssignedToAuthProd)
                {
                    if (org.mfl_evcode.Trim().Length <= 60)
                    {
                        masterfilelocationType.ev_code = org.mfl_evcode.Trim();
                    }
                    else
                    {
                        constructionErrors.Add("Organization MFL EV code can't be longer than 60 characters|"+organisationLink+org.organization_PK );
                    }
                }
                else
                {
                    masterfilelocationType.operationtype = (int)operationType;
                    if (operationType == XevprmOperationType.Insert)
                    {
                        if (org.organization_PK.ToString().Trim().Length <= 60)
                        {
                            masterfilelocationType.localnumber = org.organization_PK.ToString().Trim(); //Guid.NewGuid().ToString();
                        }
                        else
                        {
                            constructionErrors.Add("MFL local number can't be longer than 60 characters|"+organisationLink+org.organization_PK);
                        }
                    }
                    else
                    {
                        if (org.mfl_evcode.Trim().Length <= 60)
                        {
                            masterfilelocationType.ev_code = org.mfl_evcode.Trim();
                        }
                        else
                        {
                            constructionErrors.Add("Organization MFL EV code can't be longer than 60 characters|" + organisationLink + org.organization_PK);
                        }
                    }
                }

                if (!String.IsNullOrWhiteSpace(org.mflcompany))
                    if (org.mflcompany.Trim().Length <= 100) masterfilelocationType.mflcompany = org.mflcompany.Trim(); else constructionErrors.Add("Company name can't be longer than 100 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.mfldepartment))
                    if (org.mfldepartment.Trim().Length <= 100) masterfilelocationType.mfldepartment = org.mfldepartment.Trim(); else constructionErrors.Add("Department name can't be longer than 100 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.mflbuilding))
                    if (org.mflbuilding.Trim().Length <= 100) masterfilelocationType.mflbuilding = org.mflbuilding.Trim(); else constructionErrors.Add("Building name can't be longer than 100 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.address))
                    if (org.address.Trim().Length <= 100) masterfilelocationType.mflstreet = org.address.Trim(); else constructionErrors.Add("Organisation name can't be longer than 100 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.city))
                    if (org.city.Trim().Length <= 35) masterfilelocationType.mflcity = org.city.Trim(); else constructionErrors.Add("City can't be longer than 35 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.state))
                    if (org.state.Trim().Length <= 40) masterfilelocationType.mflstate = org.state.Trim(); else constructionErrors.Add("State can't be longer than 40 characters|" + organisationLink + org.organization_PK);
                if (!String.IsNullOrWhiteSpace(org.postcode))
                    if (org.postcode.Trim().Length <= 15) masterfilelocationType.mflpostcode = org.postcode.Trim(); else constructionErrors.Add("Postal code can't be longer than 15 characters|" + organisationLink + org.organization_PK);
                
                if (org.countrycode_FK.HasValue)
                {
                    countryHolder = _countryOperations.GetEntity(org.countrycode_FK);
                    if (countryHolder != null)
                        if (countryHolder.abbreviation.Trim().Length == 2) masterfilelocationType.mflcountrycode = countryHolder.abbreviation.Trim(); else constructionErrors.Add("Country code must be exactly 2 characters long.|" + organisationLink + org.organization_PK);
                }
                if (!String.IsNullOrWhiteSpace(org.comment))
                    if (org.comment.Trim().Length <= 500) masterfilelocationType.comments = org.comment.Trim(); else constructionErrors.Add("Comment can't be longer than 500 characters|" + organisationLink + org.organization_PK);
            }

            return masterfilelocationType;


        }

        void constructOrganisation(AuthorisedProduct ap, XevprmOperationType operationType)
        {
            organisationType org = null;
            if (organization != null)
            {
                org = new organisationType();
                org.operationtype = (int)operationType;
                if (ap.organizationmahcode_FK != null) org.type_org = (int)xEVPRM_OrganizationType.MAH;
                //else if (ap.organizationsponsorcode_FK != null) org.type_org = (int)xEVPRM_OrganizationType.MAH; // TODO: implement Sponsor type
                if (organization.name_org != null)
                     if (organization.name_org.Trim().Length <= 100) org.name_org = organization.name_org.Trim();  else constructionErrors.Add("Organisation name can't be longer than 100 characters|" + organisationLink + organization.organization_PK);

                if (operationType == XevprmOperationType.Insert)
                {
                    //localnumber and ev_code must not be longer than 60 chars. ()
                    org.localnumber = Guid.NewGuid().ToString(); // mandatory for xEVPRM_OrganizationType.MAH
                    org.ev_code = String.Empty;
                }
                else
                {
                    if (operationType == XevprmOperationType.Update || operationType == XevprmOperationType.Nullify) org.ev_code = organization.ev_code;
                    else org.ev_code = String.Empty;
                }
                org.organisationsenderid = "BPET"; // TODO: implement mapping ORG <-> SenderIDs
                if (!String.IsNullOrWhiteSpace(organization.address))
                    if (organization.address.Trim().Length <= 100) org.address = organization.address.Trim();  else constructionErrors.Add("Organisation adress can't be longer than 100 characters|" + organisationLink + organization.organization_PK);
                if (!String.IsNullOrWhiteSpace(organization.city))
                    if (organization.city.Trim().Length <= 50) org.city = organization.city.Trim(); else constructionErrors.Add("Organisation city name can't be longer than 50 characters|" + organisationLink + organization.organization_PK);
                if (!String.IsNullOrWhiteSpace(organization.state))
                    if (organization.state.Trim().Length <= 50) org.state = organization.state.Trim(); else constructionErrors.Add("Organisation state name can't be longer than 50 characters|" + organisationLink + organization.organization_PK); 
                if (!String.IsNullOrWhiteSpace(organization.postcode))
                    if (organization.postcode.Trim().Length <= 50) org.postcode = organization.postcode.Trim(); else constructionErrors.Add("Organisation post code can't be longer than 50 characters|" + organisationLink + organization.organization_PK); 
                if (organization.countrycode_FK.HasValue)
                {
                    countryHolder = _countryOperations.GetEntity(organization.countrycode_FK);
                    if (countryHolder != null)
                        if (org.countrycode.Trim().Length <= 2) org.countrycode = countryHolder.abbreviation.Trim(); else constructionErrors.Add("Organisation country code can't be longer than 2 characters|" + organisationLink + organization.organization_PK); 
                }

                if (organization.tel_number != null) if (organization.tel_number.Trim().Length <= 50) org.tel_number = organization.tel_number.Trim(); else constructionErrors.Add("Organisation telephone number can't be longer than 50 characters|" + organisationLink + organization.organization_PK);
                if (organization.tel_extension != null) if (organization.tel_extension.Trim().Length <= 50) org.tel_extension = organization.tel_extension.Trim(); else constructionErrors.Add("Organisation telephone extension can't be longer than 50 characters|" + organisationLink + organization.organization_PK);
                if (organization.tel_countrycode != null) if (organization.tel_countrycode.Trim().Length <= 5) org.tel_countrycode = organization.tel_countrycode.Trim(); else constructionErrors.Add("Organisation telephone country code can't be longer than 5 characters|" + organisationLink + organization.organization_PK);
                if (organization.fax_number != null) if (organization.fax_number.Trim().Length <= 50) org.fax_number = organization.fax_number.Trim(); else constructionErrors.Add("Organisation fax number can't be longer than 50 characters|" + organisationLink + organization.organization_PK);
                if (organization.fax_extenstion != null) if (organization.fax_extenstion.Trim().Length <= 50) org.fax_extension = organization.fax_extenstion.Trim(); else constructionErrors.Add("Organisation fax extension can't be longer than 50 characters|" + organisationLink + organization.organization_PK);
                if (organization.fax_countrycode != null) if (organization.fax_countrycode.Trim().Length <= 5) org.fax_countrycode = organization.fax_countrycode.Trim(); else constructionErrors.Add("Organisation fax country code can't be longer than 5 characters|" + organisationLink + organization.organization_PK);

                if (organization.email != null) if (organization.email.Trim().Length <= 100) org.email = organization.email.Trim(); else constructionErrors.Add("Organisation email can't be longer than 100 characters|" + organisationLink + organization.organization_PK);
                if (organization.comment != null) if (organization.comment.Trim().Length <= 500) org.comments = organization.comment.Trim(); else constructionErrors.Add("Comment can't be longer than 500 characters|" + organisationLink + organization.organization_PK);
                
                // mandatory if XevprmOperationType.Nullify  
                if (operationType == XevprmOperationType.Nullify && String.IsNullOrWhiteSpace(organization.comment))
                    constructionErrors.Add("Comment is mandatory when operation type is Nullifiy|" + organisationLink + organization.organization_PK);
                  
            }
            if (org != null)
            {
                Message.organisations = new evprm.organisationsLocalType();
                Message.organisations.organisation = new organisationType[] { org };
            }
        }

        void constructAttachment(authorisedproductType apt, AuthorisedProduct ap, XevprmOperationType operationType)
        {
            attachmentType attachmentType = null;

            // check for PPI on AP
            docList = _documentOperations.GetDocumentsByAP((int)ap.ap_PK);

            foreach (Document_PK document in docList)
            {
                Type_PK typeDoc = _typeOperations.GetEntity(document.type_FK);
                if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
                {
                    attachmentType = fillAttachmentType(document, operationType);
                    break;
                }
            }

            // check for PPI on Product
            //if (attachmentType == null)
            //{
            //    docList = _documentOperations.GetDocumentsByProduct((int)ap.product_FK);

            //    if (docList.Count > 0)
            //    {
            //        Document_PK doc = docList.Find(doc1 => _typeOperations.GetEntity(doc1.type_FK).name.Trim().ToLower() == "ppi");
            //        if (doc != null)
            //        {
            //            attachmentType = fillAttachmentType(doc, operationType);
            //        }
            //    }
            //}

            if (attachmentType != null)
            {
                List<authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType> ppiAttachmentTypeList = new List<authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType>();
                authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType ppiAttachmentType = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType();
                ppiAttachmentType.attachmentcode = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType.attachmentcodeLocalType();

                if (attachment != null && !string.IsNullOrWhiteSpace(attachment.ev_code))
                {
                    if (attachment.ev_code.Trim().Length <= 60)
                    {
                        ppiAttachmentType.attachmentcode.resolutionmode = 2;
                        ppiAttachmentType.attachmentcode.TypedValue = attachment.ev_code.Trim();
                    }
                    else
                    {
                        constructionErrors.Add("Attachment EV code can't be longer than 60 characters|" + apDocumentLink + attachment.attachment_PK);
                    }
                }
                else
                {
                    Message.attachments = new evprm.attachmentsLocalType();
                    Message.attachments.attachment = new attachmentType[] { attachmentType };
                    if (attachmentType.localnumber.Trim().Length <= 60)
                    {
                        ppiAttachmentType.attachmentcode.resolutionmode = 1;
                        ppiAttachmentType.attachmentcode.TypedValue = attachmentType.localnumber;
                    }
                    else
                    {
                        constructionErrors.Add("Attachment code can't be longer than 60 characters|" + apDocumentLink + attachment.attachment_PK);
                    }
                }
                ppiAttachmentType.validitydeclaration = 1;

                ppiAttachmentTypeList.Add(ppiAttachmentType);

                apt.ppiattachments = new authorisedproductType.ppiattachmentsLocalType();
                apt.ppiattachments.ppiattachment = ppiAttachmentTypeList.ToArray();
            }
        }

        private attachmentType fillAttachmentType(Document_PK doc, XevprmOperationType operationType)
        {
            attachmentType attachmentType = null;

            if (doc != null)
            {
                List<Document_language_mn_PK> documentLanguageMNList = _documentLanguageMNOperations.GetLanguagesByDocument(doc.document_PK);
                if (documentLanguageMNList != null && documentLanguageMNList.Count > 0) documentLanguageMN = documentLanguageMNList.FirstOrDefault();

                List<Attachment_PK> attachList = _attachmentOperations.GetAttachmentsForDocument((int)doc.document_PK);
                if (attachList != null && attachList.Count > 0) attachment = attachList.FirstOrDefault();

                if (attachment != null)
                {
                    attachmentType = new attachmentType();
                    attachmentType.operationtype = (int)operationType;

                    attachmentType.localnumber = attachment.attachment_PK.ToString().Trim(); //Guid.NewGuid().ToString();

                    if (!String.IsNullOrWhiteSpace(attachment.attachmentname))
                    {
                        if (attachment.attachmentname.Trim().Length <= 200)
                        {
                            attachmentType.filename = attachment.attachmentname.Trim(); // with extension
                            //attachmentType.attachmentname = Path.GetFileNameWithoutExtension(attachment.attachmentname);
                        }
                        else
                        {
                            constructionErrors.Add("Attachment file name can't be longer than 200 characters|" + apDocumentLink + doc.document_PK);
                        }
                        
                    }

                    if (!String.IsNullOrWhiteSpace(doc.name))
                    {
                        if (doc.name.Trim().Length <= 2000)
                        {
                            attachmentType.attachmentname = doc.name.Trim();
                        }
                        else 
                        {
                            constructionErrors.Add("Document name can't be longer than 2000 characters|" + apDocumentLink + doc.document_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Document name is missing|" + apDocumentLink + doc.document_PK);
                    }

                    //if (!String.IsNullOrWhiteSpace(doc.attachment_name)) attachmentType.attachmentname = doc.name;
                    attachmentType.attachmenttype1 = (int)xEVPRM_AttachmentType.PPI;

                    Type_PK type = _typeOperations.GetEntity(doc.attachment_type_FK);

                    if (type != null && !string.IsNullOrWhiteSpace(type.type))
                    {
                        int fileType = 0;
                        if (int.TryParse(type.type, out fileType))
                        {
                            if (!String.IsNullOrWhiteSpace(attachment.attachmentname))
                            {
                                string extension = Path.GetExtension(attachment.attachmentname);

                                if (!string.IsNullOrWhiteSpace(extension) &&
                                    extension.ToLower().Trim() == "." + type.name.ToLower().Trim())
                                {
                                    attachmentType.filetype = fileType;
                                }
                                else
                                {
                                    constructionErrors.Add("Specified attachment type doesn't match extension of uploaded attachment|" + apDocumentLink + doc.document_PK);
                                }
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Attachment file type is not in valid format|" + apDocumentLink + doc.document_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Attachment file type is missing|" + apDocumentLink + doc.document_PK);
                    }

                    if (documentLanguageMN != null)
                    {
                        languageCode = _languageCodeOperations.GetEntity(documentLanguageMN.language_FK);
                        if (languageCode != null)
                            if (languageCode.code.Trim().Length <= 2)
                                attachmentType.languagecode = languageCode.code.Trim();
                            else constructionErrors.Add("Attachment language code can't be longer than 2 characters|" + apDocumentLink + doc.document_PK);
                    }

                    type = _typeOperations.GetEntity(doc.version_number);

                    if (type != null && !string.IsNullOrWhiteSpace(type.name))
                    {
                        if (type.name.Trim().Length <=5)
                            attachmentType.attachmentversion = type.name.Trim();
                        else constructionErrors.Add("Attachment version can't be longer than 5 characters|" + apDocumentLink + doc.document_PK);
                    }

                    if (doc.version_date.HasValue) attachmentType.attachmentversiondate = doc.version_date.Value.ToString("yyyyMMdd").Trim();
                    attachmentType.versiondateformat = "102"; // YYYYMMDD

                }
            }

            return attachmentType;
        }


        void constructProduct(authorisedproductType apt, AuthorisedProduct ap)
        {
            product = _productOperations.GetEntity(ap.product_FK);
            if (product != null)
            {
                List<authorisedproductType.productatcsLocalType.productatcLocalType> ATCs = new List<authorisedproductType.productatcsLocalType.productatcLocalType>();

                _productATCMNOperations = new Product_atc_mn_PKDAL();
                DataSet ds = _productATCMNOperations.GetATCByProduct(product.product_PK);
                DataTable dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (dt.Columns.Contains("atccode"))
                        {
                            authorisedproductType.productatcsLocalType.productatcLocalType atc = new authorisedproductType.productatcsLocalType.productatcLocalType();
                            atc.atccode = new authorisedproductType.productatcsLocalType.productatcLocalType.atccodeLocalType();
                            if (item["atccode"] != null && item["atccode"].ToString().Length >= 1 && item["atccode"].ToString().Length <= 10)
                            {
                                atc.atccode.TypedValue = item["atccode"].ToString().Trim();
                                atc.atccode.resolutionmode = 2;
                                ATCs.Add(atc);
                            }
                            else if (item["atccode"] != null)
                            {
                                constructionErrors.Add("Product ATC code is not in valid format|"+ProductLink + product.product_PK);
                            }
                        }
                    }
                    apt.productatcs = new authorisedproductType.productatcsLocalType();
                    apt.productatcs.productatc = ATCs.ToArray();
                }
            }



            if (ap != null)
            {
                List<authorisedproductType.productindicationsLocalType.productindicationLocalType> indications = new List<authorisedproductType.productindicationsLocalType.productindicationLocalType>();

                _indicationOperations = new Meddra_ap_mn_PKDAL();
                DataSet ds = _indicationOperations.GetMEDDRAByAP(ap.ap_PK);
                DataTable dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        authorisedproductType.productindicationsLocalType.productindicationLocalType ind = new authorisedproductType.productindicationsLocalType.productindicationLocalType();
                        if (dt.Columns.Contains("code") && !string.IsNullOrWhiteSpace((string)item["code"]))
                        {
                            int code = 0;
                            if (int.TryParse(item["code"].ToString(), out code))
                            {
                                try
                                {
                                    ind.meddracode = code;
                                }
                                catch { constructionErrors.Add("MEDDRA code is not in valid format|" + apLink + ap.ap_PK); }
                            }
                            else
                            {
                                constructionErrors.Add("MEDDRA code (" + item["code"].ToString() + ") is not in valid format|" + apLink + ap.ap_PK);
                            }
                        }
                        if (dt.Columns.Contains("level_name") && !string.IsNullOrWhiteSpace((string)item["level_name"]))
                        {
                            try
                            {
                                ind.meddralevel = item["level_name"].ToString();
                            }
                            catch { constructionErrors.Add("MEDDRA level name is not in valid format|" + apLink + ap.ap_PK); }
                        }
                        if (dt.Columns.Contains("version_name") && !string.IsNullOrWhiteSpace((string)item["version_name"]))
                        {
                            decimal version = 0;
                            if (decimal.TryParse(item["version_name"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out version))
                            {
                                try
                                {
                                    ind.meddraversion = version;
                                }
                                catch { constructionErrors.Add("MEDDRA version is not in valid format|" + apLink + ap.ap_PK); }
                            }
                        }
                        indications.Add(ind);
                    }
                    apt.productindications = new authorisedproductType.productindicationsLocalType();
                    apt.productindications.productindication = indications.ToArray();
                }
            }
        }


        void constructPresentation(authorisedproductType apt, AuthorisedProduct ap)
        {
            apt.presentationname = new authorisedproductType.presentationnameLocalType();

            if (!string.IsNullOrWhiteSpace(ap.packagedesc))
                if (ap.packagedesc.Trim().Length <= 2000) apt.presentationname.packagedesc = ap.packagedesc; else constructionErrors.Add("Package description can't be longer than 2000 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.product_name))
                if (ap.product_name.Trim().Length <= 2000) apt.presentationname.productname = ap.product_name.Trim(); else constructionErrors.Add("Product name can't be longer than 2000 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.productshortname))
                if (ap.productshortname.Trim().Length <= 500) apt.presentationname.productshortname = ap.productshortname.Trim(); else constructionErrors.Add("Product short name can't be longer than 500 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.productcompanyname))
                if (ap.productcompanyname.Trim().Length <= 250) apt.presentationname.productcompanyname = ap.productcompanyname.Trim(); else constructionErrors.Add("Product company name can't be longer than 250 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.productgenericname))
                if (ap.productgenericname.Trim().Length <= 1000) apt.presentationname.productgenericname = ap.productgenericname.Trim(); else constructionErrors.Add("Product generic name can't be longer than 1000 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.productform))
                if (ap.productform.Trim().Length <= 500) apt.presentationname.productform = ap.productform.Trim(); else constructionErrors.Add("Product form name can't be longer than 500 characters|" + apLink + ap.ap_PK);
            if (!string.IsNullOrWhiteSpace(ap.productstrenght))
                if (ap.productstrenght.Trim().Length <= 250) apt.presentationname.productstrength = ap.productstrenght.Trim(); else constructionErrors.Add("Product strength can't be longer than 250 characters|" + apLink + ap.ap_PK);
        }


        void constructAuthorisation(authorisedproductType apt, AuthorisedProduct ap)
        {
            authorisedproductType.authorisationLocalType authorisedproductTypeAuthorisation = new authorisedproductType.authorisationLocalType();

            if (ap.authorisationcountrycode_FK.HasValue)
            {
                countryHolder = _countryOperations.GetEntity(ap.authorisationcountrycode_FK);
                if (countryHolder != null)
                    if (countryHolder.abbreviation.Trim().Length <= 2) authorisedproductTypeAuthorisation.authorisationcountrycode = countryHolder.abbreviation.Trim(); else constructionErrors.Add("Country code can't be longer than 2 characters|" + apLink + ap.ap_PK);
            }

            product = _productOperations.GetEntity(ap.product_FK);
            if (product != null)
            {
                type = _typeOperations.GetEntity(product.authorisation_procedure);
                if (type != null)
                {
                    int authProcedure = 0;
                    if (int.TryParse(type.ev_code, out authProcedure))
                    {
                        authorisedproductTypeAuthorisation.authorisationprocedure = authProcedure;
                    }
                }
            }

            type = _typeOperations.GetEntity(ap.authorisationstatus_FK);
            if (type != null)
            {
                int authStatus = 0;
                if (int.TryParse(type.ev_code, out authStatus))
                {
                    authorisedproductTypeAuthorisation.authorisationstatus = authStatus;
                }
            }

            if (!string.IsNullOrWhiteSpace(ap.authorisationnumber)) 
                if (ap.authorisationnumber.Trim().Length <= 100) 
                    authorisedproductTypeAuthorisation.authorisationnumber = ap.authorisationnumber.Trim();
                else constructionErrors.Add("Authorisation number can't be longer than 100 characters|" + apLink + ap.ap_PK);

            if (ap.authorisationdate != null && ap.authorisationdate <= DateTime.Now.AddHours(12))
                authorisedproductTypeAuthorisation.authorisationdate = ((DateTime)ap.authorisationdate).ToString("yyyyMMdd");
            else if (ap.authorisationdate > DateTime.Now.AddHours(12))
            {
                constructionErrors.Add("Authorisation date can't be in the future|" + apLink + ap.ap_PK);
            }
            authorisedproductTypeAuthorisation.authorisationdateformat = "102"; // YYYYMMDD

            if (product != null)
            {
                type = _typeOperations.GetEntity(product.authorisation_procedure);
                if (type != null && type.name.ToLower().Contains("centralised") && !type.name.ToLower().Contains("decentralised"))
                {
                    if (!string.IsNullOrWhiteSpace(product.product_number))
                        if (product.product_number.Trim().Length <= 50) authorisedproductTypeAuthorisation.mrpnumber = product.product_number.Trim(); else constructionErrors.Add("Product number can't be longer than 50 characters|" + ProductLink + product.product_PK);
                    if (!string.IsNullOrWhiteSpace(ap.authorisationnumber))
                        if (ap.authorisationnumber.Trim().Length <= 50) authorisedproductTypeAuthorisation.eunumber = ap.authorisationnumber.Trim(); else constructionErrors.Add("Authorisation number can't be longer than 50 characters|" + ProductLink + product.product_PK);
                }
                else if (type != null && (type.name.ToLower().Contains("mutual") || type.name.ToLower().Contains("decentralised")))
                {
                    if (!string.IsNullOrWhiteSpace(product.product_number))
                        if (product.product_number.Trim().Length <= 50) authorisedproductTypeAuthorisation.mrpnumber = product.product_number.Trim(); else constructionErrors.Add("Product number can't be longer than 50 characters|" + ProductLink + product.product_PK);
                }
                else if (type != null && type.name.ToLower().Contains("national"))
                {
                    if (!string.IsNullOrWhiteSpace(product.product_number))
                        if (product.product_number.Trim().Length <= 50) authorisedproductTypeAuthorisation.mrpnumber = product.product_number.Trim(); else constructionErrors.Add("Product number can't be longer than 50 characters|" + ProductLink + product.product_PK);
                }

                if (product.orphan_drug == null) authorisedproductTypeAuthorisation.orphandrug = 2;
                else if ((bool)product.orphan_drug) authorisedproductTypeAuthorisation.orphandrug = 1;
                else if (!((bool)product.orphan_drug)) authorisedproductTypeAuthorisation.orphandrug = 2;

                if (product.intensive_monitoring != null) authorisedproductTypeAuthorisation.intensivemonitoring = product.intensive_monitoring;
            }
            if (ap.authorisationwithdrawndate != null)
            {
                authorisedproductTypeAuthorisation.withdrawndate = ((DateTime)ap.authorisationwithdrawndate).ToString("yyyyMMdd");
                authorisedproductTypeAuthorisation.withdrawndateformat = "102"; // YYYYMMDD
            }

            apt.authorisation = authorisedproductTypeAuthorisation;
        }


        void ConstructPPT(authorisedproductType apt, AuthorisedProduct ap)
        {
            IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            IActiveingredient_PKOperations _activeIngredieent_PKOperations;
            _activeIngredieent_PKOperations = new Activeingredient_PKDAL();
            ISubstance_PKOperations _substance_PKOperations;
            _substance_PKOperations = new Substance_PKDAL();


            List<Pharmaceutical_product_PK> pplist = ap.product_FK != null ? _pharmaceuticalProductOperations.GetPPForProduct(ap.product_FK) : new List<Pharmaceutical_product_PK>();
            List<pharmaceuticalproductType> pptlist = new List<pharmaceuticalproductType>();
            foreach (Pharmaceutical_product_PK pp in pplist)
            {
                pharmaceuticalproductType ppt = new pharmaceuticalproductType();

                ppt.pharmformcode = new pharmaceuticalproductType.pharmformcodeLocalType();

                IPharmaceutical_form_PKOperations _pharmaceutical_form_PKOperations = new Pharmaceutical_form_PKDAL();
                Pharmaceutical_form_PK pharmForm = _pharmaceutical_form_PKOperations.GetEntity(pp.Pharmform_FK);
                if (pharmForm != null)
                {
                    if (!string.IsNullOrWhiteSpace(pharmForm.ev_code))
                    {
                        if (pharmForm.ev_code.Trim().Length <= 60)
                        {
                            ppt.pharmformcode.TypedValue = pharmForm.ev_code.Trim();
                            ppt.pharmformcode.resolutionmode = 2;
                        }
                        else 
                        {
                            constructionErrors.Add("Pharmaceutical form EV Code can't be longer than 60 characters.|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Pharmaceutical form is missing EV Code|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                }
                else
                {
                    constructionErrors.Add("Pharmaceutical form is missing EV Code|" + PPLink + pp.pharmaceutical_product_PK);
                }

                #region Administration routes
                IAdminroute_PKOperations _adminroute_PKOperations = new Adminroute_PKDAL();
                IPp_ar_mn_PKOperations _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

                List<Pp_ar_mn_PK> ppArList = _pp_ar_mn_PKOperations.GetAdminRoutesByPPPK(pp.pharmaceutical_product_PK);
                List<pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType> adminRoutesTypeList = new List<pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType>();
                foreach (var item in ppArList)
                {
                    Adminroute_PK adminRoute = _adminroute_PKOperations.GetEntity(item.admin_route_FK);
                    if (adminRoute != null && !string.IsNullOrWhiteSpace(adminRoute.ev_code))
                    {
                        if (adminRoute.ev_code.Trim().Length <= 60)
                        {
                            pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType adminRouteType = new pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType();
                            adminRouteType.adminroutecode = new pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType.adminroutecodeLocalType();
                            adminRouteType.adminroutecode.TypedValue = adminRoute.ev_code.Trim();
                            adminRouteType.adminroutecode.resolutionmode = 2;
                            adminRoutesTypeList.Add(adminRouteType);
                        }
                        else 
                        {
                            constructionErrors.Add("Administration route EV Code can't be longer than 60 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else if (adminRoute != null)
                    {
                        constructionErrors.Add("Administration route is missing EV Code|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                }

                ppt.adminroutes = new pharmaceuticalproductType.adminroutesLocalType();
                ppt.adminroutes.adminroute = adminRoutesTypeList.ToArray();
                #endregion

                #region Medical devices
                IMedicaldevice_PKOperations _medicaldevice_PKOperations = new Medicaldevice_PKDAL();
                IPp_md_mn_PKOperations _pp_md_mn_PKOperations = new Pp_md_mn_PKDAL();

                List<Pp_md_mn_PK> ppMdList = _pp_md_mn_PKOperations.GetMedDevicesByPPPK(pp.pharmaceutical_product_PK);

                if (ppMdList.Count > 0)
                {
                    List<pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType>
                        medicalDevicesTypeList =
                            new List<pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType>();

                    foreach (var item in ppMdList)
                    {
                        Medicaldevice_PK medicalDevice = _medicaldevice_PKOperations.GetEntity(item.pp_medical_device_FK);
                        if (medicalDevice != null && ValidationHelper.IsValidInt(medicalDevice.ev_code))
                        {
                            pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType medicalDeviceType =
                                new pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType();

                            medicalDeviceType.medicaldevicecode = Int32.Parse(medicalDevice.ev_code);
                            medicalDevicesTypeList.Add(medicalDeviceType);
                        }
                        else if (medicalDevice != null)
                        {
                            constructionErrors.Add("Medical device code is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                    }

                    ppt.medicaldevices = new pharmaceuticalproductType.medicaldevicesLocalType();
                    ppt.medicaldevices.medicaldevice = medicalDevicesTypeList.ToArray();
                }

                #endregion

                #region Active ingredients
                ppt.activeingredients = new pharmaceuticalproductType.activeingredientsLocalType();

                List<Activeingredient_PK> ailist = _activeIngredieent_PKOperations.GetIngredientsByPPPK(pp.pharmaceutical_product_PK);
                List<pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType> pptaiList = new List<pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType>();
                foreach (Activeingredient_PK ai in ailist)
                {
                    var pptai = new pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType();

                    Substance_PK substance = _substance_PKOperations.GetEntity(ai.substancecode_FK);

                    if (substance != null)
                    {
                        pptai.substancecode = new pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType.substancecodeLocalType();
                        pptai.substancecode.resolutionmode = 2;
                        
                        if (!string.IsNullOrWhiteSpace(substance.ev_code))
                        {
                            if (substance.ev_code.Trim().Length <= 60)
                            {
                                pptai.substancecode.TypedValue = substance.ev_code;
                            }
                            else
                            {
                                constructionErrors.Add("Substance code EV Code can't be longer than 60 characters|" + PPLink + pp.pharmaceutical_product_PK);
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Substance code is missing EV Code|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }

                    ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
                    Ssi__cont_voc_PK contrationTypeCode = _ssi__cont_voc_PKOperations.GetEntity(ai.concentrationtypecode);
                    if (contrationTypeCode != null && !string.IsNullOrWhiteSpace(contrationTypeCode.Evcode))
                        pptai.concentrationtypecode = contrationTypeCode.Evcode;
                    else
                    {
                        constructionErrors.Add("Concentration type code is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                        continue;
                    }

                    if (ai.lowamountnumervalue.HasValue)
                    {
                        pptai.lowamountnumervalue = ai.lowamountnumervalue.Value;
                    }
                    else
                    {
                        constructionErrors.Add("Low amount numerator value is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    if (ai.lowamountdenomvalue.HasValue)
                    {
                        pptai.lowamountdenomvalue = ai.lowamountdenomvalue.Value;
                    }
                    else
                    {
                        constructionErrors.Add("Low amount denominator value is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    if (ai.highamountnumervalue.HasValue && contrationTypeCode.Evcode == "2")
                    {
                        pptai.highamountnumervalue = ai.highamountnumervalue.Value;
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount numerator value is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    if (ai.highamountdenomvalue.HasValue && contrationTypeCode.Evcode == "2")
                    {
                        pptai.highamountdenomvalue = ai.highamountdenomvalue.Value;
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount denominator value is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    Ssi__cont_voc_PK prefix;
                    if (ValidationHelper.IsValidInt(ai.lowamountnumerprefix))
                    {
                        prefix = _ssi__cont_voc_PKOperations.GetEntity(ai.lowamountnumerprefix);
                        if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                        {
                            pptai.lowamountnumerprefix = prefix.term_name_english.Trim();
                        }
                        else if (prefix != null)
                        {
                            constructionErrors.Add("Low amount numerator prefix can't be longer than 12 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Low amount numerator prefix is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                    if (ValidationHelper.IsValidInt(ai.lowamountdenomprefix))
                    {
                        prefix = _ssi__cont_voc_PKOperations.GetEntity(ai.lowamountdenomprefix);
                        if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                        {
                            pptai.lowamountdenomprefix = prefix.term_name_english.Trim();
                        }
                        else if (prefix != null)
                        {
                            constructionErrors.Add("Low amount denominator prefix is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Low amount denominator prefix is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                    if (ValidationHelper.IsValidInt(ai.highamountnumerprefix) && contrationTypeCode.Evcode == "2")
                    {
                        prefix = _ssi__cont_voc_PKOperations.GetEntity(ai.highamountnumerprefix);
                        if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                        {
                            pptai.highamountnumerprefix = prefix.term_name_english.Trim();
                        }
                        else if (prefix != null)
                        {
                            constructionErrors.Add("High amount numerator prefix can't be longer than 12 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount numerator prefix is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    } 
                    if (ValidationHelper.IsValidInt(ai.highamountdenomprefix) && contrationTypeCode.Evcode == "2")
                    {
                        prefix = _ssi__cont_voc_PKOperations.GetEntity(ai.highamountdenomprefix);
                        if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                        {
                            pptai.highamountdenomprefix = prefix.term_name_english;
                        }
                        else if (prefix != null)
                        {
                            constructionErrors.Add("High amount denominator prefix can't be longer than 12 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount denominator prefix is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    Ssi__cont_voc_PK unit;
                    if (ValidationHelper.IsValidInt(ai.lowamountnumerunit))
                    {
                        unit = _ssi__cont_voc_PKOperations.GetEntity(ai.lowamountnumerunit);
                        if (unit != null && unit.term_name_english.Trim().Length <= 70)
                        {
                            pptai.lowamountnumerunit = unit.term_name_english.Trim();
                        }
                        else if (unit != null)
                        {
                            constructionErrors.Add("Low amount numerator unit can't be longer than 70 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Low amount numerator unit is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                    if (ValidationHelper.IsValidInt(ai.lowamountdenomunit))
                    {
                        unit = _ssi__cont_voc_PKOperations.GetEntity(ai.lowamountdenomunit);
                        if (unit != null && unit.term_name_english.Trim().Length <= 70)
                        {
                            pptai.lowamountdenomunit = unit.term_name_english.Trim();
                        }
                        else if (unit != null)
                        {
                            constructionErrors.Add("Low amount denominator unit can't be longer than 70 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else
                    {
                        constructionErrors.Add("Low amount denominator unit is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                    if (ValidationHelper.IsValidInt(ai.highamountnumerunit) && contrationTypeCode.Evcode == "2")
                    {
                        unit = _ssi__cont_voc_PKOperations.GetEntity(ai.highamountnumerunit);
                        if (unit != null && unit.term_name_english.Trim().Length <= 70)
                        {
                            pptai.highamountnumerunit = unit.term_name_english.Trim();
                        }
                        else if (unit != null)
                        {
                            constructionErrors.Add("High amount numerator unit can't be longer than 70 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount numerator unit is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }
                    if (ValidationHelper.IsValidInt(ai.highamountdenomunit) && contrationTypeCode.Evcode == "2")
                    {
                        unit = _ssi__cont_voc_PKOperations.GetEntity(ai.highamountdenomunit);
                        if (unit != null && unit.term_name_english.Trim().Length <= 70)
                        {
                            pptai.highamountdenomunit = unit.term_name_english.Trim();
                        }
                        else if (unit != null)
                        {
                            constructionErrors.Add("High amount denominator unit can't be longer than 70 characters|" + PPLink + pp.pharmaceutical_product_PK);
                        }
                    }
                    else if (contrationTypeCode.Evcode == "2")
                    {
                        constructionErrors.Add("High amount denominator unit is not in valid format|" + PPLink + pp.pharmaceutical_product_PK);
                    }

                    pptaiList.Add(pptai);
                }
                ppt.activeingredients.activeingredient = pptaiList.ToArray();
                #endregion

                #region Adjuvants

                IAdjuvant_PKOperations _adjuvant_PKOperations = new Adjuvant_PKDAL();

                List<Adjuvant_PK> adjuvantList = _adjuvant_PKOperations.GetAdjuvantsByPPPK(pp.pharmaceutical_product_PK);

                if (adjuvantList.Count > 0)
                {
                    ppt.adjuvants = new pharmaceuticalproductType.adjuvantsLocalType();

                    var adjuvantTypeList = new List<pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType>();

                    foreach (Adjuvant_PK adjuvant in adjuvantList)
                    {
                        var adjuvantType = new pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType();

                        Substance_PK substance = _substance_PKOperations.GetEntity(adjuvant.substancecode_FK);

                        if (substance != null)
                        {
                            adjuvantType.substancecode =
                                new pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType.
                                    substancecodeLocalType();
                            adjuvantType.substancecode.resolutionmode = 2;

                            if (!string.IsNullOrWhiteSpace(substance.ev_code))
                            {
                                if (substance.ev_code.Trim().Length <= 60)
                                {
                                    adjuvantType.substancecode.TypedValue = substance.ev_code.Trim();
                                }
                                else
                                { 
                                    constructionErrors.Add("Substance code EV Code can't be longer than 60 characters|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                                }  
                            }
                            else
                            {
                                constructionErrors.Add("Substance code is missing EV Code|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                        }

                        ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

                        Ssi__cont_voc_PK contrationTypeCode =
                            _ssi__cont_voc_PKOperations.GetEntity(adjuvant.concentrationtypecode);
                        if (contrationTypeCode != null && !string.IsNullOrWhiteSpace(contrationTypeCode.Evcode) && ValidationHelper.IsValidInt(contrationTypeCode.Evcode))
                            adjuvantType.concentrationtypecode = contrationTypeCode.Evcode;
                        else
                        {
                            constructionErrors.Add("Concentration type code is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            continue;
                        }

                        if (adjuvant.lowamountnumervalue.HasValue)
                        {
                            adjuvantType.lowamountnumervalue = adjuvant.lowamountnumervalue.Value;
                        }
                        else
                        {
                            constructionErrors.Add("Low amount numerator value is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        if (adjuvant.lowamountdenomvalue.HasValue)
                        {
                            adjuvantType.lowamountdenomvalue = adjuvant.lowamountdenomvalue.Value;
                        }
                        else
                        {
                            constructionErrors.Add("Low amount denominator value is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        if (adjuvant.highamountnumervalue.HasValue && contrationTypeCode.Evcode == "2")
                        {
                            adjuvantType.highamountnumervalue = adjuvant.highamountnumervalue.Value;
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount numerator value is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        if (adjuvant.highamountdenomvalue.HasValue && contrationTypeCode.Evcode == "2")
                        {
                            adjuvantType.highamountdenomvalue = adjuvant.highamountdenomvalue.Value;
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount denominator value is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        Ssi__cont_voc_PK prefix;
                        if (ValidationHelper.IsValidInt(adjuvant.lowamountnumerprefix))
                        {
                            prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerprefix);
                            if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                            {
                                adjuvantType.lowamountnumerprefix = prefix.term_name_english.Trim();
                            }
                            else if (prefix != null)
                            {
                            constructionErrors.Add("Low amount numerator prefix can't be longer than 12 characters|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Low amount numerator prefix is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.lowamountdenomprefix))
                        {
                            prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomprefix);
                            if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                            {
                                adjuvantType.lowamountdenomprefix = prefix.term_name_english.Trim();
                            }
                            else if (prefix != null)
                            {
                                constructionErrors.Add("Low amount denominator prefix can't be longer than 12 characters|" + PPLink +
                                                  pp.pharmaceutical_product_PK);
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Low amount denominator prefix is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.highamountnumerprefix) &&
                            contrationTypeCode.Evcode == "2")
                        {
                            prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.highamountnumerprefix);
                            if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                            {
                                adjuvantType.highamountnumerprefix = prefix.term_name_english.Trim();
                            }
                            else if (prefix != null)
                            {
                                constructionErrors.Add("High amount numerator prefix can't be longer than 12 characters|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount numerator prefix is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.highamountdenomprefix) &&
                            contrationTypeCode.Evcode == "2")
                        {
                            prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.highamountdenomprefix);
                            if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                            {
                                adjuvantType.highamountdenomprefix = prefix.term_name_english.Trim();
                            }
                            else if (prefix != null)
                            {
                                constructionErrors.Add("High amount denominator prefix can't be longer than 12 characters|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            }
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount denominator prefix is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        Ssi__cont_voc_PK unit;
                        if (ValidationHelper.IsValidInt(adjuvant.lowamountnumerunit))
                        {
                            unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerunit);
                            if (unit != null && unit.term_name_english.Trim().Length <= 70)
                            {
                                adjuvantType.lowamountnumerunit = unit.term_name_english.Trim();
                            }
                            else if (unit != null)
                            {
                                constructionErrors.Add("Low amount numerator unit can't be longer than 70 characters|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Low amount numerator unit is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.lowamountdenomunit))
                        {
                            unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomunit);
                            if (unit != null && unit.term_name_english.Trim().Length <= 70)
                            {
                                adjuvantType.lowamountdenomunit = unit.term_name_english.Trim();
                            }
                            else if (unit != null)
                            {
                                constructionErrors.Add("Low amount denominator unit can't be longer than 70 characters|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            }
                        }
                        else
                        {
                            constructionErrors.Add("Low amount denominator unit is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.higamountnumerunit) && contrationTypeCode.Evcode == "2")
                        {
                            unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.higamountnumerunit);
                            if (unit != null && unit.term_name_english.Trim().Length <= 70)
                            { 
                                adjuvantType.highamountnumerunit = unit.term_name_english.Trim();
                            }
                            else if (unit != null)
                            {
                                constructionErrors.Add("High amount numerator unit can't be longer than 70 characters|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                            }
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount numerator unit is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }
                        if (ValidationHelper.IsValidInt(adjuvant.highamountdenomunit) &&
                            contrationTypeCode.Evcode == "2")
                        {
                            unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.highamountdenomunit);
                            if (unit != null && unit.term_name_english.Trim().Length <= 70)
                            {
                                adjuvantType.highamountdenomunit = unit.term_name_english.Trim();
                            }
                            else if (unit != null)
                            {
                                constructionErrors.Add("High amount denominator unit can't be longer than 70 characters|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                        }
                        else if (contrationTypeCode.Evcode == "2")
                        {
                            constructionErrors.Add("High amount denominator unit is not in valid format|" + PPLink +
                                                   pp.pharmaceutical_product_PK);
                        }

                        adjuvantTypeList.Add(adjuvantType);
                    }
                    ppt.adjuvants.adjuvant = adjuvantTypeList.ToArray();
                }

                #endregion

                #region Excipients
                

                IExcipient_PKOperations _excipient_PKOperations = new Excipient_PKDAL();

                List<Excipient_PK> excipientList = _excipient_PKOperations.GetExcipientsByPPPK(pp.pharmaceutical_product_PK);

                if (excipientList.Count > 0)
                {
                    ppt.excipients = new pharmaceuticalproductType.excipientsLocalType();

                    var excipientTypeList = new List<pharmaceuticalproductType.excipientsLocalType.excipientLocalType>();

                    foreach (Excipient_PK excipient in excipientList)
                    {
                        var excipientType = new pharmaceuticalproductType.excipientsLocalType.excipientLocalType();

                        Substance_PK substance = _substance_PKOperations.GetEntity(excipient.substancecode_FK);

                        if (substance != null)
                        {
                            excipientType.substancecode =
                                new pharmaceuticalproductType.excipientsLocalType.excipientLocalType.
                                    substancecodeLocalType();
                            excipientType.substancecode.resolutionmode = 2;

                            if (!string.IsNullOrWhiteSpace(substance.ev_code))
                            {
                                if (substance.ev_code.Trim().Length <= 60)
                                {
                                    excipientType.substancecode.TypedValue = substance.ev_code.Trim();
                                }
                                else
                                {
                                    constructionErrors.Add("Substance code EV Code can+t be longer than 60 characters|" + PPLink +
                                                          pp.pharmaceutical_product_PK);
                                }
                            }
                            else
                            {
                                constructionErrors.Add("Substance code is missing EV Code|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                        }

                        if (excipient.concentrationtypecode.HasValue)
                        {
                            ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

                            Ssi__cont_voc_PK contrationTypeCode =
                                _ssi__cont_voc_PKOperations.GetEntity(excipient.concentrationtypecode);

                            if (contrationTypeCode != null && !string.IsNullOrWhiteSpace(contrationTypeCode.Evcode) && ValidationHelper.IsValidInt(contrationTypeCode.Evcode))
                            {
                                excipientType.concentrationtypecode = contrationTypeCode.Evcode;
                            }
                            else
                            {
                                constructionErrors.Add("Concentration type code is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                                continue;
                            }

                            if (excipient.lowamountnumervalue.HasValue)
                            {
                                excipientType.lowamountnumervalue = excipient.lowamountnumervalue.Value;
                            }
                            else
                            {
                                constructionErrors.Add("Low amount numerator value is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }

                            if (excipient.lowamountdenomvalue.HasValue)
                            {
                                excipientType.lowamountdenomvalue = excipient.lowamountdenomvalue.Value;
                            }
                            else
                            {
                                constructionErrors.Add("Low amount denominator value is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }

                            if (excipient.highamountnumervalue.HasValue && contrationTypeCode.Evcode == "2")
                            {
                                excipientType.highamountnumervalue = excipient.highamountnumervalue.Value;
                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount numerator value is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }

                            if (excipient.highamountdenomvalue.HasValue && contrationTypeCode.Evcode == "2")
                            {
                                excipientType.highamountdenomvalue = excipient.highamountdenomvalue.Value;
                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount denominator value is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }

                            Ssi__cont_voc_PK prefix;
                            if (ValidationHelper.IsValidInt(excipient.lowamountnumerprefix))
                            {
                                prefix = _ssi__cont_voc_PKOperations.GetEntity(excipient.lowamountnumerprefix);
                                if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                                {
                                    excipientType.lowamountnumerprefix = prefix.term_name_english.Trim();
                                }
                                else if (prefix != null)
                                {
                                    constructionErrors.Add("Low amount numerator prefix can't be longer than 12 characters|" + PPLink +
                                                        pp.pharmaceutical_product_PK);
                                }
                            }
                            else
                            {
                                constructionErrors.Add("Low amount numerator prefix is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.lowamountdenomprefix))
                            {
                                prefix = _ssi__cont_voc_PKOperations.GetEntity(excipient.lowamountdenomprefix);
                                if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                                {
                                    excipientType.lowamountdenomprefix = prefix.term_name_english.Trim();
                                }
                                else if (prefix!= null) 
                                {
                                    constructionErrors.Add("Low amount denominator prefix can't be longer than 12 characters|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                                }
                            }
                            else
                            {
                                constructionErrors.Add("Low amount denominator prefix is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.highamountnumerprefix) &&
                                contrationTypeCode.Evcode == "2")
                            {
                                prefix = _ssi__cont_voc_PKOperations.GetEntity(excipient.highamountnumerprefix);
                                if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                                {
                                    excipientType.highamountnumerprefix = prefix.term_name_english.Trim();
                                }
                                else if (prefix != null)
                                {
                                    constructionErrors.Add("High amount numerator prefix can't be longer than 12 characters|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                                }
                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount numerator prefix is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.highamountdenomprefix) &&
                                contrationTypeCode.Evcode == "2")
                            {
                                prefix = _ssi__cont_voc_PKOperations.GetEntity(excipient.highamountdenomprefix);
                                if (prefix != null && prefix.term_name_english.Trim().Length <= 12)
                                {
                                    excipientType.highamountdenomprefix = prefix.term_name_english.Trim();
                                }
                                else if (prefix != null)
                                {
                                    constructionErrors.Add("High amount denominator prefixcan't be longer than 12 character|" + PPLink +
                                                          pp.pharmaceutical_product_PK);
                                }
                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount denominator prefix is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }

                            Ssi__cont_voc_PK unit;
                            if (ValidationHelper.IsValidInt(excipient.lowamountnumerunit))
                            {
                                unit = _ssi__cont_voc_PKOperations.GetEntity(excipient.lowamountnumerunit);
                                if (unit != null && unit.term_name_english.Trim().Length <= 70)
                                {
                                    excipientType.lowamountnumerunit = unit.term_name_english.Trim();
                                }
                                else if (unit != null)
                                {
                                    constructionErrors.Add("Low amount numerator unit can't be longer than 70 characters|" + PPLink +
                                                      pp.pharmaceutical_product_PK);
                                }
                            }
                            else
                            {
                                constructionErrors.Add("Low amount numerator unit is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.lowamountdenomunit))
                            {
                                unit = _ssi__cont_voc_PKOperations.GetEntity(excipient.lowamountdenomunit);
                                if (unit != null && unit.term_name_english.Trim().Length <= 70)
                                {
                                    excipientType.lowamountdenomunit = unit.term_name_english.Trim();
                                }
                                else if (unit != null)
                                {
                                    constructionErrors.Add("Low amount denominator unit can't be longer than 70 characters|" + PPLink +
                                                      pp.pharmaceutical_product_PK);
                                }
                                    
                            }
                            else
                            {
                                constructionErrors.Add("Low amount denominator unit is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.higamountnumerunit) &&
                                contrationTypeCode.Evcode == "2")
                            {
                                unit = _ssi__cont_voc_PKOperations.GetEntity(excipient.higamountnumerunit);
                                if (unit != null && unit.term_name_english.Trim().Length <= 70)
                                {
                                    excipientType.highamountnumerunit = unit.term_name_english.Trim();
                                }
                                else if (unit != null)
                                {
                                    constructionErrors.Add("High amount numerator unitcan't be longer than 70 characters|" + PPLink +
                                                           pp.pharmaceutical_product_PK);
                                }
                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount numerator unit is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                            if (ValidationHelper.IsValidInt(excipient.highamountdenomunit) &&
                                contrationTypeCode.Evcode == "2")
                            {
                                unit = _ssi__cont_voc_PKOperations.GetEntity(excipient.highamountdenomunit);
                                if (unit != null && unit.term_name_english.Trim().Length <= 70)
                                {
                                    excipientType.highamountdenomunit = unit.term_name_english.Trim();
                                }
                                else
                                {
                                    constructionErrors.Add("High amount denominator unit can't be longer than 70 characters|" + PPLink +
                                                           pp.pharmaceutical_product_PK);
                                }

                            }
                            else if (contrationTypeCode.Evcode == "2")
                            {
                                constructionErrors.Add("High amount denominator unit is not in valid format|" + PPLink +
                                                       pp.pharmaceutical_product_PK);
                            }
                        }
                        excipientTypeList.Add(excipientType);
                    }
                    ppt.excipients.excipient = excipientTypeList.ToArray();
                }

                #endregion

                pptlist.Add(ppt);
            }
                
            apt.pharmaceuticalproducts = new authorisedproductType.pharmaceuticalproductsLocalType();
            apt.pharmaceuticalproducts.pharmaceuticalproduct = pptlist.ToArray();
        }

        public void GenerateHeader(string messagenumb)
        {
            Random rnd = new Random();

            Message.ichicsrmessageheader = new ichicsrmessageheaderType();
            Message.ichicsrmessageheader.messageformatversion = ConfigurationManager.AppSettings["messageFormatVersion"];
            Message.ichicsrmessageheader.messageformatrelease = ConfigurationManager.AppSettings["messageFormatRelease"];
            Message.ichicsrmessageheader.messagetype = ConfigurationManager.AppSettings["messageType"];
            Message.ichicsrmessageheader.messagedate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss"); //204 = CCYYMMDDHHMMSS  (example:  12 JANUARY 1997 14:02:17 --> 19970112140217)      
            Message.ichicsrmessageheader.messagedateformat = "204";
            Message.ichicsrmessageheader.messagereceiveridentifier = ConfigurationManager.AppSettings["messageReceiverID"];
            Message.ichicsrmessageheader.messagenumb = messagenumb.Trim();
            //Message.ichicsrmessageheader.messagesenderidentifier = ConfigurationManager.AppSettings["messageSenderID"];
        }

        #region Message Header
        public void ConstructMessageHeader(string messageNumber)
        {
            Message.ichicsrmessageheader = new ichicsrmessageheaderType();
            Message.ichicsrmessageheader.messageformatversion = ConfigurationManager.AppSettings["messageFormatVersion"];
            Message.ichicsrmessageheader.messageformatrelease = ConfigurationManager.AppSettings["messageFormatRelease"];
            Message.ichicsrmessageheader.messagetype = ConfigurationManager.AppSettings["messageType"];
            Message.ichicsrmessageheader.messagedate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss"); //204 = CCYYMMDDHHMMSS  (example:  12 JANUARY 1997 14:02:17 --> 19970112140217)      
            Message.ichicsrmessageheader.messagedateformat = "204";
            Message.ichicsrmessageheader.messagereceiveridentifier = ConfigurationManager.AppSettings["messageReceiverID"];
            Message.ichicsrmessageheader.messagesenderidentifier = ConfigurationManager.AppSettings["messageSenderID"];
            Message.ichicsrmessageheader.messagenumb = messageNumber;
        }
        #endregion
        #region Master File Location

        public bool ConstructMasterFileLocation(Organization_PK masterFileLocation, XevprmOperationType operationType, out List<XevprmXmlError> constructionErrors)
        {
            masterfilelocationType masterFileLocationXml = null;
            constructionErrors = new List<XevprmXmlError>();

            if (masterFileLocation == null)
            {
                return false;
            }

            if (operationType == XevprmOperationType.Insert)
            {
                masterFileLocationXml = ConstructMasterFileLocationInsert(masterFileLocation, ref constructionErrors);
            }
            else if (operationType == XevprmOperationType.Update)
            {
                masterFileLocationXml = ConstructMasterFileLocationUpdate(masterFileLocation, ref constructionErrors);
            }

            if (constructionErrors.Count > 0)
            {
                return false;
            }

            if (masterFileLocationXml != null)
            {
                List<masterfilelocationType> masterFileLocationXmlList = new List<masterfilelocationType>();
                masterFileLocationXmlList.Add(masterFileLocationXml);

                Message.masterfilelocations = new evprm.masterfilelocationsLocalType();
                Message.masterfilelocations.masterfilelocation = masterFileLocationXmlList.ToArray();

                return true;
            }

            return false;
        }

        private masterfilelocationType ConstructMasterFileLocationInsert(Organization_PK masterFileLocation, ref List<XevprmXmlError> constructionErrors)
        {
            masterfilelocationType masterfilelocationType = null;

            if (masterFileLocation != null)
            {
                string errorLink = "../Operational/OrganizationView.aspx?f=d&error=1&id=" + masterFileLocation.organization_PK;

                masterfilelocationType = new masterfilelocationType();

                masterfilelocationType.operationtype = (int)XevprmOperationType.Insert;

                if (masterFileLocation.organization_PK.HasValue)
                {
                    if (masterFileLocation.organization_PK.Value.ToString().Length <= 60)
                    {
                        masterfilelocationType.localnumber = masterFileLocation.organization_PK.Value.ToString(); //Guid.NewGuid().ToString();
                    }
                    else
                    {
                        string error = "Local number can't be longer than 60 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Local number is unique reference for the entity in the message. Mandatory for Operation Type \"Insert\".";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mflcompany))
                {
                    if (masterFileLocation.mflcompany.Trim().Length <= 100) 
                    {
                        masterfilelocationType.mflcompany = masterFileLocation.mflcompany.Trim(); 
                    }
                    else
                    {
                        string error = "Master File (MF) Company can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mfldepartment))
                {
                    if (masterFileLocation.mfldepartment.Trim().Length <= 100)
                    {
                        masterfilelocationType.mfldepartment = masterFileLocation.mfldepartment.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Department can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mflbuilding))
                {
                    if (masterFileLocation.mflbuilding.Trim().Length <= 100)
                    {
                        masterfilelocationType.mflbuilding = masterFileLocation.mflbuilding.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Building can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.address))
                {
                    if (masterFileLocation.address.Trim().Length <= 100)
                    {
                        masterfilelocationType.mflstreet = masterFileLocation.address.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Street can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Street must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.city))
                {
                    if (masterFileLocation.city.Trim().Length <= 35)
                    {
                        masterfilelocationType.mflcity = masterFileLocation.city.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) City can't be longer than 35 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) City must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.state))
                {
                    if (masterFileLocation.state.Trim().Length <= 40)
                    {
                        masterfilelocationType.mflstate = masterFileLocation.state.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) State can't be longer than 40 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.postcode))
                {
                    if (masterFileLocation.postcode.Trim().Length <= 15)
                    {
                        masterfilelocationType.mflpostcode = masterFileLocation.postcode.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Postcode can't be longer than 15 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Postcode must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (masterFileLocation.countrycode_FK.HasValue)
                {
                    ICountry_PKOperations _country_PKOperations = new Country_PKDAL();
                    Country_PK country = _country_PKOperations.GetEntity(masterFileLocation.countrycode_FK.Value);
                    if (country != null)
                    {
                        if (country.abbreviation.Trim().Length == 2)
                        {
                            masterfilelocationType.mflcountrycode = country.abbreviation.Trim();
                        }
                        else
                        {
                            string error = "Master File (MF) Country Code can't be longer than 2 characters";
                            constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                        }
                    }
                    else
                    {
                        string error = "Master File (MF) Country Code must be specified.";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Country Code must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.comment))
                {
                    if (masterFileLocation.comment.Trim().Length <= 500)
                    {
                        masterfilelocationType.comments = masterFileLocation.comment.Trim();
                    }
                    else
                    {
                        string error = "Comment can't be longer than 500 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
            }

            return masterfilelocationType;
        }

        private masterfilelocationType ConstructMasterFileLocationUpdate(Organization_PK masterFileLocation, ref List<XevprmXmlError> constructionErrors)
        {
            masterfilelocationType masterfilelocationType = null;

            if (masterFileLocation != null)
            {
                string errorLink = "../Operational/OrganizationView.aspx?f=d&error=1&id=" + masterFileLocation.organization_PK;

                masterfilelocationType = new masterfilelocationType();

                masterfilelocationType.operationtype = (int)XevprmOperationType.Update;

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mfl_evcode))
                {
                    if (masterFileLocation.mfl_evcode.Trim().Length <= 60)
                    {
                        masterfilelocationType.ev_code = masterFileLocation.mfl_evcode.Trim();
                    }
                    else
                    {
                        string error = "Ev Code can't be longer than 60 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Ev Code is mandatory if the operation type is NOT \"Insert\"";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mflcompany))
                {
                    if (masterFileLocation.mflcompany.Trim().Length <= 100)
                    {
                        masterfilelocationType.mflcompany = masterFileLocation.mflcompany.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Company can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mfldepartment))
                {
                    if (masterFileLocation.mfldepartment.Trim().Length <= 100)
                    {
                        masterfilelocationType.mfldepartment = masterFileLocation.mfldepartment.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Department can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.mflbuilding))
                {
                    if (masterFileLocation.mflbuilding.Trim().Length <= 100)
                    {
                        masterfilelocationType.mflbuilding = masterFileLocation.mflbuilding.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Building can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.address))
                {
                    if (masterFileLocation.address.Trim().Length <= 100)
                    {
                        masterfilelocationType.mflstreet = masterFileLocation.address.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Street can't be longer than 100 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Street must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.city))
                {
                    if (masterFileLocation.city.Trim().Length <= 35)
                    {
                        masterfilelocationType.mflcity = masterFileLocation.city.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) City can't be longer than 35 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) City must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.state))
                {
                    if (masterFileLocation.state.Trim().Length <= 40)
                    {
                        masterfilelocationType.mflstate = masterFileLocation.state.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) State can't be longer than 40 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.postcode))
                {
                    if (masterFileLocation.postcode.Trim().Length <= 15)
                    {
                        masterfilelocationType.mflpostcode = masterFileLocation.postcode.Trim();
                    }
                    else
                    {
                        string error = "Master File (MF) Postcode can't be longer than 15 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Postcode must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (masterFileLocation.countrycode_FK.HasValue)
                {
                    ICountry_PKOperations _country_PKOperations = new Country_PKDAL();
                    Country_PK country = _country_PKOperations.GetEntity(masterFileLocation.countrycode_FK.Value);
                    if (country != null)
                    {
                        if (country.abbreviation.Trim().Length == 2)
                        {
                            masterfilelocationType.mflcountrycode = country.abbreviation.Trim();
                        }
                        else
                        {
                            string error = "Master File (MF) Country Code can't be longer than 2 characters";
                            constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                        }
                    }
                    else
                    {
                        string error = "Master File (MF) Country Code must be specified.";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
                else
                {
                    string error = "Master File (MF) Country Code must be specified.";
                    constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                }

                if (!string.IsNullOrWhiteSpace(masterFileLocation.comment))
                {
                    if (masterFileLocation.comment.Trim().Length <= 500)
                    {
                        masterfilelocationType.comments = masterFileLocation.comment.Trim();
                    }
                    else
                    {
                        string error = "Comment can't be longer than 500 characters";
                        constructionErrors.Add(new XevprmXmlError(masterFileLocation.GetType(), masterFileLocation.organization_PK, error, errorLink));
                    }
                }
            }

            return masterfilelocationType;
        }

        #endregion
    }

    public class XevprmXmlError
    {
        #region Declarations

        Type _type;
        int? _id;
        string _propertyName;
        string _text;
        string _navigateUrl;
        XevprmOperationType _operationType;

        #endregion

        #region Properties

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public int? ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string NavigateUrl
        {
            get { return _navigateUrl; }
            set { _navigateUrl = value; }
        }

        public XevprmOperationType OperationType
        {
            get { return _operationType; }
            set { _operationType = value; }
        }

        #endregion

        #region Constructors
        
        public XevprmXmlError(Type inType, int? inID, string inText, string inNavigateUrl)
        {
            Type = inType;
            ID = inID;
            Text = inText;
            NavigateUrl = inNavigateUrl;
        }

        #endregion
    }
}
