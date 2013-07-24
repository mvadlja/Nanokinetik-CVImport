CREATE TABLE [dbo].[SUBSTANCE_ALIAS] (
    [substance_alias_PK]             INT           IDENTITY (1, 1) NOT NULL,
    [sourcecode]                     NVARCHAR (60) NULL,
    [resolutionmode]                 INT           NULL,
    [aliasname]                      NTEXT         NULL,
    [substance_aliastranslations_FK] INT           NULL,
    CONSTRAINT [PK_SUBSTANCE_ALIAS] PRIMARY KEY CLUSTERED ([substance_alias_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_ALIAS_SUBSTANCE_ALIAS_TRANSLATION] FOREIGN KEY ([substance_aliastranslations_FK]) REFERENCES [dbo].[SUBSTANCE_ALIAS_TRANSLATION] ([substance_alias_translation_PK])
);

