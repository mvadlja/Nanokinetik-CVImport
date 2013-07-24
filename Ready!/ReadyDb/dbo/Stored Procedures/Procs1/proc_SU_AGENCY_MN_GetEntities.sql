-- GetEntities
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[su_agency_mn_PK], [agency_FK], [submission_unit_FK]
	FROM [dbo].[SU_AGENCY_MN]
END
