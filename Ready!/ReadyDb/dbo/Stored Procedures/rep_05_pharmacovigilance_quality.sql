CREATE PROCEDURE [dbo].[rep_05_pharmacovigilance_quality]	
	@ActiveIngredientPk INT = -1,
	@ProductPk INT = -1,
	@ActivityPk INT = -1,
	@TaskPk INT = -1,
	@TaskResponsibleUserPk INT = -1,
	@TaskId NVARCHAR(100) = NULL,
	@TaskStartDateFrom DATE,
	@TaskStartDateTo DATE,
	@TaskExpectedFinishedDateFrom DATE,
	@TaskExpectedFinishedDateTo DATE,
	@TaskActualFinishedDateFrom DATE,
	@TaskActualFinishedDateTo DATE,
	@TaskPerformanceIndicator NVARCHAR(100) = NULL
AS
BEGIN

DECLARE @AppUrl NVARCHAR(128) = 'http://localhost:16057/READY';
				
		WITH result AS (		
		SELECT DISTINCT
			 ppAi.substancecode_FK		
			, ppAi.concise
			, ppAi.activeingredient_PK
			, s.substance_name AS SubstanceName
			, pp.pharmaceutical_product_PK
			, pp.name AS PPName
			, p.product_PK
			, p.name AS ProductName
			, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/ProductView/Preview.aspx?EntityContext=Product&idProd='+ CAST(p.product_PK as varchar)+''',''_blank''))"><div>'+ p.name +'</div></a><br/>' as ProductLink
			, a.activity_PK
			, a.name AS ActivityName
			, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct='+ CAST(a.activity_PK as varchar)+''',''_blank''))"><div>'+ a.name +'</div></a><br/>' as ActivityLink
			, t.task_PK
			, tn.task_name AS TaskName
			, '<a href="javascript:void(window.open(''' + @AppURL + '/Views/TaskView/Preview.aspx?EntityContext=Task&idTask=' + CAST(t.task_PK AS varchar)+''',''_blank''))"><div>'+ tn.task_name +'</div></a><br/>' AS TaskLink
			, t.task_ID
			, tp.FullName AS TaskResponsibleUser
			, t.start_date AS TaskStartDate
			, t.expected_finished_date AS TaskExpectedFinishedDate
			, t.actual_finished_date AS TaskActualFinishedDate
			, (CASE 
				WHEN t.task_PK IS NULL THEN '' 
				WHEN t.task_PK IS NOT NULL AND t.actual_finished_date IS NOT NULL AND t.expected_finished_date IS NOT NULL AND t.actual_finished_date > t.expected_finished_date THEN 'failed' 
				ELSE 'comply' END) AS TaskPerformanceIndicator
			
		FROM PRODUCT p
			LEFT JOIN PRODUCT_PP_MN ppMn ON ppMn.product_FK = p.product_PK
			LEFT JOIN PHARMACEUTICAL_PRODUCT pp ON pp.pharmaceutical_product_PK = ppMn.pp_FK
			LEFT JOIN PP_ACTIVE_INGREDIENT ppAi ON ppAi.pp_FK = pp.pharmaceutical_product_PK
			LEFT JOIN SUBSTANCES s ON s.substance_PK = ppAi.substancecode_FK
			LEFT JOIN ACTIVITY_PRODUCT_MN apMn ON apMn.product_FK = p.product_PK
			LEFT JOIN ACTIVITY a ON a.activity_PK = apMn.activity_FK
			LEFT JOIN TASK t ON t.activity_FK = a.activity_PK 
			LEFT JOIN TASK_NAME tn ON tn.task_name_PK = t.task_name_FK
			LEFT JOIN PERSON tp ON tp.person_PK = t.user_FK
		WHERE concise IS NOT NULL AND 
			(ppAi.substancecode_FK = @ActiveIngredientPk OR @ActiveIngredientPk = -1) AND
			(p.product_PK = @ProductPk OR @ProductPk = -1) AND
			(a.activity_PK = @ActivityPk OR @ActivityPk = -1) AND
			(t.task_PK = @TaskPk OR @TaskPk = -1) AND
			(t.task_ID LIKE '%' + @TaskId + '%' OR @TaskId IS NULL) AND
			(tp.person_PK = @TaskResponsibleUserPk OR @TaskResponsibleUserPk = -1) 
			AND
			(
			 ((t.start_date >= @TaskStartDateFrom OR @TaskStartDateFrom IS NULL) AND (t.start_date <= @TaskStartDateTo OR @TaskStartDateTo IS NULL))
				AND 
			 ((t.expected_finished_date >= @TaskExpectedFinishedDateFrom OR @TaskExpectedFinishedDateFrom IS NULL) AND (t.expected_finished_date <= @TaskExpectedFinishedDateTo OR @TaskExpectedFinishedDateTo IS NULL))
				AND
			 ((t.actual_finished_date >= @TaskActualFinishedDateFrom OR @TaskActualFinishedDateFrom IS NULL) AND (t.actual_finished_date <= @TaskActualFinishedDateTo OR @TaskActualFinishedDateTo IS NULL))
			 ) 
	)
		 
	SELECT * FROM result WHERE (TaskPerformanceIndicator = @TaskPerformanceIndicator OR @TaskPerformanceIndicator IS NULL);
END