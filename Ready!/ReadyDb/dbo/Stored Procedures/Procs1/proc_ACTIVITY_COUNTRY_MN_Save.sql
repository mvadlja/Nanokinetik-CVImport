-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_COUNTRY_MN_Save]
	@activity_country_PK int = NULL,
	@activity_FK int = NULL,
	@country_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_COUNTRY_MN]
	SET
	[activity_FK] = @activity_FK,
	[country_FK] = @country_FK
	WHERE [activity_country_PK] = @activity_country_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_COUNTRY_MN]
		([activity_FK], [country_FK])
		VALUES
		(@activity_FK, @country_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
