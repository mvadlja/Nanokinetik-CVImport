create FUNCTION [dbo].[ReturnProductsByProject]
(
	@project_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Products nvarchar(max)

	SELECT @Products = COALESCE(@Products + ', ', '') +
			isnull(rtrim(ltrim(
			CASE 
				WHEN (product_number= '' OR product_number is NULL) THEN p.name
				else p.name+' ('+p.product_number+')'
			END
			)), '')
			
	FROM [dbo].[ACTIVITY_PRODUCT_MN] mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].ACTIVITY_PROJECT_MN apMn ON apMn.activity_FK = mn.activity_FK
	LEFT JOIN [dbo].PROJECT project ON project.project_PK = apMn.project_FK
	WHERE (project.project_PK = @project_PK)
    RETURN @Products
    
  END