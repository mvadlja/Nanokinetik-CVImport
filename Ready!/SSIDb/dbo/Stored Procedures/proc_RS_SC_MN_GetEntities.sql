
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RS_SC_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[rs_sc_mn_PK], [rs_FK], [sc_FK]
	FROM [dbo].[RS_SC_MN]
END
