-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_sub_alias_trans_PK], [approved_substance_FK], [substance_alias_translation_FK]
	FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
END
