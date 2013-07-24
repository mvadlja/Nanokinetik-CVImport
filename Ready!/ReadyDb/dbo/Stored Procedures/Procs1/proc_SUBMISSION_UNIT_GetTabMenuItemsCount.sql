-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_GetTabMenuItemsCount]
	@su_PK int = NULL
AS
DECLARE @task_PK int = NULL
DECLARE @Activity_PK int = NULL
BEGIN
	SET NOCOUNT ON;
	SELECT @task_PK = dbo.SUBMISSION_UNIT.task_FK 
	from dbo.SUBMISSION_UNIT 
	where dbo.SUBMISSION_UNIT.subbmission_unit_PK = @su_PK
	
	if @task_PK is not null
		SELECT @Activity_PK = dbo.TASK.activity_FK 
		from dbo.TASK  
		where dbo.TASK.task_PK = @task_PK
	
	SELECT  'SubUnitActPreview' = case when @Activity_PK is not null then '1' else '0' end,
			'SubUnitTaskPreview' = case when @task_PK is not null then '1' else '0'	end
END
