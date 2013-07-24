-- GetEntities
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[auditing_detail_PK], [master_ID], [column_name], [old_value], [new_value], [PK_value]
	FROM [dbo].[AUDITING_DETAILS]
END
