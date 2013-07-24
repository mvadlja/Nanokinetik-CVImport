-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_GetDistinctColumns]
	@DBName [nvarchar](200) = NULL,
	@TableName [nvarchar](200) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT ColumnName FROM AuditingDetails
	LEFT JOIN AuditingMaster ON AuditingMaster.IDAuditingMaster = AuditingDetails.MasterID
	WHERE AuditingMaster.DBName = @DBName
	AND AuditingMaster.TableName = @TableName
END
