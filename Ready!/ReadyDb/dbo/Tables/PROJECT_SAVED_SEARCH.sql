CREATE TABLE [dbo].[PROJECT_SAVED_SEARCH] (
    [project_saved_search_PK]     INT            IDENTITY (1, 1) NOT NULL,
    [name]                        NVARCHAR (150) NULL,
    [user_FK]                     INT            NULL,
    [internal_status_type_FK]     INT            NULL,
    [country_FK]                  INT            NULL,
    [start_date_from]             DATE           NULL,
    [start_date_to]               DATE           NULL,
    [expected_finished_date_from] DATE           NULL,
    [expected_finished_dat_to]    DATE           NULL,
    [actual_finished_date_from]   DATE           NULL,
    [actual_finished_date_to]     DATE           NULL,
    [displayName]                 NVARCHAR (100) NULL,
    [user_FK1]                    INT            NULL,
    [gridLayout]                  NVARCHAR (MAX) NULL,
    [isPublic]                    BIT            NULL,
    CONSTRAINT [PK_PROJECT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([project_saved_search_PK] ASC)
);

