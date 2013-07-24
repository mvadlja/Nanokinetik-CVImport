-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [approved_substance_PK]) AS RowNum,
		[approved_substance_PK], [operationtype], [virtual], [localnumber], [ev_code], [sourcecode], [resolutionmode], [substancename], [casnumber], [molecularformula], [class], [cbd], [substancetranslations_FK], [substancealiases_FK], [internationalcodes_FK], [previous_ev_codes_FK], [substancessis_FK], [substance_attachment_FK], [comments]
		FROM [dbo].[APPROVED_SUBSTANCE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[APPROVED_SUBSTANCE]
END
