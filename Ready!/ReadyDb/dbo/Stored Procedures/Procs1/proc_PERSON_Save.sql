-- Save
CREATE PROCEDURE  [dbo].[proc_PERSON_Save]
	@person_PK int = NULL,
	@country_FK int = NULL,
	@name nvarchar(50) = NULL,
	@familyname nvarchar(50) = NULL,
	@ip nvarchar(50) = NULL,
	@phone nvarchar(50) = NULL,
	@address nvarchar(50) = NULL,
	@city nvarchar(50) = NULL,
	@email nvarchar(50) = NULL,
	@status int = NULL,
	@local_number nvarchar(50) = NULL,
	@ev_code nvarchar(50) = NULL,
	@givenname nvarchar(50) = NULL,
	@title nvarchar(50) = NULL,
	@company nvarchar(50) = NULL,
	@department nvarchar(50) = NULL,
	@building nvarchar(50) = NULL,
	@street nvarchar(50) = NULL,
	@state nvarchar(50) = NULL,
	@postcode nvarchar(50) = NULL,
	@countrycode nvarchar(50) = NULL,
	@tel_countrycode nvarchar(50) = NULL,
	@telnumber nvarchar(50) = NULL,
	@telextn nvarchar(50) = NULL,
	@cell_countrycode nvarchar(50) = NULL,
	@cellnumber nvarchar(50) = NULL,
	@fax_countrycode nvarchar(50) = NULL,
	@faxnumber nvarchar(50) = NULL,
	@faxextn nvarchar(50) = NULL,
	@telnum24h nvarchar(50) = NULL,
	@FullName nvarchar(200) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PERSON]
	SET
	[country_FK] = @country_FK,
	[name] = @name,
	[familyname] = @familyname,
	[ip] = @ip,
	[phone] = @phone,
	[address] = @address,
	[city] = @city,
	[email] = @email,
	[status] = @status,
	[local_number] = @local_number,
	[ev_code] = @ev_code,
	[givenname] = @givenname,
	[title] = @title,
	[company] = @company,
	[department] = @department,
	[building] = @building,
	[street] = @street,
	[state] = @state,
	[postcode] = @postcode,
	[countrycode] = @countrycode,
	[tel_countrycode] = @tel_countrycode,
	[telnumber] = @telnumber,
	[telextn] = @telextn,
	[cell_countrycode] = @cell_countrycode,
	[cellnumber] = @cellnumber,
	[fax_countrycode] = @fax_countrycode,
	[faxnumber] = @faxnumber,
	[faxextn] = @faxextn,
	[telnum24h] = @telnum24h,
	[FullName] = LTRIM(RTRIM((ISNULL(@name, '') + ' ' + ISNULL(@familyname, ''))))
	WHERE [person_PK] = @person_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PERSON]
		([country_FK], [name], [familyname], [ip], [phone], [address], [city], [email], [status], [local_number], [ev_code], [givenname], [title], [company], [department], [building], [street], [state], [postcode], [countrycode], [tel_countrycode], [telnumber], [telextn], [cell_countrycode], [cellnumber], [fax_countrycode], [faxnumber], [faxextn], [telnum24h], [FullName])
		VALUES
		(@country_FK, @name, @familyname, @ip, @phone, @address, @city, @email, @status, @local_number, @ev_code, @givenname, @title, @company, @department, @building, @street, @state, @postcode, @countrycode, @tel_countrycode, @telnumber, @telextn, @cell_countrycode, @cellnumber, @fax_countrycode, @faxnumber, @faxextn, @telnum24h, LTRIM(RTRIM((ISNULL(@name, '') + ' ' + ISNULL(@familyname, '')))))

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
