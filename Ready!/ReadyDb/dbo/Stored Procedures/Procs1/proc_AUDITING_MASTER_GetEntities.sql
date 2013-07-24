-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AUDITING_MASTER_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[auditing_master_PK], [username], [db_name], [table_name], [date], [operation], [server_name]
	FROM [dbo].[AUDITING_MASTER]
END
