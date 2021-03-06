﻿CREATE TABLE [dbo].[AP_DOCUMENT_MN] (
    [ap_document_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [document_FK]       INT NULL,
    [ap_FK]             INT NULL,
    CONSTRAINT [PK_AP_DOCUMENT_MN] PRIMARY KEY CLUSTERED ([ap_document_mn_PK] ASC),
    CONSTRAINT [FK_AP_DOCUMENT_MN_AUTHORISED_PRODUCT] FOREIGN KEY ([ap_FK]) REFERENCES [dbo].[AUTHORISED_PRODUCT] ([ap_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_AP_DOCUMENT_MN_DOCUMENT] FOREIGN KEY ([document_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]) ON DELETE CASCADE
);

