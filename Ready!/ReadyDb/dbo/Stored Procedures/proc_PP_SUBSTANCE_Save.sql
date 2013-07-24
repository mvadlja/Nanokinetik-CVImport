
-- Save
CREATE PROCEDURE [dbo].[proc_PP_SUBSTANCE_Save]
	@ppsubstance_PK int = NULL,
	@ppsubstance_FK int = NULL,
	@substancecode_FK int = NULL,
	@concentrationtypecode int = NULL,
	@lowamountnumervalue decimal(18,5) = NULL,
	@lowamountnumerprefix nvarchar(50) = NULL,
	@lowamountnumerunit nvarchar(70) = NULL,
	@lowamountdenomvalue decimal(18,5) = NULL,
	@lowamountdenomprefix nvarchar(50) = NULL,
	@lowamountdenomunit nvarchar(70) = NULL,
	@highamountnumervalue decimal(18,5) = NULL,
	@highamountnumerprefix nvarchar(50) = NULL,
	@highamountnumerunit nvarchar(70) = NULL,
	@highamountdenomvalue decimal(18,5) = NULL,
	@highamountdenomprefix nvarchar(50) = NULL,
	@highamountdenomunit nvarchar(70) = NULL,
	@pp_FK int = NULL,
	@expressedby_FK int = NULL,
	@concise nvarchar(150) = NULL,
	@substancetype nvarchar(50) = NULL,
	@user_FK int = NULL,
	@sessionid nvarchar(50) = NULL,
	@modifieddate datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_SUBSTANCE]
	SET
	[ppsubstance_FK] = @ppsubstance_FK,
	[substancecode_FK] = @substancecode_FK,
	[concentrationtypecode] = @concentrationtypecode,
	[lowamountnumervalue] = @lowamountnumervalue,
	[lowamountnumerprefix] = @lowamountnumerprefix,
	[lowamountnumerunit] = @lowamountnumerunit,
	[lowamountdenomvalue] = @lowamountdenomvalue,
	[lowamountdenomprefix] = @lowamountdenomprefix,
	[lowamountdenomunit] = @lowamountdenomunit,
	[highamountnumervalue] = @highamountnumervalue,
	[highamountnumerprefix] = @highamountnumerprefix,
	[highamountnumerunit] = @highamountnumerunit,
	[highamountdenomvalue] = @highamountdenomvalue,
	[highamountdenomprefix] = @highamountdenomprefix,
	[highamountdenomunit] = @highamountdenomunit,
	[pp_FK] = @pp_FK,
	[expressedby_FK] = @expressedby_FK,
	[concise] = @concise,
	[substancetype] = @substancetype,
	[user_FK] = @user_FK,
	[sessionid] = @sessionid,
	[modifieddate] = @modifieddate
	WHERE [ppsubstance_PK] = @ppsubstance_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_SUBSTANCE]
		([ppsubstance_FK], [substancecode_FK], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [expressedby_FK], [concise], [substancetype], [user_FK], [sessionid], [modifieddate])
		VALUES
		(@ppsubstance_FK, @substancecode_FK, @concentrationtypecode, @lowamountnumervalue, @lowamountnumerprefix, @lowamountnumerunit, @lowamountdenomvalue, @lowamountdenomprefix, @lowamountdenomunit, @highamountnumervalue, @highamountnumerprefix, @highamountnumerunit, @highamountdenomvalue, @highamountdenomprefix, @highamountdenomunit, @pp_FK, @expressedby_FK, @concise, @substancetype, @user_FK, @sessionid, @modifieddate)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END