-- Save
CREATE PROCEDURE  [dbo].[proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_Save]
	@sub_alias_sub_alias_tran_PK int = NULL,
	@sub_alias_FK int = NULL,
	@sub_alias_tran_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
	SET
	[sub_alias_FK] = @sub_alias_FK,
	[sub_alias_tran_FK] = @sub_alias_tran_FK
	WHERE [sub_alias_sub_alias_tran_PK] = @sub_alias_sub_alias_tran_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN]
		([sub_alias_FK], [sub_alias_tran_FK])
		VALUES
		(@sub_alias_FK, @sub_alias_tran_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
