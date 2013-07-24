-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_GetEntriesForMasterAudit]
	@master_ID [int] = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	[auditing_detail_PK], [master_ID], [column_name], [old_value], [new_value], [PK_value]
	FROM [dbo].[AUDITING_DETAILS]
	WHERE [master_ID] = @master_ID
END
