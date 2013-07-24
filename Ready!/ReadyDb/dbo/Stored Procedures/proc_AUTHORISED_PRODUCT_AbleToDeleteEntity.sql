
CREATE PROCEDURE [dbo].[proc_AUTHORISED_PRODUCT_AbleToDeleteEntity]
	@authorisedProductPk INT = NULL
AS
DECLARE @numberOfDocuments INT = 0;
DECLARE @numberOfxEvprmMessages INT = 0;
BEGIN
	SET NOCOUNT ON;

	IF (@authorisedProductPk = NULL)
		SELECT 0 AS AbleToDelete;
	
	ELSE
	BEGIN
		SELECT @numberOfDocuments = COUNT(*)
		FROM [dbo].[AP_DOCUMENT_MN] authorisedProductDocumentMn
		WHERE (authorisedProductDocumentMn.ap_FK = @authorisedProductPk AND @authorisedProductPk IS NOT NULL)
		
		SELECT @numberOfxEvprmMessages = COUNT(*)
		FROM dbo.XEVPRM_MESSAGE xevprmMessage
			LEFT JOIN dbo.XEVPRM_ENTITY_DETAILS_MN xevprmEntityDetailsMn ON xevprmEntityDetailsMn.xevprm_message_FK = xevprmMessage.xevprm_message_PK
			LEFT JOIN dbo.XEVPRM_AP_DETAILS xevprmAuthorisedProductDetails ON xevprmAuthorisedProductDetails.xevprm_ap_details_PK = xevprmEntityDetailsMn.xevprm_entity_details_FK
		WHERE 
			xevprmEntityDetailsMn.xevprm_entity_type_FK = 1
			AND xevprmMessage.deleted != 1
			AND (xevprmAuthorisedProductDetails.ap_FK = @authorisedProductPk)
			
		IF ((@numberOfDocuments + @numberOfxEvprmMessages) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END	
END