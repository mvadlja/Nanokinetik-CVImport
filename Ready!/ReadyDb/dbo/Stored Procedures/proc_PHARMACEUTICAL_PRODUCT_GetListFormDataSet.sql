CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_GetListFormDataSet]
	@Name nvarchar(250) = NULL,
	@ID nvarchar(250) = NULL,
	@Products nvarchar(250) = NULL,
	@PharmaceuticalForm nvarchar(250) = NULL,
	@ActiveSubstances nvarchar(250) = NULL,
	@Excipients nvarchar(250) = NULL,
	@Adjuvants nvarchar(250) = NULL,
	@AdministrationRoutes nvarchar(250) = NULL,
	@DocCount nvarchar(250) = NULL,
	@ResponsibleUserFk nvarchar(250) = NULL,

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
	SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
	PharmaceuticalProduct.* FROM
	(
		SELECT DISTINCT
		pharProd.pharmaceutical_product_PK,
		ISNULL(pharProd.name, ''Missing'') AS Name,
		pharProd.ID,
		pharProd.Products,
		pharProd.ActiveSubstances,
		pharProd.Excipients,
		pharProd.Adjuvants,
		pharProd.AdministrationRoutes,
		pharProd.responsible_user_FK AS ResponsibleUserFk,
		pharForm.name AS PharmaceuticalForm,
		(select count(DISTINCT pd.doc_FK) 
		from dbo.PP_DOCUMENT_MN pd
		where pd.pp_FK = pharProd.pharmaceutical_product_PK) as DocCount
		'
	IF @QueryBy = 'Product' 
		BEGIN 
			SET @Query = @Query +
		'FROM dbo.PRODUCT_PP_MN ppMN
		JOIN dbo.PHARMACEUTICAL_PRODUCT pharProd ON pharProd.pharmaceutical_product_PK = ppMN.pp_FK
		LEFT JOIN dbo.PHARMACEUTICAL_FORM pharForm on pharForm.pharmaceutical_form_PK = pharProd.Pharmform_FK
		WHERE ppMN.product_FK = ' +  CASE WHEN ISNUMERIC(@EntityPk) = 1 THEN @EntityPk ELSE '0' END + '
		'
		END
	ELSE SET @Query = @Query + 
		'FROM dbo.PHARMACEUTICAL_PRODUCT pharProd
			LEFT JOIN dbo.PHARMACEUTICAL_FORM pharForm on pharForm.pharmaceutical_form_PK = pharProd.Pharmform_FK
		'
		SET @Query = @Query + 
		') AS PharmaceuticalProduct 
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

		-- @ResponsibleUserFk
		IF @ResponsibleUserFk IS NOT NULL
		BEGIN
			IF LEN(@TempWhereQuery) = 0 BEGIN SET @TempWhereQuery = 'WHERE '; END
			ELSE BEGIN SET @TempWhereQuery = @TempWhereQuery + ' AND '; END
			SET @TempWhereQuery = @TempWhereQuery + 'PharmaceuticalProduct.ResponsibleUserFk = ' + REPLACE(REPLACE(@ResponsibleUserFk,'[','[[]'),'''','''''') + ''
		END

		IF LEN(@TempWhereQuery) > 0 BEGIN SET @Query = @Query + @TempWhereQuery; END
		SET @Query = @Query + 
	')
	
	'
	SET @ExecuteQuery = @Query +
	'
	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @ExecuteQuery;

	SET @ExecuteQuery = @Query +
	'
	SELECT COUNT (*)
	FROM PagingResult
	'
	EXECUTE sp_executesql @ExecuteQuery;

END