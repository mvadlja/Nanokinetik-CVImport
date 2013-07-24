-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_saved_search_PK]) AS RowNum,
		[product_saved_search_PK], [name], [pharmaaceutical_product_FK],
		[indication_FK], [product_number], [type_product_FK], [client_organization_FK],
		[domain_FK], [procedure_type], [product_ID], [country_FK], [manufacturer_FK], 
		[psur], [displayName], [user_FK], [gridLayout], [isPublic], [nextdlp_from], 
		[nextdlp_to], [responsible_user_FK], [drug_atcs], [client_name], [article57_reporting], [ActiveSubstances]
		FROM [dbo].[PRODUCT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_SAVED_SEARCH]
END
