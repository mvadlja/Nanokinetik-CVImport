-- Save
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_Save]
	@auditing_detail_PK int = NULL,
	@master_ID int = NULL,
	@column_name nvarchar(200) = NULL,
	@old_value nvarchar(MAX) = NULL,
	@new_value nvarchar(MAX) = NULL,
	@PK_value nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AUDITING_DETAILS]
	SET
	[master_ID] = @master_ID,
	[column_name] = @column_name,
	[old_value] = @old_value,
	[new_value] = @new_value,
	[PK_value] = @PK_value
	WHERE [auditing_detail_PK] = @auditing_detail_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AUDITING_DETAILS]
		([master_ID], [column_name], [old_value], [new_value], [PK_value])
		VALUES
		(@master_ID, @column_name, @old_value, @new_value, @PK_value)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
