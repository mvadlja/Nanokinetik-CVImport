CREATE TABLE [dbo].[OFFICIAL_NAME] (
    [official_name_PK]     INT          IDENTITY (1, 1) NOT NULL,
    [on_type_FK]           INT          NOT NULL,
    [on_status_FK]         INT          NOT NULL,
    [on_status_changedate] VARCHAR (10) NULL,
    [on_jurisdiction_FK]   INT          NULL,
    [on_domain_FK]         INT          NULL,
    CONSTRAINT [PK_SSI_OFFICIAL_NAME] PRIMARY KEY CLUSTERED ([official_name_PK] ASC),
    CONSTRAINT [FK_OFFICIAL_NAME_SSI_CONTROLED_VOCABULARY] FOREIGN KEY ([on_status_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK]),
    CONSTRAINT [FK_OFFICIAL_NAME_SSI_CONTROLED_VOCABULARY1] FOREIGN KEY ([on_type_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

