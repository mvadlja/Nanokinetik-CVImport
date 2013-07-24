CREATE TABLE [dbo].[XEVPRM_LOG] (
    [xevprm_log_PK]     INT            IDENTITY (1, 1) NOT NULL,
    [xevprm_message_FK] INT            NULL,
    [log_time]          DATETIME       NULL,
    [description]       NVARCHAR (MAX) NULL,
    [username]          NVARCHAR (100) NULL,
    [xevprm_status_FK]  INT            NULL,
    CONSTRAINT [PK_EVMPD_LOG] PRIMARY KEY CLUSTERED ([xevprm_log_PK] ASC)
);





