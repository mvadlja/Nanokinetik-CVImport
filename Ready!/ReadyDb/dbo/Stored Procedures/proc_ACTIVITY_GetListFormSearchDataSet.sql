CREATE PROCEDURE [dbo].[proc_ACTIVITY_GetListFormSearchDataSet]
	@SearchProductPk nvarchar(250) = NULL,
	@SearchProjectPk nvarchar(250) = NULL,
	@SearchActivityName nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchProcedureNumber nvarchar(250) = NULL,
	@SearchProcedureTypePk nvarchar(250) = NULL,
	@SearchTypePk nvarchar(250) = NULL,
	@SearchRegulatoryStatusPk nvarchar(250) = NULL,
	@SearchInternalStatusPk nvarchar(250) = NULL,
	@SearchActivityModePk nvarchar(250) = NULL,
	@SearchApplicantPk nvarchar(250) = NULL,
	@SearchCountryPk nvarchar(250) = NULL,
	@SearchLegalBasis nvarchar(250) = NULL,
	@SearchActivityID nvarchar(250) = NULL,
	@SearchBillable nvarchar(250) = NULL,
	@SearchStartDateFrom nvarchar(250) = NULL,
	@SearchStartDateTo nvarchar(250) = NULL,
	@SearchExpectedFinishedDateFrom nvarchar(250) = NULL,
	@SearchExpectedFinishedDateTo nvarchar(250) = NULL,
	@SearchActualFinishedDateFrom nvarchar(250) = NULL,
	@SearchActualFinishedDateTo nvarchar(250) = NULL,
	@SearchSubmissionDateFrom nvarchar(250) = NULL,	
	@SearchSubmissionDateTo nvarchar(250) = NULL,
	@SearchApprovalDateFrom nvarchar(250) = NULL,
	@SearchApprovalDateTo nvarchar(250) = NULL,
	
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
		SELECT DISTINCT activity_PK, ActivityName, Products, Countries, RegulatoryStatus, InternalStatus, ResponsibleUserFk, SubmissionDate, StartDate, ApprovalDate, TasksCount, 
		TimeCount, DocumentsCount, ActivityID
		FROM
		(
			SELECT Activity.* 
			FROM
			(
				SELECT DISTINCT
				act.activity_PK,
				''ActivityName'' = CASE 
					WHEN ((act.procedure_number = '''' OR act.procedure_number IS NULL) AND (act.name = '''' OR act.name IS NULL)) THEN ''Missing''
					WHEN ((act.procedure_number = '''' OR act.procedure_number IS NULL) AND (act.name != '''' AND act.name IS NOT NULL)) THEN act.name
					ELSE act.name + '' ('' + act.procedure_number + '')'' 
				END,
				dbo.ReturnCountriesOfActivityAbbrevated(act.activity_PK) AS Countries,
				dbo.ReturnProductsByActivity(act.activity_PK) AS Products,
				act.user_FK as ResponsibleUserFk,
				act.legal,
				act.activity_Id as ActivityID,
				act.procedure_type_FK,
				act.procedure_number,
				act.regulatory_status_FK,
				act.internal_status_FK,
				act.mode_FK,
				act.actual_finished_date,
				act.submission_date AS SubmissionDate,
				act.start_date AS StartDate,
				act.approval_date AS ApprovalDate,
				act.expected_finished_date AS ExpectedFinishedDate,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = act.regulatory_status_FK) AS RegulatoryStatus,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = act.internal_status_FK) AS InternalStatus,
				(SELECT COUNT(*) from dbo.TASK WHERE dbo.TASK.activity_FK = act.activity_PK) as TasksCount,
				(SELECT COUNT(*) from dbo.TIME_UNIT	WHERE dbo.TIME_UNIT.activity_FK = act.activity_PK) as TimeCount,
				(SELECT COUNT(*) from dbo.ACTIVITY_DOCUMENT_MN WHERE dbo.ACTIVITY_DOCUMENT_MN.activity_FK = act.activity_PK) AS DocumentsCount,
				CASE
					WHEN act.billable = 1 THEN ''Yes''
					WHEN act.billable = 0 THEN ''No''
					ELSE ''''
				END AS Billable'

		IF @QueryBy = 'ByProduct' 
		BEGIN 
			SET @Query = @Query + '
				FROM [dbo].[ACTIVITY_PRODUCT_MN] activityProductMn
				LEFT JOIN [dbo].[ACTIVITY] act ON act.activity_PK = activityProductMn.activity_FK
				LEFT JOIN [dbo].[PRODUCT] Product ON Product.product_PK = activityProductMn.product_FK
				WHERE activityProductMn.product_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
		END
		ELSE IF @QueryBy = 'ByProject'
		BEGIN 
			SET @Query = @Query + '
				FROM [dbo].[ACTIVITY_PROJECT_MN] activityProjectMn
				LEFT JOIN [dbo].[ACTIVITY] act ON act.activity_PK = activityProjectMn.activity_FK
				LEFT JOIN [dbo].[PROJECT] Project ON Project.project_PK = activityProjectMn.project_FK
				WHERE activityProjectMn.project_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END
		END
		ELSE SET @Query = @Query + '
				FROM [dbo].[ACTIVITY] act'

		SET @Query = @Query + '
			) AS Activity 
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
		
		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------
	    		
		-- @SearchProductPk
		IF @SearchProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchProductPk,'[','[[]'),'''','''''') + ' IN (SELECT apMN.product_FK from dbo.ACTIVITY_PRODUCT_MN apMN WHERE apMN.activity_FK = Activity.activity_PK AND apMN.product_FK IS NOT NULL)'
		END
		
		-- @SearchProjectPk
		IF @SearchProjectPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchProjectPk,'[','[[]'),'''','''''') + ' IN (SELECT apMN.project_FK from dbo.ACTIVITY_PROJECT_MN apMN WHERE apMN.activity_FK = Activity.activity_PK AND apMN.project_FK IS NOT NULL)'
		END

		-- @SearchActivityName
		IF @SearchActivityName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ActivityName LIKE N''%' + REPLACE(REPLACE(@SearchActivityName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ResponsibleUserFk = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''') + ''
		END

		-- @SearchProcedureNumber
		IF @SearchProcedureNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.procedure_number LIKE N''%' + REPLACE(REPLACE(@SearchProcedureNumber,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchProcedureTypePk
		IF @SearchProcedureTypePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.procedure_type_FK = ' + REPLACE(REPLACE(@SearchProcedureTypePk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchTypePk
		IF @SearchTypePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchTypePk,'[','[[]'),'''','''''') + ' IN (SELECT atMN.type_FK from dbo.ACTIVITY_TYPE_MN atMN WHERE atMN.activity_FK = Activity.activity_PK AND atMN.type_FK IS NOT NULL)'
		END
		
		-- @SearchRegulatoryStatusPk
		IF @SearchRegulatoryStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.regulatory_status_FK = ' + REPLACE(REPLACE(@SearchRegulatoryStatusPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchInternalStatusPk
		IF @SearchInternalStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.internal_status_FK = ' + REPLACE(REPLACE(@SearchInternalStatusPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchActivityModePk
		IF @SearchActivityModePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.mode_FK = ' + REPLACE(REPLACE(@SearchActivityModePk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchApplicantPk
		IF @SearchApplicantPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchApplicantPk,'[','[[]'),'''','''''') + ' IN (SELECT aaMN.organization_FK from dbo.ACTIVITY_ORGANIZATION_APP_MN aaMN WHERE aaMN.activity_FK = Activity.activity_PK AND aaMN.organization_FK IS NOT NULL)'
		END

		-- @SearchCountryPk
		IF @SearchCountryPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchCountryPk,'[','[[]'),'''','''''') + ' IN (SELECT acMN.country_FK from dbo.ACTIVITY_COUNTRY_MN acMN WHERE acMN.activity_FK = Activity.activity_PK AND acMN.country_FK IS NOT NULL)'
		END

		-- @SearchBillable
		IF @SearchBillable IS NOT NULL AND @SearchBillable != ''
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.Billable = ''' + REPLACE(REPLACE(@SearchBillable,'[','[[]'),'''','''''') + ''''
		END
		
		-- @SearchLegalBasis
		IF @SearchLegalBasis IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.legal LIKE N''%' + REPLACE(REPLACE(@SearchLegalBasis,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchActivityID
		IF @SearchActivityID IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ActivityID LIKE N''%' + REPLACE(REPLACE(@SearchActivityID,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchStartDateFrom
		IF @SearchStartDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.StartDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchStartDateTo
		IF @SearchStartDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.StartDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchExpectedFinishedDateFrom
		IF @SearchExpectedFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ExpectedFinishedDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchExpectedFinishedDateTo
		IF @SearchExpectedFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ExpectedFinishedDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchActualFinishedDateFrom
		IF @SearchActualFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.actual_finished_date >= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchActualFinishedDateTo
		IF @SearchActualFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.actual_finished_date <= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchSubmissionDateFrom
		IF @SearchSubmissionDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.SubmissionDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchSubmissionDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchSubmissionDateTo
		IF @SearchSubmissionDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.SubmissionDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchSubmissionDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
		
		-- @SearchApprovalDateFrom
		IF @SearchApprovalDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ApprovalDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchApprovalDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchApprovalDateTo
		IF @SearchApprovalDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Activity.ApprovalDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchApprovalDateTo,'[','[[]'),'''','''''') + ''', 104) '
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