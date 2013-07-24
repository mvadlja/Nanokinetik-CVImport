
-- Save
CREATE PROCEDURE [dbo].[proc_REFERENCE_SOURCE_Save]
	@reference_source_PK int = NULL,
	@public_domain bit = NULL,
	@rs_type_FK int = NULL,
	@rs_class_FK int = NULL,
	@rs_id varchar(12) = NULL,
	@rs_citation varchar(2500) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REFERENCE_SOURCE]
	SET
	[public_domain] = @public_domain,
	[rs_type_FK] = @rs_type_FK,
	[rs_class_FK] = @rs_class_FK,
	[rs_id] = @rs_id,
	[rs_citation] = @rs_citation
	WHERE [reference_source_PK] = @reference_source_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REFERENCE_SOURCE]
		([public_domain], [rs_type_FK], [rs_class_FK], [rs_id], [rs_citation])
		VALUES
		(@public_domain, @rs_type_FK, @rs_class_FK, @rs_id, @rs_citation)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
