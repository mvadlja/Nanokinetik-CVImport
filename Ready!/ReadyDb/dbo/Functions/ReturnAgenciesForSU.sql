CREATE FUNCTION [dbo].[ReturnAgenciesForSU]

(
	@s_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @agencies nvarchar(max)
	
	SELECT @agencies = COALESCE(@agencies + ', ', '') +
			isnull(rtrim(ltrim(o.name_org)) , '')
			FROM [dbo].[SU_AGENCY_MN]
			LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = [dbo].[SU_AGENCY_MN].[agency_FK]
			WHERE ([dbo].[SU_AGENCY_MN].[submission_unit_FK] = @s_FK)
--	print @agencies
	
    RETURN @agencies
    
  END
