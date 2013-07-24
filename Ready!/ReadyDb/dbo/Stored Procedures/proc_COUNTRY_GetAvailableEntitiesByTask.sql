-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAvailableEntitiesByTask]
	@TaskPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE [dbo].[COUNTRY].country_PK NOT IN
	(	
		SELECT country_FK 
		FROM dbo.[TASK_COUNTRY_MN] 
		WHERE task_FK = @TaskPk AND @TaskPk IS NOT NULL AND country_FK is not null
	)
	ORDER BY [custom_sort_ID] ASC
END