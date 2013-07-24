-- Save
CREATE PROCEDURE  [dbo].[proc_MASTER_FILE_LOCATION_Save]
	@master_file_location_PK int = NULL,
	@localnumber nvarchar(60) = NULL,
	@ev_code nvarchar(60) = NULL,
	@mflcompany nvarchar(100) = NULL,
	@mfldepartment nvarchar(100) = NULL,
	@mflbuilding nvarchar(100) = NULL,
	@mflstreet nvarchar(100) = NULL,
	@mflcity nvarchar(35) = NULL,
	@mflstate nvarchar(40) = NULL,
	@mflpostcode nvarchar(15) = NULL,
	@mflcountrycode nvarchar(2) = NULL,
	@comments nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MASTER_FILE_LOCATION]
	SET
	[localnumber] = @localnumber,
	[ev_code] = @ev_code,
	[mflcompany] = @mflcompany,
	[mfldepartment] = @mfldepartment,
	[mflbuilding] = @mflbuilding,
	[mflstreet] = @mflstreet,
	[mflcity] = @mflcity,
	[mflstate] = @mflstate,
	[mflpostcode] = @mflpostcode,
	[mflcountrycode] = @mflcountrycode,
	[comments] = @comments
	WHERE [master_file_location_PK] = @master_file_location_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MASTER_FILE_LOCATION]
		([localnumber], [ev_code], [mflcompany], [mfldepartment], [mflbuilding], [mflstreet], [mflcity], [mflstate], [mflpostcode], [mflcountrycode], [comments])
		VALUES
		(@localnumber, @ev_code, @mflcompany, @mfldepartment, @mflbuilding, @mflstreet, @mflcity, @mflstate, @mflpostcode, @mflcountrycode, @comments)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
