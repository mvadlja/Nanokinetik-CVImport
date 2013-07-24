-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_EXCIPIENT_GetEntity]
	@excipient_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[excipient_PK], [substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [higamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [ExpressedBy_FK], [concise]
	FROM [dbo].[PP_EXCIPIENT]
	WHERE [excipient_PK] = @excipient_PK
END
