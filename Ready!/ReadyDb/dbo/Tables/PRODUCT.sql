CREATE TABLE [dbo].[PRODUCT] (
    [product_PK]                 INT             IDENTITY (1, 1) NOT NULL,
    [newownerid]                 NVARCHAR (60)   NULL,
    [senderlocalcode]            NVARCHAR (100)  NULL,
    [orphan_drug]                BIT             NULL,
    [intensive_monitoring]       INT             NULL,
    [authorisation_procedure]    INT             NULL,
    [comments]                   NVARCHAR (MAX)  NULL,
    [responsible_user_person_FK] INT             NULL,
    [psur]                       NVARCHAR (250)  NULL,
    [next_dlp]                   DATETIME        NULL,
    [name]                       NVARCHAR (2000) NULL,
    [description]                NVARCHAR (MAX)  NULL,
    [client_organization_FK]     INT             NULL,
    [type_product_FK]            INT             NULL,
    [product_number]             NVARCHAR (100)  NULL,
    [product_ID]                 NVARCHAR (100)  NULL,
    [mrp_dcp]                    NVARCHAR (100)  NULL,
    [eu_number]                  NVARCHAR (100)  NULL,
    [ProductName]                NVARCHAR (MAX)  NULL,
    [Countries]                  NVARCHAR (MAX)  NULL,
    [ActiveSubstances]           NVARCHAR (MAX)  NULL,
    [DrugAtcs]                   NVARCHAR (MAX)  NULL,
    [client_group_FK]            INT             NULL,
    [region_FK]                  INT             NULL,
    [batch_size]                 NVARCHAR (500)  NULL,
    [pack_size]                  NVARCHAR (500)  NULL,
    [storage_conditions_FK]      INT             NULL,
    CONSTRAINT [PK_PRODUCT] PRIMARY KEY CLUSTERED ([product_PK] ASC),
    CONSTRAINT [FK_AP_GRUPA_USER] FOREIGN KEY ([responsible_user_person_FK]) REFERENCES [dbo].[PERSON] ([person_PK]),
    CONSTRAINT [FK_PRODUCT_ORGANIZATION] FOREIGN KEY ([client_organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK]),
    CONSTRAINT [FK_PRODUCT_TYPE] FOREIGN KEY ([type_product_FK]) REFERENCES [dbo].[TYPE] ([type_PK])
);






GO
CREATE NONCLUSTERED INDEX [IX_product_client_organization_FK]
    ON [dbo].[PRODUCT]([client_organization_FK] ASC);

