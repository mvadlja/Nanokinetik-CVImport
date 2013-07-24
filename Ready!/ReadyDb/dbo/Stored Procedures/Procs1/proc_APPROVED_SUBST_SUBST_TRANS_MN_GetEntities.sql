-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_TRANS_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_subst_trans_PK], [approved_substance_FK], [substance_translations_FK]
	FROM [dbo].[APPROVED_SUBST_SUBST_TRANS_MN]
END
