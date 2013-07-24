CREATE TABLE [dbo].[PRODUCT_COUNTRY_MN] (
    [product_country_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [country_FK]            INT NULL,
    [product_FK]            INT NULL,
    CONSTRAINT [PK_PRODUCT_COUNTRY_MN] PRIMARY KEY CLUSTERED ([product_country_mn_PK] ASC),
    CONSTRAINT [FK_AP_COUNTRY_MN_AP_GRUPA] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_PRODUCT_COUNTRY_MN_COUNTRY] FOREIGN KEY ([country_FK]) REFERENCES [dbo].[COUNTRY] ([country_PK])
);

