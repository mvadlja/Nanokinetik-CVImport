
-- GetEntity
CREATE PROCEDURE [dbo].[proc_RI_SCLF_MN_GetEntity]
	@ri_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ri_sclf_mn_PK], [ref_info_FK], [sclf_FK]
	FROM [dbo].[RI_SCLF_MN]
	WHERE [ri_sclf_mn_PK] = @ri_sclf_mn_PK
END
