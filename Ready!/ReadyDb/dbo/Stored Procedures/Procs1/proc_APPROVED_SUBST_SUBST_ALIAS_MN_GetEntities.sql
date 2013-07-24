-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_substance_subst_alias_PK], [approved_substance_FK], [substance_alias_FK]
	FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN]
END
