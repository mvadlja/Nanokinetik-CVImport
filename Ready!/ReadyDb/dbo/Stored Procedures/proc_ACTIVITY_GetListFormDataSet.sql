CREATE PROCEDURE [dbo].[proc_ACTIVITY_GetListFormDataSet]
	@ActivityName nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@RegulatoryStatus nvarchar(250) = NULL,
	@SubmissionDate nvarchar(250) = NULL,
	@StartDate nvarchar(250) = NULL,
	@ApprovalDate nvarchar(250) = NULL,
	@InternalStatus nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,
	@ActivityID nvarchar(100) = NULL,

	@TasksCount nvarchar(250) = NULL,
	@TimeCount nvarchar(250) = NULL,
	@DocumentsCount nvarchar(250) = NULL,
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'activity_PK'
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
		Activity.* FROM
		(
			SELECT DISTINCT
			Activity.activity_PK,
			''ActivityName'' = CASE 
				WHEN ((Activity.procedure_number = '''' OR Activity.procedure_number IS NULL) AND (Activity.name = '''' OR Activity.name IS NULL)) THEN ''Missing''
				WHEN ((Activity.procedure_number = '''' OR Activity.procedure_number IS NULL) AND (Activity.name != '''' AND Activity.name IS NOT NULL)) THEN Activity.name
				ELSE Activity.name + '' ('' + Activity.procedure_number + '')'' 
			END,
			dbo.ReturnCountriesOfActivityAbbrevated(Activity.activity_PK) AS Countries,
			dbo.ReturnProductsByActivity(Activity.activity_PK) AS Products,
			Activity.user_FK as ResponsibleUserFk,
			Activity.submission_date AS SubmissionDate,
			Activity.start_date AS StartDate,
			Activity.approval_date AS ApprovalDate,
			Activity.expected_finished_date AS ExpectedFinishedDate,
			Activity.activity_Id AS ActivityID,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Activity.regulatory_status_FK) AS RegulatoryStatus,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Activity.internal_status_FK) AS InternalStatus,
			(SELECT COUNT(*) from dbo.TASK WHERE dbo.TASK.activity_FK = Activity.activity_PK) as TasksCount,
			(SELECT COUNT(*) from dbo.TIME_UNIT	WHERE dbo.TIME_UNIT.activity_FK = Activity.activity_PK) as TimeCount,
			(SELECT COUNT(*) from dbo.ACTIVITY_DOCUMENT_MN WHERE dbo.ACTIVITY_DOCUMENT_MN.activity_FK = Activity.activity_PK) AS DocumentsCount
			'
			IF @QueryBy = 'Product' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[ACTIVITY_PRODUCT_MN] activityProductMn
								 LEFT JOIN [dbo].[ACTIVITY] Activity ON Activity.activity_PK = activityProductMn.activity_FK
								 LEFT JOIN [dbo].[PRODUCT] Product ON Product.product_PK = activityProductMn.product_FK
								 WHERE activityProductMn.product_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
		    ELSE IF @QueryBy = 'Project'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[ACTIVITY_PROJECT_MN] activityProjectMn
								 LEFT JOIN [dbo].[ACTIVITY] Activity ON Activity.activity_PK = activityProjectMn.activity_FK
								 LEFT JOIN [dbo].[PROJECT] Project ON Project.project_PK = activityProjectMn.project_FK
								 WHERE activityProjectMn.project_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'ActivityMyAlerts'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[ACTIVITY] Activity
								 WHERE Activity.expected_finished_date <=GETDATE() AND (select name from [TYPE] where [TYPE].type_PK=internal_status_FK) not in (''finished'') 
								 '
			END
			ELSE SET @Query = @Query + 
								'FROM [dbo].[ACTIVITY] Activity
								 '
			SET @Query = @Query + 
			') AS Activity 
		'
		SET @TempWhereQuery = '';

		-- @ActivityName
		IF @ActivityName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ActivityName LIKE N''%' + REPLACE(REPLACE(@ActivityName,'[','[[]'),'''','''''') + '%'''
		END

		-- @Products
		IF @Products IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.Products LIKE N''%' + REPLACE(REPLACE(@Products,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Countries
		IF @Countries IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.Countries LIKE N''%' + REPLACE(REPLACE(@Countries,'[','[[]'),'''','''''') + '%'''
		END

		-- @RegulatoryStatus
		IF @RegulatoryStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.RegulatoryStatus LIKE N''' + REPLACE(REPLACE(@RegulatoryStatus,'[','[[]'),'''','''''') + ''''
		END
		
		-- @InternalStatus
		IF @InternalStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.InternalStatus LIKE N''' + REPLACE(REPLACE(@InternalStatus,'[','[[]'),'''','''''') + ''''
		END
		
		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
		END

		-- @SubmissionDate
		IF @SubmissionDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Activity.SubmissionDate,104) LIKE ''%' + REPLACE(REPLACE(@SubmissionDate,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @StartDate
		IF @StartDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Activity.StartDate,104) LIKE ''%' + REPLACE(REPLACE(@StartDate,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @ApprovalDate
		IF @ApprovalDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Activity.ApprovalDate,104) LIKE ''%' + REPLACE(REPLACE(@ApprovalDate,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @TasksCount
		IF @TasksCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.TasksCount LIKE N''%' + REPLACE(REPLACE(@TasksCount,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @TimeCount
		IF @TimeCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.TimeCount LIKE N''%' + REPLACE(REPLACE(@TimeCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActivityID
		IF @ActivityID IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ActivityID LIKE N''%' + REPLACE(REPLACE(@ActivityID,'[','[[]'),'''','''''') + '%'''
		END

		-- @DocumentsCount
		IF @DocumentsCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.DocumentsCount LIKE N''%' + REPLACE(REPLACE(@DocumentsCount,'[','[[]'),'''','''''') + '%'''
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