-- GetEntities
CREATE PROCEDURE  [dbo].[proc_QuickLink_GetPublicEntities]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 'AuthorisedProduct' AS SearchType, ap.ap_saved_search_PK AS SearchId, ap.displayName as SearchName
	FROM [dbo].[AP_SAVED_SEARCH] ap
	where ap.[ispublic]=1

	UNION

	SELECT 'Product' AS SearchType, p.product_saved_search_PK AS SearchId, p.displayName as SearchName
	FROM [dbo].PRODUCT_SAVED_SEARCH p
	where p.[ispublic]=1

	UNION

	SELECT 'PharmaceuticalProduct' AS SearchType, pp.pharmaceutical_products_PK AS SearchId, pp.displayName as SearchName
	FROM [dbo].PHARMACEUTICAL_PRODUCT_SAVED_SEARCH pp
	where pp.[ispublic]=1

	UNION

	SELECT 'SubmissionUnit' AS SearchType, su.submission_unit_saved_search_PK AS SearchId, su.displayName as SearchName
	FROM [dbo].SUBMISSION_UNIT_SAVED_SEARCH su
	where su.[ispublic]=1

	UNION

	SELECT 'Project' AS SearchType, proj.project_saved_search_PK AS SearchId, proj.displayName as SearchName
	FROM [dbo].PROJECT_SAVED_SEARCH proj
	where proj.[ispublic]=1

	UNION

	SELECT 'Activity' AS SearchType, act.activity_saved_search_PK AS SearchId, act.displayName as SearchName
	FROM [dbo].ACTIVITY_SAVED_SEARCH act
	where act.[ispublic]=1

	UNION

	SELECT 'Task' AS SearchType, task.task_saved_search_PK AS SearchId, task.displayName as SearchName
	FROM [dbo].TASK_SAVED_SEARCH task
	where task.[ispublic]=1

	UNION

	SELECT 'TimeUnit' AS SearchType, tu.time_unit_saved_search_PK AS SearchId, tu.displayName as SearchName
	FROM [dbo].TIME_UNIT_SAVED_SEARCH tu
	where tu.[ispublic]=1

	UNION

	SELECT 'Document' AS SearchType, doc.document_saved_search_PK AS SearchId, doc.displayName as SearchName
	FROM [dbo].DOCUMENT_SAVED_SEARCH doc
	where doc.[ispublic]=1
	
	UNION
	
	SELECT 'Alerter' AS SearchType, alerter.alert_saved_search_PK AS SearchId, alerter.displayName as SearchName
	FROM [dbo].ALERT_SAVED_SEARCH alerter
	where alerter.[ispublic]=1

END