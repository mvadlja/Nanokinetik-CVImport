CREATE TABLE [dbo].[REMINDER_EMAIL_RECIPIENT] (
    [reminder_email_recipient_PK] INT IDENTITY (1, 1) NOT NULL,
    [reminder_FK]                 INT NOT NULL,
    [person_FK]                   INT NOT NULL,
    CONSTRAINT [PK_REMINDER_EMAIL_RECIPIENT] PRIMARY KEY CLUSTERED ([reminder_email_recipient_PK] ASC)
);

