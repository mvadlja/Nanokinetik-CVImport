CREATE TABLE [dbo].[EMAIL_NOTIFICATION] (
    [email_notification_PK] INT            IDENTITY (1, 1) NOT NULL,
    [notification_type_FK]  INT            NOT NULL,
    [email]                 NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EMAIL_NOTIFICATION] PRIMARY KEY CLUSTERED ([email_notification_PK] ASC),
    CONSTRAINT [FK_EMAIL_NOTIFICATION_NOTIFICATION_TYPE] FOREIGN KEY ([notification_type_FK]) REFERENCES [dbo].[NOTIFICATION_TYPE] ([notification_type_PK])
);

