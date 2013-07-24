-- Save
CREATE PROCEDURE  [dbo].[proc_AP_SAVED_SEARCH_Save]
	@AP_SAVED_SEARCH_PK int = NULL,
	@product_FK int = NULL,
	@authorisationcountrycode_FK int = NULL,
	@productshortname nvarchar(250) = NULL,
	@responsible_user_person_FK int = NULL,
	@packagedesc nvarchar(2000) = NULL,
	@authorisationstatus_FK int = NULL,
	@legalstatus nvarchar(50) = NULL,
	@marketed bit = NULL,
	@organizationmahcode_FK int = NULL,
	@authorisationdateFrom date = NULL,
	@authorisationdateTo date = NULL,
	@authorisationexpdateFrom date = NULL,
	@authorisationexpdateTo date = NULL,
	@sunsetclauseFrom date = NULL,
	@sunsetclauseTo date = NULL,
	@authorisationnumber nvarchar(100) = NULL,
	@displayName nvarchar(100)=null,
	@user_fk int =null,
	@gridlayout nvarchar(MAX)=null,
	@ispublic bit=null,
	@article57_reporting bit=null,
	@client_org_FK int = null,
	@MEDDRA_FK nvarchar(max) = null,
	@substance_translations bit=null,
	@ev_code nvarchar(50)=null,
	@qppv_person_FK int = null, 
	@local_representative_FK int = NULL,
	@indications nvarchar(MAX) = NULL,
	@local_qppv_person_FK int = NULL,
	@mflcode_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AP_SAVED_SEARCH]
	SET
	[product_FK] = @product_FK,
	[authorisationcountrycode_FK] = @authorisationcountrycode_FK,
	[productshortname] = @productshortname,
	[responsible_user_person_FK] = @responsible_user_person_FK,
	[packagedesc] = @packagedesc,
	[authorisationstatus_FK] = @authorisationstatus_FK,
	[legalstatus] = @legalstatus,
	[marketed] = @marketed,
	[organizationmahcode_FK] = @organizationmahcode_FK,
	[authorisationdateFrom] = @authorisationdateFrom,
	[authorisationdateTo] = @authorisationdateTo,
	[authorisationexpdateFrom] = @authorisationexpdateFrom,
	[authorisationexpdateTo] = @authorisationexpdateTo,
	[sunsetclauseFrom] = @sunsetclauseFrom,
	[sunsetclauseTo] = @sunsetclauseTo,
	[authorisationnumber] = @authorisationnumber,
	[displayName]=@displayName,
	[user_FK]=@user_fk,
	[gridLayout]=@gridlayout,
	[ispublic]=@ispublic,
	[article57_reporting] = @article57_reporting,
	[client_org_FK] = @client_org_FK,
	[MEDDRA_FK] = @MEDDRA_FK,
	[substance_translations] = @substance_translations,
	[ev_code] = @ev_code,
	[qppv_person_FK] = @qppv_person_FK, 
	[local_representative_FK] = @local_representative_FK,
	[indications] = @indications,
	[local_qppv_person_FK] = @local_qppv_person_FK,
	[mflcode_FK] = @mflcode_FK
	WHERE [AP_SAVED_SEARCH_PK] = @AP_SAVED_SEARCH_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AP_SAVED_SEARCH]
		([product_FK], [authorisationcountrycode_FK], [productshortname], [responsible_user_person_FK], [packagedesc], [authorisationstatus_FK], [legalstatus], [marketed], [organizationmahcode_FK], [authorisationdateFrom], [authorisationdateTo], [authorisationexpdateFrom], [authorisationexpdateTo], [authorisationnumber],[displayName],[user_FK],[gridLayout],[ispublic],[article57_reporting], [client_org_FK], [sunsetclauseFrom], [sunsetclauseTo],[MEDDRA_FK],[substance_translations],ev_code,[qppv_person_FK], [local_representative_FK], [indications], [local_qppv_person_FK], [mflcode_FK])
		VALUES
		(@product_FK, @authorisationcountrycode_FK, @productshortname, @responsible_user_person_FK, @packagedesc, @authorisationstatus_FK, @legalstatus, @marketed, @organizationmahcode_FK, @authorisationdateFrom, @authorisationdateTo, @authorisationexpdateFrom, @authorisationexpdateTo, @authorisationnumber,@displayName,@user_fk,@gridlayout,@ispublic,@article57_reporting, @client_org_FK, @sunsetclauseFrom, @sunsetclauseTo, @MEDDRA_FK,@substance_translations,@ev_code, @qppv_person_FK, @local_representative_FK, @indications, @local_qppv_person_FK, @mflcode_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
