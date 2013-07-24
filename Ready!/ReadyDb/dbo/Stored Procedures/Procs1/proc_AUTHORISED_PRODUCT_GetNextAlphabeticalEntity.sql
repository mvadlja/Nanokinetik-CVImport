-- GetEntity
CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetNextAlphabeticalEntity]
	@ap_pk int = 158
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @ap_name varchar(max);
	
	SELECT TOP 1
	[ap_PK], [product_name]
	FROM [dbo].[AUTHORISED_PRODUCT] as ap
	WHERE ([product_name] > (SELECT [product_name] 
						FROM [AUTHORISED_PRODUCT]
						WHERE [ap_PK]=@ap_pk) ) 
		  OR ([product_name]=(SELECT [product_name] 
						FROM [AUTHORISED_PRODUCT]
						WHERE [ap_PK]=@ap_pk) AND [ap_PK] > @ap_pk)
	ORDER BY [product_name],[ap_PK]
END
