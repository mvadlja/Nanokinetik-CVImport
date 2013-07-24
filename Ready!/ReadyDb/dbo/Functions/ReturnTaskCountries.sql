CREATE FUNCTION [dbo].[ReturnTaskCountries]

(
	@task_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Countries nvarchar(max)

	SELECT @Countries = COALESCE(@Countries + ', ', '') +
			isnull(rtrim(ltrim(c.abbreviation)), '')
			
	FROM dbo.COUNTRY c

	LEFT JOIN dbo.TASK_COUNTRY_MN pc
	ON pc.country_FK = c.country_PK
	where pc.task_FK = @task_PK

	ORDER BY c.name
  
    RETURN @Countries
    
  END
