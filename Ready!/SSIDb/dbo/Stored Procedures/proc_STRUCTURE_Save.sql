
-- Save
CREATE PROCEDURE [dbo].[proc_STRUCTURE_Save]
	@structure_PK int = NULL,
	@struct_repres_type_FK varchar(250) = NULL, --add _FK
	@struct_representation varchar(4000) = NULL,
	@struct_repres_attach_FK int = NULL,
	@stereochemistry_FK varchar(250) = NULL, --added _FK
	@optical_activity varchar(250) = NULL,
	@molecular_formula varchar(2000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[STRUCTURE]
	SET
	[struct_repres_type_FK] = @struct_repres_type_FK, -- change from struct_repres_type to struct_repres_type_FK
	[struct_representation] = @struct_representation,
	[struct_repres_attach_FK] = @struct_repres_attach_FK,
	[stereochemistry_FK] = @stereochemistry_FK, --Promjena s stereochemistry na stereochemistry_FK
	[optical_activity] = @optical_activity,
	[molecular_formula] = @molecular_formula
	WHERE [structure_PK] = @structure_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[STRUCTURE]
		([struct_repres_type_FK], [struct_representation], [struct_repres_attach_FK], [stereochemistry_FK], [optical_activity], [molecular_formula])
		VALUES
		(@struct_repres_type_FK, @struct_representation, @struct_repres_attach_FK, @stereochemistry_FK, @optical_activity, @molecular_formula)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
