CREATE FUNCTION [dbo].[ReturnProductsByTask]
(
	@task_PK int
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
	LEFT JOIN [dbo].[TASK] task ON task.activity_FK = mn.activity_FK
	WHERE (task.task_PK = @task_PK)
    RETURN @Products
    
  END