
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_Delete]
	@subst_clf_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_CLASSIFICATION] WHERE [subst_clf_PK] = @subst_clf_PK
END
