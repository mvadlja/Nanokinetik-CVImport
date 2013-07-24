-- GetEntity
CREATE PROCEDURE proc_TIME_UNIT_NAME_GetEntity
	@time_unit_name_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[time_unit_name_PK], [time_unit_name], [billable]
	FROM [dbo].[TIME_UNIT_NAME]
	WHERE [time_unit_name_PK] = @time_unit_name_PK
END
