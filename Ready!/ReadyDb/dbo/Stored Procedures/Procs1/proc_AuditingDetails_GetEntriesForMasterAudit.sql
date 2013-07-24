-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_GetEntriesForMasterAudit]
	@MasterID [int] = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
	[IDAuditingDetail], [MasterID], [ColumnName], [OldValue], [NewValue], [PKValue]
	FROM [dbo].[AuditingDetails]
	WHERE [MasterID] = @MasterID
END
