CREATE PROCEDURE [dbo].[rep_06_pharmacovigilance_status]	
	--@ActiveIngredientPk INT = -1,
	--@ProductPk INT = -1,
	--@ActivityPk INT = -1,
	--@TaskPk INT = -1,
	--@TaskResponsibleUserPk INT = -1,
	--@TaskStartDateFrom DATE,
	--@TaskStartDateTo DATE
AS
BEGIN

DECLARE @AppUrl NVARCHAR(128) = 'http://pantera:8084';
				
	SELECT DISTINCT
		 ppAi.substancecode_FK		
		, ppAi.concise
		, ppAi.activeingredient_PK
		, s.substance_name AS SubstanceName
		--, pp.pharmaceutical_product_PK
		--, pp.name AS PPName
		, p.product_PK
		, p.name AS ProductName
		, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/ProductView/Preview.aspx?EntityContext=Product&idProd=' + CAST(p.product_PK AS varchar) + ''',''_blank''))">' + p.name + '</a>' AS ProductLink
		, (SELECT STUFF((
				SELECT 
					--'<br /><a href="javascript:void(window.open(''' + @AppURL + '/Views/ProductView/Preview.aspx?EntityContext=Product&idProd=' + convert(varchar,ProductPk) + ''',''_blank''))"><div>'+ ProductName +'</div></a><br/>'
					', ' + CAST(ProductPk AS NVARCHAR(10))
				FROM
					(
						SELECT DISTINCT ip.product_PK AS ProductPk, ip.name AS ProductName 
						FROM dbo.PRODUCT ip 
						LEFT JOIN ACTIVITY_PRODUCT_MN apMn1 ON apMn1.product_FK = ip.product_PK
						WHERE apMn1.activity_FK = a.activity_PK) x
						FOR XML PATH(''), TYPE
				  ).value('text()[1]','NVARCHAR(MAX)'), 1, 1, '')) AS ProductLinks
		, p.Countries AS ProductCountries
		, p.next_dlp
		, ap.ap_PK
	    , ap.product_name AS AuthorisedProductName
	    , localPv.FullName AS LocalPvFullName
	    , qppv.FullName AS QppvFullName
	    , mac.abbreviation AS MACountryName
	    , ma.name_org AS MAHName
	    , mfl.name_org AS MFLName
		, a.activity_PK
		, a.name AS ActivityName
		, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct=' + CAST(a.activity_PK AS varchar)+''',''_blank''))">' + a.name + '</a>' AS ActivityLink
		, (SELECT STUFF(
							(SELECT ', ' + ac.abbreviation FROM ACTIVITY_COUNTRY_MN acMn
							 LEFT JOIN COUNTRY ac ON ac.country_PK = acMn.country_FK
							 WHERE acMn.activity_FK = a.activity_PK
							 FOR XML PATH('')
							 )
						, 1, 1, '')) AS ActivityCountries
		, a.start_date AS ActivityStartDate
		, t.task_PK
		, tn.task_name AS TaskName
		, (SELECT STUFF(
							(SELECT ', ' + ac.abbreviation FROM TASK_COUNTRY_MN tcMn
							 LEFT JOIN COUNTRY ac ON ac.country_PK = tcMn.country_FK
							 WHERE tcMn.task_FK = t.task_PK
							 FOR XML PATH('')
							 )
						, 1, 1, '')) TaskCountries
		, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/TaskView/Preview.aspx?EntityContext=Task&idTask=' + CAST(t.task_PK AS varchar)+''',''_blank''))">' + tn.task_name + '</a>' AS TaskLink
		, tp.FullName AS TaskResponsibleUser
		, t.start_date AS TaskStartDate
		
	FROM ACTIVITY a
		LEFT JOIN ACTIVITY_PRODUCT_MN apMn ON apMn.activity_FK = a.activity_PK
		LEFT JOIN PRODUCT p ON p.product_PK = apMn.product_FK
		LEFT JOIN TASK t ON t.activity_FK = a.activity_PK 
		LEFT JOIN TASK_NAME tn ON tn.task_name_PK = t.task_name_FK
		LEFT JOIN PRODUCT_PP_MN ppMn ON ppMn.product_FK = p.product_PK
		LEFT JOIN PHARMACEUTICAL_PRODUCT pp ON pp.pharmaceutical_product_PK = ppMn.pp_FK
		LEFT JOIN PP_ACTIVE_INGREDIENT ppAi ON ppAi.pp_FK = pp.pharmaceutical_product_PK
		LEFT JOIN SUBSTANCES s ON s.substance_PK = ppAi.substancecode_FK
		LEFT JOIN PERSON tp ON tp.person_PK = t.user_FK
		LEFT JOIN AUTHORISED_PRODUCT ap ON ap.product_FK = p.product_PK
		LEFT JOIN QPPV_CODE qc ON qc.qppv_code_PK = ap.qppv_code_FK
		LEFT JOIN PERSON qppv ON qppv.person_PK = qc.person_FK
		LEFT JOIN QPPV_CODE lqc ON lqc.qppv_code_PK = ap.local_qppv_code_FK
		LEFT JOIN PERSON localPv ON localPv.person_PK = lqc.person_FK
		LEFT JOIN ORGANIZATION ma ON ma.organization_PK = ap.organizationmahcode_FK
		LEFT JOIN COUNTRY mac ON mac.country_PK = ma.countrycode_FK
		LEFT JOIN ORGANIZATION mfl ON mfl.organization_PK = ap.mflcode_FK

    --WHERE p.product_PK IN (10686, 10687, 10688, 10689)
	WHERE concise IS NOT NULL --AND p.product_PK IN (10686)
		--(ppAi.substancecode_FK = @ActiveIngredientPk OR @ActiveIngredientPk = -1) AND
		--(p.product_PK = @ProductPk OR @ProductPk = -1) AND
		--(a.activity_PK = @ActivityPk OR @ActivityPk = -1) AND
		--(t.task_PK = @TaskPk OR @TaskPk = -1) AND
		--(t.task_ID LIKE '%' + @TaskId + '%' OR @TaskId IS NULL) AND
		--(tp.person_PK = @TaskResponsibleUserPk OR @TaskResponsibleUserPk  = -1) 
		--AND
		--(
		-- ((t.start_date >= @StartDateFrom OR @StartDateFrom IS NULL) AND (t.start_date <= @StartDateTo OR @StartDateTo IS NULL))
		--    AND 
		-- ((t.expected_finished_date >= @ExpectedFinishedDateFrom OR @ExpectedFinishedDateFrom IS NULL) AND (t.expected_finished_date <= @ExpectedFinishedDateTo OR @ExpectedFinishedDateTo IS NULL))
		--	AND
		-- ((t.actual_finished_date >= @ActualFinishedDateFrom OR @ActualFinishedDateFrom IS NULL) AND (t.actual_finished_date <= @ActualFinishedDateTo OR @ActualFinishedDateTo IS NULL))
		-- ) 
END