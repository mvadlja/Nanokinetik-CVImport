-- Delete
CREATE PROCEDURE  [dbo].[proc_AuditingMaster_Delete]
	@IDAuditingMaster int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AuditingMaster] WHERE [IDAuditingMaster] = @IDAuditingMaster
END
