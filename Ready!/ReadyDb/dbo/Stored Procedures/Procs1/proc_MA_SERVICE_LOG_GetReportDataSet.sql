-- GetEntitiesWP
create PROCEDURE [dbo].[proc_MA_SERVICE_LOG_GetReportDataSet]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'ap_PK'
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_service_log_PK]) AS RowNum,
		[ma_service_log_PK], [log_time], [description], [ready_id_FK], [event_type_FK],
		evtType.name AS event_name, evtType.ma_event_type_severity_FK AS severity
		FROM [dbo].[MA_SERVICE_LOG] evt
		JOIN [dbo].[MA_EVENT_TYPE] evtType ON evtType.ma_event_type_PK = evt.event_type_FK 
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_SERVICE_LOG]
END
