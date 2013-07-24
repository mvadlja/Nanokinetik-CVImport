-- Save
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_Save]
	@organization_PK int = NULL,
	@type_org_EMEA int = NULL,
	@type_org_FK int = NULL,
	@name_org nvarchar(100) = NULL,
	@localnumber nvarchar(60) = NULL,
	@ev_code nvarchar(60) = NULL,
	@organizationsenderid_EMEA nvarchar(60) = NULL,
	@address nvarchar(100) = NULL,
	@city nvarchar(50) = NULL,
	@state nvarchar(50) = NULL,
	@postcode nvarchar(50) = NULL,
	@countrycode_FK int = NULL,
	@tel_number nvarchar(50) = NULL,
	@tel_extension nvarchar(50) = NULL,
	@tel_countrycode nvarchar(5) = NULL,
	@fax_number nvarchar(50) = NULL,
	@fax_extenstion nvarchar(50) = NULL,
	@fax_countrycode nvarchar(5) = NULL,
	@email nvarchar(100) = NULL,
	@comment nvarchar(MAX) = NULL,
	@mfl_evcode nvarchar(60) = NULL,
	@mflcompany nvarchar(100) = NULL,
	@mfldepartment nvarchar(100) = NULL,
	@mflbuilding nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ORGANIZATION]
	SET
	[type_org_EMEA] = @type_org_EMEA,
	[type_org_FK] = @type_org_FK,
	[name_org] = @name_org,
	[localnumber] = @localnumber,
	[ev_code] = @ev_code,
	[organizationsenderid_EMEA] = @organizationsenderid_EMEA,
	[address] = @address,
	[city] = @city,
	[state] = @state,
	[postcode] = @postcode,
	countrycode_FK = @countrycode_FK,
	[tel_number] = @tel_number,
	[tel_extension] = @tel_extension,
	[tel_countrycode] = @tel_countrycode,
	[fax_number] = @fax_number,
	[fax_extenstion] = @fax_extenstion,
	[fax_countrycode] = @fax_countrycode,
	[email] = @email,
	[comment] = @comment,
	[mfl_evcode] = @mfl_evcode,
	[mflcompany] = @mflcompany,
	[mfldepartment] = @mfldepartment,
	[mflbuilding] = @mflbuilding
	WHERE [organization_PK] = @organization_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ORGANIZATION]
		([type_org_EMEA], [type_org_FK], [name_org], [localnumber], [ev_code], [organizationsenderid_EMEA], [address], [city], [state], [postcode], [countrycode_FK], [tel_number], [tel_extension], [tel_countrycode], [fax_number], [fax_extenstion], [fax_countrycode], [email], [comment], [mfl_evcode], [mflcompany], [mfldepartment], [mflbuilding])
		VALUES
		(@type_org_EMEA, @type_org_FK, @name_org, @localnumber, @ev_code, @organizationsenderid_EMEA, @address, @city, @state, @postcode, @countrycode_FK, @tel_number, @tel_extension, @tel_countrycode, @fax_number, @fax_extenstion, @fax_countrycode, @email, @comment, @mfl_evcode, @mflcompany, @mfldepartment, @mflbuilding)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
