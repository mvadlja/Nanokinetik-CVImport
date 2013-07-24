-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetNextAlphabeticalEntity]
	@pp_pk int = null
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT TOP 1
	[pharmaceutical_product_PK], [name]
	FROM [dbo].[PHARMACEUTICAL_PRODUCT] as pp
	WHERE ([name] > (SELECT [name] 
						FROM [PHARMACEUTICAL_PRODUCT] 
						WHERE [pharmaceutical_product_PK]=@pp_pk) ) 
		  OR ([name]=(SELECT [name] 
						FROM [PHARMACEUTICAL_PRODUCT] 
						WHERE [pharmaceutical_product_PK]=@pp_pk AND [pharmaceutical_product_PK] > @pp_pk))
	ORDER BY [name],[pharmaceutical_product_PK]
END
