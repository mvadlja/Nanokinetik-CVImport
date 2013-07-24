-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TASK_NAME_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_name_PK], [task_name]
	FROM [dbo].[TASK_NAME]
END
