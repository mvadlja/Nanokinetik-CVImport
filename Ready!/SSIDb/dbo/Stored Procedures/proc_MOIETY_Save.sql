
-- Save
CREATE PROCEDURE [dbo].[proc_MOIETY_Save]
	@moiety_PK int = NULL,
	@moiety_role varchar(250) = NULL,
	@moiety_name varchar(4000) = NULL,
	@moiety_id varchar(12) = NULL,
	@amount_type varchar(250) = NULL,
	@amount_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MOIETY]
	SET
	[moiety_role] = @moiety_role,
	[moiety_name] = @moiety_name,
	[moiety_id] = @moiety_id,
	[amount_type] = @amount_type,
	[amount_FK] = @amount_FK
	WHERE [moiety_PK] = @moiety_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MOIETY]
		([moiety_role], [moiety_name], [moiety_id], [amount_type], [amount_FK])
		VALUES
		(@moiety_role, @moiety_name, @moiety_id, @amount_type, @amount_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
