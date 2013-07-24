-- GetEntitiesWP
CREATE PROCEDURE proc_XEVPRM_ENTITY_DETAILS_MN_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xevprm_entity_details_mn_PK]) AS RowNum,
		[xevprm_entity_details_mn_PK], [xevprm_message_FK], [xevprm_entity_details_FK], [xevprm_entity_type_FK], [xevprm_entity_FK], [xevprm_operation_type]
		FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_ENTITY_DETAILS_MN]
END
