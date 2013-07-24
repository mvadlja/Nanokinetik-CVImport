CREATE TABLE [dbo].[REMINDER] (
    [reminder_PK]             INT            IDENTITY (1, 1) NOT NULL,
    [user_FK]                 INT            NULL,
    [responsible_user_FK]     INT            NULL,
    [table_name]              NVARCHAR (100) NULL,
    [entity_FK]               INT            NULL,
    [related_attribute_name]  NVARCHAR (50)  NULL,
    [related_attribute_value] NVARCHAR (MAX) NULL,
    [reminder_name]           NVARCHAR (500) NULL,
    [reminder_type]           NVARCHAR (500) NULL,
    [navigate_url]            NVARCHAR (100) NULL,
    [time_before_activation]  BIGINT         NULL,
    [remind_me_on_email]      BIT            NOT NULL,
    [additional_emails]       NVARCHAR (MAX) NULL,
    [description]             NVARCHAR (MAX) NULL,
    [is_automatic]            BIT            NULL,
    [related_entity_FK]       INT            NULL,
    [reminder_user_status_FK] INT            NULL,
    [comment]                 NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Reminders] PRIMARY KEY CLUSTERED ([reminder_PK] ASC),
    CONSTRAINT [FK_REMINDER_REMINDER_USER_STATUS] FOREIGN KEY ([reminder_user_status_FK]) REFERENCES [dbo].[REMINDER_USER_STATUS] ([reminder_user_status_PK])
);







