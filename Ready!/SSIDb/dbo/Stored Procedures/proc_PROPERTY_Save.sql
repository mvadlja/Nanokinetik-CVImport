
-- Save
CREATE PROCEDURE [dbo].[proc_PROPERTY_Save]
	@property_PK int = NULL,
	@property_type varchar(250) = NULL,
	@property_name varchar(250) = NULL,
	@substance_id varchar(12) = NULL,
	@substance_name varchar(4000) = NULL,
	@amount_type varchar(250) = NULL,
	@amount_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PROPERTY]
	SET
	[property_type] = @property_type,
	[property_name] = @property_name,
	[substance_id] = @substance_id,
	[substance_name] = @substance_name,
	[amount_type] = @amount_type,
	[amount_FK] = @amount_FK
	WHERE [property_PK] = @property_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PROPERTY]
		([property_type], [property_name], [substance_id], [substance_name], [amount_type], [amount_FK])
		VALUES
		(@property_type, @property_name, @substance_id, @substance_name, @amount_type, @amount_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
