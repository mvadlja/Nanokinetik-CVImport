
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_UpdateCalculatedColumn]
	@PharmaceuticalProductPk int = NULL,
	@CalculatedColumn nvarchar(50) = NULL

AS

DECLARE @Products nvarchar(MAX);
DECLARE	@ActiveSubstances nvarchar(MAX);
DECLARE	@Excipients nvarchar(MAX);
DECLARE	@Adjuvants nvarchar(MAX);
DECLARE	@AdministrationRoutes nvarchar(MAX);
DECLARE	@MedicalDevices nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	IF (@PharmaceuticalProductPk IS NULL) RETURN;

	IF @CalculatedColumn = 'Products' OR @CalculatedColumn = 'All'

	BEGIN
		SET @Products = STUFF ( (
						SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + ProductTable.ProductName
						FROM (
							SELECT DISTINCT
							prod.name + ' || ' + CAST(prod.product_PK AS NVARCHAR(50)) AS ProductName
							from PRODUCT_PP_MN as ppMN
							JOIN PRODUCT prod on prod.product_PK = ppMN.product_FK
							WHERE ppMN.pp_FK = @PharmaceuticalProductPk
						) AS ProductTable 
						for xml path('') ), 1, 5, '');

		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET Products = @Products
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

	IF @CalculatedColumn = 'ActiveSubstances' OR @CalculatedColumn = 'All'

	BEGIN
		SET @ActiveSubstances = STUFF ( (
							SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + ActiveSubstanceTable.Name
							FROM (
								SELECT DISTINCT
								CASE 
									WHEN ppai.concise IS NOT NULL AND LTRIM(ppai.concise) != '' THEN ppai.concise
									WHEN ss.substance_name IS NOT NULL AND LTRIM(ss.substance_name) != '' THEN ss.substance_name
									ELSE 'N/A'
								END AS Name
								FROM dbo.PP_ACTIVE_INGREDIENT ppai
								LEFT JOIN dbo.SUBSTANCES ss on ppai.substancecode_FK = ss.substance_PK
								WHERE ppai.pp_FK = @PharmaceuticalProductPk
							) AS ActiveSubstanceTable 
							for xml path('') ), 1, 5, '');

		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET ActiveSubstances = @ActiveSubstances
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

	IF @CalculatedColumn = 'Excipients' OR @CalculatedColumn = 'All'

	BEGIN
		SET @Excipients = STUFF ( (
						SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + ExcipientTable.Name
						FROM (
							SELECT DISTINCT
							CASE
								WHEN ppexc.concise IS NOT NULL AND LTRIM(ppexc.concise) != '' THEN ppexc.concise
								WHEN ss.substance_name IS NOT NULL AND LTRIM(ss.substance_name) != '' THEN ss.substance_name
								ELSE 'N/A'
							END AS Name
							FROM dbo.PP_EXCIPIENT ppexc
							LEFT JOIN dbo.SUBSTANCES ss on ppexc.substancecode_FK=ss.substance_PK
							WHERE ppexc.pp_FK = @PharmaceuticalProductPk
						) as ExcipientTable 
						for xml path('') ), 1, 5, '')
	
		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET Excipients = @Excipients
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

	IF @CalculatedColumn = 'Adjuvants' OR @CalculatedColumn = 'All'

	BEGIN
		SET @Adjuvants = STUFF ( (
					SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + AdjuvantTable.Name
					FROM (
						SELECT DISTINCT
						CASE
							WHEN ppadj.concise IS NOT NULL AND LTRIM(ppadj.concise) != '' THEN ppadj.concise
							WHEN ss.substance_name IS NOT NULL AND LTRIM(ss.substance_name) != '' THEN ss.substance_name
							ELSE 'N/A'
						END AS Name
						FROM dbo.PP_ADJUVANT ppadj
						LEFT JOIN dbo.SUBSTANCES ss on ppadj.substancecode_FK=ss.substance_PK
						WHERE ppadj.pp_FK = @PharmaceuticalProductPk
					) as AdjuvantTable 
					for xml path('') ), 1, 5, '')

		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET Adjuvants = @Adjuvants
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

	IF @CalculatedColumn = 'AdministrationRoutes' OR @CalculatedColumn = 'All'

	BEGIN
		SET @AdministrationRoutes = STUFF ( (
				SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + AdministrationRouteTable.Name
					FROM (
					SELECT DISTINCT
					CASE
						WHEN ppAR.adminroutecode IS NOT NULL AND LTRIM(ppAR.adminroutecode) != '' THEN ppAR.adminroutecode
						ELSE 'N/A'
					END AS Name
					FROM [dbo].[PP_AR_MN]
					LEFT JOIN [dbo].[PP_ADMINISTRATION_ROUTE] ppAR on ppAR.adminroute_PK = [dbo].[PP_AR_MN].admin_route_FK
					WHERE [dbo].[PP_AR_MN].pharmaceutical_product_FK = @PharmaceuticalProductPk
				) as AdministrationRouteTable 
				for xml path('') ), 1, 5, '')

		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET AdministrationRoutes = @AdministrationRoutes
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

	IF @CalculatedColumn = 'MedicalDevices' OR @CalculatedColumn = 'All'

	BEGIN
		SET @MedicalDevices = STUFF ( (
				SELECT CAST(' ||| ' AS NVARCHAR(MAX)) + MedicalDeviceTable.Name
					FROM (
					SELECT DISTINCT
					CASE
						WHEN ppMD.medicaldevicecode IS NOT NULL AND LTRIM(ppMD.medicaldevicecode) != '' THEN ppMD.medicaldevicecode
						ELSE 'N/A'
					END AS Name
					FROM [dbo].[PP_MD_MN]
					LEFT JOIN [dbo].[PP_MEDICAL_DEVICE] ppMD on ppMD.medicaldevice_PK = [dbo].[PP_MD_MN].pp_medical_device_FK
					WHERE [dbo].[PP_MD_MN].pharmaceutical_product_FK = @PharmaceuticalProductPk
				) as MedicalDeviceTable 
				for xml path('') ), 1, 5, '')

		UPDATE dbo.PHARMACEUTICAL_PRODUCT SET MedicalDevices = @MedicalDevices
		WHERE dbo.PHARMACEUTICAL_PRODUCT.pharmaceutical_product_PK = @PharmaceuticalProductPk
	END

END