-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TASK_COUNTRY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_country_PK], [task_FK], [country_FK]
	FROM [dbo].[TASK_COUNTRY_MN]
END
