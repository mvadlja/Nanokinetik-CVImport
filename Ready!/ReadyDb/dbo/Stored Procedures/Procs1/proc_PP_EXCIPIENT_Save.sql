-- Save
CREATE PROCEDURE  [dbo].[proc_PP_EXCIPIENT_Save]
	@excipient_PK int = NULL,
	@substancecode_FK int = NULL,
	@resolutionmode int = NULL,
	@concentrationtypecode int = NULL,
	@lowamountnumervalue decimal(18,5) = NULL,
	@lowamountnumerprefix nvarchar(12) = NULL,
	@lowamountnumerunit nvarchar(70) = NULL,
	@lowamountdenomvalue decimal(18,5) = NULL,
	@lowamountdenomprefix nvarchar(12) = NULL,
	@lowamountdenomunit nvarchar(70) = NULL,
	@highamountnumervalue decimal(18,5) = NULL,
	@highamountnumerprefix nvarchar(12) = NULL,
	@higamountnumerunit nvarchar(70) = NULL,
	@highamountdenomvalue decimal(18,5) = NULL,
	@highamountdenomprefix nvarchar(12) = NULL,
	@highamountdenomunit nvarchar(70) = NULL,
	@pp_FK int = NULL,
	@userID int = NULL,
	@ExpressedBy_FK int = NULL,
	@concise nvarchar(150)=NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_EXCIPIENT]
	SET
	[substancecode_FK] = @substancecode_FK,
	[resolutionmode] = @resolutionmode,
	[concentrationtypecode] = @concentrationtypecode,
	[lowamountnumervalue] = @lowamountnumervalue,
	[lowamountnumerprefix] = @lowamountnumerprefix,
	[lowamountnumerunit] = @lowamountnumerunit,
	[lowamountdenomvalue] = @lowamountdenomvalue,
	[lowamountdenomprefix] = @lowamountdenomprefix,
	[lowamountdenomunit] = @lowamountdenomunit,
	[highamountnumervalue] = @highamountnumervalue,
	[highamountnumerprefix] = @highamountnumerprefix,
	[higamountnumerunit] = @higamountnumerunit,
	[highamountdenomvalue] = @highamountdenomvalue,
	[highamountdenomprefix] = @highamountdenomprefix,
	[highamountdenomunit] = @highamountdenomunit,
	[pp_FK] = @pp_FK,
	[userID] = @userID,
	[ExpressedBy_FK] = @ExpressedBy_FK,
	[concise] = @concise
	WHERE [excipient_PK] = @excipient_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_EXCIPIENT]
		([substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [higamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [ExpressedBy_FK], [concise])
		VALUES
		(@substancecode_FK, @resolutionmode, @concentrationtypecode, @lowamountnumervalue, @lowamountnumerprefix, @lowamountnumerunit, @lowamountdenomvalue, @lowamountdenomprefix, @lowamountdenomunit, @highamountnumervalue, @highamountnumerprefix, @higamountnumerunit, @highamountdenomvalue, @highamountdenomprefix, @highamountdenomunit, @pp_FK, @userID, @ExpressedBy_FK, @concise)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
