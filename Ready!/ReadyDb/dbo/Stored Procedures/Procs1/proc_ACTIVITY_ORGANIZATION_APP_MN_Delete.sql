-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_Delete]
	@activity_organization_applicant_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_ORGANIZATION_APP_MN] WHERE [activity_organization_applicant_PK] = @activity_organization_applicant_PK
END
