-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_Save]
	@approved_substance_PK int = NULL,
	@operationtype int = NULL,
	@virtual int = NULL,
	@localnumber nvarchar(60) = NULL,
	@ev_code nvarchar(60) = NULL,
	@sourcecode nvarchar(60) = NULL,
	@resolutionmode int = NULL,
	@substancename ntext = NULL,
	@casnumber nvarchar(15) = NULL,
	@molecularformula nvarchar(255) = NULL,
	@class int = NULL,
	@cbd ntext = NULL,
	@substancetranslations_FK int = NULL,
	@substancealiases_FK int = NULL,
	@internationalcodes_FK int = NULL,
	@previous_ev_codes_FK int = NULL,
	@substancessis_FK int = NULL,
	@substance_attachment_FK int = NULL,
	@comments nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBSTANCE]
	SET
	[operationtype] = @operationtype,
	[virtual] = @virtual,
	[localnumber] = @localnumber,
	[ev_code] = @ev_code,
	[sourcecode] = @sourcecode,
	[resolutionmode] = @resolutionmode,
	[substancename] = @substancename,
	[casnumber] = @casnumber,
	[molecularformula] = @molecularformula,
	[class] = @class,
	[cbd] = @cbd,
	[substancetranslations_FK] = @substancetranslations_FK,
	[substancealiases_FK] = @substancealiases_FK,
	[internationalcodes_FK] = @internationalcodes_FK,
	[previous_ev_codes_FK] = @previous_ev_codes_FK,
	[substancessis_FK] = @substancessis_FK,
	[substance_attachment_FK] = @substance_attachment_FK,
	[comments] = @comments
	WHERE [approved_substance_PK] = @approved_substance_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBSTANCE]
		([operationtype], [virtual], [localnumber], [ev_code], [sourcecode], [resolutionmode], [substancename], [casnumber], [molecularformula], [class], [cbd], [substancetranslations_FK], [substancealiases_FK], [internationalcodes_FK], [previous_ev_codes_FK], [substancessis_FK], [substance_attachment_FK], [comments])
		VALUES
		(@operationtype, @virtual, @localnumber, @ev_code, @sourcecode, @resolutionmode, @substancename, @casnumber, @molecularformula, @class, @cbd, @substancetranslations_FK, @substancealiases_FK, @internationalcodes_FK, @previous_ev_codes_FK, @substancessis_FK, @substance_attachment_FK, @comments)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
