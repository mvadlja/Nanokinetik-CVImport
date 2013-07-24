CREATE FUNCTION [dbo].[ReturnCountriesOfActivity]

(
	@activity_PK INT
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @country_code nvarchar(max)

	SELECT @country_code = COALESCE(@country_code + ', ', '') +
			isnull(rtrim(ltrim( c.name )), '')
			
	FROM dbo.COUNTRY c

	LEFT JOIN dbo.ACTIVITY_COUNTRY_MN amn
	ON c.country_pk = amn.country_FK
	where amn.activity_FK = @activity_PK

	ORDER BY c.name
  
    RETURN @country_code
    
  END
