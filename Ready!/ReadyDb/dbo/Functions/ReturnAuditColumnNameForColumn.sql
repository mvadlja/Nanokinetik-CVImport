CREATE FUNCTION ReturnAuditColumnNameForColumn
(
	@table nvarchar(max) = NULL,
	@column nvarchar(max) = NULL
)
RETURNS nvarchar(max)

AS
  BEGIN
  	
    DECLARE @var nvarchar(max) = NULL

	IF @table = 'REMINDER' 
	BEGIN
		IF @column = 'REMINDER_EMAIL_RECIPIENT' RETURN 'Email recipients'
		IF @column = 'REMINDER_DATE' RETURN 'Alert date'
		IF @column = 'reminder_name' RETURN 'Name';
		IF @column = 'reminder_type' RETURN 'Type';
		IF @column = 'related_attribute_name' RETURN 'Related date name';
		IF @column = 'related_attribute_value' RETURN 'Related date';
		IF @column = 'time_before_activation' RETURN 'Days before';
		IF @column = 'reminder_date' RETURN 'Alert date';
		IF @column = 'additional_emails' RETURN 'Additional addresses';
		IF @column = 'responsible_user_FK' RETURN 'Responsible user';
		IF @column = 'reminder_user_status_FK' RETURN 'Status';
		IF @column = 'remind_me_on_email' RETURN 'Send Email';
	END

	IF @table = 'AUTHORISED_PRODUCT' 
	BEGIN
		IF @column = 'product_ID' RETURN 'Authorised product ID'
		IF @column = 'qppv_code_FK' RETURN 'QPPV'
		IF @column = 'license_holder_group_FK' RETURN 'Licence holder group'
		IF @column = 'local_codes' RETURN 'Local codes'
		IF @column = 'reimbursment_status' RETURN 'Reimbursment status'
		IF @column = 'reservation_confirmed' RETURN 'Reservation confirmed'
		IF @column = 'reserved_to' RETURN 'Reserved to'
	    IF @column = 'pack_size' RETURN 'Pack size (approved national)' 
	END
	
	IF @table = 'PRODUCT' 
	BEGIN
	    IF @column = 'pack_size' RETURN 'Pack size (approved by procedure)' 
	END

	IF @table = 'PHARMACEUTICAL_PRODUCT' 
	BEGIN
	    IF @column = 'comments' RETURN 'Comment' 
	END

	IF @column = 'product_ID' RETURN 'Product ID'
	IF @column = 'comments' RETURN 'Comments'
	IF @column = 'orphan_drug' RETURN 'Orphan drug';
    IF @column = 'attachment_name' RETURN 'Name';
    IF @column = 'intensive_monitoring' RETURN 'Intensive monitoring';
    IF @column = 'authorisation_procedure' RETURN 'Authorisation procedure';
    IF @column = 'responsible_user_person_FK' RETURN 'Responsible user';
    IF @column = 'psur' RETURN 'PSUR cycle';
    IF @column = 'next_dlp' RETURN 'Next DLP';
    IF @column = 'name' RETURN 'Name';
    IF @column = 'description' RETURN 'Description';
    IF @column = 'client_organization_FK' RETURN 'Client';
    IF @column = 'type_product_FK' RETURN 'Product type';
    IF @column = 'product_number' RETURN 'Product number';
    IF @column = 'PRODUCT_COUNTRY_MN' RETURN 'Countries';
    IF @column = 'PRODUCT_DOMAIN_MN' RETURN 'Domain';
    IF @column = 'PRODUCT_PP_MN' RETURN 'Pharmaceutical products';
    IF @column = 'PRODUCT_ATC_MN' RETURN 'Drug ATCs';
    IF @column = 'PRODUCT_MANUFACTURER_MN' RETURN 'Manufacturer';
    IF @column = 'PRODUCT_PARTNER_MN' RETURN 'Partner';
    IF @column = 'activity_ID' RETURN 'Activity ID';
    IF @column = 'responsible_user_FK' RETURN 'Responsible user';
    IF @column = 'booked_slots' RETURN 'Booked slots';
    IF @column = 'PP_ADMINISTRATION_ROUTES' RETURN 'Administration routes';
    IF @column = 'PP_MEDICAL_DEVICES' RETURN 'Medical devices';
    IF @column = 'PP_ADJUVANTS' RETURN 'Adjuvants';
    IF @column = 'PP_EXCIPIENTS' RETURN 'Excipients';
    IF @column = 'PP_ACTIVE_INGREDIENTS' RETURN 'Active ingredients';
    IF @column = 'PP_PRODUCTS' RETURN 'Products';
    IF @column = 'Pharmform_FK' RETURN 'Pharmaceutical form';
    IF @column = 'document_PK' RETURN 'Document PK';
    IF @column = 'person_FK' RETURN 'Responsible user';
    IF @column = 'type_FK' RETURN 'Document Type';
    IF @column = 'document_code' RETURN 'Document number';
    IF @column = 'comment' RETURN 'Comment';
    IF @column = 'regulatory_status' RETURN 'Regulatory status';
    IF @column = 'version_number' RETURN 'Version number';
    IF @column = 'version_label' RETURN 'Version label';
    IF @column = 'change_date' RETURN 'Change date';
    IF @column = 'effective_start_date' RETURN 'Effective start date';
    IF @column = 'effective_end_date' RETURN 'Effective end date';
    IF @column = 'attachment_type_FK' RETURN 'Attachment type';
    IF @column = 'DOCUMENT_LANGUAGE_CODE' RETURN 'Language codes';
    IF @column = 'DOCUMENT_PROJECT_MN' RETURN 'Projects';
    IF @column = 'DOCUMENT_PRODUCT_MN' RETURN 'Products';
    IF @column = 'DOCUMENT_AP_MN' RETURN 'Authorised products';
    IF @column = 'DOCUMENT_PP_MN' RETURN 'Pharmaceutical products';
    IF @column = 'DOCUMENT_ACTIVITY_MN' RETURN 'Activities';
    IF @column = 'DOCUMENT_ATTACHMENTS' RETURN 'Attachments';
    IF @column = 'version_date' RETURN 'Version date';
    IF @column = 'product_FK' RETURN 'Related product';
    IF @column = 'authorisationcountrycode_FK' RETURN 'Authorisation country';
    IF @column = 'organizationmahcode_FK' RETURN 'Licence holder';
    IF @column = 'product_name' RETURN 'Full presentation name';
    IF @column = 'productshortname' RETURN 'Product short name';
    IF @column = 'authorisationnumber' RETURN 'Authorisation number';
    IF @column = 'authorisationstatus_FK' RETURN 'Authorisation status';
    IF @column = 'authorisationdate' RETURN 'Authorisation date';
    IF @column = 'authorisationexpdate' RETURN 'Authorization expiry date';
    IF @column = 'authorisationwithdrawndate' RETURN 'Withdrawn date';
    IF @column = 'packagedesc' RETURN 'Package description';
    IF @column = 'marketed' RETURN 'Marketed';
    IF @column = 'legalstatus' RETURN 'Legal status';
    IF @column = 'mflcode_FK' RETURN 'Master File Location';
    IF @column = 'qppvcode_person_FK' RETURN 'QPPV';
    IF @column = 'ev_code' RETURN 'EVCODE';
    IF @column = 'launchdate' RETURN 'Launch date';
    IF @column = 'evprm_comments' RETURN 'Comment (EVPRM)';
    IF @column = 'shelflife' RETURN 'Shelf life';
    IF @column = 'productgenericname' RETURN 'Product generic name';
    IF @column = 'productcompanyname' RETURN 'Product company name';
    IF @column = 'productstrenght' RETURN 'Product strength name';
    IF @column = 'productform' RETURN 'Product form name';
    IF @column = 'infodate' RETURN 'Info date';
    IF @column = 'phv_email' RETURN 'PhV EMail';
    IF @column = 'article_57_reporting' RETURN 'Article 57 reporting';
    IF @column = 'sunsetclause' RETURN 'Sunset clause';
    IF @column = 'substance_translations' RETURN 'Substance translations';
    IF @column = 'AP_ORGANIZATION_DIST_MN' RETURN 'Distributor';
    IF @column = 'phv_phone' RETURN 'PhV Phone';
    IF @column = 'XEVPRM_status' RETURN 'XEVPRM status';
    IF @column = 'user_FK' RETURN 'Responsible user';
    IF @column = 'mode_FK' RETURN 'Activity mode';
    IF @column = 'procedure_type_FK' RETURN 'Procedure type';
    IF @column = 'regulatory_status_FK' RETURN 'Regulatory status';
    IF @column = 'start_date' RETURN 'Start date';
    IF @column = 'expected_finished_date' RETURN 'Expected finished date';
    IF @column = 'actual_finished_date' RETURN 'Actual finished date';
    IF @column = 'approval_date' RETURN 'Approval date';
    IF @column = 'submission_date' RETURN 'Submission date';
    IF @column = 'procedure_number' RETURN 'Procedure number';
    IF @column = 'legal' RETURN 'Legal basis of application';
    IF @column = 'internal_status_FK' RETURN 'Internal status';
    IF @column = 'ACTIVITY_PRODUCT_MN' RETURN 'Products';
    IF @column = 'ACTIVITY_TYPE_MN' RETURN 'Type';
    IF @column = 'ACTIVITY_APPLICANT_MN' RETURN 'Applicant';
    IF @column = 'ACTIVITY_COUNTRY_MN' RETURN 'Countries';
    IF @column = 'ACTIVITY_PROJECT_MN' RETURN 'Projects';
    IF @column = 'billable' RETURN 'Billable';
    IF @column = 'automatic_alerts_on' RETURN 'Automatic alerts';
    IF @column = 'local_representative_FK' RETURN 'Local representative';
	IF @column = 'local_qppv_code_FK' RETURN 'Local PV Contact';   
    IF @column = 'PRODUCT_PACKAGING_MATERIAL_MN' RETURN 'Packaging materials';   
    IF @column = 'region_FK' RETURN 'Region';   
    IF @column = 'storage_conditions_FK' RETURN 'Storage conditions';   
	IF @column = 'batch_size' RETURN 'Batch size';   
	IF @column = 'client_group_FK' RETURN 'Client group';   
	IF @column = 'MEDDRA_AP_MN' RETURN 'Indications';   

	RETURN @column   
  END