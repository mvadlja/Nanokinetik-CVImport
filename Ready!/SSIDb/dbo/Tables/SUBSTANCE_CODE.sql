CREATE TABLE [dbo].[SUBSTANCE_CODE] (
    [substance_code_PK]      INT            IDENTITY (1, 1) NOT NULL,
    [code]                   VARCHAR (500)  NOT NULL,
    [code_system_FK]         INT            NOT NULL,
    [code_system_id_FK]      INT            NULL,
    [code_system_status_FK]  INT            NOT NULL,
    [code_system_changedate] VARCHAR (10)   NULL,
    [comment]                VARCHAR (4000) NULL,
    CONSTRAINT [PK_SSI_SUBSTANCE_CODE] PRIMARY KEY CLUSTERED ([substance_code_PK] ASC),
    CONSTRAINT [FK_SUBSTANCE_CODE_SSI_CONTROLED_VOCABULARY] FOREIGN KEY ([code_system_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_SUBSTANCE_CODE_SSI_CONTROLED_VOCABULARY1] FOREIGN KEY ([code_system_id_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_SUBSTANCE_CODE_SSI_CONTROLED_VOCABULARY2] FOREIGN KEY ([code_system_status_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

