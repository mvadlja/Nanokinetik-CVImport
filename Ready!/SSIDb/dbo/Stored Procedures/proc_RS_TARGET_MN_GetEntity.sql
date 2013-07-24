
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_TARGET_MN_GetEntity]
	@rs_target_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_target_mn_PK], [rs_FK], [target_FK]
	FROM [dbo].[RS_TARGET_MN]
	WHERE [rs_target_mn_PK] = @rs_target_mn_PK
END
