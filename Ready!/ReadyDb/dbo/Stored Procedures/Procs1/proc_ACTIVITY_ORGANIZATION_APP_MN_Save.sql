-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_ORGANIZATION_APP_MN_Save]
	@activity_organization_applicant_PK int = NULL,
	@activity_FK int = NULL,
	@organization_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_ORGANIZATION_APP_MN]
	SET
	[activity_FK] = @activity_FK,
	[organization_FK] = @organization_FK
	WHERE [activity_organization_applicant_PK] = @activity_organization_applicant_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_ORGANIZATION_APP_MN]
		([activity_FK], [organization_FK])
		VALUES
		(@activity_FK, @organization_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
