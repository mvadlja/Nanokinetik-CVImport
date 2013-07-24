CREATE PROCEDURE  [dbo].[rep_03_regulatory_activity_OLD] 

	
as

select distinct p.name as product, 
       s.substance_name,
       a.name as activity, 
       a.procedure_number, 
       atype.name as activity_type, 
       amode.name AS activity_mode, 
       (  SELECT
			   ( SELECT CAST (country.abbreviation AS nvarchar) + ','
				   FROM ACTIVITY_COUNTRY_MN c2
				   left join COUNTRY country on c2.country_FK = country.country_PK
				  WHERE c2.activity_FK = a2.activity_PK          
					FOR XML PATH('') ) 
		  FROM ACTIVITY a2 
		  left outer join ACTIVITY_COUNTRY_MN c1 on a2.activity_PK = c1.activity_FK
		  where a.activity_PK = a2.activity_PK
		  GROUP BY a2.activity_PK ) as countries,				  
       regSt.name as regulatory_status, 
       a.submission_date, 
       a.approval_date 
from PRODUCT p
    left join dbo.ACTIVITY_PRODUCT_MN ap_mn on p.product_PK = ap_mn.product_FK
    left join dbo.ACTIVITY a on ap_mn.activity_FK = a.activity_PK 
    left join dbo.[TYPE] regSt on regSt.type_PK = a.regulatory_status_FK 
    left join dbo.ACTIVITY_TYPE_MN atmn on atmn.activity_FK = a.activity_PK
    left join dbo.[TYPE] atype on atype.type_PK = atmn.type_FK 
    left join dbo.[TYPE] amode on amode.type_PK = a.mode_FK
    left join dbo.PRODUCT_PP_MN ppmn on ppmn.product_FK = p.product_PK
    left join dbo.PHARMACEUTICAL_PRODUCT pp on pp.pharmaceutical_product_PK = ppmn.pp_FK
    left join dbo.PP_ACTIVE_INGREDIENT ai on pp.pharmaceutical_product_PK = ai.pp_FK
    left join substances s on ai.substancecode_FK = s.substance_PK
order by product, substance_name
    
-- select distinct pr.name, s.substance_name from   
--        dbo.PP_ACTIVE_INGREDIENT ai
--		left join substances s on ai.substancecode_FK = s.substance_PK
--		left join dbo.PHARMACEUTICAL_PRODUCT pp on pp.pharmaceutical_product_PK = ai.pp_FK				
--		left join dbo.PRODUCT_PP_MN ppmn on pp.pharmaceutical_product_PK = ppmn.pp_FK
--		left join product pr on ppmn.product_FK = pr.product_PK



--select distinct p.name as product, 
--       s.substance_name
--from PRODUCT p    
--    left join dbo.PRODUCT_PP_MN ppmn on ppmn.product_FK = p.product_PK
--    left join dbo.PHARMACEUTICAL_PRODUCT pp on pp.pharmaceutical_product_PK = ppmn.pp_FK
--    left join dbo.PP_ACTIVE_INGREDIENT ai on pp.pharmaceutical_product_PK = ai.pp_FK
--    left join substances s on ai.substancecode_FK = s.substance_PK
