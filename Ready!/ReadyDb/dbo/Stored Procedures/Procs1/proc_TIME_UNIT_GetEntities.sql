-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].[TIME_UNIT].*, [dbo].[TIME_UNIT_NAME].time_unit_name AS Name
	FROM [dbo].[TIME_UNIT]
	LEFT JOIN [dbo].[TIME_UNIT_NAME] ON [dbo].[TIME_UNIT].time_unit_name_FK = [dbo].[TIME_UNIT_NAME].time_unit_name_PK
END
