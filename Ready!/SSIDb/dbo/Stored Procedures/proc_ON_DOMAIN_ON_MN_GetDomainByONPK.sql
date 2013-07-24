
-- GetEntities
create PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_GetDomainByONPK]
	@ONPK int = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ssi.*
	FROM [dbo].[ON_DOMAIN_ON_MN]
	left join [dbo].[SSI_CONTROLED_VOCABULARY] ssi on ssi.ssi__cont_voc_PK = [dbo].[ON_DOMAIN_ON_MN].on_domain_FK
	where [dbo].[ON_DOMAIN_ON_MN].on_FK = @ONPK

END
