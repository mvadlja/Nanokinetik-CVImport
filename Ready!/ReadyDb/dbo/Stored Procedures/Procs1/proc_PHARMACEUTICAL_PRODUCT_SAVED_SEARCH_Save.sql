-- Save
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_Save]
	@pharmaceutical_products_PK int = NULL,
	@name nvarchar(450) = NULL,
	@responsible_user_FK int = NULL,
	@description nvarchar(MAX) = NULL,
	@product_FK int = NULL,
	@Pharmform_FK int = NULL,
	@comments nvarchar(MAX) = NULL,
	@displayName nvarchar(100) = NULL,
	@user_FK int = NULL,
	@gridLayout nvarchar(MAX) = NULL,
	@isPublic bit = NULL,
	@administrationRoutes nvarchar(MAX) = NULL,
	@activeIngridients nvarchar(MAX) = NULL,
	@excipients nvarchar(MAX) = NULL,
	@adjuvants nvarchar(MAX) = NULL,
	@medical_devices nvarchar(MAX) = NUll,
	@pp_FK int=null
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
	SET
	[name] = @name,
	[responsible_user_FK] = @responsible_user_FK,
	[description] = @description,
	[product_FK] = @product_FK,
	[Pharmform_FK] = @Pharmform_FK,
	[comments] = @comments,
	[displayName] = @displayName,
	[user_FK] = @user_FK,
	[gridLayout] = @gridLayout,
	[isPublic] = @isPublic,
	[administrationRoutes] = @administrationRoutes,
	[activeIngridients] = @activeIngridients,
	[excipients] = @excipients,
	[adjuvants] = @adjuvants,
	[medical_devices] = @medical_devices,
	pp_FK=@pp_FK
	WHERE [pharmaceutical_products_PK] = @pharmaceutical_products_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
		([name], [responsible_user_FK], [description], [product_FK], [Pharmform_FK], [comments], [displayName], [user_FK], [gridLayout], [isPublic], [administrationRoutes], [activeIngridients], [excipients], [adjuvants], [medical_devices],pp_FK)
		VALUES
		(@name, @responsible_user_FK, @description, @product_FK, @Pharmform_FK, @comments, @displayName, @user_FK, @gridLayout, @isPublic, @administrationRoutes, @activeIngridients, @excipients, @adjuvants, @medical_devices,@pp_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
