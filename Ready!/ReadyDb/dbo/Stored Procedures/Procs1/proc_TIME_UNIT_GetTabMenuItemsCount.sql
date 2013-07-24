-- GetEntities
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetTabMenuItemsCount]
	@time_unit_PK int = NULL
AS
DECLARE @Activity_PK int = NULL;
BEGIN
	SET NOCOUNT ON;
	SELECT @Activity_PK = dbo.TIME_UNIT.activity_FK 
	from dbo.TIME_UNIT 
	where dbo.TIME_UNIT.time_unit_PK = @time_unit_PK;
	
	WITH cte_result AS 
	(
			SELECT (CASE when @Activity_PK is not null then '1'	else '0' END) AS 'TimeUnitActPreview',
			(select COUNT(*) from [dbo].[PRODUCT]
			LEFT JOIN dbo.TYPE AuthProc on AuthProc.type_PK = [dbo].[PRODUCT].authorisation_procedure
			LEFT JOIN ACTIVITY_PRODUCT_MN as actProduct on actProduct.product_FK = [dbo].[PRODUCT].product_PK
			where actProduct.activity_FK = @Activity_PK) as 'TimeUnitProdList'
	  )
	  
	 SELECT 
		cte_result.*,
		cte_result.TimeUnitProdList AS TimeUnitMyProdList,
		cte_result.TimeUnitActPreview AS TimeUnitMyActPreview
	  FROM cte_result;
END
