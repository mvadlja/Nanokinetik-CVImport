-- GetEntities
create PROCEDURE  [dbo].[proc_MARKETING_AUTHORISATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[marketing_authorisation_PK], [ready_id], [ma_status_FK], [message_folder], [creation_time]
	FROM [dbo].[MARKETING_AUTHORISATION]
END
