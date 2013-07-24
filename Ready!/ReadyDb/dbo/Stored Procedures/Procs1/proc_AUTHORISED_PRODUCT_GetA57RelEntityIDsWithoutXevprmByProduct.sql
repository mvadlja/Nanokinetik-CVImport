CREATE PROCEDURE  [dbo].[proc_AUTHORISED_PRODUCT_GetA57RelEntityIDsWithoutXevprmByProduct]
	@product_FK int = NULL
AS

BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[AUTHORISED_PRODUCT].ap_PK

	FROM [dbo].[AUTHORISED_PRODUCT]

	WHERE [dbo].[AUTHORISED_PRODUCT].product_FK = @product_FK AND
		[dbo].[AUTHORISED_PRODUCT].article_57_reporting = 1 AND
		[dbo].[AUTHORISED_PRODUCT].ap_PK NOT IN

		(	SELECT dbo.XEVPRM_AP_DETAILS.ap_FK 
			FROM dbo.XEVPRM_MESSAGE
			LEFT JOIN dbo.XEVPRM_ENTITY_DETAILS_MN ON dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = dbo.XEVPRM_MESSAGE.xevprm_message_PK
			LEFT JOIN dbo.XEVPRM_AP_DETAILS ON dbo.XEVPRM_AP_DETAILS.xevprm_ap_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
		
			WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1
			AND dbo.XEVPRM_MESSAGE.deleted != 1
			AND dbo.XEVPRM_AP_DETAILS.related_product_FK = @product_FK
			AND dbo.XEVPRM_MESSAGE.message_status_FK IN (1,2,3,4,5,6,7,11,14,16))

END
