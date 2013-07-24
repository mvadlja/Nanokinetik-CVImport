
-- GetEntity
CREATE PROCEDURE [dbo].[proc_STEREOCHEMISTRY_GetEntity]
	@stereochemistry_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[stereochemistry_PK], [name]
	FROM [dbo].[STEREOCHEMISTRY]
	WHERE [stereochemistry_PK] = @stereochemistry_PK
END
