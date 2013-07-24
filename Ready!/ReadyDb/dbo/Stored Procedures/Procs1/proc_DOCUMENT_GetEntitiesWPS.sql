-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'document_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[document_PK], [person_FK], [type_FK], [name], [description], [comment], [document_code], [regulatory_status], [version_number], [version_label], [change_date], [effective_start_date], [effective_end_date], [version_date], [localnumber], [version_date_format], [attachment_name], [EDMSBindingRule], [EDMSModifyDate], [EDMSDocumentId], [EDMSVersionNumber], [EDMSDocument]
		FROM [dbo].[DOCUMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[DOCUMENT]
END
