CREATE TABLE [dbo].[STRUCTURE] (
    [structure_PK]            INT            IDENTITY (1, 1) NOT NULL,
    [struct_repres_type_FK]   INT            NOT NULL,
    [struct_representation]   VARCHAR (4000) NOT NULL,
    [struct_repres_attach_FK] INT            NULL,
    [stereochemistry_FK]      INT            NOT NULL,
    [optical_activity]        VARCHAR (250)  NOT NULL,
    [molecular_formula]       VARCHAR (2000) NULL,
    CONSTRAINT [PK_SSI_STRUCTURE] PRIMARY KEY CLUSTERED ([structure_PK] ASC),
    CONSTRAINT [FK_STRUCTURE_STEREOCHEMISTRY] FOREIGN KEY ([stereochemistry_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_STRUCTURE_STRUCT_REPRES_ATTACHMENT] FOREIGN KEY ([struct_repres_attach_FK]) REFERENCES [dbo].[STRUCT_REPRES_ATTACHMENT] ([struct_repres_attach_PK]),
    CONSTRAINT [FK_STRUCTURE_STRUCTURE] FOREIGN KEY ([struct_repres_type_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

