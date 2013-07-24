
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetListFormSearchDataSet]
	@SearchProjectName nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchInternalStatusPk nvarchar(250) = NULL,
	@SearchCountryPk nvarchar(250) = NULL,
	@SearchStartDateFrom nvarchar(250) = NULL,
	@SearchStartDateTo nvarchar(250) = NULL,
	@SearchExpectedFinishedDateFrom nvarchar(250) = NULL,
	@SearchExpectedFinishedDateTo nvarchar(250) = NULL,
	@SearchActualFinishedDateFrom nvarchar(250) = NULL,
	@SearchActualFinishedDateTo nvarchar(250) = NULL,

	@ProjectName nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@Activities nvarchar(250) = NULL,
	@start_date nvarchar(250) = NULL,
	@expected_finished_date nvarchar(250) = NULL,
	@actual_finished_date nvarchar(250) = NULL,
	@InternalStatus nvarchar(250) = NULL,

	@ActCount nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ProjectName'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT DISTINCT project_PK, ProjectName, Countries, InternalStatus, [start_date], expected_finished_date, actual_finished_date, ActCount, DocCount, Activities
		FROM
		(
			SELECT Project.* 
			FROM
			(
				SELECT DISTINCT
				proj.project_PK,
				ISNULL(proj.name, ''Missing'') AS ProjectName,
				dbo.[ReturnProjectCountries](proj.project_PK) as Countries,
				proj.[start_date],
				proj.expected_finished_date,
				proj.actual_finished_date,
				proj.user_FK,
				proj.internal_status_type_FK,
				(select count(DISTINCT paMN.activity_FK) 
					from [dbo].[ACTIVITY_PROJECT_MN] paMN
					where paMN.project_FK = proj.project_PK) as ActCount,
				(select count(DISTINCT pd.document_FK) 
					from dbo.PROJECT_DOCUMENT_MN pd
					where pd.project_FK = proj.project_PK) as DocCount,
				(SELECT name FROM dbo.[TYPE] t
				 WHERE t.type_PK=proj.internal_status_type_FK ) as InternalStatus,
				[dbo].[ReturnActivityForProject](proj.project_PK) as Activities
				FROM dbo.PROJECT proj'
		
		SET @Query = @Query + '
			) AS Project 
			'
		SET @TempWhereQuery = '';

		-- @ProjectName
		IF @ProjectName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.ProjectName LIKE N''%' + REPLACE(REPLACE(@ProjectName,'[','[[]'),'''','''''') + '%'''
		END

		-- @Countries
		IF @Countries IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.Countries LIKE N''%' + REPLACE(REPLACE(@Countries,'[','[[]'),'''','''''') + '%'''
		END

		-- @Activities
		IF @Activities IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.Activities LIKE N''%' + REPLACE(REPLACE(@Activities,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @InternalStatus
		IF @InternalStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.InternalStatus LIKE N''%' + REPLACE(REPLACE(@InternalStatus,'[','[[]'),'''','''''') + '%'''
		END

		-- @start_date
		IF @start_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),Project.[start_date],104) LIKE ''%' + REPLACE(REPLACE(@start_date,'[','[[]'),'''','''''') + '%'''
		END	

		-- @expected_finished_date
		IF @expected_finished_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),Project.[expected_finished_date],104) LIKE ''%' + REPLACE(REPLACE(@expected_finished_date,'[','[[]'),'''','''''') + '%'''
		END	

		-- @actual_finished_date
		IF @actual_finished_date IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30),Project.[actual_finished_date],104) LIKE ''%' + REPLACE(REPLACE(@actual_finished_date,'[','[[]'),'''','''''') + '%'''
		END	
		-- @ActCount
		IF @ActCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.ActCount LIKE N''%' + REPLACE(REPLACE(@ActCount,'[','[[]'),'''','''''') + '%'''
		END

		-- @DocCount
		IF @DocCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.DocCount LIKE N''%' + REPLACE(REPLACE(@DocCount,'[','[[]'),'''','''''') + '%'''
		END

		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

		-- @SearchProjectName
		IF @SearchProjectName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.ProjectName LIKE N''%' + REPLACE(REPLACE(@SearchProjectName,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.user_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END

		-- @SearchInternalStatusPk
		IF @SearchInternalStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.internal_status_type_FK = ' + REPLACE(REPLACE(@SearchInternalStatusPk,'[','[[]'),'''','''''')
		END

		-- @SearchCountryPk
		IF @SearchCountryPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + REPLACE(REPLACE(@SearchCountryPk,'[','[[]'),'''','''''') + ' in (SELECT pcMN.country_FK from dbo.PROJECT_COUNTRY_MN pcMN WHERE pcMN.project_FK = Project.project_PK AND pcMN.country_FK IS NOT NULL)'
		END

		-- @SearchStartDateFrom
		IF @SearchStartDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.[start_date] >= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchStartDateTo
		IF @SearchStartDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.[start_date] <= convert(datetime, ''' + REPLACE(REPLACE(@SearchStartDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchExpectedFinishedDateFrom
		IF @SearchExpectedFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.expected_finished_date >= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchExpectedFinishedDateTo
		IF @SearchExpectedFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.expected_finished_date <= convert(datetime, ''' + REPLACE(REPLACE(@SearchExpectedFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchActualFinishedDateFrom
		IF @SearchActualFinishedDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.actual_finished_date >= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchActualFinishedDateTo
		IF @SearchActualFinishedDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.actual_finished_date <= convert(datetime, ''' + REPLACE(REPLACE(@SearchActualFinishedDateTo,'[','[[]'),'''','''''') + ''', 104) '
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