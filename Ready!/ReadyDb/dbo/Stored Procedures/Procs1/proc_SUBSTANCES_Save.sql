-- Save
CREATE PROCEDURE  [dbo].[proc_SUBSTANCES_Save]
	@substance_PK int = NULL,
	@ev_code nvarchar(50) = NULL,
	@substance_name nvarchar(255) = NULL,
	@synonym1 nvarchar(255) = NULL,
	@synonym1_language nvarchar(255) = NULL,
	@synonym2 nvarchar(255) = NULL,
	@synonym2_language nvarchar(255) = NULL,
	@synonym3 nvarchar(255) = NULL,
	@synonym3_language nvarchar(255) = NULL,
	@synonym4 nvarchar(255) = NULL,
	@synonym4_language nvarchar(255) = NULL,
	@synonym5 nvarchar(255) = NULL,
	@synonym5_language nvarchar(255) = NULL,
	@synonym6 nvarchar(255) = NULL,
	@synonym6_language nvarchar(255) = NULL,
	@synonym7 nvarchar(255) = NULL,
	@synonym7_language nvarchar(255) = NULL,
	@synonym8 nvarchar(255) = NULL,
	@synonym8_language nvarchar(255) = NULL,
	@synonym9 nvarchar(255) = NULL,
	@synonym9_language nvarchar(255) = NULL,
	@synonym10 nvarchar(255) = NULL,
	@synonym10_language nvarchar(255) = NULL,
	@synonym11 nvarchar(255) = NULL,
	@synonym11_language nvarchar(255) = NULL,
	@synonym12 nvarchar(255) = NULL,
	@synonym12_language nvarchar(255) = NULL,
	@synonym13 nvarchar(255) = NULL,
	@synonym13_language nvarchar(255) = NULL,
	@synonym14 nvarchar(255) = NULL,
	@synonym14_language nvarchar(255) = NULL,
	@synonym15 nvarchar(255) = NULL,
	@synonym15_language nvarchar(255) = NULL,
	@synonym16 nvarchar(255) = NULL,
	@synonym16_language nvarchar(255) = NULL,
	@synonym17 nvarchar(255) = NULL,
	@synonym17_language nvarchar(255) = NULL,
	@synonym18 nvarchar(255) = NULL,
	@synonym18_language nvarchar(255) = NULL,
	@synonym19 nvarchar(255) = NULL,
	@synonym19_language nvarchar(255) = NULL,
	@synonym20 nvarchar(255) = NULL,
	@synonym20_language nvarchar(255) = NULL,
	@synonym21 nvarchar(255) = NULL,
	@synonym21_language nvarchar(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCES]
	SET
	[ev_code] = @ev_code,
	[substance_name] = @substance_name,
	[synonym1] = @synonym1,
	[synonym1_language] = @synonym1_language,
	[synonym2] = @synonym2,
	[synonym2_language] = @synonym2_language,
	[synonym3] = @synonym3,
	[synonym3_language] = @synonym3_language,
	[synonym4] = @synonym4,
	[synonym4_language] = @synonym4_language,
	[synonym5] = @synonym5,
	[synonym5_language] = @synonym5_language,
	[synonym6] = @synonym6,
	[synonym6_language] = @synonym6_language,
	[synonym7] = @synonym7,
	[synonym7_language] = @synonym7_language,
	[synonym8] = @synonym8,
	[synonym8_language] = @synonym8_language,
	[synonym9] = @synonym9,
	[synonym9_language] = @synonym9_language,
	[synonym10] = @synonym10,
	[synonym10_language] = @synonym10_language,
	[synonym11] = @synonym11,
	[synonym11_language] = @synonym11_language,
	[synonym12] = @synonym12,
	[synonym12_language] = @synonym12_language,
	[synonym13] = @synonym13,
	[synonym13_language] = @synonym13_language,
	[synonym14] = @synonym14,
	[synonym14_language] = @synonym14_language,
	[synonym15] = @synonym15,
	[synonym15_language] = @synonym15_language,
	[synonym16] = @synonym16,
	[synonym16_language] = @synonym16_language,
	[synonym17] = @synonym17,
	[synonym17_language] = @synonym17_language,
	[synonym18] = @synonym18,
	[synonym18_language] = @synonym18_language,
	[synonym19] = @synonym19,
	[synonym19_language] = @synonym19_language,
	[synonym20] = @synonym20,
	[synonym20_language] = @synonym20_language,
	[synonym21] = @synonym21,
	[synonym21_language] = @synonym21_language
	WHERE [substance_PK] = @substance_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCES]
		([ev_code], [substance_name], [synonym1], [synonym1_language], [synonym2], [synonym2_language], [synonym3], [synonym3_language], [synonym4], [synonym4_language], [synonym5], [synonym5_language], [synonym6], [synonym6_language], [synonym7], [synonym7_language], [synonym8], [synonym8_language], [synonym9], [synonym9_language], [synonym10], [synonym10_language], [synonym11], [synonym11_language], [synonym12], [synonym12_language], [synonym13], [synonym13_language], [synonym14], [synonym14_language], [synonym15], [synonym15_language], [synonym16], [synonym16_language], [synonym17], [synonym17_language], [synonym18], [synonym18_language], [synonym19], [synonym19_language], [synonym20], [synonym20_language], [synonym21], [synonym21_language])
		VALUES
		(@ev_code, @substance_name, @synonym1, @synonym1_language, @synonym2, @synonym2_language, @synonym3, @synonym3_language, @synonym4, @synonym4_language, @synonym5, @synonym5_language, @synonym6, @synonym6_language, @synonym7, @synonym7_language, @synonym8, @synonym8_language, @synonym9, @synonym9_language, @synonym10, @synonym10_language, @synonym11, @synonym11_language, @synonym12, @synonym12_language, @synonym13, @synonym13_language, @synonym14, @synonym14_language, @synonym15, @synonym15_language, @synonym16, @synonym16_language, @synonym17, @synonym17_language, @synonym18, @synonym18_language, @synonym19, @synonym19_language, @synonym20, @synonym20_language, @synonym21, @synonym21_language)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
