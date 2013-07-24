
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetListFormSearchDataSet]
	@SearchProductPk nvarchar(250) = NULL,
	@SearchName nvarchar(250) = NULL,
	@SearchResponsibleUserPk nvarchar(250) = NULL,
	@SearchDescription nvarchar(250) = NULL,
	@SearchPharmaceuticalFormPk nvarchar(250) = NULL,
	@SearchAdministrationRoutes nvarchar(250) = NULL,
	@SearchActiveSubstances nvarchar(250) = NULL,
	@SearchExcipients nvarchar(250) = NULL,
	@SearchAdjuvants nvarchar(250) = NULL,
	@SearchMedicalDevices nvarchar(250) = NULL,
	@SearchComment nvarchar(250) = NULL,

	@Name nvarchar(250) = NULL,
	@ID nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@PharmaceuticalForm nvarchar(250) = NULL,
	@ActiveSubstances nvarchar(250) = NULL,
	@Excipients nvarchar(250) = NULL,
	@Adjuvants nvarchar(250) = NULL,
	@AdministrationRoutes nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,

	@QueryBy nvarchar(25) = NULL,
	@EntityPk nvarchar(25) = NULL,
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'Name'
AS

DECLARE @Query nvarchar(MAX);
DECLARE @ExecuteQuery nvarchar(MAX);
DECLARE @TempWhereQuery nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT DISTINCT pharmaceutical_product_PK, Name, ID, Products, ActiveSubstances, Excipients, Adjuvants, AdministrationRoutes, PharmaceuticalForm, DocCount
		FROM
		(
			SELECT PharmaceuticalProduct.* 
			FROM
			(
				SELECT DISTINCT
				pharProd.pharmaceutical_product_PK,
				ISNULL(pharProd.name, ''Missing'') AS Name,
				pharProd.ID,
				pharProd.[description],
				pharProd.comments,
				pharProd.Pharmform_FK,
				pharProd.responsible_user_FK,
				pharProd.Products,
				pharProd.ActiveSubstances,
				(SELECT Stuff((SELECT ''||| '' + ev_code FROM dbo.PP_ACTIVE_INGREDIENT ppaiMn JOIN dbo.SUBSTANCES s ON ppaiMn.substancecode_FK = s.substance_PK
					WHERE ppaiMn.pp_FK = pharProd.pharmaceutical_product_PK	FOR XML PATH('''')),1,1,'''')) AS ActiveSubstancesEVCODE,
				pharProd.Excipients,
				(SELECT Stuff((SELECT ''||| '' + ev_code FROM dbo.PP_EXCIPIENT ppexMn JOIN dbo.SUBSTANCES s ON ppexMn.substancecode_FK = s.substance_PK
					WHERE ppexMn.pp_FK = pharProd.pharmaceutical_product_PK	FOR XML PATH('''')),1,1,'''')) AS ExcipientsEVCODE,
				pharProd.Adjuvants,
				(SELECT Stuff((SELECT ''||| '' + ev_code FROM dbo.PP_ADJUVANT ppadMn JOIN dbo.SUBSTANCES s ON ppadMn.substancecode_FK = s.substance_PK
					WHERE ppadMn.pp_FK = pharProd.pharmaceutical_product_PK	FOR XML PATH('''')),1,1,'''')) AS AdjuvantsEVCODE,
				pharProd.AdministrationRoutes,
				pharProd.MedicalDevices,
				pharForm.name AS PharmaceuticalForm,
				ppMN.product_FK,
				(select count(DISTINCT pd.doc_FK) 
				from dbo.PP_DOCUMENT_MN pd
				where pd.pp_FK = pharProd.pharmaceutical_product_PK) as DocCount
				FROM dbo.PHARMACEUTICAL_PRODUCT pharProd
				LEFT JOIN dbo.PRODUCT_PP_MN ppMN ON ppMN.pp_FK = pharProd.pharmaceutical_product_PK
				LEFT JOIN dbo.PHARMACEUTICAL_FORM pharForm on pharForm.pharmaceutical_form_PK = pharProd.Pharmform_FK
			) AS PharmaceuticalProduct 
			'
		SET @TempWhereQuery = '';

		-- @Name
		IF @Name IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Name LIKE N''%' + REPLACE(REPLACE(@Name,'[','[[]'),'''','''''') + '%'''
		END

		-- @ID
		IF @ID IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ID LIKE N''%' + REPLACE(REPLACE(@ID,'[','[[]'),'''','''''') + '%'''
		END

		-- @Products
		IF @Products IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Products LIKE N''%' + REPLACE(REPLACE(@Products,'[','[[]'),'''','''''') + '%'''
		END

		-- @ActiveSubstances
		IF @ActiveSubstances IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ActiveSubstances LIKE N''%' + REPLACE(REPLACE(@ActiveSubstances,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @Excipients
		IF @Excipients IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Excipients LIKE N''%' + REPLACE(REPLACE(@Excipients,'[','[[]'),'''','''''') + '%'''
		END

		-- @Adjuvants
		IF @Adjuvants IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Adjuvants LIKE N''%' + REPLACE(REPLACE(@Adjuvants,'[','[[]'),'''','''''') + '%'''
		END
		
		-- @AdministrationRoutes
		IF @AdministrationRoutes IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.AdministrationRoutes LIKE N''%' + REPLACE(REPLACE(@AdministrationRoutes,'[','[[]'),'''','''''') + '%'''
		END

		-- @PharmaceuticalForm
		IF @PharmaceuticalForm IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.PharmaceuticalForm LIKE N''%' + REPLACE(REPLACE(@PharmaceuticalForm,'[','[[]'),'''','''''') + '%'''
		END

		-- @DocCount
		IF @DocCount IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.DocCount LIKE N''%' + REPLACE(REPLACE(@DocCount,'[','[[]'),'''','''''') + '%'''
		END

		------------------------------------------------------------------------SEARCH------------------------------------------------------------------------

		-- @SearchProductPk
		IF @SearchProductPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.product_FK = ' + REPLACE(REPLACE(@SearchProductPk,'[','[[]'),'''','''''')
		END

		-- @SearchName
		IF @SearchName IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Name LIKE N''%' + REPLACE(REPLACE(@SearchName,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchResponsibleUserPk
		IF @SearchResponsibleUserPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.responsible_user_FK = ' + REPLACE(REPLACE(@SearchResponsibleUserPk,'[','[[]'),'''','''''')
		END

		-- @SearchDescription
		IF @SearchDescription IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.description LIKE N''%' + REPLACE(REPLACE(@SearchDescription,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchPharmaceuticalFormPk
		IF @SearchPharmaceuticalFormPk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Pharmform_FK = ' + REPLACE(REPLACE(@SearchPharmaceuticalFormPk,'[','[[]'),'''','''''')
		END

		-- @SearchAdministrationRoutes
		IF @SearchAdministrationRoutes IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.AdministrationRoutes LIKE N''%' + REPLACE(REPLACE(@SearchAdministrationRoutes,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchActiveSubstances
		IF @SearchActiveSubstances IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE ('; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND ('; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ActiveSubstances LIKE N''%' + REPLACE(REPLACE(@SearchActiveSubstances,'[','[[]'),'''','''''') + '%''';
			SET @TempWhereQuery = @TempWhereQuery + ' OR ';
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ActiveSubstancesEVCODE LIKE N''%' + REPLACE(REPLACE(@SearchActiveSubstances,'[','[[]'),'''','''''') + '%''';	
			SET @TempWhereQuery = @TempWhereQuery + ')'
		END

		-- @SearchExcipients
		IF @SearchExcipients IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE ('; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND ('; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Excipients LIKE N''%' + REPLACE(REPLACE(@SearchExcipients,'[','[[]'),'''','''''') + '%'''
			SET @TempWhereQuery = @TempWhereQuery + ' OR ';
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ExcipientsEVCODE LIKE N''%' + REPLACE(REPLACE(@SearchExcipients,'[','[[]'),'''','''''') + '%''';	
			SET @TempWhereQuery = @TempWhereQuery + ')'
		END

		-- @SearchAdjuvants
		IF @SearchAdjuvants IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE ('; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND ('; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.Adjuvants LIKE N''%' + REPLACE(REPLACE(@SearchAdjuvants,'[','[[]'),'''','''''') + '%'''
			SET @TempWhereQuery = @TempWhereQuery + ' OR ';
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.AdjuvantsEVCODE LIKE N''%' + REPLACE(REPLACE(@SearchAdjuvants,'[','[[]'),'''','''''') + '%''';	
			SET @TempWhereQuery = @TempWhereQuery + ')'
		END

		-- @SearchMedicalDevices
		IF @SearchMedicalDevices IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.MedicalDevices LIKE N''%' + REPLACE(REPLACE(@SearchMedicalDevices,'[','[[]'),'''','''''') + '%'''
		END

		-- @SearchComment
		IF @SearchComment IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.comments LIKE N''%' + REPLACE(REPLACE(@SearchComment,'[','[[]'),'''','''''') + '%'''
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + '
		) DistinctResult
	)
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,*
		FROM PagingResult
	)x
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END