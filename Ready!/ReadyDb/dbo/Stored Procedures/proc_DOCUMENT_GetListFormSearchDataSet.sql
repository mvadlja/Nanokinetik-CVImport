CREATE PROCEDURE proc_DOCUMENT_GetListFormSearchDataSet
	@SearchProductPk nvarchar(250) = NULL,
	@SearchAuthorisedProductPk nvarchar(250) = NULL,
	@SearchPharmaceuticalProductPk nvarchar(250) = NULL,
	@SearchProjectPk nvarchar(250) = NULL,
	@SearchActivityPk nvarchar(250) = NULL,
	@SearchTaskPk nvarchar(250) = NULL,
	@SearchEvCodeName nvarchar(250) = NULL,
	@SearchDocumentName nvarchar(250) = NULL,
	@SearchTextSearch nvarchar(250) = NULL,
	@SearchDocumentTypePk nvarchar(250) = NULL,
	@SearchVersionNumber nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchVersionLabelPk nvarchar(250) = NULL,
	@SearchDocumentNumber nvarchar(250) = NULL,
	@SearchRegulatoryStatusPk nvarchar(250) = NULL,
	@SearchLanguageCode nvarchar(250) = NULL,
	@SearchComments nvarchar(250) = NULL,
	@SearchChangeDateFrom nvarchar(250) = NULL,
	@SearchChangeDateTo nvarchar(250) = NULL,
	@SearchEffectiveStartDateFrom nvarchar(250) = NULL,
	@SearchEffectiveStartDateTo nvarchar(250) = NULL,
	@SearchEffectiveEndDateFrom nvarchar(250) = NULL,
	@SearchEffectiveEndDateTo nvarchar(250) = NULL,
	@SearchVersionDateFrom nvarchar(250) = NULL,
	@SearchVersionDateTo nvarchar(250) = NULL,
	
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
	
	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 2000,
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
		SELECT DISTINCT document_PK, DocumentName, Attachments, DocumentType, RelatedEntities, VersionNumber, VersionDate, VersionLabel, RegulatoryStatus, DocumentNumber, LanguageCode, ChangeDate, 
		EffectiveStartDate, EffectiveEndDate, ResponsibleUser
		FROM
		(
			SELECT Document.* 
			FROM
			(
				SELECT DISTINCT
				doc.document_PK,
				ISNULL(doc.name, ''Missing'') AS DocumentName,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = doc.version_number) AS VersionNumber,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = doc.version_label) AS VersionLabel,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK =regulatory_status) AS RegulatoryStatus,
				doc.version_number,
				doc.version_label,
				doc.type_FK,
				doc.regulatory_status,
				doc.change_date AS ChangeDate,
				doc.effective_start_date AS EffectiveStartDate,
				doc.effective_end_date AS EffectiveEndDate,
				doc.version_date AS VersionDate,
				(SELECT name FROM dbo.[TYPE] type WHERE type.type_PK = doc.type_FK) AS DocumentType,
				doc.document_code AS DocumentNumber,
				doc.comment AS Comments,
				doc.person_FK,
				doc.LanguageCodes as LanguageCode,
				(SELECT FullName FROM dbo.PERSON person WHERE person.person_PK = doc.person_FK) AS ResponsibleUser,
				doc.RelatedEntities,
				doc.Attachments,
				attachment.ev_code AS EvCode,
				docProduct.product_FK,
				docProject.project_FK,
				docAp.ap_FK,
				docActivity.activity_FK,
				docPp.pp_FK,
				docTask.task_FK
				FROM [dbo].[DOCUMENT] doc
				LEFT JOIN [dbo].[ATTACHMENT] attachment ON attachment.document_FK = doc.document_PK
				LEFT OUTER JOIN [dbo].PRODUCT_DOCUMENT_MN AS docProduct ON doc.document_PK = docProduct.document_FK	
				LEFT OUTER JOIN [dbo].PROJECT_DOCUMENT_MN AS docProject on doc.document_PK = docProject.document_FK
				LEFT OUTER JOIN [dbo].AP_DOCUMENT_MN AS docAp on doc.document_PK = docAp.document_FK
				LEFT OUTER JOIN [dbo].ACTIVITY_DOCUMENT_MN AS docActivity on doc.document_PK = docActivity.document_FK
				LEFT OUTER JOIN [dbo].PP_DOCUMENT_MN AS docPp on doc.document_PK = docPp.doc_FK
				LEFT OUTER JOIN [dbo].TASK_DOCUMENT_MN AS docTask on doc.document_PK = docTask.document_FK
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
	
		--------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

		-- @SearchProductPk
		IF @SearchProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.product_FK = ' + REPLACE(REPLACE(@SearchProductPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchAuthorisedProductPk
		IF @SearchAuthorisedProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ap_FK = ' + REPLACE(REPLACE(@SearchAuthorisedProductPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchPharmaceuticalProductPk
		IF @SearchPharmaceuticalProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.pp_FK = ' + REPLACE(REPLACE(@SearchPharmaceuticalProductPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchProjectPk
		IF @SearchProjectPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.project_FK = ' + REPLACE(REPLACE(@SearchProjectPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchActivityPk
		IF @SearchActivityPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.activity_FK = ' + REPLACE(REPLACE(@SearchActivityPk,'[','[[]'),'''','''''')
		END	
				
		-- @SearchTaskPk
		IF @SearchTaskPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.task_FK = ' + REPLACE(REPLACE(@SearchTaskPk,'[','[[]'),'''','''''')
		END	
			
	    -- @SearchEvCodeName
		IF @SearchEvCodeName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.EvCode LIKE N''%' + REPLACE(REPLACE(@SearchEvCodeName,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchDocumentName
		IF @SearchDocumentName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentName LIKE N''%' + REPLACE(REPLACE(@SearchDocumentName,'[','[[]'),'''','''''') + '%'''
		END	

		-- @SearchDocumentTypePk
		IF @SearchDocumentTypePk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.type_FK = ' + REPLACE(REPLACE(@SearchDocumentTypePk,'[','[[]'),'''','''''')
		END	
			
		-- @SearchVersionNumber
		IF @SearchVersionNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.VersionNumber LIKE N''%' + REPLACE(REPLACE(@SearchVersionNumber,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.person_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchVersionLabelPk
		IF @SearchVersionLabelPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.version_label = ' + REPLACE(REPLACE(@SearchVersionLabelPk,'[','[[]'),'''','''''')
		END	
		
		-- @SearchDocumentNumber
		IF @SearchDocumentNumber IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.DocumentNumber LIKE N''%' + REPLACE(REPLACE(@SearchDocumentNumber,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @SearchRegulatoryStatusPk
		IF @SearchRegulatoryStatusPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.regulatory_status = ' + REPLACE(REPLACE(@SearchRegulatoryStatusPk,'[','[[]'),'''','''''')
		END	

		-- @SearchLanguageCode
		IF @SearchLanguageCode IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.LanguageCode LIKE N''%' + REPLACE(REPLACE(@SearchLanguageCode,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchComments
		IF @SearchComments IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.Comments LIKE N''%' + REPLACE(REPLACE(@SearchComments,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchChangeDateFrom
		IF @SearchChangeDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ChangeDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchChangeDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchChangeDateTo
		IF @SearchChangeDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.ChangeDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchChangeDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchEffectiveStartDateFrom
		IF @SearchEffectiveStartDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.EffectiveStartDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchEffectiveStartDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchEffectiveStartDateTo
		IF @SearchEffectiveStartDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.EffectiveStartDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchEffectiveStartDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		-- @SearchEffectiveEndDateFrom
		IF @SearchEffectiveEndDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.EffectiveEndDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchEffectiveEndDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchEffectiveEndDateTo
		IF @SearchEffectiveEndDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.EffectiveEndDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchEffectiveEndDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchVersionDateFrom
		IF @SearchVersionDateFrom IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.VersionDate >= convert(datetime, ''' + REPLACE(REPLACE(@SearchVersionDateFrom,'[','[[]'),'''','''''') + ''', 104) '
		END
			
		-- @SearchVersionDateTo
		IF @SearchVersionDateTo IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'Document.VersionDate <= convert(datetime, ''' + REPLACE(REPLACE(@SearchVersionDateTo,'[','[[]'),'''','''''') + ''', 104) '
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + '
		) AS DistinctResult
	),
	
	FinalResult AS 
	(
		SELECT DISTINCT PagingResult.* 
		FROM PagingResult
		LEFT JOIN [dbo].[ATTACHMENT] attachment ON attachment.document_FK = document_PK
		' 
		-- @SearchTextSearch
		IF @SearchTextSearch IS NOT NULL
		BEGIN
			SET @Query = @Query + ' WHERE CONTAINS(attachment.disk_file, N''"'+ @SearchTextSearch+'*"'') '
		END	
		
		SET @Query = @Query + 
	 ')
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,*
		FROM FinalResult
	)x
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM FinalResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END