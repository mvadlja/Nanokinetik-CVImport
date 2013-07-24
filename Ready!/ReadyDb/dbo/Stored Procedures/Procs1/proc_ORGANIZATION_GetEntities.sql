-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;
--novi upit filtrira samo client
SELECT * FROM (
SELECT 
	-1 AS organization_pk, 
	'- Choose -' as name_org 
UNION ALL
SELECT  
	  organization_pk
	, name_org 
FROM 
	ORGANIZATION o

inner join organization_in_role oir
on oir.organization_FK = o.organization_PK
where role_org_FK = 10

--ako �e biti problema sa reportima vratiti ovaj ispod upit
--SELECT * FROM (
--SELECT -1 AS organization_pk, '- Choose -' as name_org UNION ALL
--SELECT
--	organization_pk, name_org FROM ORGANIZATION
--	where    organization_PK 
--	--zbog lose scheme stare baze nekih ima duplo ti su nabrojani u not in uvjetu zadnje dodano agencije...
--	not in 
--	( 23,24,25,26,27,28,29,30,31,32,33,34,35
--	  ,36,37,38,39,40,41,42,43,44,45,46,47,48
--	  ,49,50,51,52,53,54,55,56,57,58,59,60,61,62
--	  ,63,64,65,66,67,68, 127,128,129,130,131,132
--	  ,133,134,135,136,137,138,139,140,141,142,143,144,145,146,147,148,149
--	,150,151,152,153,154,155,156,157,158,159,160,161,162,163,164,165,166,167,168
--	,169,170,171,172,173,174,175,176,177
--	,178,179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194, 201, 203, 82 )
)x
ORDER BY CASE WHEN organization_pk = -1 THEN 'AAA' ELSE name_org end
--dragan: zamjeniti upit ne�to kao sa ovim dok se stablilizira verzija
--if object_id ('tempdb..#cte_org') is not null drop table #cte_org
--select * 
--into #cte_org
--from organization o
--inner join organization_in_role oir
--on o.organization_PK = oir.organization_FK
--where oir.role_org_fk in (6,8,10) --2,3,,54,
--select DISTINCT name_org from #cte_org 
--order by name_org

END
