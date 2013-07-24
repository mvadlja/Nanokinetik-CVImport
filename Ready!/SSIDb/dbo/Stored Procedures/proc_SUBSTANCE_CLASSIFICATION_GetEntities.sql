
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subst_clf_PK], [domain], [substance_classification], [sclf_type], [sclf_code]
	FROM [dbo].[SUBSTANCE_CLASSIFICATION]
END
