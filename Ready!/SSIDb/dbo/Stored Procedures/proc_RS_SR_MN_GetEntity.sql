
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_SR_MN_GetEntity]
	@rs_sr_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sr_mn_PK], [rs_FK], [sr_FK]
	FROM [dbo].[RS_SR_MN]
	WHERE [rs_sr_mn_PK] = @rs_sr_mn_PK
END
