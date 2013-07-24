-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_MD_MN_Delete]
	@pp_md_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_MD_MN] WHERE [pp_md_mn_PK] = @pp_md_mn_PK
END
