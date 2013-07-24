-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_TYPE_MN_Delete]
	@activity_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_TYPE_MN] WHERE [activity_type_PK] = @activity_type_PK
END
