
-- GetEntities
CREATE PROCEDURE [dbo].[proc_RI_SCLF_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_sclf_mn_PK], [ref_info_FK], [sclf_FK]
	FROM [dbo].[RI_SCLF_MN]
END
