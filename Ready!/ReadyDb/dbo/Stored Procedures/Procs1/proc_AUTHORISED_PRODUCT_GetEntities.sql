-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ap_PK], [product_FK], [authorisationcountrycode_FK], [organizationmahcode_FK], [product_name], [productshortname], 
	[authorisationnumber], [authorisationstatus_FK], [authorisationdate], [authorisationexpdate], [comment], 
	[authorisationwithdrawndate], [packagedesc], [marketed], [legalstatus], [withdrawndateformat], [mflcode_FK], 
	[qppvcode_person_FK], [product_ID], [ev_code],[qppv_code_FK],
	 [dbo].[ReturnAuthorisedProductXevprmStatus]([ap_PK]) AS XEVPRM_status,
	  [responsible_user_person_FK], [launchdate], 
	[description], [authorised_product_ID], [authorisationdateformat], [evprm_comments], [localnumber], [ap_ID], 
	[shelflife], [productgenericname], [productcompanyname], [productstrenght], [productform], [infodate], [phv_email] , [phv_phone],
	 [article_57_reporting], [sunsetclause],[substance_translations], [local_representative_FK], [Indications], [local_qppv_code_FK]
	 ,[license_holder_group_FK], [reservation_confirmed], [reserved_to], [local_codes], [pack_size], [reimbursment_status]
	FROM [dbo].[AUTHORISED_PRODUCT]
END
