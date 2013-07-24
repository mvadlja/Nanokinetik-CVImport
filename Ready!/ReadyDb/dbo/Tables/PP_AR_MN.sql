CREATE TABLE [dbo].[PP_AR_MN] (
    [pp_ar_mn_PK]               INT IDENTITY (1, 1) NOT NULL,
    [admin_route_FK]            INT NULL,
    [pharmaceutical_product_FK] INT NULL,
    CONSTRAINT [PK_PP_AR_MN] PRIMARY KEY CLUSTERED ([pp_ar_mn_PK] ASC),
    CONSTRAINT [FK_PP_AR_MN_PHARMACEUTICAL_PRODUCT] FOREIGN KEY ([pharmaceutical_product_FK]) REFERENCES [dbo].[PHARMACEUTICAL_PRODUCT] ([pharmaceutical_product_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_PP_AR_MN_PP_ADMINISTRATION_ROUTE] FOREIGN KEY ([admin_route_FK]) REFERENCES [dbo].[PP_ADMINISTRATION_ROUTE] ([adminroute_PK])
);

