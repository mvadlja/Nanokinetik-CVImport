CREATE TABLE [dbo].[PRODUCT_DOMAIN_MN] (
    [product_domain_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]           INT NULL,
    [domain_FK]            INT NULL,
    CONSTRAINT [PK_PRODUCT_DOMAIN_MN] PRIMARY KEY CLUSTERED ([product_domain_mn_PK] ASC),
    CONSTRAINT [FK_PRODUCT_DOMAIN_MN_AP_GRUPA] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_PRODUCT_DOMAIN_MN_DOMAIN] FOREIGN KEY ([domain_FK]) REFERENCES [dbo].[DOMAIN] ([domain_PK])
);

