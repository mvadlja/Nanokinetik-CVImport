-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_SAVED_SEARCH_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_saved_search_PK], [project_FK], [product_FK], [name], [user_FK], [procedure_number], [procedure_type_FK], [type_FK], [regulatory_status_FK], [internal_status_FK], [activity_mode_FK], [applicant_FK], [country_FK], [legal], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_date_to], [actual_finished_date_from], [actual_finished_date_to], [approval_date_from], [approval_date_to], [submission_date_from], [submission_date_to], [displayName], [user_FK1], [gridLayout], [isPublic], [billable], [activity_ID]
	FROM [dbo].[ACTIVITY_SAVED_SEARCH]
END
