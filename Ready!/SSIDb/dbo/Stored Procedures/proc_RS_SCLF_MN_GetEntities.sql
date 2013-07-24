
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_SCLF_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sclf_mn_PK], [sclf_FK], [rs_FK]
	FROM [dbo].[RS_SCLF_MN]
END
