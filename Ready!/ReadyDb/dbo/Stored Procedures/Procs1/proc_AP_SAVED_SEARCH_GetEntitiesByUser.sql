-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AP_SAVED_SEARCH_GetEntitiesByUser]
	@user_fk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[AP_SAVED_SEARCH_PK], [product_FK], [authorisationcountrycode_FK], 
	[productshortname], [responsible_user_person_FK], [packagedesc], [authorisationstatus_FK], [legalstatus], [marketed],
	 [organizationmahcode_FK], [authorisationdateFrom], [authorisationdateTo], [authorisationexpdateFrom], [authorisationexpdateTo], 
	 [authorisationnumber],[displayName],[user_FK],[gridLayout],[ispublic],
	 [article57_reporting], [client_org_FK],[sunsetclauseFrom],[sunsetclauseTo], [MEDDRA_FK],[substance_translations],[ev_code], 
	 [qppv_person_FK], [local_representative_FK], [indications], [local_qppv_person_FK], [mflcode_FK]
	FROM [dbo].[AP_SAVED_SEARCH]
	where [user_FK]=@user_fk
END
