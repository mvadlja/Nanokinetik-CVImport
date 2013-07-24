CREATE TABLE [dbo].[ALERT_SAVED_SEARCH] (
    [alert_saved_search_PK]      INT            IDENTITY (1, 1) NOT NULL,
    [product_FK]                 INT            NULL,
    [ap_FK]                      INT            NULL,
    [project_FK]                 INT            NULL,
    [activity_FK]                INT            NULL,
    [task_FK]                    INT            NULL,
    [document_FK]                INT            NULL,
    [gridLayout]                 NVARCHAR (MAX) NULL,
    [isPublic]                   BIT            NULL,
    [name]                       NVARCHAR (250) NULL,
    [reminder_repeating_mode_FK] INT            NULL,
    [send_mail]                  BIT            NULL,
    [displayName]                NVARCHAR (100) NULL,
    [user_FK]                    INT            NULL,
    CONSTRAINT [PK_ALERT_SAVED_SEARCH] PRIMARY KEY CLUSTERED ([alert_saved_search_PK] ASC)
);

