-- Save
CREATE PROCEDURE proc_TIME_UNIT_NAME_Save
	@time_unit_name_PK int = NULL,
	@time_unit_name nvarchar(150) = NULL,
	@billable bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[TIME_UNIT_NAME]
	SET
	[time_unit_name] = @time_unit_name,
	[billable] = @billable
	WHERE [time_unit_name_PK] = @time_unit_name_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[TIME_UNIT_NAME]
		([time_unit_name], [billable])
		VALUES
		(@time_unit_name, @billable)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
