CREATE PROCEDURE [dbo].[proc_SUBMISSION_UNIT_GetListFormDataSet]
	@SubmissionUnitDescription nvarchar(250) = NULL,
	@Attachments nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@ActivityName nvarchar(250) = NULL,
	@TaskName nvarchar(250) = NULL,
	@SubmissionId nvarchar(250) = NULL,
	@SubmissionFormat nvarchar(250) = NULL,
	@Sequence nvarchar(250) = NULL,
	@DispatchDate nvarchar(250) = NULL,
	@Agency nvarchar(250) = NULL,
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'subbmission_unit_PK'
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
		SubmissionUnit.* FROM
		(
			SELECT DISTINCT
			SubmissionUnit.subbmission_unit_PK,
			(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = SubmissionUnit.description_type_FK) AS SubmissionUnitDescription,
			''Attachments'' = CASE 
				WHEN SubmissionUnit.ness_FK IS NOT NULL AND SubmissionUnit.document_FK is not null THEN ([dbo].[ReturnAttachmentByDoc](SubmissionUnit.document_FK) + '' '' + [dbo].[ReturnAttachmentByDoc](SubmissionUnit.ness_FK))
				WHEN SubmissionUnit.ness_FK IS NOT NULL AND SubmissionUnit.document_FK is null THEN ([dbo].[ReturnAttachmentByDoc](SubmissionUnit.ness_FK))
				WHEN SubmissionUnit.ectd_FK IS NOT NULL AND SubmissionUnit.document_FK is not null THEN ([dbo].[ReturnAttachmentByDoc](SubmissionUnit.document_FK) + '' '' + [dbo].[ReturnAttachmentByDoc](SubmissionUnit.ectd_FK))
				WHEN SubmissionUnit.ectd_FK IS NOT NULL AND SubmissionUnit.document_FK is null THEN ([dbo].[ReturnAttachmentByDoc](SubmissionUnit.ectd_FK))
				ELSE [dbo].[ReturnAttachmentByDoc](SubmissionUnit.document_FK)
			END,
			dbo.ReturnProductForSU(SubmissionUnit.subbmission_unit_PK) as Products,
			activity.activity_PK AS activityPk,
			activity.name as ActivityName,
			task.task_PK AS taskPk,
			taskName.task_name as TaskName,
			SubmissionUnit.submission_ID AS SubmissionId,
			(SELECT name FROM dbo.TYPE WHERE type_PK = s_format_FK) AS SubmissionFormat,
			SubmissionUnit.dispatch_date AS DispatchDate,
			dbo.ReturnAgenciesForSU(SubmissionUnit.subbmission_unit_PK) AS Agency,
			SubmissionUnit.document_FK,
			SubmissionUnit.ness_FK,
			SubmissionUnit.ectd_FK,
			SubmissionUnit.sequence AS Sequence
			'
			IF @QueryBy = 'Activity' OR @QueryBy = 'ActivityMy'
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[SUBMISSION_UNIT] SubmissionUnit
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK
								 WHERE activity.activity_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'Task' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[SUBMISSION_UNIT] SubmissionUnit
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK
								 WHERE task.task_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'Product' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[PRODUCT_SUB_UNIT_MN] as suMN 
								 LEFT JOIN [dbo].[SUBMISSION_UNIT] SubmissionUnit on suMN.submission_unit_FK = SubmissionUnit.subbmission_unit_PK
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK
								 WHERE suMN.product_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE SET @Query = @Query + 
								'FROM [dbo].[SUBMISSION_UNIT] SubmissionUnit
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK	
								 '
			SET @Query = @Query + 
			') AS SubmissionUnit 
		'
		SET @TempWhereQuery = '';

		-- @SubmissionUnitDescription
		IF @SubmissionUnitDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.SubmissionUnitDescription LIKE N''%' + REPLACE(REPLACE(@SubmissionUnitDescription,'[','[[]'),'''','''''') + '%'''
		END

		-- @Attachments
		IF @Attachments IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.Attachments LIKE N''%' + REPLACE(REPLACE(@Attachments,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActivityName
		IF @ActivityName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.ActivityName LIKE N''%' + REPLACE(REPLACE(@ActivityName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Products
		IF @Products IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.Products LIKE N''%' + REPLACE(REPLACE(@Products,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @TaskName
		IF @TaskName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.TaskName LIKE N''%' + REPLACE(REPLACE(@TaskName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SubmissionId
		IF @SubmissionId IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.SubmissionId LIKE N''%' + REPLACE(REPLACE(@SubmissionId,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SubmissionFormat
		IF @SubmissionFormat IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.SubmissionFormat LIKE N''' + REPLACE(REPLACE(@SubmissionFormat,'[','[[]'),'''','''''') + ''''
		END
		
		-- @Sequence
		IF @Sequence IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.Sequence LIKE N''%' + REPLACE(REPLACE(@Sequence,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Agency
		IF @Agency IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.Agency LIKE N''%' + REPLACE(REPLACE(@Agency,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @DispatchDate
		IF @DispatchDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), SubmissionUnit.DispatchDate,104) LIKE ''%' + REPLACE(REPLACE(@DispatchDate,'[','[[]'),'''','''''') + '%'''
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