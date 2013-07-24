CREATE TABLE [dbo].[XEVPRM_ENTITY_TYPE] (
    [xevprm_entity_type_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]                  NVARCHAR (50) NULL,
    [table_name]            NVARCHAR (50) NULL,
    CONSTRAINT [PK_XEVPRM_ENTITY_TYPE] PRIMARY KEY CLUSTERED ([xevprm_entity_type_PK] ASC)
);

