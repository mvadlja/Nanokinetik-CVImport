-- GetApprovedSubstance
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_GetApprovedSubstance]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'approved_substance_PK'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @QueryCount nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		DistinctAPPROVED_SUBSTANCE.* FROM
		(
			SELECT 
			[dbo].[APPROVED_SUBSTANCE].[approved_substance_PK], [dbo].[APPROVED_SUBSTANCE].[operationtype], 
			[dbo].[APPROVED_SUBSTANCE].[virtual], [dbo].[APPROVED_SUBSTANCE].[localnumber], [dbo].[APPROVED_SUBSTANCE].[ev_code], 
			[dbo].[APPROVED_SUBSTANCE].[sourcecode], [dbo].[APPROVED_SUBSTANCE].[resolutionmode], 
			[dbo].[APPROVED_SUBSTANCE].[substancename], [dbo].[APPROVED_SUBSTANCE].[casnumber], 
			[dbo].[APPROVED_SUBSTANCE].[molecularformula], [dbo].[APPROVED_SUBSTANCE].[class], [dbo].[APPROVED_SUBSTANCE].[cbd], 
			[dbo].[APPROVED_SUBSTANCE].[substancetranslations_FK], [dbo].[APPROVED_SUBSTANCE].[substancealiases_FK], 
			[dbo].[APPROVED_SUBSTANCE].[internationalcodes_FK], [dbo].[APPROVED_SUBSTANCE].[previous_ev_codes_FK],
			[dbo].[APPROVED_SUBSTANCE].[substancessis_FK], [dbo].[APPROVED_SUBSTANCE].[substance_attachment_FK], 
			[dbo].[APPROVED_SUBSTANCE].[comments]
			FROM [dbo].[APPROVED_SUBSTANCE]
			'
			SET @TempWhereQuery = '';

			IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
			SET @Query = @Query + '
		) DistinctAPPROVED_SUBSTANCE
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SET @QueryCount = '
	SELECT COUNT(DISTINCT [dbo].[APPROVED_SUBSTANCE].[approved_substance_PK]) FROM [dbo].[APPROVED_SUBSTANCE]
	' + @TempWhereQuery

	EXECUTE sp_executesql @QueryCount;
END
