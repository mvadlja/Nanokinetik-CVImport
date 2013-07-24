
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REFERENCE_INFORMATION_GetEntity]
	@reference_info_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reference_info_PK], [comment]
	FROM [dbo].[REFERENCE_INFORMATION]
	WHERE [reference_info_PK] = @reference_info_PK
END
