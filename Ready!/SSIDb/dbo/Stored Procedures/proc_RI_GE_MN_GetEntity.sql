
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RI_GE_MN_GetEntity]
	@ri_ge_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_ge_mn_PK], [ri_FK], [ge_FK]
	FROM [dbo].[RI_GE_MN]
	WHERE [ri_ge_mn_PK] = @ri_ge_mn_PK
END
