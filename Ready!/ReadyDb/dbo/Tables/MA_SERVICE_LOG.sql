CREATE TABLE [dbo].[MA_SERVICE_LOG] (
    [ma_service_log_PK] INT            IDENTITY (1, 1) NOT NULL,
    [log_time]          DATETIME       NULL,
    [description]       NVARCHAR (MAX) NULL,
    [ready_id_FK]       VARCHAR (32)   NULL,
    [event_type_FK]     INT            NULL,
    CONSTRAINT [PK_MA_SERVICE_LOG] PRIMARY KEY CLUSTERED ([ma_service_log_PK] ASC)
);

