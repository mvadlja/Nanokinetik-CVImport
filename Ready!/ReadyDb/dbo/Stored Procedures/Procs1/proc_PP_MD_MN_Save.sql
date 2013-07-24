-- Save
CREATE PROCEDURE  [dbo].[proc_PP_MD_MN_Save]
	@pp_md_mn_PK int = NULL,
	@pp_medical_device_FK int = NULL,
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_MD_MN]
	SET
	[pp_medical_device_FK] = @pp_medical_device_FK,
	[pharmaceutical_product_FK] = @pharmaceutical_product_FK
	WHERE [pp_md_mn_PK] = @pp_md_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_MD_MN]
		([pp_medical_device_FK], [pharmaceutical_product_FK])
		VALUES
		(@pp_medical_device_FK, @pharmaceutical_product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
