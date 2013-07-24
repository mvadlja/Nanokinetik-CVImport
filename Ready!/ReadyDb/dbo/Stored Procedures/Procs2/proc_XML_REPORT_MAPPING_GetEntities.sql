-- GetEntities
CREATE PROCEDURE  [dbo].[proc_XML_REPORT_MAPPING_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xml_report_mapping_PK], [xml_tag], [display_tag]
	FROM [dbo].[XML_REPORT_MAPPING]
END
