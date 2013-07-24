-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_ADJUVANT_Delete]
	@adjuvant_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_ADJUVANT] WHERE [adjuvant_PK] = @adjuvant_PK
END
