
-- Delete
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRESENTATION_TYPE_Delete]
	@struct_repres_type_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[STRUCT_REPRESENTATION_TYPE] WHERE [struct_repres_type_PK] = @struct_repres_type_PK
END
