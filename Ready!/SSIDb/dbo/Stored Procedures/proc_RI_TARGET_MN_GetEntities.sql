
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_target_mn_PK], [ri_FK], [target_FK]
	FROM [dbo].[RI_TARGET_MN]
END
