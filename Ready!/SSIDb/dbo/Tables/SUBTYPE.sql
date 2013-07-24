CREATE TABLE [dbo].[SUBTYPE] (
    [subtype_PK]              INT           IDENTITY (1, 1) NOT NULL,
    [substance_class_subtype] VARCHAR (250) NULL,
    CONSTRAINT [PK_SSI_SUBTYPE] PRIMARY KEY CLUSTERED ([subtype_PK] ASC)
);

