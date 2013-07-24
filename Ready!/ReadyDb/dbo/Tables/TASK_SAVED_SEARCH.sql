CREATE TABLE [dbo].[TASK_SAVED_SEARCH] (
    [task_saved_search_PK]        INT            IDENTITY (1, 1) NOT NULL,
    [activity_FK]                 INT            NULL,
    [name]                        NVARCHAR (500) NULL,
    [user_FK]                     INT            NULL,
    [type_internal_status_FK]     INT            NULL,
    [country_FK]                  INT            NULL,
    [start_date_from]             DATE           NULL,
    [start_date_to]               DATE           NULL,
    [expected_finished_date_from] DATE           NULL,
    [expected_finished_date_to]   DATE           NULL,
    [actual_finished_date_from]   DATE           NULL,
    [actual_finished_date_to]     DATE           NULL,
    [displayName]                 NVARCHAR (100) NULL,
    [user_FK1]                    INT            NULL,
    [gridLayout]                  NVARCHAR (MAX) NULL,
    [isPublic]                    BIT            NULL,
    CONSTRAINT [PK_TASK_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([task_saved_search_PK] ASC)
);

