CREATE PROCEDURE  [dbo].[rep_list_organization]
AS

SELECT * FROM (
SELECT -1 AS organization_pk, '-- Choose --' as name_org UNION ALL
SELECT
	organization_pk, name_org FROM ORGANIZATION
	where    organization_PK 
	--zbog lose scheme stare baze nekih ima duplo NE PITAJ...
	not in ( 125,126,127,128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149
,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,176,177
,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193 )
)x
ORDER BY CASE WHEN organization_pk = -1 THEN 'AAA' ELSE name_org end
