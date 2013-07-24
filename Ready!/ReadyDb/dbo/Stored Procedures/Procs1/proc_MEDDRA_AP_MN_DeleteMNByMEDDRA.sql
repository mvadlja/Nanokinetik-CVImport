-- Delete
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_DeleteMNByMEDDRA]
	@meddra_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	--DECLARE @toDelete table ( id int );
	--INSERT INTO @toDelete 
	--	SELECT dbo.MEDDRA.meddra_pk
	--	FROM dbo.MEDDRA
	--	JOIN dbo.MEDDRA_AP_MN ON (dbo.MEDDRA.meddra_pk=dbo.MEDDRA_AP_MN.meddra_FK AND dbo.MEDDRA_AP_MN.meddra_FK=@ap_FK);

	DELETE FROM dbo.MEDDRA_AP_MN WHERE meddra_FK = @meddra_FK;
END
