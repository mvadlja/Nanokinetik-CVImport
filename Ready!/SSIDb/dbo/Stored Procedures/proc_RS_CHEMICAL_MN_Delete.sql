
-- Delete
CREATE PROCEDURE [dbo].[proc_RS_CHEMICAL_MN_Delete]
	@rs_chemical_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[RS_CHEMICAL_MN] WHERE [rs_chemical_mn_PK] = @rs_chemical_mn_PK
END
