-- GetEntity
create PROCEDURE  [dbo].[proc_MARKETING_AUTHORISATION_GetEntity]
	@marketing_authorisation_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[marketing_authorisation_PK], [ready_id], [ma_status_FK], [message_folder], [creation_time]
	FROM [dbo].[MARKETING_AUTHORISATION]
	WHERE [marketing_authorisation_PK] = @marketing_authorisation_PK
END
