-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PP_MEDICAL_DEVICE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [medicaldevice_PK]) AS RowNum,
		[medicaldevice_PK], [medicaldevicecode], [ev_code]
		FROM [dbo].[PP_MEDICAL_DEVICE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_MEDICAL_DEVICE]
END
