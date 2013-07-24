-- GetEntity
create PROCEDURE [dbo].[proc_MARKETING_AUTHORISATION_GetEntitiesByStatus]
	@ma_status_FK int = 0
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[marketing_authorisation_PK], [ready_id], [ma_status_FK], [message_folder], [creation_time]
	FROM [dbo].[MARKETING_AUTHORISATION]
	WHERE [ma_status_FK] = @ma_status_FK
END
