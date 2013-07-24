
CREATE PROCEDURE  [dbo].[proc_TYPE_GetEntityByGroup]
	@group nvarchar(256) = NULL,
	@name nvarchar(256) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 [dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [dbo].[TYPE].[group] = @group AND LOWER([dbo].[TYPE].[name]) = LOWER(@name)

END