-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_AR_MN_Delete]
	@pp_ar_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_AR_MN] WHERE [pp_ar_mn_PK] = @pp_ar_mn_PK
END
