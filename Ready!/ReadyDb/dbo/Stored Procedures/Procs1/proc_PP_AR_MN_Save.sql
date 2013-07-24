-- Save
CREATE PROCEDURE  [dbo].[proc_PP_AR_MN_Save]
	@pp_ar_mn_PK int = NULL,
	@admin_route_FK int = NULL,
	@pharmaceutical_product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_AR_MN]
	SET
	[admin_route_FK] = @admin_route_FK,
	[pharmaceutical_product_FK] = @pharmaceutical_product_FK
	WHERE [pp_ar_mn_PK] = @pp_ar_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_AR_MN]
		([admin_route_FK], [pharmaceutical_product_FK])
		VALUES
		(@admin_route_FK, @pharmaceutical_product_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
