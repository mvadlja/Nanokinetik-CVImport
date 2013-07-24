
CREATE PROCEDURE  [dbo].[proc_PRODUCT_UpdateCalculatedColumnByPharmaceuticalProduct]
	@PharmaceuticalProductPk int = NULL,
	@CalculatedColumn nvarchar(50) = NULL

AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	IF (@PharmaceuticalProductPk IS NULL) RETURN;

	SET @Query = STUFF ( (
						SELECT CAST(' ' AS NVARCHAR(MAX)) + CommandTable.Command
						FROM (
							SELECT 'EXEC [dbo].[proc_PRODUCT_UpdateCalculatedColumn] ' + CAST(dbo.PRODUCT_PP_MN.product_FK AS NVARCHAR(25)) + ', ''' + @CalculatedColumn + '''; ' AS Command
							FROM dbo.PRODUCT_PP_MN
							WHERE dbo.PRODUCT_PP_MN.pp_FK = @PharmaceuticalProductPk
						) as CommandTable 
						for xml path('') ), 1, 1, '')

	EXECUTE sp_executesql @Query;
END