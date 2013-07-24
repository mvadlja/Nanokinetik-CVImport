CREATE FUNCTION  [dbo].[ReturnIsXevprmActionAvailable]
( 
	@msg_PK int = NULL
)

RETURNS int
AS

BEGIN

	declare @ap_FK int = NULL
	declare @IsActionAvailable int = NULL; -- 0-False or 1-True
	declare @MaxMsgPK_OperationType1 int = NULL;
	declare @MaxMsgPK_OperationType4 int = NULL;

	set @ap_FK = 
		(	SELECT TOP 1 [dbo].[XEVPRM_AP_DETAILS].[ap_FK]
		
			FROM dbo.XEVPRM_ENTITY_DETAILS_MN
				LEFT JOIN [dbo].[XEVPRM_AP_DETAILS] ON [dbo].[XEVPRM_AP_DETAILS].xevprm_ap_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
	
			WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK = @msg_PK AND
				dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1

			ORDER BY dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK DESC	)

	set @MaxMsgPK_OperationType1 = 
		(	SELECT max([dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK)
			
			FROM [dbo].XEVPRM_ENTITY_DETAILS_MN
				LEFT JOIN dbo.XEVPRM_AP_DETAILS ON dbo.XEVPRM_AP_DETAILS.xevprm_ap_details_PK = [dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
				LEFT JOIN dbo.XEVPRM_MESSAGE ON dbo.XEVPRM_MESSAGE.xevprm_message_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK

			WHERE [dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1 AND
				[dbo].XEVPRM_AP_DETAILS.ap_FK = @ap_FK AND 
				dbo.XEVPRM_MESSAGE.deleted != 1 AND
				[dbo].XEVPRM_AP_DETAILS.operation_type = 1	)

	set @MaxMsgPK_OperationType4 = 
		(	SELECT max([dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK)
			
			FROM [dbo].XEVPRM_ENTITY_DETAILS_MN
				LEFT JOIN dbo.XEVPRM_AP_DETAILS ON dbo.XEVPRM_AP_DETAILS.xevprm_ap_details_PK = [dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
				LEFT JOIN dbo.XEVPRM_MESSAGE ON dbo.XEVPRM_MESSAGE.xevprm_message_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK

			WHERE [dbo].XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1 AND
				[dbo].XEVPRM_AP_DETAILS.ap_FK = @ap_FK AND 
				dbo.XEVPRM_MESSAGE.deleted != 1 AND
				[dbo].XEVPRM_AP_DETAILS.operation_type = 4 AND	
				dbo.XEVPRM_MESSAGE.message_status_FK != 12	)

	if (@MaxMsgPK_OperationType4 is null or
		@MaxMsgPK_OperationType1 > @MaxMsgPK_OperationType4)
		if (@msg_PK >= @MaxMsgPK_OperationType1 ) set @IsActionAvailable = 1
		else set @IsActionAvailable = 0
	else set @IsActionAvailable = 0;

	if (@ap_FK is null) set @IsActionAvailable = 0;
	if (@IsActionAvailable is null) set @IsActionAvailable = 0;

	RETURN @IsActionAvailable 
end