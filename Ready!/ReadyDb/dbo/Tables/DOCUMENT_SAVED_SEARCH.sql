﻿CREATE TABLE [dbo].[DOCUMENT_SAVED_SEARCH] (
    [document_saved_search_PK]  INT            IDENTITY (1, 1) NOT NULL,
    [product_FK]                INT            NULL,
    [ap_FK]                     INT            NULL,
    [project_FK]                INT            NULL,
    [activity_FK]               INT            NULL,
    [task_FK]                   INT            NULL,
    [name]                      NVARCHAR (250) NULL,
    [type_FK]                   INT            NULL,
    [version_number]            INT            NULL,
    [version_label]             INT            NULL,
    [document_number]           NVARCHAR (500) NULL,
    [person_FK]                 INT            NULL,
    [regulatory_status]         INT            NULL,
    [change_date_from]          DATETIME       NULL,
    [change_date_to]            DATETIME       NULL,
    [effective_start_date_from] DATETIME       NULL,
    [effective_start_date_to]   DATETIME       NULL,
    [effective_end_date_from]   DATETIME       NULL,
    [effective_end_date_to]     DATETIME       NULL,
    [displayName]               NVARCHAR (100) NULL,
    [user_FK1]                  INT            NULL,
    [gridLayout]                NVARCHAR (MAX) NULL,
    [isPublic]                  BIT            NULL,
    [pp_FK]                     INT            NULL,
    [version_date_from]         DATETIME       NULL,
    [version_date_to]           DATETIME       NULL,
    [ev_code]                   NVARCHAR (250) NULL,
    [content]                   NVARCHAR (250) NULL,
    [language_code]             NVARCHAR (250) NULL,
    [comments]                  NVARCHAR (250) NULL,
    CONSTRAINT [PK_DOCUMENT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([document_saved_search_PK] ASC)
);



