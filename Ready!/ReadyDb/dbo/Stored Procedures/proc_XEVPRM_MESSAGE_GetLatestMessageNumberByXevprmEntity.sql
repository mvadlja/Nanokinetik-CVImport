create PROCEDURE  proc_XEVPRM_MESSAGE_GetLatestMessageNumberByXevprmEntity
@XevprmEntityPk int = NULL,
@XevprmEntityType int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 dbo.XEVPRM_MESSAGE.message_number AS MessageNumber

	FROM dbo.XEVPRM_ENTITY_DETAILS_MN
		JOIN dbo.XEVPRM_MESSAGE ON dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = dbo.XEVPRM_MESSAGE.xevprm_message_PK
	WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = @XevprmEntityType
		AND dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_FK = @XevprmEntityPk
	
	ORDER BY dbo.XEVPRM_MESSAGE.xevprm_message_PK desc

END