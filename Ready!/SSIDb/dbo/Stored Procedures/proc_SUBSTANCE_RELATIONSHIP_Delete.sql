
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_RELATIONSHIP_Delete]
	@substance_relationship_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_RELATIONSHIP] WHERE [substance_relationship_PK] = @substance_relationship_PK
END
