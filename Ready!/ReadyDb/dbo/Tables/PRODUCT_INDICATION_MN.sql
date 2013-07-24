CREATE TABLE [dbo].[PRODUCT_INDICATION_MN] (
    [product_indication_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]            INT NULL,
    [prod_indication_FK]    INT NULL,
    CONSTRAINT [PK_PRODUCT_INDICATION_MN] PRIMARY KEY CLUSTERED ([product_indication_PK] ASC)
);

