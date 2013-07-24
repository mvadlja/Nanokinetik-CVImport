-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_attachment_details_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[xevprm_attachment_details_PK], [attachment_FK], [file_name], [file_type], [attachment_name], [attachment_type], [language_code], [attachment_version], [attachment_version_date], [operation_type], [ev_code]
		FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS]
END
