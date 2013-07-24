CREATE TABLE [dbo].[ACTIVITY_SAVED_SEARCH] (
    [activity_saved_search_PK]    INT            IDENTITY (1, 1) NOT NULL,
    [project_FK]                  INT            NULL,
    [product_FK]                  INT            NULL,
    [name]                        NVARCHAR (350) NULL,
    [user_FK]                     INT            NULL,
    [procedure_number]            NVARCHAR (250) NULL,
    [procedure_type_FK]           INT            NULL,
    [type_FK]                     INT            NULL,
    [regulatory_status_FK]        INT            NULL,
    [internal_status_FK]          INT            NULL,
    [activity_mode_FK]            INT            NULL,
    [applicant_FK]                INT            NULL,
    [country_FK]                  INT            NULL,
    [legal]                       NVARCHAR (150) NULL,
    [start_date_from]             DATETIME       NULL,
    [start_date_to]               DATETIME       NULL,
    [expected_finished_date_from] DATETIME       NULL,
    [expected_finished_date_to]   DATETIME       NULL,
    [actual_finished_date_from]   DATETIME       NULL,
    [actual_finished_date_to]     DATETIME       NULL,
    [approval_date_from]          DATETIME       NULL,
    [approval_date_to]            DATETIME       NULL,
    [submission_date_from]        DATETIME       NULL,
    [submission_date_to]          DATETIME       NULL,
    [displayName]                 NVARCHAR (100) NULL,
    [user_FK1]                    INT            NULL,
    [gridLayout]                  NVARCHAR (MAX) NULL,
    [isPublic]                    BIT            NULL,
    [billable]                    BIT            NULL,
    [activity_ID]                 NVARCHAR (100) NULL,
    CONSTRAINT [PK_ACTIVITY_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([activity_saved_search_PK] ASC)
);







