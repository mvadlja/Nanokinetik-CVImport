-- GetEntitiesWPS
CREATE PROCEDURE proc_XEVPRM_ENTITY_DETAILS_MN_GetEntitiesWPS
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_entity_details_mn_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[xevprm_entity_details_mn_PK], [xevprm_message_FK], [xevprm_entity_details_FK], [xevprm_entity_type_FK], [xevprm_entity_FK], [xevprm_operation_type]
		FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN]
END
