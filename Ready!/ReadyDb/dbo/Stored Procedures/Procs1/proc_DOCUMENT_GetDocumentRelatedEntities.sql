-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_GetDocumentRelatedEntities]
	@doc_PK int = null
AS
BEGIN
	SET NOCOUNT ON;

	with athorised_products as
			(
				select adm.ap_document_mn_PK mn_PK,
					   ap.product_name AS Name,
					   ap.ap_PK AS PKValue,
					   ('Views/AuthorisedProductView/Preview.aspx?EntityContext=AuthorisedProduct&idAuthProd=' + CONVERT(nvarchar(MAX), ap.ap_PK )) AS FullURL
					 from AP_DOCUMENT_MN adm
					 LEFT JOIN [AUTHORISED_PRODUCT] ap ON ap.ap_PK = adm.ap_FK
					 LEFT JOIN [DOCUMENT] d ON d.document_PK = adm.document_FK
					 where d.document_PK = @doc_PK
				UNION ALL
				select pdm.product_document_mn_PK mn_PK,
					   p.name AS Name,
					   p.product_PK AS PKValue,
					   ('Views/ProductView/Preview.aspx?EntityContext=Product&idProd=' + CONVERT(nvarchar(MAX), p.product_PK )) AS FullURL
					 from PRODUCT_DOCUMENT_MN pdm
					 LEFT JOIN [PRODUCT] p ON p.product_PK = pdm.product_FK
					 LEFT JOIN [DOCUMENT] d ON d.document_PK = pdm.document_FK
					 where d.document_PK = @doc_PK
				UNION ALL
				select ppdm.pp_document_PK mn_PK,
					   pp.name AS Name,
					   pp.pharmaceutical_product_PK AS PKValue,
					   ('Views/PharmaceuticalProductView/Preview.aspx?EntityContext=PharmaceuticalProduct&idPharmProd=' + CONVERT(nvarchar(MAX), pp.pharmaceutical_product_PK )) AS FullURL
					 from PP_DOCUMENT_MN ppdm
					 LEFT JOIN [PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppdm.pp_FK
					 LEFT JOIN [DOCUMENT] d ON d.document_PK = ppdm.doc_FK
					 where d.document_PK = @doc_PK
				UNION ALL
				SELECT pdm.project_document_PK mn_PK,
					   p.name,
					   p.project_PK AS PKValue,
					  ('Views/ProjectView/Preview.aspx?EntityContext=Project&idProj=' + CONVERT(nvarchar(max), p.project_PK)) AS FullURL
					  from PROJECT_DOCUMENT_MN  pdm
					  LEFT JOIN [PROJECT] p ON p.project_PK = pdm.project_FK
					  LEFT JOIN [DOCUMENT] d ON d.document_PK = pdm.document_FK
					  where d.document_PK = @doc_PK
				UNION ALL
				SELECT adm.activity_document_PK mn_PK,
					   a.name,
					   a.activity_PK AS PKValue,
					  ('Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct=' + CONVERT(nvarchar(max), a.activity_PK)) AS FullURL
					  from ACTIVITY_DOCUMENT_MN  adm
					  LEFT JOIN [ACTIVITY] a ON a.activity_PK = adm.activity_FK
					  LEFT JOIN [DOCUMENT] d ON d.document_PK = adm.document_FK
					  where d.document_PK = @doc_PK	
			    union all
				SELECT tdm.task_document_PK mn_PK,
					   tn.task_name  as Name,
					   t.task_PK AS PKValue,
					  ('Views/TaskView/Preview.aspx?EntityContext=Task&idTask=' + CONVERT(nvarchar(max), t.task_PK )) AS FullURL
					  from TASK_DOCUMENT_MN  tdm
					  LEFT JOIN [TASK] t ON t.task_PK = tdm.task_FK 
					  LEFT JOIN [DOCUMENT] d ON d.document_PK = tdm.document_FK 
					  LEFT JOIN [TASK_NAME] tn on tn.task_name_PK = t.task_name_FK
					  where d.document_PK = @doc_PK	
			)
	
	
	select mn_PK, Name,PKValue, FullURL from athorised_products;
	
END
