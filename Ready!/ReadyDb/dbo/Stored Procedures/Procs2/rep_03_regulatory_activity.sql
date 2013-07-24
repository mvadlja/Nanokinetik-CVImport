CREATE PROCEDURE [dbo].[rep_03_regulatory_activity]
    @client int = -1,
	@product_PK int = -1,
	@country NVARCHAR(10) = 'All',
	@regulatoryStatus int = -1,
	@type NVARCHAR(100) = 'All',
	@substance int = -1,
	@submission_date_from date,
	@submission_date_to date,
	@approval_date_from date,
	@approval_date_to date,
	@DatesAndCondition bit
	
AS

DECLARE 
	@client_ int = @client,
	@product_ int = @product_PK,
	@country_ VARCHAR(10) = @country,
	@regulatoryStatus_ int = @regulatoryStatus,
	@type_ VARCHAR(100) = @type,
	@substance_ int = @substance,
	@submission_date_from_ date = @submission_date_from,
	@submission_date_to_  date = @submission_date_to,
	@approval_date_from_  date = @approval_date_from,
	@approval_date_to_  date = @approval_date_to,	
	@AppURL NVARCHAR(50) = 'http://pantera:8084'


--DROP TABLE dbo.TMP_GD_PARAM;


--SELECT 
--@client AS client,
--	@product product,
--	@country country,
--	@regulatoryStatus regulatoryStatus,
--	@type  TYPE ,
--	@substance substance ,
--	@submission_date_from date_from,
--	@submission_date_to  date_to,
--	@approval_date_from app_date_from,
--	@approval_date_to  app_date_to
--INTO dbo.TMP_GD_PARAM
--;

IF object_id('tempdb..#products') IS NOT NULL DROP TABLE #products


	
	select 
         o.organization_PK
         ,  o.name_org as Client
         
         ,(
			SELECT Stuff((
			SELECT
				N'/'+ isnull(s.substance_name,'N/A') AS [text()]
			FROM
			    dbo.PP_ACTIVE_INGREDIENT ai
				inner join substances s 
					on ai.substancecode_FK = s.substance_PK
				INNER join dbo.PHARMACEUTICAL_PRODUCT pp
					on pp.pharmaceutical_product_PK = ai.pp_FK	 		
				INNER join dbo.PRODUCT_PP_MN ppmn 
					on pp.pharmaceutical_product_PK = ppmn.pp_FK 
			    WHERE
			    	ppmn.product_FK = pr.product_PK
			    	
			    	AND ( s.substance_PK = @substance_ OR @substance_ = -1 )
			    	
			    	
	         FOR XML PATH(''),TYPE
					).value('text()[1]','nvarchar(max)'),1,1,N'')
			
		 ) AS Substance
         , pr.name as SubstanceProductsLabel
         ,'<a href="javascript:void(window.open(''' + @AppURL + '/Views/ProductView/Preview.aspx?EntityContext=Product&idProd=' + convert(varchar,product_pk) + ''',''_blank''))">'+ pr.name +'</a>' AS SubstanceProducts
	     , pr.product_PK
	INTO #products
	from
		dbo.organization o
		INNER JOIN dbo.product pr on pr.client_organization_FK = o.organization_PK
	WHERE
		1=1	
		AND ( o.organization_PK = @client_ or @client_  = -1 )
		AND ( pr.product_PK = @product_ OR @product_ = -1 )
 


--SELECT * FROM products


IF object_id('tempdb..#activities') IS NOT NULL DROP TABLE #activities

	SELECT
	*
	INTO #activities
	FROM
	(
	SELECT
		pr.product_PK,
		
		amn.activity_FK 
	     , '<a href="javascript:void(window.open(''' + @AppURL + '/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct='+ CAST(amn.activity_FK as varchar)+''',''_blank''))"><div>'+ act.name +'</div></a><br/>' as activity_name
		 , act.procedure_number
		 , mod.name AS activity_mode
		 , reg.name AS regulatory_status
		 , reg.type_PK AS regulatory_status_FK
		 , [dbo].[ReturnCountriesOfActivityAbbrevated](activity_PK) as activity_countries
	     ,(
			SELECT Stuff((
			SELECT
				N','+ acttype.name AS [text()]
			FROM
				dbo.ACTIVITY_TYPE_MN typ
				LEFT JOIN dbo.TYPE acttype ON acttype.type_PK = typ.type_FK
			WHERE
				typ.activity_FK = act.activity_PK
			FOR XML PATH(''),TYPE
				).value('text()[1]','nvarchar(max)'),1,1,N'')
			
		 ) AS activity_type
	     , act.submission_date
	     , act.approval_date
	FROM
		#products pr
			LEFT JOIN dbo.PRODUCT_PP_MN ppmn ON ppmn.product_FK = pr.product_PK
			left join dbo.ACTIVITY_PRODUCT_MN amn on amn.product_FK = pr.product_PK
			lEFT JOIN dbo.ACTIVITY act ON act.activity_PK = amn.activity_FK 
		   					
	   		LEFT JOIN dbo.TYPE mod 
	   			ON mod.type_PK = act.mode_FK
	   		LEFT JOIN dbo.TYPE reg 
	   			ON reg.type_PK = act.regulatory_status_FK		  
	   					
	  )x
	  
	  --FILTERI !
	  WHERE
-- Mario, 20130102 - Promjenjena logika za datume	 AND > OR

		 		(  
		 			(
		 				'true' != @DatesAndCondition AND
						(((submission_date >= @submission_date_from_ OR @submission_date_from_ IS NULL) AND (submission_date <= @submission_date_to_ OR @submission_date_to_ IS NULL))
							OR ((approval_date >= @approval_date_from_ OR @approval_date_from_ IS NULL) AND (approval_date <= @approval_date_to_ OR @approval_date_to_ IS NULL)))
					)
					
					OR 

					(
						'true' = @DatesAndCondition AND
						(((submission_date >= @submission_date_from_ OR @submission_date_from_ IS NULL) AND (submission_date <= @submission_date_to_ OR @submission_date_to_ IS NULL))
							AND ((approval_date >= @approval_date_from_ OR @approval_date_from_ IS NULL) AND (approval_date <= @approval_date_to_ OR @approval_date_to_ IS NULL)))
					)
				)

		 	--	(((submission_date 	>= @submission_date_from_ OR @submission_date_from_ IS NULL )
		  -- 	AND ( submission_date 	<= @submission_date_to_ OR @submission_date_to_ IS NULL ))
		   	
		  -- 	OR 
				--(( approval_date 	>= @approval_date_from_ OR @approval_date_from_ IS NULL )
		  -- 	AND ( approval_date 	<= @approval_date_to_ OR @approval_date_to_ IS NULL )))
		   	
		 	and	( activity_countries LIKE '%' + @country_ + '%' OR @country_ = 'All' )
		 	
		 	AND (@regulatoryStatus_ = -1 OR regulatory_status_FK = @regulatoryStatus_)
		 	
		 	AND (activity_type LIKE '%' + @type_ + '%' OR @type_ = 'All')

--SELECT * FROM #activities



SELECT
*
FROM
	(
	SELECT
			CASE 
				WHEN
				
				--CASE WHEN activity_mode 		= 'N/A' THEN 1 ELSE 0 END +
				CASE WHEN isnull(regulatory_status,'N/A') 	= 'N/A' THEN 2 ELSE 0 END +
				CASE WHEN isnull(activity_type,'N/A') 		= 'N/A' THEN 2 ELSE 0 END 
					> 1 THEN 0 
				ELSE 1 
			END is_valid_row 
		, MAX(CASE 
				WHEN
				
				--CASE WHEN activity_mode 		= 'N/A' THEN 1 ELSE 0 END +
				CASE WHEN isnull(regulatory_status,'N/A') 	= 'N/A' THEN 2 ELSE 0 END +
				CASE WHEN isnull(activity_type,'N/A') 		= 'N/A' THEN 2 ELSE 0 END 
					> 1 THEN 0 
				ELSE 1 
			END) OVER (PARTITION BY pr.organization_PK, pr.product_PK) is_valid_row_group
		,pr.*
		,a.activity_FK
		,a.activity_name
		 , a.procedure_number
		 , a.activity_mode
		 , a.regulatory_status
		 , a.regulatory_status_FK
		 , a.activity_countries
	     ,a.activity_type
	     ,a.submission_date AS submission_date
	     ,a.approval_date AS approval_date
		, row_number() OVER (PARTITION BY pr.organization_PK, pr.product_PK ORDER BY  a.activity_FK) AS rown
	FROM
		#products pr
		LEFT JOIN #activities a ON a.product_PK = pr.product_PK	
	WHERE
		SUBSTANCE IS NOT null
)x


WHERE 
	is_valid_row_group = 1 AND x.is_valid_row = 1 OR  ( is_valid_row_group = 0  AND rown = 1 )
	
	
	
IF object_id('tempdb..#activities') IS NOT NULL DROP TABLE #activities
IF object_id('tempdb..#products') IS NOT NULL DROP TABLE #products
