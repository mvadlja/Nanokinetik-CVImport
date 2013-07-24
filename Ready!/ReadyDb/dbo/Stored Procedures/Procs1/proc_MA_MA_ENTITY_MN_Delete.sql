-- Delete
create PROCEDURE [dbo].[proc_MA_MA_ENTITY_MN_Delete]
	@ma_ma_entity_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_MA_ENTITY_MN] WHERE [ma_ma_entity_mn_PK] = @ma_ma_entity_mn_PK
END
