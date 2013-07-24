-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'approved_substance_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[approved_substance_PK], [operationtype], [virtual], [localnumber], [ev_code], [sourcecode], [resolutionmode], [substancename], [casnumber], [molecularformula], [class], [cbd], [substancetranslations_FK], [substancealiases_FK], [internationalcodes_FK], [previous_ev_codes_FK], [substancessis_FK], [substance_attachment_FK], [comments]
		FROM [dbo].[APPROVED_SUBSTANCE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[APPROVED_SUBSTANCE]
END
