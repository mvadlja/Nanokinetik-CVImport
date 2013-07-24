-- Save
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_TYPE_MN_Save]
	@activity_type_PK int = NULL,
	@activity_FK int = NULL,
	@type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ACTIVITY_TYPE_MN]
	SET
	[activity_FK] = @activity_FK,
	[type_FK] = @type_FK
	WHERE [activity_type_PK] = @activity_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ACTIVITY_TYPE_MN]
		([activity_FK], [type_FK])
		VALUES
		(@activity_FK, @type_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
