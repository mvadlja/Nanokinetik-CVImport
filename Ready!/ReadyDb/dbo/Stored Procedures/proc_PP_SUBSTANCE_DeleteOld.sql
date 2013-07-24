
-- Delete
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_DeleteOld]
	@ppsubstance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_SUBSTANCE] WHERE DATEADD(DAY, 7, modifieddate) < GETDATE()
END