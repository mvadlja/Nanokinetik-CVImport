
-- Delete
CREATE PROCEDURE [dbo].[proc_PROPERTY_Delete]
	@property_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROPERTY] WHERE [property_PK] = @property_PK
END
