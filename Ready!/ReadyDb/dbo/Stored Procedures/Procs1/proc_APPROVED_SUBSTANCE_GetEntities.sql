-- GetEntities
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBSTANCE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[approved_substance_PK], [operationtype], [virtual], [localnumber], [ev_code], [sourcecode], [resolutionmode], [substancename], [casnumber], [molecularformula], [class], [cbd], [substancetranslations_FK], [substancealiases_FK], [internationalcodes_FK], [previous_ev_codes_FK], [substancessis_FK], [substance_attachment_FK], [comments]
	FROM [dbo].[APPROVED_SUBSTANCE]
END
