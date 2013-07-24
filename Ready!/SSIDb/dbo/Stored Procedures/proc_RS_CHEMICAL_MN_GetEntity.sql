
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RS_CHEMICAL_MN_GetEntity]
	@rs_chemical_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_chemical_mn_PK], [rs_FK], [chemical_FK]
	FROM [dbo].[RS_CHEMICAL_MN]
	WHERE [rs_chemical_mn_PK] = @rs_chemical_mn_PK
END
