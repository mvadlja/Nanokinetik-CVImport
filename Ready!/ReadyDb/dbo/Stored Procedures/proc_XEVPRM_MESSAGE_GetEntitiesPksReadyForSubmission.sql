﻿create PROCEDURE  proc_XEVPRM_MESSAGE_GetEntitiesPksReadyForSubmission
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[XEVPRM_MESSAGE].xevprm_message_PK
	
	FROM [dbo].[XEVPRM_MESSAGE]
	
	WHERE [dbo].[XEVPRM_MESSAGE].[message_status_FK] = 4
END