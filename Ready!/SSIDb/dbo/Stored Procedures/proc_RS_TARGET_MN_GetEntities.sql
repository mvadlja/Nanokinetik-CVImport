
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_TARGET_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_target_mn_PK], [rs_FK], [target_FK]
	FROM [dbo].[RS_TARGET_MN]
END
