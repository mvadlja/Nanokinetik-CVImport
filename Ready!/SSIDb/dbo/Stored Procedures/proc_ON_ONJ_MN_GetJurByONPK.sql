
-- GetEntities
create PROCEDURE [dbo].[proc_ON_ONJ_MN_GetJurByONPK]
	@ONPK int = null
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ssi.*
	FROM [dbo].[ON_ONJ_MN]
	left join [dbo].[SSI_CONTROLED_VOCABULARY] ssi on ssi.ssi__cont_voc_PK = [dbo].[ON_ONJ_MN].onj_FK
	where [dbo].[ON_ONJ_MN].on_FK = @ONPK

END
