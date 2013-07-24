CREATE TABLE [dbo].[SERVICE_LOG] (
    [service_log_PK] INT            IDENTITY (1, 1) NOT NULL,
    [log_time]       DATETIME       NULL,
    [description]    NVARCHAR (MAX) NULL,
    [user_FK]        INT            NULL,
    CONSTRAINT [PK_SERVICE_LOG] PRIMARY KEY CLUSTERED ([service_log_PK] ASC)
);

