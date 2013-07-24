CREATE TABLE [dbo].[PRODUCT_DOCUMENT_MN] (
    [product_document_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [product_FK]             INT NULL,
    [document_FK]            INT NULL,
    CONSTRAINT [PK_PRODUCT_DOCUMENT_MN] PRIMARY KEY CLUSTERED ([product_document_mn_PK] ASC),
    CONSTRAINT [FK_PRODUCT_DOCUMENT_MN_DOCUMENT] FOREIGN KEY ([document_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_PRODUCT_DOCUMENT_MN_PRODUCT] FOREIGN KEY ([product_FK]) REFERENCES [dbo].[PRODUCT] ([product_PK])
);

