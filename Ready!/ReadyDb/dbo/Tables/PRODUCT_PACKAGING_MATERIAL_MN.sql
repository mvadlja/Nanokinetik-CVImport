CREATE TABLE [dbo].[PRODUCT_PACKAGING_MATERIAL_MN] (
    [product_packaging_material_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]                       INT NOT NULL,
    [type_FK]                          INT NOT NULL,
    CONSTRAINT [PK_PRODUCT_PACKAGING_MATERIAL_MN] PRIMARY KEY CLUSTERED ([product_packaging_material_mn_PK] ASC),
    CONSTRAINT [FK_PRODUCT_PACKAGING_MATERIAL_MN_PRODUCT] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK]),
    CONSTRAINT [FK_PRODUCT_PACKAGING_MATERIAL_MN_TYPE] FOREIGN KEY ([type_FK]) REFERENCES [dbo].[TYPE] ([type_PK])
);

