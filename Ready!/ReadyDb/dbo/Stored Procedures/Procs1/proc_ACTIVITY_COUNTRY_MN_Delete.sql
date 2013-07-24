-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_Delete]
	@activity_country_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_COUNTRY_MN] WHERE [activity_country_PK] = @activity_country_PK
END
