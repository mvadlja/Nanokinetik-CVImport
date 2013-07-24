
-- GetEntities
create PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_GetEntitiesByONPK]
@ONPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_domain_on_mn_PK], [on_domain_FK], [on_FK]
	FROM [dbo].[ON_DOMAIN_ON_MN]
	where [dbo].[ON_DOMAIN_ON_MN].on_FK = @ONPK
END
