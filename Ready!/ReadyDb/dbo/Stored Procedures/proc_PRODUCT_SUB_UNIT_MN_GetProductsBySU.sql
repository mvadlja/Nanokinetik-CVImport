-- GetProductsBySU
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_GetProductsBySU]
	@submission_unit_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.product_submission_unit_PK, mn.product_FK, mn.submission_unit_FK
	FROM [dbo].[PRODUCT_SUB_UNIT_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].SUBMISSION_UNIT su ON su.subbmission_unit_PK = mn.submission_unit_FK
	WHERE (mn.submission_unit_FK = @submission_unit_FK OR @submission_unit_FK IS NULL)

END