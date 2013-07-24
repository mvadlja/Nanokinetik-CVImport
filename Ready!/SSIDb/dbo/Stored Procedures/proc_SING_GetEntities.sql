
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SING_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sing_PK], [chemical_FK]
	FROM [dbo].[SING]
END
