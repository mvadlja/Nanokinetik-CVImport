CREATE TABLE [dbo].[PRODUCT_SAVED_SEARCH] (
    [product_saved_search_PK]    INT             IDENTITY (1, 1) NOT NULL,
    [name]                       NVARCHAR (2000) NULL,
    [pharmaaceutical_product_FK] INT             NULL,
    [indication_FK]              INT             NULL,
    [product_number]             NVARCHAR (100)  NULL,
    [type_product_FK]            INT             NULL,
    [client_organization_FK]     INT             NULL,
    [domain_FK]                  INT             NULL,
    [procedure_type]             INT             NULL,
    [product_ID]                 NVARCHAR (100)  NULL,
    [country_FK]                 INT             NULL,
    [manufacturer_FK]            INT             NULL,
    [psur]                       NVARCHAR (250)  NULL,
    [displayName]                NVARCHAR (100)  NULL,
    [user_FK]                    INT             NULL,
    [gridLayout]                 NVARCHAR (MAX)  NULL,
    [isPublic]                   BIT             NULL,
    [nextdlp_from]               DATETIME        NULL,
    [nextdlp_to]                 DATETIME        NULL,
    [responsible_user_FK]        INT             NULL,
    [drug_atcs]                  NVARCHAR (MAX)  NULL,
    [client_name]                NVARCHAR (2000) NULL,
    [article57_reporting]        BIT             NULL,
    [ActiveSubstances]           NCHAR (250)     NULL,
    CONSTRAINT [PK_PRODUCT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([product_saved_search_PK] ASC)
);







