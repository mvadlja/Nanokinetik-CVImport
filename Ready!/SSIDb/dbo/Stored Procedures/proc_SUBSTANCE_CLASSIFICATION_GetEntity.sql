
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_GetEntity]
	@subst_clf_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subst_clf_PK], [domain], [substance_classification], [sclf_type], [sclf_code]
	FROM [dbo].[SUBSTANCE_CLASSIFICATION]
	WHERE [subst_clf_PK] = @subst_clf_PK
END
