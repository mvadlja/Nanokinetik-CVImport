-----------------------------------------------
CREATE PROCEDURE  [dbo].[proc_AUDIT_GetProductForAP]
	@PKValue int = 55, --2147483647,	
	@date datetime = '2099-01-01 12:30:50'
AS
BEGIN
	DECLARE	@product_FK int

	EXEC	@product_FK = [dbo].[proc_AUDIT_GetFK] @PKValue, @date, 'AUTHORISED_PRODUCT', 'product_FK'
	print   @product_FK
	EXEC    [dbo].proc_AUDIT_GetEntity @product_FK, 'PRODUCT', @date
	--EXEC    [dbo].proc_AUDIT_GetEntity 21122, 'PRODUCT','2099-01-01 12:30:50'

END
