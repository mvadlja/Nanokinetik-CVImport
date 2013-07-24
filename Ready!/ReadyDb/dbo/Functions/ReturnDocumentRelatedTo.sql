CREATE FUNCTION ReturnDocumentRelatedTo

(
	@doc_PK int=693
)

RETURNS NVARCHAR(4000)
AS
  BEGIN	
	
	declare @related_to nvarchar(max)

	SELECT @related_to = COALESCE(@related_to + ', ', '') +
			isnull(rtrim(ltrim(Name)), '')
			
	FROM	(select  ap.product_name AS Name
					 from AP_DOCUMENT_MN adm
					 LEFT JOIN [AUTHORISED_PRODUCT] ap ON ap.ap_PK = adm.ap_FK
					 where adm.document_FK = @doc_PK
				UNION ALL
				select p.name AS Name
					 from PRODUCT_DOCUMENT_MN pdm
					 LEFT JOIN [PRODUCT] p ON p.product_PK = pdm.product_FK
					 where pdm.document_FK = @doc_PK
				UNION ALL
				select pp.name AS Name
					 from PP_DOCUMENT_MN ppdm
					 LEFT JOIN [PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppdm.pp_FK
					 where ppdm.doc_FK = @doc_PK
				UNION ALL
				SELECT p.name AS Name
					  from PROJECT_DOCUMENT_MN  pdm
					  LEFT JOIN [PROJECT] p ON p.project_PK = pdm.project_FK
					  where pdm.document_FK = @doc_PK
				UNION ALL
				SELECT a.name AS Name
					  from ACTIVITY_DOCUMENT_MN  adm
					  LEFT JOIN [ACTIVITY] a ON a.activity_PK = adm.activity_FK
					  where adm.document_FK = @doc_PK	
			    union all
				SELECT tn.task_name  as Name
					  from TASK_DOCUMENT_MN  tdm
					  LEFT JOIN [TASK] t ON t.task_PK = tdm.task_FK 
					  LEFT JOIN [TASK_NAME] tn on tn.task_name_PK = t.task_name_FK
					  where tdm.document_FK = @doc_PK
			) as tejbl
  
    return @related_to
    
  END
