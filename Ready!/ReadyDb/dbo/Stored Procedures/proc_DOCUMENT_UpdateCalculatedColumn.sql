create PROCEDURE  proc_DOCUMENT_UpdateCalculatedColumn
	@DocumentPk int = NULL,
	@CalculatedColumn nvarchar(50) = NULL

AS

DECLARE	@LanguageCodes nvarchar(MAX);
DECLARE @Attachments nvarchar(MAX);
DECLARE	@RelatedEntities nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	IF (@DocumentPk IS NULL) RETURN;

	IF @CalculatedColumn = 'LanguageCodes' OR @CalculatedColumn = 'All'

	BEGIN
		SET @LanguageCodes =  STUFF ( (
				SELECT CAST(', ' AS NVARCHAR(MAX)) + LanguageCodeTable.Name
					FROM (
					SELECT DISTINCT
					CASE
						WHEN l.code IS NOT NULL AND LTRIM(l.code) != '' THEN l.code
						ELSE 'N/A'
					END AS Name
					FROM [dbo].[DOCUMENT_LANGUAGE_MN] dlMn
					LEFT JOIN [dbo].[LANGUAGE_CODE] l on dlMn.language_FK = l.languagecode_PK
					WHERE dlMn.[document_FK] = @DocumentPk
				) as LanguageCodeTable 
				for xml path('') ), 1, 2, '');


		UPDATE dbo.DOCUMENT SET LanguageCodes = @LanguageCodes
		WHERE dbo.DOCUMENT.document_PK = @DocumentPk
	END

	IF @CalculatedColumn = 'Attachments' OR @CalculatedColumn = 'All'

	BEGIN
		SET @Attachments =  STUFF ( (
				SELECT CAST(', ' AS NVARCHAR(MAX)) + LanguageCodeTable.Name
					FROM (
					SELECT DISTINCT
					CASE
						WHEN att.attachmentname IS NOT NULL AND LTRIM(att.attachmentname) != '' THEN att.attachmentname
						ELSE 'N/A'
					END AS Name
					FROM [dbo].[ATTACHMENT] att
					WHERE att.[document_FK] = @DocumentPk
				) as LanguageCodeTable 
				for xml path('') ), 1, 2, '');

		UPDATE dbo.DOCUMENT SET Attachments = @Attachments
		WHERE dbo.DOCUMENT.document_PK = @DocumentPk
	END

	IF @CalculatedColumn = 'RelatedEntities' OR @CalculatedColumn = 'All'
	
	BEGIN
		SET @RelatedEntities = STUFF ( (
							SELECT CAST(', ' AS NVARCHAR(MAX)) + RelatedEntityTable.Name
							FROM (
								select  ap.product_name AS Name
									 from AP_DOCUMENT_MN adm
									 LEFT JOIN [AUTHORISED_PRODUCT] ap ON ap.ap_PK = adm.ap_FK
									 where adm.document_FK = @DocumentPk
								UNION ALL
								select p.name AS Name
									 from PRODUCT_DOCUMENT_MN pdm
									 LEFT JOIN [PRODUCT] p ON p.product_PK = pdm.product_FK
									 where pdm.document_FK = @DocumentPk
								UNION ALL
								select pp.name AS Name
									 from PP_DOCUMENT_MN ppdm
									 LEFT JOIN [PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppdm.pp_FK
									 where ppdm.doc_FK = @DocumentPk
								UNION ALL
								SELECT p.name AS Name
									  from PROJECT_DOCUMENT_MN  pdm
									  LEFT JOIN [PROJECT] p ON p.project_PK = pdm.project_FK
									  where pdm.document_FK = @DocumentPk
								UNION ALL
								SELECT a.name AS Name
									  from ACTIVITY_DOCUMENT_MN  adm
									  LEFT JOIN [ACTIVITY] a ON a.activity_PK = adm.activity_FK
									  where adm.document_FK = @DocumentPk	
								union all
								SELECT tn.task_name  as Name
									  from TASK_DOCUMENT_MN  tdm
									  LEFT JOIN [TASK] t ON t.task_PK = tdm.task_FK 
									  LEFT JOIN [TASK_NAME] tn on tn.task_name_PK = t.task_name_FK
									  where tdm.document_FK = @DocumentPk
							) as RelatedEntityTable
							for xml path('') ), 1, 2, '');

		UPDATE dbo.DOCUMENT SET RelatedEntities = @RelatedEntities
		WHERE dbo.DOCUMENT.document_PK = @DocumentPk
	END

	
	
END