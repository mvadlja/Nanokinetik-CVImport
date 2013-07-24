-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_GetEntity]
	@auditing_master_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[auditing_master_PK], [username], [db_name], [table_name], [date], [operation], [server_name]
	FROM [dbo].[AUDITING_MASTER]
	WHERE [auditing_master_PK] = @auditing_master_PK
END
