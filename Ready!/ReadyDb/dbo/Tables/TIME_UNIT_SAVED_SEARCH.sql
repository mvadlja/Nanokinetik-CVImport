CREATE TABLE [dbo].[TIME_UNIT_SAVED_SEARCH] (
    [time_unit_saved_search_PK] INT            IDENTITY (1, 1) NOT NULL,
    [activity_FK]               INT            NULL,
    [time_unit_FK]              INT            NULL,
    [user_FK]                   INT            NULL,
    [actual_date_from]          DATE           NULL,
    [actual_date_to]            DATE           NULL,
    [displayName]               NVARCHAR (100) NULL,
    [user_FK1]                  INT            NULL,
    [gridLayout]                NVARCHAR (MAX) NULL,
    [isPublic]                  BIT            NULL,
    CONSTRAINT [PK_TIME_UNIT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([time_unit_saved_search_PK] ASC)
);

