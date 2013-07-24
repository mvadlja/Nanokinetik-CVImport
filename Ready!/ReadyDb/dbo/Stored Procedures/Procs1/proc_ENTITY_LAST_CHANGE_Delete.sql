-- Delete
create PROCEDURE [dbo].[proc_ENTITY_LAST_CHANGE_Delete]
	@last_change_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ENTITY_LAST_CHANGE] WHERE [last_change_PK] = @last_change_PK
END
