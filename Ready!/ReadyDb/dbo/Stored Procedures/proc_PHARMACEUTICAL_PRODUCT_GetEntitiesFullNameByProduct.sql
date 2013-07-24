-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetEntitiesFullNameByProduct]
	@ProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	pp.[pharmaceutical_product_PK], 

	ISNULL(NULLIF(LTRIM(
		stuff (	(
		SELECT cast('+' as varchar(max)) + tableAI.aiName
		from (
			SELECT DISTINCT
			ppai.pp_FK,
			CASE 
				WHEN ppai.concise IS NOT NULL AND LTRIM(ppai.concise) != '' THEN ppai.concise
				WHEN ss.substance_name IS NOT NULL AND LTRIM(ss.substance_name) != '' THEN ss.substance_name
				ELSE 'N/A'
			END as aiName
			FROM dbo.PP_ACTIVE_INGREDIENT ppai
			LEFT JOIN dbo.SUBSTANCES ss on ppai.substancecode_FK = ss.substance_PK
		) as tableAI 
		WHERE tableAI.pp_FK = pp.pharmaceutical_product_PK
		for xml path('')
		), 1, 1, '') + 
		' (' + ISNULL(NULLIF(LTRIM(pf.name), ''), 'N/A') + ')'
	), ''), pp.name) AS FullName

	FROM [dbo].[PHARMACEUTICAL_PRODUCT] pp
	JOIN [dbo].[PRODUCT_PP_MN] ON [dbo].[PRODUCT_PP_MN].pp_FK = pp.pharmaceutical_product_PK
	LEFT JOIN [dbo].PHARMACEUTICAL_FORM pf ON pf.pharmaceutical_form_PK = pp.Pharmform_FK
	WHERE [dbo].[PRODUCT_PP_MN].product_FK = @ProductPk AND @ProductPk IS NOT NULL
END