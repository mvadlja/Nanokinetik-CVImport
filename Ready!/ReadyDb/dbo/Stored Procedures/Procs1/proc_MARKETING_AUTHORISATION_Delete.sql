-- Delete
create PROCEDURE  [dbo].[proc_MARKETING_AUTHORISATION_Delete]
	@marketing_authorisation_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MARKETING_AUTHORISATION] WHERE [marketing_authorisation_PK] = @marketing_authorisation_PK
END
