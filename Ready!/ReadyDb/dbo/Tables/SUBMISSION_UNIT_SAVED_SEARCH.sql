CREATE TABLE [dbo].[SUBMISSION_UNIT_SAVED_SEARCH] (
    [submission_unit_saved_search_PK] INT            IDENTITY (1, 1) NOT NULL,
    [product_FK]                      INT            NULL,
    [activity_FK]                     INT            NULL,
    [task_FK]                         INT            NULL,
    [description_type_FK]             INT            NULL,
    [agency_FK]                       INT            NULL,
    [rms_FK]                          INT            NULL,
    [submission_ID]                   NVARCHAR (200) NULL,
    [s_format_FK]                     INT            NULL,
    [sequence]                        NVARCHAR (100) NULL,
    [dtd_schema_FK]                   INT            NULL,
    [dispatch_date_from]              DATETIME       NULL,
    [dispatch_date_to]                DATETIME       NULL,
    [receipt_date_from]               DATETIME       NULL,
    [receipt_to]                      DATETIME       NULL,
    [displayName]                     NVARCHAR (100) NULL,
    [user_FK]                         INT            NULL,
    [gridLayout]                      NVARCHAR (MAX) NULL,
    [isPublic]                        BIT            NULL,
    CONSTRAINT [PK_SUBMISSION_UNIT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([submission_unit_saved_search_PK] ASC)
);





