CREATE TABLE [dbo].[PRODUCT_INDICATION] (
    [product_indications_PK] INT            IDENTITY (1, 1) NOT NULL,
    [meddraversion]          DECIMAL (3, 1) NULL,
    [meddralevel]            NVARCHAR (50)  NULL,
    [meddracode]             INT            NULL,
    [name]                   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_PRODUCT_INDICATION] PRIMARY KEY CLUSTERED ([product_indications_PK] ASC)
);

