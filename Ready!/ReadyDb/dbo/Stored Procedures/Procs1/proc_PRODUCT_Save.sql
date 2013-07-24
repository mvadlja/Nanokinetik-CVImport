-- Save
CREATE PROCEDURE  [dbo].[proc_PRODUCT_Save]
	@product_PK int = NULL,
	@newownerid nvarchar(60) = NULL,
	@senderlocalcode nvarchar(100) = NULL,
	@orphan_drug bit = NULL,
	@intensive_monitoring int = NULL,
	@authorisation_procedure int = NULL,
	@comments nvarchar(MAX) = NULL,
	@responsible_user_person_FK int = NULL,
	@psur nvarchar(250) = NULL,
	@next_dlp datetime = NULL,
	@name nvarchar(2000) = NULL,
	@description nvarchar(MAX) = NULL,
	@client_organization_FK int = NULL,
	@type_product_FK int = NULL,
	@product_number nvarchar(100) = NULL,
	@product_ID nvarchar(100) = NULL,
	@mrp_dcp nvarchar(100) = null,
	@eu_number nvarchar(100) = null,
	@client_group_FK int = NULL, 
	@region_FK int = NULL, 
	@batch_size nvarchar(500) = NULL, 
	@pack_size nvarchar(500) = NULL, 
	@storage_conditions_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PRODUCT]
	SET
	[newownerid] = @newownerid,
	[senderlocalcode] = @senderlocalcode,
	[orphan_drug] = @orphan_drug,
	[intensive_monitoring] = @intensive_monitoring,
	[authorisation_procedure] = @authorisation_procedure,
	[comments] = @comments,
	[responsible_user_person_FK] = @responsible_user_person_FK,
	[psur] = @psur,
	[next_dlp] = @next_dlp,
	[name] = @name,
	[description] = @description,
	[client_organization_FK] = @client_organization_FK,
	[type_product_FK] = @type_product_FK,
	[product_number] = @product_number,
	[product_ID] = @product_ID,
	[mrp_dcp] = @mrp_dcp,
	[eu_number] = @eu_number,
	[client_group_FK] = @client_group_FK, 
	[region_FK] = @region_FK, 
	[batch_size] = @batch_size, 
	[pack_size] = @pack_size, 
	[storage_conditions_FK] = @storage_conditions_FK
	WHERE [product_PK] = @product_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PRODUCT]
		([newownerid], [senderlocalcode], [orphan_drug], [intensive_monitoring], [authorisation_procedure], [comments], [responsible_user_person_FK], [psur], [next_dlp], [name], [description], [client_organization_FK], [type_product_FK], [product_number], [product_ID], [mrp_dcp], [eu_number], [client_group_FK], [region_FK], [batch_size], [pack_size], [storage_conditions_FK])
		VALUES
		(@newownerid, @senderlocalcode, @orphan_drug, @intensive_monitoring, @authorisation_procedure, @comments, @responsible_user_person_FK, @psur, @next_dlp, @name, @description, @client_organization_FK, @type_product_FK, @product_number, @product_ID, @mrp_dcp, @eu_number, @client_group_FK, @region_FK, @batch_size, @pack_size, @storage_conditions_FK )

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
