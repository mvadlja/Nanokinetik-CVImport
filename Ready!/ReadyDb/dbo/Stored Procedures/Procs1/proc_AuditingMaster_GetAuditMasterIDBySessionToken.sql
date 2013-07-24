-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_GetAuditMasterIDBySessionToken]
	@session_token nvarchar(max) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT
	[IDAuditingMaster]
	FROM [dbo].[AuditingMaster]
	WHERE [SessionToken] = @session_token
END
