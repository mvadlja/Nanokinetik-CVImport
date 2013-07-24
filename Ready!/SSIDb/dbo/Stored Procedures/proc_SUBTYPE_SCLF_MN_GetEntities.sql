
-- GetEntities
CREATE PROCEDURE [dbo].[proc_SUBTYPE_SCLF_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subtype_sclf_mn_PK], [subtype_FK], [sclf_FK]
	FROM [dbo].[SUBTYPE_SCLF_MN]
END
