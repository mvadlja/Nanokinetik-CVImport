-- GetTypesForDDL
CREATE PROCEDURE  [dbo].[proc_TYPE_GetTypesForDDL]
	@group nvarchar(19) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [dbo].[TYPE].[group] = @group

END
