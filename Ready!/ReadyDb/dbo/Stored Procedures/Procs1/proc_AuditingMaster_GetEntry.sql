-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_GetEntry] 
	@IDAuditingMaster [int] = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
	[IDAuditingMaster], [Username], [DBName], [TableName], [Date], [Operation], [ServerName]
	FROM [dbo].[AuditingMaster]
	WHERE [IDAuditingMaster] = @IDAuditingMaster
END
