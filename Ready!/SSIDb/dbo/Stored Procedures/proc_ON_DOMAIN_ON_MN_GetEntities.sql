
-- GetEntities
CREATE PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_domain_on_mn_PK], [on_domain_FK], [on_FK]
	FROM [dbo].[ON_DOMAIN_ON_MN]
END
