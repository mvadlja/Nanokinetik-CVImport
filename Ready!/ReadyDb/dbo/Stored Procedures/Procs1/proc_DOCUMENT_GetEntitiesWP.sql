-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [document_PK]) AS RowNum,
		[document_PK], [person_FK], [type_FK], [name], [description], [comment], [document_code], [regulatory_status], [version_number], [version_label], [change_date], [effective_start_date], [effective_end_date], [version_date], [localnumber], [version_date_format], [attachment_name], [EDMSBindingRule], [EDMSModifyDate], [EDMSDocumentId], [EDMSVersionNumber], [EDMSDocument]
		FROM [dbo].[DOCUMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[DOCUMENT]
END
