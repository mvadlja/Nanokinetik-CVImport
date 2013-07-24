
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SING_GetEntity]
	@sing_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sing_PK], [chemical_FK]
	FROM [dbo].[SING]
	WHERE [sing_PK] = @sing_PK
END
