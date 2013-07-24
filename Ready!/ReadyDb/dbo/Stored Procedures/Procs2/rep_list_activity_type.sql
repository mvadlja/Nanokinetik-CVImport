CREATE PROCEDURE  [dbo].[rep_list_activity_type]

AS

SELECT * FROM (
SELECT 'All' AS type, 'All' as name UNION ALL
SELECT
				DISTINCT LEFT(acttype.name,30) AS type,  LEFT(acttype.name,30) AS name
			FROM
				dbo.ACTIVITY_TYPE_MN typ
				inner JOIN dbo.TYPE acttype ON acttype.type_PK = typ.type_FK
			WHERE acttype.[group] = 'A'
	
)pr
ORDER BY CASE WHEN type = 'All' THEN 'AAA' ELSE name end
