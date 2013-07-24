-- Save
CREATE PROCEDURE  [dbo].[proc_COUNTRY_Save]
	@country_PK int = NULL,
	@name nvarchar(50) = NULL,
	@abbreviation nvarchar(10) = NULL,
	@region nvarchar(25) = NULL,
	@code nvarchar(4) = NULL,
	@custom_sort_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[COUNTRY]
	SET
	[name] = @name,
	[abbreviation] = @abbreviation,
	[region] = @region,
	[code] = @code,
	[custom_sort_ID] = @custom_sort_ID
	WHERE [country_PK] = @country_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[COUNTRY]
		([name], [abbreviation], [region], [code], [custom_sort_ID])
		VALUES
		(@name, @abbreviation, @region, @code, @custom_sort_ID)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
