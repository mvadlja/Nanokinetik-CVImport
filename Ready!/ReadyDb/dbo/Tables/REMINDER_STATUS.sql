CREATE TABLE [dbo].[REMINDER_STATUS] (
    [reminder_status_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]               NVARCHAR (100) NULL,
    [enum_name]          NVARCHAR (100) NULL,
    CONSTRAINT [PK_REMINDER_STATUS] PRIMARY KEY CLUSTERED ([reminder_status_PK] ASC)
);

