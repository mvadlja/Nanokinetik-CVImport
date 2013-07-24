-- GetEntity
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_GetEntity]
	@approved_substance_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_substance_PK], [operationtype], [virtual], [localnumber], [ev_code], [sourcecode], [resolutionmode], [substancename], [casnumber], [molecularformula], [class], [cbd], [substancetranslations_FK], [substancealiases_FK], [internationalcodes_FK], [previous_ev_codes_FK], [substancessis_FK], [substance_attachment_FK], [comments]
	FROM [dbo].[APPROVED_SUBSTANCE]
	WHERE [approved_substance_PK] = @approved_substance_PK
END
