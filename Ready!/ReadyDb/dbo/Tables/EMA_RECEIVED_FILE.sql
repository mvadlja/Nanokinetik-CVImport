CREATE TABLE [dbo].[EMA_RECEIVED_FILE] (
    [ema_received_file_PK] INT             IDENTITY (1, 1) NOT NULL,
    [file_type]            NVARCHAR (50)   NULL,
    [file_data]            VARBINARY (MAX) NULL,
    [xevprm_path]          NVARCHAR (1000) NULL,
    [data_path]            NVARCHAR (1000) NULL,
    [status]               INT             NULL,
    [received_time]        DATETIME        NULL,
    [processed_time]       DATETIME        NULL,
    [as2_from]             NVARCHAR (100)  NULL,
    [as2_to]               NVARCHAR (100)  NULL,
    [as2_header]           NVARCHAR (MAX)  NULL,
    [mdn_orig_msg_number]  NVARCHAR (100)  NULL,
    CONSTRAINT [PK_EMA_RECEIVED_FILE] PRIMARY KEY CLUSTERED ([ema_received_file_PK] ASC)
);

