
-- GetEntity
CREATE PROCEDURE [dbo].[proc_ON_ONJ_MN_GetEntity]
	@on_onj_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_onj_mn_PK], [onj_FK], [on_FK]
	FROM [dbo].[ON_ONJ_MN]
	WHERE [on_onj_mn_PK] = @on_onj_mn_PK
END
