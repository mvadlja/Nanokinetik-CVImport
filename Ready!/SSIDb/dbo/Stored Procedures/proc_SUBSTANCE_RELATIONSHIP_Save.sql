
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_RELATIONSHIP_Save]
	@substance_relationship_PK int = NULL,
	@relationship varchar(250) = NULL,
	@interaction_type varchar(250) = NULL,
	@substance_id varchar(12) = NULL,
	@substance_name varchar(4000) = NULL,
	@amount_type varchar(250) = NULL,
	@amount_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_RELATIONSHIP]
	SET
	[relationship] = @relationship,
	[interaction_type] = @interaction_type,
	[substance_id] = @substance_id,
	[substance_name] = @substance_name,
	[amount_type] = @amount_type,
	[amount_FK] = @amount_FK
	WHERE [substance_relationship_PK] = @substance_relationship_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_RELATIONSHIP]
		([relationship], [interaction_type], [substance_id], [substance_name], [amount_type], [amount_FK])
		VALUES
		(@relationship, @interaction_type, @substance_id, @substance_name, @amount_type, @amount_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
