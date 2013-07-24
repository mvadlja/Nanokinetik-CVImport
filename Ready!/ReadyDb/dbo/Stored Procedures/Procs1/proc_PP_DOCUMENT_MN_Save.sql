-- Save
CREATE PROCEDURE  [dbo].[proc_PP_DOCUMENT_MN_Save]
	@pp_document_PK int = NULL,
	@pp_FK int = NULL,
	@doc_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_DOCUMENT_MN]
	SET
	[pp_FK] = @pp_FK,
	[doc_FK] = @doc_FK
	WHERE [pp_document_PK] = @pp_document_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_DOCUMENT_MN]
		([pp_FK], [doc_FK])
		VALUES
		(@pp_FK, @doc_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
