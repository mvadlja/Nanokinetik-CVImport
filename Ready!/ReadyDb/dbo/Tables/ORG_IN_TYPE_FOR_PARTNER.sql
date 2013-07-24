CREATE TABLE [dbo].[ORG_IN_TYPE_FOR_PARTNER] (
    [org_in_type_for_partner_ID] INT IDENTITY (1, 1) NOT NULL,
    [organization_FK]            INT NOT NULL,
    [org_type_for_partner_FK]    INT NULL,
    [product_FK]                 INT NULL,
    CONSTRAINT [PK_ORG_IN_TYPE_FOR_PARTNER] PRIMARY KEY CLUSTERED ([org_in_type_for_partner_ID] ASC),
    CONSTRAINT [FK_ORG_IN_TYPE_FOR_PARTNER_ORGANIZATION] FOREIGN KEY ([organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK])
);



