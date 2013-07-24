
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetEDMSRelatedEntities]
	@EDMSDocumentId nvarchar(128) = null
AS
BEGIN
	SET NOCOUNT ON;

	with RelatedEntities as
			(
				SELECT ap.ap_PK AS ID,
					   ap.product_name AS Name,
					   ap.[description] AS [Description],
					   'AuthorisedProduct' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM AP_DOCUMENT_MN adMn
				LEFT JOIN [AUTHORISED_PRODUCT] ap ON ap.ap_PK = adMn.ap_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = adMn.document_FK
				LEFT JOIN [PERSON] p ON p.person_PK = ap.responsible_user_person_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
				
				UNION ALL
				
				SELECT pr.product_PK AS ID,
					   pr.name AS Name,
					   pr.[description] AS [Description],
					   'Product' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM PRODUCT_DOCUMENT_MN pdm
				LEFT JOIN [PRODUCT] pr ON pr.product_PK = pdm.product_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = pdm.document_FK
				LEFT JOIN [PERSON] p ON p.person_PK = pr.responsible_user_person_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
			
				UNION ALL
				
				SELECT pp.pharmaceutical_product_PK AS ID,
					   pp.name AS Name,
					   pp.[description] AS [Description],
					   'PharmaceuticalProduct' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM PP_DOCUMENT_MN ppdm
				LEFT JOIN [PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppdm.pp_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = ppdm.doc_FK
				LEFT JOIN [PERSON] p ON p.person_PK = pp.responsible_user_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
			
				UNION ALL
				
				SELECT pr.project_PK AS ID,
					   pr.name AS Name,
					   pr.[description] AS [Description],
					   'Project' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM PROJECT_DOCUMENT_MN pdm
				LEFT JOIN [PROJECT] pr ON pr.project_PK = pdm.project_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = pdm.document_FK
				LEFT JOIN [PERSON] p ON p.person_PK = pr.user_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
				
				UNION ALL
				
				SELECT a.activity_PK AS ID,
					   a.name AS Name,
					   a.[description] AS [Description],
					   'Activity' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM ACTIVITY_DOCUMENT_MN adm
				LEFT JOIN [ACTIVITY] a ON a.activity_PK = adm.activity_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = adm.document_FK
				LEFT JOIN [PERSON] p ON p.person_PK = a.user_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
				
				UNION ALL
				
				SELECT t.task_PK AS ID,
					   tn.task_name AS Name,
					   t.[description] AS [Description],
					   'Task' AS [Type],
					   p.FullName AS ResponsibleUser
				FROM TASK_DOCUMENT_MN tdm
				LEFT JOIN [TASK] t ON t.task_PK = tdm.task_FK
				LEFT JOIN [DOCUMENT] d ON d.document_PK = tdm.document_FK
				LEFT JOIN [PERSON] p ON p.person_PK = t.user_FK
				LEFT JOIN [TASK_NAME] tn on tn.task_name_PK = t.task_name_FK
				WHERE d.EDMSDocumentId = @EDMSDocumentId
			)
	
	
	SELECT  ID, Name, [Description], [Type], ResponsibleUser FROM RelatedEntities;
	
END