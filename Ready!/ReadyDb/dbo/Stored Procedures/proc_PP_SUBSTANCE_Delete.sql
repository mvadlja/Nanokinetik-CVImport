
-- Delete
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_Delete]
	@ppsubstance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_SUBSTANCE] WHERE [ppsubstance_PK] = @ppsubstance_PK
END