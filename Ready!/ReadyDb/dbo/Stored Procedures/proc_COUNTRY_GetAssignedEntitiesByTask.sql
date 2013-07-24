-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAssignedEntitiesByTask]
	@TaskPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	JOIN [dbo].[TASK_COUNTRY_MN] ON [dbo].[TASK_COUNTRY_MN].country_FK = [dbo].[COUNTRY].country_PK
	WHERE [dbo].[TASK_COUNTRY_MN].task_FK = @TaskPk AND @TaskPk IS NOT NULL
	ORDER BY [dbo].[COUNTRY].[custom_sort_ID] ASC
END