-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TYPE_GetEntity]
	@type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[dbo].[TYPE].*
	FROM [dbo].[TYPE]
	WHERE [type_PK] = @type_PK
END
