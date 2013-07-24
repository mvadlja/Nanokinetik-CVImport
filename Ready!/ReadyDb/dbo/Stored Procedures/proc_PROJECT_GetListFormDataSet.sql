
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetListFormDataSet]
	@ProjectName nvarchar(250) = NULL,
	@Countries nvarchar(250) = NULL,
	@Activities nvarchar(250) = NULL,
	@start_date nvarchar(250) = NULL,
	@expected_finished_date nvarchar(250) = NULL,
	@actual_finished_date nvarchar(250) = NULL,
	@InternalStatus nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,

	@ActCount nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,

	@QueryBy nvarchar(25) = NULL,

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
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		Project.* FROM
		(
			SELECT DISTINCT
			proj.project_PK,
			ISNULL(proj.name, ''Missing'') AS ProjectName,
			dbo.[ReturnProjectCountries](proj.project_PK) as Countries,
			proj.[start_date],
			proj.expected_finished_date,
			proj.actual_finished_date,
			proj.user_FK AS ResponsibleUserFk,

			(select count(DISTINCT paMN.activity_FK) 
				from [dbo].[ACTIVITY_PROJECT_MN] paMN
				where paMN.project_FK = proj.project_PK) as ActCount,
			(select count(DISTINCT pd.document_FK) 
				from dbo.PROJECT_DOCUMENT_MN pd
				where pd.project_FK = proj.project_PK) as DocCount,
			(SELECT name FROM dbo.[TYPE] t
			 WHERE t.type_PK=proj.internal_status_type_FK ) as InternalStatus,
			[dbo].[ReturnActivityForProject](proj.project_PK) as Activities
			'
			IF @QueryBy = 'ProjectMyAlerts'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[PROJECT] proj
								where proj.expected_finished_date <=GETDATE() AND (select name from [TYPE] where [TYPE].type_PK=internal_status_type_FK) not in (''finished'') 
								'
			END
			ELSE SET @Query = @Query + 
								'FROM dbo.PROJECT proj
								 '
		SET @Query = @Query + 
		') AS Project 
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

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Project.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
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