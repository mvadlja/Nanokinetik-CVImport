CREATE TABLE [dbo].[REMINDER_USER_STATUS] (
    [reminder_user_status_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                    NVARCHAR (100) NULL,
    [enum_name]               NVARCHAR (50)  NULL,
    CONSTRAINT [PK_REMINDER_USER_STATUS] PRIMARY KEY CLUSTERED ([reminder_user_status_PK] ASC)
);

