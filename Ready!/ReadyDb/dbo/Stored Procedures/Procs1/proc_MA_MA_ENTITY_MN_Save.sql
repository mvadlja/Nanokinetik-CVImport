-- Save
create PROCEDURE [dbo].[proc_MA_MA_ENTITY_MN_Save]
	@ma_ma_entity_mn_PK int = NULL,
	@ma_FK int = NULL,
	@ma_entity_FK int = NULL,
	@ma_entity_type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_MA_ENTITY_MN]
	SET
	[ma_FK] = @ma_FK,
	[ma_entity_FK] = @ma_entity_FK,
	[ma_entity_type_FK] = @ma_entity_type_FK
	WHERE [ma_ma_entity_mn_PK] = @ma_ma_entity_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_MA_ENTITY_MN]
		([ma_FK], [ma_entity_FK], [ma_entity_type_FK])
		VALUES
		(@ma_FK, @ma_entity_FK, @ma_entity_type_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
