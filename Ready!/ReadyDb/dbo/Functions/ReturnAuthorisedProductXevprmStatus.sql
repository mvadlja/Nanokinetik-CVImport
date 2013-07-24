CREATE FUNCTION  [dbo].[ReturnAuthorisedProductXevprmStatus]
(
	@AuthorisedProductPk int = NULL
)
RETURNS nvarchar(100)
AS

BEGIN

	DECLARE @XevprmStatus nvarchar(100) = NULL;

	SELECT TOP 1 @XevprmStatus = xs.xevprm_grid_status_name
	FROM dbo.XEVPRM_ENTITY_DETAILS_MN
	LEFT JOIN dbo.XEVPRM_AP_DETAILS ON dbo.XEVPRM_AP_DETAILS.xevprm_ap_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
	LEFT JOIN dbo.XEVPRM_MESSAGE ON dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = dbo.XEVPRM_MESSAGE.xevprm_message_PK
	LEFT JOIN dbo.XEVPRM_MESSAGE_STATUS xs ON xs.xevprm_message_status_PK = dbo.XEVPRM_MESSAGE.message_status_FK
	WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1 AND dbo.XEVPRM_AP_DETAILS.ap_FK = @AuthorisedProductPk AND dbo.XEVPRM_MESSAGE.[deleted] != 1
	ORDER BY dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK DESC

	RETURN ISNULL(@XevprmStatus,'') 
end