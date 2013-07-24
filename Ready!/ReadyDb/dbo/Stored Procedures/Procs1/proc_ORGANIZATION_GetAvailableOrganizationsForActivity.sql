-- fdsfds
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetAvailableOrganizationsForActivity]
	@activity_pk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [dbo].[ORGANIZATION]
	WHERE [dbo].[ORGANIZATION].[organization_PK] NOT IN
	(
		select organization_FK from dbo.ACTIVITY_ORGANIZATION_APP_MN
		where activity_FK = @activity_pk
	)
	
END
