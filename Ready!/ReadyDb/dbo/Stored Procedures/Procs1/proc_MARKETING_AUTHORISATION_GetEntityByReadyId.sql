-- GetEntity
create PROCEDURE [dbo].[proc_MARKETING_AUTHORISATION_GetEntityByReadyId]
	@ready_id nvarchar(20) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[marketing_authorisation_PK], [ready_id], [ma_status_FK], [message_folder], [creation_time]
	FROM [dbo].[MARKETING_AUTHORISATION]
	WHERE [ready_id] = @ready_id
END
