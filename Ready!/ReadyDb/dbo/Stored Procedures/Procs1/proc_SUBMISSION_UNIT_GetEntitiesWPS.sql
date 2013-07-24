-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'subbmission_unit_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[subbmission_unit_PK], [task_FK], [submission_ID], [agency_role_FK], [comment], [s_format_FK], [description_type_FK], [dispatch_date], [receipt_date], [sequence], [dtd_schema_FK], [organization_agency_FK], [document_FK], [ness_FK], [ectd_FK], [person_FK]
		FROM [dbo].[SUBMISSION_UNIT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBMISSION_UNIT]
END
