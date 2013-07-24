create view [dbo].[vProductXpo] as
SELECT
			[dbo].[PRODUCT].product_PK,
			[dbo].[PRODUCT].name as product,
			[dbo].[PRODUCT].product_number,
			(select count(*) from dbo.AUTHORISED_PRODUCT ap
			where ap.product_FK = [dbo].[PRODUCT].product_PK) as APCount,
			(
			select top 1 ss.substance_name +'/'+CONVERT(varchar, ppai.strength_value)+ ppai.strength_unit + '; ' + pf.name
			from dbo.PP_ACTIVE_INGREDIENT ppai
			left join dbo.SUBSTANCES ss on ppai.substancecode_FK=ss.substance_PK
			LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppai.pp_FK
			LEFT JOIN [dbo].PHARMACEUTICAL_FORM pf ON pf.pharmaceutical_form_PK = pp.Pharmform_FK
			left join [dbo].[PRODUCT_PP_MN] ppmn ON pp.pharmaceutical_product_PK = ppmn.pp_FK
			where ppmn.product_FK=[dbo].[PRODUCT].product_PK
			) as IngrStrFrom,
			
			AuthProc.name as AuthorProcedure,
			dbo.[ReturnProductCountries]([dbo].[PRODUCT].product_PK) as Countries,
			(select count(*) from dbo.PRODUCT_PP_MN pp
			where pp.product_FK = [dbo].[PRODUCT].product_PK) as PPCount,
			(select count(*) from dbo.PRODUCT_DOCUMENT_MN pd
			where pd.product_FK = [dbo].[PRODUCT].product_PK) as PDCount,
			(select count(*) from [dbo].[ACTIVITY_PRODUCT_MN] ap
			where ap.product_FK = [dbo].[PRODUCT].product_PK) as ACount
		FROM [dbo].[PRODUCT]
		LEFT JOIN dbo.[TYPE] AuthProc 
		on AuthProc.type_PK = [dbo].[PRODUCT].authorisation_procedure
