-- GetEntity
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_GetEntityForXevprm]
	@xevprm_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		[dbo].[XEVPRM_AP_DETAILS].[xevprm_ap_details_PK], 
		[dbo].[XEVPRM_AP_DETAILS].[ap_FK], 
		[dbo].[XEVPRM_AP_DETAILS].[ap_name], 
		[dbo].[XEVPRM_AP_DETAILS].[package_description], 
		[dbo].[XEVPRM_AP_DETAILS].[authorisation_country_code], 
		[dbo].[XEVPRM_AP_DETAILS].[related_product_FK], 
		[dbo].[XEVPRM_AP_DETAILS].[related_product_name], 
		[dbo].[XEVPRM_AP_DETAILS].[licence_holder], 
		[dbo].[XEVPRM_AP_DETAILS].[authorisation_status], 
		[dbo].[XEVPRM_AP_DETAILS].[authorisation_number], 
		[dbo].[XEVPRM_AP_DETAILS].[operation_type], 
		[dbo].[XEVPRM_AP_DETAILS].[ev_code]

	FROM dbo.XEVPRM_ENTITY_DETAILS_MN
	LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] ON [dbo].[XEVPRM_AP_DETAILS].xevprm_ap_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
	
	WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = @xevprm_message_PK AND
		dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1 AND
		@xevprm_message_PK IS NOT NULL
END
