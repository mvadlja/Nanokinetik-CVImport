-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [subbmission_unit_PK]) AS RowNum,
		[subbmission_unit_PK], [task_FK], [submission_ID], [agency_role_FK], [comment], [s_format_FK], [description_type_FK], [dispatch_date], [receipt_date], [sequence], [dtd_schema_FK], [organization_agency_FK], [document_FK], [ness_FK], [ectd_FK], [person_FK]
		FROM [dbo].[SUBMISSION_UNIT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBMISSION_UNIT]
END
