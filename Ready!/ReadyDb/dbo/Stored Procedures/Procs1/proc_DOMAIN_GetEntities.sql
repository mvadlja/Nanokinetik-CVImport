-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[domain_PK], [name]
	FROM [dbo].[DOMAIN]
END
