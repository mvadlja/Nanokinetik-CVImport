-- Save
CREATE PROCEDURE  [dbo].[proc_PP_ACTIVE_INGREDIENT_Save]
	@activeingredient_PK int = NULL,
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
	@highamountnumerunit nvarchar(70) = NULL,
	@highamountdenomvalue decimal(18,5) = NULL,
	@highamountdenomprefix nvarchar(12) = NULL,
	@highamountdenomunit nvarchar(70) = NULL,
	@pp_FK int = NULL,
	@userID int = NULL,
	@strength_value int = NULL,
	@strength_unit nvarchar(100) = NULL,
	@ExpressedBy_FK int = NULL,
	@concise nvarchar(150) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[PP_ACTIVE_INGREDIENT]
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
	[highamountnumerunit] = @highamountnumerunit,
	[highamountdenomvalue] = @highamountdenomvalue,
	[highamountdenomprefix] = @highamountdenomprefix,
	[highamountdenomunit] = @highamountdenomunit,
	[pp_FK] = @pp_FK,
	[userID] = @userID,
	[strength_value] = @strength_value,
	[strength_unit] = @strength_unit,
	[ExpressedBy_FK] = @ExpressedBy_FK,
	[concise] = @concise
	WHERE [activeingredient_PK] = @activeingredient_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[PP_ACTIVE_INGREDIENT]
		([substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [highamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [strength_value], [strength_unit], [ExpressedBy_FK], [concise])
		VALUES
		(@substancecode_FK, @resolutionmode, @concentrationtypecode, @lowamountnumervalue, @lowamountnumerprefix, @lowamountnumerunit, @lowamountdenomvalue, @lowamountdenomprefix, @lowamountdenomunit, @highamountnumervalue, @highamountnumerprefix, @highamountnumerunit, @highamountdenomvalue, @highamountdenomprefix, @highamountdenomunit, @pp_FK, @userID, @strength_value, @strength_unit, @ExpressedBy_FK, @concise)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
