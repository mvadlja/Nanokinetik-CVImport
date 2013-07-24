
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RI_GE_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_ge_mn_PK], [ri_FK], [ge_FK]
	FROM [dbo].[RI_GE_MN]
END
