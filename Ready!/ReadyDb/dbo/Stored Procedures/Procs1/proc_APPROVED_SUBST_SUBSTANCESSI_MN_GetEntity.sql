-- GetEntity
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBSTANCESSI_MN_GetEntity]
	@approved_subst_substancessi_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_subst_substancessi_PK], [approved_substance_FK], [substancessi_FK]
	FROM [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
	WHERE [approved_subst_substancessi_PK] = @approved_subst_substancessi_PK
END
