-- Delete
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_Delete]
	@auditing_master_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AUDITING_MASTER] WHERE [auditing_master_PK] = @auditing_master_PK
END
