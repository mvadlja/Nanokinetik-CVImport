CREATE TABLE [dbo].[REMINDER_REPEATING_MODES] (
    [reminder_repeating_mode_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                       NVARCHAR (100) NULL,
    [enum_name]                  NVARCHAR (100) NULL,
    CONSTRAINT [PK_REMINDER_REPEATING_MODES] PRIMARY KEY CLUSTERED ([reminder_repeating_mode_PK] ASC)
);

