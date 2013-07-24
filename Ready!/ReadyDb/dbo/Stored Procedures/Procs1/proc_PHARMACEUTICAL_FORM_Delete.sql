-- Delete
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_FORM_Delete]
	@pharmaceutical_form_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PHARMACEUTICAL_FORM] WHERE [pharmaceutical_form_PK] = @pharmaceutical_form_PK
END
