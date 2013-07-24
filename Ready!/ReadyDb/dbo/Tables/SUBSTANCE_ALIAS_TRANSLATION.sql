CREATE TABLE [dbo].[SUBSTANCE_ALIAS_TRANSLATION] (
    [substance_alias_translation_PK] INT           IDENTITY (1, 1) NOT NULL,
    [languagecode]                   NVARCHAR (50) NULL,
    [term]                           NTEXT         NULL,
    CONSTRAINT [PK_SUBSTANCE_ALIAS_TRANSLATION] PRIMARY KEY CLUSTERED ([substance_alias_translation_PK] ASC)
);

