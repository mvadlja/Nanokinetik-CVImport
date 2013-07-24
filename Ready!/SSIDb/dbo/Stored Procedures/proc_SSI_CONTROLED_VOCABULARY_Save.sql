
-- Save
CREATE PROCEDURE [dbo].[proc_SSI_CONTROLED_VOCABULARY_Save]
	@ssi__cont_voc_PK int = NULL,
	@list_name nvarchar(255) = NULL,
	@term_id float = NULL,
	@term_name_english nvarchar(255) = NULL,
	@latin_name_latin nvarchar(255) = NULL,
	@synonim1 nvarchar(255) = NULL,
	@synonim2 nvarchar(255) = NULL,
	@Description nvarchar(MAX) = NULL,
	@Field8 nvarchar(255) = NULL,
	@Field9 nvarchar(255) = NULL,
	@Field10 nvarchar(255) = NULL,
	@Field11 nvarchar(255) = NULL,
	@Field12 nvarchar(255) = NULL,
	@Field13 nvarchar(255) = NULL,
	@Field14 nvarchar(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SSI_CONTROLED_VOCABULARY]
	SET
	[list_name] = @list_name,
	[term_id] = @term_id,
	[term_name_english] = @term_name_english,
	[latin_name_latin] = @latin_name_latin,
	[synonim1] = @synonim1,
	[synonim2] = @synonim2,
	[Description] = @Description,
	[Field8] = @Field8,
	[Field9] = @Field9,
	[Field10] = @Field10,
	[Field11] = @Field11,
	[Field12] = @Field12,
	[Field13] = @Field13,
	[Field14] = @Field14
	WHERE [ssi__cont_voc_PK] = @ssi__cont_voc_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SSI_CONTROLED_VOCABULARY]
		([list_name], [term_id], [term_name_english], [latin_name_latin], [synonim1], [synonim2], [Description], [Field8], [Field9], [Field10], [Field11], [Field12], [Field13], [Field14])
		VALUES
		(@list_name, @term_id, @term_name_english, @latin_name_latin, @synonim1, @synonim2, @Description, @Field8, @Field9, @Field10, @Field11, @Field12, @Field13, @Field14)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
