-- =============================================
-- Author:		Mateo
-- Create date: 5.3.2012
-- Description:	Check if product contatins artice 57 AP
-- =============================================
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_IS_ARTICLE_57]
	@product_PK int=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @rowNumber int;
	
	 Select COUNT(*) from AUTHORISED_PRODUCT ap
		join PRODUCT p on p.product_PK=ap.product_FK
		Where ap.article_57_reporting=1 AND p.product_PK=@product_PK
					
END
