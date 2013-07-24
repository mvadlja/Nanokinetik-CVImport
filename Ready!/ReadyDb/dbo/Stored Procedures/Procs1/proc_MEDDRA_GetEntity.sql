-- GetEntity
CREATE PROCEDURE  [dbo].[proc_MEDDRA_GetEntity]
	@meddra_pk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[meddra_pk], [version_type_FK], [level_type_FK], [code], [term],
	MeddraFullName = 
		ISNULL(NULLIF('<' + meddraVersion.name + '>, ', '<>'), '') + 
		ISNULL(NULLIF(meddraLevel.name + ', ', ', '), '') + 
		ISNULL(meddra.code, '') + 
		ISNULL(NULLIF(', ' + meddra.term, ', '), '')
	FROM [dbo].[MEDDRA_AP_MN] meddraAuthorisedProductMn
	LEFT JOIN [dbo].[MEDDRA] meddra on meddra.meddra_pk = meddraAuthorisedProductMn.meddra_FK
	LEFT JOIN [TYPE] meddraVersion on meddraVersion.type_PK = meddra.version_type_FK
	LEFT JOIN [TYPE] meddraLevel on meddraLevel.type_PK = meddra.level_type_FK
	WHERE [meddra_pk] = @meddra_pk
END
