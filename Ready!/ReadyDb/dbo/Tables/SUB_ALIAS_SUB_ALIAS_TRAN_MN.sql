﻿CREATE TABLE [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN] (
    [sub_alias_sub_alias_tran_PK] INT IDENTITY (1, 1) NOT NULL,
    [sub_alias_FK]                INT NULL,
    [sub_alias_tran_FK]           INT NULL,
    CONSTRAINT [PK_SUB_ALIAS_SUB_ALIAS_TRAN_MN] PRIMARY KEY CLUSTERED ([sub_alias_sub_alias_tran_PK] ASC),
    CONSTRAINT [FK_SUB_ALIAS_SUB_ALIAS_TRAN_MN_SUBSTANCE_ALIAS] FOREIGN KEY ([sub_alias_FK]) REFERENCES [dbo].[SUBSTANCE_ALIAS] ([substance_alias_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_SUB_ALIAS_SUB_ALIAS_TRAN_MN_SUBSTANCE_ALIAS_TRANSLATION] FOREIGN KEY ([sub_alias_tran_FK]) REFERENCES [dbo].[SUBSTANCE_ALIAS_TRANSLATION] ([substance_alias_translation_PK]) ON DELETE CASCADE
);

