CREATE TABLE [dbo].[PRODUCT_ATC_MN] (
    [product_atc_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]        INT NULL,
    [atc_FK]            INT NULL,
    CONSTRAINT [PK_PRODUCT_ATC_MN] PRIMARY KEY CLUSTERED ([product_atc_mn_PK] ASC),
    CONSTRAINT [FK_PRODUCT_ATC_MN_ATC] FOREIGN KEY ([atc_FK]) REFERENCES [dbo].[ATC] ([atc_PK]),
    CONSTRAINT [FK_PRODUCT_ATC_MN_PRODUCT] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]) ON DELETE CASCADE
);

