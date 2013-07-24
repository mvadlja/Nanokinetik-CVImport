-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subbmission_unit_PK], [task_FK], [submission_ID], [agency_role_FK], [comment], [s_format_FK], [description_type_FK], [dispatch_date], [receipt_date], [sequence], [dtd_schema_FK], [organization_agency_FK], [document_FK], [ness_FK], [ectd_FK],[person_FK]
	FROM [dbo].[SUBMISSION_UNIT]
END
