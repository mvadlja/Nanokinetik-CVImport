CREATE TABLE [dbo].[LANGUAGE_CODE] (
    [languagecode_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]            NVARCHAR (50) NULL,
    [code]            NVARCHAR (50) NULL,
    CONSTRAINT [PK_LANGUAGE_CODE] PRIMARY KEY CLUSTERED ([languagecode_PK] ASC)
);

