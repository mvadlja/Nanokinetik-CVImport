-- GetEntities
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_GetEntityForXevprm]
		@xevprm_message_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[xevprm_attachment_details_PK], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[attachment_FK], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[file_name], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[file_type], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[attachment_name], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[attachment_type], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[language_code], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[attachment_version], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[attachment_version_date], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[operation_type], 
		[dbo].[XEVPRM_ATTACHMENT_DETAILS].[ev_code]

	FROM dbo.XEVPRM_ENTITY_DETAILS_MN
	LEFT JOIN [dbo].[XEVPRM_ATTACHMENT_DETAILS] ON [dbo].[XEVPRM_ATTACHMENT_DETAILS].xevprm_attachment_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
	
	WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = @xevprm_message_PK AND
		dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 2

	ORDER BY dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK DESC
END
