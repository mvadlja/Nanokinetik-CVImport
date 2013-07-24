-- GetAuthorisedProductsDataSet
CREATE PROCEDURE  [dbo].[proc_TASK_GetTasksByActivity]
	@activity_PK int = NULL
	as
	begin
			SELECT DISTINCT
			[dbo].[TASK].*,
			t.task_name,
			a.name as activity_name
			
			FROM [dbo].[TASK]
			LEFT JOIN [dbo].[TASK_NAME] t ON t.task_name_PK = [dbo].[TASK].[task_name_FK]
			LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = [dbo].[TASK].[activity_FK]
			where a.activity_PK=@activity_PK
			
END
