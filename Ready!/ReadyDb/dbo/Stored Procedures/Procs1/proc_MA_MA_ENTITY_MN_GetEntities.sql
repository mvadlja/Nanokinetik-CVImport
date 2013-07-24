-- GetEntities
create PROCEDURE [dbo].[proc_MA_MA_ENTITY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_ma_entity_mn_PK], [ma_FK], [ma_entity_FK], [ma_entity_type_FK]
	FROM [dbo].[MA_MA_ENTITY_MN]
END
