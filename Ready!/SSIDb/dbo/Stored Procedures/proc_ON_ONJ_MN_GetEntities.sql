
-- GetEntities
CREATE PROCEDURE [dbo].[proc_ON_ONJ_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[on_onj_mn_PK], [onj_FK], [on_FK]
	FROM [dbo].[ON_ONJ_MN]
END
