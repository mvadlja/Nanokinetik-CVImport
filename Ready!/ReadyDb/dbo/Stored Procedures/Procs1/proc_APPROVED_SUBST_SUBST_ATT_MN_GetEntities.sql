-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ATT_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_subst_att_PK], [approved_substance_FK], [substance_attachment_FK]
	FROM [dbo].[APPROVED_SUBST_SUBST_ATT_MN]
END
