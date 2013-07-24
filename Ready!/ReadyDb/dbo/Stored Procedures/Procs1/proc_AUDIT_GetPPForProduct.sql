-- [proc_AUDIT_GetPPForProduct] 1274
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetPPForProduct]
	@product_PK int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct
		pp.[pharmaceutical_product_PK], pp.[name], pp.[ID], pp.[responsible_user_FK], pp.[description], pp.[comments], pp.[Pharmform_FK]
	from product p 
		inner join [dbo].PRODUCT_PP_MN ppmn on p.product_PK = ppmn.product_FK
		inner join [dbo].[PHARMACEUTICAL_PRODUCT] pp on pp.pharmaceutical_product_PK = ppmn.pp_FK
	where p.product_PK = @product_PK	
END
