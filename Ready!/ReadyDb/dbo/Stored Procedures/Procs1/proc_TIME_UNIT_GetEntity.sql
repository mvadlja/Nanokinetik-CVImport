-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetEntity]
	@time_unit_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].[TIME_UNIT].*, [dbo].[TIME_UNIT_NAME].time_unit_name AS Name
	FROM [dbo].[TIME_UNIT]
	LEFT JOIN [dbo].[TIME_UNIT_NAME] ON [dbo].[TIME_UNIT].time_unit_name_FK = [dbo].[TIME_UNIT_NAME].time_unit_name_PK
	WHERE [time_unit_PK] = @time_unit_PK
END
