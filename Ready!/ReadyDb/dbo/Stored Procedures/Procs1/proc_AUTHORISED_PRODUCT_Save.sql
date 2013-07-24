-- Save
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_Save]
	@ap_PK int = NULL,
	@product_FK int = NULL,
	@authorisationcountrycode_FK int = NULL,
	@organizationmahcode_FK int = NULL,
	@product_name nvarchar(2000) = NULL,
	@productshortname nvarchar(250) = NULL,
	@authorisationnumber nvarchar(100) = NULL,
	@authorisationstatus_FK int = NULL,
	@authorisationdate date = NULL,
	@authorisationexpdate date = NULL,
	@comment nvarchar(MAX) = NULL,
	@authorisationwithdrawndate date = NULL,
	@packagedesc nvarchar(2000) = NULL,
	@marketed int = NULL,
	@legalstatus nvarchar(50) = NULL,
	@withdrawndateformat nvarchar(3) = NULL,
	@mflcode_FK int = NULL,
	@qppvcode_person_FK int = NULL,
	@product_ID nvarchar(50) = NULL,
	@ev_code nvarchar(50) = NULL,
	@XEVPRM_status nvarchar(50) = NULL,
	@responsible_user_person_FK int = NULL,
	@launchdate date = NULL,
	@description nvarchar(MAX) = NULL,
	@authorised_product_ID nvarchar(50) = NULL,
	@authorisationdateformat nvarchar(3) = NULL,
	@evprm_comments nvarchar(MAX) = NULL,
	@localnumber nvarchar(200) = NULL,
	@ap_ID nvarchar(50) = NULL,
	@shelflife nvarchar(500) = NULL,
	@productgenericname nvarchar(1000) = NULL, 
	@productcompanyname nvarchar(250) = NULL, 
	@productstrenght nvarchar(250) = NULL, 
	@productform nvarchar(500) = NULL,
	@infodate date = NULL,
	@phv_email nvarchar(500) = NULL,
	@phv_phone nvarchar(500) = NULL,
	@article_57_reporting int = NULL,
	@sunsetclause date = NULL,
	@substance_translations bit=null,
	@qppv_code_FK int = NULL,
	@local_representative_FK int = NULL,
    @Indications nvarchar(500) = NULL,
	@local_qppv_code_FK int = NULL,
	@license_holder_group_FK int = NULL,
	@reservation_confirmed bit = NULL,
	@reserved_to nvarchar(200) = NULL,
	@local_codes nvarchar(200) = NULL,
	@pack_size nvarchar(200) = NULL,
	@reimbursment_status bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AUTHORISED_PRODUCT]
	SET
	[product_FK] = @product_FK,
	[authorisationcountrycode_FK] = @authorisationcountrycode_FK,
	[organizationmahcode_FK] = @organizationmahcode_FK,
	[product_name] = @product_name,
	[productshortname] = @productshortname,
	[authorisationnumber] = @authorisationnumber,
	[authorisationstatus_FK] = @authorisationstatus_FK,
	[authorisationdate] = @authorisationdate,
	[authorisationexpdate] = @authorisationexpdate,
	[comment] = @comment,
	[authorisationwithdrawndate] = @authorisationwithdrawndate,
	[packagedesc] = @packagedesc,
	[marketed] = @marketed,
	[legalstatus] = @legalstatus,
	[withdrawndateformat] = @withdrawndateformat,
	[mflcode_FK] = @mflcode_FK,
	[qppvcode_person_FK] = @qppvcode_person_FK,
	[product_ID] = @product_ID,
	[ev_code] = @ev_code,
	[XEVPRM_status] = @XEVPRM_status,
	[responsible_user_person_FK] = @responsible_user_person_FK,
	[launchdate] = @launchdate,
	[description] = @description,
	[authorised_product_ID] = @authorised_product_ID,
	[authorisationdateformat] = @authorisationdateformat,
	[evprm_comments] = @evprm_comments,
	[localnumber] = @localnumber,
	[ap_ID] = @ap_ID,
	[shelflife] = @shelflife,
	[productgenericname] = @productgenericname, 
	[productcompanyname] = @productcompanyname, 
	[productstrenght] = @productstrenght, 
	[productform] = @productform,
	[infodate] = @infodate ,
	[phv_email] = @phv_email,
	[phv_phone] = @phv_phone,
	[article_57_reporting] = @article_57_reporting,
	[sunsetclause] = @sunsetclause,
	[substance_translations]=@substance_translations,
	[qppv_code_FK] = @qppv_code_FK,
	[local_representative_FK] = @local_representative_FK,
	[Indications] = @Indications,
	[local_qppv_code_FK] = @local_qppv_code_FK,
	[license_holder_group_FK] = @license_holder_group_FK, 
	[reservation_confirmed] = @reservation_confirmed, 
	[reserved_to] = @reserved_to, 
	[local_codes] = @local_codes, 
	[pack_size] = @pack_size, 
	[reimbursment_status] = @reimbursment_status
	WHERE [ap_PK] = @ap_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AUTHORISED_PRODUCT]
		([product_FK], [authorisationcountrycode_FK], [organizationmahcode_FK], [product_name], [productshortname], [authorisationnumber], [authorisationstatus_FK], [authorisationdate], [authorisationexpdate], [comment], [authorisationwithdrawndate], [packagedesc], [marketed], [legalstatus], [withdrawndateformat], [mflcode_FK], [qppvcode_person_FK], [product_ID], [ev_code], [XEVPRM_status], [responsible_user_person_FK], [launchdate], [description], [authorised_product_ID], [authorisationdateformat], [evprm_comments], [localnumber], [ap_ID], [shelflife], [productgenericname], [productcompanyname], [productstrenght], [productform], [infodate], [phv_email] , [phv_phone], [article_57_reporting], [sunsetclause],[substance_translations],[qppv_code_FK], [local_representative_FK], [Indications], [local_qppv_code_FK],[license_holder_group_FK], [reservation_confirmed], [reserved_to], [local_codes], [pack_size], [reimbursment_status])
		VALUES
		(@product_FK, @authorisationcountrycode_FK, @organizationmahcode_FK, @product_name, @productshortname, @authorisationnumber, @authorisationstatus_FK, @authorisationdate, @authorisationexpdate, @comment, @authorisationwithdrawndate, @packagedesc, @marketed, @legalstatus, @withdrawndateformat, @mflcode_FK, @qppvcode_person_FK, @product_ID, @ev_code, @XEVPRM_status, @responsible_user_person_FK, @launchdate, @description, @authorised_product_ID, @authorisationdateformat, @evprm_comments, @localnumber, @ap_ID, @shelflife, @productgenericname, @productcompanyname, @productstrenght, @productform, @infodate, @phv_email , @phv_phone, @article_57_reporting, @sunsetclause,@substance_translations, @qppv_code_FK, @local_representative_FK, @Indications, @local_qppv_code_FK, @license_holder_group_FK, @reservation_confirmed, @reserved_to, @local_codes, @pack_size, @reimbursment_status)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
