-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'xevprm_ap_details_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[xevprm_ap_details_PK], [ap_FK], [ap_name], [package_description], [authorisation_country_code], [related_product_FK], [related_product_name], [licence_holder], [authorisation_status], [authorisation_number], [operation_type], [ev_code]
		FROM [dbo].[XEVPRM_AP_DETAILS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_AP_DETAILS]
END
