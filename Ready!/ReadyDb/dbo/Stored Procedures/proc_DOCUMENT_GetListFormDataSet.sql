CREATE PROCEDURE [dbo].[proc_DOCUMENT_GetListFormDataSet]
	@DocumentName nvarchar(250) = NULL,
	@Attachments nvarchar(250) = NULL,
	@DocumentType nvarchar(250) = NULL,
	@RelatedEntities nvarchar(250) = NULL,
	@VersionNumber nvarchar(250) = NULL,
	@VersionLabel nvarchar(250) = NULL,
	@RegulatoryStatus nvarchar(250) = NULL,
	@DocumentNumber nvarchar(250) = NULL,
	@LanguageCode nvarchar(250) = NULL,
	@ChangeDate nvarchar(250) = NULL,
	@EffectiveStartDate nvarchar(250) = NULL,
	@EffectiveEndDate nvarchar(250) = NULL,
	@ResponsibleUser nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,

	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'document_PK'

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
			Document.person_FK AS ResponsibleUserFk,
		    ISNULL(Document.name, ''Missing'') AS DocumentName,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.version_number) AS VersionNumber,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.version_label) AS VersionLabel,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.regulatory_status) AS RegulatoryStatus,
		    Document.change_date AS ChangeDate,
		    Document.effective_start_date AS EffectiveStartDate,
		    Document.effective_end_date AS EffectiveEndDate,
		    (SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = Document.type_FK) AS DocumentType,
		    Document.document_code AS DocumentNumber,
			Document.LanguageCodes as LanguageCode,
			(SELECT FullName FROM dbo.PERSON person WHERE person.person_PK = Document.person_FK) AS ResponsibleUser,
		    Document.RelatedEntities,
			Document.Attachments
			'
			IF @QueryBy = 'Product' 
			BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[PRODUCT_DOCUMENT_MN] productDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = productDocumentMn.document_FK
								 WHERE productDocumentMn.product_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
		    ELSE IF @QueryBy = 'Project'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[PROJECT_DOCUMENT_MN] projectDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = projectDocumentMn.document_FK
								 WHERE projectDocumentMn.project_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'PharmaceuticalProduct'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[PP_DOCUMENT_MN] pharmaceuticalProductDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = pharmaceuticalProductDocumentMn.doc_FK
								 WHERE pharmaceuticalProductDocumentMn.pp_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'AuthorisedProduct'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[AP_DOCUMENT_MN] authorisedProductDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = authorisedProductDocumentMn.document_FK
								 WHERE authorisedProductDocumentMn.ap_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'Activity' OR @QueryBy = 'ActivityMy'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[ACTIVITY_DOCUMENT_MN] activityDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = activityDocumentMn.document_FK
								 WHERE activityDocumentMn.activity_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE IF @QueryBy = 'Task'
		    BEGIN 
				SET @Query = @Query +
								'FROM [dbo].[TASK_DOCUMENT_MN] taskDocumentMn
								 LEFT JOIN [dbo].[DOCUMENT] Document ON Document.document_PK = taskDocumentMn.document_FK
								 WHERE taskDocumentMn.task_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
								'
			END
			ELSE SET @Query = @Query + 
								'FROM [dbo].[DOCUMENT] Document
								 '
			SET @Query = @Query + 
			') AS Document 
		'
		SET @TempWhereQuery = '';

		-- @DocumentName
		IF @DocumentName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentName LIKE N''%' + REPLACE(REPLACE(@DocumentName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Attachments
		IF @Attachments IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.Attachments LIKE N''%' + REPLACE(REPLACE(@Attachments,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @DocumentType
		IF @DocumentType IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentType LIKE N''%' + REPLACE(REPLACE(@DocumentType,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @RelatedEntities
		IF @RelatedEntities IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.RelatedEntities LIKE N''%' + REPLACE(REPLACE(@RelatedEntities,'[','[[]'),'''','''''') + '%'''
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
		
		-- @DocumentNumber
		IF @DocumentNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentNumber LIKE N''%' + REPLACE(REPLACE(@DocumentNumber,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @LanguageCode
		IF @LanguageCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.LanguageCode LIKE N''%' + REPLACE(REPLACE(@LanguageCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @ChangeDate
		IF @ChangeDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Document.ChangeDate,104) LIKE ''%' + REPLACE(REPLACE(@ChangeDate,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @EffectiveStartDate
		IF @EffectiveStartDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Document.EffectiveStartDate,104) LIKE ''%' + REPLACE(REPLACE(@EffectiveStartDate,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @EffectiveEndDate
		IF @EffectiveEndDate IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'CONVERT(VARCHAR(30), Document.EffectiveEndDate,104) LIKE ''%' + REPLACE(REPLACE(@EffectiveEndDate,'[','[[]'),'''','''''') + '%'''
		END	
		
		-- @ResponsibleUser
		IF @ResponsibleUser IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ResponsibleUser LIKE N''%' + REPLACE(REPLACE(@ResponsibleUser,'[','[[]'),'''','''''') + '%'''
		END

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
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

	print @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END