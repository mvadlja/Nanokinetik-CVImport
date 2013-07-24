-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TYPE_GetGroups]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
	t.[group], 
	t.[group_description]

	FROM [dbo].[TYPE] t
	WHERE t.[group] NOT IN ('DTD', 'M', 'P', 'PP', 'SU', 'T', 'TU', '0')
END
