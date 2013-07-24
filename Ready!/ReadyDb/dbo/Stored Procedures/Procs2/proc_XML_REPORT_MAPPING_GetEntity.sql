-- GetEntity
CREATE PROCEDURE  [dbo].[proc_XML_REPORT_MAPPING_GetEntity]
	@xml_report_mapping_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xml_report_mapping_PK], [xml_tag], [display_tag]
	FROM [dbo].[XML_REPORT_MAPPING]
	WHERE [xml_report_mapping_PK] = @xml_report_mapping_PK
END
