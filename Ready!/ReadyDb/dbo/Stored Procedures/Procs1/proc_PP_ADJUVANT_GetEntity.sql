-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PP_ADJUVANT_GetEntity]
	@adjuvant_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[adjuvant_PK], [substancecode_FK], [resolutionmode], [concentrationtypecode], [lowamountnumervalue], [lowamountnumerprefix], [lowamountnumerunit], [lowamountdenomvalue], [lowamountdenomprefix], [lowamountdenomunit], [highamountnumervalue], [highamountnumerprefix], [higamountnumerunit], [highamountdenomvalue], [highamountdenomprefix], [highamountdenomunit], [pp_FK], [userID], [ExpressedBy_FK], [concise]
	FROM [dbo].[PP_ADJUVANT]
	WHERE [adjuvant_PK] = @adjuvant_PK
END
