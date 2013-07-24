
-- GetEntities
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_DOMAIN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_domain_PK], [name_domain]
	FROM [dbo].[OFFICIAL_NAME_DOMAIN]
END
