-- Save
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_Save]
	@IDAuditingDetail int = NULL,
	@MasterID int = NULL,
	@ColumnName nvarchar(200) = NULL,
	@OldValue nvarchar(MAX) = NULL,
	@NewValue nvarchar(MAX) = NULL,
	@PKValue nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AuditingDetails]
	SET
	[MasterID] = @MasterID,
	[ColumnName] = @ColumnName,
	[OldValue] = @OldValue,
	[NewValue] = @NewValue,
	[PKValue] = @PKValue
	WHERE [IDAuditingDetail] = @IDAuditingDetail

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AuditingDetails]
		([MasterID], [ColumnName], [OldValue], [NewValue], [PKValue])
		VALUES
		(@MasterID, @ColumnName, @OldValue, @NewValue, @PKValue)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
