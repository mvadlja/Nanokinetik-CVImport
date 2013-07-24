CREATE TABLE [dbo].[XEVPRM_MESSAGE_STATUS] (
    [xevprm_message_status_PK]        INT           IDENTITY (1, 1) NOT NULL,
    [enum_name]                       NVARCHAR (50) NULL,
    [name]                            NVARCHAR (50) NOT NULL,
    [xevprm_grid_status_name]         NVARCHAR (50) NULL,
    [xevprm_grid_gateway_status_name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_XEVPRM_MESSAGE_STATUS] PRIMARY KEY CLUSTERED ([xevprm_message_status_PK] ASC)
);

