-- GetEntitiesWP
CREATE PROCEDURE  proc_XEVPRM_LOG_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xevprm_log_PK]) AS RowNum,
		[xevprm_log_PK], [xevprm_message_FK], [log_time], [description], [username], [xevprm_status_FK]
		FROM [dbo].[XEVPRM_LOG]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_LOG]
END
