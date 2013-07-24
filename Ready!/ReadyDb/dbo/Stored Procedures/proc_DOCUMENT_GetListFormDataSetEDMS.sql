
CREATE PROCEDURE [dbo].[proc_DOCUMENT_GetListFormDataSetEDMS]
	@DocumentName nvarchar(250) = NULL,
	@DocumentType nvarchar(250) = NULL,
	@VersionNumber nvarchar(250) = NULL,
	@VersionLabel nvarchar(250) = NULL,
	@RegulatoryStatus nvarchar(250) = NULL,
	@ResponsibleUser nvarchar(250) = NULL,

	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'DocumentName ASC'
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
		Document.* FROM
		(
			SELECT DISTINCT
				Document.document_PK,
				ISNULL(Document.name, ''Missing'') AS DocumentName,
				ISNULL((SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.type_FK), ''Missing'') AS DocumentType,
				ISNULL((SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.version_number), ''Missing'') AS VersionNumber,
				ISNULL((SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.version_label), ''Missing'') AS VersionLabel,
				ISNULL((SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.regulatory_status), ''Missing'') AS RegulatoryStatus,
				ISNULL((SELECT FullName FROM dbo.PERSON person WHERE person.person_PK = Document.person_FK), ''Missing'') AS ResponsibleUser
		    FROM [dbo].[DOCUMENT] Document
		 ) AS Document 
		'
		SET @TempWhereQuery = '';

		-- @DocumentName
		IF @DocumentName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentName LIKE N''%' + REPLACE(REPLACE(@DocumentName,'[','[[]'),'''','''''') + '%'''
		END
				
		-- @DocumentType
		IF @DocumentType IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentType LIKE N''%' + REPLACE(REPLACE(@DocumentType,'[','[[]'),'''','''''') + '%'''
		END
				
		-- @VersionNumber
		IF @VersionNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.VersionNumber LIKE N''%' + REPLACE(REPLACE(@VersionNumber,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @VersionLabel
		IF @VersionLabel IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.VersionLabel LIKE N''%' + REPLACE(REPLACE(@VersionLabel,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @RegulatoryStatus
		IF @RegulatoryStatus IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.RegulatoryStatus LIKE N''%' + REPLACE(REPLACE(@RegulatoryStatus,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUser
		IF @ResponsibleUser IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ResponsibleUser LIKE N''%' + REPLACE(REPLACE(@ResponsibleUser,'[','[[]'),'''','''''') + '%'''
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