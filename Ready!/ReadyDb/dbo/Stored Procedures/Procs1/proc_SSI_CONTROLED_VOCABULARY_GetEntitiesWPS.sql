-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SSI_CONTROLED_VOCABULARY_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ssi__cont_voc_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[ssi__cont_voc_PK], [list_name], [term_id], [term_name_english], [latin_name_latin], [synonim1], [synonim2], [Description], [Field8], [Field9], [Field10], [Field11], [Field12], [Field13], [Field14], [evcode]
		FROM [dbo].[SSI_CONTROLED_VOCABULARY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SSI_CONTROLED_VOCABULARY]
END
