﻿CREATE TABLE [dbo].[ON_ONJ_MN] (
    [on_onj_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [onj_FK]       INT NULL,
    [on_FK]        INT NULL,
    CONSTRAINT [PK_ON_ONJ_MN] PRIMARY KEY CLUSTERED ([on_onj_mn_PK] ASC),
    CONSTRAINT [FK_ON_ONJ_MN_OFFICIAL_NAME] FOREIGN KEY ([on_FK]) REFERENCES [dbo].[OFFICIAL_NAME] ([official_name_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_ON_ONJ_MN_SSI_CONTROLED_VOCABULARY] FOREIGN KEY ([onj_FK]) REFERENCES [dbo].[SSI_CONTROLED_VOCABULARY] ([ssi__cont_voc_PK])
);

