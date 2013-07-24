-- Delete
CREATE PROCEDURE  [dbo].[proc_XML_REPORT_MAPPING_Delete]
	@xml_report_mapping_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[XML_REPORT_MAPPING] WHERE [xml_report_mapping_PK] = @xml_report_mapping_PK
END
