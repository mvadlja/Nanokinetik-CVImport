
CREATE PROCEDURE  [dbo].[proc_PRODUCT_UpdateCalculatedColumn]
	@ProductPk int = NULL,
	@CalculatedColumn nvarchar(50) = NULL

AS

DECLARE @ProductName nvarchar(MAX);
DECLARE	@Countries nvarchar(MAX);
DECLARE	@ActiveSubstances nvarchar(MAX);
DECLARE	@DrugAtcs nvarchar(MAX);


BEGIN
	SET NOCOUNT ON;

	IF (@ProductPk IS NULL) RETURN;

	IF @CalculatedColumn = 'ProductName' OR @CalculatedColumn = 'All'

	BEGIN

		SET @ProductName =  (	SELECT 
								CASE 
									WHEN name IS NOT NULL AND LTRIM(name) != ''  THEN prod.name
									ELSE 'N/A'
								END +
								CASE 
									WHEN product_number IS NOT NULL AND LTRIM(product_number) != ''  THEN ' (' + prod.product_number + ')'
									ELSE ''
								END as ProductName
							FROM dbo.PRODUCT AS prod
							WHERE prod.product_PK = @ProductPk	);

		UPDATE dbo.PRODUCT SET ProductName = @ProductName
		WHERE dbo.PRODUCT.product_PK = @ProductPk
	END

	IF @CalculatedColumn = 'Countries' OR @CalculatedColumn = 'All'

	BEGIN
		SET @Countries = STUFF ( (
					SELECT CAST(', ' AS NVARCHAR(MAX)) + CountyTable.Name
					from (
						SELECT DISTINCT
						c.abbreviation as Name
						FROM dbo.PRODUCT_COUNTRY_MN pcmn
						JOIN dbo.COUNTRY c on c.country_PK = pcmn.country_FK
						WHERE pcmn.product_FK = @ProductPk
					) as CountyTable
					for xml path('') ), 1, 2, '')
		
		UPDATE dbo.PRODUCT SET Countries = @Countries
		WHERE dbo.PRODUCT.product_PK = @ProductPk
	END

	IF @CalculatedColumn = 'ActiveSubstances' OR @CalculatedColumn = 'All'

	BEGIN
		SET @ActiveSubstances = STUFF ( (
					SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + ActiveSubstanceTable.Name
					FROM (
						SELECT DISTINCT
						STUFF ( (
							SELECT CAST('+' AS NVARCHAR(MAX)) + ActiveSubstanceTable.Name
							FROM (
								SELECT DISTINCT
								CASE 
									WHEN ppai.concise IS NOT NULL AND LTRIM(ppai.concise) != '' THEN ppai.concise
									WHEN ss.substance_name IS NOT NULL AND LTRIM(ss.substance_name) != '' THEN ss.substance_name
									ELSE 'N/A'
								END AS Name
								FROM dbo.PP_ACTIVE_INGREDIENT ppai
								LEFT JOIN dbo.SUBSTANCES ss on ppai.substancecode_FK = ss.substance_PK
								WHERE ppai.pp_FK = ppmn.pp_FK
							) AS ActiveSubstanceTable 
							for xml path('') ), 1, 1, '') + 
						CASE
							WHEN pf.name IS NOT NULL AND LTRIM(pf.name) != '' THEN ' (' + pf.name + ')'
							ELSE ' (N/A)'
						END AS Name
						from [dbo].[PRODUCT_PP_MN] ppmn
						JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppmn.pp_FK
						LEFT JOIN [dbo].PHARMACEUTICAL_FORM pf ON pf.pharmaceutical_form_PK = pp.Pharmform_FK
						WHERE ppmn.product_FK = @ProductPk
					) as ActiveSubstanceTable 
					for xml path('') ), 1, 5, '');

		UPDATE dbo.PRODUCT SET ActiveSubstances = @ActiveSubstances
		WHERE dbo.PRODUCT.product_PK = @ProductPk
	END

	IF @CalculatedColumn = 'DrugAtcs' OR @CalculatedColumn = 'All'

	BEGIN
		SET @DrugAtcs = STUFF ( (
				SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + DrugAtcTable.Name
					FROM (
					SELECT DISTINCT
					CASE 
						WHEN atc.atccode IS NOT NULL AND LTRIM(atc.atccode) != ''  THEN atc.atccode
						ELSE 'N/A'
					END +
					CASE 
						WHEN atc.name IS NOT NULL AND LTRIM(atc.name) != ''  THEN ' (' + atc.name + ')'
						ELSE ''
					END AS Name
					FROM [dbo].PRODUCT_ATC_MN paMN
					JOIN [dbo].ATC atc on atc.atc_PK = paMN.atc_FK
					WHERE paMN.product_FK = @ProductPk
				) as DrugAtcTable 
				for xml path('') ), 1, 5, '')

		UPDATE dbo.PRODUCT SET DrugAtcs = @DrugAtcs
		WHERE dbo.PRODUCT.product_PK = @ProductPk
	END

END