
-- GetEntity
CREATE PROCEDURE [dbo].[proc_REFERENCE_SOURCE_GetEntity]
	@reference_source_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reference_source_PK], [public_domain], [rs_type_FK], [rs_class_FK], [rs_id], [rs_citation]
	FROM [dbo].[REFERENCE_SOURCE]
	WHERE [reference_source_PK] = @reference_source_PK
END
