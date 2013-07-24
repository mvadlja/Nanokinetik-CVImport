-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_GetTabMenuItemsCount]
	@product_PK int = NULL,
	@ResponsibleUserPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AuditRecordsCount NVARCHAR(MAX) = NULL;
	DECLARE @AlertRecordsCount NVARCHAR(MAX) = NULL;

	DECLARE @PersonEmail NVARCHAR(500) = (SELECT email FROM PERSON WHERE person_PK = CONVERT(INT, @ResponsibleUserPk));

	select @AuditRecordsCount = count (distinct SessionToken)
	from AuditingMaster am
	left join [AuditingDetails] ad on am.IDAuditingMaster = ad.MasterID		
	where am.TableName = 'PRODUCT' and ad.PKValue = CAST (@product_PK AS NVARCHAR(MAX));	

	SELECT @AlertRecordsCount = COUNT(DISTINCT  rmd.reminder_PK) 
			FROM [dbo].[REMINDER] rmd
			LEFT JOIN dbo.REMINDER_EMAIL_RECIPIENT rmdERcv ON rmdERcv.reminder_FK = rmd.reminder_PK
			LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = rmd.reminder_PK
			LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
			WHERE  (rmd.responsible_user_FK = @ResponsibleUserPk OR
			(rmdERcv.person_FK = @ResponsibleUserPk AND @ResponsibleUserPk IS NOT NULL OR
			rmd.additional_emails LIKE @PersonEmail OR 
			rmd.additional_emails LIKE @PersonEmail + ',%' OR
			rmd.additional_emails LIKE '%, ' + @PersonEmail +',%' OR
			rmd.additional_emails LIKE '%, ' + @PersonEmail)) AND
			(rmd.is_automatic != 1 or reminderStatus.name != 'Dismissed') AND (rmd.reminder_type LIKE 'Product' OR rmd.reminder_type LIKE 'P Document') AND rmd.related_entity_FK = @product_PK

	SELECT (select count(*) from dbo.AUTHORISED_PRODUCT ap
			where ap.product_FK = @product_PK) as 'ProdAuthProdList',
			(select count(DISTINCT pp.pp_FK) from dbo.PRODUCT_PP_MN pp
			where pp.product_FK = @product_PK) as 'ProdPharmProdList',
			(select count(DISTINCT pd.document_FK) from dbo.PRODUCT_DOCUMENT_MN pd
			where pd.product_FK = @product_PK) as 'ProdDocList',
			(select count(DISTINCT ap.activity_FK) from [dbo].[ACTIVITY_PRODUCT_MN] ap
			where ap.product_FK = @product_PK) as 'ProdActList',
			(select COUNT(DISTINCT su.submission_unit_FK) FROM
			dbo.PRODUCT_SUB_UNIT_MN as SU
			where product_FK = @product_PK) as 'ProdSubUnitList',
		    (select @AlertRecordsCount) as 'ProdAlertList',
			(select @AuditRecordsCount) as 'ProdAuditTrailList',
            (SELECT COUNT(DISTINCT dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK) 
				
				FROM [dbo].XEVPRM_ENTITY_DETAILS_MN
					LEFT JOIN dbo.XEVPRM_AP_DETAILS ON dbo.XEVPRM_AP_DETAILS.xevprm_ap_details_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_details_FK
					LEFT JOIN dbo.XEVPRM_MESSAGE ON dbo.XEVPRM_MESSAGE.xevprm_message_PK = dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_message_FK

				WHERE dbo.XEVPRM_ENTITY_DETAILS_MN.xevprm_entity_type_FK = 1 AND
					dbo.XEVPRM_AP_DETAILS.related_product_FK = @product_PK AND
					dbo.XEVPRM_MESSAGE.deleted != 1 ) as 'ProdXevprmList'
END
