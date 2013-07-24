CREATE PROCEDURE  [dbo].[rep_list_regulatory_status]

AS

SELECT * FROM (
SELECT -1 AS type, 'All' as name UNION ALL
SELECT 
	type_pk AS type, name
	FROM 
	dbo.TYPE 
	WHERE 
		type_PK IN (
		
		SELECT
			DISTINCT regulatory_status_FK
		FROM
			dbo.ACTIVITY 
		WHERE 
			regulatory_status_FK IS NOT null
		)
	
)pr
ORDER BY CASE WHEN type = -1 THEN 'AAA' ELSE name end
