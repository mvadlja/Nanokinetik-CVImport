-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AuditingDetails_AuditSingleColumn]
	@MasterID [int] = NULL,
	@ColumnName [nvarchar](200) = NULL,
	@OldValue [nvarchar](max) = NULL,
	@NewValue [nvarchar](max) = NULL,
	@PKValue [nvarchar](50) = NULL
AS
BEGIN
	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[AuditingDetails]
	([MasterID], [ColumnName], [OldValue], [NewValue], [PKValue])
	VALUES
	(@MasterID, @ColumnName, @OldValue, @NewValue, @PKValue)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
