CREATE TABLE [dbo].[SUBSTANCE_TRANSLATION] (
    [substance_translations_PK] INT           IDENTITY (1, 1) NOT NULL,
    [languagecode]              NVARCHAR (50) NULL,
    [term]                      NTEXT         NULL,
    CONSTRAINT [PK_SUBSTANCE_TRANSLATION] PRIMARY KEY CLUSTERED ([substance_translations_PK] ASC)
);

