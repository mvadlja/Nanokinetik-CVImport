-- GetEntities
CREATE PROCEDURE  [dbo].[proc_XEVPRM_AP_DETAILS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[xevprm_ap_details_PK], [ap_FK], [ap_name], [package_description], [authorisation_country_code], [related_product_FK], [related_product_name], [licence_holder], [authorisation_status], [authorisation_number], [operation_type], [ev_code]
	FROM [dbo].[XEVPRM_AP_DETAILS]
END
