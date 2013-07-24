CREATE TABLE [dbo].[PRODUCT_ORGANIZATION_PARTNER_MN] (
    [product_organization_partner_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [organization_FK]                    INT NULL,
    [product_FK]                         INT NULL,
    CONSTRAINT [PK_PRODUCT_ORGANIZATION_PARTNER_MN] PRIMARY KEY CLUSTERED ([product_organization_partner_mn_PK] ASC),
    CONSTRAINT [FK_PRODUCT_ORGANIZATION_PARTNER_MN_ORGANIZATION] FOREIGN KEY ([organization_FK]) REFERENCES [dbo].[ORGANIZATION] ([organization_PK]),
    CONSTRAINT [FK_PRODUCT_ORGANIZATION_PARTNER_MN_PRODUCT] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]) ON DELETE CASCADE
);

