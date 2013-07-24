CREATE TABLE [dbo].[REMINDER_DATES] (
    [reminder_date_PK]           INT      IDENTITY (1, 1) NOT NULL,
    [reminder_date]              DATETIME NULL,
    [reminder_repeating_mode_FK] INT      NULL,
    [reminder_status_FK]         INT      NULL,
    [reminder_FK]                INT      NULL,
    CONSTRAINT [PK_REMINDER_DATE_MN] PRIMARY KEY CLUSTERED ([reminder_date_PK] ASC),
    CONSTRAINT [FK_REMINDER_DATE_MN_REMINDER_REPEATING_MODES] FOREIGN KEY ([reminder_repeating_mode_FK]) REFERENCES [dbo].[REMINDER_REPEATING_MODES] ([reminder_repeating_mode_PK]),
    CONSTRAINT [FK_REMINDER_DATES_REMINDER] FOREIGN KEY ([reminder_FK]) REFERENCES [dbo].[REMINDER] ([reminder_PK]) ON DELETE CASCADE
);



