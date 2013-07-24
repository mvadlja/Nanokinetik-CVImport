CREATE FUNCTION ReturnAUDITNameForID
(
	@tableName nvarchar(max) = NULL,
	@columnName nvarchar(max) = NULL,
	@field_FK int = NULL
)

RETURNS nvarchar(max)
AS
  BEGIN

	IF (@tableName = 'REMINDER')
	BEGIN
		IF (@columnName = 'responsible_user_FK')
			RETURN (select FullName from [PERSON] where person_PK=@field_FK)
		IF (@columnName = 'reminder_user_status_FK')
			RETURN (select name from [REMINDER_USER_STATUS] where reminder_user_status_PK=@field_FK)
	END
	
	  		 
    DECLARE @var nvarchar(max)
    
    set @var =	case 
			-- country abbreviation
    		when @columnName = 'authorisationcountrycode_FK' then (select abbreviation from [Country] where country_PK=@field_FK ) 
    		
    		-- product name
    		when @columnName = 'product_FK' then (select name from [Product] where product_PK=@field_FK)
    	
    	    when @columnName = 'qppv_code_FK' 
			  or @columnName = 'local_qppv_code_FK' then (
    			select FullName + ISNULL(NULLIF(' (' + qppv_code  + ')', ' ()'), '')
    			from QPPV_CODE qppvCode
    			LEFT JOIN [PERSON] p on p.person_PK = qppvCode.person_FK
    			where qppv_code_PK=@field_FK
    	    )
			    		
    		--person
    		when (@columnName = 'responsible_user_person_FK' OR 
    			  @columnName = 'responsible_user_FK' OR
    			  @columnName='user_FK' OR
    			  @columnName='qppvcode_person_FK' OR
    			  @columnName='person_FK') 
    					then (select FullName from [PERSON] where person_PK=@field_FK) 
    		
    		-- organization
    		when (@columnName = 'client_organization_FK' OR
    			  @columnName='organization_FK' OR
    			  @columnName='organizationmahcode_FK' OR  
    			  @columnName='local_representative_FK' OR
				  @columnName='license_holder_group_FK' OR
    			  @columnName='mflcode_FK' OR
    			  @columnName='client_group_FK' ) 
    					then (select name_org from [ORGANIZATION] where organization_PK=@field_FK )
    		
    		--type 
    		when (@columnName = 'type_FK' OR
    			  @columnName = 'type_product_FK' OR 
    			  @columnName='procedure_type_FK' OR 
    			  @columnName='regulatory_status_FK' OR
    			  @columnName='internal_status_FK' OR
    			  @columnName='authorisationstatus_FK' OR
    			  @columnName='authorisation_procedure' OR
    			  @columnName='legalstatus' OR
    			  @columnname='mode_FK' OR
    			  @columnName='regulatory_status' OR
    			  @columnName='version_number' OR
    			  @columnName='version_label' OR
    			  @columnName='attachment_type_FK' OR
    			  @columnName='region_FK' OR
    			  @columnName='storage_conditions_FK') 
    					then (select name from [TYPE] where type_PK = @field_FK)
    		
    		when (@columnName='Pharmform_FK')
    					then (select name from [PHARMACEUTICAL_FORM] where pharmaceutical_form_PK = @field_FK)
    		
    		end
    		
    if(@field_FK is null OR @field_FK='') 
    		set @var = ''

	IF (@var IS NULL)
		SET @var = '';
	    		
    RETURN @var
    
  END
