
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_SCLF_MN_GetEntity]
	@rs_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sclf_mn_PK], [sclf_FK], [rs_FK]
	FROM [dbo].[RS_SCLF_MN]
	WHERE [rs_sclf_mn_PK] = @rs_sclf_mn_PK
END
