
-- Delete
CREATE PROCEDURE [dbo].[STRUCT_REPRES_ATTACHMENT_DeleteNULLByUserID]
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].STRUCT_REPRES_ATTACHMENT
	WHERE [dbo].STRUCT_REPRES_ATTACHMENT.[userID] = @UserID
	AND struct_repres_attach_PK in (	
		select struct_repres_attach_PK from SSI.dbo.STRUCT_REPRES_ATTACHMENT
		left join SSI.dbo.STRUCTURE on struct_repres_attach_FK = struct_repres_attach_PK 
		where struct_repres_attach_FK is null);
END
