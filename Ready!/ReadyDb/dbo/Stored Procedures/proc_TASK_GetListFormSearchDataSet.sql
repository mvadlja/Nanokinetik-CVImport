CREATE PROCEDURE  [dbo].[proc_TASK_GetListFormSearchDataSet]
	@SearchActivityPk nvarchar(250) = NULL,
	@SearchTaskName nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchInternalStatusPk nvarchar(250) = NULL,
	@SearchCountryPk nvarchar(250) = NULL,
	@SearchStartDateFrom nvarchar(250) = NULL,
	@SearchStartDateTo nvarchar(250) = NULL,
	@SearchExpectedFinishedDateFrom nvarchar(250) = NULL,
	@SearchExpectedFinishedDateTo nvarchar(250) = NULL,
	@SearchActualFinishedDateFrom nvarchar(250) = NULL,
	@SearchActualFinishedDateTo nvarchar(250) = NULL,
	
	@TaskName nvarchar(250) = NULL,
	@ActivityName nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@StartDate nvarchar(250) = NULL,
	@ExpectedFinishedDate nvarchar(250) = NULL,
	@ActualFinishedDate nvarchar(250) = NULL,
	@InternalStatus nvarchar(250) = NULL,

	@SUCount nvarchar(250) = NULL,
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
		SELECT DISTINCT task_PK, activityPk, TaskName, ActivityName, Products, Countries, InternalStatus, StartDate, ExpectedFinishedDate, ActualFinishedDate, SUcount, DocCount
		FROM
		(
			SELECT Task.* 
			FROM
			(
				SELECT DISTINCT
				task.task_PK,
				a.activity_PK as activityPk,
				ISNULL(a.name, ''Missing'') as ActivityName,
				task.type_internal_status_FK,
				(SELECT task_name FROM [dbo].[TASK_NAME] taskName WHERE taskName.task_name_PK = task.task_name_FK) AS TaskName,
				dbo.[ReturnTaskCountries](task.task_PK) AS Countries,
				dbo.ReturnProductsByActivity(a.activity_PK) AS Products,
				task.[start_date] AS StartDate,
				task.user_FK,
				task.expected_finished_date AS ExpectedFinishedDate,
				task.actual_finished_date AS ActualFinishedDate,	
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = task.type_internal_status_FK) AS InternalStatus,
				(select COUNT(*) from SUBMISSION_UNIT as su where su.task_FK = task_PK) AS SUcount,
				(select count(*) from dbo.TASK_DOCUMENT_MN where dbo.TASK_DOCUMENT_MN.task_FK = task.task_PK) AS DocCount'

			IF @QueryBy = 'ByActivity' 
			BEGIN 
				SET @Query = @Query + '
				FROM [dbo].[TASK] Task
				LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = task.[activity_FK]
				WHERE task.[activity_FK] = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
			END
			ELSE SET @Query = @Query + '
				FROM [dbo].[TASK] Task
				LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = task.[activity_FK]'
			SET @Query = @Query + '
			) AS Task 
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
		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

		-- @SearchActivityPk
		IF @SearchActivityPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.activityPk = ' + REPLACE(REPLACE(@SearchActivityPk,'[','[[]'),'''','''''')
		END

		-- @SearchTaskName
		IF @SearchTaskName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.TaskName LIKE N''%' + REPLACE(REPLACE(@SearchTaskName,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.user_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END

		-- @SearchInternalStatusPk
		IF @SearchInternalStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.type_internal_status_FK = ' + REPLACE(REPLACE(@SearchInternalStatusPk,'[','[[]'),'''','''''')
		END

		-- @SearchCountryPk
		IF @SearchCountryPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchCountryPk,'[','[[]'),'''','''''') + ' in (SELECT tcMN.country_FK from dbo.TASK_COUNTRY_MN tcMN WHERE tcMN.task_FK = Task.task_PK AND tcMN.country_FK IS NOT NULL)'
		END

		-- @SearchStartDateFrom
		IF @SearchStartDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.StartDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchStartDateTo
		IF @SearchStartDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.StartDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchExpectedFinishedDateFrom
		IF @SearchExpectedFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ExpectedFinishedDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchExpectedFinishedDateTo
		IF @SearchExpectedFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ExpectedFinishedDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchActualFinishedDateFrom
		IF @SearchActualFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ActualFinishedDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchActualFinishedDateTo
		IF @SearchActualFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Task.ActualFinishedDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + '
		) DistinctResult
	)
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,*
		FROM PagingResult
	)x
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END