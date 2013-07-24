-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PROJECT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_PK], [user_FK], [name], [comment], [start_date], [expected_finished_date], [actual_finished_date], [description], [internal_status_type_FK],  [automatic_alerts_on], [prevent_start_date_alert], [prevent_exp_finish_date_alert]
	FROM [dbo].[PROJECT]
END
