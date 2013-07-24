-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetPrevAlphabeticalEntity]
	@ap_pk int = null
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @ap_name varchar(max);
	
	set @ap_name=(SELECT [product_name] 
						FROM [AUTHORISED_PRODUCT]
						WHERE [ap_PK]=@ap_pk)
	
	SELECT TOP 1
	[ap_PK], [product_name]
	FROM [dbo].[AUTHORISED_PRODUCT] as ap
	WHERE ([product_name] < (SELECT [product_name] 
						FROM [AUTHORISED_PRODUCT]
						WHERE [ap_PK]=@ap_pk)) 
				OR ([product_name]=(SELECT [product_name] 
						FROM [AUTHORISED_PRODUCT]
						WHERE [ap_PK]=@ap_pk) AND [ap_PK] < @ap_pk)
	ORDER BY [product_name] DESC, [ap_PK] DESC
END
