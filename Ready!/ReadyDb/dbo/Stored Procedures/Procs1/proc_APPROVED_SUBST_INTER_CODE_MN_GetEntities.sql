-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_INTER_CODE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_inter_code_PK], [approved_substance_FK], [international_code_FK]
	FROM [dbo].[APPROVED_SUBST_INTER_CODE_MN]
END
