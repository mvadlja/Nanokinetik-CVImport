-- GetEntities
CREATE PROCEDURE proc_TIME_UNIT_NAME_GetEntities
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[time_unit_name_PK], [time_unit_name], [billable]
	FROM [dbo].[TIME_UNIT_NAME]
END
