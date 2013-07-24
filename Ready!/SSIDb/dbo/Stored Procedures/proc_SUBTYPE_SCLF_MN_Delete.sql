
-- Delete
CREATE PROCEDURE [dbo].[proc_SUBTYPE_SCLF_MN_Delete]
	@subtype_sclf_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBTYPE_SCLF_MN] WHERE [subtype_sclf_mn_PK] = @subtype_sclf_mn_PK
END
