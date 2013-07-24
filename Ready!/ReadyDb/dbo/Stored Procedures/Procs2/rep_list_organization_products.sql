CREATE PROCEDURE  [dbo].[rep_list_organization_products]

	@organization_pk int

AS

SELECT * FROM (
SELECT -1 AS product_PK, 'All' as name UNION ALL
SELECT
	pr.product_PK, pr.name
	from
	dbo.product pr 
	WHERE pr.client_organization_FK = @organization_pk OR @organization_pk = -1
)x
ORDER BY CASE WHEN product_PK = -1 THEN 'AAA' ELSE name end
