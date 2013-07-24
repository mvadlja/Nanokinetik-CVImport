
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_SR_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sr_mn_PK], [rs_FK], [sr_FK]
	FROM [dbo].[RS_SR_MN]
END
