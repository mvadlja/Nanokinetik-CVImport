CREATE TABLE [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER] (
    [org_in_type_for_manufacturer_ID] INT IDENTITY (1, 1) NOT NULL,
    [organization_FK]                 INT NOT NULL,
    [org_type_for_manu_FK]            INT NULL,
    [product_FK]                      INT NULL,
    [substance_FK]                    INT NULL,
    CONSTRAINT [PK_ORG_IN_TYPE_FOR_MANUFACTURER] PRIMARY KEY CLUSTERED ([org_in_type_for_manufacturer_ID] ASC),
    CONSTRAINT [FK_ORG_IN_TYPE_FOR_MANUFACTURER_ORGANIZATION] FOREIGN KEY ([organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK])
);





