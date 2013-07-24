
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SUBTYPE_SCLF_MN_GetEntity]
	@subtype_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[subtype_sclf_mn_PK], [subtype_FK], [sclf_FK]
	FROM [dbo].[SUBTYPE_SCLF_MN]
	WHERE [subtype_sclf_mn_PK] = @subtype_sclf_mn_PK
END
