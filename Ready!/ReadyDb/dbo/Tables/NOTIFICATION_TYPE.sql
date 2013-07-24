CREATE TABLE [dbo].[NOTIFICATION_TYPE] (
    [notification_type_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]                 NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NOTIFICATION_TYPE] PRIMARY KEY CLUSTERED ([notification_type_PK] ASC)
);

