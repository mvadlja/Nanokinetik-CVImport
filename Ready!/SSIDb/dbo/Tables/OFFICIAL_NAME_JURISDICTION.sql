﻿CREATE TABLE [dbo].[OFFICIAL_NAME_JURISDICTION] (
    [jurisdiction_PK] INT         IDENTITY (1, 1) NOT NULL,
    [on_jurisd]       VARCHAR (3) NOT NULL,
    CONSTRAINT [PK_SSI_OFFICIAL_NAME_JURISDICTION] PRIMARY KEY CLUSTERED ([jurisdiction_PK] ASC)
);

