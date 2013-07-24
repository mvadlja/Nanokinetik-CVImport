-- Save
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_Save]
	@pharmaceutical_product_PK int = NULL,
	@name nvarchar(450) = NULL,
	@ID nvarchar(100) = NULL,
	@responsible_user_FK int = NULL,
	@description nvarchar(MAX) = NULL,
	@comments nvarchar(MAX) = NULL,
	@Pharmform_FK int = NULL,
	@booked_slots nvarchar(MAX)=NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PHARMACEUTICAL_PRODUCT]
	SET
	[name] = @name,
	[ID] = @ID,
	[responsible_user_FK] = @responsible_user_FK,
	[description] = @description,
	[comments] = @comments,
	[Pharmform_FK] = @Pharmform_FK,
	[booked_slots]=@booked_slots
	WHERE [pharmaceutical_product_PK] = @pharmaceutical_product_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PHARMACEUTICAL_PRODUCT]
		([name], [ID], [responsible_user_FK], [description], [comments], [Pharmform_FK],[booked_slots])
		VALUES
		(@name, @ID, @responsible_user_FK, @description, @comments, @Pharmform_FK,@booked_slots)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
