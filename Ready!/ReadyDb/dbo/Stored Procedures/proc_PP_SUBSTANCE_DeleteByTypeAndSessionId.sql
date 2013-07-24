
-- Delete
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_DeleteByTypeAndSessionId]
	@SubstanceType nvarchar(50) = NULL,
	@SessionId nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_SUBSTANCE] WHERE substancetype = @SubstanceType AND sessionid = @SessionId AND @SubstanceType IS NOT NULL AND @SessionId IS NOT NULL
END