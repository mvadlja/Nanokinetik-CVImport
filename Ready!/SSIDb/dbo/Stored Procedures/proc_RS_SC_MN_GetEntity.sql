
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_SC_MN_GetEntity]
	@rs_sc_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sc_mn_PK], [rs_FK], [sc_FK]
	FROM [dbo].[RS_SC_MN]
	WHERE [rs_sc_mn_PK] = @rs_sc_mn_PK
END
