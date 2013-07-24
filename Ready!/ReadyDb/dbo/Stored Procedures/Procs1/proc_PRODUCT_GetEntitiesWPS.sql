-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'product_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[product_PK], [newownerid], [senderlocalcode], [orphan_drug], [intensive_monitoring], [authorisation_procedure], [comments], [responsible_user_person_FK], [psur], [next_dlp], [name], [description], [client_organization_FK], [type_product_FK], [product_number], [product_ID], [mrp_dcp], [eu_number],
		client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
		FROM [dbo].[PRODUCT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT]
END
