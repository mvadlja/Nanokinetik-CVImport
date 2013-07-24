
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_NAME_MN_Delete]
	@substance_substance_name_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN] WHERE [substance_substance_name_mn_PK] = @substance_substance_name_mn_PK
END
