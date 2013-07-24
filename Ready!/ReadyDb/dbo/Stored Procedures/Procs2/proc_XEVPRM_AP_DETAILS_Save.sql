-- Save
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_Save]
	@xevprm_ap_details_PK int = NULL,
	@ap_FK int = NULL,
	@ap_name nvarchar(2000) = NULL,
	@package_description nvarchar(2000) = NULL,
	@authorisation_country_code nvarchar(50) = NULL,
	@related_product_FK int = NULL,
	@related_product_name nvarchar(2000) = NULL,
	@licence_holder nvarchar(100) = NULL,
	@authorisation_status nvarchar(50) = NULL,
	@authorisation_number nvarchar(100) = NULL,
	@operation_type int = NULL,
	@ev_code nvarchar(60) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XEVPRM_AP_DETAILS]
	SET
	[ap_FK] = @ap_FK,
	[ap_name] = @ap_name,
	[package_description] = @package_description,
	[authorisation_country_code] = @authorisation_country_code,
	[related_product_FK] = @related_product_FK,
	[related_product_name] = @related_product_name,
	[licence_holder] = @licence_holder,
	[authorisation_status] = @authorisation_status,
	[authorisation_number] = @authorisation_number,
	[operation_type] = @operation_type,
	[ev_code] = @ev_code
	WHERE [xevprm_ap_details_PK] = @xevprm_ap_details_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XEVPRM_AP_DETAILS]
		([ap_FK], [ap_name], [package_description], [authorisation_country_code], [related_product_FK], [related_product_name], [licence_holder], [authorisation_status], [authorisation_number], [operation_type], [ev_code])
		VALUES
		(@ap_FK, @ap_name, @package_description, @authorisation_country_code, @related_product_FK, @related_product_name, @licence_holder, @authorisation_status, @authorisation_number, @operation_type, @ev_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
