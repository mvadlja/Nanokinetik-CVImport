
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_Save]
	@subst_clf_PK int = NULL,
	@domain varchar(250) = NULL,
	@substance_classification varchar(250) = NULL,
	@sclf_type varchar(250) = NULL,
	@sclf_code varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_CLASSIFICATION]
	SET
	[domain] = @domain,
	[substance_classification] = @substance_classification,
	[sclf_type] = @sclf_type,
	[sclf_code] = @sclf_code
	WHERE [subst_clf_PK] = @subst_clf_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_CLASSIFICATION]
		([domain], [substance_classification], [sclf_type], [sclf_code])
		VALUES
		(@domain, @substance_classification, @sclf_type, @sclf_code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
