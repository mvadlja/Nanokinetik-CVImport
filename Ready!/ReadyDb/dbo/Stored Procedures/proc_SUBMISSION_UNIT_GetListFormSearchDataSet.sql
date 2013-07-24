CREATE PROCEDURE [dbo].[proc_SUBMISSION_UNIT_GetListFormSearchDataSet]
	@SearchProductPk nvarchar(250) = NULL,
	@SearchActivityPk nvarchar(250) = NULL,
	@SearchTaskPk nvarchar(250) = NULL,
	@SearchSubmissionUnitDescriptionPk nvarchar(250) = NULL,
	@SearchAgencyPk nvarchar(250) = NULL,
	@SearchRmsPk nvarchar(250) = NULL,
	@SearchSubmissionId nvarchar(250) = NULL,
	@SearchSubmissionFormatPk nvarchar(250) = NULL,
	@SearchSequenceId nvarchar(250) = NULL,
	@SearchDtdSchemaVersionPk nvarchar(250) = NULL,
	@SearchDispatchDateFrom nvarchar(250) = NULL,
	@SearchDispatchDateTo nvarchar(250) = NULL,
	@SearchReceiptDateFrom nvarchar(250) = NULL,
	@SearchReceiptDateTo nvarchar(250) = NULL,
	
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
		SELECT DISTINCT subbmission_unit_PK, SubmissionUnitDescription, Attachments, Products, activityPk, ActivityName, taskPk, TaskName, SubmissionId, SubmissionFormat, Sequence,DispatchDate, Agency, agency_role_FK, document_FK, ness_FK, ectd_FK
		FROM
		(
			SELECT SubmissionUnit.* 
			FROM
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
			(SELECT name FROM dbo.TYPE WHERE type_PK = SubmissionUnit.s_format_FK) AS SubmissionFormat,
			SubmissionUnit.dispatch_date AS DispatchDate,
			SubmissionUnit.receipt_date AS ReceiptDate,
			dbo.ReturnAgenciesForSU(SubmissionUnit.subbmission_unit_PK) AS Agency,
			SubmissionUnit.document_FK,
			SubmissionUnit.ness_FK,
			SubmissionUnit.ectd_FK,
			SubmissionUnit.sequence AS Sequence,
			SubmissionUnit.dtd_schema_FK,
			SubmissionUnit.agency_role_FK,
			SubmissionUnit.description_type_FK,
			SubmissionUnit.s_format_FK 
			'
			IF @QueryBy = 'ByActivity' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[SUBMISSION_UNIT] SubmissionUnit
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK
								 WHERE activity.activity_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'ByTask' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[SUBMISSION_UNIT] SubmissionUnit
								 LEFT JOIN dbo.TASK task ON SubmissionUnit.task_FK = task.task_PK
								 LEFT JOIN dbo.TASK_NAME taskName ON task.task_name_FK = taskName.task_name_PK
								 LEFT JOIN ACTIVITY activity ON activity.activity_PK = task.activity_FK
								 WHERE task.task_PK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'ByProduct' 
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
		
		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------
		
		-- @SearchProductPk
		IF @SearchProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchProductPk,'[','[[]'),'''','''''') + ' IN (SELECT spMN.product_FK from dbo.PRODUCT_SUB_UNIT_MN spMN WHERE spMN.submission_unit_FK = SubmissionUnit.subbmission_unit_PK AND spMN.product_FK IS NOT NULL)'
		END
		
		-- @SearchActivityPk
		IF @SearchActivityPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.activityPk = ' + REPLACE(REPLACE(@SearchActivityPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchTaskPk
		IF  @SearchTaskPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.taskPk = ' + REPLACE(REPLACE(@SearchTaskPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchSubmissionUnitDescriptionPk
		IF  @SearchSubmissionUnitDescriptionPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.description_type_FK = ' + REPLACE(REPLACE(@SearchSubmissionUnitDescriptionPk,'[','[[]'),'''','''''')
		END
	
		-- @SearchAgencyPk
		IF @SearchAgencyPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchAgencyPk,'[','[[]'),'''','''''') + ' in (SELECT saMN.agency_FK from dbo.SU_AGENCY_MN saMN WHERE saMN.submission_unit_FK = SubmissionUnit.subbmission_unit_PK AND saMN.agency_FK IS NOT NULL)'
		END
		
		-- @SearchRmsPk
		IF  @SearchRmsPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.agency_role_FK = ' + REPLACE(REPLACE(@SearchRmsPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchSubmissionId
		IF @SearchSubmissionId IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.SubmissionId LIKE N''%' + REPLACE(REPLACE(@SearchSubmissionId,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchSubmissionFormatPk
		IF  @SearchSubmissionFormatPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.s_format_FK = ' + REPLACE(REPLACE(@SearchSubmissionFormatPk,'[','[[]'),'''','''''')
		END
		
		-- @SearchSequenceId
		IF @SearchSequenceId IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.Sequence LIKE N''%' + REPLACE(REPLACE(@SearchSequenceId,'[','[[]'),'''','''''') + '%'''
		END
		
	    -- @SearchDtdSchemaVersionPk
		IF  @SearchDtdSchemaVersionPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.dtd_schema_FK = ' + REPLACE(REPLACE(@SearchDtdSchemaVersionPk,'[','[[]'),'''','''''')
		END
		-- @SearchDispatchDateFrom
		IF @SearchDispatchDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.DispatchDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchDispatchDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchDispatchDateTo
		IF @SearchDispatchDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.DispatchDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchDispatchDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchReceiptDateFrom
		IF @SearchReceiptDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.ReceiptDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchReceiptDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchReceiptDateTo
		IF @SearchReceiptDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'SubmissionUnit.ReceiptDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchReceiptDateTo,'[','[[]'),'''','''''') + ''', 104) '
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