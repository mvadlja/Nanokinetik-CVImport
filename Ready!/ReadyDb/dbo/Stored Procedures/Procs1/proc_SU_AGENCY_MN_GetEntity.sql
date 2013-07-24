-- GetEntity
CREATE PROCEDURE  [dbo].[proc_SU_AGENCY_MN_GetEntity]
	@su_agency_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[su_agency_mn_PK], [agency_FK], [submission_unit_FK]
	FROM [dbo].[SU_AGENCY_MN]
	WHERE [su_agency_mn_PK] = @su_agency_mn_PK
END
