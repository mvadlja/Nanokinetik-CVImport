CREATE PROCEDURE [dbo].[rep_02_customer_time]
	@client int,
	@person INT,
	@date_from date,
	@date_to DATE,
	@show_auth_numbers bit,
	@project_PK INT,
	@calculateBillingRate bit
	
as
DECLARE @AppURL NVARCHAR(50) = 'http://office.possimusit.com:8085';

if object_id ('tempdb..#cte_substances_complete') is not null drop table #cte_substances_complete


	select 
         o.organization_PK
         , o.name_org as Client        
         , isnull(s.substance_name,'N/A') as Substance
         , s.substance_PK
         , pr.name as SubstanceProducts
	     , pr.product_PK
	     
         , s2.substance_PK AS subs2_pk
         , isnull(s2.substance_name,'N/A') as Substance2
         , pr2.name as SubstanceProducts2
	     , pr2.product_PK AS product2_pk
	      
	     , amn.activity_FK 
	     ,act.name AS activity
	 	, [dbo].[ReturnCountriesOfActivityAbbrevated](activity_PK) as ActivityCountries
	 	--time uniti
	 	, tu.time_unit_PK
	    , tum.time_unit_name
	    , tum.time_unit_name_PK	    
	    , tu.actual_date
	    
	    , tu.[description]
	    , tu.comment
		, isnull(tu.time_hours*60,0) + isnull(tu.time_minutes,0) as time_min
		, p.name AS responsible_person
	     
	     
	   into #cte_substances_complete
	from 
		dbo.PP_ACTIVE_INGREDIENT ai
		left join substances s 
		on ai.substancecode_FK = s.substance_PK
		left join dbo.PHARMACEUTICAL_PRODUCT pp
		on pp.pharmaceutical_product_PK = ai.pp_FK		
		
		left join dbo.PRODUCT_PP_MN ppmn 
		on pp.pharmaceutical_product_PK = ppmn.pp_FK
		left join product pr 
		on ppmn.product_FK = pr.product_PK
		inner join organization o 
		on pr.client_organization_FK = o.organization_PK
		inner join dbo.ACTIVITY_PRODUCT_MN amn
   			on amn.product_FK = pr.product_PK
   		INNER JOIN dbo.ACTIVITY act ON act.activity_PK = amn.activity_FK
   		INNER JOIN dbo.ACTIVITY_PRODUCT_MN amn2 ON amn2.activity_FK = act.activity_PK
   		INNER join product pr2 on pr2.product_PK = amn2.product_FK
   		INNER JOIN dbo.PRODUCT_PP_MN ppmn2 ON ppmn2.product_FK = pr2.product_PK
   		INNER JOIN dbo.PP_ACTIVE_INGREDIENT ai2 ON ai2.pp_FK = ppmn2.pp_FK
   		left join substances s2 on ai2.substancecode_FK = s2.substance_PK
   		INNER join time_unit tu	on amn.activity_FK  = tu.activity_FK AND tu.actual_date BETWEEN @date_from AND @date_to   		
		left join dbo.TIME_UNIT_NAME tum on tu.time_unit_name_FK = tum.time_unit_name_PK
   		left join person p on tu.user_FK = p.person_PK   		
   		LEFT JOIN ACTIVITY_PROJECT_MN activityProject_mn on act.activity_PK = activityProject_mn.activity_FK
   		LEFT JOIN PROJECT project ON project.project_PK = activityProject_mn.project_FK
WHERE
	1=1
	AND (o.organization_PK = @client OR @client = -1)
	AND (p.person_PK = @person or @person = -1)
	AND (project.project_PK = @project_PK OR @project_PK = -1)
GROUP BY
	o.organization_PK
     ,  o.name_org        
     , isnull(s.substance_name,'N/A')
     , s.substance_PK
     , pr.name 
     , pr.product_PK
     , [dbo].[ReturnCountriesOfActivityAbbrevated](activity_PK)
     , s2.substance_PK 
     , isnull(s2.substance_name,'N/A') 
     , pr2.name
     , pr2.product_PK 
      
     , amn.activity_FK 
     ,act.name 
	 
	 	--time uniti
	 	, time_unit_PK
	    , tum.time_unit_name
	    , tum.time_unit_name_PK	  
	    , tu.actual_date
	    , tu.comment
	    , tu.[description]
		, isnull(tu.time_hours*60,0) + isnull(tu.time_minutes,0)
		, p.name
		, project.project_PK
--order by case when s2.substance_PK = 685 then 'AAA' else s2.substance_PK end


CREATE NONCLUSTERED INDEX IX_cte_substances_complete
ON [dbo].#cte_substances_complete(organization_PK, product2_pk, activity_FK)

--select * from #cte_substances_complete where Client like 'BMS%' and activity_FK = 256


if object_id ('tempdb..#tmp2') is not null drop table #tmp2

SELECT
	--stvaramo umjetni pk da stvorimo index po koloni (brzi join za sljedece tablice)
	dense_rank() OVER (PARTITION BY x.organization_PK ORDER BY substance_final) AS substance_final_pk,
	max (CASE WHEN product_pk = product2_pk THEN 1 ELSE 0 end) OVER (PARTITION BY x.organization_PK, x.substance_final, x.activity_FK, x.product_PK) AS is_same_substance ,
	row_number() OVER (PARTITION BY x.organization_PK, x.substance_final, x.activity_FK, x.product_PK ORDER BY product2_pk) AS product_rownum ,			

	x.*
	
	INTO #tmp2
FROM
(
	SELECT 
		organization_PK,
		Client,	
		
--		SUBSTANCE,
--		Substance2,
		
		
		
		-- kombinacije SUBSTANCI ( A + B,  A,  C, ...)
		(
			SELECT Stuff((
			SELECT
				' +<br />' + Substance2 AS [text()]
			FROM
				(
					SELECT 
						DISTINCT Substance2 
					FROM 
						#cte_substances_complete s2
					WHERE
						s2.organization_PK 		= s.organization_PK AND
					   	--s2.substance_pk 		= s.substance_pk AND
					   	s2.product2_pk			= s.product2_pk AND
					   	s2.activity_FK			= s.activity_FK
			   			
			   ) s2
				
			FOR XML PATH(''),TYPE
				).value('text()[1]','nvarchar(max)'),1,8,N'')
					
		) AS substance_final,		
		
		product2_pk,
		SubstanceProducts2,
		product_pk,
		s.activity,
		activity_FK,
		'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/APropertiesView.aspx?f=l&idAct='+ CAST(activity_FK as varchar)+''',''_blank''))">'+ Activity +'</a><br/>' as ActivityLink,
		
		ActivityCountries,
		
		SubstanceProducts,
		time_unit_PK,
		time_unit_name,
		time_unit_name_PK,
		comment,
		s.actual_date,
		s.time_min,
		s.description,
		s.responsible_person
	FROM 
		(
			SELECT
				distinct
				s.organization_PK,
				s.Client,
				s.substance_pk,
				s.product2_pk,
				
--				s.Substance,
--				s.Substance2,
				
				
				s.SubstanceProducts2,
				
				s.SubstanceProducts,
				s.product_pk,
				s.activity_FK,
				s.Activity,
				s.ActivityCountries,
				
				--detalji aktivnosti
				s.time_unit_PK,
				s.description,
				comment,
				s.time_unit_name,
				s.time_unit_name_PK,
				s.actual_date,
				s.time_min,
				s.responsible_person
			FROM
				#cte_substances_complete s
		)s
--	WHERE 
--		Client LIKE 'ScanVet%'
)x





CREATE NONCLUSTERED INDEX IX_cte_substances_complete_final
ON [dbo].#tmp2(organization_PK, substance_final_pk, activity_FK)

-- Temporary table for billing rate calculation
if object_id ('tempdb..#timeUnitsForBillingRateCalculation') is not null drop table #timeUnitsForBillingRateCalculation

SELECT DISTINCT time_unit_PK, time_min, activity_FK, time_unit_name_PK INTO #timeUnitsForBillingRateCalculation FROM #tmp2

DECLARE 
	@billableWorkTime int = 0, 	
	@allWorkTime int = 0,
	@nonBillableWorkTime int = 0,
	@billingRate decimal(18, 5) = NULL,
	@billingRateFormatted nvarchar(MAX) = NULL,
	@nominator int = 0,
	@denominator int = 0;
	
	IF @calculateBillingRate = '1' 
	BEGIN
		SET @billableWorkTime = 
				(SELECT 
					SUM(tu.time_min) 
				FROM #timeUnitsForBillingRateCalculation tu
				LEFT JOIN ACTIVITY activity on activity.activity_PK = tu.activity_FK
				LEFT JOIN TIME_UNIT_NAME timeUnitName on timeUnitName.time_unit_name_PK = tu.time_unit_name_PK
				WHERE timeUnitName.billable = '1' AND activity.billable = '1')
		SET @nonBillableWorkTime = 
				(SELECT 
					SUM(tu.time_min) 
				FROM #timeUnitsForBillingRateCalculation tu
				LEFT JOIN TIME_UNIT_NAME timeUnitName on timeUnitName.time_unit_name_PK = tu.time_unit_name_PK
				WHERE timeUnitName.billable = '0')
		SET @allWorkTime = 
				(SELECT 
					SUM(tu.time_min) 
				FROM #timeUnitsForBillingRateCalculation tu)
		
		IF @billableWorkTime IS NULL SET @billableWorkTime = 0;
	    IF @nonBillableWorkTime IS NULL SET @nonBillableWorkTime = 0;
		IF @allWorkTime IS NULL SET @allWorkTime = 0;
		
		SET @nominator = @billableWorkTime;
		SET @denominator = @allWorkTime - @nonBillableWorkTime;

		IF @denominator != '0'
		BEGIN
			SET @billingRate = (CAST(@nominator as decimal(18, 5))) / @denominator;
			SET @billingRateFormatted = CAST(CONVERT(DECIMAL(18, 1), ROUND(@billingRate * 100, 1)) as NVARCHAR(MAX))+ '%'; 
		END
     END

SELECT
	--@billableWorkTime as billableWorkTime,
	--@nonBillableWorkTime as nonBillableWorkTime,
    --@allWorkTime as allWorkTime,
    --dbo.ReturnVarcharFromTime(@allWorkTime) as allWorkTimeHours,
	@billingRate as billingRate,
	@billingRateFormatted as billingRateFormatted,
	dbo.ReturnVarcharFromTime(activity_calc_time) AS label_activity_calc_time,
	dbo.ReturnVarcharFromTime(SUM(CASE WHEN activity_rn = 1 THEN activity_calc_time ELSE 0 end) OVER (PARTITION BY organization_PK, substance_final_pk) ) AS substance_calc_time,
	dbo.ReturnVarcharFromTime(SUM(CASE WHEN activity_rn = 1 THEN activity_calc_time ELSE 0 end) OVER (PARTITION BY organization_PK) ) AS client_calc_time,
	dbo.ReturnVarcharFromTime(SUM(CASE WHEN activity_rn = 1 THEN activity_calc_time ELSE 0 end) OVER (PARTITION BY 1) ) AS total_calc_time,
	CASE WHEN ProductLink LIKE '%<i>%' THEN '( ! )' else '' END AS usklicnik,
	x.*,
	row_number() OVER (PARTITION BY organization_PK ORDER BY
														CASE 
															WHEN substance_final = 'N/A' THEN 'aaaaaaa'
															ELSE substance_final
														END 
														) AS order_num
	FROM
	(
	SELECT
		x.*,
		ROUND(
			SUM(time_min) OVER (PARTITION BY organization_PK, substance_final_pk, activity_FK) 
			* ( 1. * MAX(count_products_same_substance)  OVER (PARTITION BY organization_PK, substance_final_pk, activity_FK) 
				/  MAX(count_total_products)  OVER (PARTITION BY organization_PK, substance_final_pk, activity_FK)
			)
		,0) AS activity_calc_time,
		row_number() OVER (PARTITION BY organization_PK, substance_final_pk, activity_FK ORDER BY time_unit_PK) AS activity_rn
--		row_number() OVER (PARTITION BY organization_PK, substance_final_pk, activity_FK ORDER BY actual_date ASC) AS actual_date_rown
	FROM
		(
		SELECT
			organization_PK, 
			Client,
			substance_final,
			substance_final_pk,
			max(substance_product_final) AS	 substance_product_final,
			max(ProductLink) AS ProductLink,
			
			SUM(count_total_products) AS count_total_products,
			SUM(count_products_same_substance) AS count_products_same_substance,
			activity_FK,
			max(ActivityLink) AS ActivityLink, 
			max(ActivityCountries) AS ActivityCountries,
			time_unit_PK,
			--time_unit_name AS time_unit_name,
			
			'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/TimeUnitProperties.aspx?f=p&idtu=' + convert(varchar,time_unit_PK) + ''',''_blank''))"><div>'+ time_unit_name +'</div></a><br/>' AS time_unit_name,
			CASE WHEN comment = '' THEN NULL ELSE comment END AS comment,
			description AS description,
			actual_date ,
--			actual_date AS actual_date_orig,
			time_min AS time_min,
			dbo.ReturnVarcharFromTime(time_min) AS label_time_min,
			responsible_person AS responsible_person 
		FROM
		(
			SELECT
				organization_PK, 
				Client,
				substance_final,
				substance_final_pk,
				-- SUBSTANCE PRODUCTS, LINKs!
				(
					SELECT Stuff((
					SELECT
						'<br />' + '<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/ProductPKPPropertiesView.aspx?f=l&id=' + convert(varchar,product2_pk) + ''',''_blank''))"><div>'+ SubstanceProducts2 +'</div></a><br/>' AS [text()]
					FROM
						(
							SELECT 
								DISTINCT SubstanceProducts2, product2_pk
							FROM 
								#tmp2 s2
							WHERE
								s2.organization_PK 		= s.organization_PK AND
							   	s2.substance_final_pk	= s.substance_final_pk		   			
					   ) s2
						
					FOR XML PATH(''),TYPE
						).value('text()[1]','nvarchar(max)'),1,6,N'')
							
				) AS substance_product_final,
				
				(
					SELECT Stuff((
					SELECT
						'<br />' + 
							CASE WHEN is_same_substance = 0 THEN '<i>' ELSE '<b>' END +
							'<a href="javascript:void(window.open(''' + @AppURL + '/Views/Business/ProductPKPPropertiesView.aspx?f=l&id=' + convert(varchar,product_pk) + ''',''_blank''))"><div>'+ SubstanceProducts +'</div></a><br/>' +
							CASE WHEN is_same_substance = 0 THEN '</i>' ELSE '</b>' END
							
							AS [text()]
					FROM
						(
							SELECT 
								SubstanceProducts, product_PK, MAX(is_same_substance) AS is_same_substance
							FROM 
								#tmp2 s2
							WHERE
								s2.organization_PK 		= s.organization_PK AND
							   	s2.substance_final_pk	= s.substance_final_pk AND 
							    s2.activity_FK			= s.activity_FK 
							GROUP BY
								SubstanceProducts, product_pk
					   ) s2
						
					FOR XML PATH(''),TYPE
						).value('text()[1]','nvarchar(max)'),1,6,N'')
							
				) AS ProductLink,
				
				--ovo je zapravo count distinct, bitno radi djeljenja vremena, usklicnici  (!)
				CASE WHEN product_rownum = 1 THEN 1 ELSE 0 END AS count_total_products,
				CASE WHEN product_rownum = 1 AND is_same_substance = 1 THEN 1 ELSE 0 END AS count_products_same_substance,
				activity_FK,
				ActivityLink,
				ActivityCountries,
				time_unit_PK,
				time_unit_name,
				--comment,
				
				
				-- SUBSTANCE PRODUCTS, LINKs!
				isnull(
						(
							SELECT Stuff(
								(
									SELECT
										
										', ' + authorisationnumber AS [text()]
									FROM
										(
											
											SELECT
												authp.authorisationnumber
												
											FROM 
												dbo.AUTHORISED_PRODUCT authp
											WHERE
												@show_auth_numbers = 1
												and
												authp.product_fk IN 
												(
													SELECT
														s2.product2_pk
													FROM
														#tmp2 s2
													WHERE
														s2.organization_PK 		= s.organization_PK AND
													   	s2.substance_final_pk	= s.substance_final_pk 
												   		--and	s2.activity_FK			= s.activity_FK 
											   	)
									   ) s2
										
									FOR XML PATH(''),TYPE
								).value('text()[1]','nvarchar(max)'),1,2,N''
							)
									
						) + CASE WHEN isnull(comment,'') <> '' THEN '<br />' ELSE '' END
				,'') + 
				isnull(comment,'') AS comment,
				
				description,
				actual_date,
				time_min,
				responsible_person
			FROM
				#tmp2 s
		
		)x

		
		GROUP BY
			organization_PK, Client,
			substance_final,
			substance_final_pk,
		--	substance_product_final,
			activity_FK,
			time_unit_PK,
			time_unit_name ,
			description ,
			CASE WHEN comment = '' THEN NULL ELSE comment end,
			actual_date,
--			actual_date ,
			time_min ,
			responsible_person
	)x
)x



if object_id ('tempdb..#cte_substances_complete') is not null drop table #cte_substances_complete

if object_id ('tempdb..#tmp2') is not null drop table #tmp2
