
-- Delete
create PROCEDURE [dbo].[proc_REFERENCE_SOURCE_Delete]
	@reference_source_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REFERENCE_SOURCE] WHERE [reference_source_PK] = @reference_source_PK
END
