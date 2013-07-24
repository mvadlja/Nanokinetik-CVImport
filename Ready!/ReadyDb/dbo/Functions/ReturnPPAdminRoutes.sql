CREATE FUNCTION [dbo].[ReturnPPAdminRoutes]

(
	@pp_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @AdminRoutes nvarchar(max)

	SELECT @AdminRoutes = COALESCE(@AdminRoutes + ', ', '') +
			isnull(rtrim(ltrim(
		CASE
			WHEN ppAR.adminroutecode IS NULL OR ppAR.adminroutecode = '' THEN ''
			ELSE ppAR.adminroutecode
		END
     )), '')     
			from [dbo].[PP_AR_MN]
			left join [dbo].[PP_ADMINISTRATION_ROUTE] ppAR on ppAR.adminroute_PK = [dbo].[PP_AR_MN].admin_route_FK
			where [dbo].[PP_AR_MN].pharmaceutical_product_FK = @pp_PK
	
	--if @AdminRoutes is null or @AdminRoutes = ''
	--	RETURN 'N/A'
		
	RETURN @AdminRoutes
    
  END
