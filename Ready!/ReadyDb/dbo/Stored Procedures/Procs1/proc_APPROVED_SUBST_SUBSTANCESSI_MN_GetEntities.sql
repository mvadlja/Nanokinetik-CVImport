-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBSTANCESSI_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_substancessi_PK], [approved_substance_FK], [substancessi_FK]
	FROM [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
END
