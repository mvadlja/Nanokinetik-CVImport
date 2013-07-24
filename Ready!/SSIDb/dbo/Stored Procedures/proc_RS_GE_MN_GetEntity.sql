
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_GE_MN_GetEntity]
	@rs_ge_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_ge_mn_PK], [rs_FK], [ge_FK]
	FROM [dbo].[RS_GE_MN]
	WHERE [rs_ge_mn_PK] = @rs_ge_mn_PK
END
