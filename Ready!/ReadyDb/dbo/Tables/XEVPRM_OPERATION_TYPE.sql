CREATE TABLE [dbo].[XEVPRM_OPERATION_TYPE] (
    [xevprm_operation_type_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]                     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_XEVPRM_OPERATION_TYPE] PRIMARY KEY CLUSTERED ([xevprm_operation_type_PK] ASC)
);

