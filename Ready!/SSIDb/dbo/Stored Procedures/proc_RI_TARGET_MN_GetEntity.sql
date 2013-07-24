
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_GetEntity]
	@ri_target_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_target_mn_PK], [ri_FK], [target_FK]
	FROM [dbo].[RI_TARGET_MN]
	WHERE [ri_target_mn_PK] = @ri_target_mn_PK
END
