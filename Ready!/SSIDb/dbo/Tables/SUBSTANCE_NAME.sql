CREATE TABLE [dbo].[SUBSTANCE_NAME] (
    [substance_name_PK]  INT            IDENTITY (1, 1) NOT NULL,
    [subst_name]         VARCHAR (4000) NOT NULL,
    [subst_name_type_FK] INT            NOT NULL,
    [language_FK]        INT            NOT NULL,
    CONSTRAINT [PK_SSI_SUBSTANCE_NAME] PRIMARY KEY CLUSTERED ([substance_name_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_NAME_SSI_CONTROLED_VOCABULARY] FOREIGN KEY ([language_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_SUBSTANCE_NAME_SSI_CONTROLED_VOCABULARY1] FOREIGN KEY ([subst_name_type_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

