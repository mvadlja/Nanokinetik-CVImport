
-- GetEntity
CREATE PROCEDURE [dbo].[proc_SR_RI_MN_GetEntity]
	@sr_ri_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[sr_ri_mn_PK], [ri_FK], [sr_FK]
	FROM [dbo].[SR_RI_MN]
	WHERE [sr_ri_mn_PK] = @sr_ri_mn_PK
END
