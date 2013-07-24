﻿CREATE TABLE [dbo].[TASK_COUNTRY_MN] (
    [task_country_PK] INT IDENTITY (1, 1) NOT NULL,
    [task_FK]         INT NULL,
    [country_FK]      INT NULL,
    CONSTRAINT [PK_TASK_COUNTRY_MN] PRIMARY KEY CLUSTERED ([task_country_PK] ASC),
    CONSTRAINT [FK_TASK_COUNTRY_MN_COUNTRY_2] FOREIGN KEY ([country_FK]) REFERENCES [dbo].[COUNTRY] ([country_PK]) ON UPDATE CASCADE,
    CONSTRAINT [FK_TASK_COUNTRY_MN_TASK_1] FOREIGN KEY ([task_FK]) REFERENCES [dbo].[TASK] ([task_PK]) ON DELETE CASCADE
);
