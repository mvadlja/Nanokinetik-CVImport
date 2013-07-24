-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_PK]) AS RowNum,
		[product_PK], [newownerid], [senderlocalcode], [orphan_drug], [intensive_monitoring], [authorisation_procedure], [comments], [responsible_user_person_FK], [psur], [next_dlp], [name], [description], [client_organization_FK], [type_product_FK], [product_number], [product_ID], [mrp_dcp], [eu_number],
		client_group_FK, region_FK, batch_size, pack_size, storage_conditions_FK
		FROM [dbo].[PRODUCT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT]
END
