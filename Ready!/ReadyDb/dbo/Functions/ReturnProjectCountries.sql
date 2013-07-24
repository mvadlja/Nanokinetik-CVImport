CREATE FUNCTION [dbo].[ReturnProjectCountries]

(
	@project_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Countries nvarchar(max)

	SELECT @Countries = COALESCE(@Countries + ', ', '') +
			isnull(rtrim(ltrim(c.abbreviation)), '')
			
	FROM dbo.COUNTRY c

	LEFT JOIN dbo.PROJECT_COUNTRY_MN pc
	ON pc.country_FK = c.country_PK
	where pc.project_FK = @project_PK

	ORDER BY c.name
  
    RETURN @Countries
    
  END
