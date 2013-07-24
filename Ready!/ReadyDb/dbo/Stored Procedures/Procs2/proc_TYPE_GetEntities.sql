-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TYPE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [dbo].[TYPE].[group] NOT IN ('DTD', 'M', 'P', 'PP', 'SU', 'T', 'TU', '0')
END
