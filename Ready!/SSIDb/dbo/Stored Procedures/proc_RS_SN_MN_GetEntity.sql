
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_SN_MN_GetEntity]
	@rs_sn_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sn_mn_PK], [rs_FK], [substance_name_FK]
	FROM [dbo].[RS_SN_MN]
	WHERE [rs_sn_mn_PK] = @rs_sn_mn_PK
END
