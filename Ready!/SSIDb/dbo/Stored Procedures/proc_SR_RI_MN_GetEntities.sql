
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SR_RI_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sr_ri_mn_PK], [ri_FK], [sr_FK]
	FROM [dbo].[SR_RI_MN]
END
