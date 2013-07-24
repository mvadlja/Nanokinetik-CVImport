
-- Delete
CREATE PROCEDURE [dbo].[proc_REFERENCE_INFORMATION_Delete]
	@reference_info_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REFERENCE_INFORMATION] WHERE [reference_info_PK] = @reference_info_PK
END
