CREATE TABLE [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN] (
    [approved_subst_sub_alias_trans_PK] INT IDENTITY (1, 1) NOT NULL,
    [approved_substance_FK]             INT NULL,
    [substance_alias_translation_FK]    INT NULL,
    CONSTRAINT [PK_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN] PRIMARY KEY CLUSTERED ([approved_subst_sub_alias_trans_PK] ASC),
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_APPROVED_SUBST_1] FOREIGN KEY ([approved_substance_FK]) REFERENCES [dbo].[APPROVED_SUBSTANCE] ([approved_substance_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_SUBST_ALIAS_TRANS_2] FOREIGN KEY ([substance_alias_translation_FK]) REFERENCES [dbo].[SUBSTANCE_ALIAS_TRANSLATION] ([substance_alias_translation_PK]) ON DELETE CASCADE
);

