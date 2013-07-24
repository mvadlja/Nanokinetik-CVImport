-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_XML_REPORT_MAPPING_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xml_report_mapping_PK]) AS RowNum,
		[xml_report_mapping_PK], [xml_tag], [display_tag]
		FROM [dbo].[XML_REPORT_MAPPING]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XML_REPORT_MAPPING]
END
