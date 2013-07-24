-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xevprm_ap_details_PK]) AS RowNum,
		[xevprm_ap_details_PK], [ap_FK], [ap_name], [package_description], [authorisation_country_code], [related_product_FK], [related_product_name], [licence_holder], [authorisation_status], [authorisation_number], [operation_type], [ev_code]
		FROM [dbo].[XEVPRM_AP_DETAILS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_AP_DETAILS]
END
