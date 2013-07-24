-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_FORM_GetEntity]
	@pharmaceutical_form_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[pharmaceutical_form_PK], [name], [ev_code]
	FROM [dbo].[PHARMACEUTICAL_FORM]
	WHERE [pharmaceutical_form_PK] = @pharmaceutical_form_PK
END
