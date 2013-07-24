-- =============================================
-- Author:		<Author,,Name>
-- alter date: <alter Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_AuditSingleColumn]
	@master_ID [int] = NULL,
	@column_name [nvarchar](200) = NULL,
	@old_value [nvarchar](max) = NULL,
	@new_value [nvarchar](max) = NULL,
	@PK_value [nvarchar](50) = NULL
AS
BEGIN
	DECLARE @ScopeIdentity int

	INSERT INTO [dbo].[AUDITING_DETAILS]
	([master_ID], [column_name], [old_value], [new_value], [PK_value])
	VALUES
	(@master_ID, @column_name, @old_value, @new_value, @PK_value)
	
	SET @ScopeIdentity = SCOPE_IDENTITY();
	SELECT @ScopeIdentity AS ScopeIdentity
END
