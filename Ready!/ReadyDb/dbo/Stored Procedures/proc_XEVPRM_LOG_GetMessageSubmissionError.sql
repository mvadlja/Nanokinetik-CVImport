-- GetEntity
create PROCEDURE  proc_XEVPRM_LOG_GetMessageSubmissionError
	@XevprmMessagePk int = NULL,
	@MessageType nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@MessageType = 'Xevprm')
	BEGIN
		 SELECT TOP 1 SUBSTRING(l.[description], 27, LEN(l.[description]) - 26) as [Description]
		 FROM [dbo].[XEVPRM_LOG] l
		 WHERE l.[description] like 'Xevprm submission failed.%'
		 AND l.[xevprm_message_FK] = @XevprmMessagePk
		 AND @XevprmMessagePk IS NOT NULL
		 ORDER BY xevprm_log_PK DESC
	END
	ELSE IF (@MessageType = 'MDN')
	BEGIN
		SELECT TOP 1 SUBSTRING(l.[description], 24, LEN(l.[description]) - 23) as [Description]
		FROM [dbo].[XEVPRM_LOG] l
		WHERE l.[description] like 'MDN submission failed.%'
		AND l.[xevprm_message_FK] = @XevprmMessagePk
		AND @XevprmMessagePk IS NOT NULL
		ORDER BY xevprm_log_PK DESC
	END
END