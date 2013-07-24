
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_SN_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sn_mn_PK], [rs_FK], [substance_name_FK]
	FROM [dbo].[RS_SN_MN]
END
