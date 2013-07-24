-- GetSUByProduct
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_GetSUByProduct]
	@product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT mn.submission_unit_FK
	FROM [dbo].[PRODUCT_SUB_UNIT_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].SUBMISSION_UNIT su ON su.subbmission_unit_PK = mn.submission_unit_FK
	WHERE (mn.product_FK = @product_FK OR @product_FK IS NULL) AND su.ectd_FK IS NOT NULL

END
