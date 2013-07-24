-- GetEntities
CREATE PROCEDURE  proc_ACTIVITY_GetEntities
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[activity_PK], [user_FK], [mode_FK], [procedure_type_FK], [name], [description], [comment], [regulatory_status_FK], [start_date], [expected_finished_date], [actual_finished_date], [approval_date], [submission_date], [procedure_number], [legal], [cost], [internal_status_FK], [activity_ID], [automatic_alerts_on],
	 [prevent_start_date_alert],[prevent_exp_finish_date_alert], [dbo].[ACTIVITY].[billable]
	FROM [dbo].[ACTIVITY]
END
