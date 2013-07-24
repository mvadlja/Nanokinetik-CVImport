-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PERSON_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [person_PK]) AS RowNum,
		[person_PK], [country_FK], [name], [familyname], [ip], [phone], [address], [city], [email], [status], [local_number], [ev_code], [givenname], [title], [company], [department], [building], [street], [state], [postcode], [countrycode], [tel_countrycode], [telnumber], [telextn], [cell_countrycode], [cellnumber], [fax_countrycode], [faxnumber], [faxextn], [telnum24h], [FullName]
		FROM [dbo].[PERSON]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PERSON]
END
