CREATE PROCEDURE  [dbo].[proc_TASK_GetListFormDataSet]
	@TaskName nvarchar(250) = NULL,
	@ActivityName nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@StartDate nvarchar(250) = NULL,
	@ExpectedFinishedDate nvarchar(250) = NULL,
	@ActualFinishedDate nvarchar(250) = NULL,
	@InternalStatus nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,

	@SUcount nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'task_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		Task.* FROM
		(
			SELECT DISTINCT
			task.task_PK,
			task.user_FK as ResponsibleUserFk,
			a.activity_PK as activityPk,
			ISNULL(a.name, ''Missing'') as ActivityName,
		    (SELECT task_name FROM [dbo].[TASK_NAME] taskName WHERE taskName.task_name_PK = task.task_name_FK) AS TaskName,
			dbo.[ReturnTaskCountries](task.task_PK) AS Countries,
			dbo.ReturnProductsByActivity(a.activity_PK) AS Products,
			task.[start_date] AS StartDate,
			task.expected_finished_date AS ExpectedFinishedDate,
			task.actual_finished_date AS ActualFinishedDate,	
			(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = task.type_internal_status_FK) AS InternalStatus,
			(select COUNT(*) from SUBMISSION_UNIT as su where su.task_FK = task_PK) AS SUcount,
			(select count(*) from dbo.TASK_DOCUMENT_MN where dbo.TASK_DOCUMENT_MN.task_FK = task.task_PK) AS DocCount
			'
			IF @QueryBy = 'Activity' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[TASK] Task
								 LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = task.[activity_FK]
								 WHERE task.[activity_FK] = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'TaskMyAlerts' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[TASK] Task
								 LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = task.[activity_FK]
								 where Task.expected_finished_date <=GETDATE() AND (select name from [TYPE] where [TYPE].type_PK=type_internal_status_FK) not in (''finished'')
								 '
			END
			ELSE SET @Query = @Query + 
								'FROM [dbo].[TASK] Task
								 LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = task.[activity_FK]
								 '
			SET @Query = @Query + 
			') AS Task 
		'
		SET @TempWhereQuery = '';

		-- @TaskName
		IF @TaskName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.TaskName LIKE N''%' + REPLACE(REPLACE(@TaskName,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActivityName
		IF @ActivityName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ActivityName LIKE N''%' + REPLACE(REPLACE(@ActivityName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Products
		IF @Products IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.Products LIKE N''%' + REPLACE(REPLACE(@Products,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Countries
		IF @Countries IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.Countries LIKE N''%' + REPLACE(REPLACE(@Countries,'[','[[]'),'''','''''') + '%'''
		END

		-- @InternalStatus
		IF @InternalStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.InternalStatus LIKE N''' + REPLACE(REPLACE(@InternalStatus,'[','[[]'),'''','''''') + ''''
		END

		-- @StartDate
		IF @StartDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Task.StartDate,104) LIKE ''%' + REPLACE(REPLACE(@StartDate,'[','[[]'),'''','''''') + '%'''
		END	

		-- @ExpectedFinishedDate
		IF @ExpectedFinishedDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Task.ExpectedFinishedDate,104) LIKE ''%' + REPLACE(REPLACE(@ExpectedFinishedDate,'[','[[]'),'''','''''') + '%'''
		END	

		-- @aActualFinishedDate
		IF @ActualFinishedDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Task.ActualFinishedDate,104) LIKE ''%' + REPLACE(REPLACE(@ActualFinishedDate,'[','[[]'),'''','''''') + '%'''
		END	
		-- @SUcount
		IF @SUcount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.SUcount LIKE N''%' + REPLACE(REPLACE(@SUcount,'[','[[]'),'''','''''') + '%'''
		END

		-- @DocCount
		IF @DocCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.DocCount LIKE N''%' + REPLACE(REPLACE(@DocCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	')
	
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END