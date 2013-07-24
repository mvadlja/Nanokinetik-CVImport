-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SAVED_SEARCH_Save]
	@product_saved_search_PK int = NULL,
	@name nvarchar(2000) = NULL,
	@pharmaaceutical_product_FK int = NULL,
	@indication_FK int = NULL,
	@product_number nvarchar(100) = NULL,
	@type_product_FK int = NULL,
	@client_organization_FK int = NULL,
	@domain_FK int = NULL,
	@procedure_type int = NULL,
	@product_ID nvarchar(100) = NULL,
	@country_FK int = NULL,
	@manufacturer_FK int = NULL,
	@psur nvarchar(250) = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL,
	@nextdlp_from date = NULL,
	@nextdlp_to date = NULL,
	@responsible_user_FK int = NULL,
	@drug_atcs nvarchar(max) = NULL,
	@client_name nvarchar(2000) = NULL,
	@article57_reporting bit = NULL,
	@ActiveSubstances nvarchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT_SAVED_SEARCH]
	SET
	[name] = @name,
	[pharmaaceutical_product_FK] = @pharmaaceutical_product_FK,
	[indication_FK] = @indication_FK,
	[product_number] = @product_number,
	[type_product_FK] = @type_product_FK,
	[client_organization_FK] = @client_organization_FK,
	[domain_FK] = @domain_FK,
	[procedure_type] = @procedure_type,
	[product_ID] = @product_ID,
	[country_FK] = @country_FK,
	[manufacturer_FK] = @manufacturer_FK,
	[psur] = @psur,
	[displayName] = @displayName,
	[user_FK] = @user_FK,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic,
	[nextdlp_from] = @nextdlp_from,
	[nextdlp_to] = @nextdlp_to,
	[responsible_user_FK]=@responsible_user_FK,
	[drug_atcs] = @drug_atcs ,
	[client_name] = @client_name,
	[article57_reporting] = @article57_reporting,
	[ActiveSubstances] = @ActiveSubstances
	WHERE [product_saved_search_PK] = @product_saved_search_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT_SAVED_SEARCH]
		([name], [pharmaaceutical_product_FK], [indication_FK], [product_number], [type_product_FK], [client_organization_FK], [domain_FK], [procedure_type], [product_ID], [country_FK], [manufacturer_FK], [psur], [displayName], [user_FK], [gridLayout], [isPublic], [nextdlp_from], [nextdlp_to],[responsible_user_FK],[drug_atcs],client_name, [article57_reporting], [ActiveSubstances])
		VALUES
		(@name, @pharmaaceutical_product_FK, @indication_FK, @product_number, @type_product_FK, @client_organization_FK, @domain_FK, @procedure_type, @product_ID, @country_FK, @manufacturer_FK, @psur, @displayName, @user_FK, @gridLayout, @isPublic, @nextdlp_from, @nextdlp_to,@responsible_user_FK, @drug_atcs,@client_name, @article57_reporting, @ActiveSubstances )

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
