
-- GetEntities
create PROCEDURE [dbo].[proc_ON_ONJ_MN_GetEntitiesByONPK]
@ONPK int = null
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_onj_mn_PK], [onj_FK], [on_FK]
	FROM [dbo].[ON_ONJ_MN]
	where [dbo].[ON_ONJ_MN].on_FK = @ONPK
END
