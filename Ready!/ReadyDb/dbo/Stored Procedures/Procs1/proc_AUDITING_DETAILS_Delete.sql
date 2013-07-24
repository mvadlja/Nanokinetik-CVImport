-- Delete
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_Delete]
	@auditing_detail_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AUDITING_DETAILS] WHERE [auditing_detail_PK] = @auditing_detail_PK
END
