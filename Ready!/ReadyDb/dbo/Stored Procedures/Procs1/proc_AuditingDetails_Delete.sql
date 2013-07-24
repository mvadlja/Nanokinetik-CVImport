-- Delete
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_Delete]
	@IDAuditingDetail int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AuditingDetails] WHERE [IDAuditingDetail] = @IDAuditingDetail
END
