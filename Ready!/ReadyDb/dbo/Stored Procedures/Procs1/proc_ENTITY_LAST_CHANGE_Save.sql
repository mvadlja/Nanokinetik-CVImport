-- Save
CREATE PROCEDURE  proc_ENTITY_LAST_CHANGE_Save
	@last_change_PK int = NULL,
	@change_table nvarchar(50) = NULL,
	@change_date datetime = NULL,
	@user_FK int = NULL,
	@entity_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ENTITY_LAST_CHANGE]
	SET
	[change_table] = @change_table,
	[change_date] = @change_date,
	[user_FK] = @user_FK,
	[entity_FK] = @entity_FK
	WHERE [last_change_PK] = @last_change_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ENTITY_LAST_CHANGE]
		([change_table], [change_date], [user_FK], [entity_FK])
		VALUES
		(@change_table, @change_date, @user_FK, @entity_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
